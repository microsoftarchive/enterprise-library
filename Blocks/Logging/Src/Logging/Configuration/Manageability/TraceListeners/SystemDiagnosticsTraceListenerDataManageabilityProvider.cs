//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================


namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.TraceListeners
{
    /// <summary>
    /// Provides an implementation for <see cref="SystemDiagnosticsTraceListenerData"/> that
    /// splits policy overrides processing and WMI objects generation, performing appropriate logging of 
    /// policy processing errors.
    /// </summary>
    public class SystemDiagnosticsTraceListenerDataManageabilityProvider
        : BasicCustomTraceListenerDataManageabilityProvider<SystemDiagnosticsTraceListenerData>
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="SystemDiagnosticsTraceListenerDataManageabilityProvider"/> class.
        /// </summary>
        public SystemDiagnosticsTraceListenerDataManageabilityProvider()
        { }
    }
}
