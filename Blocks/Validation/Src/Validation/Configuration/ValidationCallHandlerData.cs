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
using System.Linq.Expressions;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Validation.PolicyInjection;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration
{
    /// <summary>
    /// A configuration element class that stores the configuration data for
    /// the <see cref="ValidationCallHandler"/>.
    /// </summary>
    public partial class ValidationCallHandlerData : CallHandlerData
    {
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
