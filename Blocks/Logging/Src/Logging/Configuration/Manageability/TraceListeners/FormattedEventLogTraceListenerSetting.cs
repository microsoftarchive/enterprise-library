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
    /// Represents an instance of <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.FormattedEventLogTraceListenerData"/>
    /// as an instrumentation class.
    /// </summary>
    [ManagementEntity]
    public class FormattedEventLogTraceListenerSetting : TraceListenerSetting
    {
        string formatter;
        string log;
        string machineName;
        string source;

        /// <summary>
        /// Initialize a new instance of the <see cref="FormattedEventLogTraceListenerSetting"/> class.
        /// </summary>
        /// <param name="sourceElement">The trace listener configuration.</param>
        /// <param name="name">The name of the trace listener.</param>
        /// <param name="source">The source for the trace listener.</param>
        /// <param name="log">The log for the trace listener.</param>
        /// <param name="machineName">The machine name.</param>
        /// <param name="formatter">The formatter to use.</param>
        /// <param name="traceOutputOptions">The trace output options.</param>
		/// <param name="filter">The filter value.</param>
		public FormattedEventLogTraceListenerSetting(FormattedEventLogTraceListenerData sourceElement,
                                                     string name,
                                                     string source,
                                                     string log,
                                                     string machineName,
                                                     string formatter,
													 string traceOutputOptions,
													 string filter)
            : base(sourceElement, name, traceOutputOptions, filter)
        {
            this.source = source;
            this.log = log;
            this.machineName = machineName;
            this.formatter = formatter;
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
        /// Gets the log name for the represented configuration element.
        /// </summary>
        [ManagementConfiguration]
        public string Log
        {
            get { return log; }
            set { log = value; }
        }

        /// <summary>
        /// Gets the machine name for the represented configuration element.
        /// </summary>
        [ManagementConfiguration]
        public string MachineName
        {
            get { return machineName; }
            set { machineName = value; }
        }

        /// <summary>
        /// Gets the source name for the represented configuration element.
        /// </summary>
        [ManagementConfiguration]
        public string Source
        {
            get { return source; }
            set { source = value; }
        }

        /// <summary>
        /// Returns the <see cref="FormattedEventLogTraceListenerSetting"/> instance corresponding to the provided values for the key properties.
        /// </summary>
        /// <param name="ApplicationName">The value for the ApplicationName key property.</param>
        /// <param name="SectionName">The value for the SectionName key property.</param>
        /// <param name="Name">The value for the Name key property.</param>
        /// <returns>The published <see cref="TraceSourceSetting"/> instance specified by the values for the key properties, or <see langword="null"/> if no such an instance is currently published.</returns>
        [ManagementBind]
        public static FormattedEventLogTraceListenerSetting BindInstance(string ApplicationName,
                                                                         string SectionName,
                                                                         string Name)
        {
            return BindInstance<FormattedEventLogTraceListenerSetting>(ApplicationName, SectionName, Name);
        }

        /// <summary>
        /// Returns an enumeration of the published <see cref="FormattedEventLogTraceListenerSetting"/> instances.
        /// </summary>
        [ManagementEnumerator]
        public static IEnumerable<FormattedEventLogTraceListenerSetting> GetInstances()
        {
            return GetInstances<FormattedEventLogTraceListenerSetting>();
        }

        /// <summary>
        /// Pushes the current property values to the <see cref="ConfigurationSetting.SourceElement"/>
        /// </summary>
        /// <remarks>Must be overridden by subclasses to perform the actual save.</remarks>
        /// <param name="sourceElement">The configuration object on which the changes must be saved.</param>
        /// <returns><see langword="true"/> if changes have been saved, <see langword="false"/> otherwise.</returns>
        protected override bool SaveChanges(ConfigurationElement sourceElement)
        {
            return FormattedEventLogTraceListenerDataWmiMapper.SaveChanges(this, sourceElement);
        }
    }
}
