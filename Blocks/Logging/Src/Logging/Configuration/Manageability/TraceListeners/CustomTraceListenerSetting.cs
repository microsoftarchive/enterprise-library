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

using System.Collections.Generic;
using System.Configuration;
using System.Management.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.TraceListeners
{
	/// <summary>
	/// Represents an instance of <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.CustomTraceListenerData"/>
	/// as an instrumentation class.
	/// </summary>
	[ManagementEntity]
	public class CustomTraceListenerSetting : TraceListenerSetting
	{
		string[] attributes;
		string formatter;
		string initData;
		string listenerType;

		/// <summary>
		/// Initialize a new instance of the <see cref="CustomTraceListenerSetting"/> class.
		/// </summary>
		/// <param name="sourceElement">The custom listner configuration.</param>
		/// <param name="name">The name of the listener.</param>
		/// <param name="listenerType">The listerner type.</param>
		/// <param name="initData">The initialization data for the listerner.</param>
		/// <param name="attributes">The attributes for the listener.</param>
		/// <param name="traceOutputOptions">The trace output options.</param>
		/// <param name="filter">The filter value.</param>
		/// <param name="formatter">The formatter for the listener.</param>
		public CustomTraceListenerSetting(BasicCustomTraceListenerData sourceElement,
										  string name,
										  string listenerType,
										  string initData,
										  string[] attributes,
										  string traceOutputOptions,
										  string filter,
										  string formatter)
			: base(sourceElement, name, traceOutputOptions, filter)
		{
			this.listenerType = listenerType;
			this.initData = initData;
			this.attributes = attributes;
			this.formatter = formatter;
		}

		/// <summary>
		/// Gets the attributes for the represented configuration element.
		/// </summary>
		/// <remarks>
		/// The attributes are encoded as an string array of name/value pairs.
		/// </remarks>
		[ManagementConfiguration]
		public string[] Attributes
		{
			get { return attributes; }
			set { attributes = value; }
		}

		/// <summary>
		/// Gets the name of the formatter for the represented configuration element.
		/// </summary>
		[ManagementConfiguration]
		public string Formatter
		{
			get { return formatter; }
			set { formatter = value; }
		}

		/// <summary>
		/// Gets the initialization data for the represented configuration element.
		/// </summary>
		[ManagementConfiguration]
		public string InitData
		{
			get { return initData; }
			set { initData = value; }
		}

		/// <summary>
		/// Gets the assembly qualified name of the listener type for 
		/// the represented configuration element.
		/// </summary>
		[ManagementConfiguration]
		public string ListenerType
		{
			get { return listenerType; }
			set { listenerType = value; }
		}

		/// <summary>
		/// Returns the <see cref="CustomTraceListenerSetting"/> instance corresponding to the provided values for the key properties.
		/// </summary>
		/// <param name="ApplicationName">The value for the ApplicationName key property.</param>
		/// <param name="SectionName">The value for the SectionName key property.</param>
		/// <param name="Name">The value for the Name key property.</param>
		/// <returns>The published <see cref="CustomTraceListenerSetting"/> instance specified by the values for the key properties,
		/// or <see langword="null"/> if no such an instance is currently published.</returns>
		[ManagementBind]
		public static CustomTraceListenerSetting BindInstance(string ApplicationName,
															  string SectionName,
															  string Name)
		{
			return BindInstance<CustomTraceListenerSetting>(ApplicationName, SectionName, Name);
		}

		/// <summary>
		/// Returns an enumeration of the published <see cref="CustomTraceListenerSetting"/> instances.
		/// </summary>
		[ManagementEnumerator]
		public static IEnumerable<CustomTraceListenerSetting> GetInstances()
		{
			return GetInstances<CustomTraceListenerSetting>();
		}

		/// <summary>
		/// Saves the changes on the <see cref="CustomTraceListenerSetting"/> to its corresponding configuration object.
		/// </summary>
		/// <param name="sourceElement">The configuration object on which the changes must be saved.</param>
		/// <returns><see langword="true"/> if changes have been saved, <see langword="false"/> otherwise.</returns>
		protected override bool SaveChanges(ConfigurationElement sourceElement)
		{
			return CustomTraceListenerDataWmiMapper.SaveChanges(this, sourceElement);
		}
	}
}