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
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Instrumentation;


namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration
{
    /// <summary>
    /// Represents the Exception Handling Application Block configuration section in a configuration file.
    /// </summary>
    [ViewModel(ExceptionHandlingDesignTime.ViewModelTypeNames.ExceptionHandlingSectionViewModel)]
    [ResourceDescription(typeof(DesignResources), "ExceptionHandlingSettingsDescription")]
    [ResourceDisplayName(typeof(DesignResources), "ExceptionHandlingSettingsDisplayName")]
    public partial class ExceptionHandlingSettings : SerializableConfigurationSection, ITypeRegistrationsProvider
    {
        private const string policiesProperty = "exceptionPolicies";

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
        [ResourceDescription(typeof(DesignResources), "ExceptionHandlingSettingsExceptionPoliciesDescription")]
        [ResourceDisplayName(typeof(DesignResources), "ExceptionHandlingSettingsExceptionPoliciesDisplayName")]
        [ConfigurationCollection(typeof(ExceptionPolicyData))]
        [Command(ExceptionHandlingDesignTime.CommandTypeNames.AddExceptionPolicyCommand, CommandPlacement = CommandPlacement.ContextAdd, Replace = CommandReplacement.DefaultAddCommandReplacement)]
        public NamedElementCollection<ExceptionPolicyData> ExceptionPolicies
        {
            get { return (NamedElementCollection<ExceptionPolicyData>)this[policiesProperty]; }
        }

        private static IEnumerable<TypeRegistration> GetDefaultInstrumentationRegistrations(IConfigurationSource configurationSource)
        {
            var instrumentationSection = InstrumentationConfigurationSection.GetSection(configurationSource);

            yield return new TypeRegistration<DefaultExceptionHandlingEventLogger>(
                () => new DefaultExceptionHandlingEventLogger(instrumentationSection.EventLoggingEnabled))
            {
                Lifetime = TypeRegistrationLifetime.Transient,
                IsDefault = true

            };

            yield return new TypeRegistration<IDefaultExceptionHandlingInstrumentationProvider>(
                () => new DefaultExceptionHandlingEventLogger(
                    instrumentationSection.PerformanceCountersEnabled,
                    instrumentationSection.EventLoggingEnabled,
                    instrumentationSection.ApplicationInstanceName))
            {
                Lifetime = TypeRegistrationLifetime.Transient,
                IsDefault = true
            };
        }
    }
}
