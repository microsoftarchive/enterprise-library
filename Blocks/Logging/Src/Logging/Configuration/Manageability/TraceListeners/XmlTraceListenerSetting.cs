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
    /// Represents an instance of <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.XmlTraceListenerData"/>
    /// as an instrumentation class.
    /// </summary>
    [ManagementEntity]
    public class XmlTraceListenerSetting : TraceListenerSetting
    {
        string fileName;

        /// <summary>
        /// Initialize a new instance of the <see cref="XmlTraceListenerSetting"/> class with the 
        /// listerner configuration, the name of the listener, the file name to use and the trace output
        /// options.
        /// </summary>
        /// <param name="sourceElement">The listener configuration.</param>
        /// <param name="name">The name of the listener.</param>
        /// <param name="fileName">The file name to use.</param>
        /// <param name="traceOutputOptions">The trace output options.</param>
		/// <param name="filter">The filter value.</param>
		public XmlTraceListenerSetting(XmlTraceListenerData sourceElement,
                                       string name,
                                       string fileName,
									   string traceOutputOptions,
									   string filter)
            : base(sourceElement, name, traceOutputOptions, filter)
        {
            this.fileName = fileName;
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
        /// Returns the <see cref="XmlTraceListenerSetting"/> instance corresponding to the provided values for the key properties.
        /// </summary>
        /// <param name="ApplicationName">The value for the ApplicationName key property.</param>
        /// <param name="SectionName">The value for the SectionName key property.</param>
        /// <param name="Name">The value for the Name key property.</param>
        /// <returns>The published <see cref="XmlTraceListenerSetting"/> instance specified by the values for the key properties,
        /// or <see langword="null"/> if no such an instance is currently published.</returns>
        [ManagementBind]
        public static XmlTraceListenerSetting BindInstance(string ApplicationName,
                                                           string SectionName,
                                                           string Name)
        {
            return BindInstance<XmlTraceListenerSetting>(ApplicationName, SectionName, Name);
        }

        /// <summary>
        /// Returns an enumeration of the published <see cref="XmlTraceListenerSetting"/> instances.
        /// </summary>
        [ManagementEnumerator]
        public static IEnumerable<XmlTraceListenerSetting> GetInstances()
        {
            return GetInstances<XmlTraceListenerSetting>();
        }

        /// <summary>
        /// Saves the changes on the <see cref="XmlTraceListenerSetting"/> to its corresponding configuration object.
        /// </summary>
        /// <param name="sourceElement">The configuration object on which the changes must be saved.</param>
        /// <returns><see langword="true"/> if changes have been saved, <see langword="false"/> otherwise.</returns>
        protected override bool SaveChanges(ConfigurationElement sourceElement)
        {
            return XmlTraceListenerDataWmiMapper.SaveChanges(this, sourceElement);
        }
    }
}
