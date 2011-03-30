using System;

namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
    partial class LogWriter
    {
        /// <summary>
        /// Starts a new tracing operation.
        /// </summary>
        /// <param name="operation">The operation id.</param>
        /// <returns>A <see cref="Tracer"/> representing the tracing operation.</returns>
        public abstract Tracer StartTrace(string operation);

        /// <summary>
        /// Starts a new tracing operation.
        /// </summary>
        /// <param name="operation">The operation id.</param>
        /// <param name="activityId">The activity id.</param>
        /// <returns>A <see cref="Tracer"/> representing the tracing operation.</returns>
        public abstract Tracer StartTrace(string operation, Guid activityId);
    }
}
