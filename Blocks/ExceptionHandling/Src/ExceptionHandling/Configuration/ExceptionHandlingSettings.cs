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
using System.Configuration;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Properties;


namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration
{
    /// <summary>
    /// Represents the Exception Handling Application Block configuration section in a configuration file.
    /// </summary>
    [ResourceDisplayName(typeof(Resources), "SectionDisplayName")]
    [ViewModel("Console.Wpf.ViewModel.HierarchicalSectionViewModel, Console.Wpf")]
    public class ExceptionHandlingSettings : SerializableConfigurationSection, ITypeRegistrationsProvider
    {
        /// <summary>
        /// Gets the configuration section name for the library.
        /// </summary>
        public const string SectionName = "exceptionHandling";


        private const string policiesProperty = "exceptionPolicies";

        /// <summary>
        /// Gets the <see cref="ExceptionHandlingSettings"/> section in the configuration source.
        /// </summary>
        /// <param name="configurationSource">The <see cref="IConfigurationSource"/> to get the section from.</param>
        /// <returns>The exception handling section.</returns>
        public static ExceptionHandlingSettings GetExceptionHandlingSettings(IConfigurationSource configurationSource)
        {
            return (ExceptionHandlingSettings)configurationSource.GetSection(SectionName);
        }

        /// <summary>
        /// Initializes a new instance of an <see cref="ExceptionHandlingSettings"/> class.
        /// </summary>
        public ExceptionHandlingSettings()
        {
            this[policiesProperty] = new NamedElementCollection<ExceptionPolicyData>();
        }

        /// <summary>
        /// Gets a collection of <see cref="ExceptionPolicyData"/> objects.
        /// </summary>
        /// <value>
        /// A collection of <see cref="ExceptionPolicyData"/> objects.
        /// </value>
        [ConfigurationProperty(policiesProperty)]
        [ResourceDisplayName(typeof(Resources), "ExceptionPoliciesDisplayName")]
        [System.ComponentModel.Browsable(false)]
        [ConfigurationCollection(typeof(ExceptionPolicyData))]
        public NamedElementCollection<ExceptionPolicyData> ExceptionPolicies
        {
            get { return (NamedElementCollection<ExceptionPolicyData>)this[policiesProperty]; }
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
                        policyTypeData.ExceptionHandlers.SelectMany(ehd =>ehd.GetRegistrations(policyTypeRegistration.Name)));
                }
            }

            registrations.Add(
                GetManagerRegistration(ExceptionPolicies.Select(p => p.Name).ToArray())
                );

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

        private static IEnumerable<TypeRegistration> GetDefaultInstrumentationRegistrations(IConfigurationSource configurationSource)
        {
            var instrumentationSection = InstrumentationConfigurationSection.GetSection(configurationSource);

            yield return new TypeRegistration<DefaultExceptionHandlingEventLogger>(
                () => new DefaultExceptionHandlingEventLogger(
                    instrumentationSection.EventLoggingEnabled, 
                    instrumentationSection.WmiEnabled))
                {
                    Lifetime = TypeRegistrationLifetime.Transient,
                    IsDefault = true

                };

            yield return new TypeRegistration<IDefaultExceptionHandlingInstrumentationProvider>(
                () => new DefaultExceptionHandlingEventLogger(
                    instrumentationSection.PerformanceCountersEnabled,
                    instrumentationSection.EventLoggingEnabled,
                    instrumentationSection.WmiEnabled,
                    instrumentationSection.ApplicationInstanceName))
                {
                    Lifetime = TypeRegistrationLifetime.Transient,
                    IsDefault = true
                };
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
