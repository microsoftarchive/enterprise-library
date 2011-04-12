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

using System;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration
{
    /// <summary>
    /// Configuration element that stores the configuration information for an instance
    /// of <see cref="MethodSignatureMatchingRule"/>.
    /// </summary>
    public partial class MethodSignatureMatchingRuleData : StringBasedMatchingRuleData
    {
        /// <summary>
        /// Get the set of <see cref="TypeRegistration"/> objects needed to
        /// register the matching rule represented by this config element and its associated objects.
        /// </summary>
        /// <param name="nameSuffix">A suffix for the names in the generated type registration objects.</param>
        /// <returns>The set of <see cref="TypeRegistration"/> objects.</returns>
        public override IEnumerable<TypeRegistration> GetRegistrations(string nameSuffix)
        {
            List<string> parameterTypes = new List<string>();
            foreach (ParameterTypeElement parameterType in this.Parameters)
            {
                parameterTypes.Add(parameterType.ParameterTypeName);
            }

            yield return
                new TypeRegistration<IMatchingRule>(
                    () => new MethodSignatureMatchingRule(this.Match, parameterTypes, this.IgnoreCase))
                {
                    Name = this.Name + nameSuffix,
                    Lifetime = TypeRegistrationLifetime.Transient
                };
        }
    }
    
    /// <summary>
    /// A configuration element representing a single parameter in a method signature.
    /// </summary>
    public partial class ParameterTypeElement 
    {
        /// <summary>
        /// Constructs a new <see cref="ParameterTypeElement"/> instance.
        /// </summary>
        public ParameterTypeElement()
        {
        }

        /// <summary>
        /// Constructs a new <see cref="ParameterTypeElement"/> instance.
        /// </summary>
        /// <param name="name">unique identifier for this parameter. The name does
        /// NOT need to match the target's parameter name.</param>
        /// <param name="parameterType">Expected type of parameter</param>
        public ParameterTypeElement(string name, string parameterType)
        {
            Name = name;
            ParameterTypeName = parameterType;
        }
    }
}
