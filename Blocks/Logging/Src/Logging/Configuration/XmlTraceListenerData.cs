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
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
	/// <summary>
	/// Represents the configuration settings that describe a <see cref="XmlTraceListener"/>.
	/// </summary>
	[Assembler(typeof(XmlTraceListenerAssembler))]
	public class XmlTraceListenerData : TraceListenerData
	{
		private const string fileNameProperty = "fileName";

		/// <summary>
		/// Initializes a <see cref="XmlTraceListenerData"/>.
		/// </summary>
		public XmlTraceListenerData()
		{
		}

		/// <summary>
		/// Initializes a <see cref="XmlTraceListenerData"/> with a filename and a formatter name.
		/// </summary>
		/// <param name="fileName">The file name.</param>
		/// <param name="name">The name for the configuration object.</param>
		public XmlTraceListenerData(string name, string fileName)
			: base(name, typeof(XmlTraceListener), TraceOptions.None)
		{
			this.FileName = fileName;
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
	}

	/// <summary>
	/// This type supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
	/// Represents the process to build a <see cref="XmlTraceListener"/> described by a <see cref="XmlTraceListenerData"/> configuration object.
	/// </summary>
	/// <remarks>This type is linked to the <see cref="XmlTraceListenerData"/> type and it is used by the <see cref="TraceListenerCustomFactory"/> 
	/// to build the specific <see cref="TraceListener"/> object represented by the configuration object.
	/// </remarks>
	public class XmlTraceListenerAssembler : TraceListenerAsssembler
	{
		/// <summary>
		/// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// Builds a <see cref="XmlTraceListener"/> based on an instance of <see cref="XmlTraceListenerData"/>.
		/// </summary>
		/// <seealso cref="TraceListenerCustomFactory"/>
		/// <param name="context">The <see cref="IBuilderContext"/> that represents the current building process.</param>
		/// <param name="objectConfiguration">The configuration object that describes the object to build. Must be an instance of <see cref="XmlTraceListenerData"/>.</param>
		/// <param name="configurationSource">The source for configuration objects.</param>
		/// <param name="reflectionCache">The cache to use retrieving reflection information.</param>
		/// <returns>A fully initialized instance of <see cref="XmlTraceListener"/>.</returns>
		public override TraceListener Assemble(IBuilderContext context, TraceListenerData objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
		{
			XmlTraceListenerData castedObjectConfiguration
				= (XmlTraceListenerData)objectConfiguration;

			XmlTraceListener createdObject
				= new XmlTraceListener(castedObjectConfiguration.FileName);

			return createdObject;
		}
	}
}
