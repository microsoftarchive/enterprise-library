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
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Validation
{
    /// <summary>
    /// Factory for creating <see cref="Validator"/> objects for types.
    /// </summary>
    /// <seealso cref="Validation"/>
    /// <seealso cref="Validator"/>
    public static class ValidationFactory
    {
        /// <summary>
        /// Resets the cached validators.
        /// </summary>
        public static void ResetCaches()
        {
            DefaultCompositeValidatorFactory.ResetCache();
        }

        /// <summary>
        /// Returns a validator representing the validation criteria specified for type <typeparamref name="T"/>
        /// through configuration and attributes on type <typeparamref name="T"/> and its ancestors for the default ruleset.
        /// </summary>
        /// <typeparam name="T">The type to get the validator for.</typeparam>
        /// <returns>The validator.</returns>
        public static Validator<T> CreateValidator<T>()
        {
            return DefaultCompositeValidatorFactory.CreateValidator<T>();
        }

        /// <summary>
        /// Returns a validator representing the validation criteria specified for type <typeparamref name="T"/>
        /// through configuration and attributes on type <typeparamref name="T"/> and its ancestors for the supplied ruleset.
        /// </summary>
        /// <typeparam name="T">The type to get the validator for.</typeparam>
        /// <param name="ruleset">The name of the required ruleset.</param>
        /// <returns>The validator.</returns>
        /// <exception cref="ArgumentNullException">when the <paramref name="ruleset"/> is <see langword="null"/>.</exception>
        public static Validator<T> CreateValidator<T>(string ruleset)
        {
            return DefaultCompositeValidatorFactory.CreateValidator<T>(ruleset);
        }

        /// <summary>
        /// Returns a validator representing the validation criteria specified for type <typeparamref name="T"/>
        /// through configuration and attributes on type <typeparamref name="T"/> and its ancestors for the default ruleset
        /// retrieving configuration information from the supplied <see cref="IConfigurationSource"/>.
        /// </summary>
        /// <typeparam name="T">The type to get the validator for.</typeparam>
        /// <param name="configurationSource">The configuration source from where configuration information is to be retrieved.</param>
        /// <returns>The validator.</returns>
        /// <exception cref="ArgumentNullException">when the <paramref name="configurationSource"/> is <see langword="null"/>.</exception>
        public static Validator<T> CreateValidator<T>(IConfigurationSource configurationSource)
        {
            return CreateValidator<T>(configurationSource, ValidationSpecificationSource.All);
        }

        /// <summary>
        /// Returns a validator representing the validation criteria specified for type <typeparamref name="T"/>
        /// through configuration and attributes on type <typeparamref name="T"/> and its ancestors for the supplied ruleset
        /// retrieving configuration information from the supplied <see cref="IConfigurationSource"/>.
        /// </summary>
        /// <typeparam name="T">The type to get the validator for.</typeparam>
        /// <param name="ruleset">The name of the required ruleset.</param>
        /// <param name="configurationSource">The configuration source from where configuration information is to be retrieved.</param>
        /// <returns>The validator.</returns>
        /// <exception cref="ArgumentNullException">when the <paramref name="ruleset"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">when the <paramref name="configurationSource"/> is <see langword="null"/>.</exception>
        public static Validator<T> CreateValidator<T>(string ruleset, IConfigurationSource configurationSource)
        {
            return CreateValidator<T>(ruleset, configurationSource, ValidationSpecificationSource.All);
        }

        /// <summary>
        /// Returns a validator representing the validation criteria specified for type <paramref name="targetType"/>
        /// through configuration and aatributes on type <paramref name="targetType"/> and its ancestors for the default ruleset.
        /// </summary>
        /// <param name="targetType">The type to get the validator for.</param>
        /// <returns>The validator.</returns>
        public static Validator CreateValidator(Type targetType)
        {
            return DefaultCompositeValidatorFactory.CreateValidator(targetType);
        }

        /// <summary>
        /// Returns a validator representing the validation criteria specified for type <paramref name="targetType"/>
        /// through configuration and attributes on type <paramref name="targetType"/> and its ancestors for the supplied ruleset.
        /// </summary>
        /// <param name="targetType">The type to get the validator for.</param>
        /// <param name="ruleset">The name of the required ruleset.</param>
        /// <returns>The validator.</returns>
        /// <exception cref="ArgumentNullException">when the <paramref name="ruleset"/> is <see langword="null"/>.</exception>
        public static Validator CreateValidator(Type targetType, string ruleset)
        {
            return DefaultCompositeValidatorFactory.CreateValidator(targetType, ruleset);
        }

        /// <summary>
        /// Returns a validator representing the validation criteria specified for type <paramref name="targetType"/>
        /// through configuration and attributes on type <paramref name="targetType"/> and its ancestors for the supplied ruleset
        /// retrieving configuration information from the supplied <see cref="IConfigurationSource"/>.
        /// </summary>
        /// <param name="targetType">The type to get the validator for.</param>
        /// <param name="ruleset">The name of the required ruleset.</param>
        /// <param name="configurationSource">The configuration source from where configuration information is to be retrieved.</param>
        /// <returns>The validator.</returns>
        /// <exception cref="ArgumentNullException">when the <paramref name="ruleset"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">when the <paramref name="configurationSource"/> is <see langword="null"/>.</exception>
        public static Validator CreateValidator(Type targetType, string ruleset, IConfigurationSource configurationSource)
        {
            return CreateValidator(targetType, ruleset, configurationSource, ValidationSpecificationSource.All);
        }

        /// <summary>
        /// Returns a validator representing the validation criteria specified for type <typeparamref name="T"/>
        /// through configuration and attributes on type <typeparamref name="T"/> and its ancestors for the default ruleset.
        /// </summary>
        /// <typeparam name="T">The type to get the validator for.</typeparam>
        /// <param name="source">The source of validation information.</param>
        /// <returns>The validator.</returns>
        public static Validator<T> CreateValidator<T>(ValidationSpecificationSource source)
        {
            return GetValidatorFactory(source).CreateValidator<T>();
        }

        /// <summary>
        /// Returns a validator representing the validation criteria specified for type <typeparamref name="T"/>
        /// through configuration and attributes on type <typeparamref name="T"/> and its ancestors for the supplied ruleset.
        /// </summary>
        /// <typeparam name="T">The type to get the validator for.</typeparam>
        /// <param name="ruleset">The name of the required ruleset.</param>
        /// <param name="source">The source of validation information.</param>
        /// <returns>The validator.</returns>
        /// <exception cref="ArgumentNullException">when the <paramref name="ruleset"/> is <see langword="null"/>.</exception>
        public static Validator<T> CreateValidator<T>(string ruleset, ValidationSpecificationSource source)
        {
            return GetValidatorFactory(source).CreateValidator<T>(ruleset);
        }

        /// <summary>
        /// Returns a validator representing the validation criteria specified for type <typeparamref name="T"/>
        /// through configuration and attributes on type <typeparamref name="T"/> and its ancestors for the default ruleset
        /// retrieving configuration information from the supplied <see cref="IConfigurationSource"/>.
        /// </summary>
        /// <typeparam name="T">The type to get the validator for.</typeparam>
        /// <param name="configurationSource">The configuration source from where configuration information is to be retrieved.</param>
        /// <param name="source">The source of validation information.</param>
        /// <returns>The validator.</returns>
        /// <exception cref="ArgumentNullException">when the <paramref name="configurationSource"/> is <see langword="null"/>.</exception>
        public static Validator<T> CreateValidator<T>(IConfigurationSource configurationSource, ValidationSpecificationSource source)
        {
            return GetValidatorFactory(configurationSource, source).CreateValidator<T>();
        }

        /// <summary>
        /// Returns a validator representing the validation criteria specified for type <typeparamref name="T"/>
        /// through configuration and attributes on type <typeparamref name="T"/> and its ancestors for the supplied ruleset
        /// retrieving configuration information from the supplied <see cref="IConfigurationSource"/>.
        /// </summary>
        /// <typeparam name="T">The type to get the validator for.</typeparam>
        /// <param name="ruleset">The name of the required ruleset.</param>
        /// <param name="configurationSource">The configuration source from where configuration information is to be retrieved.</param>
        /// <param name="source">The source of validation information.</param>
        /// <returns>The validator.</returns>
        /// <exception cref="ArgumentNullException">when the <paramref name="ruleset"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">when the <paramref name="configurationSource"/> is <see langword="null"/>.</exception>
        public static Validator<T> CreateValidator<T>(string ruleset, IConfigurationSource configurationSource, ValidationSpecificationSource source)
        {
            return GetValidatorFactory(configurationSource, source).CreateValidator<T>(ruleset);
        }

        /// <summary>
        /// Returns a validator representing the validation criteria specified for type <paramref name="targetType"/>
        /// through configuration and aatributes on type <paramref name="targetType"/> and its ancestors for the default ruleset.
        /// </summary>
        /// <param name="targetType">The type to get the validator for.</param>
        /// <param name="source">The source of validation information.</param>
        /// <returns>The validator.</returns>
        public static Validator CreateValidator(Type targetType, ValidationSpecificationSource source)
        {
            return GetValidatorFactory(source).CreateValidator(targetType);
        }

        /// <summary>
        /// Returns a validator representing the validation criteria specified for type <paramref name="targetType"/>
        /// through configuration and attributes on type <paramref name="targetType"/> and its ancestors for the supplied ruleset.
        /// </summary>
        /// <param name="targetType">The type to get the validator for.</param>
        /// <param name="ruleset">The name of the required ruleset.</param>
        /// <param name="source">The source of validation information.</param>
        /// <returns>The validator.</returns>
        /// <exception cref="ArgumentNullException">when the <paramref name="ruleset"/> is <see langword="null"/>.</exception>
        public static Validator CreateValidator(Type targetType, string ruleset, ValidationSpecificationSource source)
        {
            return GetValidatorFactory(source).CreateValidator(targetType, ruleset);
        }

        /// <summary>
        /// Returns a validator representing the validation criteria specified for type <paramref name="targetType"/>
        /// through configuration and attributes on type <paramref name="targetType"/> and its ancestors for the supplied ruleset
        /// retrieving configuration information from the supplied <see cref="IConfigurationSource"/>.
        /// </summary>
        /// <param name="targetType">The type to get the validator for.</param>
        /// <param name="ruleset">The name of the required ruleset.</param>
        /// <param name="configurationSource">The configuration source from where configuration information is to be retrieved.</param>
        /// <param name="source">The source of validation information.</param>
        /// <returns>The validator.</returns>
        /// <exception cref="ArgumentNullException">when the <paramref name="ruleset"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">when the <paramref name="configurationSource"/> is <see langword="null"/>.</exception>
        public static Validator CreateValidator(Type targetType, string ruleset, IConfigurationSource configurationSource, ValidationSpecificationSource source)
        {
            return GetValidatorFactory(configurationSource, source).CreateValidator(targetType, ruleset);
        }

        /// <summary>
        /// Returns a validator representing the validation criteria specified for type <typeparamref name="T"/>
        /// through attributes on type <typeparamref name="T"/> and its ancestors for the default ruleset.
        /// </summary>
        /// <typeparam name="T">The type to get the validator for.</typeparam>
        /// <returns>The validator.</returns>
        public static Validator<T> CreateValidatorFromAttributes<T>()
        {
            return DefaultAttributeValidatorFactory.CreateValidator<T>();
        }

        /// <summary>
        /// Returns a validator representing the validation criteria specified for type <typeparamref name="T"/>
        /// through attributes on type <typeparamref name="T"/> and its ancestors for the supplied ruleset.
        /// </summary>
        /// <typeparam name="T">The type to get the validator for.</typeparam>
        /// <param name="ruleset">The name of the required ruleset.</param>
        /// <returns>The validator.</returns>
        /// <exception cref="ArgumentNullException">when the <paramref name="ruleset"/> is <see langword="null"/>.</exception>
        public static Validator<T> CreateValidatorFromAttributes<T>(string ruleset)
        {
            return DefaultAttributeValidatorFactory.CreateValidator<T>(ruleset);
        }

        /// <summary>
        /// Returns a validator representing the validation criteria specified for type <paramref name="targetType"/>
        /// through attributes on type <paramref name="targetType"/> and its ancestors for the supplied ruleset.
        /// </summary>
        /// <param name="targetType">The type to get the validator for.</param>
        /// <param name="ruleset">The name of the required ruleset.</param>
        /// <returns>The validator.</returns>
        /// <exception cref="ArgumentNullException">when the <paramref name="ruleset"/> is <see langword="null"/>.</exception>
        public static Validator CreateValidatorFromAttributes(Type targetType, string ruleset)
        {
            return DefaultAttributeValidatorFactory.CreateValidator(targetType, ruleset);
        }

        /// <summary>
        /// Returns a validator representing the validation criteria specified for type <typeparamref name="T"/>
        /// through configuration for the default ruleset.
        /// </summary>
        /// <typeparam name="T">The type to get the validator for.</typeparam>
        /// <returns>The validator.</returns>
        public static Validator<T> CreateValidatorFromConfiguration<T>()
        {
            return DefaultConfigurationValidatorFactory.CreateValidator<T>();
        }

        /// <summary>
        /// Returns a validator representing the validation criteria specified for type <typeparamref name="T"/>
        /// through configuration for the default ruleset
        /// retrieving configuration information from the supplied <see cref="IConfigurationSource"/>.
        /// </summary>
        /// <typeparam name="T">The type to get the validator for.</typeparam>
        /// <param name="configurationSource">The configuration source from where configuration information is to be retrieved.</param>
        /// <returns>The validator.</returns>
        /// <exception cref="ArgumentNullException">when the <paramref name="configurationSource"/> is <see langword="null"/>.</exception>
        public static Validator<T> CreateValidatorFromConfiguration<T>(IConfigurationSource configurationSource)
        {
            var validationFactory = ConfigurationValidatorFactory.FromConfigurationSource(configurationSource);

            return validationFactory.CreateValidator<T>();
        }

        /// <summary>
        /// Returns a validator representing the validation criteria specified for type <typeparamref name="T"/>
        /// through configuration for the supplied ruleset.
        /// </summary>
        /// <typeparam name="T">The type to get the validator for.</typeparam>
        /// <param name="ruleset">The name of the required ruleset.</param>
        /// <returns>The validator.</returns>
        /// <exception cref="ArgumentNullException">when the <paramref name="ruleset"/> is <see langword="null"/>.</exception>
        public static Validator<T> CreateValidatorFromConfiguration<T>(string ruleset)
        {
            return DefaultConfigurationValidatorFactory.CreateValidator<T>(ruleset);
        }

        /// <summary>
        /// Returns a validator representing the validation criteria specified for type <typeparamref name="T"/>
        /// through configuration for the supplied ruleset
        /// retrieving configuration information from the supplied <see cref="IConfigurationSource"/>.
        /// </summary>
        /// <typeparam name="T">The type to get the validator for.</typeparam>
        /// <param name="ruleset">The name of the required ruleset.</param>
        /// <param name="configurationSource">The configuration source from where configuration information is to be retrieved.</param>
        /// <returns>The validator.</returns>
        /// <exception cref="ArgumentNullException">when the <paramref name="ruleset"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">when the <paramref name="configurationSource"/> is <see langword="null"/>.</exception>
        public static Validator<T> CreateValidatorFromConfiguration<T>(string ruleset, IConfigurationSource configurationSource)
        {
            var validationFactory = ConfigurationValidatorFactory.FromConfigurationSource(configurationSource);
            return validationFactory.CreateValidator<T>(ruleset);
        }

        /// <summary>
        /// Returns a validator representing the validation criteria specified for type <paramref name="targetType"/>
        /// through configuration for the supplied ruleset, retrieving configuration information from
        /// the default configuration source.
        /// </summary>
        /// <param name="targetType">The type to get the validator for.</param>
        /// <param name="ruleset">The name of the validation ruleset.</param>
        /// <returns>The validator.</returns>
        public static Validator CreateValidatorFromConfiguration(Type targetType, string ruleset)
        {
            return DefaultConfigurationValidatorFactory.CreateValidator(targetType, ruleset);
        }

        /// <summary>
        /// Returns a validator representing the validation criteria specified for type <paramref name="targetType"/>
        /// through configuration for the supplied ruleset
        /// retrieving configuration information from the supplied <see cref="IConfigurationSource"/>.
        /// </summary>
        /// <param name="targetType">The type to get the validator for.</param>
        /// <param name="ruleset">The name of the required ruleset.</param>
        /// <param name="configurationSource">The configuration source from where configuration information is to be retrieved.</param>
        /// <returns>The validator.</returns>
        /// <exception cref="ArgumentNullException">when the <paramref name="ruleset"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">when the <paramref name="configurationSource"/> is <see langword="null"/>.</exception>
        public static Validator CreateValidatorFromConfiguration(Type targetType, string ruleset, IConfigurationSource configurationSource)
        {
            var validationFactory = ConfigurationValidatorFactory.FromConfigurationSource(configurationSource);

            return validationFactory.CreateValidator(targetType, ruleset);
        }

        private static CompositeValidatorFactory CreateCompositeValidatorFactory(
            IValidationInstrumentationProvider instrumentationProvider,
            IEnumerable<ValidatorFactory> validatorFactories)
        {
            return new CompositeValidatorFactory(
                instrumentationProvider,
                validatorFactories.ToArray());
        }

        private static ValidatorFactory GetValidatorFactory(ValidationSpecificationSource source)
        {
            if (source == ValidationSpecificationSource.All || source == ValidationSpecificationSource.Both)
            {
                return DefaultCompositeValidatorFactory;
            }

            List<ValidatorFactory> factories = new List<ValidatorFactory>();
            if (source.IsSet(ValidationSpecificationSource.Attributes))
            {
                factories.Add(DefaultAttributeValidatorFactory);
            }
            if (source.IsSet(ValidationSpecificationSource.Configuration))
            {
                factories.Add(DefaultConfigurationValidatorFactory);
            }
            if (source.IsSet(ValidationSpecificationSource.DataAnnotations))
            {
                factories.Add(DefaultValidationAttributeValidatorFactory);
            }

            if (factories.Count == 1)
            {
                return factories[0];
            }

            return CreateCompositeValidatorFactory(
                EnterpriseLibraryContainer.Current.GetInstance<IValidationInstrumentationProvider>(),
                factories);
        }

        private static ValidatorFactory GetValidatorFactory(
            IConfigurationSource configurationSource,
            ValidationSpecificationSource source)
        {
#if !SILVERLIGHT    // todo remove when including other sources
            var instrumentationProvider = ValidationInstrumentationProvider.FromConfigurationSource(configurationSource);
#else
            var instrumentationProvider = new NullValidationInstrumentationProvider();
#endif

            List<ValidatorFactory> factories = new List<ValidatorFactory>();
            if (source.IsSet(ValidationSpecificationSource.Attributes))
            {
                factories.Add(DefaultAttributeValidatorFactory);
            }
            if (source.IsSet(ValidationSpecificationSource.Configuration))
            {
                factories.Add(new ConfigurationValidatorFactory(configurationSource, instrumentationProvider));
            }
            if (source.IsSet(ValidationSpecificationSource.DataAnnotations))
            {
                factories.Add(DefaultValidationAttributeValidatorFactory);
            }

            if (factories.Count == 1)
            {
                return factories[0];
            }

            return CreateCompositeValidatorFactory(instrumentationProvider, factories);
        }

        private static AttributeValidatorFactory DefaultAttributeValidatorFactory
        {
            get { return EnterpriseLibraryContainer.Current.GetInstance<AttributeValidatorFactory>(); }
        }

        private static ConfigurationValidatorFactory DefaultConfigurationValidatorFactory
        {
            get { return EnterpriseLibraryContainer.Current.GetInstance<ConfigurationValidatorFactory>(); }
        }

        private static ValidationAttributeValidatorFactory DefaultValidationAttributeValidatorFactory
        {
            get { return EnterpriseLibraryContainer.Current.GetInstance<ValidationAttributeValidatorFactory>(); }
        }

        /// <summary>
        /// Gets the <see cref="ValidatorFactory"/> to use by default.
        /// </summary>
        public static ValidatorFactory DefaultCompositeValidatorFactory
        {
            get { return EnterpriseLibraryContainer.Current.GetInstance<ValidatorFactory>(); }
        }
    }
}
