//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Validation Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq.Expressions;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Validation.PolicyInjection;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration
{
    /// <summary>
    /// A configuration element class that stores the configuration data for
    /// the <see cref="ValidationCallHandler"/>.
    /// </summary>
    [ResourceDescription(typeof(DesignResources), "ValidationCallHandlerDataDescription")]
    [ResourceDisplayName(typeof(DesignResources), "ValidationCallHandlerDataDisplayName")]
    [AddSateliteProviderCommand(ValidationSettings.SectionName)]
    public class ValidationCallHandlerData : CallHandlerData
    {
        private const string RuleSetPropertyName = "ruleSet";
        private const string SpecificationSourcePropertyName = "specificationSource";

        /// <summary>
        /// Create a new <see cref="ValidationCallHandlerData"/> instance.
        /// </summary>
        public ValidationCallHandlerData()
        {
            Type = typeof (ValidationCallHandler);
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
        [Reference(typeof(ValidationSettings), typeof(ValidationRulesetData))]
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
        [ResourceDescription(typeof(DesignResources), "ValidationCallHandlerDataSpecificationSourceDescription")]
        [ResourceDisplayName(typeof(DesignResources), "ValidationCallHandlerDataSpecificationSourceDisplayName")]
        public SpecificationSource SpecificationSource
        {
            get { return (SpecificationSource)base[SpecificationSourcePropertyName]; }
            set { base[SpecificationSourcePropertyName] = value; }
        }

        /// <summary>
        /// Get the set of <see cref="TypeRegistration"/> objects needed to
        /// register the call handler represented by this config element and its associated objects.
        /// </summary>
        /// <param name="nameSuffix">A suffix for the names in the generated type registration objects.</param>
        /// <returns>The set of <see cref="TypeRegistration"/> objects.</returns>
        public override IEnumerable<TypeRegistration> GetRegistrations(string nameSuffix)
        {
            Expression<Func<ICallHandler>> registrationExpression;
            switch (this.SpecificationSource)
            {
                case SpecificationSource.Both:
                    registrationExpression = () =>
                                             new ValidationCallHandler(this.RuleSet, Container.Resolved<ValidatorFactory>(), this.Order);
                    break;
                case SpecificationSource.Attributes:
                    registrationExpression = () =>
                                             new ValidationCallHandler(this.RuleSet, Container.Resolved<AttributeValidatorFactory>(), this.Order);
                    break;
                case SpecificationSource.Configuration:
                    registrationExpression = () =>
                                             new ValidationCallHandler(this.RuleSet, Container.Resolved<ConfigurationValidatorFactory>(), this.Order);
                    break;
                default:
                    registrationExpression = () =>
                                             new ValidationCallHandler(this.RuleSet, (ValidatorFactory)null, this.Order);
                    break;
            }

            yield return
                new TypeRegistration<ICallHandler>(registrationExpression)
                    {
                        Name = this.Name + nameSuffix,
                        Lifetime = TypeRegistrationLifetime.Transient
                    };
        }
    }
}
