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
// SimCivil - SimCivil - IPrefabRepository.cs
// Create Date: 2018/01/07
// Update Date: 2018/01/07

using SimCivil.Model;

namespace SimCivil.Store
{
    /// <summary>
    /// Prefab Repository
    /// </summary>
    public interface IPrefabRepository
    {
        /// <summary>
        /// Gets the prefab.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <returns></returns>
        Entity GetPrefab(string tag);
        /// <summary>
        /// Sets the prefab.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="prefab">The prefab.</param>
        void SetPrefab(string tag, Entity prefab);
    }
}