//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Policy Injection Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Configuration.Design.Tests
{
    [TestClass]
    public class PerformanceCounterCallHandlerFixture
    {
        [TestMethod]
        public void PerformanceCounterCallHandlerNodeHasProperName()
        {
            PerformanceCounterCallHandlerNode node = new PerformanceCounterCallHandlerNode();
            Assert.AreEqual("Performance Counters Handler", node.Name);
        }

        [TestMethod]
        public void PerformanceCounterCallHandlerNodeHasProperDefaults()
        {
            PerformanceCounterCallHandlerNode node = new PerformanceCounterCallHandlerNode();
            Assert.AreEqual("Performance Counters Handler", node.Name);
            Assert.AreEqual(0, node.Order);
        }

        [TestMethod]
        public void CanCreateCustomAttributeMatchingRuleNodeFromData()
        {
            PerformanceCounterCallHandlerData handlerData = new PerformanceCounterCallHandlerData();
            handlerData.Name = "perfCounterHandler";
            handlerData.CategoryName = "categoryName";
            handlerData.InstanceName = "instanceName";
            handlerData.IncrementAverageCallDuration = true;
            handlerData.IncrementCallsPerSecond = true;
            handlerData.IncrementExceptionsPerSecond = true;
            handlerData.IncrementNumberOfCalls = true;
            handlerData.IncrementTotalExceptions = true;
            handlerData.UseTotalCounter = true;
            handlerData.Order = 5;

            PerformanceCounterCallHandlerNode handlerNode = new PerformanceCounterCallHandlerNode(handlerData);
            Assert.AreEqual(handlerData.Name, handlerNode.Name);
            Assert.AreEqual(handlerData.CategoryName, handlerNode.CategoryName);
            Assert.AreEqual(handlerData.InstanceName, handlerNode.InstanceName);
            Assert.AreEqual(handlerData.IncrementAverageCallDuration, handlerNode.IncrementAverageCallDuration);
            Assert.AreEqual(handlerData.IncrementCallsPerSecond, handlerNode.IncrementCallsPerSecond);
            Assert.AreEqual(handlerData.IncrementExceptionsPerSecond, handlerNode.IncrementExceptionsPerSecond);
            Assert.AreEqual(handlerData.IncrementNumberOfCalls, handlerNode.IncrementNumberOfCalls);
            Assert.AreEqual(handlerData.IncrementTotalExceptions, handlerNode.IncrementTotalExceptions);
            Assert.AreEqual(handlerData.UseTotalCounter, handlerNode.UseTotalCounter);
            Assert.AreEqual(handlerData.Order, handlerNode.Order);
        }

        [TestMethod]
        public void CanCreateRuleDataFromCustomAttributeMatchingRuleNode()
        {
            PerformanceCounterCallHandlerNode handlerNode = new PerformanceCounterCallHandlerNode();
            handlerNode.Name = "perfCounterHandler";
            handlerNode.CategoryName = "categoryName";
            handlerNode.InstanceName = "instanceName";
            handlerNode.IncrementAverageCallDuration = true;
            handlerNode.IncrementCallsPerSecond = true;
            handlerNode.IncrementExceptionsPerSecond = true;
            handlerNode.IncrementNumberOfCalls = true;
            handlerNode.IncrementTotalExceptions = true;
            handlerNode.UseTotalCounter = true;

            PerformanceCounterCallHandlerData handlerData = handlerNode.CreateCallHandlerData() as PerformanceCounterCallHandlerData;

            Assert.IsNotNull(handlerData);
            Assert.AreEqual(handlerNode.Name, handlerData.Name);
            Assert.AreEqual(handlerNode.CategoryName, handlerData.CategoryName);
            Assert.AreEqual(handlerNode.InstanceName, handlerData.InstanceName);
            Assert.AreEqual(handlerNode.IncrementAverageCallDuration, handlerData.IncrementAverageCallDuration);
            Assert.AreEqual(handlerNode.IncrementCallsPerSecond, handlerData.IncrementCallsPerSecond);
            Assert.AreEqual(handlerNode.IncrementExceptionsPerSecond, handlerData.IncrementExceptionsPerSecond);
            Assert.AreEqual(handlerNode.IncrementNumberOfCalls, handlerData.IncrementNumberOfCalls);
            Assert.AreEqual(handlerNode.IncrementTotalExceptions, handlerData.IncrementTotalExceptions);
            Assert.AreEqual(handlerNode.UseTotalCounter, handlerData.UseTotalCounter);
        }
    }
}
