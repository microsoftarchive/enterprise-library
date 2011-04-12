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

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration
{
    partial class ExceptionHandlingSettings
    {
        /// <summary>
        /// Gets the configuration section name for the library.
        /// </summary>
        public const string SectionName = "exceptionHandling";

        /// <summary>
        /// Gets the <see cref="ExceptionHandlingSettings"/> section in the configuration source.
        /// </summary>
        /// <param name="configurationSource">The <see cref="IConfigurationSource"/> to get the section from.</param>
        /// <returns>The exception handling section.</returns>
        public static ExceptionHandlingSettings GetExceptionHandlingSettings(IConfigurationSource configurationSource)
        {
            if (configurationSource == null) throw new ArgumentNullException("configurationSource");
            return (ExceptionHandlingSettings)configurationSource.GetSection(SectionName);
        }

        /// <summary>
        /// Creates the <see cref="TypeRegistration"/> entries to use when configuring a container for Exception Handling
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TypeRegistration> GetRegistrations(IConfigurationSource configurationSource)
        {
            var registrations = new List<TypeRegistration>();

            registrations.AddRange(GetDefaultInstrumentationRegistrations(configurationSource));

            foreach (ExceptionPolicyData policyData in ExceptionPolicies)
            {
                registrations.AddRange(policyData.GetRegistration(configurationSource));

                foreach (var policyTypeData in policyData.ExceptionTypes)
                {
                    TypeRegistration policyTypeRegistration =
                        policyTypeData.GetRegistration(policyData.Name);
                    registrations.Add(policyTypeRegistration);

                    registrations.AddRange(
                        policyTypeData.ExceptionHandlers.SelectMany(ehd => ehd.GetRegistrations(policyTypeRegistration.Name)));
                }
            }

            TypeRegistration managerRegistration =
                GetManagerRegistration(ExceptionPolicies.Select(p => p.Name).ToArray());
            managerRegistration.IsPublicName = true;

            registrations.Add(managerRegistration);

            return registrations;
        }

        /// <summary>
        /// Return the <see cref="TypeRegistration"/> objects needed to reconfigure
        /// the container after a configuration source has changed.
        /// </summary>
        /// <remarks>If there are no reregistrations, return an empty sequence.</remarks>
        /// <param name="configurationSource">The <see cref="IConfigurationSource"/> containing
        /// the configuration information.</param>
        /// <returns>The sequence of <see cref="TypeRegistration"/> objects.</returns>
        public IEnumerable<TypeRegistration> GetUpdatedRegistrations(IConfigurationSource configurationSource)
        {
            return GetRegistrations(configurationSource);
        }

        private static TypeRegistration GetManagerRegistration(string[] policyNames)
        {
            return new TypeRegistration<ExceptionManager>(
                () => new ExceptionManagerImpl(Container.ResolvedEnumerable<ExceptionPolicyImpl>(policyNames),
                    Container.Resolved<IDefaultExceptionHandlingInstrumentationProvider>())
                ) { IsDefault = true };
        }
    }
}
