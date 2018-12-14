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
// SimCivil - SimCivil.Orleans.Interfaces - Config.cs
// Create Date: 2018/06/14
// Update Date: 2018/12/13

using System;
using System.Text;

using Orleans.Concurrency;

namespace SimCivil.Orleans.Interfaces
{
    /// <summary>
    /// Save all Configs include constant value and variable value.
    /// It's recommend to put 
    /// <code>
    /// using static SimCivil.Config;
    /// </code>
    /// </summary>
    [Immutable, Obsolete]
    public class Config
    {
        /// <summary>
        /// The default period
        /// </summary>
        public const int DefaultPeriod = 50;

        /// <summary>
        /// The default atlas width
        /// </summary>
        public const int DefaultAtlasSize = 64;

        /// <summary>
        /// The maximum ground entities
        /// </summary>
        public const int MaxGroundEntities = 4;

        /// <summary>
        /// The sea level
        /// </summary>
        public const int SeaLevel = 64;

        /// <summary>
        /// The prefab name key
        /// </summary>
        public const string PrefabNameKey = "PrefabName";

        /// <summary>
        /// The chunk size
        /// </summary>
        public const int ChunkSize = 32;

        /// <summary>
        /// The sim civil protocol version
        /// </summary>
        public static readonly Version SimCivilProtocolVersion = new Version(1, 0);

        /// <summary>
        /// The available sex
        /// </summary>
        public static readonly string[] AvailableSex = {"male", "female"};

        public static readonly double LagLearningRate = 0.9;
        public static readonly double MaxLag = 1000;
        public static readonly TimeSpan MaxUpdateDelta = TimeSpan.FromMilliseconds(200);
        public static readonly bool NoSpeedFix = false;

        /// <summary>
        /// Gets or sets the plant height limit.
        /// </summary>
        /// <value>
        /// The plant height limit.
        /// </value>
        public int PlantHeightLimit { get; set; } = 200;

        /// <summary>
        /// Gets or sets the plant density.
        /// </summary>
        /// <value>
        /// The plant density.
        /// </value>
        public double PlantDensity { get; set; } = 0.3;

        /// <summary>
        /// Gets or sets the spawn point.
        /// </summary>
        /// <value>
        /// The spawn point.
        /// </value>
        public (int X, int Y) SpawnPoint { get; set; }

        /// <summary>
        /// Gets or sets the seed.
        /// </summary>
        /// <value>
        /// The seed.
        /// </value>
        public int Seed { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the available races.
        /// </summary>
        /// <value>
        /// The available races.
        /// </value>
        public string[] AvailableRaces { get; set; } = {"human"};
    }
}