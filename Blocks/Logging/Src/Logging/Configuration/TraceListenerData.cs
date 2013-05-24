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
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Properties;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
    /// <summary>
    /// Represents the configuration for a <see cref="TraceListener"/>.
    /// </summary>
    /// <remarks>
    /// Since trace listeners are not under our control, the building mechanism can't rely 
    /// on annotations to the trace listeners to determine the concrete <see cref="TraceListenerData"/> subtype 
    /// when deserializing. Because of this, the schema for <see cref="TraceListenerData"/> includes the actual 
    /// type of the instance to build.
    /// </remarks>
    public class TraceListenerData : NameTypeConfigurationElement
    {
        private AssemblyQualifiedTypeNameConverter typeConverter = new AssemblyQualifiedTypeNameConverter();

        internal const string TraceListenerNameSuffix = "\u200Cimplementation";

        /// <summary>
        /// Name of the property that holds the type for a <see cref="TraceListenerData"/>.
        /// </summary>
        /// <remarks>
        /// This property will hold the type of the object it holds it. However, it's used during the 
        /// deserialization process when the actual type of configuration element to create has to be determined.
        /// </remarks>
        protected internal const string listenerDataTypeProperty = "listenerDataType";

        /// <summary>
        /// Name of the property that holds the <see cref="TraceOptions"/> of a <see cref="TraceListenerData"/>.
        /// </summary>
        protected internal const string traceOutputOptionsProperty = "traceOutputOptions";

        /// <summary>
        /// Name of the property that holds the Filter of a <see cref="TraceListenerData"/>
        /// </summary>
        protected internal const string filterProperty = "filter";

        /// <summary>
        /// Name of the property that holds the asynchronous flag of a <see cref="TraceListenerData"/>
        /// </summary>
        protected internal const string asynchronousProperty = "asynchronous";

        /// <summary>
        /// Name of the property that holds the asynchronous dispose timeout of a <see cref="TraceListenerData"/>
        /// </summary>
        protected internal const string asynchronousDisposeTimeoutProperty = "asynchronousDisposeTimeout";

        /// <summary>
        /// Name of the property that holds the asynchronous buffer size of a <see cref="TraceListenerData"/>
        /// </summary>
        protected internal const string asynchronousBufferSizeProperty = "asynchronousBufferSize";

        /// <summary>
        /// Name of the property that holds the asynchronous max degree of parallelism of a <see cref="TraceListenerData"/>
        /// </summary>
        protected internal const string asynchronousMaxDegreeOfParallelismProperty = "asynchronousDegreeOfParallelism";

        private static IDictionary<string, string> emptyAttributes = new Dictionary<string, string>(0);

        /// <summary>
        /// Initializes an instance of the <see cref="TraceListenerData"/> class.
        /// </summary>
        public TraceListenerData()
        {
        }

        /// <summary>
        /// Initializes an instance of <see cref="TraceListenerData"/> for the given <paramref name="traceListenerType"/>.
        /// </summary>
        /// <param name="traceListenerType">Type of trace listener this element represents.</param>
        public TraceListenerData(Type traceListenerType)
            : base(null, traceListenerType)
        {
            this.ListenerDataType = this.GetType();
        }

        /// <summary>
        /// Initializes an instance of <see cref="TraceListenerData"/> with a name and <see cref="TraceOptions"/> for 
        /// a TraceListenerType.
        /// </summary>
        /// <param name="name">The name for the instance.</param>
        /// <param name="traceListenerType">The trace listener type.</param>
        /// <param name="traceOutputOptions">The trace options.</param>
        protected TraceListenerData(string name, Type traceListenerType, TraceOptions traceOutputOptions)
            : base(name, traceListenerType)
        {
            this.ListenerDataType = this.GetType();
            this.TraceOutputOptions = traceOutputOptions;
        }

        /// <summary>
        /// Initializes an instance of <see cref="TraceListenerData"/> with a name, a <see cref="TraceOptions"/> for 
        /// a TraceListenerType and a <see cref="SourceLevels"/> for a Filter.
        /// </summary>
        /// <param name="name">The name for the instance.</param>
        /// <param name="traceListenerType">The trace listener type.</param>
        /// <param name="traceOutputOptions">The trace options.</param>
        /// <param name="filter">The filter.</param>
        protected TraceListenerData(string name, Type traceListenerType, TraceOptions traceOutputOptions, SourceLevels filter)
            : base(name, traceListenerType)
        {
            this.ListenerDataType = this.GetType();
            this.TraceOutputOptions = traceOutputOptions;
            this.Filter = filter;
        }

        /// <summary>
        /// Gets or sets the type of the actual <see cref="TraceListenerData"/> type.
        /// </summary>
        /// <remarks>
        /// Should match the this.GetType().
        /// </remarks>
        public Type ListenerDataType
        {
            get { return (Type)typeConverter.ConvertFrom(ListenerDataTypeName); }
            set { ListenerDataTypeName = typeConverter.ConvertToString(value); }
        }

        /// <summary>
        /// Gets or sets the fully qualified name of the actual <see cref="TraceListenerData"/> type.
        /// </summary>
        /// <value>
        /// the fully qualified name of the actual <see cref="TraceListenerData"/> type.
        /// </value>
        [ConfigurationProperty(listenerDataTypeProperty, IsRequired = true)]
        [Browsable(false)]
        public string ListenerDataTypeName
        {
            get { return (string)this[listenerDataTypeProperty]; }
            set { this[listenerDataTypeProperty] = value; }
        }

        /// <summary>
        /// Gets or sets the <see cref="TraceOptions"/> for the represented <see cref="TraceListener"/>.
        /// </summary>
        [ConfigurationProperty(traceOutputOptionsProperty, IsRequired = false, DefaultValue = TraceOptions.None)]
        [ResourceDescription(typeof(DesignResources), "TraceListenerDataTraceOutputOptionsDescription")]
        [ResourceDisplayName(typeof(DesignResources), "TraceListenerDataTraceOutputOptionsDisplayName")]
        [Editor(CommonDesignTime.EditorTypes.Flags, CommonDesignTime.EditorTypes.FrameworkElement)]
        public TraceOptions TraceOutputOptions
        {
            get
            {
                return (TraceOptions)this[traceOutputOptionsProperty];
            }
            set
            {
                this[traceOutputOptionsProperty] = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="Filter"/> for the represented <see cref="TraceListener"/>
        /// </summary>
        [ConfigurationProperty(filterProperty, IsRequired = false, DefaultValue = SourceLevels.All)]
        [ResourceDescription(typeof(DesignResources), "TraceListenerDataFilterDescription")]
        [ResourceDisplayName(typeof(DesignResources), "TraceListenerDataFilterDisplayName")]
        [ViewModel(LoggingDesignTime.ViewModelTypeNames.SourceLevelsProperty)]
        public SourceLevels Filter
        {
            get { return (SourceLevels)this[filterProperty]; }
            set { this[filterProperty] = value; }
        }

        /// <summary>
        /// Gets or sets the asynchronous flag for the represented <see cref="TraceListener"/>
        /// </summary>
        [ConfigurationProperty(asynchronousProperty, IsRequired = false, DefaultValue = false)]
        [ResourceDescription(typeof(DesignResources), "TraceListenerDataAsynchronousDescription")]
        [ResourceDisplayName(typeof(DesignResources), "TraceListenerDataAsynchronousDisplayName")]
        public bool Asynchronous
        {
            get { return (bool)this[asynchronousProperty]; }
            set { this[asynchronousProperty] = value; }
        }

        /// <summary>
        /// Gets or sets the asynchronous dispose timeout for the represented <see cref="TraceListener"/>
        /// </summary>
        [ConfigurationProperty(asynchronousDisposeTimeoutProperty, IsRequired = false, DefaultValue = TimeSpanOrInfiniteConverter.Infinite)]
        [NonNegativeOrInfiniteTimeSpanValidator]
        [TypeConverter(typeof(TimeSpanOrInfiniteConverter))]
        [ResourceDescription(typeof(DesignResources), "TraceListenerDataAsynchronousDisposeTimeoutDescription")]
        [ResourceDisplayName(typeof(DesignResources), "TraceListenerDataAsynchronousDisposeTimeoutDisplayName")]
        [ViewModel(LoggingDesignTime.ViewModelTypeNames.TimeSpanElementConfigurationProperty)]
        public TimeSpan AsynchronousDisposeTimeout
        {
            get { return (TimeSpan)this[asynchronousDisposeTimeoutProperty]; }
            set { this[asynchronousDisposeTimeoutProperty] = value; }
        }

        /// <summary>
        /// Gets or sets the asynchronous buffer size for the represented <see cref="TraceListener"/>
        /// </summary>
        [ConfigurationProperty(asynchronousBufferSizeProperty, IsRequired = false, DefaultValue = AsynchronousTraceListenerWrapper.DefaultBufferSize)]
        [ResourceDescription(typeof(DesignResources), "TraceListenerDataAsynchronousBufferSizeDescription")]
        [ResourceDisplayName(typeof(DesignResources), "TraceListenerDataAsynchronousBufferSizeDisplayName")]
        public int AsynchronousBufferSize
        {
            get { return (int)this[asynchronousBufferSizeProperty]; }
            set { this[asynchronousBufferSizeProperty] = value; }
        }

        /// <summary>
        /// Gets or sets the asynchronous max degree of parallelism for the represented <see cref="TraceListener"/>
        /// </summary>
        [ConfigurationProperty(asynchronousMaxDegreeOfParallelismProperty, IsRequired = false, DefaultValue = null)]
        [ResourceDescription(typeof(DesignResources), "TraceListenerDataAsynchronousMaxDegreeOfParallelismDescription")]
        [ResourceDisplayName(typeof(DesignResources), "TraceListenerDataAsynchronousMaxDegreeOfParallelismDisplayName")]
        public int? AsynchronousMaxDegreeOfParallelism
        {
            get { return (int?)this[asynchronousMaxDegreeOfParallelismProperty]; }
            set { this[asynchronousMaxDegreeOfParallelismProperty] = value; }
        }

        /// <summary>
        /// Builds the <see cref="TraceListener" /> object represented by this configuration object.
        /// </summary>
        /// <param name="settings">The logging configuration settings.</param>
        /// <returns>
        /// A trace listener.
        /// </returns>
        public TraceListener BuildTraceListener(LoggingSettings settings)
        {
            Guard.ArgumentNotNull(settings, "settings");

            var listener = this.CoreBuildTraceListener(settings);

            listener.Name = this.Name;
            listener.TraceOutputOptions = this.TraceOutputOptions;

            if (this.Asynchronous)
            {
                listener = 
                    new AsynchronousTraceListenerWrapper(
                        listener, 
                        ownsWrappedTraceListener: true, 
                        bufferSize: this.AsynchronousBufferSize, 
                        maxDegreeOfParallelism: this.AsynchronousMaxDegreeOfParallelism, 
                        disposeTimeout: this.AsynchronousDisposeTimeout);
            }

            if (this.Filter != SourceLevels.All)
            {
                listener.Filter = new EventTypeFilter(this.Filter);
            }

            return listener;
        }

        /// <summary>
        /// Builds the <see cref="TraceListener" /> object represented by this configuration object.
        /// </summary>
        /// <param name="settings">The logging configuration settings.</param>
        /// <returns>
        /// A trace listener.
        /// </returns>
        protected virtual TraceListener CoreBuildTraceListener(LoggingSettings settings)
        {
            throw new NotImplementedException(Resources.ExceptionMethodMustBeImplementedBySubclasses);
        }

        /// <summary>
        /// Builds the log formatter represented by the name <paramref name="formatterName"/> in <paramref name="settings"/>.
        /// </summary>
        /// <param name="settings">The logging settings.</param>
        /// <param name="formatterName">The formatter name, or a null or empty string.</param>
        /// <returns>A new formatter if <paramref name="formatterName"/> is not null or empty; otherwise, null.</returns>
        protected ILogFormatter BuildFormatterSafe(LoggingSettings settings, string formatterName)
        {
            if (string.IsNullOrEmpty(formatterName))
            {
                return null;
            }

            var formatterData = settings.Formatters.Get(formatterName);
            if (formatterData == null)
            {
                throw new InvalidOperationException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Resources.ExceptionFormatterNotDefined,
                        formatterName,
                        this.ElementInformation.Source,
                        this.ElementInformation.LineNumber));
            }

            return formatterData.BuildFormatter();
        }
    }
}
