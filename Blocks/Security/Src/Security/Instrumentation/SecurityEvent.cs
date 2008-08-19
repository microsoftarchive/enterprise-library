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
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using System.Management.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Instrumentation
{
    /// <summary>
    /// Base class for security WMI events.
    /// </summary>
    public abstract class SecurityEvent : BaseWmiEvent
    {
		private string instanceName;

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityEvent"/> class.
        /// </summary>
		/// <param name="instanceName">The name of the provider this event applies to.</param>
        public SecurityEvent(string instanceName)
        {
			this.instanceName = instanceName;
        }

		/// <summary>
        /// Gets the name of the provider this event applies to.
		/// </summary>
		public string InstanceName
		{
			get { return instanceName; }
		}
	}
}
