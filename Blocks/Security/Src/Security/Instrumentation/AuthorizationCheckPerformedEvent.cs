//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Management.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Instrumentation
{
    /// <summary>
    /// Represents the WMI event fired when an authorization check is performed.
    /// </summary>
    [InstrumentationClass(InstrumentationType.Event)]
    public class AuthorizationCheckPerformedEvent: SecurityEvent
    {
        private string userName;
        private string taskName;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationCheckPerformedEvent"/> class.
        /// </summary>
        /// <param name="instanceName">The name of the <see cref="AuthorizationProvider"/> the authorization check is performed on.</param>
        /// <param name="userName">The name of the authority the authorization check is performed on.</param>
        /// <param name="taskName">The name of the task this authorization check is performed against.</param>
        public AuthorizationCheckPerformedEvent(string instanceName, string userName, string taskName)
			: base(instanceName)
        {
            this.userName = userName;
            this.taskName = taskName;
        }

        /// <summary>
        /// Gets the name of the authority the authorization check is performed on.
        /// </summary>
        public string UserName
        {
            get { return userName; }
        }

        /// <summary>
        /// Gets the name of the task this authorization check is performed against.
        /// </summary>
        public string TaskName
        {
            get { return taskName; }
        }
    }
}
