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
// SimCivil - SimCivil.Rpc - UtilHelper.cs
// Create Date: 2018/01/02
// Update Date: 2018/01/05

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Autofac;
using Autofac.Builder;

using Newtonsoft.Json;

using SimCivil.Rpc.Serialize;
using SimCivil.Rpc.Session;

namespace SimCivil.Rpc
{
    public static class UtilHelper
    {
        internal const string RpcServiceMarker = "RPC";

        public static JsonSerializerSettings RpcJsonSerializerSettings { get; } = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            NullValueHandling = NullValueHandling.Ignore
        };

        static UtilHelper()
        {
            RpcJsonSerializerSettings.Converters.Add(new IPAddressConverter());
            RpcJsonSerializerSettings.Converters.Add(new IPEndPointConverter());
        }

        public static IRegistrationBuilder<TProvider, ConcreteReflectionActivatorData, SingleRegistrationStyle>
            RegisterRpcProvider<TProvider, TService>(this ContainerBuilder builder) where TProvider : TService
        {
            return builder.RegisterType<TProvider>()
                .As<TService>()
                .WithMetadata(RpcServiceMarker, typeof(TService))
                .InstancePerRequest();
        }

        public static IEnumerable<Type> GetRpcServiceTypes(this IContainer container)
        {
            return container.ComponentRegistry.Registrations
                .Where(r => r.Metadata.ContainsKey(RpcServiceMarker))
                .Select(r => r.Metadata[RpcServiceMarker] as Type);
        }

        public static ContainerBuilder UseRpcSession(this ContainerBuilder builder)
        {
            return builder.UseRpcSession<LocalRpcSession>();
        }

        public static ContainerBuilder UseRpcSession<T>(this ContainerBuilder builder) where T : IRpcSession
        {
            builder.RegisterType<T>().As<IRpcSession>().InstancePerChannel();

            return builder;
        }

        public static IRegistrationBuilder<TLimit, TActivatorData, TStyle>
            InstancePerChannel<TLimit, TActivatorData, TStyle>(
                this IRegistrationBuilder<TLimit, TActivatorData, TStyle> registration,
                params object[] lifetimeScopeTags)
        {
            if (registration == null)
                throw new ArgumentNullException(nameof(registration));

            var array = new object[]
                {
                    RpcServiceMarker
                }.Concat(lifetimeScopeTags)
                .ToArray();

            return registration.InstancePerMatchingLifetimeScope(array);
        }

        public static T Get<T>(this IRpcSession session, string key)
        {
            return (T) session[key];
        }

        public static T Get<T>(this IRpcSession session)
        {
            return (T) session[typeof(T).FullName];
        }

        public static IRpcSession Set<T>(this IRpcSession session, T value)
        {
            session[typeof(T).FullName] = value;

            return session;
        }

        public static bool IsSet<T>(this IRpcSession session)
        {
            return session.ContainsKey(typeof(T).FullName);
        }

        internal static RpcSessionAssigner<T> AssignTo<T>(this IRpcSession session, T service) where T : class
        {
            return new RpcSessionAssigner<T>(session, service);
        }

        public static MethodType GetDelegateType(this Type type)
        {
            if (type == typeof(Task))
                return MethodType.AsyncAction;
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Task<>))
                return MethodType.AsyncFunction;

            return MethodType.Synchronous;
        }

        public static async Task<object> InvokeAsync(this MethodInfo @this, object obj, params object[] parameters)
        {
            dynamic awaitable = @this.Invoke(obj, parameters);
            await awaitable;

            return awaitable.GetAwaiter().GetResult();
        }
    }
}