//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration
{
    /// <summary>
    /// Represents the configuration for an exception policy.
    /// </summary> 
    public partial class ExceptionPolicyData : NamedConfigurationElement
    {
        private const string exceptionTypesProperty = "exceptionTypes";

        /// <summary>
        /// Creates a new instance of ExceptionPolicyData.
        /// </summary>
        public ExceptionPolicyData()
        {
            this.ExceptionTypes = new NamedElementCollection<ExceptionTypeData>();
        }

        /// <summary>
        /// Gets a collection of <see cref="ExceptionTypeData"/> objects.
        /// </summary>
        /// <value>
        /// A collection of <see cref="ExceptionTypeData"/> objects.
        /// </value>
        public NamedElementCollection<ExceptionTypeData> ExceptionTypes
        {
            get;
            private set;
        }

        /// <summary>
        /// Retrieves the <see cref="TypeRegistration"/> for registering a <see cref="ExceptionPolicyImpl"/> in the container.
        /// </summary>
        /// <returns>A completed <see cref="TypeRegistration"/></returns>
        public IEnumerable<TypeRegistration> GetRegistration(IConfigurationSource configurationSource)
        {
            yield return new TypeRegistration<ExceptionPolicyImpl>(
                () => new ExceptionPolicyImpl(
                    Name,
                    Container.ResolvedEnumerable<ExceptionPolicyEntry>(
                        from data in ExceptionTypes
                        select BuildChildName(data.Name)

                    )))
                    {
                        Name = Name,
                        Lifetime = TypeRegistrationLifetime.Transient
                    };

            yield return GetInstrumentationRegistration(configurationSource);
        }

        private TypeRegistration GetInstrumentationRegistration(IConfigurationSource configurationSource)
        {
            return new TypeRegistration<IExceptionHandlingInstrumentationProvider>(
                () => new NullExceptionHandlingInstrumentationProvider())
                       {
                           Name = Name,
                           Lifetime = TypeRegistrationLifetime.Transient
                       };
        }
    }
}