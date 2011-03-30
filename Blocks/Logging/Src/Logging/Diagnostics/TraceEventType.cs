using System.ComponentModel;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Diagnostics
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1027:MarkEnumsWithFlags", Justification = "Not a flags, but inteded to play nicely with the SourceLevels flag")]
    public enum TraceEventType
    {
        Critical = 1,
        Error = 2,
        Warning = 4,
        Information = 8,
        Verbose = 16,
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        Start = 0x100,
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        Stop = 0x200,
    }
}
