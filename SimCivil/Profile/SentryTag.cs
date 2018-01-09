using System;

using log4net.Layout;

namespace SimCivil.Profile
{
    /// <summary>
    /// 
    /// </summary>
    public class SentryTag
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the layout.
        /// </summary>
        /// <value>
        /// The layout.
        /// </value>
        public IRawLayout Layout { get; set; }
    }
}