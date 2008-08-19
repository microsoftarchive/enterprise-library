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

using System.ComponentModel;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Security.AzMan.Configuration.Design.Properties;
using System;

namespace Microsoft.Practices.EnterpriseLibrary.Security.AzMan.Configuration.Design
{
	/// <summary>
	/// 
	/// </summary>
	public class AzManAuthorizationProviderNode : AuthorizationProviderNode
	{
		private const string defaultStoreLocation = "msxml://c:/myAuthStore.xml";
		private const string applicationName = "My Application";
		private static readonly string scopeName = string.Empty;
		private const string auditIdentifierPrefix = "AzMan Authorization Provider";

		private string scope;
		private string application;
		private string storeLocation;
		private string auditIdentifier;


		/// <summary>
		/// 
		/// </summary>
		public AzManAuthorizationProviderNode()
			: this(new AzManAuthorizationProviderData(Resources.AzManProvider, defaultStoreLocation, applicationName, auditIdentifierPrefix, scopeName))
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="azManAuthorizationProviderData"></param>
		public AzManAuthorizationProviderNode(AzManAuthorizationProviderData azManAuthorizationProviderData)
		{
			if (azManAuthorizationProviderData == null)
			{
				throw new ArgumentNullException("azManAuthorizationProviderData");
			}
			this.scope = azManAuthorizationProviderData.Scope;
			this.application = azManAuthorizationProviderData.Application;
			this.storeLocation = azManAuthorizationProviderData.StoreLocation;
			this.auditIdentifier = azManAuthorizationProviderData.AuditIdentifierPrefix;
			Rename(azManAuthorizationProviderData.Name);
		}

		/// <summary>
		/// 
		/// </summary>
		[Required]
		[SRDescription("ApplicationDescription", typeof(Resources))]
		[SRCategory("CategoryGeneral", typeof(Resources))]
		public string Application
		{
			get { return application; }
			set { application = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		[SRDescription("ScopeDescription", typeof(Resources))]
		[SRCategory("CategoryGeneral", typeof(Resources))]
		public string Scope
		{
			get { return scope; }
			set { scope = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		[Required]
		[SRDescription("StoreLocationDescription", typeof(Resources))]
		[SRCategory("CategoryGeneral", typeof(Resources))]
		public string StoreLocation
		{
			get { return storeLocation; }
			set { storeLocation = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		[Required]
		[SRDescription("AuditIdentifierPrefixDescription", typeof(Resources))]
		[SRCategory("CategoryGeneral", typeof(Resources))]
		public string AuditIdentifierPrefix
		{
			get { return auditIdentifier; }
			set { auditIdentifier = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		[Browsable(false)]
		public override AuthorizationProviderData AuthorizationProviderData
		{
			get { return new AzManAuthorizationProviderData(Name, this.storeLocation, this.application, this.auditIdentifier, this.scope); }
		}
	}
}