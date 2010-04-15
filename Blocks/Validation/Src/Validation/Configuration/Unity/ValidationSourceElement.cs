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
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Properties;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Unity
{
    /// <summary>
    /// A configuration element that can be used in a Unity configuration
    /// section when specifying a parameter value. This one lets you
    /// specify the validation source (attributes, config, and so forth) to be
    /// used when creating the validators.
    /// </summary>
    public class ValidationSourceElement : ParameterValueElement
    {
        /// <summary>
        /// Ruleset to use when creating validator.
        /// </summary>
        [ConfigurationProperty("ruleSet", IsRequired = false, DefaultValue = null)]
        public string RuleSet
        {
            get { return (string) this["ruleSet"]; }
            set { this["ruleSet"] = value; }
        }

        /// <summary>
        /// The <see cref="ValidationSpecificationSource"/> to use when creating the
        /// validator.
        /// </summary>
        [ConfigurationProperty("validationSource", IsRequired = false, DefaultValue = ValidationSpecificationSource.All)]
        public ValidationSpecificationSource ValidationSource
        {
            get { return (ValidationSpecificationSource) this["validationSource"]; }
            set { this["validationSource"] = value; }
        }

        /// <summary>
        /// Generate an <see cref="InjectionParameterValue"/> object
        /// that will be used to configure the container for a type registration.
        /// </summary>
        /// <param name="container">Container that is being configured. Supplied in order
        /// to let custom implementations retrieve services; do not configure the container
        /// directly in this method.</param>
        /// <param name="parameterType">Type of the target parameter that will recieve this value.</param>
        /// <returns>The created <see cref="InjectionParameterValue"/>.</returns>
        public override InjectionParameterValue GetInjectionParameterValue(IUnityContainer container, Type parameterType)
        {
            GuardTypeIsValidator(parameterType);
            Type typeToValidate = GetTypeToValidate(parameterType);
            return new ValidatorParameter(typeToValidate, RuleSet, ValidationSource);
        }

        private static void GuardTypeIsValidator(Type targetType)
        {
            if(!targetType.IsGenericType ||
                targetType.GetGenericTypeDefinition() != typeof(Validator<>))
            {
                throw new InvalidOperationException(Resources.IllegalUseOfInjectionValidationSource);
            }
        }

        private static Type GetTypeToValidate(Type validatorType)
        {
            return validatorType.GetGenericArguments()[0];
        }
    }
}
