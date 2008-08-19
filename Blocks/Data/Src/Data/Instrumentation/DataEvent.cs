//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
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

namespace Microsoft.Practices.EnterpriseLibrary.Data.Instrumentation
{
    /// <summary>
    /// Base class for data WMI events.
    /// </summary>
    public abstract class DataEvent: BaseWmiEvent
    {
		private string instanceName;

		/// <summary>
		/// Initializes a new instance of the <see cref="DataEvent"/> class.
		/// </summary>
		/// <param name="instanceName">name of the <see cref="Database"/> this event applies on.</param>
		protected DataEvent(string instanceName)
		{
			this.instanceName = instanceName;
		}

		/// <summary>
		/// Gets the name of the <see cref="Database"/> this event applies on.
		/// </summary>
		public string InstanceName
		{
			get { return instanceName; }
		}
	}
}
