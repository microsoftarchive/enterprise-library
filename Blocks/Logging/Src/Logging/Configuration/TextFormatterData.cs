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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
	/// <summary>
	/// Represents the configuration settings for a <see cref="TextFormatter"/>.
	/// </summary>
	[Assembler(typeof(TextFormatterAssembler))]
	public class TextFormatterData : FormatterData
	{
		private const string templateProperty = "template";

		/// <summary>
		/// Initializes a new instance of the <see cref="TextFormatterData"/> class with default values.
		/// </summary>
		public TextFormatterData()
		{
		}

		/// <summary>
		/// Initialize a new instance of the <see cref="TextFormatterData"/> class with a template.
		/// </summary>
		/// <param name="templateData">
		/// Template containing tokens to replace.
		/// </param>
		public TextFormatterData(string templateData)
			: this("unnamed", templateData)
		{
		}

		/// <summary>
		/// Initialize a new instance of the <see cref="TextFormatterData"/> class with a name and template.
		/// </summary>
		/// <param name="name">
		/// The name of the formatter.
		/// </param>
		/// <param name="templateData">
		/// Template containing tokens to replace.
		/// </param>
		public TextFormatterData(string name, string templateData)
			: this(name, typeof(TextFormatter), templateData)
		{
		}

		/// <summary>
		/// Initialize a new instance of the <see cref="TextFormatterData"/> class with a name and template.
		/// </summary>
		/// <param name="name">
		/// The name of the formatter.
		/// </param>
		/// <param name="formatterType">
		/// The type of the formatter.
		/// </param>
		/// <param name="templateData">
		/// Template containing tokens to replace.
		/// </param>
		public TextFormatterData(string name, Type formatterType, string templateData)
			: base(name, formatterType)
		{
			this.Template = templateData;
		}


		/// <summary>
		/// Gets or sets the template containing tokens to replace.
		/// </summary>
		[ConfigurationProperty(templateProperty, IsRequired= true)]
		public string Template
		{
			get
			{
				return (string)this[templateProperty];
			}
			set
			{
				this[templateProperty] = value;
			}
		}
	}

	/// <summary>
	/// This type supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
	/// Represents the process to build a <see cref="TextFormatter"/> described by a <see cref="TextFormatterData"/> configuration object.
	/// </summary>
	/// <remarks>This type is linked to the <see cref="TextFormatterData"/> type and it is used by the <see cref="LogFormatterCustomFactory"/> 
	/// to build the specific <see cref="ILogFormatter"/> object represented by the configuration object.
	/// </remarks>
	public class TextFormatterAssembler : IAssembler<ILogFormatter, FormatterData>
	{
		/// <summary>
		/// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// Builds a <see cref="TextFormatter"/> based on an instance of <see cref="TextFormatterData"/>.
		/// </summary>
		/// <seealso cref="LogFormatterCustomFactory"/>
		/// <param name="context">The <see cref="IBuilderContext"/> that represents the current building process.</param>
		/// <param name="objectConfiguration">The configuration object that describes the object to build. Must be an instance of <see cref="TextFormatterData"/>.</param>
		/// <param name="configurationSource">The source for configuration objects.</param>
		/// <param name="reflectionCache">The cache to use retrieving reflection information.</param>
		/// <returns>A fully initialized instance of <see cref="TextFormatter"/>.</returns>
		public ILogFormatter Assemble(IBuilderContext context, FormatterData objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
		{
			TextFormatterData castedObjectConfiguration = (TextFormatterData)objectConfiguration;

			ILogFormatter createdObject	= new TextFormatter(castedObjectConfiguration.Template);

			return createdObject;
		}
	}
}