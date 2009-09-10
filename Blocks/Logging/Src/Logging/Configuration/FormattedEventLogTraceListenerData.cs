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

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
    /// <summary>
    /// Represents the configuration settings that describe a <see cref="FormattedEventLogTraceListener"/>.
    /// </summary>
    public class FormattedEventLogTraceListenerData : TraceListenerData
    {
        private const string sourceProperty = "source";
        private const string formatterNameProperty = "formatter";
        private const string logNameProperty = "log";
        private const string machineNameProperty = "machineName";

        /// <summary>
        /// Initializes a <see cref="FormattedEventLogTraceListenerData"/>.
        /// </summary>
        public FormattedEventLogTraceListenerData()
            : base(typeof(FormattedEventLogTraceListener))
        {
            ListenerDataType = typeof(FormattedEventLogTraceListenerData);
        }

        /// <summary>
        /// Initializes a <see cref="FormattedEventLogTraceListenerData"/> with a source name and a formatter name.
        /// </summary>
        /// <param name="source">The event log source name.</param>
        /// <param name="formatterName">The formatter name.</param>
        public FormattedEventLogTraceListenerData(string source, string formatterName)
            : this("unnamed", source, formatterName)
        {
        }

        /// <summary>
        /// Initializes a named instance of <see cref="FormattedEventLogTraceListenerData"/> with name, source name 
        /// and formatter name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="source">The event log source name.</param>
        /// <param name="formatterName">The formatter name.</param>
        public FormattedEventLogTraceListenerData(string name, string source, string formatterName)
            : this(name, source, FormattedEventLogTraceListener.DefaultLogName, FormattedEventLogTraceListener.DefaultMachineName, formatterName)
        {
        }

        /// <summary>
        /// Initializes a named instance of <see cref="FormattedEventLogTraceListenerData"/> with name, source name 
        /// log name, machine name, and formatter name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="source">The event log source name.</param>
        /// <param name="logName">The log name.</param>
        /// <param name="machineName">The machine name.</param>
        /// <param name="formatterName">The formatter name.</param>
        public FormattedEventLogTraceListenerData(string name, string source, string logName, string machineName, string formatterName)
            : this(name, source, logName, machineName, formatterName, TraceOptions.None)
        {
        }

        /// <summary>
        /// Initializes a named instance of <see cref="FormattedEventLogTraceListenerData"/> with name, source name 
        /// log name, machine name, formatter name, and <see cref="TraceOptions"/>.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="source">The event log source name.</param>
        /// <param name="logName">The log name.</param>
        /// <param name="machineName">The machine name.</param>
        /// <param name="formatterName">The formatter name.</param>
        /// <param name="traceOutputOptions">The output options.</param>
        public FormattedEventLogTraceListenerData(string name, string source, string logName,
                    string machineName, string formatterName, TraceOptions traceOutputOptions)
            : base(name, typeof(FormattedEventLogTraceListener), traceOutputOptions)
        {
            this.Source = source;
            this.Log = logName;
            this.MachineName = machineName;
            this.Formatter = formatterName;
        }

        /// <summary>
        /// Gets or sets the event log source of the <see cref="FormattedEventLogTraceListenerData"/>.
        /// </summary>
        [ConfigurationProperty(sourceProperty, IsRequired = true, DefaultValue = "Enterprise Library Logging")]
        public string Source
        {
            get { return (string)base[sourceProperty]; }
            set { base[sourceProperty] = value; }
        }

        /// <summary>
        /// Gets or sets the name of the formatter for the <see cref="FormattedEventLogTraceListenerData"/>.
        /// </summary>
        [ConfigurationProperty(formatterNameProperty, IsRequired = false)]
        public string Formatter
        {
            get { return (string)base[formatterNameProperty]; }
            set { base[formatterNameProperty] = value; }
        }

        /// <summary>
        /// Gets or sets the name of the log for the <see cref="FormattedEventLogTraceListenerData"/>.
        /// </summary>
        [ConfigurationProperty(logNameProperty, IsRequired = false, DefaultValue = FormattedEventLogTraceListener.DefaultLogName)]
        public string Log
        {
            get { return (string)base[logNameProperty]; }
            set { base[logNameProperty] = value; }
        }

        /// <summary>
        /// Gets or sets the name of the machine for the <see cref="FormattedEventLogTraceListenerData"/>.
        /// </summary>
        [ConfigurationProperty(machineNameProperty, IsRequired = false, DefaultValue = FormattedEventLogTraceListener.DefaultMachineName)]
        public string MachineName
        {
            get { return (string)base[machineNameProperty]; }
            set { base[machineNameProperty] = value; }
        }

        /// <summary>
        /// Returns a lambda expression that represents the creation of the trace listener described by this
        /// configuration object.
        /// </summary>
        /// <returns>A lambda expression to create a trace listener.</returns>
        protected override Expression<Func<TraceListener>> GetCreationExpression()
        {
            return () =>
                new FormattedEventLogTraceListener(
                    this.Source,
                    this.Log,
                    this.MachineName,
                    Container.ResolvedIfNotNull<ILogFormatter>(this.Formatter));
        }
    }
}
