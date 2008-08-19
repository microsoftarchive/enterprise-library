//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Policy Injection Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Configuration
{
    /// <summary>
    /// Configuration element storing configuration information for the
    /// <see cref="ExceptionCallHandler"/> class.
    /// </summary>
    [Assembler(typeof(ExceptionCallHandlerAssembler))]
    public class ExceptionCallHandlerData : CallHandlerData
    {
        private const string ExceptionPolicyNamePropertyName = "exceptionPolicyName";

        /// <summary>
        /// Construct a new <see cref="ExceptionCallHandlerData"/>.
        /// </summary>
        public ExceptionCallHandlerData()
        {
        }

        /// <summary>
        /// Construct a new <see cref="ExceptionCallHandlerData"/>.
        /// </summary>
        /// <param name="handlerName">Name of the handler.</param>
        public ExceptionCallHandlerData(string handlerName)
            :base(handlerName, typeof(ExceptionCallHandler))
        {
        }

        /// <summary>
        /// Construct a new <see cref="ExceptionCallHandlerData"/>.
        /// </summary>
        /// <param name="handlerName">Name of the handler.</param>
        /// <param name="exceptionPolicyName">Exception policy name to use in handler.</param>
        public ExceptionCallHandlerData(string handlerName, string exceptionPolicyName) 
            : base(handlerName, typeof(ExceptionCallHandler))
        {
            ExceptionPolicyName = exceptionPolicyName;
        }

        /// <summary>
        /// Construct a new <see cref="ExceptionCallHandlerData"/>.
        /// </summary>
        /// <param name="handlerName">Name of the handler.</param>
        /// <param name="handlerOrder">Order to use in handler.</param>
        public ExceptionCallHandlerData(string handlerName, int handlerOrder)
            : base(handlerName, typeof(ExceptionCallHandler))
        {
            Order = handlerOrder;
        }

        /// <summary>
        /// The exception policy name as defined in configuration for the Exception Handling block.
        /// </summary>
        /// <value>The "exceptionPolicyName" attribute in configuration</value>
        [ConfigurationProperty(ExceptionPolicyNamePropertyName, IsRequired=true)]
        public string ExceptionPolicyName
        {
            get { return (string)base[ExceptionPolicyNamePropertyName]; }
            set { base[ExceptionPolicyNamePropertyName] = value; }
        }
    }

    /// <summary>
    /// Class used by ObjectBuilder to construct an <see cref="ExceptionCallHandler"/> from
    /// a <see cref="ExceptionCallHandlerData"/> instance.
    /// </summary>
    public class ExceptionCallHandlerAssembler : IAssembler<ICallHandler, CallHandlerData>
    {
        /// <summary>
        /// Builds an instance of the subtype of <typeparamref name="TObject"/> type the receiver knows how to build,  based on 
        /// an a configuration object.
        /// </summary>
        /// <param name="context">The <see cref="IBuilderContext"/> that represents the current building process.</param>
        /// <param name="objectConfiguration">The configuration object that describes the object to build.</param>
        /// <param name="configurationSource">The source for configuration objects.</param>
        /// <param name="reflectionCache">The cache to use retrieving reflection information.</param>
        /// <returns>A fully initialized instance of the <typeparamref name="TObject"/> subtype.</returns>
        public ICallHandler Assemble(IBuilderContext context, CallHandlerData objectConfiguration,
                                     IConfigurationSource configurationSource,
                                     ConfigurationReflectionCache reflectionCache)
        {
            ExceptionCallHandlerData handlerData = (ExceptionCallHandlerData) objectConfiguration;

            ExceptionCallHandler handler = new ExceptionCallHandler(handlerData.ExceptionPolicyName, handlerData.Order);
            return handler;
        }
    }
}
