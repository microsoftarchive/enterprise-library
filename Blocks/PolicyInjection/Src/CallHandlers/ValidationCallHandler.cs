//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Policy Injection Application Block
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
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers
{
    /// <summary>
    /// An <see cref="ICallHandler"/> that runs validation of a call's parameters
    /// before calling the target.
    /// </summary>
    [ConfigurationElementType(typeof(ValidationCallHandlerData))]
    public class ValidationCallHandler : ICallHandler
    {
        string ruleSet;
        SpecificationSource specificationSource;
        private int order = 0;

        /// <summary>
        /// Creates a <see cref="ValidationCallHandler"/> that uses the given
        /// ruleset and <see cref="SpecificationSource"/> to get the validation rules.
        /// </summary>
        /// <param name="ruleSet">Validation ruleset as specified in configuration.</param>
        /// <param name="specificationSource">Should the validation come from config, attributes, or both?</param>
        public ValidationCallHandler(string ruleSet, SpecificationSource specificationSource)
        {
            this.ruleSet = ruleSet;
            this.specificationSource = specificationSource;
        }

        /// <summary>
        /// Creates a <see cref="ValidationCallHandler"/> that uses the given
        /// ruleset and <see cref="SpecificationSource"/> to get the validation rules.
        /// </summary>
        /// <param name="ruleSet">Validation ruleset as specified in configuration.</param>
        /// <param name="specificationSource">Should the validation come from config, attributes, or both?</param>
        /// <param name="handlerOrder">Order of the handler</param>
        public ValidationCallHandler(string ruleSet, SpecificationSource specificationSource, int handlerOrder)
        {
            this.ruleSet = ruleSet;
            this.specificationSource = specificationSource;
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
            switch (specificationSource)
            {
                case SpecificationSource.Both:
                    return ValidationFactory.CreateValidator(type, ruleSet);
                case SpecificationSource.Attributes:
                    return ValidationFactory.CreateValidatorFromAttributes(type, ruleSet);
                case SpecificationSource.Configuration:
                    return ValidationFactory.CreateValidatorFromConfiguration(type, ruleSet);
                case SpecificationSource.ParameterAttributesOnly:
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
        /// Source for validation information.
        /// </summary>
        /// <value>validation source.</value>
        public SpecificationSource SpecificationSource
        {
            get { return specificationSource; }
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
