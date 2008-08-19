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
    /// Represents an instance of <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.RollingFlatFileTraceListenerData"/>
    /// as an instrumentation class.
    /// </summary>
    [ManagementEntity]
    public class RollingFlatFileTraceListenerSetting : TraceListenerSetting
    {
        string fileName;
        string footer;
        string formatter;
        string header;
        string rollFileExistsBehavior;
        string rollInterval;
        int rollSizeKB;
        string timeStampPattern;

        /// <summary>
        /// Initialize a new instance of the <see cref="RollingFlatFileTraceListenerDataManageabilityProvider"/> class.
        /// </summary>
        /// <param name="sourceElement">The configuraiton source element for the trace listner.</param>
        /// <param name="name">The name of the tace listener.</param>
        /// <param name="fileName">The file name for the trace listener.</param>
        /// <param name="header">The header to use.</param>
        /// <param name="footer">The fotter to use.</param>
        /// <param name="formatter">The formatter to use.</param>
        /// <param name="rollFileExistsBehavior">The behavior to use if the file exists.</param>
        /// <param name="rollInterval">The roll interval.</param>
        /// <param name="rollSizeKB">The roll size in kilobytes.</param>
        /// <param name="timeStampPattern">The time stamp patter to use.</param>
        /// <param name="traceOutputOptions">The trace output options.</param>
		/// <param name="filter">The filter value.</param>
        public RollingFlatFileTraceListenerSetting(RollingFlatFileTraceListenerData sourceElement,
                                                   string name,
                                                   string fileName,
                                                   string header,
                                                   string footer,
                                                   string formatter,
                                                   string rollFileExistsBehavior,
                                                   string rollInterval,
                                                   int rollSizeKB,
                                                   string timeStampPattern,
												   string traceOutputOptions,
												   string filter)
            : base(sourceElement, name, traceOutputOptions, filter)
        {
            this.fileName = fileName;
            this.header = header;
            this.footer = footer;
            this.formatter = formatter;
            this.rollFileExistsBehavior = rollFileExistsBehavior;
            this.rollInterval = rollInterval;
            this.rollSizeKB = rollSizeKB;
            this.timeStampPattern = timeStampPattern;
        }

        /// <summary>
        /// Gets the file name for the represented configuration element.
        /// </summary>
        [ManagementConfiguration]
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        /// <summary>
        /// Gets the footer for the represented configuration element.
        /// </summary>
        [ManagementConfiguration]
        public string Footer
        {
            get { return footer; }
            set { footer = value; }
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
        /// Gets the header for the represented configuration element.
        /// </summary>
        [ManagementConfiguration]
        public string Header
        {
            get { return header; }
            set { header = value; }
        }

        /// <summary>
        /// Gets the rollFileExistsBehavior for the represented configuration element.
        /// </summary>
        [ManagementConfiguration]
        public string RollFileExistsBehavior
        {
            get { return rollFileExistsBehavior; }
            set { rollFileExistsBehavior = value; }
        }

        /// <summary>
        /// Gets the roll interval for the represented configuration element.
        /// </summary>
        [ManagementConfiguration]
        public string RollInterval
        {
            get { return rollInterval; }
            set { rollInterval = value; }
        }

        /// <summary>
        /// Gets the roll size for the represented configuration element.
        /// </summary>
        [ManagementConfiguration]
        public int RollSizeKB
        {
            get { return rollSizeKB; }
            set { rollSizeKB = value; }
        }

        /// <summary>
        /// Gets the timestamp pattern for the represented configuration element.
        /// </summary>
        [ManagementConfiguration]
        public string TimeStampPattern
        {
            get { return timeStampPattern; }
            set { timeStampPattern = value; }
        }

        /// <summary>
        /// Returns the <see cref="RollingFlatFileTraceListenerSetting"/> instance corresponding to the provided values for the key properties.
        /// </summary>
        /// <param name="ApplicationName">The value for the ApplicationName key property.</param>
        /// <param name="SectionName">The value for the SectionName key property.</param>
        /// <param name="Name">The value for the Name key property.</param>
        /// <returns>The published <see cref="RollingFlatFileTraceListenerSetting"/> instance specified by the values for the key properties, or <see langword="null"/> if no such an instance is currently published.</returns>
        [ManagementBind]
        public static RollingFlatFileTraceListenerSetting BindInstance(string ApplicationName,
                                                                       string SectionName,
                                                                       string Name)
        {
            return BindInstance<RollingFlatFileTraceListenerSetting>(ApplicationName, SectionName, Name);
        }

        /// <summary>
        /// Returns an enumeration of the published <see cref="RollingFlatFileTraceListenerSetting"/> instances.
        /// </summary>
        [ManagementEnumerator]
        public static IEnumerable<RollingFlatFileTraceListenerSetting> GetInstances()
        {
            return GetInstances<RollingFlatFileTraceListenerSetting>();
        }

        /// <summary>
        /// Saves the changes on the <see cref="RollingFlatFileTraceListenerSetting"/> to its corresponding configuration object.
        /// </summary>
        /// <param name="sourceElement">The configuration object on which the changes must be saved.</param>
        /// <returns><see langword="true"/> if changes have been saved, <see langword="false"/> otherwise.</returns>
        protected override bool SaveChanges(ConfigurationElement sourceElement)
        {
            return RollingFlatFileTraceListenerDataWmiMapper.SaveChanges(this, sourceElement);
        }
    }
}