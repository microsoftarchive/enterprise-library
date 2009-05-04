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
using System.Collections.Specialized;
using System.Diagnostics;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel.Unity;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.Configuration
{

    [TestClass]
    public class GivenTraceListenerRegistrationAndWrappedRegistration
    {
        private MockEventListener mockListener;
        private AttributeSettingTraceListenerWrapper wrapper;


        [TestInitialize]
        public void Setup()
        {
            mockListener = new MockEventListener();
            mockListener.TraceOutputOptions = TraceOptions.LogicalOperationStack | TraceOptions.DateTime;
            mockListener.IndentSize = 32;
            mockListener.Filter = new EventTypeFilter(SourceLevels.Error);
            mockListener.IndentLevel = 12;

            wrapper
                = new AttributeSettingTraceListenerWrapper(
                    mockListener,
                    new NameValueCollection(){{"foo","bar"}});
        }


        [TestMethod]
        public void ThenSetsAttributeValuesOnWrappedListener()
        {
            Assert.AreEqual("bar", mockListener.Attributes["foo"]);
        }

        [TestMethod]
        public void ThenSetsTraceOptionsOnWrapper()
        {
            Assert.AreEqual(mockListener.TraceOutputOptions, wrapper.TraceOutputOptions);
        }

        [TestMethod]
        public void ThenSetsIndentSizeOnWrapper()
        {
            Assert.AreEqual(mockListener.IndentSize, wrapper.IndentSize);
        }

        [TestMethod]
        public void ThenSetsIndentLevelOnWrapper()
        {
            Assert.AreEqual(mockListener.IndentLevel, wrapper.IndentLevel);
        }

        [TestMethod]
        public void ThenSetsFilterOnWrapper()
        {
            Assert.AreEqual(mockListener.Filter, wrapper.Filter);
        }
    }

    public class MockEventListener : TraceListener 
    {

        public override void Write(string message)
        {
            throw new NotImplementedException();
        }

        public override void WriteLine(string message)
        {
            throw new NotImplementedException();
        }
    }
}

