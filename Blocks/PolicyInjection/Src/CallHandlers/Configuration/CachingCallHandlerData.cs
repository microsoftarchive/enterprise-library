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
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Configuration
{
    /// <summary>
    /// Configuration element class that manages the config data for the <see cref="CachingCallHandler"/>.
    /// </summary>
    public class CachingCallHandlerData : CallHandlerData
    {
        private const string ExpirationTimePropertyName = "expirationTime";

        /// <summary>
        /// Create a new <see cref="CachingCallHandlerData"/> instance.
        /// </summary>
        public CachingCallHandlerData()
        {
            ExpirationTime = CachingCallHandler.DefaultExpirationTime;
        }

        /// <summary>
        /// Create a new <see cref="CachingCallHandlerData"/> instance with the given name.
        /// </summary>
        /// <param name="handlerName">Name of handler to store in config file.</param>
        public CachingCallHandlerData(string handlerName)
            : base(handlerName, typeof(CachingCallHandler))
        {
            ExpirationTime = CachingCallHandler.DefaultExpirationTime;
        }

        /// <summary>
        /// Create a new <see cref="CachingCallHandlerData"/> instance with the given name.
        /// </summary>
        /// <param name="handlerName">Name of handler to store in config file.</param>
        /// <param name="handlerOrder">Order of handler to store in config file.</param>
        public CachingCallHandlerData(string handlerName, int handlerOrder)
            : base(handlerName, typeof(CachingCallHandler), handlerOrder)
        {
            ExpirationTime = CachingCallHandler.DefaultExpirationTime;
        }

        /// <summary>
        /// Expiration time
        /// </summary>
        /// <value>The "expirationTime" attribute</value>
        [ConfigurationProperty(ExpirationTimePropertyName)]
        public TimeSpan ExpirationTime
        {
            get { return (TimeSpan)base[ExpirationTimePropertyName]; }
            set { base[ExpirationTimePropertyName] = value; }
        }

        /// <summary>
        /// Get the set of <see cref="TypeRegistration"/> objects needed to
        /// register the call handler represented by this config element and its associated objects.
        /// </summary>
        /// <param name="nameSuffix">A suffix for the names in the generated type registration objects.</param>
        /// <returns>The set of <see cref="TypeRegistration"/> objects.</returns>
        public override IEnumerable<TypeRegistration> GetRegistrations(string nameSuffix)
        {
            yield return
                new TypeRegistration<ICallHandler>(() => new CachingCallHandler(this.ExpirationTime, this.Order))
                {
                    Name = this.Name + nameSuffix
                };
        }
    }
}
