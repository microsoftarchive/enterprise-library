//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
//===============================================================================
// Copyright © Microsoft Corporation. All rights reserved.
// Adapted from ACA.NET with permission from Avanade Inc.
// ACA.NET copyright © Avanade Inc. All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
{
    /// <summary>
    /// <para>Represents a view for navigating the <see cref="ExceptionHandlingSettings"/> configuration data.</para>
    /// </summary>
    public class ExceptionHandlingConfigurationView
    {
        IConfigurationSource configurationSource;

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="ExceptionHandlingConfigurationView"/> class with an <see cref="Common.Configuration.IConfigurationSource"/> object.</para>
        /// </summary>
        /// <param name="configurationSource">
        /// <para>An <see cref="Common.Configuration.IConfigurationSource"/> object.</para>
        /// </param>
        public ExceptionHandlingConfigurationView(IConfigurationSource configurationSource)
        {
            if (configurationSource == null) throw new ArgumentNullException("configurationSource");

            this.configurationSource = configurationSource;
        }

        /// <summary>
        /// <para>Gets the <see cref="ExceptionHandlingSettings"/> configuration data.</para>
        /// </summary>
        /// <returns>
        /// <para>The <see cref="ExceptionHandlingSettings"/> configuration data.</para>
        /// </returns>
        public ExceptionHandlingSettings ExceptionHandlingSettings
        {
            get { return ExceptionHandlingSettings.GetExceptionHandlingSettings(configurationSource); }
        }

        /// <summary>
        /// <para>Gets the <see cref="ExceptionPolicyData"/> from configuration by name.</para>
        /// </summary>
        /// <param name="policyName">
        /// <para>The name of the policy in configuration.</para>
        /// </param>
        /// <returns>
        /// <para>An <see cref="ExceptionPolicyData"/> object.</para>
        /// </returns>
        public ExceptionPolicyData GetExceptionPolicyData(string policyName)
        {
            if (string.IsNullOrEmpty(policyName)) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty);

            ExceptionHandlingSettings settings = ExceptionHandlingSettings;
            if (settings.ExceptionPolicies.Contains(policyName))
            {
                return settings.ExceptionPolicies.Get(policyName);
            }
            throw new ConfigurationErrorsException(string.Format(Resources.Culture, Resources.ExceptionSimpleProviderNotFound, policyName));
        }
    }
}