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

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Filters
{
	/// <summary>
	/// Represents a boolean on/off filter.
	/// </summary>
	[ConfigurationElementType(typeof(LogEnabledFilterData))]
	public class LogEnabledFilter : LogFilter
	{
		private bool enabled = false;

		/// <summary>
		/// Initializes an instance of <see cref="LogEnabledFilter"/>.
		/// </summary>
		/// <param name="name">The name of the filter.</param>
		/// <param name="enabled">True if the filter allows messages, false otherwise.</param>
		public LogEnabledFilter(string name, bool enabled)
			: base(name)
		{
			this.enabled = enabled;
		}

		/// <summary>
		/// Tests to see if a message meets the criteria to be processed. 
		/// </summary>
		/// <param name="log">Log entry to test.</param>
		/// <returns><b>true</b> if the message passes through the filter and should be logged, <b>false</b> otherwise.</returns>
		public override bool Filter(LogEntry log)
		{
			return enabled;
		}

		/// <summary>
		/// Gets or set the boolean flag for the filter.
		/// </summary>
		public bool Enabled
		{
			get { return this.enabled; }
			set { this.enabled = value; }
		}
	}
}
