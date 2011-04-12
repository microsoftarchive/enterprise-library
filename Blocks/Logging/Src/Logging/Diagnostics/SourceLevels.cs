using System;
using System.ComponentModel;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Diagnostics
{
    /// <summary>
    /// Specifies the levels of trace messages filtered for sources.
    /// </summary>
    [Flags]
    public enum SourceLevels
    {
        /// <summary>
        /// Does not allow any events through.
        /// </summary>
        Off = 0,

        /// <summary>
        /// Allows only Critical events through.
        /// </summary>
        Critical = 1,

        /// <summary>
        /// Allows Critical and Error events through.
        /// </summary>
        Error = 3,

        /// <summary>
        /// Allows Critical, Error, and Warning events through.
        /// </summary>
        Warning = 7,

        /// <summary>
        /// Allows Critical, Error, Warning, and Information events through.
        /// </summary>
        Information = 15,

        /// <summary>
        /// Allows Critical, Error, Warning, Information, and Verbose events through.
        /// </summary>
        Verbose = 31,

        /// <summary>
        /// Allows the Stop and Start events through.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        ActivityTracing = 0xff00,

        /// <summary>
        /// Allows all events through.
        /// </summary>
        All = -1,
    }
}
