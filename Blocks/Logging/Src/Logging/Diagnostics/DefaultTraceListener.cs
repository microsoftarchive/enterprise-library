
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Diagnostics
{
    /// <summary>
    /// Default trace listener, with no implementation.
    /// </summary>
    public class DefaultTraceListener : TraceListener
    {
        /// <summary>
        /// Writes the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public override void Write(string message)
        {
        }

        /// <summary>
        /// Writes the specified message, followed by a line terminator.
        /// </summary>
        /// <param name="message">The message.</param>
        public override void WriteLine(string message)
        {
        }
    }
}
