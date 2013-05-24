//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.Properties;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.Practices.Unity;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    /// <summary>
    /// Base class for configuration information stored about a call handler.
    /// </summary>
    public class CallHandlerData : NameTypeConfigurationElement
    {
        private const string orderProperty = "order";

        /// <summary>
        /// Initializes a new instance of the <see cref="CallHandlerData"/> class.
        /// </summary>
        public CallHandlerData()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CallHandlerData"/> class with the specified name and type.
        /// </summary>
        /// <param name="handlerName">The name of the handler entry.</param>
        /// <param name="handlerType">The type of handler to create.</param>
        public CallHandlerData(string handlerName, Type handlerType)
            : base(handlerName, handlerType)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CallHandlerData"/> class with the specified name, type, and handler order.
        /// </summary>
        /// <param name="handlerName">The name of the handler entry.</param>
        /// <param name="handlerType">The type of handler to create.</param>
        /// <param name="order">The order in which the handler will be executed.</param>
        public CallHandlerData(string handlerName, Type handlerType, int order)
            : base(handlerName, handlerType)
        {
            this.Order = order;
        }

        /// <summary>
        /// Gets or sets the the order in which the handler will be executed.
        /// </summary>
        [ConfigurationProperty(orderProperty, DefaultValue = 0, IsRequired = false)]
        [ResourceDescription(typeof(DesignResources), "CallHandlerDataOrderDescription")]
        [ResourceDisplayName(typeof(DesignResources), "CallHandlerDataOrderDisplayName")]
        public int Order
        {
            get { return (int)this[orderProperty]; }
            set { this[orderProperty] = value; }
        }

        /// <summary>
        /// Configures the specified container.
        /// </summary>
        /// <param name="container">The container to configure.</param>
        /// <param name="nameSuffix">The name suffix to use for the handler registration name.</param>
        /// <returns>The actual handler registration name.</returns>
        public string ConfigureContainer(IUnityContainer container, string nameSuffix)
        {
            var registrationName = this.Name + nameSuffix;

            this.DoConfigureContainer(container, registrationName);

            return registrationName;
        }

        /// <summary>
        /// Configures an <see cref="IUnityContainer"/> to resolve the represented call handler by using the specified name.
        /// </summary>
        /// <param name="container">The container to configure.</param>
        /// <param name="registrationName">The name of the registration.</param>
        protected virtual void DoConfigureContainer(IUnityContainer container, string registrationName)
        {
            throw new NotImplementedException(Resources.ExceptionShouldBeImplementedBySubclass);
        }
    }
}
