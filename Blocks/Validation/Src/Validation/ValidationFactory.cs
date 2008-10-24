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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
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
		private static IDictionary<ValidatorCacheKey, Validator> attributeOnlyValidatorsCache
			= new Dictionary<ValidatorCacheKey, Validator>();
		private static object attributeOnlyValidatorsCacheLock = new object();
		private static IDictionary<ValidatorCacheKey, Validator> attributeAndDefaultConfigurationValidatorsCache
			= new Dictionary<ValidatorCacheKey, Validator>();
		private static object attributeAndDefaultConfigurationValidatorsCacheLock = new object();
		private static IDictionary<ValidatorCacheKey, Validator> defaultConfigurationOnlyValidatorsCache
			= new Dictionary<ValidatorCacheKey, Validator>();
		private static object defaultConfigurationOnlyValidatorsCacheLock = new object();

		/// <summary>
		/// Resets the cached validators.
		/// </summary>
		public static void ResetCaches()
		{
			lock (attributeOnlyValidatorsCacheLock)
			{
				attributeOnlyValidatorsCache.Clear();
			}
			lock (attributeAndDefaultConfigurationValidatorsCacheLock)
			{
				attributeAndDefaultConfigurationValidatorsCache.Clear();
			}
			lock (defaultConfigurationOnlyValidatorsCacheLock)
			{
				defaultConfigurationOnlyValidatorsCache.Clear();
			}
		}

		/// <summary>
		/// Returns a validator representing the validation criteria specified for type <typeparamref name="T"/>
		/// through configuration and attributes on type <typeparamref name="T"/> and its ancestors for the default ruleset.
		/// </summary>
		/// <typeparam name="T">The type to get the validator for.</typeparam>
		/// <returns>The validator.</returns>
		public static Validator<T> CreateValidator<T>()
		{
			return CreateValidator<T>(string.Empty, ConfigurationSourceFactory.Create(), true);
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
			return CreateValidator<T>(ruleset, ConfigurationSourceFactory.Create(), true);
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
			return CreateValidator<T>(string.Empty, configurationSource, false);
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
			return CreateValidator<T>(ruleset, configurationSource, false);
		}

		private static Validator<T> CreateValidator<T>(string ruleset, IConfigurationSource configurationSource, bool cacheValidator)
		{
			Validator<T> wrapperValidator = null;

			if (cacheValidator)
			{
				lock (attributeAndDefaultConfigurationValidatorsCacheLock)
				{
					ValidatorCacheKey key = new ValidatorCacheKey(typeof(T), ruleset, true);

					Validator cachedValidator;
					if (attributeAndDefaultConfigurationValidatorsCache.TryGetValue(key, out cachedValidator))
					{
						return (Validator<T>)cachedValidator;
					}


                    Validator validator = GetValidator(InnerCreateValidatorFromAttributes(typeof(T), ruleset), InnerCreateValidatorFromConfiguration(typeof(T), ruleset, configurationSource));

					wrapperValidator = WrapAndInstrumentValidator<T>(validator, configurationSource);

					attributeAndDefaultConfigurationValidatorsCache[key] = wrapperValidator;
				}
			}
			else
			{
                Validator validator = GetValidator(InnerCreateValidatorFromAttributes(typeof(T), ruleset), InnerCreateValidatorFromConfiguration(typeof(T), ruleset, configurationSource));
				wrapperValidator = WrapAndInstrumentValidator<T>(validator, configurationSource);
			}

			return wrapperValidator;
		}

		/// <summary>
		/// Returns a validator representing the validation criteria specified for type <paramref name="targetType"/>
		/// through configuration and aatributes on type <paramref name="targetType"/> and its ancestors for the default ruleset.
		/// </summary>
		/// <param name="targetType">The type to get the validator for.</param>
		/// <returns>The validator.</returns>
		public static Validator CreateValidator(Type targetType)
		{
			return CreateValidator(targetType, string.Empty);
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
			return CreateValidator(targetType, ruleset, ConfigurationSourceFactory.Create(), true);
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
			return CreateValidator(targetType, ruleset, configurationSource, false);
		}

		private static Validator CreateValidator(Type targetType, string ruleset, IConfigurationSource configurationSource, bool cacheValidator)
		{
			Validator wrapperValidator = null;

            if (cacheValidator)
            {
                lock (attributeAndDefaultConfigurationValidatorsCacheLock)
                {
                    ValidatorCacheKey key = new ValidatorCacheKey(targetType, ruleset, false);

                    Validator cachedValidator;
                    if (attributeAndDefaultConfigurationValidatorsCache.TryGetValue(key, out cachedValidator))
                    {
                        return cachedValidator;
                    }

                    Validator validator = GetValidator(InnerCreateValidatorFromAttributes(targetType, ruleset), InnerCreateValidatorFromConfiguration(targetType, ruleset, configurationSource));

                    wrapperValidator = WrapAndInstrumentValidator(validator, configurationSource);

                    attributeAndDefaultConfigurationValidatorsCache[key] = wrapperValidator;
                }
            }
            else
            {
                Validator validator = GetValidator(InnerCreateValidatorFromAttributes(targetType, ruleset), InnerCreateValidatorFromConfiguration(targetType, ruleset, configurationSource));
                wrapperValidator = WrapAndInstrumentValidator(validator, configurationSource);
            }

            return wrapperValidator;
		}

		/// <summary>
		/// Returns a validator representing the validation criteria specified for type <typeparamref name="T"/>
		/// through attributes on type <typeparamref name="T"/> and its ancestors for the default ruleset.
		/// </summary>
		/// <typeparam name="T">The type to get the validator for.</typeparam>
		/// <returns>The validator.</returns>
		public static Validator<T> CreateValidatorFromAttributes<T>()
		{
			return CreateValidatorFromAttributes<T>(string.Empty);
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
			if (null == ruleset)
			{
				throw new ArgumentNullException("ruleset");
			}

			Validator<T> wrapperValidator = null;

			lock (attributeOnlyValidatorsCacheLock)
			{
				ValidatorCacheKey key = new ValidatorCacheKey(typeof(T), ruleset, true);

				Validator cachedValidator;
				if (attributeOnlyValidatorsCache.TryGetValue(key, out cachedValidator))
				{
					return (Validator<T>)cachedValidator;
				}

				Validator validator = InnerCreateValidatorFromAttributes(typeof(T), ruleset);
				wrapperValidator = WrapAndInstrumentValidator<T>(validator, ConfigurationSourceFactory.Create());

				attributeOnlyValidatorsCache[key] = wrapperValidator;
			}

			return wrapperValidator;
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
			if (null == ruleset)
			{
				throw new ArgumentNullException("ruleset");
			}

			Validator wrapperValidator = null;

			lock (attributeOnlyValidatorsCacheLock)
			{
				ValidatorCacheKey key = new ValidatorCacheKey(targetType, ruleset, false);

				Validator cachedValidator;
				if (attributeOnlyValidatorsCache.TryGetValue(key, out cachedValidator))
				{
					return cachedValidator;
				}

				Validator validator = InnerCreateValidatorFromAttributes(targetType, ruleset);
				wrapperValidator = WrapAndInstrumentValidator(validator, ConfigurationSourceFactory.Create());

				attributeOnlyValidatorsCache[key] = wrapperValidator;
			}

			return wrapperValidator;
		}

		/// <summary>
		/// Returns a validator representing the validation criteria specified for type <typeparamref name="T"/>
		/// through configuration for the default ruleset.
		/// </summary>
		/// <typeparam name="T">The type to get the validator for.</typeparam>
		/// <returns>The validator.</returns>
		public static Validator<T> CreateValidatorFromConfiguration<T>()
		{
			return CreateValidatorFromConfiguration<T>(string.Empty, ConfigurationSourceFactory.Create(), true);
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
			return CreateValidatorFromConfiguration<T>(string.Empty, configurationSource, false);
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
			return CreateValidatorFromConfiguration<T>(ruleset, ConfigurationSourceFactory.Create(), true);
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
			return CreateValidatorFromConfiguration<T>(ruleset, configurationSource, false);
		}

		private static Validator<T> CreateValidatorFromConfiguration<T>(string ruleset, IConfigurationSource configurationSource, bool cacheValidator)
		{

			if (null == ruleset)
			{
				throw new ArgumentNullException("ruleset");
			}

			if (null == configurationSource)
			{
				throw new ArgumentNullException("configurationSource");
			}

			Validator<T> wrapperValidator = null;

			if (cacheValidator)
			{
				lock (defaultConfigurationOnlyValidatorsCacheLock)
				{
					ValidatorCacheKey key = new ValidatorCacheKey(typeof(T), ruleset, true);

					Validator cachedValidator;
					if (defaultConfigurationOnlyValidatorsCache.TryGetValue(key, out cachedValidator))
					{
						return (Validator<T>)cachedValidator;
					}

					Validator validator = InnerCreateValidatorFromConfiguration(typeof(T), ruleset, configurationSource);
					wrapperValidator = WrapAndInstrumentValidator<T>(validator, configurationSource);

					defaultConfigurationOnlyValidatorsCache[key] = wrapperValidator;
				}
			}
			else
			{
				Validator validator = InnerCreateValidatorFromConfiguration(typeof(T), ruleset, configurationSource);
				wrapperValidator = WrapAndInstrumentValidator<T>(validator, configurationSource);
			}

			return wrapperValidator;
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
            return CreateValidatorFromConfiguration(targetType, ruleset, ConfigurationSourceFactory.Create(), true);
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
            return CreateValidatorFromConfiguration(targetType, ruleset, configurationSource, true);
		}

        private static Validator CreateValidatorFromConfiguration(Type targetType, string ruleset, IConfigurationSource configurationSource, bool cacheValidator)
        {

            if (null == ruleset)
            {
                throw new ArgumentNullException("ruleset");
            }

            if (null == configurationSource)
            {
                throw new ArgumentNullException("configurationSource");
            }

            Validator wrapperValidator = null;

            if (cacheValidator)
            {
                lock (defaultConfigurationOnlyValidatorsCacheLock)
                {
                    ValidatorCacheKey key = new ValidatorCacheKey(targetType, ruleset, true);

                    Validator cachedValidator;
                    if (defaultConfigurationOnlyValidatorsCache.TryGetValue(key, out cachedValidator))
                    {
                        return cachedValidator;
                    }

                    Validator validator = InnerCreateValidatorFromConfiguration(targetType, ruleset, configurationSource);
                    wrapperValidator = WrapAndInstrumentValidator(validator, configurationSource);

                    defaultConfigurationOnlyValidatorsCache[key] = wrapperValidator;
                }
            }
            else
            {
                Validator validator = InnerCreateValidatorFromConfiguration(targetType, ruleset, configurationSource);
                wrapperValidator = WrapAndInstrumentValidator(validator, configurationSource);
            }

            return wrapperValidator;
        }

        private static Validator InnerCreateValidatorFromAttributes(Type targetType, string ruleset)
		{
			MetadataValidatorBuilder builder = new MetadataValidatorBuilder();
			Validator validator = builder.CreateValidator(targetType, ruleset);

			return validator;
		}

		private static Validator InnerCreateValidatorFromConfiguration(Type targetType, string ruleset, IConfigurationSource configurationSource)
		{
			ConfigurationValidatorBuilder builder = new ConfigurationValidatorBuilder(configurationSource);
			Validator validator = builder.CreateValidator(targetType, ruleset);

			return validator;
		}

		private static Validator<T> WrapAndInstrumentValidator<T>(Validator validator, IConfigurationSource configurationSource)
		{
			GenericValidatorWrapper<T> validatorWrapper = new GenericValidatorWrapper<T>(validator);
			AttachInstrumentationListener(validatorWrapper, configurationSource);

			return validatorWrapper;
		}

		private static Validator WrapAndInstrumentValidator(Validator validator, IConfigurationSource configurationSource)
		{
			ValidatorWrapper validatorWrapper = new ValidatorWrapper(validator);
			AttachInstrumentationListener(validatorWrapper, configurationSource);

			return validatorWrapper;
		}

		private static void AttachInstrumentationListener(IInstrumentationEventProvider validator, IConfigurationSource configurationSource)
		{
			ValidationInstrumentationListener instrumentationListener = CreateInstrumentationListener(configurationSource);

			if (instrumentationListener.EventLoggingEnabled || instrumentationListener.PerformanceCountersEnabled || instrumentationListener.WmiEnabled)
			{
				ReflectionInstrumentationBinder instrumentationBinder = new ReflectionInstrumentationBinder();
				instrumentationBinder.Bind(validator.GetInstrumentationEventProvider(), instrumentationListener);
			}
		}

		private static ValidationInstrumentationListener CreateInstrumentationListener(IConfigurationSource configurationSource)
		{
			ValidationInstrumentationListenerCustomFactory instrumentationListenerFactory = new ValidationInstrumentationListenerCustomFactory();
			return (ValidationInstrumentationListener)instrumentationListenerFactory.CreateObject(null, null, configurationSource, null);
        }

        #region Private implementation
        private static Validator GetValidator(Validator validatorFromAttributes, Validator validatorFromConfiguration)
        {
            Validator validator = null;

            if (!CheckIfValidatorIsAppropiate(validatorFromAttributes))
            {
                validator = validatorFromConfiguration;
            }
            else if (!CheckIfValidatorIsAppropiate(validatorFromConfiguration))
            {
                validator = validatorFromAttributes;
            }
            else
            {
                validator = new AndCompositeValidator(validatorFromAttributes, validatorFromConfiguration);
            }

            return validator;
        }

        private static bool CheckIfValidatorIsAppropiate(Validator validator)
        {
            if (IsComposite(validator))
            {
                return CompositeHasValidators(validator);
            }
            else
            {
                return true;
            }
        }

        private static bool IsComposite(Validator validator)
        {
            return validator is AndCompositeValidator || validator is OrCompositeValidator;
        }

        private static bool CompositeHasValidators(Validator validator)
        {
            AndCompositeValidator andValidator = validator as AndCompositeValidator;

            if (andValidator != null)
            {
                return ((Validator[])andValidator.Validators).Length > 0;
            }

            OrCompositeValidator orValidator = validator as OrCompositeValidator;

            if (orValidator != null)
            {
                return ((Validator[])orValidator.Validators).Length > 0;
            }

            return false;
        }
        #endregion

        private struct ValidatorCacheKey : IEquatable<ValidatorCacheKey>
		{
			private Type sourceType;
			private string ruleset;
			private bool generic;

			public ValidatorCacheKey(Type sourceType, string ruleset, bool generic)
			{
				this.sourceType = sourceType;
				this.ruleset = ruleset;
				this.generic = generic;
			}

			public override int GetHashCode()
			{
				return this.sourceType.GetHashCode()
					^ (this.ruleset != null ? this.ruleset.GetHashCode() : 0);
			}

			#region IEquatable<ValidatorCacheKey> Members

			bool IEquatable<ValidatorCacheKey>.Equals(ValidatorCacheKey other)
			{
				return (this.sourceType == other.sourceType)
					&& (this.ruleset == null ? other.ruleset == null : this.ruleset.Equals(other.ruleset))
					&& (this.generic == other.generic);
			}

			#endregion
		}
	}
}
