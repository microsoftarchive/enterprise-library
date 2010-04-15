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
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Validation.Properties;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Unity
{
    /// <summary>
    /// An object that can be passed to <see cref="IUnityContainer.RegisterType"/> in order
    /// to specify where validation comes from: config, attribute, data annotations,or
    /// some combination thereof.
    /// </summary>
    public class InjectionValidationSource : InjectionMember
    {
        /// <summary>
        /// Create a new instance of <see cref="InjectionValidationSource"/>, specifying
        /// the validation source.
        /// </summary>
        /// <param name="source">Source of validation metadata.</param>
        public InjectionValidationSource(ValidationSpecificationSource source)
        {
            Source = source;
        }

        /// <summary>
        /// The current source of validation metadata.
        /// </summary>
        public ValidationSpecificationSource Source { get; private set; }

        /// <summary>
        /// Add policies to the <paramref name="policies"/> to configure the
        ///             container to call this constructor with the appropriate parameter values.
        /// </summary>
        /// <param name="serviceType">Type of interface being registered. If no interface,
        ///             this will be null.</param><param name="implementationType">Type of concrete type being registered.</param><param name="name">Name used to resolve the type object.</param><param name="policies">Policy list to add policies to.</param>
        public override void AddPolicies(Type serviceType, Type implementationType, string name, IPolicyList policies)
        {
            if (implementationType == null) throw new ArgumentNullException("implementationType");

            if(!implementationType.IsGenericType ||
                implementationType.GetGenericTypeDefinition() != typeof(Validator<>))
            {
                throw new InvalidOperationException(Resources.IllegalUseOfInjectionValidationSource);
            }

            var key = new NamedTypeBuildKey(implementationType, name);
            var policy = new ValidationSpecificationSourcePolicy(Source);

            policies.Set<ValidationSpecificationSourcePolicy>(policy, key);
        }
    }
}
