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
    /// A configuration element class that stores the config data for
    /// the <see cref="ValidationCallHandler"/>.
    /// </summary>
    [Assembler(typeof(ValidationCallHandlerAssembler))]
    public class ValidationCallHandlerData : CallHandlerData
    {
        private const string RuleSetPropertyName = "ruleSet";
        private const string SpecificationSourcePropertyName = "specificationSource";

        /// <summary>
        /// Create a new <see cref="ValidationCallHandlerData"/> instance.
        /// </summary>
        public ValidationCallHandlerData()
            : base()
        {
        }

        /// <summary>
        /// Create a new <see cref="ValidationCallHandlerData"/> instance.
        /// </summary>
        /// <param name="handlerName">Name of handler in configuration.</param>
        public ValidationCallHandlerData(string handlerName)
            : base(handlerName, typeof(ValidationCallHandler))
        {
        }

        /// <summary>
        /// Create a new <see cref="ValidationCallHandlerData"/> instance.
        /// </summary>
        /// <param name="handlerName">Name of handler in configuration.</param>
        /// <param name="handlerOrder">Order of handler in configuration.</param>
        public ValidationCallHandlerData(string handlerName, int handlerOrder)
            : base(handlerName, typeof(ValidationCallHandler))
        {
            this.Order = handlerOrder;
        }

        /// <summary>
        /// The ruleset name to use for all types. Empty string means default ruleset 
        /// </summary>
        /// <value>The "ruleSet" configuration property.</value>
        [ConfigurationProperty(RuleSetPropertyName)]
        public string RuleSet
        {
            get { return (string)base[RuleSetPropertyName]; }
            set { base[RuleSetPropertyName] = value; }
        }

        /// <summary>
        /// SpecificationSource (Both | Attributes | Configuration) : Where to look for validation rules. Default is Both.
        /// </summary>
        /// <value>The "specificationSource" configuration attribute.</value>
        [ConfigurationProperty(SpecificationSourcePropertyName, IsRequired = true, DefaultValue = SpecificationSource.Both)]
        public SpecificationSource SpecificationSource
        {
            get { return (SpecificationSource) base[SpecificationSourcePropertyName]; }
            set { base[SpecificationSourcePropertyName] = value; }
        }
    }

    /// <summary>
    /// A class use 
    /// </summary>
    public class ValidationCallHandlerAssembler : IAssembler<ICallHandler, CallHandlerData>
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
        public ICallHandler Assemble(IBuilderContext context, CallHandlerData objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
        {
            ValidationCallHandlerData castedHandlerData = (ValidationCallHandlerData)objectConfiguration;

            ValidationCallHandler callHandler = new ValidationCallHandler(castedHandlerData.RuleSet, castedHandlerData.SpecificationSource);

            return callHandler;
        }
    }
}
