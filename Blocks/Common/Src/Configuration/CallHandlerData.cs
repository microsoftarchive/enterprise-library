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
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    /// <summary>
    /// Base class for configuration information stored about a call handler.
    /// </summary>
    public class CallHandlerData : NameTypeConfigurationElement
    {
        private const string orderProperty = "order";

        /// <summary>
        /// Creates a new instance of <see cref="CallHandlerData"/>.
        /// </summary>
        public CallHandlerData()
            : base()
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="CallHandlerData"/>.
        /// </summary>
        /// <param name="handlerName">Name of handler entry.</param>
        /// <param name="handlerType">Type of handler to create.</param>
        public CallHandlerData(string handlerName, Type handlerType)
            : base(handlerName, handlerType)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="CallHandlerData"/>.
        /// </summary>
        /// <param name="handlerName">Name of handler entry.</param>
        /// <param name="handlerType">Type of handler to create.</param>
        /// <param name="order">The order of the handler.</param>
        public CallHandlerData(string handlerName, Type handlerType, int order)
            : base(handlerName, handlerType)
        {
            this.Order = order;
        }

        /// <summary>
        /// Gets or sets the Order in which the call handler will be executed
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
        /// Get the set of <see cref="TypeRegistration"/> objects needed to
        /// register the call handler represented by this config element and its associated objects.
        /// </summary>
        /// <param name="nameSuffix">A suffix for the names in the generated type registration objects.</param>
        /// <returns>The set of <see cref="TypeRegistration"/> objects.</returns>
        public virtual IEnumerable<TypeRegistration> GetRegistrations(string nameSuffix)
        {
            throw new NotImplementedException();
        }
    }
}
