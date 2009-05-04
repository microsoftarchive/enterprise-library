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
using System.Collections.Specialized;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
    /// <summary>
    /// Represents the configuration settings for any trace listener.
    /// </summary>
    [Assembler(typeof(SystemDiagnosticsTraceListenerAssembler))]
    [ContainerPolicyCreator(typeof(BaseCustomTraceListenerPolicyCreator))]
    public class SystemDiagnosticsTraceListenerData
        : BasicCustomTraceListenerData
    {
        /// <summary>
        /// Initializes with default values.
        /// </summary>
        public SystemDiagnosticsTraceListenerData()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SystemDiagnosticsTraceListenerData"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        /// <param name="initData">The init data.</param>
        public SystemDiagnosticsTraceListenerData(string name, Type type, string initData)
            : base(name, type, initData)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="T:SystemDiagnosticsTraceListenerData"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="typeName">The type.</param>
        /// <param name="initData">The init data.</param>
        public SystemDiagnosticsTraceListenerData(string name, string typeName, string initData)
            : base(name, typeName, initData)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SystemDiagnosticsTraceListenerData"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        /// <param name="initData">The init data.</param>
        /// <param name="traceOutputOptions">The trace output options.</param>
        public SystemDiagnosticsTraceListenerData(string name, Type type, string initData, TraceOptions traceOutputOptions)
            : base(name, type, initData, traceOutputOptions)
        {
        }
    }

    /// <summary>
    /// This type supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
    /// Represents the process to build a <see cref="TraceListener"/> that is not specifically designed to work with 
    /// EnterpriseLibrary described by a <see cref="SystemDiagnosticsTraceListenerData"/> configuration object.
    /// </summary>
    /// <remarks>This type is linked to the <see cref="SystemDiagnosticsTraceListenerData"/> type and it is used by the <see cref="TraceListenerCustomFactory"/> 
    /// to build the specific <see cref="TraceListener"/> object represented by the configuration object.
    /// </remarks>
    public class SystemDiagnosticsTraceListenerAssembler : TraceListenerAsssembler
    {
        /// <summary>
        /// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
        /// Builds a <see cref="TraceListener"/> based on an instance of <see cref="SystemDiagnosticsTraceListenerData"/>.
        /// </summary>
        /// <remarks>
        /// The building process for non Enterprise Library specific <see cref="TraceListener"/>s mimics the creation process used by System.Diagnostics' configuration.
        /// </remarks>
        /// <seealso cref="TraceListenerCustomFactory"/>
        /// <param name="context">The <see cref="IBuilderContext"/> that represents the current building process.</param>
        /// <param name="objectConfiguration">The configuration object that describes the object to build. Must be an instance of <see cref="SystemDiagnosticsTraceListenerData"/>.</param>
        /// <param name="configurationSource">The source for configuration objects.</param>
        /// <param name="reflectionCache">The cache to use retrieving reflection information.</param>
        /// <returns>A fully initialized instance of a <see cref="TraceListener"/> subclass.</returns>
        public override TraceListener Assemble(IBuilderContext context, TraceListenerData objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
        {
            BasicCustomTraceListenerData castedObjectConfiguration
                = (BasicCustomTraceListenerData)objectConfiguration;

            Type type = castedObjectConfiguration.Type;
            string name = castedObjectConfiguration.Name;
            TraceOptions traceOutputOptions = castedObjectConfiguration.TraceOutputOptions;
            string initData = castedObjectConfiguration.InitData;
            NameValueCollection attributes = castedObjectConfiguration.Attributes;

            return SystemDiagnosticsTraceListenerCreationHelper.CreateSystemDiagnosticsTraceListener(name, type, initData, attributes);
        }
    }
}
