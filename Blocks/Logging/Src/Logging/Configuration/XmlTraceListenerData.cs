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
using System.Configuration;
using System.Diagnostics;
using System.Linq.Expressions;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
    /// <summary>
    /// Represents the configuration settings that describe a <see cref="XmlTraceListener"/>.
    /// </summary>
    public class XmlTraceListenerData : TraceListenerData
    {
        private const string fileNameProperty = "fileName";

        /// <summary>
        /// Initializes a <see cref="XmlTraceListenerData"/>.
        /// </summary>
        public XmlTraceListenerData()
        {
        }

        /// <summary>
        /// Initializes a <see cref="XmlTraceListenerData"/> with a filename and a formatter name.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        /// <param name="name">The name for the configuration object.</param>
        public XmlTraceListenerData(string name, string fileName)
            : base(name, typeof(XmlTraceListener), TraceOptions.None)
        {
            this.FileName = fileName;
        }

        /// <summary>
        /// Gets and sets the file name.
        /// </summary>
        [ConfigurationProperty(fileNameProperty, IsRequired = true)]
        public string FileName
        {
            get { return (string)base[fileNameProperty]; }
            set { base[fileNameProperty] = value; }
        }

        /// <summary>
        /// Returns a lambda expression that represents the creation of the trace listener described by this
        /// configuration object.
        /// </summary>
        /// <returns>A lambda expression to create a trace listener.</returns>
        protected override Expression<Func<TraceListener>> GetCreationExpression()
        {
            return () => new XmlTraceListener(this.FileName,
                                              Container.Resolved<ILoggingInstrumentationProvider>());
        }
    }
}
