using System;
using System.Collections.Generic;
using System.Text;

namespace SimCivil
{
    /// <summary>
    /// Implement this interface to be called period.
    /// </summary>
    public interface ITicker
    {
        /// <summary>
        /// Called when tick.
        /// </summary>
        /// <param name="tickCount">Total tick.</param>
        void Update(int tickCount);
        /// <summary>
        /// Start Service.
        /// </summary>
        void Start();
        /// <summary>
        /// Stop Service.
        /// </summary>
        void Stop();
    }
}
