#region license
// ==============================================================================
// Microsoft patterns & practices Enterprise Library
// Semantic Logging Application Block
// ==============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ==============================================================================
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Etw
{
    /// <summary>
    /// Provides data for <see cref="TraceEventService.StatusChanged"/> event.
    /// </summary>
    public class StatusChangedEventArgs : EventArgs
    {
        internal StatusChangedEventArgs(ServiceStatus status)
        {
            this.Status = status;
        }

        /// <summary>
        /// Gets the changed status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public ServiceStatus Status { get; private set; }
    }
}
