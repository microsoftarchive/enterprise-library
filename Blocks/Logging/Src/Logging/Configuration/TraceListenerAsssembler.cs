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

using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
	/// <summary>
	/// Base class to represent the process to build a <see cref="TraceListener"/> described by a <see cref="TraceListenerData"/> subclass configuration object.
	/// </summary>
	public abstract class TraceListenerAsssembler : IAssembler<TraceListener, TraceListenerData>
	{
		/// <summary>
		/// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// Builds a specific <see cref="TraceListener"/> based on an instance of <see cref="TraceListenerData"/> subclass.
		/// </summary>
		/// <seealso cref="TraceListenerCustomFactory"/>
		/// <param name="context">The <see cref="IBuilderContext"/> that represents the current building process.</param>
		/// <param name="objectConfiguration">The configuration object that describes the object to build.</param>
		/// <param name="configurationSource">The source for configuration objects.</param>
		/// <param name="reflectionCache">The cache to use retrieving reflection information.</param>
		/// <returns>A fully initialized instance of a <see cref="TraceListener"/> subclass.</returns>
		public abstract TraceListener Assemble(IBuilderContext context, TraceListenerData objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache);

		/// <summary>
		/// Returns a new instance of the <see cref="ILogFormatter"/> represented by <paramref name="formatterName"/>.
		/// </summary>
		/// <param name="context">The <see cref="IBuilderContext"/> that represents the current building process.</param>
		/// <param name="formatterName">The name of the requested <see cref="ILogFormatter"/>, or null.</param>
		/// <param name="configurationSource">The source for configuration objects.</param>
		/// <param name="reflectionCache">The cache to use retrieving reflection information.</param>
		/// <returns>The <see cref="ILogFormatter"/> instance represented by the name <paramref name="formatterName"/> in the <paramref name="configurationSource"/> 
		/// if <paramref name="formatterName"/> is not <see langword="null"/> nor empty, <see langword="null"/> otherwise.</returns>
		protected ILogFormatter GetFormatter(IBuilderContext context, string formatterName, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
		{
			ILogFormatter formatter
				= string.IsNullOrEmpty(formatterName)
					? null
					: LogFormatterCustomFactory.Instance.Create(context, formatterName, configurationSource, reflectionCache);

			return formatter;
		}
	}
}
