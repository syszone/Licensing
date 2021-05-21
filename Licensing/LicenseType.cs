using System;
using System.Collections.Generic;
using System.Text;

namespace Licensing
{
    /// <summary>
    /// Defines the type of a <see cref="License"/>
    /// </summary>
    public enum LicenseType
    {
        /// <summary>
        /// For trial or demo use
        /// </summary>
        Trial = 1,

        /// <summary>
        /// Standard license
        /// </summary>
        Standard = 2,

        /// <summary>
        /// Subscription license
        /// </summary>
        Subscription = 3
    }
}
