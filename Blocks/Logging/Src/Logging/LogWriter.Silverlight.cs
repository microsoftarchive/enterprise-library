using System;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;

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

        /// <summary>
        /// Provides an update context to batch change requests to the <see cref="LogWriter"/> configuration,
        /// and apply all the changes in a single call <see cref="ILogWriterUpdateContext.ApplyChanges"/>.
        /// </summary>
        /// <returns>Returns an <see cref="ILogWriterUpdateContext"/> instance that can be used to apply the configuration changes.</returns>
        public abstract ILogWriterUpdateContext GetUpdateContext();
    }
}
