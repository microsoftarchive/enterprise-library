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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
    /// <summary>
    /// Represents the configuration settings that describe a <see cref="FlatFileTraceListener"/>.
    /// </summary>
    [Assembler(typeof(FlatFileTraceListenerAssembler))]
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
        {
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
        [ConfigurationProperty(fileNameProperty, IsRequired = true)]
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
        public string Formatter
        {
            get { return (string)base[formatterNameProperty]; }
            set { base[formatterNameProperty] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<TypeRegistration> GetContainerConfigurationModel()
        {
            return new[] 
            { 
                new TypeRegistration<TraceListener>(
                    () => 
                        new FlatFileTraceListener(
                            this.FileName, 
                            this.Header, 
                            this.Footer, 
                            Container.ResolvedIfNotNull<ILogFormatter>(this.Formatter))
                        { 
                            Name = this.Name, 
                            TraceOutputOptions = this.TraceOutputOptions,
                            Filter = new EventTypeFilter(this.Filter)
                        }) 
                { Name = this.Name }
            };
        }
    }

    /// <summary>
    /// This type supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
    /// Represents the process to build a <see cref="FlatFileTraceListener"/> described by a <see cref="FlatFileTraceListenerData"/> configuration object.
    /// </summary>
    /// <remarks>This type is linked to the <see cref="FlatFileTraceListenerData"/> type and it is used by the <see cref="TraceListenerCustomFactory"/> 
    /// to build the specific <see cref="TraceListener"/> object represented by the configuration object.
    /// </remarks>
    public class FlatFileTraceListenerAssembler : TraceListenerAsssembler
    {
        /// <summary>
        /// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
        /// Builds a <see cref="FlatFileTraceListener"/> based on an instance of <see cref="FlatFileTraceListenerData"/>.
        /// </summary>
        /// <seealso cref="TraceListenerCustomFactory"/>
        /// <param name="context">The <see cref="IBuilderContext"/> that represents the current building process.</param>
        /// <param name="objectConfiguration">The configuration object that describes the object to build. Must be an instance of <see cref="FlatFileTraceListenerData"/>.</param>
        /// <param name="configurationSource">The source for configuration objects.</param>
        /// <param name="reflectionCache">The cache to use retrieving reflection information.</param>
        /// <returns>A fully initialized instance of <see cref="FlatFileTraceListener"/>.</returns>
        public override TraceListener Assemble(IBuilderContext context, TraceListenerData objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
        {
            FlatFileTraceListenerData castedObjectConfiguration
                = (FlatFileTraceListenerData)objectConfiguration;

            ILogFormatter formatter = GetFormatter(context, castedObjectConfiguration.Formatter, configurationSource, reflectionCache);

            TraceListener createdObject
                = new FlatFileTraceListener(
                    castedObjectConfiguration.FileName,
                    castedObjectConfiguration.Header,
                    castedObjectConfiguration.Footer,
                    formatter);

            return createdObject;
        }
    }
}
