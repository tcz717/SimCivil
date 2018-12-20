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
// SimCivil - SimCivil.Orleans.Interfaces - ITerrainRepository.cs
// Create Date: 2018/10/17
// Update Date: 2018/10/18

using System;
using System.Text;

using JetBrains.Annotations;

namespace SimCivil.Orleans.Interfaces.Service
{
    public interface ITerrainRepository
    {
        Terrain GetTerrain(int id);
    }

    public class Terrain
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float BaseMoveSpeed { get; set; }
        public TerrainFlags Flags { get; set; }

        /// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
        public Terrain(
            int id = 0,
            float baseMoveSpeed = 0,
            TerrainFlags flags = TerrainFlags.Normal) : this(id, null, baseMoveSpeed, flags) { }

        /// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
        public Terrain(
            int id,
            [CanBeNull] string name,
            float baseMoveSpeed) : this(id, name, baseMoveSpeed, TerrainFlags.Normal) { }

        /// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
        public Terrain(
            int id,
            [CanBeNull] string name,
            float baseMoveSpeed,
            TerrainFlags flags)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            BaseMoveSpeed = baseMoveSpeed;
            Flags = flags;
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return
                $"{nameof(Id)}: {Id}, {nameof(Name)}: {Name}, {nameof(BaseMoveSpeed)}: {BaseMoveSpeed}, {nameof(Flags)}: {Flags}";
        }
    }

    [Flags]
    public enum TerrainFlags
    {
        Normal = 0,
        NotMovable = 1,
        NoBuilding = 2
    }
}