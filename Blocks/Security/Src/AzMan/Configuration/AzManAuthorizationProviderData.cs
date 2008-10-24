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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Security.AzMan.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.Security.AzMan.Configuration
{
	/// <summary>
	/// Represents the configuration settings for the <see cref="AzManAuthorizationProvider"/>.
	/// </summary>
	[Assembler(typeof (AzManAuthorizationProviderAssembler))]
	[ContainerPolicyCreator(typeof (AzManAuthorizationProviderPolicyCreator))]
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
			: base(name, typeof (AzManAuthorizationProvider))
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
			get { return (string) this[storeLocationProperty]; }
			set { this[storeLocationProperty] = value; }
		}

		/// <summary>
		/// Name of the AzMan application.
		/// </summary>
		[ConfigurationProperty(applicationNameProperty)]
		public string Application
		{
			get { return (string) this[applicationNameProperty]; }
			set { this[applicationNameProperty] = value; }
		}

		/// <summary>
		/// Optional name of the application scope.
		/// </summary>
		[ConfigurationProperty(scopeNameProperty)]
		public string Scope
		{
			get { return (string) this[scopeNameProperty]; }
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
			get { return (string) this[auditIdentifierPrefixProperty]; }
			set { this[auditIdentifierPrefixProperty] = value; }
		}
	}

	/// <summary>
	/// This type supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
	/// Represents the process to build for an <see cref="AzManAuthorizationProvider"/> described by a <see cref="AzManAuthorizationProviderData"/> configuration object.
	/// </summary>
	/// <remarks>This type is linked to the <see cref="AzManAuthorizationProviderData"/> type and it is used by the <see cref="AuthorizationProviderCustomFactory"/> 
	/// to build the specific <see cref="IAuthorizationProvider"/> object represented by the configuration object.
	/// </remarks>
	public class AzManAuthorizationProviderAssembler : IAssembler<IAuthorizationProvider, AuthorizationProviderData>
	{
		/// <summary>
		/// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// Builds an <see cref="AzManAuthorizationProvider"/> based on an instance of <see cref="AzManAuthorizationProviderData"/>.
		/// </summary>
		/// <seealso cref="AuthorizationProviderCustomFactory"/>
		/// <param name="context">The <see cref="IBuilderContext"/> that represents the current building process.</param>
		/// <param name="objectConfiguration">The configuration object that describes the object to build. Must be an instance of <see cref="AzManAuthorizationProviderData"/>.</param>
		/// <param name="configurationSource">The source for configuration objects.</param>
		/// <param name="reflectionCache">The cache to use retrieving reflection information.</param>
		/// <returns>A fully initialized instance of <see cref="AzManAuthorizationProvider"/>.</returns>
		public IAuthorizationProvider Assemble(IBuilderContext context,
		                                       AuthorizationProviderData objectConfiguration,
		                                       IConfigurationSource configurationSource,
		                                       ConfigurationReflectionCache reflectionCache)
		{
			AzManAuthorizationProviderData castedObjectConfiguration
				= (AzManAuthorizationProviderData) objectConfiguration;

			IAuthorizationProvider createdObject
				= new AzManAuthorizationProvider(
					castedObjectConfiguration.StoreLocation,
					castedObjectConfiguration.Application,
					castedObjectConfiguration.AuditIdentifierPrefix,
					castedObjectConfiguration.Scope);

			return createdObject;
		}
	}
}
