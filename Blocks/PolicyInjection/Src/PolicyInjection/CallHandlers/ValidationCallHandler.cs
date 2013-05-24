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
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.PolicyInjection
{
    /// <summary>
    /// An <see cref="ICallHandler"/> that runs validation of a call's parameters
    /// before calling the target.
    /// </summary>
    [ConfigurationElementType(typeof(ValidationCallHandlerData))]
    public class ValidationCallHandler : ICallHandler
    {
        private readonly string ruleSet;
        private readonly ValidatorFactory validatorFactory;
        private int order;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationCallHandler"/> class that uses the given
        /// rule set and <see cref="SpecificationSource"/> to get the validation rules.
        /// </summary>
        /// <param name="ruleSet">The validation rule set specified in the configuration.</param>
        /// <param name="specificationSource">A value that indicates whether the validation should come from the configuration, from attributes, or from both sources.</param>
        public ValidationCallHandler(string ruleSet, SpecificationSource specificationSource)
            : this(ruleSet, specificationSource, 0)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationCallHandler"/> class that uses the given
        /// rule set, <see cref="SpecificationSource"/> to get the validation rules, and handler order.
        /// </summary>
        /// <param name="ruleSet">The validation rule set specified in the configuration.</param>
        /// <param name="specificationSource">A value that indicates whether the validation should come from the configuration, from attributes, or from both sources.</param>
        /// <param name="handlerOrder">The order of the handler.</param>
        public ValidationCallHandler(string ruleSet, SpecificationSource specificationSource, int handlerOrder)
            : this(ruleSet, GetValidatorFactory(specificationSource), handlerOrder)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationCallHandler"/> class that uses the given
        /// rule set, <see cref="ValidatorFactory"/> to get the validation rules.
        /// </summary>
        /// <param name="ruleSet">The validation rule set specified in the configuration.</param>
        /// <param name="validatorFactory">The <see cref="ValidatorFactory"/> to use when building the validator for the 
        /// type of a parameter, or <see langword="null"/> if no such validator is desired.</param>
        /// <param name="handlerOrder">The order of the handler.</param>
        public ValidationCallHandler(string ruleSet, ValidatorFactory validatorFactory, int handlerOrder)
        {
            this.ruleSet = ruleSet;
            this.validatorFactory = validatorFactory;
            this.order = handlerOrder;
        }

        /// <summary>
        /// Gets or sets the order in which the handler will be executed.
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
        /// <param name="input">The <see cref="IMethodInvocation"/> that contains the details of the current call.</param>
        /// <param name="getNext">The delegate to call to get the next handler in the pipeline.</param>
        /// <returns>The eturn value from the target.</returns>
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

            switch (specificationSource)
            {
                case SpecificationSource.Both:
                    return ValidationFactory.DefaultCompositeValidatorFactory;
                case SpecificationSource.Attributes:
                    return new AttributeValidatorFactory();
                case SpecificationSource.Configuration:
                    return ValidationFactory.DefaultConfigurationValidatorFactory;
                default:
                    return null;
            }
        }

        #region Test methods

        /// <summary>
        /// Gets the rule set for this call handler.
        /// </summary>
        /// <value>The rule set name.</value>
        public string RuleSet
        {
            get { return ruleSet; }
        }

        /// <summary>
        /// Gets the factory used to build validators.
        /// </summary>
        /// <value>The validator factory.</value>
        public ValidatorFactory ValidatorFactory
        {
            get { return this.validatorFactory; }
        }

        #endregion
    }

    /// <summary>
    /// Specifies where the information for validation should come from.
    /// </summary>
    public enum SpecificationSource
    {
        /// <summary>
        /// Configuration and type attributes.
        /// </summary>
        Both = 0,
        /// <summary>
        /// Type attributes only, ignoring configuration.
        /// </summary>
        Attributes,
        /// <summary>
        /// Configuration only, ignoring type attributes.
        /// </summary>
        Configuration,
        /// <summary>
        /// Only use attributes on the parameters themselves; ignore types and configuration.
        /// </summary>
        ParameterAttributesOnly
    }
}
