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
using System.Management.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using System.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability
{
	/// <summary>
	/// Represents an instance of <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.TraceSourceData"/>
	/// as an instrumentation class.
	/// </summary>
	[ManagementEntity]
	public partial class TraceSourceSetting : NamedConfigurationSetting
	{
		private string defaultLevel;
		private string[] traceListeners;
		private string kind;

        /// <summary>
        /// Initialize a new instance of the <see cref="TraceSourceSetting"/> class.
        /// </summary>
        /// <param name="sourceElement">The <see cref="TraceSourceData"/> configuration element.</param>
        /// <param name="name">The name of the trace source.</param>
        /// <param name="defaultLevel">The default level.</param>
        /// <param name="traceListeners">The list of trace listeners.</param>
        /// <param name="kind">The kind of listner.</param>
		public TraceSourceSetting(TraceSourceData sourceElement,
			string name, 
			string defaultLevel, 
			string[] traceListeners, 
			string kind)
			: base(sourceElement, name)
		{
			this.defaultLevel = defaultLevel;
			this.traceListeners = traceListeners;
			this.kind = kind;
		}

		/// <summary>
		/// Gets the name of the value of the default level for the represented configuration element.
		/// </summary>
		[ManagementConfiguration]
		public string DefaultLevel
		{
			get { return defaultLevel; }
			set { defaultLevel = value; }
		}

		/// <summary>
		/// Gets the names of the referenced trace listeners for the represented configuration element.
		/// </summary>
		[ManagementConfiguration]
		public string[] TraceListeners
		{
			get { return traceListeners; }
			set { traceListeners = value; }
		}

		/// <summary>
		/// Gets the kind of the represented configuration element.
		/// </summary>
		/// <remarks>
		/// Trace sources can be:
		/// <list type="table">
		/// <listheader>
		/// <term>Kind</term>
		/// <description>Description</description>
		/// </listheader>
		/// <item><term>Category</term>
		/// <description>A plain category source.</description></item>
		/// <item><term>All events</term>
		/// <description>The <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.SpecialTraceSourcesData.AllEventsTraceSource">allEvents</see> special source.</description></item>
		/// <item><term>Errors</term>
		/// <description>The <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.SpecialTraceSourcesData.ErrorsTraceSource">errors</see> special source.</description></item>
		/// <item><term>Not processed</term>
		/// <description>The <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.SpecialTraceSourcesData.NotProcessedTraceSource">notProcessed</see> special source.</description></item>
		/// </list>
		/// </remarks>
		[ManagementProbe]
		public string Kind
		{
			get { return kind; }
		}

        /// <summary>
        /// Returns an enumeration of the published <see cref="TraceSourceSetting"/> instances.
        /// </summary>
        [ManagementEnumerator]
        public static IEnumerable<TraceSourceSetting> GetInstances()
        {
            return NamedConfigurationSetting.GetInstances<TraceSourceSetting>();
        }

        /// <summary>
        /// Returns the <see cref="TraceSourceSetting"/> instance corresponding to the provided values for the key properties.
        /// </summary>
        /// <param name="ApplicationName">The value for the ApplicationName key property.</param>
        /// <param name="SectionName">The value for the SectionName key property.</param>
        /// <param name="Name">The value for the Name key property.</param>
        /// <returns>The published <see cref="TraceSourceSetting"/> instance specified by the values for the key properties,
        /// or <see langword="null"/> if no such an instance is currently published.</returns>
        [ManagementBind]
        public static TraceSourceSetting BindInstance(string ApplicationName, string SectionName, string Name)
        {
            return NamedConfigurationSetting.BindInstance<TraceSourceSetting>(ApplicationName, SectionName, Name);
        }

        /// <summary>
        /// Saves the changes on the <see cref="TraceSourceSetting"/> to its corresponding configuration object.
        /// </summary>
        /// <param name="sourceElement">The configuration object on which the changes must be saved.</param>
        /// <returns><see langword="true"/> if changes have been saved, <see langword="false"/> otherwise.</returns>
		protected override bool SaveChanges(ConfigurationElement sourceElement)
		{
			return LoggingSettingsWmiMapper.SaveChanges(this, sourceElement);
		}
	}
}
