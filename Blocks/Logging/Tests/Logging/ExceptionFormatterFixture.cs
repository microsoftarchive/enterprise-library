//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.Remoting.Messaging;
using System.Security;
using System.Security.Permissions;
using System.Text.RegularExpressions;
using Microsoft.Practices.EnterpriseLibrary.Logging.Properties;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners.Tests;
#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#endif

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests
{
	[TestClass]
	public class ExceptionFormatterFixture
	{
		private Regex DemandedEntryRegex = new Regex("^Demanded: (.*)$", RegexOptions.Multiline);

		[TestMethod]
		public void CanReflectOnSecurityExceptionWithoutPermission()
		{
			SecurityPermission denyPermission
				= new SecurityPermission(SecurityPermissionFlag.ControlPolicy | SecurityPermissionFlag.ControlEvidence);
			PermissionSet permissions = new PermissionSet(PermissionState.None);
			permissions.AddPermission(denyPermission);
			permissions.Deny();

			SecurityException exception = null;
			try
			{
				DemandException(denyPermission);
			}
			catch (SecurityException e)
			{
				exception = e;
			}
			
			ExceptionFormatter formatter = new ExceptionFormatter();

			String message = formatter.GetMessage(exception);
			Match demandedMatch = DemandedEntryRegex.Match(message);
			Assert.IsNotNull(demandedMatch);
			Assert.AreEqual(Resources.PropertyAccessFailed, demandedMatch.Groups[1].Value);

			CodeAccessPermission.RevertDeny();

			message = formatter.GetMessage(exception);
			demandedMatch = DemandedEntryRegex.Match(message);
			Assert.IsNotNull(demandedMatch);
			Assert.AreNotEqual(Resources.PropertyAccessFailed, demandedMatch.Groups[1].Value);
		}

		private static void DemandException(SecurityPermission denyPermission)
		{
			denyPermission.Demand();
		}
	}
}
