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
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Configuration
{
    /// <summary>
    /// Configuration element class that manages the config data for the <see cref="CachingCallHandler"/>.
    /// </summary>
    [Assembler(typeof(CachingCallHandlerAssembler))]
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
    }

    /// <summary>
    /// Class used by ObjectBuilder to construct a <see cref="CachingCallHandler"/>
    /// from a <see cref="CachingCallHandlerData"/>.
    /// </summary>
    public class CachingCallHandlerAssembler : IAssembler<ICallHandler, CallHandlerData>
    {

        #region IAssembler<ICallHandler,CallHandlerData> Members

        /// <summary>
        /// Builds an instance of the subtype of <typeparamref name="TObject"/> type the receiver knows how to build,  based on 
        /// an a configuration object.
        /// </summary>
        /// <param name="context">The <see cref="IBuilderContext"/> that represents the current building process.</param>
        /// <param name="objectConfiguration">The configuration object that describes the object to build.</param>
        /// <param name="configurationSource">The source for configuration objects.</param>
        /// <param name="reflectionCache">The cache to use retrieving reflection information.</param>
        /// <returns>A fully initialized instance of the <typeparamref name="TObject"/> subtype.</returns>
        public ICallHandler Assemble(
            IBuilderContext context,
            CallHandlerData objectConfiguration,
            IConfigurationSource configurationSource,
            ConfigurationReflectionCache reflectionCache)
        {
            CachingCallHandlerData handlerConfiguration = (CachingCallHandlerData)objectConfiguration;

            return new CachingCallHandler(handlerConfiguration.ExpirationTime, handlerConfiguration.Order);
        }

        #endregion
    }
}
