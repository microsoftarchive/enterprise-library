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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
    //TODOC : review missing comments in class
    /// <summary>
    /// Represents the configuration data for a <see cref="RollingFlatFileTraceListener"/>.
    /// </summary>	
    public class RollingFlatFileTraceListenerData : TraceListenerData
    {
        const string FileNamePropertyName = "fileName";
        const string footerProperty = "footer";
        const string formatterNameProperty = "formatter";
        const string headerProperty = "header";
        const string RollFileExistsBehaviorPropertyName = "rollFileExistsBehavior";
        const string RollIntervalPropertyName = "rollInterval";
        const string RollSizeKBPropertyName = "rollSizeKB";
        const string TimeStampPatternPropertyName = "timeStampPattern";
        const string MaxArchivedFilesPropertyName = "maxArchivedFiles";


        /// <summary>
        /// Initializes a new instance of the <see cref="TraceListenerData"/> class.
        /// </summary>
        public RollingFlatFileTraceListenerData()
            : base(typeof(RollingFlatFileTraceListener))
        {
            ListenerDataType = typeof(RollingFlatFileTraceListenerData);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RollingFlatFileTraceListenerData"/> class.
        /// </summary>
        /// <param name="name">The name for the configuration object.</param>
        /// <param name="traceOutputOptions">The trace options.</param>
        /// <param name="fileName"></param>
        /// <param name="footer"></param>
        /// <param name="header"></param>
        /// <param name="rollSizeKB"></param>
        /// <param name="timeStampPattern"></param>
        /// <param name="rollFileExistsBehavior"></param>
        /// <param name="rollInterval"></param>
        /// <param name="formatter"></param>
        public RollingFlatFileTraceListenerData(string name,
                                                string fileName,
                                                string header,
                                                string footer,
                                                int rollSizeKB,
                                                string timeStampPattern,
                                                RollFileExistsBehavior rollFileExistsBehavior,
                                                RollInterval rollInterval,
                                                TraceOptions traceOutputOptions,
                                                string formatter)
            : base(name, typeof(RollingFlatFileTraceListener), traceOutputOptions)
        {
            FileName = fileName;
            Header = header;
            Footer = footer;
            RollSizeKB = rollSizeKB;
            RollFileExistsBehavior = rollFileExistsBehavior;
            RollInterval = rollInterval;
            TimeStampPattern = timeStampPattern;
            Formatter = formatter;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RollingFlatFileTraceListenerData"/> class.
        /// </summary>
        /// <param name="name">The name for the configuration object.</param>
        /// <param name="traceOutputOptions">The trace options.</param>
        /// <param name="filter">The filter to apply.</param>
        /// <param name="fileName"></param>
        /// <param name="footer"></param>
        /// <param name="header"></param>
        /// <param name="rollSizeKB"></param>
        /// <param name="timeStampPattern"></param>
        /// <param name="rollFileExistsBehavior"></param>
        /// <param name="rollInterval"></param>
        /// <param name="formatter"></param>
        public RollingFlatFileTraceListenerData(string name,
                                                string fileName,
                                                string header,
                                                string footer,
                                                int rollSizeKB,
                                                string timeStampPattern,
                                                RollFileExistsBehavior rollFileExistsBehavior,
                                                RollInterval rollInterval,
                                                TraceOptions traceOutputOptions,
                                                string formatter,
                                                SourceLevels filter)
            : base(name, typeof(RollingFlatFileTraceListener), traceOutputOptions, filter)
        {
            FileName = fileName;
            Header = header;
            Footer = footer;
            RollSizeKB = rollSizeKB;
            RollFileExistsBehavior = rollFileExistsBehavior;
            RollInterval = rollInterval;
            TimeStampPattern = timeStampPattern;
            Formatter = formatter;
        }

        /// <summary>
        /// FileName
        /// </summary>
        [ConfigurationProperty(FileNamePropertyName, DefaultValue="rolling.log")]
        public string FileName
        {
            get { return (string)this[FileNamePropertyName]; }
            set { this[FileNamePropertyName] = value; }
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
        /// Gets and sets the header.
        /// </summary>
        [ConfigurationProperty(headerProperty, IsRequired = false)]
        public string Header
        {
            get { return (string)base[headerProperty]; }
            set { base[headerProperty] = value; }
        }

        /// <summary>
        /// Exists Behavior
        /// </summary>
        [ConfigurationProperty(RollFileExistsBehaviorPropertyName)]
        public RollFileExistsBehavior RollFileExistsBehavior
        {
            get { return (RollFileExistsBehavior)this[RollFileExistsBehaviorPropertyName]; }
            set { this[RollFileExistsBehaviorPropertyName] = value; }
        }

        /// <summary>
        /// Roll Intervall
        /// </summary>
        [ConfigurationProperty(RollIntervalPropertyName)]
        public RollInterval RollInterval
        {
            get { return (RollInterval)this[RollIntervalPropertyName]; }
            set { this[RollIntervalPropertyName] = value; }
        }

        /// <summary>
        /// Roll Size KB 
        /// </summary>
        [ConfigurationProperty(RollSizeKBPropertyName)]
        public int RollSizeKB
        {
            get { return (int)this[RollSizeKBPropertyName]; }
            set { this[RollSizeKBPropertyName] = value; }
        }

        /// <summary>
        /// Time stamp
        /// </summary>
        [ConfigurationProperty(TimeStampPatternPropertyName)]
        public string TimeStampPattern
        {
            get { return (string)this[TimeStampPatternPropertyName]; }
            set { this[TimeStampPatternPropertyName] = value; }
        }

        /// <summary>
        /// Max rolled files
        /// </summary>
        [ConfigurationProperty(MaxArchivedFilesPropertyName)]
        public int MaxArchivedFiles
        {
            get { return (int)this[MaxArchivedFilesPropertyName]; }
            set { this[MaxArchivedFilesPropertyName] = value; }
        }

        /// <summary>
        /// Returns a lambda expression that represents the creation of the trace listener described by this
        /// configuration object.
        /// </summary>
        /// <returns>A lambda expression to create a trace listener.</returns>
        protected override Expression<Func<TraceListener>> GetCreationExpression()
        {
            return
                () =>
                    new RollingFlatFileTraceListener(
                        this.FileName,
                        this.Header,
                        this.Footer,
                        Container.ResolvedIfNotNull<ILogFormatter>(this.Formatter),
                        this.RollSizeKB,
                        this.TimeStampPattern,
                        this.RollFileExistsBehavior,
                        this.RollInterval,
                        this.MaxArchivedFiles);
        }
    }
}
