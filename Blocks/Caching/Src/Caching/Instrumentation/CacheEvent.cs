//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Instrumentation
{
	/// <summary>
    /// Base class for caching WMI events.
	/// </summary>
	public abstract class CacheEvent : BaseWmiEvent
	{
		private string instanceName;

		/// <summary>
        /// Initializes a new instance of the <see cref="CacheEvent"/> class.
		/// </summary>
		/// <param name="instanceName">name of the <see cref="CacheManager"/> this event applies on.</param>
		protected CacheEvent(string instanceName)
		{
			this.instanceName = instanceName;
		}

		/// <summary>
        /// Gets the name of the <see cref="CacheManager"/> this event applies on.
		/// </summary>
		public string InstanceName
		{
			get { return instanceName; }
		}
	
	}
}
