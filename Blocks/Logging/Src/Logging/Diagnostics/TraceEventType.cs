using System.ComponentModel;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Diagnostics
{
    /// <summary>
    /// Identifies the type of event that has caused the trace.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1027:MarkEnumsWithFlags", Justification = "Not a flags, but inteded to play nicely with the SourceLevels flag")]
    public enum TraceEventType
    {
        /// <summary>
        /// Fatal error or application crash.
        /// </summary>
        Critical = 1,

        /// <summary>
        /// Recoverable error.
        /// </summary>
        Error = 2,

        /// <summary>
        /// Noncritical problem.
        /// </summary>
        Warning = 4,

        /// <summary>
        /// Informational message.
        /// </summary>
        Information = 8,

        /// <summary>
        /// Debugging trace.
        /// </summary>
        Verbose = 16,

        /// <summary>
        /// Starting of a logical operation.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        Start = 0x100,

        /// <summary>
        /// Suspension of a logical operation.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        Stop = 0x200,
    }
}
