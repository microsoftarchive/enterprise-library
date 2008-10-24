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
using System.Security.Principal;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace ExceptionHandlingQuickStart.BusinessLayer
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class AppService
	{
		private ExceptionManager exceptionManager;

		public AppService(ExceptionManager exceptionManager)
		{
			this.exceptionManager = exceptionManager;
		}


		/// <summary>
		/// </summary>
		public void ProcessA()
		{
			throw new System.Exception("Original Exception: Fatal exception in business layer");
		}

		/// <summary>
		/// </summary>
		public void ProcessB()
		{
			throw new System.Data.DBConcurrencyException("Original Exception: Concurrency problem in business layer");
		}

		/// <summary>
		/// </summary>
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
				"\n\nIdentity Values for current thread:" +
				"Identity Name: {3}" +
				"Identity Type: {4}" +
				"Identity Token: {5}",
				principalName, principalType, principalAuth,
				identName, identType, identToken);

			throw new System.Security.SecurityException(identityInfo);
		}

		/// <summary>
		/// </summary>
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
			try
			{
				this.ProcessA();
			}
			catch (Exception ex)
			{
				// Quick Start is configured so that the Propagate Policy will
				// log the exception and then recommend a rethrow.
				bool rethrow = this.exceptionManager.HandleException(ex, "Propagate Policy");

				if (rethrow)
				{
					throw;
				}
			}

			return true;
		}

		/// <summary>
		/// Demonstrates handling of exceptions coming out of a layer. The policy
		/// demonstrated here will show how original exceptions can be wrapped
		/// with a different exception before being propagated back out.
		/// </summary>
		public bool ProcessWithWrap()
		{
			try
			{
				this.ProcessB();
			}
			catch (Exception ex)
			{
				// Quick Start is configured so that the Wrap Policy will
				// log the exception and then recommend a rethrow.
				bool rethrow = this.exceptionManager.HandleException(ex, "Wrap Policy");

				if (rethrow)
				{
					throw;
				}
			}

			return true;
		}

		/// <summary>
		/// </summary>
		public void ProcessWithReplace()
		{
			try
			{
				ProcessC();
			}
			catch (Exception ex)
			{
				// Invoke our policy that is responsible for making sure no secure information
				// gets out of our layer.
				bool rethrow = this.exceptionManager.HandleException(ex, "Replace Policy");

				if (rethrow)
				{
					throw;
				}
			}
		}

		/// <summary>
		/// </summary>
		public void ProcessAndResume()
		{
			try
			{
				ProcessC();
			}
			catch (Exception ex)
			{
				// Invoke our policy that is responsible for making sure no secure information
				// gets out of our layer.
				bool rethrow = this.exceptionManager.HandleException(ex, "Handle and Resume Policy");

				if (rethrow)
				{
					throw;
				}
			}
		}

		/// <summary>
		/// </summary>
		public void ProcessAndNotify()
		{
			try
			{
				ProcessD();
			}
			catch (Exception ex)
			{
				// Invoke our policy that is responsible for making sure no secure information
				// gets out of our layer.
				bool rethrow = this.exceptionManager.HandleException(ex, "Notify Policy");

				if (rethrow)
				{
					throw;
				}
			}
		}
	}
}
