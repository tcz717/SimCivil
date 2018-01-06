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
// SimCivil - SimCivil.Rpc - RpcSessionManager.cs
// Create Date: 2018/01/02
// Update Date: 2018/01/02

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace SimCivil.Rpc.Session
{
    public class RpcSessionManager : IReadOnlyCollection<IRpcSession>
    {
        protected Collection<IRpcSession> Collection = new Collection<IRpcSession>();

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<IRpcSession> GetEnumerator()
        {
            return Collection.GetEnumerator();
        }

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>Gets the number of elements in the collection.</summary>
        /// <returns>The number of elements in the collection.</returns>
        public int Count => Collection.Count;
        public event EventHandler<EventArgs<IRpcSession>> Entering;
        public event EventHandler<EventArgs<IRpcSession>> Exiting;

        [MethodImpl(MethodImplOptions.Synchronized)]
        internal virtual void OnEntering(IRpcSession session)
        {
            if (session == null) throw new ArgumentNullException(nameof(session));

            if (Collection.Contains(session))
            {
                return;
            }

            Entering?.Invoke(this, new EventArgs<IRpcSession>(session));
            Collection.Add(session);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        internal virtual void OnExiting(IRpcSession session)
        {
            if (session == null) throw new ArgumentNullException(nameof(session));

            if (!Collection.Contains(session))
            {
                return;
            }

            Exiting?.Invoke(this, new EventArgs<IRpcSession>(session));

            Collection.Remove(session);
        }
    }
}