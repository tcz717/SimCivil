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
// SimCivil - SimCivil.Contract - IPlayerController.cs
// Create Date: 2018/02/09
// Update Date: 2018/02/09

using System;
using System.Text;

namespace SimCivil.Contract
{
    public interface IPlayerController
    {
        /// <summary>
        /// Gets the state of the move.
        /// </summary>
        /// <returns></returns>
        (float X, float Y, float Speed) GetMoveState();
        /// <summary>
        /// Moves the specified direction.
        /// </summary>
        /// <param name="direction">The direction.</param>
        /// <param name="speed">The speed.</param>
        /// <returns></returns>
        (float X, float Y, float Speed) Move((float X, float Y) direction, float speed);
        /// <summary>
        /// Moves the percentage.
        /// </summary>
        /// <param name="direction">The direction.</param>
        /// <param name="relativeSpeed">The relative speed.</param>
        /// <returns></returns>
        (float X, float Y, float Speed) MovePercentage((float X, float Y) direction, float relativeSpeed);
        void Stop();

        /// <summary>
        /// Interactions the specified target.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="interactionType">Type of the interaction.</param>
        void Interaction(Guid target, InteractionType interactionType);
        /// <summary>
        /// Builds the specified tile element.
        /// </summary>
        /// <param name="tileElement">The tile element.</param>
        /// <param name="position">The position.</param>
        void Build(Guid tileElement, (int X, int Y) position);  
    }

    public enum InteractionType { }
}