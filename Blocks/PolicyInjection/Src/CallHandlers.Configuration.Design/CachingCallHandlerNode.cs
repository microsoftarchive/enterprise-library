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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Configuration.Design
{
    /// <summary>
    /// A <see cref="ConfigurationNode"/> that stores the configuration information
    /// for the <see cref="Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.CachingCallHandler"/>.
    /// </summary>
    public class CachingCallHandlerNode : CallHandlerNode
    {
        TimeSpan expirationTime;

        /// <summary>
        /// Create a new <see cref="CachingCallHandlerNode"/> with default configuration.
        /// </summary>
        public CachingCallHandlerNode()
            : this(new CachingCallHandlerData(Resources.CachingCallHandlerNodeName)) {}

        /// <summary>
        /// Create a new <see cref="CachingCallHandlerNode"/> initialized with the
        /// configuration data stored in <paramref name="callHandlerData"/>.
        /// </summary>
        /// <param name="callHandlerData">Configuration data to initialize the node with.</param>
        public CachingCallHandlerNode(CachingCallHandlerData callHandlerData)
            : base(callHandlerData)
        {
            expirationTime = callHandlerData.ExpirationTime;
        }

        /// <summary>
        /// Sliding expiration time to expire items in the cache.
        /// </summary>
        /// <value>Sets or get the expiration time.</value>
        [SRDescription("ExpirationTimeDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public TimeSpan ExpirationTime
        {
            get { return expirationTime; }
            set { expirationTime = value; }
        }

        /// <summary>
        /// Convert the data stored into this node into the corresponding
        /// configuration class (<see cref="CachingCallHandlerData"/>).
        /// </summary>
        /// <returns>Newly created <see cref="CachingCallHandlerData"/> containing
        /// the configuration data from this node.</returns>
        public override CallHandlerData CreateCallHandlerData()
        {
            CachingCallHandlerData callHandlerData = new CachingCallHandlerData(Name, Order);
            callHandlerData.ExpirationTime = expirationTime;

            return callHandlerData;
        }
    }
}
