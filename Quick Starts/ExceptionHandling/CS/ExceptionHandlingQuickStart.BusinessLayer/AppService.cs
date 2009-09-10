//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block QuickStart
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Data;
using System.Security.Principal;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace ExceptionHandlingQuickStart.BusinessLayer
{
	public class AppService
	{
	    private readonly ExceptionManager exceptionManager;

        public AppService()
        {
            exceptionManager = EnterpriseLibraryContainer.Current.GetInstance<ExceptionManager>();
        }

		public void ProcessA()
		{
			throw new Exception("Original Exception: Fatal exception in business layer");
		}

		public void ProcessB()
		{
			throw new DBConcurrencyException("Original Exception: Concurrency problem in business layer");
		}
		
		private void ProcessC()
		{
			// Assume operation failed due to authentication. Get current
			// user's identity information.

			//Get the current identity and put it into an identity object.
			WindowsIdentity myIdentity = WindowsIdentity.GetCurrent();

			//Put the previous identity into a principal object.
			WindowsPrincipal myPrincipal = new WindowsPrincipal(myIdentity);

			//Principal values.
			string principalName = myPrincipal.Identity.Name;
			string principalType = myPrincipal.Identity.AuthenticationType;
			string principalAuth = myPrincipal.Identity.IsAuthenticated.ToString();

			//Identity values.
			string identName = myIdentity.Name;
			string identType = myIdentity.AuthenticationType;
			string identToken = myIdentity.Token.ToString();

			//Print the values.
			string identityInfo = String.Format("Principal Values for current thread:" + 
				"\n\nPrincipal Name: {0}" +
				"Principal Type: {1}" + 
				"Principal IsAuthenticated: {2}" + 
				"\n\nIdentity Values for current thread:"  +
				"Identity Name: {3}" + 
				"Identity Type: {4}" + 
				"Identity Token: {5}", 
				principalName, principalType, principalAuth, 
				identName, identType, identToken);

			throw new System.Security.SecurityException(identityInfo);
		}

		public void ProcessD()
		{
			throw new BusinessLayerException("Original Exception: Problem in business layer");
		}

		/// <summary>
		/// Demonstrates handling of exceptions coming out of a layer. The policy
		/// demonstrated here will show how original exceptions can be propagated back out.
		/// </summary>
		public bool ProcessWithPropagate()
		{
		    exceptionManager.Process(ProcessA, "Propagate Policy");
		    return true;
		}

		/// <summary>
		/// Demonstrates handling of exceptions coming out of a layer. The policy
		/// demonstrated here will show how original exceptions can be wrapped
		/// with a different exception before being propagated back out.
		/// </summary>
		public bool ProcessWithWrap()
		{
            // Quick Start is configured so that the Wrap Policy will
            // log the exception and then recommend a rethrow.
            exceptionManager.Process(ProcessB, "Wrap Policy");
		    return true;
		}
	
		public void ProcessWithReplace()
		{
            // Invoke our policy that is responsible for making sure no secure information
            // gets out of our layer.
            exceptionManager.Process(ProcessC, "Replace Policy");
		}

		public void ProcessAndResume()
		{
            // Invoke our policy that is responsible for making sure no secure information
            // gets out of our layer.
            exceptionManager.Process(ProcessC, "Handle and Resume Policy");
		}

		public void ProcessAndNotify()
		{
            // Invoke our policy that is responsible for making sure no secure information
            // gets out of our layer.
            exceptionManager.Process(ProcessD, "Notify Policy");
		}
	}
}
