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
using System.Configuration;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation;
using System.Linq.Expressions;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
    /// <summary>
    /// Represents the configuration settings that describe a <see cref="FlatFileTraceListener"/>.
    /// </summary>
    public class FlatFileTraceListenerData : TraceListenerData
    {
        private const string fileNameProperty = "fileName";
        private const string headerProperty = "header";
        private const string footerProperty = "footer";
        private const string formatterNameProperty = "formatter";

        /// <summary>
        /// Initializes a <see cref="FlatFileTraceListenerData"/>.
        /// </summary>
        public FlatFileTraceListenerData()
            : base(typeof(FlatFileTraceListener))
        {
            ListenerDataType = typeof(FlatFileTraceListenerData);
        }

        /// <summary>
        /// Initializes a <see cref="FlatFileTraceListenerData"/> with a filename and a formatter name.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        /// <param name="formatterName">The formatter name.</param>
        public FlatFileTraceListenerData(string fileName, string formatterName)
            : this("unnamed", fileName, formatterName)
        {
        }

        /// <summary>
        /// Initializes a named instance of <see cref="FlatFileTraceListenerData"/> with 
        /// name, file name and formatter name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="fileName">The file name.</param>
        /// <param name="formatterName">The formatter name.</param>
        public FlatFileTraceListenerData(string name, string fileName, string formatterName)
            : this(name, typeof(FlatFileTraceListener), fileName, formatterName)
        {
        }

        /// <summary>
        /// Initializes a named instance of <see cref="FlatFileTraceListenerData"/>.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="fileName">The file name.</param>
        /// <param name="header">The header.</param>
        /// <param name="footer">The footer.</param>
        /// <param name="formatterName">The formatter name.</param>
        public FlatFileTraceListenerData(string name, string fileName, string header, string footer, string formatterName)
            : this(name, fileName, header, footer, formatterName, TraceOptions.None)
        {
        }

        /// <summary>
        /// Initializes a named instance of <see cref="FlatFileTraceListenerData"/>.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="fileName">The file name.</param>
        /// <param name="header">The header.</param>
        /// <param name="footer">The footer.</param>
        /// <param name="formatterName">The formatter name.</param>
        /// <param name="traceOutputOptions">The trace options.</param>
        public FlatFileTraceListenerData(string name, string fileName, string header, string footer, string formatterName,
                        TraceOptions traceOutputOptions)
            : this(name, typeof(FlatFileTraceListener), fileName, formatterName, traceOutputOptions)
        {
            this.Header = header;
            this.Footer = footer;
        }

        /// <summary>
        /// Initializes a named instance of <see cref="FlatFileTraceListenerData"/>.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="listenerType">The type of the represented <see cref="System.Diagnostics.TraceListener"/></param>
        /// <param name="fileName">The file name.</param>
        /// <param name="formatterName">The formatter name.</param>
        public FlatFileTraceListenerData(string name, Type listenerType, string fileName, string formatterName)
            : this(name, listenerType, fileName, formatterName, TraceOptions.None)
        {
        }

        /// <summary>
        /// Initializes a named instance of <see cref="FlatFileTraceListenerData"/>.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="listenerType">The type of the represented <see cref="System.Diagnostics.TraceListener"/></param>
        /// <param name="fileName">The file name.</param>
        /// <param name="formatterName">The formatter name.</param>
        /// <param name="traceOutputOptions">The trace options.</param>
        public FlatFileTraceListenerData(string name, Type listenerType, string fileName, string formatterName, TraceOptions traceOutputOptions)
            : base(name, listenerType, traceOutputOptions)
        {
            this.FileName = fileName;
            this.Formatter = formatterName;
        }

        /// <summary>
        /// Gets and sets the file name.
        /// </summary>
        [ConfigurationProperty(fileNameProperty, IsRequired = true, DefaultValue = "trace.log")]
        public string FileName
        {
            get { return (string)base[fileNameProperty]; }
            set { base[fileNameProperty] = value; }
        }

        /// <summary>
        /// Gets and sets the header.
        /// </summary>
        [ConfigurationProperty(headerProperty, IsRequired = false)]
        public string Header
        {
            get { return (string)base[headerProperty]; }
            set { base[headerProperty] = value; }
        }

        /// <summary>
        /// Gets and sets the footer.
        /// </summary>
        [ConfigurationProperty(footerProperty, IsRequired = false)]
        public string Footer
        {
            get { return (string)base[footerProperty]; }
            set { base[footerProperty] = value; }
        }

        /// <summary>
        /// Gets and sets the formatter name.
        /// </summary>
        [ConfigurationProperty(formatterNameProperty, IsRequired = false)]
        [Reference(typeof(LoggingSettings), typeof(FormatterData))]
        public string Formatter
        {
            get { return (string)base[formatterNameProperty]; }
            set { base[formatterNameProperty] = value; }
        }

        /// <summary>
        /// Returns a lambda expression that represents the creation of the trace listener described by this
        /// configuration object.
        /// </summary>
        /// <returns>A lambda expression to create a trace listener.</returns>
        protected override Expression<Func<TraceListener>> GetCreationExpression()
        {
            return () =>
                new FlatFileTraceListener(
                    this.FileName,
                    this.Header,
                    this.Footer,
                    Container.ResolvedIfNotNull<ILogFormatter>(this.Formatter));
        }
    }
}
