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
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.PolicyInjection
{
    /// <summary>
    /// An <see cref="ICallHandler"/> that runs validation of a call's parameters
    /// before calling the target.
    /// </summary>
#if !SILVERLIGHT
    [Common.Configuration.ConfigurationElementType(typeof(ValidationCallHandlerData))]
#endif
    public class ValidationCallHandler : ICallHandler
    {
        private readonly string ruleSet;
        private readonly ValidatorFactory validatorFactory;
        private int order;

        /// <summary>
        /// Creates a <see cref="ValidationCallHandler"/> that uses the given
        /// ruleset and <see cref="SpecificationSource"/> to get the validation rules.
        /// </summary>
        /// <param name="ruleSet">Validation ruleset as specified in configuration.</param>
        /// <param name="specificationSource">Should the validation come from config, attributes, or both?</param>
        public ValidationCallHandler(string ruleSet, SpecificationSource specificationSource)
            : this(ruleSet, specificationSource, 0)
        { }

        /// <summary>
        /// Creates a <see cref="ValidationCallHandler"/> that uses the given
        /// ruleset and <see cref="SpecificationSource"/> to get the validation rules.
        /// </summary>
        /// <param name="ruleSet">Validation ruleset as specified in configuration.</param>
        /// <param name="specificationSource">Should the validation come from config, attributes, or both?</param>
        /// <param name="handlerOrder">Order of the handler</param>
        public ValidationCallHandler(string ruleSet, SpecificationSource specificationSource, int handlerOrder)
            : this(ruleSet, GetValidatorFactory(specificationSource), handlerOrder)
        { }

        /// <summary>
        /// Creates a <see cref="ValidationCallHandler"/> that uses the given
        /// ruleset and <see cref="ValidatorFactory"/> to get the validation rules.
        /// </summary>
        /// <param name="ruleSet">Validation ruleset as specified in configuration.</param>
        /// <param name="validatorFactory">The <see cref="ValidatorFactory"/> to use when building the validator for the 
        /// type of a parameter, or <see langword="null"/> if no such validator is desired.</param>
        /// <param name="handlerOrder">Order of the handler</param>
        public ValidationCallHandler(string ruleSet, ValidatorFactory validatorFactory, int handlerOrder)
        {
            this.ruleSet = ruleSet;
            this.validatorFactory = validatorFactory;
            this.order = handlerOrder;
        }

        /// <summary>
        /// Gets or sets the order in which the handler will be executed
        /// </summary>
        public int Order
        {
            get
            {
                return order;
            }
            set
            {
                order = value;
            }
        }

        /// <summary>
        /// Runs the call handler. This does validation on the parameters, and if validation
        /// passes it calls the handler. It throws <see cref="ArgumentValidationException"/>
        /// if validation fails.
        /// </summary>
        /// <param name="input"><see cref="IMethodInvocation"/> containing details of the current call.</param>
        /// <param name="getNext">delegate to call to get the next handler in the pipeline.</param>
        /// <returns>Return value from the target.</returns>
        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            if (input == null) throw new ArgumentNullException("input");
            if (getNext == null) throw new ArgumentNullException("getNext");

            for (int index = 0; index < input.Inputs.Count; ++index)
            {
                ParameterInfo inputParameter = input.Inputs.GetParameterInfo(index);
                Validator validator = CreateValidator(inputParameter);

                object parameterValue = input.Inputs[index];
                ValidationResults results = validator.Validate(parameterValue);

                if (!results.IsValid)
                {
                    ArgumentValidationException exception =
                        new ArgumentValidationException(results, inputParameter.Name);
                    return input.CreateExceptionMethodReturn(exception);
                }
            }
            return getNext().Invoke(input, getNext);
        }

        private Validator CreateValidator(ParameterInfo parameter)
        {
            Validator typeValidator = CreateValidator(parameter.ParameterType);
            Validator parameterValidator = ParameterValidatorFactory.CreateValidator(parameter);

            if (typeValidator != null)
            {
                return new AndCompositeValidator(typeValidator, parameterValidator);
            }
            return parameterValidator;
        }

        private Validator CreateValidator(Type type)
        {
            return this.validatorFactory != null ? this.validatorFactory.CreateValidator(type, this.ruleSet) : null;
        }

        private static ValidatorFactory GetValidatorFactory(SpecificationSource specificationSource)
        {
            if (specificationSource == SpecificationSource.ParameterAttributesOnly)
            {
                return null;
            }

            var configurationSource = ConfigurationSourceFactory.Create();
#if !SILVERLIGHT
            var instrumentationProvider = ValidationInstrumentationProvider.FromConfigurationSource(configurationSource);
#else
            var instrumentationProvider = new NullValidationInstrumentationProvider();
#endif
            switch (specificationSource)
            {
                case SpecificationSource.Both:
                    return new CompositeValidatorFactory(
                        instrumentationProvider,
                        new AttributeValidatorFactory(instrumentationProvider),
                        new ConfigurationValidatorFactory(configurationSource, instrumentationProvider));
                case SpecificationSource.Attributes:
                    return new AttributeValidatorFactory(instrumentationProvider);
                case SpecificationSource.Configuration:
                    return new ConfigurationValidatorFactory(configurationSource, instrumentationProvider);
                default:
                    return null;
            }
        }

        #region Test methods

        /// <summary>
        /// Gets the ruleset for this call handler.
        /// </summary>
        /// <value>Ruleset name.</value>
        public string RuleSet
        {
            get { return ruleSet; }
        }

        /// <summary>
        /// Factory used to build validators.
        /// </summary>
        public ValidatorFactory ValidatorFactory
        {
            get { return this.validatorFactory; }
        }

        #endregion

    }

    /// <summary>
    /// Where should the information for validation come from?
    /// </summary>
    public enum SpecificationSource
    {
        /// <summary>
        /// Configuration and type attributes
        /// </summary>
        Both = 0,
        /// <summary>
        /// Type attributes only, ignoring configuration
        /// </summary>
        Attributes,
        /// <summary>
        /// Configuration only, ignoring type attributes
        /// </summary>
        Configuration,
        /// <summary>
        /// Only use attributes on the parameters themselves, ignoring types and configuration
        /// </summary>
        ParameterAttributesOnly
    }
}
