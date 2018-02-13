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
// Update Date: 2018/02/02

using System;
using System.Text;
using System.Threading.Tasks;

using JetBrains.Annotations;

namespace SimCivil.Contract
{
    public interface IViewSynchronizer
    {
        void RegisterViewSync(Action<ViewChange> callback);
    }

    public class ViewChange
    {
        public int TickCount { get; set; }
        [CanBeNull]
        public TileDto[] TileChange { get; set; }
        [CanBeNull]
        public EntityDto[] EntityChange { get; set; }
        [CanBeNull]
        public ViewEvent[] Events { get; set; }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return
                $"{nameof(TileChange)}: {TileChange?.Length ?? 0}, {nameof(EntityChange)}: {EntityChange?.Length ?? 0}, {nameof(Events)}: {Events?.Length ?? 0}";
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
        public Guid Id { get; set; }
        [CanBeNull]
        public string Name { get; set; }
        public (float X, float Y) Pos { get; set; }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Name)}: {Name}, {nameof(Pos)}: {Pos}";
        }
    }

    public class TileDto
    {
        public (int X, int Y) Position { get; set; }
        public string Surface { get; set; }
    }
}

namespace SimCivil.Contract
{
    public enum ViewEventType
    {
        EntityLeave
    }
}