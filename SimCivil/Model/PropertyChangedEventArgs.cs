using System;

namespace SimCivil.Model
{
    /// <summary>
    /// EventArgs for PropertyChanged event.
    /// </summary>
    /// <typeparam name="T">Property Type.</typeparam>
    /// <seealso cref="System.EventArgs" />
    public class PropertyChangedEventArgs<T> : EventArgs
    {
        /// <summary>
        /// Gets the old value.
        /// </summary>
        /// <value>
        /// The old value.
        /// </value>
        public T OldValue { get; }
        /// <summary>
        /// Gets the new value.
        /// </summary>
        /// <value>
        /// The new value.
        /// </value>
        public T NewValue { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyChangedEventArgs{T}"/> class.
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        public PropertyChangedEventArgs(T oldValue,T newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}