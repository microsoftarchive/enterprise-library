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

using System.Configuration;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
    //TODOC : review missing comments in class
    /// <summary>
    /// Represents the configuration data for a <see cref="RollingFlatFileTraceListener"/>.
    /// </summary>	
    [ResourceDescription(typeof(DesignResources), "RollingFlatFileTraceListenerDataDescription")]
    [ResourceDisplayName(typeof(DesignResources), "RollingFlatFileTraceListenerDataDisplayName")]
    public class RollingFlatFileTraceListenerData : TraceListenerData
    {
        private const string FileNamePropertyName = "fileName";
        private const string footerProperty = "footer";
        private const string formatterNameProperty = "formatter";
        private const string headerProperty = "header";
        private const string RollFileExistsBehaviorPropertyName = "rollFileExistsBehavior";
        private const string RollIntervalPropertyName = "rollInterval";
        private const string RollSizeKBPropertyName = "rollSizeKB";
        private const string TimeStampPatternPropertyName = "timeStampPattern";
        private const string MaxArchivedFilesPropertyName = "maxArchivedFiles";


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
        [ConfigurationProperty(FileNamePropertyName, DefaultValue = "rolling.log")]
        [ResourceDescription(typeof(DesignResources), "RollingFlatFileTraceListenerDataFileNameDescription")]
        [ResourceDisplayName(typeof(DesignResources), "RollingFlatFileTraceListenerDataFileNameDisplayName")]
        [System.ComponentModel.Editor(CommonDesignTime.EditorTypes.FilteredFilePath, CommonDesignTime.EditorTypes.UITypeEditor)]
        [FilteredFileNameEditor(typeof(DesignResources), "LogFileDialogFilter", CheckFileExists = false)]
        public string FileName
        {
            get { return (string)this[FileNamePropertyName]; }
            set { this[FileNamePropertyName] = value; }
        }

        /// <summary>
        /// Gets and sets the footer.
        /// </summary>
        [ConfigurationProperty(footerProperty, IsRequired = false, DefaultValue = "----------------------------------------")]
        [ResourceDescription(typeof(DesignResources), "RollingFlatFileTraceListenerDataFooterDescription")]
        [ResourceDisplayName(typeof(DesignResources), "RollingFlatFileTraceListenerDataFooterDisplayName")]
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
        [ResourceDescription(typeof(DesignResources), "RollingFlatFileTraceListenerDataFormatterDescription")]
        [ResourceDisplayName(typeof(DesignResources), "RollingFlatFileTraceListenerDataFormatterDisplayName")]
        public string Formatter
        {
            get { return (string)base[formatterNameProperty]; }
            set { base[formatterNameProperty] = value; }
        }

        /// <summary>
        /// Gets and sets the header.
        /// </summary>
        [ConfigurationProperty(headerProperty, IsRequired = false, DefaultValue = "----------------------------------------")]
        [ResourceDescription(typeof(DesignResources), "RollingFlatFileTraceListenerDataHeaderDescription")]
        [ResourceDisplayName(typeof(DesignResources), "RollingFlatFileTraceListenerDataHeaderDisplayName")]
        public string Header
        {
            get { return (string)base[headerProperty]; }
            set { base[headerProperty] = value; }
        }

        /// <summary>
        /// Exists Behavior
        /// </summary>
        [ConfigurationProperty(RollFileExistsBehaviorPropertyName)]
        [ResourceDescription(typeof(DesignResources), "RollingFlatFileTraceListenerDataRollFileExistsBehaviorDescription")]
        [ResourceDisplayName(typeof(DesignResources), "RollingFlatFileTraceListenerDataRollFileExistsBehaviorDisplayName")]
        public RollFileExistsBehavior RollFileExistsBehavior
        {
            get { return (RollFileExistsBehavior)this[RollFileExistsBehaviorPropertyName]; }
            set { this[RollFileExistsBehaviorPropertyName] = value; }
        }

        /// <summary>
        /// Roll Intervall
        /// </summary>
        [ConfigurationProperty(RollIntervalPropertyName)]
        [ResourceDescription(typeof(DesignResources), "RollingFlatFileTraceListenerDataRollIntervalDescription")]
        [ResourceDisplayName(typeof(DesignResources), "RollingFlatFileTraceListenerDataRollIntervalDisplayName")]
        public RollInterval RollInterval
        {
            get { return (RollInterval)this[RollIntervalPropertyName]; }
            set { this[RollIntervalPropertyName] = value; }
        }

        /// <summary>
        /// Roll Size KB 
        /// </summary>
        [ConfigurationProperty(RollSizeKBPropertyName)]
        [ResourceDescription(typeof(DesignResources), "RollingFlatFileTraceListenerDataRollSizeKBDescription")]
        [ResourceDisplayName(typeof(DesignResources), "RollingFlatFileTraceListenerDataRollSizeKBDisplayName")]
        public int RollSizeKB
        {
            get { return (int)this[RollSizeKBPropertyName]; }
            set { this[RollSizeKBPropertyName] = value; }
        }

        /// <summary>
        /// Time stamp
        /// </summary>
        [ConfigurationProperty(TimeStampPatternPropertyName, DefaultValue = "yyyy-MM-dd")]
        [ResourceDescription(typeof(DesignResources), "RollingFlatFileTraceListenerDataTimeStampPatternDescription")]
        [ResourceDisplayName(typeof(DesignResources), "RollingFlatFileTraceListenerDataTimeStampPatternDisplayName")]
        public string TimeStampPattern
        {
            get { return (string)this[TimeStampPatternPropertyName]; }
            set { this[TimeStampPatternPropertyName] = value; }
        }

        /// <summary>
        /// Max rolled files
        /// </summary>
        [ConfigurationProperty(MaxArchivedFilesPropertyName)]
        [ResourceDescription(typeof(DesignResources), "RollingFlatFileTraceListenerDataMaxArchivedFilesDescription")]
        [ResourceDisplayName(typeof(DesignResources), "RollingFlatFileTraceListenerDataMaxArchivedFilesDisplayName")]
        public int MaxArchivedFiles
        {
            get { return (int)this[MaxArchivedFilesPropertyName]; }
            set { this[MaxArchivedFilesPropertyName] = value; }
        }

        /// <summary>
        /// Builds the <see cref="TraceListener" /> object represented by this configuration object.
        /// </summary>
        /// <param name="settings">The logging configuration settings.</param>
        /// <returns>
        /// A trace listener.
        /// </returns>
        protected override TraceListener CoreBuildTraceListener(LoggingSettings settings)
        {
            var formatter = this.BuildFormatterSafe(settings, this.Formatter);

            return new RollingFlatFileTraceListener(
                        this.FileName,
                        this.Header,
                        this.Footer,
                        formatter,
                        this.RollSizeKB,
                        this.TimeStampPattern,
                        this.RollFileExistsBehavior,
                        this.RollInterval,
                        this.MaxArchivedFiles);
        }
    }
}
