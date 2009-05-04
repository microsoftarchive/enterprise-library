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
using System.Linq.Expressions;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
    /// <summary>
    /// Configuration object for custom trace listenrs.
    /// </summary>
    [Assembler(typeof(CustomTraceListenerAssembler))]
    [ContainerPolicyCreator(typeof(BaseCustomTraceListenerPolicyCreator))]
    public class CustomTraceListenerData
        : BasicCustomTraceListenerData
    {
        internal const string formatterNameProperty = "formatter";

        /// <summary>
        /// Initializes with default values.
        /// </summary>
        public CustomTraceListenerData()
            : base()
        {
        }

        /// <summary>
        /// Initializes with name and provider type.
        /// </summary>
        public CustomTraceListenerData(string name, Type type, string initData)
            : base(name, type, initData)
        {
        }

        /// <summary>
        /// Initializes with name and provider type.
        /// </summary>
        public CustomTraceListenerData(string name, Type type, string initData, TraceOptions traceOutputOptions)
            : base(name, type, initData, traceOutputOptions)
        {
        }

        /// <summary>
        /// Initializes with name and fully qualified type name of the provider type.
        /// </summary>
        public CustomTraceListenerData(string name, string typeName, string initData, TraceOptions traceOutputOptions)
            : base(name, typeName, initData, traceOutputOptions)
        {
        }

        /// <summary>
        /// Gets or sets the name of the formatter. Can be <see langword="null"/>.
        /// </summary>
        public string Formatter
        {
            get { return (string)base[formatterNameProperty]; }
            set { base[formatterNameProperty] = value; }
        }

        /// <summary>
        /// Creates the helper that enapsulates the configuration properties management.
        /// </summary>
        /// <returns></returns>
        protected override CustomProviderDataHelper<BasicCustomTraceListenerData> CreateHelper()
        {
            return new CustomTraceListenerDataHelper(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override Expression<Func<TraceListener>> GetCreationExpression()
        {
            Expression<Func<TraceListener>> baseLambdaExpression = base.GetCreationExpression();

            if (!(typeof(CustomTraceListener).IsAssignableFrom(this.Type) && !string.IsNullOrEmpty(this.Formatter)))
            {
                return baseLambdaExpression;
            }

            Expression<Func<ILogFormatter>> resolveFormatterExpression =
                () => Container.Resolved<ILogFormatter>(this.Formatter);

            return Expression.Lambda<Func<TraceListener>>(
                Expression.MemberInit(
                    (NewExpression)baseLambdaExpression.Body,
                    Expression.Bind(this.Type.GetProperty("Formatter"), resolveFormatterExpression.Body)));
        }
    }

    /// <summary>
    /// This type supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
    /// Represents the process to build a custom trace listener described by a <see cref="CustomTraceListenerData"/> configuration object.
    /// </summary>
    /// <remarks>This type is linked to the <see cref="CustomTraceListenerData"/> type and it is used by the <see cref="TraceListenerCustomFactory"/> 
    /// to build the specific <see cref="TraceListener"/> object represented by the configuration object.
    /// </remarks>
    public class CustomTraceListenerAssembler : SystemDiagnosticsTraceListenerAssembler
    {
        /// <summary>
        /// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
        /// Builds a custom trace listener based on an instance of <see cref="CustomTraceListenerData"/>.
        /// </summary>
        /// <seealso cref="TraceListenerCustomFactory"/>
        /// <param name="context">The <see cref="IBuilderContext"/> that represents the current building process.</param>
        /// <param name="objectConfiguration">The configuration object that describes the object to build. Must be an instance of <see cref="CustomTraceListenerData"/>.</param>
        /// <param name="configurationSource">The source for configuration objects.</param>
        /// <param name="reflectionCache">The cache to use retrieving reflection information.</param>
        /// <returns>A fully initialized custom trace listener.</returns>
        public override TraceListener Assemble(IBuilderContext context, TraceListenerData objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
        {
            TraceListener createdObject = base.Assemble(context, objectConfiguration, configurationSource, reflectionCache);

            if (createdObject is CustomTraceListener)
            {
                CustomTraceListenerData castedObjectConfiguration
                    = (CustomTraceListenerData)objectConfiguration;
                ILogFormatter formatter = GetFormatter(context, castedObjectConfiguration.Formatter, configurationSource, reflectionCache);
                ((CustomTraceListener)createdObject).Formatter = formatter;
            }

            return createdObject;
        }
    }
}
