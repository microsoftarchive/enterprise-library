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
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF
{
	/// <summary>
	/// 
	/// </summary>
    public class ValidationParameterInspector : IParameterInspector
    {
        private List<Validator> inputValidators = new List<Validator>();
        private List<string> inputValidatorParameterNames = new List<string>();
		/// <summary>
		/// 
		/// </summary>
		/// <param name="operation"></param>
		/// <param name="ruleSet"></param>
        public ValidationParameterInspector(OperationDescription operation, string ruleSet )
        {
            MethodInfo method = operation.SyncMethod;

            foreach(ParameterInfo param in method.GetParameters())
            {
                switch(param.Attributes)
                {
                case ParameterAttributes.Out:
                case ParameterAttributes.Retval:
                    break;

                default:
                    inputValidators.Add(CreateInputParameterValidator(param, ruleSet));
                    inputValidatorParameterNames.Add(param.Name);
                    break;
                }
            }
        }

        private Validator CreateInputParameterValidator(ParameterInfo param, string ruleSet)
        {
            Validator paramAttributeValidator = ParameterValidatorFactory.CreateValidator(param);
            Validator typeValidator = ValidationFactory.CreateValidator(param.ParameterType, ruleSet);
            return new AndCompositeValidator(paramAttributeValidator, typeValidator);
        }

		/// <summary>
		/// 
		/// </summary>
        public IList<Validator> InputValidators
        {
            get { return inputValidators; }
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="operationName"></param>
		/// <param name="inputs"></param>
		/// <returns></returns>
        public object BeforeCall(string operationName, object[] inputs)
        {
            ValidationFault fault = new ValidationFault();
            for(int i = 0; i < inputValidators.Count; ++i)
            {
                ValidationResults results = inputValidators[i].Validate(inputs[i]);
                AddFaultDetails(fault, inputValidatorParameterNames[i], results);
            }

            if(!fault.IsValid)
            {
                throw new FaultException<ValidationFault>(fault);
            }

            return null;
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="operationName"></param>
		/// <param name="outputs"></param>
		/// <param name="returnValue"></param>
		/// <param name="correlationState"></param>
        public void AfterCall(
            string operationName, object[] outputs, object returnValue, object correlationState)
        {
            // Deliberate noop - we don't need to do anything after the call
        }

        private void AddFaultDetails(ValidationFault fault, string parameterName, ValidationResults results)
        {
            if(!results.IsValid)
            {
                foreach(ValidationResult result in results)
                {
                    fault.Add(CreateValidationDetail(result, parameterName));
                }
            }
        }

        private ValidationDetail CreateValidationDetail(ValidationResult result, string parameterName)
        {
            return new ValidationDetail(result.Message, result.Key, parameterName);
        }
    }
}