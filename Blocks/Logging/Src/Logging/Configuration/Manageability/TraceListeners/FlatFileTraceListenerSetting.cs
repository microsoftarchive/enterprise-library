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

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.TraceListeners
{
    /// <summary>
    /// Represents an instance of <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.FlatFileTraceListenerData"/>
    /// as an instrumentation class.
    /// </summary>
    [ManagementEntity]
    public partial class FlatFileTraceListenerSetting : TraceListenerSetting
    {
        string fileName;
        string footer;
        string formatter;
        string header;

        /// <summary>
        /// Initialize a new instance of the <see cref="FlatFileTraceListenerSetting"/> class.
        /// </summary>
        /// <param name="sourceElement">The configuration source element.</param>
        /// <param name="name">The name of the trace listener.</param>
        /// <param name="fileName">The file name to use.</param>
        /// <param name="header">The header to use.</param>
        /// <param name="footer">The footer to use.</param>
        /// <param name="formatter">The formatter to use.</param>
        /// <param name="traceOutputOptions">The trace output options.</param>
		/// <param name="filter">The filter value.</param>
		public FlatFileTraceListenerSetting(FlatFileTraceListenerData sourceElement,
                                            string name,
                                            string fileName,
                                            string header,
                                            string footer,
                                            string formatter,
                                            string traceOutputOptions,
											string filter)
            : base(sourceElement, name, traceOutputOptions, filter)
        {
            this.fileName = fileName;
            this.header = header;
            this.footer = footer;
            this.formatter = formatter;
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
        /// Returns the <see cref="FlatFileTraceListenerSetting"/> instance corresponding to the provided values for the key properties.
        /// </summary>
        /// <param name="ApplicationName">The value for the ApplicationName key property.</param>
        /// <param name="SectionName">The value for the SectionName key property.</param>
        /// <param name="Name">The value for the Name key property.</param>
        /// <returns>The published <see cref="FlatFileTraceListenerSetting"/> instance specified by the values for the key properties,
        /// or <see langword="null"/> if no such an instance is currently published.</returns>
        [ManagementBind]
        public static FlatFileTraceListenerSetting BindInstance(string ApplicationName,
                                                                string SectionName,
                                                                string Name)
        {
            return BindInstance<FlatFileTraceListenerSetting>(ApplicationName, SectionName, Name);
        }

        /// <summary>
        /// Returns an enumeration of the published <see cref="FlatFileTraceListenerSetting"/> instances.
        /// </summary>
        [ManagementEnumerator]
        public static IEnumerable<FlatFileTraceListenerSetting> GetInstances()
        {
            return GetInstances<FlatFileTraceListenerSetting>();
        }

        /// <summary>
        /// Saves the changes on the <see cref="FlatFileTraceListenerSetting"/> to its corresponding configuration object.
        /// </summary>
        /// <param name="sourceElement">The configuration object on which the changes must be saved.</param>
        /// <returns><see langword="true"/> if changes have been saved, <see langword="false"/> otherwise.</returns>
        protected override bool SaveChanges(ConfigurationElement sourceElement)
        {
            return FlatFileTraceListenerDataWmiMapper.SaveChanges(this, sourceElement);
        }
    }
}