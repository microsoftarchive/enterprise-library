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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.TestSupport.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation.Tests
{
    [TestClass]
    public class MinimumLengthAttributeFixture : ConfigurationDesignHost
    {
        MyMinLengthTestNode minLengthNode;
        PropertyInfo valueInfo1;
        PropertyInfo valueInfo2;
        IConfigurationUIHierarchy hierarchy;
        IServiceProvider serviceProvider;

        protected override void InitializeCore()
        {
            ConfigurationApplicationNode appNode = new ConfigurationApplicationNode();
            minLengthNode = new MyMinLengthTestNode();
            appNode.AddNode(minLengthNode);
            valueInfo1 = minLengthNode.GetType().GetProperty("Value1");
            valueInfo2 = minLengthNode.GetType().GetProperty("Value2");
            serviceProvider = ServiceBuilder.Build();
            hierarchy = new ConfigurationUIHierarchy(appNode, serviceProvider);
        }

        protected override void CleanupCore()
        {
            hierarchy.Dispose();
        }

        [TestMethod]
        public void MinLengthViolationWithNull()
        {
            MinimumLengthAttribute attribute = new MinimumLengthAttribute(8);
            List<ValidationError> errors = new List<ValidationError>();
            attribute.Validate(minLengthNode, valueInfo1, errors, ServiceProvider);

            Assert.AreEqual(1, errors.Count);
        }

        [TestMethod]
        public void MinLengthViolationWithEmptyString()
        {
            MinimumLengthAttribute attribute = new MinimumLengthAttribute(8);
            List<ValidationError> errors = new List<ValidationError>();
            minLengthNode.Value1 = string.Empty;
            attribute.Validate(minLengthNode, valueInfo1, errors, ServiceProvider);

            Assert.AreEqual(1, errors.Count);
        }

        [TestMethod]
        public void MinLengthTest()
        {
            MinimumLengthAttribute attribute = new MinimumLengthAttribute(8);
            minLengthNode.Value2 = "MyTestPassword";
            List<ValidationError> errors = new List<ValidationError>();
            attribute.Validate(minLengthNode, valueInfo2, errors, ServiceProvider);

            Assert.AreEqual(0, errors.Count);
        }

        [TestMethod]
        public void MinLengthViolationTestWithValidateCommand()
        {
            ValidateNodeCommand cmd = new ValidateNodeCommand(serviceProvider);
            cmd.Execute(minLengthNode);

            Assert.AreEqual(2, ValidationAttributeHelper.GetValidationErrorsCount(serviceProvider));
        }

        [TestMethod]
        public void MinLengthTestWithValidateCommand()
        {
            ValidateNodeCommand cmd = new ValidateNodeCommand(serviceProvider);
            minLengthNode.Value1 = "MyTest";
            minLengthNode.Value2 = "MyTestPassword";
            cmd.Execute(minLengthNode);

            Assert.AreEqual(0, ValidationAttributeHelper.GetValidationErrorsCount(serviceProvider));
        }

        class MyMinLengthTestNode : ConfigurationNode
        {
            string value1;
            string value2;

            public MyMinLengthTestNode()
                : base("Test") { }

            [MinimumLength(3)]
            public string Value1
            {
                get { return value1; }
                set { value1 = value; }
            }

            [MinimumLength(8)]
            public string Value2
            {
                get { return value2; }
                set { value2 = value; }
            }
        }
    }
}
