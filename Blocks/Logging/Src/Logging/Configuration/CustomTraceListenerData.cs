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
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
    /// <summary>
    /// Configuration object for custom trace listenrs.
    /// </summary>
    [ResourceDescription(typeof(DesignResources), "CustomTraceListenerDataDescription")]
    [ResourceDisplayName(typeof(DesignResources), "CustomTraceListenerDataDisplayName")]
    [ViewModel(LoggingDesignTime.ViewModelTypeNames.CustomTraceListenerDataViewModel)]
    [TypePickingCommand(TitleResourceName = "CustomTraceListenerDataDisplayName", TitleResourceType = typeof(DesignResources), Replace = CommandReplacement.DefaultAddCommandReplacement)]
    public class CustomTraceListenerData
        : BasicCustomTraceListenerData
    {
        internal const string formatterNameProperty = "formatter";

        /// <summary>
        /// Initializes with default values.
        /// </summary>
        public CustomTraceListenerData()
            : base()
        {
            ListenerDataType = typeof(CustomTraceListenerData);
        }

        /// <summary>
        /// Initializes with name and provider type.
        /// </summary>
        public CustomTraceListenerData(string name, Type type, string initData)
            : base(name, type, initData)
        {
        }

        /// <summary>
        /// Initializes with name and provider type.
        /// </summary>
        public CustomTraceListenerData(string name, Type type, string initData, TraceOptions traceOutputOptions)
            : base(name, type, initData, traceOutputOptions)
        {
        }

        /// <summary>
        /// Initializes with name and fully qualified type name of the provider type.
        /// </summary>
        public CustomTraceListenerData(string name, string typeName, string initData, TraceOptions traceOutputOptions)
            : base(name, typeName, initData, traceOutputOptions)
        {
        }

        /// <summary>
        /// Gets or sets the name of the formatter. Can be <see langword="null"/>.
        /// </summary>
        [Reference(typeof(NameTypeConfigurationElementCollection<FormatterData, CustomFormatterData>), typeof(FormatterData))]
        [ResourceDescription(typeof(DesignResources), "CustomTraceListenerDataFormatterDescription")]
        [ResourceDisplayName(typeof(DesignResources), "CustomTraceListenerDataFormatterDisplayName")]
        public string Formatter
        {
            get { return (string)base[formatterNameProperty]; }
            set { base[formatterNameProperty] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [System.ComponentModel.Editor(CommonDesignTime.EditorTypes.TypeSelector, CommonDesignTime.EditorTypes.UITypeEditor)]
        [BaseType(typeof(CustomTraceListener), typeof(CustomTraceListenerData))]
        [System.ComponentModel.Browsable(true)]
        public override string TypeName
        {
            get
            {
                return base.TypeName;
            }
            set
            {
                base.TypeName = value;
            }
        }

        /// <summary>
        /// Creates the helper that enapsulates the configuration properties management.
        /// </summary>
        /// <returns></returns>
        protected override CustomProviderDataHelper<BasicCustomTraceListenerData> CreateHelper()
        {
            return new CustomTraceListenerDataHelper(this);
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
            var listener = base.CoreBuildTraceListener(settings);

            var customTraceListener = listener as CustomTraceListener;
            if (customTraceListener != null && !string.IsNullOrEmpty(this.Formatter))
            {
                customTraceListener.Formatter = this.BuildFormatterSafe(settings, this.Formatter);
            }

            return listener;
        }
    }
}
