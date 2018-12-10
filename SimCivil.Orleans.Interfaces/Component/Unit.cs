using System;
using System.Text;

using SimCivil.Contract.Model;

namespace SimCivil.Orleans.Interfaces.Component
{
    public class Unit
    {
        /// <summary>
        /// Gets or sets the gender.
        /// </summary>
        /// <value>
        /// The gender.
        /// </value>
        public Gender Gender { get; set; }
        /// <summary>
        /// Gets or sets the race.
        /// </summary>
        /// <value>
        /// The race.
        /// </value>
        public Race Race { get; set; }

        /// <summary>
        /// Gets or sets the move speed.
        /// </summary>
        /// <value>
        /// The move speed.
        /// </value>
        public float MoveSpeed { get; set; }

        /// <summary>
        /// Gets or sets the sight range.
        /// </summary>
        /// <value>
        /// The sight range.
        /// </value>
        public float SightRange { get; set; }
    }
}