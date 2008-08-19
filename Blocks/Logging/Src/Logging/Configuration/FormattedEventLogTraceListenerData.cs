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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
	/// <summary>
	/// Represents the configuration settings that describe a <see cref="FormattedEventLogTraceListener"/>.
	/// </summary>
	[Assembler(typeof(FormattedEventLogTraceListenerAssembler))]
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
		{
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
		[ConfigurationProperty(sourceProperty, IsRequired = true)]
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
	}

	/// <summary>
	/// This type supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
	/// Represents the process to build a <see cref="FormattedEventLogTraceListener"/> described by a <see cref="FormattedEventLogTraceListenerData"/> configuration object.
	/// </summary>
	/// <remarks>This type is linked to the <see cref="FormattedEventLogTraceListenerData"/> type and it is used by the <see cref="TraceListenerCustomFactory"/> 
	/// to build the specific <see cref="TraceListener"/> object represented by the configuration object.
	/// </remarks>
	public class FormattedEventLogTraceListenerAssembler : TraceListenerAsssembler
	{
		/// <summary>
		/// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// Builds a <see cref="FormattedEventLogTraceListener"/> based on an instance of <see cref="FormattedEventLogTraceListenerData"/>.
		/// </summary>
		/// <seealso cref="TraceListenerCustomFactory"/>
		/// <param name="context">The <see cref="IBuilderContext"/> that represents the current building process.</param>
		/// <param name="objectConfiguration">The configuration object that describes the object to build. Must be an instance of <see cref="FormattedEventLogTraceListenerData"/>.</param>
		/// <param name="configurationSource">The source for configuration objects.</param>
		/// <param name="reflectionCache">The cache to use retrieving reflection information.</param>
		/// <returns>A fully initialized instance of <see cref="FormattedEventLogTraceListener"/>.</returns>
		public override TraceListener Assemble(IBuilderContext context, TraceListenerData objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
		{
			FormattedEventLogTraceListenerData castedObjectConfiguration
				= (FormattedEventLogTraceListenerData)objectConfiguration;

			ILogFormatter formatter 
				= GetFormatter(context, castedObjectConfiguration.Formatter, configurationSource, reflectionCache);

			TraceListener createdObject
				= new FormattedEventLogTraceListener(
					castedObjectConfiguration.Source,
					castedObjectConfiguration.Log,
					castedObjectConfiguration.MachineName,
					formatter);

			return createdObject;
		}
	}
}
