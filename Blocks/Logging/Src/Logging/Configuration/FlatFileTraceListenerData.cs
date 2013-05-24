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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
    /// <summary>
    /// Represents the configuration settings that describe a <see cref="FlatFileTraceListener"/>.
    /// </summary>
    [ResourceDescription(typeof(DesignResources), "FlatFileTraceListenerDataDescription")]
    [ResourceDisplayName(typeof(DesignResources), "FlatFileTraceListenerDataDisplayName")]
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
        [ResourceDescription(typeof(DesignResources), "FlatFileTraceListenerDataFileNameDescription")]
        [ResourceDisplayName(typeof(DesignResources), "FlatFileTraceListenerDataFileNameDisplayName")]
        [System.ComponentModel.Editor(CommonDesignTime.EditorTypes.FilteredFilePath, CommonDesignTime.EditorTypes.UITypeEditor)]
        [FilteredFileNameEditor(typeof(DesignResources), "LogFileDialogFilter", CheckFileExists = false)]
        public string FileName
        {
            get { return (string)base[fileNameProperty]; }
            set { base[fileNameProperty] = value; }
        }

        /// <summary>
        /// Gets and sets the header.
        /// </summary>
        [ConfigurationProperty(headerProperty, IsRequired = false, DefaultValue = "----------------------------------------")]
        [ResourceDescription(typeof(DesignResources), "FlatFileTraceListenerDataHeaderDescription")]
        [ResourceDisplayName(typeof(DesignResources), "FlatFileTraceListenerDataHeaderDisplayName")]
        public string Header
        {
            get { return (string)base[headerProperty]; }
            set { base[headerProperty] = value; }
        }

        /// <summary>
        /// Gets and sets the footer.
        /// </summary>
        [ConfigurationProperty(footerProperty, IsRequired = false, DefaultValue = "----------------------------------------")]
        [ResourceDescription(typeof(DesignResources), "FlatFileTraceListenerDataFooterDescription")]
        [ResourceDisplayName(typeof(DesignResources), "FlatFileTraceListenerDataFooterDisplayName")]
        public string Footer
        {
            get { return (string)base[footerProperty]; }
            set { base[footerProperty] = value; }
        }

        /// <summary>
        /// Gets and sets the formatter name.
        /// </summary>
        [ConfigurationProperty(formatterNameProperty, IsRequired = false)]
        [Reference(typeof(NameTypeConfigurationElementCollection<FormatterData, CustomFormatterData>), typeof(FormatterData))]
        [ResourceDescription(typeof(DesignResources), "FlatFileTraceListenerDataFormatterDescription")]
        [ResourceDisplayName(typeof(DesignResources), "FlatFileTraceListenerDataFormatterDisplayName")]
        public string Formatter
        {
            get { return (string)base[formatterNameProperty]; }
            set { base[formatterNameProperty] = value; }
        }

        /// <summary>
        /// Builds the <see cref="TraceListener" /> object represented by this configuration object.
        /// </summary>
        /// <param name="settings">The logging configuration settings.</param>
        /// <returns>
        /// A <see cref="FlatFileTraceListener"/>.
        /// </returns>
        protected override TraceListener CoreBuildTraceListener(LoggingSettings settings)
        {
            var formatter = this.BuildFormatterSafe(settings, this.Formatter);

            return new FlatFileTraceListener(
                    this.FileName,
                    this.Header,
                    this.Footer,
                    formatter);
        }
    }
}
