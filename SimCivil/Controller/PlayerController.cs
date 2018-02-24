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
// SimCivil - SimCivil - PlayerController.cs
// Create Date: 2017/08/25
// Update Date: 2018/02/10

using System;
using System.Text;

using SimCivil.Auth;
using SimCivil.Components;
using SimCivil.Contract;
using SimCivil.Model;
using SimCivil.Rpc;
using SimCivil.Rpc.Session;

namespace SimCivil.Controller
{
    /// <summary>
    /// Handle all player's action.
    /// </summary>
    [RoleRequired]
    public class PlayerController : IPlayerController, ICallWarper
    {
        /// <summary>
        /// Gets the session.
        /// </summary>
        /// <value>
        /// The session.
        /// </value>
        public IRpcSession Session { get; }

        /// <summary>
        /// Gets the role.
        /// </summary>
        /// <value>
        /// The role.
        /// </value>
        public Entity Role { get; private set; }

        /// <summary>
        /// Gets the unit.
        /// </summary>
        /// <value>
        /// The unit.
        /// </value>
        public UnitComponent Unit { get; private set; }

        /// <summary>
        /// Gets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public PositionComponent Position { get; private set; }

        /// <summary>
        /// Gets the state of the move.
        /// </summary>
        /// <value>
        /// The state of the move.
        /// </value>
        public MovableComponent MoveState { get; private set; }

        /// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
        public PlayerController(IRpcSession session)
        {
            Session = session;
        }

        /// <summary>
        /// Befores the call.
        /// </summary>
        /// <param name="session">The session.</param>
        public void BeforeCall(IRpcSession session)
        {
            // TODO: Use INotifyChangeCollection to set up
            Role = session.Get<Entity>();
            MoveState = Role.GetMovable();
            Position = Role.GetPos();
            Unit = Role.Get<UnitComponent>();
        }

        /// <summary>
        /// Afters the call.
        /// </summary>
        /// <param name="session">The session.</param>
        public void AfterCall(IRpcSession session) { }

        /// <summary>
        /// Gets the state of the move.
        /// </summary>
        /// <returns></returns>
        public (float X, float Y, float Speed) GetMoveState()
        {
            return (MoveState.Direction.X, MoveState.Direction.Y, MoveState.Speed);
        }

        /// <summary>
        /// Moves the specified direction.
        /// </summary>
        /// <param name="direction">The direction.</param>
        /// <param name="speed">The speed.</param>
        /// <returns></returns>
        public (float X, float Y, float Speed) Move((float X, float Y) direction, float speed)
        {
            float directionLength = direction.GetLength();

            if (!(directionLength > 0))
                throw new ArgumentOutOfRangeException(nameof(direction));
            if (!(speed >= 0 && speed <= Unit.MoveSpeed))
                throw new ArgumentOutOfRangeException(nameof(speed));

            direction = (direction.X / directionLength, direction.Y / directionLength);
            MoveState.Direction = direction;
            MoveState.Speed = speed;

            return (MoveState.Direction.X, MoveState.Direction.Y, MoveState.Speed);
        }

        /// <summary>
        /// Moves the percentage.
        /// </summary>
        /// <param name="direction">The direction.</param>
        /// <param name="relativeSpeed">The relative speed.</param>
        /// <returns></returns>
        public (float X, float Y, float Speed) MovePercentage((float X, float Y) direction, float relativeSpeed)
        {
            // TODO
            return (MoveState.Direction.X, MoveState.Direction.Y, MoveState.Speed);
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            MoveState.Speed = 0;
        }

        /// <summary>
        /// Interactions the specified target.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="interactionType">Type of the interaction.</param>
        public void Interaction(Guid target, InteractionType interactionType)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Builds the specified tile element.
        /// </summary>
        /// <param name="tileElement">The tile element.</param>
        /// <param name="position">The position.</param>
        public void Build(Guid tileElement, (int X, int Y) position)
        {
            throw new NotImplementedException();
        }
    }
}