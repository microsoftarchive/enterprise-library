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

using System;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Principal;
using Microsoft.Interop.Security.AzRoles;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.AzMan.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Security.AzMan
{
	/// <summary>
	/// Represents the AzMan authorization provider.  
	/// Authorize method checks to see if the specified identity has access to a task.
	/// </summary>
	/// <remarks>
	/// Supports Windows authentication only.
	/// Requires AzMan on Windows Server 2003, or Windows XP users will need to install the 
	/// Windows Server 2003 Administration Pack.
	/// Implements the <see cref="IAuthorizationProvider"/> interface.</remarks>
	[ConfigurationElementType(typeof(AzManAuthorizationProviderData))]
	public class AzManAuthorizationProvider : AuthorizationProvider
	{
		private const string OperationContextPrefix = "O:";
		private const string StoreLocationTargetToken = "{currentPath}";
		private const string StoreLocationAppBaseToken = "{baseDirectory}";

		private readonly string storeLocation;
		private readonly string applicationName;
		private readonly string auditIdentifierPrefix;
		private readonly string scopeName;

		private readonly object contextLock = new object();
        /// <summary>
        /// Creates a new instance of the <see cref="AzManAuthorizationProvider"/> class.
        /// </summary>
        /// <param name="storeLocation">The AzMan store location.</param>
        /// <param name="applicationName">The AzMan application name.</param>
        /// <param name="auditIdentifierPrefix">The AzMan identifier prefix.</param>
        /// <param name="scopeName">The AzMan scope name.</param>
        public AzManAuthorizationProvider(
            string storeLocation,
            string applicationName,
            string auditIdentifierPrefix,
            string scopeName)
            : this(storeLocation, applicationName, auditIdentifierPrefix, scopeName, new NullAuthorizationProviderInstrumentationProvider())
        {
        }


		/// <summary>
		/// Creates a new instance of the <see cref="AzManAuthorizationProvider"/> class.
		/// </summary>
		/// <param name="storeLocation">The AzMan store location.</param>
		/// <param name="applicationName">The AzMan application name.</param>
		/// <param name="auditIdentifierPrefix">The AzMan identifier prefix.</param>
		/// <param name="scopeName">The AzMan scope name.</param>
        /// <param name="instrumentationProvider">The instrumentation provider to use.</param>
		public AzManAuthorizationProvider(
			string storeLocation,
			string applicationName,
			string auditIdentifierPrefix,
			string scopeName,
            IAuthorizationProviderInstrumentationProvider instrumentationProvider)
            :base(instrumentationProvider)
		{
			this.storeLocation = GetStoreLocationPath(storeLocation);
			this.applicationName = applicationName;
			this.auditIdentifierPrefix = auditIdentifierPrefix;
			this.scopeName = scopeName;
		}

		/// <summary>
		/// Evaluates the specified authority against the specified context that is either a task or operation in Authorization Manager. If the context is an operation it should be prefixed by "O".
		/// </summary>
		/// <param name="principal">Principal object containing a windows identity.</param>
		/// <param name="context">Name of the task or operation to evaluate.</param>
		/// <returns><strong>True</strong> if AzMan evaluates to true,
		/// otherwise <strong>false</strong>.</returns>
		public override bool Authorize(IPrincipal principal, string context)
		{
			if (principal == null) throw new ArgumentNullException("principal");
			if (context == null) throw new ArgumentNullException("context");

			WindowsIdentity winIdentity = principal.Identity as WindowsIdentity;
			if (winIdentity == null)
			{
				throw new ArgumentException(Properties.Resources.WindowsIdentityOnly);
			}

			string auditIdentifier = this.auditIdentifierPrefix + principal.Identity.Name + ":" + context;

			bool result = false;
			bool operation = false;
			if (context.IndexOf(OperationContextPrefix) == 0)
			{
				operation = true;
				context = context.Substring(OperationContextPrefix.Length);
			}

			if (operation)
			{
				string[] operations = new string[] { context };
				result = CheckAccessOperations(auditIdentifier, winIdentity, operations);
			}
			else
			{
				string[] tasks = new string[] { context };
				result = CheckAccessTasks(auditIdentifier, winIdentity, tasks);
			}
			InstrumentationProvider.FireAuthorizationCheckPerformed(principal.Identity.Name, context);

			if (result == false)
			{
				InstrumentationProvider.FireAuthorizationCheckFailed(principal.Identity.Name, context);
			}
			return result;
		}

		/// <devdoc>
		/// Checks access to specified a set of tasks in a specified application in a specified scope.
		/// </devdoc>      
		private bool CheckAccessTasks(string auditIdentifier, WindowsIdentity identity, string[] tasks)
		{
			string[] scopes = new string[] { this.scopeName };

			IAzApplication azApp = null;
			try
			{
				IAzClientContext clientCtx = GetClientContext(identity, this.applicationName, out azApp);
				object[] operationIds = GetTaskOperations(azApp, tasks);

				object[] internalScopes = null;
				if (scopes != null)
				{
					internalScopes = new object[1];
					internalScopes[0] = scopes[0];
				}

				object[] result = (object[])clientCtx.AccessCheck(auditIdentifier,
																   internalScopes, operationIds, null, null, null, null, null);
				foreach (int accessAllowed in result)
				{
					if (accessAllowed != 0)
					{
						return false;
					}
				}
			}
			catch (COMException comEx)
			{
				throw new SecurityException(comEx.Message, comEx);
			}
			return true;
		}

		private object[] GetTaskOperations(IAzApplication azApp, string[] tasks)
		{
			string[] scopes = new string[] { this.scopeName };
			StringCollection operations = new StringCollection();
			foreach (String task in tasks)
			{
				IAzScope scope = null;
				if ((scopes != null) && (scopes[0].Length > 0))
				{
					scope = azApp.OpenScope(scopes[0], null);
				}

				IAzTask azTask = null;
				if (scope != null)
				{
					azTask = scope.OpenTask(task, null);
				}
				else
				{
					azTask = azApp.OpenTask(task, null);
				}

				Array ops = azTask.Operations as Array;
				foreach (String op in ops)
				{
					operations.Add(op);
				}
			}

			if (operations.Count == 0)
			{
				throw new ConfigurationErrorsException(Properties.Resources.NoOperations);
			}

			object[] operationIds = new object[operations.Count];
			for (int index = 0; index < operations.Count; index++)
			{
				operationIds[index] = azApp.OpenOperation(operations[index], null).OperationID;
			}

			return operationIds;
		}

		/// <devdoc>
		/// Checks access to specified a set of operations in a specified application in a specified scope.
		/// </devdoc>        
		private bool CheckAccessOperations(string auditIdentifier, WindowsIdentity identity, string[] operations)
		{
			string[] scopes = new string[] { this.scopeName };

			IAzApplication azApp = null;
			try
			{
				IAzClientContext clientCtx = GetClientContext(identity, this.applicationName, out azApp);
				object[] operationIds = new object[operations.Length];
				for (int index = 0; index < operations.Length; index++)
				{
					operationIds[index] = azApp.OpenOperation(operations[index], null).OperationID;
				}

				object[] internalScopes = null;
				if (scopes != null)
				{
					internalScopes = new object[1];
					internalScopes[0] = scopes[0];
				}

				object[] result = (object[])clientCtx.AccessCheck(auditIdentifier,
																   internalScopes, operationIds, null, null, null, null, null);
				foreach (int accessAllowed in result)
				{
					if (accessAllowed != 0)
					{
						return false;
					}
				}
			}
			catch (COMException comEx)
			{
				throw new SecurityException(comEx.Message, comEx);
			}
			return true;
		}

		/// <devdoc>
		/// Gets the client context for the call based on the identity, system and parameters.
		/// </devdoc>        
		private IAzClientContext GetClientContext(WindowsIdentity identity, String applicationName, out IAzApplication azApp)
		{
			lock (contextLock)
			{
				AzAuthorizationStoreClass store = new AzAuthorizationStoreClass();
				store.Initialize(0, this.storeLocation, null);
				azApp = store.OpenApplication(applicationName, null);
			}

			ulong tokenHandle = (ulong)identity.Token.ToInt64();
			IAzClientContext clientCtx = azApp.InitializeClientContextFromToken(tokenHandle, null);
			return clientCtx;
		}

		/// <summary>
		/// Calculates and returns the effective store location.
		/// </summary>
		public static string GetStoreLocationPath(string storeLocation)
		{
			string store = storeLocation;
			if (store.IndexOf(StoreLocationTargetToken) > -1)
			{
				string dir = Directory.GetCurrentDirectory().Replace(@"\", "/");
				store = store.Replace(StoreLocationTargetToken, dir);
			}
			if (store.IndexOf(StoreLocationAppBaseToken) > -1)
			{
				string dir = AppDomain.CurrentDomain.BaseDirectory.Replace(@"\", "/");
				store = store.Replace(StoreLocationAppBaseToken, dir);
			}

			return store;
		}

		/// <summary>
		/// Gets the AzMan store location.
		/// </summary>
		public string StoreLocation
		{
			get { return storeLocation; }
		}

		/// <summary>
		/// Gets the application name.
		/// </summary>
		public string ApplicationName
		{
			get { return applicationName; }
		}

		/// <summary>
		/// Gets the audit identifier prefix.
		/// </summary>
		public string AuditIdentifierPrefix
		{
			get { return auditIdentifierPrefix; }
		}
		
		/// <summary>
		/// Gets the scope name.
		/// </summary>
		public string ScopeName
		{
			get { return scopeName; }	
		}

	}
}
