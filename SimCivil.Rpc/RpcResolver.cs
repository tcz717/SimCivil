// Copyright (c) 2017 TPDT
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// 
// SimCivil - SimCivil.Rpc - RpcResolver.cs
// Create Date: 2018/01/02
// Update Date: 2018/01/07

using System;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Autofac;

using DotNetty.Transport.Channels;

using SimCivil.Rpc.Callback;
using SimCivil.Rpc.Filter;
using SimCivil.Rpc.Session;

namespace SimCivil.Rpc
{
    public class RpcResolver : SimpleChannelInboundHandler<RpcRequest>
    {
        private ILifetimeScope _scope;
        public RpcServer Server { get; }
        private CallbackProxyBuilder _proxyBuilder = new CallbackProxyBuilder();

        public RpcResolver(RpcServer rpcServer)
        {
            Server = rpcServer;
        }

        public override void ChannelActive(IChannelHandlerContext context)
        {
            _scope = Server.Container.BeginLifetimeScope(UtilHelper.RpcServiceMarker);
            if (Server.SupportSession)
            {
                var session = _scope.Resolve<IRpcSession>();
                session.RemoteEndPoint = context.Channel.RemoteAddress as IPEndPoint;
                Server.Sessions.OnEntering(session);
                session.OnEntering(context.Channel.RemoteAddress);
            }

            base.ChannelActive(context);
        }

        public override void ChannelInactive(IChannelHandlerContext context)
        {
            if (Server.SupportSession)
            {
                var session = _scope.Resolve<IRpcSession>();
                Server.Sessions.OnExiting(session);
                session.OnExiting();
            }

            _scope.Dispose();
            base.ChannelInactive(context);
        }

        protected override void ChannelRead0(IChannelHandlerContext ctx, RpcRequest msg)
        {
            if (!Server.Services.ContainsKey(msg.ServiceName))
            {
                throw new ArgumentException(nameof(msg.ServiceName));
            }

            Server.OnRemoteCalling(msg);

            try
            {
                var session = _scope.Resolve<IRpcSession>();
                var type = Server.Services[msg.ServiceName];
                var service = _scope.Resolve(type);
                var method = service.GetType().GetMethod(msg.MethodName);

                var result = CheckFilter(session, method);
                if (!result.Allowed)
                {
                    ctx.Channel.WriteAndFlushAsync(new RpcResponse(msg, null, result.ErrorInfo));

                    return;
                }

                var args = msg.Arguments;
                var parameters = method.GetParameters();
                if (parameters.Length != args.Length)
                {
                    ctx.Channel.WriteAndFlushAsync(new RpcResponse(msg, null, "Wrong arguments count"));

                    return;
                }

                for (var i = 0; i < args.Length; i++)
                {
                    Type parameterType = parameters[i].ParameterType;
                    if (typeof(Delegate).IsAssignableFrom(parameterType))
                    {
                        args[i] = _proxyBuilder.Build(parameterType, Convert.ToInt32(args[i]), ctx.Channel);
                    }
                    if (!parameterType.IsInstanceOfType(args[i]))
                    {
                        args[i] = Convert.ChangeType(args[i], parameterType);
                    }
                }

                ICallWarper warper = service as ICallWarper;
                warper?.BeforeCall(session);
                using (session.AssignTo(service))
                {
                    switch (method.ReturnType.GetDelegateType())
                    {
                        case MethodType.Synchronous:
                            object returnValue = method.Invoke(service, args);

                            ctx.Channel.WriteAndFlushAsync(new RpcResponse(msg, returnValue));

                            break;
                        case MethodType.AsyncAction:
                            ((Task) method.Invoke(service, args))
                                .ContinueWith(
                                    t =>
                                    {
                                        warper?.AfterCall(session);
                                        ctx.Channel.WriteAndFlushAsync(
                                            t.Exception != null
                                                ? new RpcResponse(msg, null, t.Exception)
                                                : new RpcResponse(msg, null));
                                    });

                            break;
                        case MethodType.AsyncFunction:
                            method.InvokeAsync(service, args)
                                .ContinueWith(
                                    t =>
                                    {
                                        warper?.AfterCall(session);
                                        ctx.Channel.WriteAndFlushAsync(
                                            t.Exception != null
                                                ? new RpcResponse(msg, null, t.Exception)
                                                : new RpcResponse(msg, t.Result));
                                    });

                            break;
                        default:

                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            catch (TargetInvocationException e)
            {
                ctx.Channel.WriteAndFlushAsync(new RpcResponse(msg, null, e.InnerException));

                throw;
            }
            catch (Exception e)
            {
                ctx.Channel.WriteAndFlushAsync(
                    Server.Debug ? new RpcResponse(msg, null, e) : new RpcResponse(msg, null, "Internal error"));

                throw;
            }
        }


        private static CheckResult CheckFilter(IRpcSession session, MethodInfo method)
        {
            var filterAttributes = method.DeclaringType.GetCustomAttributes<SessionFilterAttribute>()
                .Concat(
                    method.GetCustomAttributes<SessionFilterAttribute>());

            return filterAttributes
                       .Select(f => f.CheckPermission(session))
                       .FirstOrDefault(r => !r.Allowed) ?? CheckResult.Allow;
        }
    }

    public class RemoteDeniedException : Exception
    {
        public RemoteDeniedException(string errorInfo)
            : base(errorInfo) { }
    }
}