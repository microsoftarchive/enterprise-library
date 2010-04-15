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
using System.Globalization;
using System.Reflection;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF.Properties;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF
{
    /// <summary>
    /// The behavior class that set up the validation contract behavior
    /// for implementing the validation process.
    /// </summary>
    public class ValidationBehavior : IEndpointBehavior, IContractBehavior, IOperationBehavior
    {
        #region ValidationBehavior Members

        private string ruleSet;
        private bool enabled;
        private bool enableClientValidation;

        /// <summary>
        /// Internal use initializer that set the client validation flag.
        /// </summary>
        /// <param name="enabled">if set to <c>true</c> [enabled].</param>
        /// <param name="enableClientValidation">if set to <c>true</c> [enable client validation].</param>
        /// <param name="ruleSet"></param>
        internal ValidationBehavior(bool enabled, bool enableClientValidation, string ruleSet)
        {
            this.enableClientValidation = enableClientValidation;
            this.enabled = enabled;
            this.ruleSet = ruleSet;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ValidationBehavior"/> class.
        /// The <see cref="Enabled"/> property will be set as 'true'.
        /// </summary>
        public ValidationBehavior()
            : this(true, false, string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ValidationBehavior"/> class.
        /// The <see cref="Enabled"/> property will be set to 'true'.
        /// </summary>
        /// <param name="ruleSet">The name of the validation ruleset to apply.</param>
        public ValidationBehavior(string ruleSet)
            : this(true, false, ruleSet)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ValidationBehavior"/> class.
        /// </summary>
        /// <param name="enabled">if set to <c>true</c> [enabled].</param>
        public ValidationBehavior(bool enabled)
            : this(enabled, enabled, string.Empty)
        {
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:ValidationBehavior"/> is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>. The dafault value is true.</value>
        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        #endregion

        #region IEndpointBehavior Members

        /// <summary>
        /// Implement to pass data at runtime to bindings to support custom behavior.
        /// </summary>
        /// <param name="endpoint">The endpoint to modify.</param>
        /// <param name="bindingParameters">The objects that binding elements require to support the behavior.</param>
        public void AddBindingParameters(
            ServiceEndpoint endpoint,
            BindingParameterCollection bindingParameters)
        {
            if (endpoint == null) throw new ArgumentNullException("endpoint");

            AddBindingParameters(endpoint.Contract, endpoint, bindingParameters);
        }

        /// <summary>
        /// Implements a modification or extension of the client across an endpoint.
        /// </summary>
        /// <param name="endpoint">The endpoint that is to be customized.</param>
        /// <param name="clientRuntime">The client runtime to be customized.</param>
        public void ApplyClientBehavior(
            ServiceEndpoint endpoint,
            ClientRuntime clientRuntime)
        {
            if (endpoint == null) throw new ArgumentNullException("endpoint");

            ApplyClientBehavior(endpoint.Contract, endpoint, clientRuntime);
        }

        /// <summary>
        /// Implements a modification or extension of the service across an endpoint.
        /// </summary>
        /// <param name="endpoint">The endpoint that exposes the contract.</param>
        /// <param name="endpointDispatcher">The endpoint dispatcher to be modified or extended.</param>
        public void ApplyDispatchBehavior(
            ServiceEndpoint endpoint,
            EndpointDispatcher endpointDispatcher)
        {
            if (endpoint == null) throw new ArgumentNullException("endpoint");
            if (endpointDispatcher == null) throw new ArgumentNullException("endpointDispatcher");

            ApplyDispatchBehavior(endpoint.Contract, endpoint, endpointDispatcher.DispatchRuntime);
        }

        /// <summary>
        /// Implement to confirm that the endpoint meets some intended criteria.
        /// </summary>
        /// <param name="endpoint">The endpoint to validate.</param>
        public void Validate(ServiceEndpoint endpoint)
        {
            if (endpoint == null) throw new ArgumentNullException("endpoint");

            Validate(endpoint.Contract, endpoint);
        }

        #endregion

        #region IContractBehavior Members

        /// <summary>
        /// Configures any binding elements to support the contract behavior.
        /// </summary>
        /// <param name="contractDescription">The contract description to modify.</param>
        /// <param name="endpoint">The endpoint to modify.</param>
        /// <param name="bindingParameters">The objects that binding elements require to support the behavior.</param>
        public void AddBindingParameters(
            ContractDescription contractDescription,
            ServiceEndpoint endpoint,
            BindingParameterCollection bindingParameters)
        {
        }

        /// <summary>
        /// Implements a modification or extension of the client across a contract.
        /// </summary>
        /// <param name="contractDescription">The contract description for which the extension is intended.</param>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="clientRuntime">The client runtime.</param>
        public void ApplyClientBehavior(
            ContractDescription contractDescription,
            ServiceEndpoint endpoint,
            ClientRuntime clientRuntime)
        {
            if (false == enabled ||
                false == enableClientValidation)
            {
                return;
            }

            // perform validation on client side
            foreach (ClientOperation clientOperation in clientRuntime.Operations)
            {
                OperationDescription operationDescription =
                    contractDescription.Operations.Find(clientOperation.Name);
                ApplyClientBehavior(operationDescription, clientOperation);
            }
        }

        /// <summary>
        /// Implements a modification or extension of the client across a contract.
        /// </summary>
        /// <param name="contractDescription">The contract description to be modified.</param>
        /// <param name="endpoint">The endpoint that exposes the contract.</param>
        /// <param name="dispatchRuntime">The dispatch runtime that controls service execution.</param>
        public void ApplyDispatchBehavior(
            ContractDescription contractDescription,
            ServiceEndpoint endpoint,
            DispatchRuntime dispatchRuntime)
        {
            if (false == enabled)
            {
                return;
            }

            // perform validation on server side.
            // add the faultdescription and validation parameters
            foreach (DispatchOperation dispatchOperation in dispatchRuntime.Operations)
            {
                OperationDescription operationDescription =
                    contractDescription.Operations.Find(dispatchOperation.Name);
                ApplyDispatchBehavior(operationDescription, dispatchOperation);
            }
        }

        /// <summary>
        /// Implement to confirm that the contract and endpoint can support the contract behavior.
        /// </summary>
        /// <param name="contractDescription">The contract to validate.</param>
        /// <param name="endpoint">The endpoint to validate.</param>
        public void Validate(ContractDescription contractDescription, ServiceEndpoint endpoint)
        {
            // by pass validation if this behavior is disabled
            if (false == enabled)
            {
                return;
            }

            // check of all operations with validators has the FaultContract attribute with 
            // a ValidationFault type
            foreach (OperationDescription operation in contractDescription.Operations)
            {
                Validate(operation);
            }
        }

        #endregion

        #region IOperationBehavior Members

        /// <summary>
        /// Configures any binding elements to support the operation behavior.
        /// </summary>
        /// <param name="operationDescription">The operation being examined. Use for examination only. If the operation 
        /// description is modified, the results are undefined.</param>
        /// <param name="bindingParameters">The objects that binding elements require to support the behavior.</param>
        public void AddBindingParameters(OperationDescription operationDescription, BindingParameterCollection bindingParameters)
        {
            // nothing to do
        }

        /// <summary>
        /// Implements a modification or extension of the client accross an operation.
        /// </summary>
        /// <param name="operationDescription">The operation being examined. Use for examination only. If the operation 
        /// description is modified, the results are undefined.</param>
        /// <param name="clientOperation">The run-time object that exposes customization properties for the operation 
        /// described by <paramref name="operationDescription"/>.</param>
        public void ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation)
        {
            clientOperation.ParameterInspectors.Add(new ValidationParameterInspector(operationDescription, ruleSet));
        }

        /// <summary>
        /// Implements a modification or extension of the service accross an operation.
        /// </summary>
        /// <param name="operationDescription">The operation being examined. Use for examination only. If the operation 
        /// description is modified, the results are undefined.</param>
        /// <param name="dispatchOperation">The run-time object that exposes customization properties for the operation 
        /// described by <paramref name="operationDescription"/>.</param>
        public void ApplyDispatchBehavior(OperationDescription operationDescription, DispatchOperation dispatchOperation)
        {
            dispatchOperation.ParameterInspectors.Add(new ValidationParameterInspector(operationDescription, ruleSet));
        }

        /// <summary>
        /// Implement to confirm that the operation meets some intended criteria.
        /// </summary>
        /// <param name="operationDescription">The operation being examined. Use for examination only. If the operation 
        /// description is modified, the results are undefined.</param>
        public void Validate(OperationDescription operationDescription)
        {
            if (HasValidationAssertions(operationDescription) &&
               !HasFaultDescription(operationDescription))
            {
                throw new InvalidOperationException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Resources.MissingFaultDescription,
                        operationDescription.Name));
            }
        }

        #endregion

        private bool HasValidationAssertions(OperationDescription operation)
        {
            MethodInfo methodInfo = operation.SyncMethod;
            if (methodInfo == null)
            {
                throw new ArgumentNullException("operation.SyncMethod");
            }
            return methodInfo.GetCustomAttributes(typeof(ValidatorAttribute), false).Length > 0 ||
                HasParametersWithValidationAssertions(methodInfo.GetParameters());
        }

        private bool HasFaultDescription(OperationDescription operation)
        {
            foreach (FaultDescription fault in operation.Faults)
            {
                if (fault.DetailType == typeof(ValidationFault))
                {
                    return true;
                }
            }
            return false;
        }

        private bool HasParametersWithValidationAssertions(ParameterInfo[] parameters)
        {
            foreach (ParameterInfo parameter in parameters)
            {
                if (parameter.GetCustomAttributes(typeof(ValidatorAttribute), false).Length > 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
