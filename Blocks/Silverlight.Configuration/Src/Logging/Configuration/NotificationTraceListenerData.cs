//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Silverlight Design-Time Configuration
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.ComponentModel;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Silverlight.Configuration.Logging.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
    /// <summary>
    /// <para>Configuration data for an <c>NotificationTraceListener</c>.</para>
    /// </summary>	
    [ResourceDescription(typeof(LoggingResources), "NotificationTraceListenerDataDescription")]
    [ResourceDisplayName(typeof(LoggingResources), "NotificationTraceListenerDataDisplayName")]
    [Browsable(true)]
    public class NotificationTraceListenerData : TraceListenerData
    {
        /// <summary>
        /// <para>Initializes a new instance of <see cref="NotificationTraceListenerData"/> class.</para>
        /// </summary>
        public NotificationTraceListenerData()
            : base(typeof(NotificationTraceListener))
        {
        }

        /// <summary>
        /// Overridden in order to hide from the configuration designtime.
        /// </summary>
        [Browsable(false)]
        public new TraceOptions TraceOutputOptions
        {
            get;
            set;
        }

        /// <summary>
        /// Overridden in order to hide from the configuration designtime.
        /// </summary>
        [Browsable(false)]
        public new SourceLevels Filter
        {
            get;
            set;
        }
    }
}
