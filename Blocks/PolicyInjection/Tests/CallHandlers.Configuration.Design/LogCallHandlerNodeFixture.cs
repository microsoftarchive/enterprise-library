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

using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Configuration.Design.Tests
{
    [TestClass]
    public class LogCallHandlerFixture
    {
        [TestMethod]
        public void LogCallHandlerNodeHasProperName()
        {
            LogCallHandlerNode node = new LogCallHandlerNode();
            Assert.AreEqual("Logging Handler", node.Name);
        }

        [TestMethod]
        public void LogCallHandlerNodeHasProperDefaultValues()
        {
            LogCallHandlerNode node = new LogCallHandlerNode();
            Assert.AreEqual("Logging Handler", node.Name);
            Assert.AreEqual(LogCallHandlerDefaults.Order, node.Order);
        }

        [TestMethod]
        public void CanCreateCustomAttributeMatchingRuleNodeFromData()
        {
            LogCallHandlerData handlerData = new LogCallHandlerData();
            handlerData.Name = "logHandler";
            handlerData.AfterMessage = "AfterMessage";
            handlerData.BeforeMessage = "BeforeMessage";
            handlerData.EventId = 99;
            handlerData.Categories.Add(new LogCallHandlerCategoryEntry("Cat1"));
            handlerData.Categories.Add(new LogCallHandlerCategoryEntry("Cat2"));
            handlerData.IncludeCallStack = true;
            handlerData.IncludeCallTime = true;
            handlerData.IncludeParameterValues = true;
            handlerData.LogBehavior = HandlerLogBehavior.After;
            handlerData.Priority = 123;
            handlerData.Severity = TraceEventType.Error;
            handlerData.Order = 10;

            LogCallHandlerNode handlerNode = new LogCallHandlerNode(handlerData);
            Assert.AreEqual(handlerData.Name, handlerNode.Name);
            Assert.AreEqual(handlerData.AfterMessage, handlerNode.AfterMessage);
            Assert.AreEqual(handlerData.BeforeMessage, handlerNode.BeforeMessage);
            Assert.AreEqual(handlerData.EventId, handlerNode.EventId);
            Assert.AreEqual(handlerData.IncludeCallStack, handlerNode.IncludeCallStack);
            Assert.AreEqual(handlerData.IncludeCallTime, handlerNode.IncludeCallTime);
            Assert.AreEqual(handlerData.IncludeParameterValues, handlerNode.IncludeParameterValues);
            Assert.AreEqual(handlerData.LogBehavior, handlerNode.LogBehavior);
            Assert.AreEqual(handlerData.Priority, handlerNode.Priority);
            Assert.AreEqual(handlerData.Severity, handlerNode.Severity);
            Assert.AreEqual(handlerData.Categories.Count, handlerNode.Categories.Count);
            Assert.AreEqual(handlerData.Categories.Get(0).Name, handlerNode.Categories[0].CategoryName);
            Assert.AreEqual(handlerData.Categories.Get(1).Name, handlerNode.Categories[1].CategoryName);
            Assert.AreEqual(handlerData.Order, handlerNode.Order);
        }

        [TestMethod]
        public void CanCreateRuleDataFromCustomAttributeMatchingRuleNode()
        {
            LogCallHandlerNode handlerNode = new LogCallHandlerNode();
            handlerNode.Name = "logHandler";
            handlerNode.AfterMessage = "AfterMessage";
            handlerNode.BeforeMessage = "BeforeMessage";
            handlerNode.Categories.Add(new LogCategory("Cat1"));
            handlerNode.Categories.Add(new LogCategory("Cat2"));
            handlerNode.EventId = 124;
            handlerNode.IncludeCallStack = true;
            handlerNode.IncludeCallTime = true;
            handlerNode.IncludeParameterValues = true;
            handlerNode.LogBehavior = HandlerLogBehavior.After;
            handlerNode.Priority = 123;
            handlerNode.Severity = TraceEventType.Error;
            handlerNode.Order = 10;

            LogCallHandlerData handlerData = handlerNode.CreateCallHandlerData() as LogCallHandlerData;

            Assert.IsNotNull(handlerData);
            Assert.AreEqual(handlerNode.Name, handlerData.Name);
            Assert.AreEqual(handlerNode.AfterMessage, handlerData.AfterMessage);
            Assert.AreEqual(handlerNode.BeforeMessage, handlerData.BeforeMessage);
            Assert.AreEqual(handlerNode.EventId, handlerData.EventId);
            Assert.AreEqual(handlerNode.IncludeCallStack, handlerData.IncludeCallStack);
            Assert.AreEqual(handlerNode.IncludeCallTime, handlerData.IncludeCallTime);
            Assert.AreEqual(handlerNode.IncludeParameterValues, handlerData.IncludeParameterValues);
            Assert.AreEqual(handlerNode.LogBehavior, handlerData.LogBehavior);
            Assert.AreEqual(handlerNode.Priority, handlerData.Priority);
            Assert.AreEqual(handlerNode.Severity, handlerData.Severity);
            Assert.AreEqual(handlerNode.Categories.Count, handlerData.Categories.Count);
            Assert.AreEqual(handlerNode.Categories[0].CategoryName, handlerData.Categories.Get(0).Name);
            Assert.AreEqual(handlerNode.Categories[1].CategoryName, handlerData.Categories.Get(1).Name);
            Assert.AreEqual(handlerNode.Order, handlerData.Order);
        }
    }
}
