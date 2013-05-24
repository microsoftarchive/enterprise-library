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
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF
{
    /// <summary>
    /// The behavior class that set up the <see cref="ExceptionShieldingErrorHandler"/> 
    /// for implementing the exception shielding process.
    /// </summary>
    public class ExceptionShieldingBehavior : IServiceBehavior, IContractBehavior
    {
        #region ExceptionShieldingBehavior Members

        private string exceptionPolicyName;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ExceptionShieldingBehavior"/> class.
        /// </summary>
        public ExceptionShieldingBehavior()
            : this(ExceptionShielding.DefaultExceptionPolicy)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ExceptionShieldingBehavior"/> class.
        /// </summary>
        /// <param name="exceptionPolicyName">Name of the exception policy.</param>
        public ExceptionShieldingBehavior(string exceptionPolicyName)
        {
            this.exceptionPolicyName = exceptionPolicyName;
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
            // Not implemented.
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
            // Not implemented.
        }

        /// <summary>
        /// Applies the dispatch behavior.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <param name="serviceHostBase">The service host base.</param>
        public void ApplyDispatchBehavior(ServiceDescription description, ServiceHostBase serviceHostBase)
        {
            foreach (ChannelDispatcher dispatcher in serviceHostBase.ChannelDispatchers)
            {
                AddErrorHandler(dispatcher);
            }
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
            System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
            // Not implemented.
        }

        /// <summary>
        /// Implements a modification or extension of the client across a contract.
        /// </summary>
        /// <param name="contractDescription">The contract description for which the extension is intended.</param>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="clientRuntime">The client runtime.</param>
        public void ApplyClientBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            // Not implemented.
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
            if (dispatchRuntime == null) throw new ArgumentNullException("dispatchRuntime");
            AddErrorHandler(dispatchRuntime.ChannelDispatcher);
        }

        /// <summary>
        /// Implement to confirm that the contract and endpoint can support the contract behavior.
        /// </summary>
        /// <param name="contractDescription">The contract to validate.</param>
        /// <param name="endpoint">The endpoint to validate.</param>
        public void Validate(ContractDescription contractDescription, ServiceEndpoint endpoint)
        {
            // Not implemented.
        }

        #endregion

        #region Private Members

        private void AddErrorHandler(ChannelDispatcher channelDispatcher)
        {
            if (!channelDispatcher.IncludeExceptionDetailInFaults &&
                !ContainsExceptionShieldingErrorHandler(channelDispatcher.ErrorHandlers) &&
                !ContainsMetadataEndpoint(channelDispatcher.Endpoints))
            {
                IErrorHandler errorHandler = new ExceptionShieldingErrorHandler(exceptionPolicyName);
                channelDispatcher.ErrorHandlers.Add(errorHandler);
            }
        }

        // Check if the handler is already in the collection 
        private bool ContainsExceptionShieldingErrorHandler(Collection<IErrorHandler> handlers)
        {
            foreach (IErrorHandler handler in handlers)
            {
                if (typeof(ExceptionShieldingErrorHandler).IsInstanceOfType(handler))
                {
                    return true;
                }
            }
            return false;
        }

        private bool ContainsMetadataEndpoint(SynchronizedCollection<EndpointDispatcher> endpoints)
        {
            string mexContractName = typeof(IMetadataExchange).Name;
            foreach (EndpointDispatcher endpoint in endpoints)
            {
                if (endpoint.ContractName == mexContractName)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion
    }
}
