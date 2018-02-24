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
// SimCivil - SimCivil.Rpc - CallbackProxyBuilder.cs
// Create Date: 2018/01/29
// Update Date: 2018/01/30

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

using DotNetty.Transport.Channels;

namespace SimCivil.Rpc.Callback
{
    public class CallbackProxyBuilder
    {
        private readonly Dictionary<string, DynamicMethod> _cacheMethods = new Dictionary<string, DynamicMethod>();

        public Delegate Build(Type delegateType, int callbackId, IChannel channel)
        {
            var context = new CallbackContext(callbackId, channel);
            if (_cacheMethods.ContainsKey(delegateType.FullName))
            {
                return _cacheMethods[delegateType.FullName].CreateDelegate(delegateType, context);
            }

            DynamicMethod method = DoBuild(delegateType);
            _cacheMethods[delegateType.FullName] = method;

            return method.CreateDelegate(delegateType, context);
        }

        private DynamicMethod DoBuild(Type delegateType)
        {
            MethodInfo invoke = delegateType.GetMethod("Invoke");
            Type[] parameters = new []{typeof(CallbackContext)}.Concat(
                invoke.GetParameters().Select(p => p.ParameterType))
                .ToArray();
            DynamicMethod method = new DynamicMethod(
                delegateType.Name,
                invoke.ReturnType,
                parameters);
            ILGenerator generator = method.GetILGenerator();

            var parametersLocal = generator.DeclareLocal(typeof(object[]));
            generator.Emit(OpCodes.Ldc_I4, parameters.Length - 1);
            generator.Emit(OpCodes.Newarr, typeof(object));
            generator.Emit(OpCodes.Stloc, parametersLocal);
            for (int i = 1; i < parameters.Length; i++)
            {
                generator.Emit(OpCodes.Ldloc, parametersLocal);
                generator.Emit(OpCodes.Ldc_I4, i - 1);
                generator.Emit(OpCodes.Ldarg, i);
                generator.Emit(OpCodes.Stelem_Ref);
            }

            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldloc, parametersLocal);
            generator.Emit(OpCodes.Call, typeof(CallbackContext).GetMethod("Call"));

            generator.Emit(OpCodes.Ret);

            return method;
        }
    }
}