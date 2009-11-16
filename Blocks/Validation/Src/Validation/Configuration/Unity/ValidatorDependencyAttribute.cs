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
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Validation.Properties;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Unity
{
    /// <summary>
    /// An attribute used when injecting dependencies that are <see cref="Validator{T}"/>
    /// that allows you to specify the ruleset and an optional <see cref="ValidationSpecificationSource"/>.
    /// </summary>
    public sealed class ValidatorDependencyAttribute : DependencyResolutionAttribute
    {
        /// <summary>
        /// A new <see cref="ValidatorDependencyAttribute"/> instance with the default
        /// rule set and a ValidationSpecificationSource of all.
        /// </summary>
        public ValidatorDependencyAttribute() :
            this(null)
        {
        }

        /// <summary>
        /// A new <see cref="ValidatorDependencyAttribute"/> instance with the given
        /// rule set and a ValidationSpecificationSource of all.
        /// </summary>
        /// <param name="ruleSetName">Name of the rule set used to resolve this
        /// validator.</param>
        public ValidatorDependencyAttribute(string ruleSetName)
        {
            RuleSet = ruleSetName;
            ValidationSource = ValidationSpecificationSource.All;
        }

        /// <summary>
        /// Rule set name to resolve with.
        /// </summary>
        public string RuleSet { get; set; }

        /// <summary>
        /// The source for the validation configuration: configuration, attributes,
        /// data annotations, or some combination thereof.
        /// </summary>
        public ValidationSpecificationSource ValidationSource { get; set; }

        /// <summary>
        /// Create an instance of <see cref="T:Microsoft.Practices.ObjectBuilder2.IDependencyResolverPolicy"/> that
        ///             will be used to get the value for the member this attribute is
        ///             applied to.
        /// </summary>
        /// <param name="typeToResolve">Type of parameter or property that
        ///             this attribute is decoration.</param>
        /// <returns>
        /// The resolver object.
        /// </returns>
        public override IDependencyResolverPolicy CreateResolver(Type typeToResolve)
        {
            if(!typeToResolve.IsGenericType ||
                typeToResolve.GetGenericTypeDefinition() != typeof(Validator<>))
            {
                throw new InvalidOperationException(
                    string.Format(CultureInfo.CurrentUICulture,
                                  Resources.IllegalUseOfValidationDependencyAttribute,
                                  typeToResolve.Name));
            }

            return new ValidatorResolver(RuleSet, ValidationSource, typeToResolve);
        }
    }
}
