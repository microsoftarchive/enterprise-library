//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Collections.Generic;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Security.PolicyInjection;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration
{
    /// <summary>
    /// Call handler data describing the information for the authorization call handler
    /// in configuration.
    /// </summary>
    [ResourceDescription(typeof(DesignResources), "AuthorizationCallHandlerDataDescription")]
    [ResourceDisplayName(typeof(DesignResources), "AuthorizationCallHandlerDataDisplayName")]
    [AddSateliteProviderCommand(SecuritySettings.SectionName, typeof(SecuritySettings), "DefaultAuthorizationProviderName", "AuthorizationProvider")]
    public class AuthorizationCallHandlerData : CallHandlerData
    {
        private const string AuthorizationProviderPropertyName = "authorizationProvider";
        private const string OperationNamePropertyName = "operationName";

        /// <summary>
        /// Create a new <see cref="AuthorizationCallHandlerData"/>.
        /// </summary>
        public AuthorizationCallHandlerData()
        {
            Type = typeof(AuthorizationCallHandler);
        }

        /// <summary>
        /// Create a new <see cref="AuthorizationCallHandlerData"/>.
        /// </summary>
        /// <param name="handlerName">Name of the call handler.</param>
        public AuthorizationCallHandlerData(string handlerName)
            : base(handlerName, typeof(AuthorizationCallHandler))
        {
        }

        /// <summary>
        /// Create a new <see cref="AuthorizationCallHandlerData"/>.
        /// </summary>
        /// <param name="handlerName">Name of the call handler.</param>
        /// <param name="handlerOrder">Order of the call handler.</param>
        public AuthorizationCallHandlerData(string handlerName, int handlerOrder)
            : base(handlerName, typeof(AuthorizationCallHandler))
        {
            this.Order = handlerOrder;
        }

        /// <summary>
        /// Authorization provider to use for this call handler.
        /// </summary>
        /// <value>The "authorizationProvider" attribute.</value>
        [ConfigurationProperty(AuthorizationProviderPropertyName)]
        [ResourceDescription(typeof(DesignResources), "AuthorizationCallHandlerDataAuthorizationProviderDescription")]
        [ResourceDisplayName(typeof(DesignResources), "AuthorizationCallHandlerDataAuthorizationProviderDisplayName")]
        [Reference(typeof(SecuritySettings), typeof(AuthorizationProviderData))]
        public string AuthorizationProvider
        {
            get { return (string)base[AuthorizationProviderPropertyName]; }
            set { base[AuthorizationProviderPropertyName] = value; }
        }

        /// <summary>
        /// Operation name to use for this call handler.
        /// </summary>
        /// <value>The "operationName" attribute.</value>
        [ConfigurationProperty(OperationNamePropertyName, IsRequired = true)]
        [ResourceDescription(typeof(DesignResources), "AuthorizationCallHandlerDataOperationNameDescription")]
        [ResourceDisplayName(typeof(DesignResources), "AuthorizationCallHandlerDataOperationNameDisplayName")]
        public string OperationName
        {
            get { return (string)base[OperationNamePropertyName]; }
            set { base[OperationNamePropertyName] = value; }
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
                new TypeRegistration<ICallHandler>(() =>
                                                   new AuthorizationCallHandler(
                                                       Container.Resolved<IAuthorizationProvider>(this.AuthorizationProvider),
                                                       this.OperationName,
                                                       this.Order))
                    {
                        Name = this.Name + nameSuffix,
                        Lifetime = TypeRegistrationLifetime.Transient
                    };
        }
    }
}
