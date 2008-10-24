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
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
	/// <summary>
	/// Represents the configuration settings that describe a <see cref="BinaryLogFormatter"/>.
	/// </summary>
	[Assembler(typeof(BinaryLogFormatterAssembler))]
	public class BinaryLogFormatterData : FormatterData
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="BinaryLogFormatterData"/> class with default values.
		/// </summary>
		public BinaryLogFormatterData()
			: base()
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="BinaryLogFormatterData"/> class with a name.
		/// </summary>
		/// <param name="name">The name for the represented <see cref="BinaryLogFormatter"/>.</param>
		public BinaryLogFormatterData(string name)
			: base(name, typeof(BinaryLogFormatter))
		{ }
	}

	/// <summary>
	/// This type supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
	/// Represents the process to build a <see cref="BinaryLogFormatter"/> described by a <see cref="BinaryLogFormatterData"/> configuration object.
	/// </summary>
	/// <remarks>This type is linked to the <see cref="BinaryLogFormatterData"/> type and it is used by the <see cref="LogFormatterCustomFactory"/> 
	/// to build the specific <see cref="ILogFormatter"/> object represented by the configuration object.
	/// </remarks>
	public class BinaryLogFormatterAssembler : IAssembler<ILogFormatter, FormatterData>
	{
		/// <summary>
		/// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// Builds a <see cref="BinaryLogFormatter"/> based on an instance of <see cref="BinaryLogFormatterData"/>.
		/// </summary>
		/// <seealso cref="LogFormatterCustomFactory"/>
		/// <param name="context">The <see cref="IBuilderContext"/> that represents the current building process.</param>
		/// <param name="objectConfiguration">The configuration object that describes the object to build. Must be an instance of <see cref="BinaryLogFormatterData"/>.</param>
		/// <param name="configurationSource">The source for configuration objects.</param>
		/// <param name="reflectionCache">The cache to use retrieving reflection information.</param>
		/// <returns>A fully initialized instance of <see cref="BinaryLogFormatter"/>.</returns>
		public ILogFormatter Assemble(IBuilderContext context, FormatterData objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
		{
			return new BinaryLogFormatter();
		}
	}
}
