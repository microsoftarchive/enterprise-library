//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation.Tests
{
    [TestClass]
    public class MaximumLengthAttributeFixture : ConfigurationDesignHost
    {
        MyMaxLengthTestNode maxLengthNode;
        PropertyInfo valueInfo1;
        PropertyInfo valueInfo2;
        IConfigurationUIHierarchy hierarchy;
        IServiceProvider serviceProvider;

        protected override void InitializeCore()
        {
            ConfigurationApplicationNode appNode = new ConfigurationApplicationNode();
            maxLengthNode = new MyMaxLengthTestNode();
            appNode.AddNode(maxLengthNode);
            valueInfo1 = maxLengthNode.GetType().GetProperty("Value1");
            valueInfo2 = maxLengthNode.GetType().GetProperty("Value2");
            serviceProvider = ServiceBuilder.Build();
            hierarchy = new ConfigurationUIHierarchy(appNode, serviceProvider);
        }

        protected override void CleanupCore()
        {
            hierarchy.Dispose();
        }

        [TestMethod]
        public void MaxLengthViolationTest()
        {
            MaximumLengthAttribute attribute = new MaximumLengthAttribute(3);
            maxLengthNode.Value1 = "aaaa";
            List<ValidationError> errors = new List<ValidationError>();
            attribute.Validate(maxLengthNode, valueInfo1, errors, ServiceProvider);

            Assert.AreEqual(1, errors.Count);
        }

        [TestMethod]
        public void MaxLengthTest()
        {
            MaximumLengthAttribute attribute = new MaximumLengthAttribute(8);
            maxLengthNode.Value2 = "aaaa";
            List<ValidationError> errors = new List<ValidationError>();
            attribute.Validate(maxLengthNode, valueInfo2, errors, ServiceProvider);

            Assert.AreEqual(0, errors.Count);
        }

        [TestMethod]
        public void MaxLengthViolationTestWithCommand()
        {
            maxLengthNode.Value1 = "MyTest";
            maxLengthNode.Value2 = "MyTestPassword";
            ValidateNodeCommand cmd = new ValidateNodeCommand(serviceProvider);
            cmd.Execute(maxLengthNode);

            Assert.AreEqual(2, ValidationAttributeHelper.GetValidationErrorsCount(serviceProvider));
        }

        [TestMethod]
        public void MaxLengthTestWithCommand()
        {
            maxLengthNode.Value1 = "aaa";
            maxLengthNode.Value2 = "aaaaaa";
            ValidateNodeCommand cmd = new ValidateNodeCommand(serviceProvider);
            cmd.Execute(maxLengthNode);

            Assert.AreEqual(0, ValidationAttributeHelper.GetValidationErrorsCount(serviceProvider));
        }

        class MyMaxLengthTestNode : ConfigurationNode
        {
            string value1;
            string value2;

            public MyMaxLengthTestNode()
                : base("Test") {}

            [MaximumLength(3)]
            public string Value1
            {
                get { return value1; }
                set { value1 = value; }
            }

            [MaximumLength(8)]
            public string Value2
            {
                get { return value2; }
                set { value2 = value; }
            }
        }
    }
}