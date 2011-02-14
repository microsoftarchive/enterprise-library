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

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Instrumentation;
using System;
using System.Windows.Markup;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration
{
    /// <summary>
    /// Configuration section for stored validation information.
    /// </summary>
    /// <seealso cref="ValidatedTypeReference"/>
    [ContentProperty("Types")]
    public class ValidationSettings : ConfigurationSection
    {
        ///<summary>
        /// Tries to retrieve the <see cref="ValidationSettings"/> and notifies the provided <see cref="IValidationInstrumentationProvider"/>
        /// if there is a <see cref="Exception"/>.  The exception is rethrown.
        ///</summary>
        ///<param name="configurationSource"></param>
        ///<param name="instrumentationProvider"></param>
        ///<returns></returns>
        public static ValidationSettings TryGet(
            IConfigurationSource configurationSource,
            IValidationInstrumentationProvider instrumentationProvider
            )
        {
            if (configurationSource == null) throw new ArgumentNullException("configurationSource");
            if (instrumentationProvider == null) throw new ArgumentNullException("instrumentationProvider");

            try
            {
                return configurationSource.GetSection(ValidationSettings.SectionName) as ValidationSettings;
            }
            catch (Exception e)
            {
                instrumentationProvider.NotifyConfigurationFailure(e);
                throw;
            }
        }

        /// <summary>
        /// The name used to serialize the configuration section.
        /// </summary>
        public const string SectionName = BlockSectionNames.Validation;

        private NamedElementCollection<ValidatedTypeReference> types = new NamedElementCollection<ValidatedTypeReference>();
        /// <summary>
        /// Gets the collection of types for which validation has been configured.
        /// </summary>
        public NamedElementCollection<ValidatedTypeReference> Types
        {
            get { return this.types; }
        }
    }
}
