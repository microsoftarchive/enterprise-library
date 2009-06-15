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

using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;
using System.Linq.Expressions;
using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Security.Instrumentation;
using System.Collections.Generic;

namespace Microsoft.Practices.EnterpriseLibrary.Security.AzMan.Configuration
{
    /// <summary>
    /// Represents the configuration settings for the <see cref="AzManAuthorizationProvider"/>.
    /// </summary>
    public class AzManAuthorizationProviderData : AuthorizationProviderData
    {
        private const string storeLocationProperty = "storeLocation";
        private const string applicationNameProperty = "application";
        private const string auditIdentifierPrefixProperty = "auditIdentifierPrefix";
        private const string scopeNameProperty = "scope";

        /// <summary>
        /// Initialize an instance of the <see cref="AzManAuthorizationProviderData"/> class.
        /// </summary>
        public AzManAuthorizationProviderData()
        {
        }

        /// <summary>
        /// Initialize an instance of the <see cref="AzManAuthorizationProviderData"/> class.
        /// </summary>
        /// <param name="storeLocation">Location of the authorization store, Active Directory or xml file</param>
        /// <param name="applicationName">Name of the AzMan application.</param>
        /// <param name="auditIdentifierPrefix">Audit identifier prefix to prepend to the generated audit identifer</param>
        /// <param name="scopeName">Optional name of the application scope</param>
        public AzManAuthorizationProviderData(string storeLocation,
                                              string applicationName,
                                              string auditIdentifierPrefix,
                                              string scopeName)
            : this("unnamed", storeLocation, applicationName, auditIdentifierPrefix, scopeName)
        {
        }

        /// <summary>
        /// Initialize an instance of the <see cref="AzManAuthorizationProviderData"/> class.
        /// </summary>
        /// <param name="name">Name of <see cref="AzManAuthorizationProvider"></see> found in configuration</param>
        /// <param name="storeLocation">Location of the authorization store, Active Directory or xml file</param>
        /// <param name="applicationName">Name of the AzMan application.</param>
        /// <param name="auditIdentifierPrefix">Audit identifier prefix to prepend to the generated audit identifer</param>
        /// <param name="scopeName">Optional name of the application scope</param>
        public AzManAuthorizationProviderData(string name,
                                              string storeLocation,
                                              string applicationName,
                                              string auditIdentifierPrefix,
                                              string scopeName)
            : base(name, typeof(AzManAuthorizationProvider))
        {
            this.StoreLocation = storeLocation;
            this.Application = applicationName;
            this.AuditIdentifierPrefix = auditIdentifierPrefix;
            this.Scope = scopeName;
        }

        /// <summary>
        /// Location of the authorization store, Active Directory or xml file.
        /// </summary>
        /// <remarks>Absolute file paths are required for xml storage.  
        /// View this link for more information about the expected format http://msdn.microsoft.com/library/default.asp?url=/library/en-us/security/security/azauthorizationstore_initialize.asp.</remarks>
        [ConfigurationProperty(storeLocationProperty)]
        public string StoreLocation
        {
            get { return (string)this[storeLocationProperty]; }
            set { this[storeLocationProperty] = value; }
        }

        /// <summary>
        /// Name of the AzMan application.
        /// </summary>
        [ConfigurationProperty(applicationNameProperty)]
        public string Application
        {
            get { return (string)this[applicationNameProperty]; }
            set { this[applicationNameProperty] = value; }
        }

        /// <summary>
        /// Optional name of the application scope.
        /// </summary>
        [ConfigurationProperty(scopeNameProperty)]
        public string Scope
        {
            get { return (string)this[scopeNameProperty]; }
            set { this[scopeNameProperty] = value; }
        }

        /// <summary>
        /// Audit identifier prefix to append to the generated audit identifer.
        /// </summary>
        /// <remarks>
        /// The audit identifier is generated to be "prefix username:operation"
        /// </remarks>
        [ConfigurationProperty(auditIdentifierPrefixProperty)]
        public string AuditIdentifierPrefix
        {
            get { return (string)this[auditIdentifierPrefixProperty]; }
            set { this[auditIdentifierPrefixProperty] = value; }
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="configurationSource"></param>
        /// <returns></returns>
        public override IEnumerable<TypeRegistration> GetRegistrations(IConfigurationSource configurationSource)
        {
            yield return GetInstrumentationProviderRegistration(configurationSource);

            yield return new TypeRegistration<IAuthorizationProvider>(() => new AzManAuthorizationProvider(StoreLocation,
                                                        Application,
                                                        AuditIdentifierPrefix,
                                                        Scope,
                                                        Container.Resolved<IAuthorizationProviderInstrumentationProvider>(Name)))
            {
                Name = this.Name,
                Lifetime = TypeRegistrationLifetime.Transient
            };
        }
    }
}
