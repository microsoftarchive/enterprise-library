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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Validation
{
    ///<summary>
    /// A <see cref="Validator"/> factory producing validators from rules specified in a configuration file.
    ///</summary>
    /// <seealso cref="ValidatorFactory"/>
    public class ConfigurationValidatorFactory : ValidatorFactory
    {
        ///<summary>
        /// Creates a <see cref="ConfigurationValidatorFactory"/> based on a configuration source.
        ///</summary>
        ///<param name="configurationSource"></param>
        ///<returns>A new ConfigurationValidatorFactory</returns>
        public static ConfigurationValidatorFactory FromConfigurationSource(IConfigurationSource configurationSource)
        {
            var instrumentationProvider =
#if !SILVERLIGHT    // todo remove when including other sources
                ValidationInstrumentationProvider.FromConfigurationSource(configurationSource);
#else
                new NullValidationInstrumentationProvider();
#endif
            return new ConfigurationValidatorFactory(configurationSource, instrumentationProvider);
        }

        ///<summary>
        /// Initializes a <see cref="ConfigurationValidatorFactory"/>.
        ///</summary>
        ///<param name="configurationSource">The configuration source containing the validation rules to create validators from.</param>
        ///<param name="instrumentationProvider">The <see cref="IValidationInstrumentationProvider"/> provider to use for instrumentation purposes.</param>
        public ConfigurationValidatorFactory(IConfigurationSource configurationSource, IValidationInstrumentationProvider instrumentationProvider)
            : base(instrumentationProvider)
        {
            ConfigurationSource = configurationSource;
        }

        ///<summary>
        /// The <see cref="IConfigurationSource"/> the factory uses for determining validation rules.
        ///</summary>
        public IConfigurationSource ConfigurationSource { get; private set; }

        /// <summary>
        /// Creates the validator for the specified target and ruleset.
        /// </summary>
        /// <param name="targetType">The <see cref="Type"/>to validate.</param>
        /// <param name="ruleset">The ruleset to use when validating</param>
        /// <param name="mainValidatorFactory">Factory to use when building nested validators.</param>
        /// <returns>A <see cref="Validator"/></returns>
        protected internal override Validator InnerCreateValidator(Type targetType, string ruleset, ValidatorFactory mainValidatorFactory)
        {
            ConfigurationValidatorBuilder builder =
                new ConfigurationValidatorBuilder(
                    ValidationSettings.TryGet(ConfigurationSource, InstrumentationProvider),
                    InstrumentationProvider,
                    MemberAccessValidatorBuilderFactory.Default,
                    mainValidatorFactory);

            return builder.CreateValidator(targetType, ruleset);
        }
    }
}
