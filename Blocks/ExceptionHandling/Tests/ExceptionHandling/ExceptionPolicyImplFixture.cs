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
using System.Collections.Generic;
using System.Collections.Specialized;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests
{
    [TestClass]
    public class ExceptionPolicyImplFixture
    {
        ExceptionPolicyData PolicyData
        {
            get
            {
                ExceptionPolicyData data = new ExceptionPolicyData("Policy");
                data.ExceptionTypes.Add(new ExceptionTypeData("Exception", typeof(Exception), PostHandlingAction.ThrowNewException));
                return data;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ExceptionHandlingException))]
        public void HandlerInChainThrowsExceptionWhenProduceError()
        {
            ExceptionPolicyData policyData = PolicyData;
            Dictionary<Type, ExceptionPolicyEntry> entries = GetEntries(policyData);
            ExceptionPolicyDefinition policyIml = new ExceptionPolicyDefinition(policyData.Name, entries);
            policyIml.HandleException(new ArgumentException());
        }

        [TestMethod]
        public void HandleExceptionThatHasNoEntryReturnsTrue()
        {
            ExceptionPolicyData policyData = PolicyData;
            Dictionary<Type, ExceptionPolicyEntry> entries = GetEntries(policyData);
            ExceptionPolicyDefinition policyIml = new ExceptionPolicyDefinition(policyData.Name, entries);
            bool handled = policyIml.HandleException(new InvalidCastException());
            Assert.IsTrue(handled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void HandleExceptionWithNullExceptionThrows()
        {
            ExceptionPolicyData policyData = PolicyData;
            Dictionary<Type, ExceptionPolicyEntry> entries = GetEntries(policyData);
            ExceptionPolicyDefinition policyIml = new ExceptionPolicyDefinition(policyData.Name, entries);
            policyIml.HandleException(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructWithNullNameThrows()
        {
            ExceptionPolicyData policyData = PolicyData;
            new ExceptionPolicyDefinition(null, GetEntries(policyData));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullEntriesThrows()
        {
            ExceptionPolicyData policyData = PolicyData;
            new ExceptionPolicyDefinition(policyData.Name, (Dictionary<Type, ExceptionPolicyEntry>)null);
        }

        static Dictionary<Type, ExceptionPolicyEntry> GetEntries(ExceptionPolicyData policyData)
        {
            Dictionary<Type, ExceptionPolicyEntry> entries = new Dictionary<Type, ExceptionPolicyEntry>();
            List<IExceptionHandler> handlers = new List<IExceptionHandler>();
            handlers.Add(new MockThrowingExceptionHandler());
            handlers.Add(new MockExceptionHandler(new NameValueCollection()));
            foreach (ExceptionTypeData typeData in policyData.ExceptionTypes)
            {
                entries.Add(typeof(ArgumentException), new ExceptionPolicyEntry(typeof(ArgumentException), 
                                                                                typeData.PostHandlingAction,
                                                                                handlers));
            }
            return entries;
        }
    }


    [TestClass]
    public class GivenTwoPolicyEntries
    {
        private MockExceptionHandler handler1 = new MockExceptionHandler(new NameValueCollection());
        private MockExceptionHandler handler2 = new MockExceptionHandler(new NameValueCollection());
        ExceptionPolicyEntry entry1;
        ExceptionPolicyEntry entry2;
        public GivenTwoPolicyEntries()
        {
            entry1 = new ExceptionPolicyEntry(typeof(ArgumentNullException), PostHandlingAction.None, new List<IExceptionHandler>(){handler1});
            entry2 = new ExceptionPolicyEntry(typeof(Exception), PostHandlingAction.None, new List<IExceptionHandler>(){handler2});
        }

        [TestMethod]
        public void WhenRegisteredAgainstExceptionType_OnlyCorrectHandlerIsCalled()
        {

            ExceptionPolicyDefinition policyImpl = new ExceptionPolicyDefinition("APolicyName",
                                                                     new List<ExceptionPolicyEntry>() {entry1, entry2});

            policyImpl.HandleException(new ArgumentNullException("TestException"));
            Assert.AreEqual(1, handler1.instanceHandledExceptionCount);
            Assert.AreEqual(0, handler2.instanceHandledExceptionCount);
        }
    }
}
