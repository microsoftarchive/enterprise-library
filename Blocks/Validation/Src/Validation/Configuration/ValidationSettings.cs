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
using System.Configuration;
using System.Linq.Expressions;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration
{
    /// <summary>
    /// Configuration section for stored validation information.
    /// </summary>
    /// <seealso cref="ValidatedTypeReference"/>
    public class ValidationSettings : SerializableConfigurationSection
    {
        ///<summary>
        /// Tries to retrieve the <see cref="ValidationSettings"/> and notifies the provided <see cref="IValidationInstrumentationProvider"/>
        /// if there is a <see cref="ConfigurationErrorsException"/>.  The exception is rethrown.
        ///</summary>
        ///<param name="configurationSource"></param>
        ///<param name="instrumentationProvider"></param>
        ///<returns></returns>
        public static ValidationSettings TryGet(
            IConfigurationSource configurationSource,
            IValidationInstrumentationProvider instrumentationProvider
            )
        {
            try
            {
                return configurationSource.GetSection(ValidationSettings.SectionName) as ValidationSettings;
            }
            catch(ConfigurationErrorsException e)
            {
                instrumentationProvider.NotifyConfigurationFailure(e);
                throw;
            }
        }

        /// <summary>
        /// The name used to serialize the configuration section.
        /// </summary>
        public const string SectionName = BlockSectionNames.Validation;

        private const string TypesPropertyName = "";
        /// <summary>
        /// Gets the collection of types for which validation has been configured.
        /// </summary>
        [ConfigurationProperty(TypesPropertyName, IsDefaultCollection = true)]
        public ValidatedTypeReferenceCollection Types
        {
            get { return (ValidatedTypeReferenceCollection)this[TypesPropertyName]; }
        }
    }
}
