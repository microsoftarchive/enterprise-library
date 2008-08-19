//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners.Tests;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Tests.Configuration.Unity
{
	[TestClass]
	public class LoggingExceptionHandlerPolicyCreatorFixture
	{
		private IUnityContainer container;

		[TestInitialize]
		public void SetUp()
		{
			container = new UnityContainer();
			container.AddExtension(new EnterpriseLibraryCoreExtension());
		}

		[TestCleanup]
		public void TearDown()
		{
			container.Dispose();
		}

		// test logic cloned from LoggingExceptionHandlerFixture, using the same configuration file
		[TestMethod]
		public void ExceptionHandledThroughLoggingBlock()
		{
			MockTraceListener.Reset();

			container.AddExtension(new ExceptionHandlingBlockExtension());
			container.AddExtension(new LoggingBlockExtension());

			ExceptionPolicyImpl policy = container.Resolve<ExceptionPolicyImpl>("Logging Policy");

			Assert.IsFalse(policy.HandleException(new Exception("TEST EXCEPTION")));

			Assert.AreEqual(1, MockTraceListener.LastEntry.Categories.Count);
			Assert.IsTrue(MockTraceListener.LastEntry.Categories.Contains("TestCat"));
			Assert.AreEqual(5, MockTraceListener.LastEntry.EventId);
			Assert.AreEqual(TraceEventType.Error, MockTraceListener.LastEntry.Severity);
			Assert.AreEqual("TestTitle", MockTraceListener.LastEntry.Title);
		}

		[TestMethod]
		public void MultipleRequestsUseSameLogWriterInstance()
		{
			MockTraceListener.Reset();

			container.AddExtension(new ExceptionHandlingBlockExtension());
			container.AddExtension(new LoggingBlockExtension());

			{
				ExceptionPolicyImpl policy = container.Resolve<ExceptionPolicyImpl>("Logging Policy");
				Assert.IsFalse(policy.HandleException(new Exception("TEST EXCEPTION")));
			}
			{
				ExceptionPolicyImpl policy = container.Resolve<ExceptionPolicyImpl>("Logging Policy");
				Assert.IsFalse(policy.HandleException(new Exception("TEST EXCEPTION")));
			}
			{
				ExceptionPolicyImpl policy = container.Resolve<ExceptionPolicyImpl>("Logging Policy");
				Assert.IsFalse(policy.HandleException(new Exception("TEST EXCEPTION")));
			}

			Assert.AreEqual(3, MockTraceListener.Entries.Count);
			Assert.AreEqual(3, MockTraceListener.Instances.Count);
			Assert.AreSame(MockTraceListener.Instances[0], MockTraceListener.Instances[1]);
			Assert.AreSame(MockTraceListener.Instances[0], MockTraceListener.Instances[2]);
		}
	}
}
