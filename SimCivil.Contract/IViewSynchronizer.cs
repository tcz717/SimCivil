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
// SimCivil - SimCivil.Contract - IViewSynchronizer.cs
// Create Date: 2018/01/10
// Update Date: 2018/12/18

using System;
using System.Text;
using System.Threading.Tasks;

using JetBrains.Annotations;

namespace SimCivil.Contract
{
    public interface IViewSynchronizer
    {
        /// <summary>Registers the view synchronize.</summary>
        /// <param name="callback">The callback.</param>
        void RegisterViewSync(Action<ViewChange> callback);

        /// <summary>Deregisters the view synchronize.</summary>
        void DeregisterViewSync();

        /// <summary>Gets the atlas.</summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        Task<TileDto[]> GetAtlas((int X, int Y) index);

        /// <summary>Gets the atlas time stamp in **UTC**.</summary>
        /// <param name="index">The atlas index.</param>
        /// <returns>Last edit time in **UTC**</returns>
        Task<DateTime> GetAtlasTimeStamp((int X, int Y) index);

        /// <summary>Gets the appearance.</summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        Task<AppearanceDto[]> GetAppearance(Guid entity);
    }

    public class AppearanceDto
    {
        public AppearanceType Type { get; set; }
        public uint Id { get; set; }
        public uint PrimaryColor { get; set; }
        public uint SecondaryColor { get; set; }
        public float Quality { get; set; }
        public Material Material { get; set; }
    }

    public enum Material
    {
        None,
        Wood,
    }

    public enum AppearanceType
    {
        Block = 1,
        Body = 10,
        Hair,
        Helmet = 20,
        Mask,
        Necklace,
        Shoulder,
        Belt,
        Armor,
        Backpack,
        RightHanded,
        LeftHanded,
        TwoHanded,
        Gloves,
        Pants,
        Shoes
    }

    public class ViewChange
    {
        /// <summary>Gets or sets the tick count.</summary>
        /// <value>The tick count.</value>
        public int TickCount { get; set; }
        /// <summary>Gets or sets the tile change.</summary>
        /// <value>The tile change.</value>
        [CanBeNull]
        public TileDto[] TileChange { get; set; }
        /// <summary>Gets or sets the entity change.</summary>
        /// <value>The entity change.</value>
        [CanBeNull]
        public EntityDto[] EntityChange { get; set; }
        /// <summary>Gets or sets the events.</summary>
        /// <value>The events.</value>
        [CanBeNull]
        public ViewEvent[] Events { get; set; }
        /// <summary></summary>
        public (float X, float Y) Position { get; set; }
        /// <summary>Gets or sets the index of the atlas.</summary>
        /// <value>The index of the atlas.</value>
        public (int X, int Y) AtlasIndex { get; set; }
        /// <summary>Gets or sets the speed.</summary>
        /// <value>The speed.</value>
        public float Speed { get; set; }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $"{nameof(TickCount)}: {TickCount}, {nameof(Position)}: {Position}, {nameof(Speed)}: {Speed}";
        }
    }

    public class ViewEvent
    {
        public ViewEventType EventType { get; set; }

        public Guid TargetEntityId { get; set; }

        public static ViewEvent EntityLeave(Guid targetEntityId)
        {
            return new ViewEvent
            {
                EventType = ViewEventType.EntityLeave,
                TargetEntityId = targetEntityId
            };
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $"{nameof(EventType)}: {EventType}, {nameof(TargetEntityId)}: {TargetEntityId}";
        }
    }

    public class EntityDto
    {
        /// <summary>Gets or sets the identifier.</summary>
        /// <value>The identifier.</value>
        public Guid Id { get; set; }
        /// <summary>Gets or sets the name.</summary>
        /// <value>The name.</value>
        [CanBeNull]
        public string Name { get; set; }
        /// <summary>Gets or sets the position.</summary>
        /// <value>The position.</value>
        public (float X, float Y) Pos { get; set; }
        /// <summary>Gets or sets the heath point. 1.0 means the hp. If the heath point is not applicable, it will be -1.</summary>
        /// <value>The hp.</value>
        public float Hp { get; set; }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Name)}: {Name}, {nameof(Pos)}: {Pos}, {nameof(Hp)}: {Hp}";
        }
    }

    public class TileDto
    {
        /// <summary>Gets or sets the position.</summary>
        /// <value>The position.</value>
        public (int X, int Y) Position { get; set; }
        /// <summary>Gets or sets the terrain.</summary>
        /// <value>The terrain.</value>
        public int Terrain { get; set; }
        /// <summary>Gets or sets the height.</summary>
        /// <value>The height.</value>
        public int Height { get; set; }
    }
}

namespace SimCivil.Contract
{
    public enum ViewEventType
    {
        EntityLeave
    }
}