//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;
using System.Web.Services.Protocols;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF
{
    /// <summary>
    /// Indicates that an implementation service class will use exception shielding. 
    /// </summary>
    /// <remarks>
    /// Add this attribute to your service implementation class or your service contract interface 
    /// and configure your host configuration file to use the Enterprise Library Exception Handling 
    /// Application Block adding the <see cref="FaultContractExceptionHandler"/> class to the 
    /// exceptionHandlers collection and set your FaultContract type that maps to a particular exception.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface,
        Inherited = false, AllowMultiple = false)]
    public class ExceptionShieldingAttribute : 
        Attribute, IServiceBehavior, IContractBehavior, IErrorHandler
    {
        #region ExceptionShieldingAttribute Members 

        private string exceptionPolicyName;
        private IServiceBehavior serviceBehavior;
        private IContractBehavior contractBehavior;
        private IErrorHandler errorHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ExceptionShieldingAttribute"/> class.
        /// </summary>
        public ExceptionShieldingAttribute()
            : this(ExceptionShielding.DefaultExceptionPolicy)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ExceptionShieldingAttribute"/> class.
        /// </summary>
        /// <param name="exceptionPolicyName">Name of the exception policy.</param>
        public ExceptionShieldingAttribute(string exceptionPolicyName)
        {
            this.exceptionPolicyName = exceptionPolicyName;
            
            //The ServiceHost applies behaviors in the following order:
            // Contract
            // Operation
            // Endpoint
            // Service

            ExceptionShieldingBehavior behavior = new ExceptionShieldingBehavior(exceptionPolicyName);
            this.contractBehavior = (IContractBehavior)behavior;
            this.serviceBehavior = (IServiceBehavior)behavior;
            this.errorHandler = new ExceptionShieldingErrorHandler(exceptionPolicyName);
        }

        /// <summary>
        /// Gets or sets the name of the exception policy.
        /// </summary>
        /// <value>The name of the exception policy.</value>
        public string ExceptionPolicyName
        {
            get { return exceptionPolicyName; }
            set
            {
                if(string.IsNullOrEmpty(value))
                {
                    value = ExceptionShielding.DefaultExceptionPolicy;
                }
                exceptionPolicyName = value;
            }
        }

        #endregion

        #region IServiceBehavior Members

        /// <summary>
        /// Validates the specified description.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <param name="serviceHostBase">The service host base.</param>
        public void Validate(ServiceDescription description, ServiceHostBase serviceHostBase)
        {
            this.serviceBehavior.Validate(description, serviceHostBase);
        }

        /// <summary>
        /// Adds the binding parameters.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <param name="serviceHostBase">The service host base.</param>
        /// <param name="endpoints">The endpoints.</param>
        /// <param name="parameters">The parameters.</param>
        public void AddBindingParameters(ServiceDescription description,
            ServiceHostBase serviceHostBase,
            System.Collections.ObjectModel.Collection<ServiceEndpoint> endpoints,
            System.ServiceModel.Channels.BindingParameterCollection parameters)
        {
            this.serviceBehavior.AddBindingParameters(description, serviceHostBase, endpoints, parameters);
        }

        /// <summary>
        /// Applies the dispatch behavior.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <param name="serviceHostBase">The service host base.</param>
        public void ApplyDispatchBehavior(ServiceDescription description, ServiceHostBase serviceHostBase)
        {
            this.serviceBehavior.ApplyDispatchBehavior(description, serviceHostBase);
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
            this.contractBehavior.AddBindingParameters(contractDescription, endpoint, bindingParameters);
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
            this.contractBehavior.ApplyClientBehavior(contractDescription, endpoint, clientRuntime);
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
             this.contractBehavior.ApplyDispatchBehavior(contractDescription, endpoint, dispatchRuntime);
        }

        /// <summary>
        /// Implement to confirm that the contract and endpoint can support the contract behavior.
        /// </summary>
        /// <param name="contractDescription">The contract to validate.</param>
        /// <param name="endpoint">The endpoint to validate.</param>
        public void Validate(ContractDescription contractDescription, ServiceEndpoint endpoint)
        {
            this.contractBehavior.Validate(contractDescription, endpoint);
        }

        #endregion

        #region IErrorHandler Members

        /// <summary>
        /// Enables error-related processing and returns a value that indicates whether subsequent HandleError implementations are called.
        /// </summary>
        /// <param name="error">The exception thrown during processing.</param>
        /// <returns>
        /// true if subsequent <see cref="T:System.ServiceModel.Dispatcher.IErrorHandler"></see> implementations must not be called; otherwise, false. The default is false.
        /// </returns>
        public bool HandleError(Exception error)
        {
            return this.errorHandler.HandleError(error);
        }

        /// <summary>
        /// Enables the creation of a custom <see cref="T:System.ServiceModel.FaultException`1"></see> that is returned from an exception in the course of a service method.
        /// </summary>
        /// <param name="error">The <see cref="T:System.Exception"></see> object thrown in the course of the service operation.</param>
        /// <param name="version">The SOAP version of the message.</param>
        /// <param name="fault">The <see cref="T:System.ServiceModel.Channels.Message"></see> object that is returned to the client, or service, in the duplex case.</param>
        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
            this.errorHandler.ProvideFault(error, version, ref fault);
        }

        #endregion
    }
}
