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
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
	/// <summary>
	/// Represents the configuration settings that describe a <see cref="WmiTraceListener"/>.
	/// </summary>
	[Assembler(typeof(WmiTraceListenerAssembler))]
	public class WmiTraceListenerData : TraceListenerData
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="WmiTraceListenerData"/> class.
		/// </summary>
		public WmiTraceListenerData()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="WmiTraceListenerData"/> class.
		/// </summary>
		/// <param name="name">The name for the configuration object.</param>
		public WmiTraceListenerData(string name)
			: this(name, TraceOptions.None)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="WmiTraceListenerData"/> class.
		/// </summary>
		/// <param name="name">The name for the configuration object.</param>
		/// <param name="traceOutputOptions">The trace options.</param>
		public WmiTraceListenerData(string name, TraceOptions traceOutputOptions)
			: base(name, typeof(WmiTraceListener), traceOutputOptions)
		{
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="WmiTraceListenerData"/> class.
        /// </summary>
        /// <param name="name">The name for the configuration object.</param>
        /// <param name="traceOutputOptions">The trace options.</param>
        /// <param name="filter">The filter to apply.</param>
        public WmiTraceListenerData(string name, TraceOptions traceOutputOptions, SourceLevels filter)
            : base(name, typeof(WmiTraceListener), traceOutputOptions, filter)
        {
        }
	}

	/// <summary>
	/// This type supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
	/// Represents the process to build a <see cref="WmiTraceListener"/> described by a <see cref="WmiTraceListenerData"/> configuration object.
	/// </summary>
	/// <remarks>This type is linked to the <see cref="WmiTraceListenerData"/> type and it is used by the <see cref="TraceListenerCustomFactory"/> 
	/// to build the specific <see cref="TraceListener"/> object represented by the configuration object.
	/// </remarks>
	public class WmiTraceListenerAssembler : TraceListenerAsssembler
	{
		/// <summary>
		/// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// Builds a <see cref="WmiTraceListener"/> based on an instance of <see cref="WmiTraceListenerData"/>.
		/// </summary>
		/// <seealso cref="TraceListenerCustomFactory"/>
		/// <param name="context">The <see cref="IBuilderContext"/> that represents the current building process.</param>
		/// <param name="objectConfiguration">The configuration object that describes the object to build. Must be an instance of <see cref="WmiTraceListenerData"/>.</param>
		/// <param name="configurationSource">The source for configuration objects.</param>
		/// <param name="reflectionCache">The cache to use retrieving reflection information.</param>
		/// <returns>A fully initialized instance of <see cref="WmiTraceListener"/>.</returns>
		public override TraceListener Assemble(IBuilderContext context, TraceListenerData objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
		{
			return new WmiTraceListener();
		}
	}
}
