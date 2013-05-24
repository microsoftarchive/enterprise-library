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
using System.Globalization;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration
{
    /// <summary>
    /// Represents the Exception Handling Application Block configuration section in a configuration file.
    /// </summary>
    [ViewModel(ExceptionHandlingDesignTime.ViewModelTypeNames.ExceptionHandlingSectionViewModel)]
    [ResourceDescription(typeof(DesignResources), "ExceptionHandlingSettingsDescription")]
    [ResourceDisplayName(typeof(DesignResources), "ExceptionHandlingSettingsDisplayName")]
    public class ExceptionHandlingSettings : SerializableConfigurationSection
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
            if (configurationSource == null) throw new ArgumentNullException("configurationSource");
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
        [ResourceDescription(typeof(DesignResources), "ExceptionHandlingSettingsExceptionPoliciesDescription")]
        [ResourceDisplayName(typeof(DesignResources), "ExceptionHandlingSettingsExceptionPoliciesDisplayName")]
        [ConfigurationCollection(typeof(ExceptionPolicyData))]
        [Command(ExceptionHandlingDesignTime.CommandTypeNames.AddExceptionPolicyCommand, CommandPlacement = CommandPlacement.ContextAdd, Replace = CommandReplacement.DefaultAddCommandReplacement)]
        public NamedElementCollection<ExceptionPolicyData> ExceptionPolicies
        {
            get { return (NamedElementCollection<ExceptionPolicyData>)this[policiesProperty]; }
        }

        /// <summary>
        /// Builds a <see cref="ExceptionManager"/> based on the configuration.
        /// </summary>
        /// <returns>An <see cref="ExceptionManager"/>.</returns>
        public ExceptionManager BuildExceptionManager()
        {
            var policies = this.ExceptionPolicies.Select(p => p.BuildExceptionPolicy());

            return new ExceptionManager(policies);
        }

        /// <summary>
        /// Builds an <see cref="ExceptionPolicyDefinition"/> based on the configuration.
        /// </summary>
        /// <param name="policyName">The policy name.</param>
        /// <returns>The policy instance.</returns>
        public ExceptionPolicyDefinition BuildExceptionPolicy(string policyName)
        {
            var policyData = this.ExceptionPolicies.Get(policyName);

            if (policyData == null)
            {
                throw new InvalidOperationException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Resources.ExceptionPolicyNotFoundInConfigurationException,
                        policyName));
            }

            return policyData.BuildExceptionPolicy();
        }
    }
}
