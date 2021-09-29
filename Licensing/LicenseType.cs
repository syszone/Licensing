using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public enum LicenseTypeDuration
    {
        [Description("3 Month")]
        Month3 = 1,
        [Description("6 Month")]
        Month6 = 2,
        [Description("1 Year")]
        Year1 = 3,
        [Description("2 Year")]
        Year2 = 4,
        [Description("3 Year")]
        Year3 = 5,
        [Description("10 Days")]
        Days10 = 6,
        [Description("15 Days")]
        Days15 = 7,
        [Description("20 Days")]
        Days20 = 8,
        [Description("25 Days")]
        Days25 = 9,
        [Description("30 Days")]
        Days30 = 10


    }

}
