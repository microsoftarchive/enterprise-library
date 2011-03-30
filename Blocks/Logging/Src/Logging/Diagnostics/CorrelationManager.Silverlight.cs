using System;
using System.Collections.Generic;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Diagnostics
{
    /// <summary>
    /// Correlates traces that are part of a logical transaction. 
    /// </summary>
    /// <remarks>
    /// This class supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
    /// </remarks>
    public sealed class CorrelationManager
    {
        /// Need to use thread-static fields for thread-isolated storage in Silverlight

        [ThreadStatic]
        private static Stack<object> logicalOperationStack;

        [ThreadStatic]
        private static Guid activityId;

        internal CorrelationManager()
        { }

        /// <summary>
        /// Gets or sets the activity id for the current thread.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Consistent with the .NET API")]
        public Guid ActivityId
        {
            get { return activityId; }
            set { activityId = value; }
        }

        /// <summary>
        /// Gets the logical operation stack for the current thread.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Consistent with the .NET API")]
        public Stack<object> LogicalOperationStack
        {
            get { return this.GetLogicalOperationStack(); }
        }

        /// <summary>
        /// Starts a new logical operation with the given operation id.
        /// </summary>
        /// <param name="operationId">The operation id.</param>
        public void StartLogicalOperation(object operationId)
        {
            this.GetLogicalOperationStack().Push(operationId);
        }

        /// <summary>
        /// Stops the last operation.
        /// </summary>
        public void StopLogicalOperation()
        {
            this.GetLogicalOperationStack().Pop();
        }

        private Stack<object> GetLogicalOperationStack()
        {
            var stack = logicalOperationStack;

            if (stack == null)
            {
                logicalOperationStack = stack = new Stack<object>();
            }

            return stack;
        }
    }
}
