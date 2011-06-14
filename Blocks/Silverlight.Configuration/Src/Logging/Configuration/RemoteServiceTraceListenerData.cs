//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Silverlight Design-Time Configuration
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Silverlight.Configuration.Validation;
using Microsoft.Practices.EnterpriseLibrary.Silverlight.Configuration.Logging.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
    /// <summary>
    /// <para>Configuration data for an <c>RemoteServiceTraceListener</c>.</para>
    /// </summary>	
    [ResourceDescription(typeof(LoggingResources), "RemoteServiceTraceListenerDataDescription")]
    [ResourceDisplayName(typeof(LoggingResources), "RemoteServiceTraceListenerDataDisplayName")]
    [Browsable(true)]
    public class RemoteServiceTraceListenerData : TraceListenerData
    {
        private const string loggingServiceFactoryProperty = "loggingServiceFactory";
        private const string submitIntervalProperty = "submitInterval";
        private const string maxElementsInBufferProperty = "maxElementsInBuffer";
        private const string isolatedStorageBufferMaxSizeInKilobytesProperty = "isolatedStorageBufferMaxSizeInKilobytes";
        private const string sendImmediatelyProperty = "sendImmediately";
        
        private const int isolatedStorageBufferMaxSizeInKilobytesDefaultValue = 0;
        private const int maxElementsInBufferDefaultValue = 100;
        private const string submitIntervalDefaultValue = "00:01:00";

        /// <summary>
        /// <para>Initializes a new instance of <see cref="RemoteServiceTraceListenerData"/> class.</para>
        /// </summary>
        public RemoteServiceTraceListenerData()
            : base(typeof(RemoteServiceTraceListener))
        {
        }

        /// <summary>
        /// Initializes an instance of <see cref="RemoteServiceTraceListenerData"/> with a name and <see cref="TraceOptions"/> for 
        /// a TraceListenerType.
        /// </summary>
        /// <param name="name">The name for the instance.</param>
        public RemoteServiceTraceListenerData(string name)
            : base(typeof(RemoteServiceTraceListener))
        {
            Name = name;
        }

        /// <summary>
        /// Gets or sets the WCF endpoint configuration name. This configuration must be present in the ServiceReferences.ClientConfig file of the main application. 
        /// If you need more flexibility for configuring the logging service factory, you should provide an instance of ILoggingServiceFactory, 
        /// which should be set in XAML directly and cannot be set from the Enterprise Library Configuration Tool.
        /// </summary>
        [ConfigurationProperty(loggingServiceFactoryProperty, IsRequired = true)]
        [ResourceDescription(typeof(LoggingResources), "LoggingServiceFactoryDescription")]
        [ResourceDisplayName(typeof(LoggingResources), "LoggingServiceFactoryDisplayName")]
        [Browsable(true)]
        public string LoggingServiceFactory
        {
            get { return (string)this[loggingServiceFactoryProperty]; }
            set { this[loggingServiceFactoryProperty] = value; }
        }

        /// <summary>
        /// Gets or sets the time interval that will be used for submiting the log entries to the server.
        /// </summary>
        [ConfigurationProperty(submitIntervalProperty, DefaultValue = submitIntervalDefaultValue)]
        [BaseType(typeof(TimeSpan))]
        [ResourceDescription(typeof(LoggingResources), "SubmitIntervalDescription")]
        [ResourceDisplayName(typeof(LoggingResources), "SubmitIntervalDisplayName")]
        [Browsable(true)]
        public TimeSpan SubmitInterval
        {
            get { return TimeSpan.Parse(this[submitIntervalProperty].ToString()); }
            set { this[submitIntervalProperty] = value.ToString(); }
        }

        /// <summary>
        /// Gets or sets the maximum amount of elements that will be buffered in memory for when there are connectivity issues that prevent the listener from submiting the log entries.
        /// </summary>
        [ConfigurationProperty(maxElementsInBufferProperty,
            DefaultValue = maxElementsInBufferDefaultValue)]
        [ResourceDescription(typeof(LoggingResources), "MaxElementsInBufferDescription")]
        [ResourceDisplayName(typeof(LoggingResources), "MaxElementsInBufferDisplayName")]
        [Browsable(true)]
        [Validation(typeof(PositiveValidator))]
        public int MaxElementsInBuffer
        {
            get { return (int)this[maxElementsInBufferProperty]; }
            set { this[maxElementsInBufferProperty] = value; }
        }

        /// <summary>
        /// Gets or sets the maximum size in bytes to be used when storing entries into the isolated storage as a backup strategy.
        /// </summary>
        [ConfigurationProperty(isolatedStorageBufferMaxSizeInKilobytesProperty,
            DefaultValue = isolatedStorageBufferMaxSizeInKilobytesDefaultValue)]
        [ResourceDescription(typeof(LoggingResources), "IsolatedStorageBufferMaxSizeInKilobytesDescription")]
        [ResourceDisplayName(typeof(LoggingResources), "IsolatedStorageBufferMaxSizeInKilobytesDisplayName")]
        [Browsable(true)]
        [Validation(typeof(SizeOverSilverlightDefaultQuotaValidator))]
        [Validation(typeof(IsMoreThanFiveOrEqualZeroValidator))]
        public int IsolatedStorageBufferMaxSizeInKilobytes
        {
            get { return (int)this[isolatedStorageBufferMaxSizeInKilobytesProperty]; }
            set { this[isolatedStorageBufferMaxSizeInKilobytesProperty] = value; }
        }

        /// <summary>
        /// Gets or sets the maximum size in bytes to be used when storing entries into the isolated storage as a backup strategy.
        /// </summary>
        [ConfigurationProperty(sendImmediatelyProperty)]
        [ResourceDescription(typeof(LoggingResources), "SendImmediatelyDescription")]
        [ResourceDisplayName(typeof(LoggingResources), "SendImmediatelyDisplayName")]
        [Browsable(true)]
        public bool SendImmediately
        {
            get { return (bool)this[sendImmediatelyProperty]; }
            set { this[sendImmediatelyProperty] = value; }
        }

        /// <summary>
        /// Overridden in order to hide from the configuration designtime.
        /// </summary>
        [Browsable(false)]
        public new TraceOptions TraceOutputOptions
        {
            get;
            set;
        }

        /// <summary>
        /// Overridden in order to hide from the configuration designtime.
        /// </summary>
        [Browsable(false)]
        public new SourceLevels Filter
        {
            get;
            set;
        }
    }
}
