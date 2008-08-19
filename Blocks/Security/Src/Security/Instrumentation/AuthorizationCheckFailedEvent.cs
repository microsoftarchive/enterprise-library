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

namespace Microsoft.Practices.EnterpriseLibrary.Security.Instrumentation
{
    /// <summary>
    /// Represents the WMI event fired when authorization is denied by an instance of <see cref="AuthorizationProvider"/>.
    /// </summary>
    public class AuthorizationCheckFailedEvent : SecurityEvent
    {
        private string userName;
        private string taskName;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationCheckFailedEvent"/> class.
        /// </summary>
        /// <param name="instanceName">The name of the <see cref="AuthorizationProvider"/> instance this event applies to.</param>
        /// <param name="userName">The username for which the authorization failed.</param>
        /// <param name="taskName">The name of the task for which the authorization failed.</param>
        public AuthorizationCheckFailedEvent(string instanceName, string userName, string taskName)
			: base(instanceName)
        {
            this.userName = userName;
            this.taskName = taskName;
        }

        /// <summary>
        /// Gets the username for which the authorization failed.
        /// </summary>
        public string UserName
        {
            get { return userName; }
        }

        /// <summary>
        /// Gets the name of the task for which the authorization failed.
        /// </summary>
        public string TaskName
        {
            get { return taskName; }
        }
    }
}
