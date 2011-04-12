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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Instrumentation;


namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration
{
    /// <summary>
    /// Represents the Exception Handling Application Block configuration section in a configuration file.
    /// </summary>
    public partial class ExceptionHandlingSettings : ConfigurationSection, ITypeRegistrationsProvider
    {
        /// <summary>
        /// Initializes a new instance of an <see cref="ExceptionHandlingSettings"/> class.
        /// </summary>
        public ExceptionHandlingSettings()
        {
            this.ExceptionPolicies = new NamedElementCollection<ExceptionPolicyData>();
        }

        /// <summary>
        /// Gets a collection of <see cref="ExceptionPolicyData"/> objects.
        /// </summary>
        /// <value>
        /// A collection of <see cref="ExceptionPolicyData"/> objects.
        /// </value>
        public NamedElementCollection<ExceptionPolicyData> ExceptionPolicies
        {
            get;
            private set;
        }

        private static IEnumerable<TypeRegistration> GetDefaultInstrumentationRegistrations(IConfigurationSource configurationSource)
        {
            yield return new TypeRegistration<IDefaultExceptionHandlingInstrumentationProvider>(
                () => new NullDefaultExceptionHandlingInstrumentationProvider())
            {
                Lifetime = TypeRegistrationLifetime.Transient,
                IsDefault = true
            };
        }
    }
}
