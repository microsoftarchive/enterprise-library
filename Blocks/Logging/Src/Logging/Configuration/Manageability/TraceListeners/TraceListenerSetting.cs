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

using System.Diagnostics;
using System.Management.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.TraceListeners
{
	/// <summary>
	/// Represents the configuration information for an instance of a concrete subclass of
	/// <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.TraceListenerData"/>.
	/// </summary>
	/// <seealso cref="Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.TraceListenerData"/>
	/// <seealso cref="NamedConfigurationSetting"/>
	/// <seealso cref="ConfigurationSetting"/>
	[ManagementEntity]
	public abstract class TraceListenerSetting : NamedConfigurationSetting
	{
		private string traceOutputOptions;
		private string filter;

		/// <summary>
		/// Initializes a new instance of the <see cref="TraceListenerSetting"/> class.
		/// </summary>
		protected TraceListenerSetting(TraceListenerData sourceElement, string name, string traceOutputOptions, string filter)
			: base(sourceElement, name)
		{
			this.traceOutputOptions = traceOutputOptions;
			this.filter = filter;
		}

		/// <summary>
		/// Gets the trace options for the represented configuration element.
		/// </summary>
		[ManagementConfiguration]
		public string TraceOutputOptions
		{
			get { return traceOutputOptions; }
			set { traceOutputOptions = value; }
		}

		/// <summary>
		/// Gets or sets the filter for the represented configuration element.
		/// </summary>
		/// <remarks>
		/// The property represents a <see cref="SourceLevels"/> value.
		/// </remarks>
		[ManagementConfiguration]
		public string Filter
		{
			get { return filter; }
			set { filter = value; }
		}
	}
}