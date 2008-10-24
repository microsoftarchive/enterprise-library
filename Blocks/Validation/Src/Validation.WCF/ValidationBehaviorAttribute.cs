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
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF
{
    /// <summary>
    /// Indicates that an implementation service class will use message validation constraints. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Method,
        Inherited = false, AllowMultiple = false)]
    public class ValidationBehaviorAttribute :
        Attribute, IEndpointBehavior, IContractBehavior, IOperationBehavior
    {
        #region ValidationAttribute Members

        private string ruleSet = string.Empty;
        private IEndpointBehavior endpointBehavior;
        private IContractBehavior contractBehavior;
        private IOperationBehavior operationBehavior;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ValidationAttribute"/> class.
        /// </summary>
        public ValidationBehaviorAttribute()
            : this(string.Empty)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ruleSet"></param>
        public ValidationBehaviorAttribute(string ruleSet)
        {
            this.ruleSet = ruleSet;
            SetBehaviors();
        }

        private void SetBehaviors()
        {
            ValidationBehavior validation = new ValidationBehavior(true, false, ruleSet);
            endpointBehavior = (IEndpointBehavior)validation;
            contractBehavior = (IContractBehavior)validation;
            operationBehavior = (IOperationBehavior)validation;
        }

        /// <summary>
        /// 
        /// </summary>
        public string RuleSet
        {
            get { return ruleSet; }
            set { ruleSet = value; SetBehaviors(); }
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
            endpointBehavior.AddBindingParameters(endpoint, bindingParameters);
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
            endpointBehavior.ApplyClientBehavior(endpoint, clientRuntime);
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
            endpointBehavior.ApplyDispatchBehavior(endpoint, endpointDispatcher);
        }

        /// <summary>
        /// Implement to confirm that the endpoint meets some intended criteria.
        /// </summary>
        /// <param name="endpoint">The endpoint to validate.</param>
        public void Validate(ServiceEndpoint endpoint)
        {
            endpointBehavior.Validate(endpoint);
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
            contractBehavior.AddBindingParameters(contractDescription, endpoint, bindingParameters);
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
            contractBehavior.ApplyClientBehavior(contractDescription, endpoint, clientRuntime);
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
            contractBehavior.ApplyDispatchBehavior(contractDescription, endpoint, dispatchRuntime);
        }

        /// <summary>
        /// Implement to confirm that the contract and endpoint can support the contract behavior.
        /// </summary>
        /// <param name="contractDescription">The contract to validate.</param>
        /// <param name="endpoint">The endpoint to validate.</param>
        public void Validate(ContractDescription contractDescription, ServiceEndpoint endpoint)
        {
            contractBehavior.Validate(contractDescription, endpoint);
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
            operationBehavior.AddBindingParameters(operationDescription, bindingParameters);
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
            operationBehavior.ApplyClientBehavior(operationDescription, clientOperation);
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
            operationBehavior.ApplyDispatchBehavior(operationDescription, dispatchOperation);
        }

        /// <summary>
        /// Implement to confirm that the operation meets some intended criteria.
        /// </summary>
        /// <param name="operationDescription">The operation being examined. Use for examination only. If the operation 
        /// description is modified, the results are undefined.</param>
        public void Validate(OperationDescription operationDescription)
        {
            operationBehavior.Validate(operationDescription);
        }

        #endregion
    }
}
