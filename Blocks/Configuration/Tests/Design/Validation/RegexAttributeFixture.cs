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
    public class RegexAttributeFixture : ConfigurationDesignHost
    {
        MyRegexTestNode regexNode;
        PropertyInfo emailInfo;
        IConfigurationUIHierarchy hierarchy;
        IServiceProvider serviceProvider;

        protected override void InitializeCore()
        {
            regexNode = new MyRegexTestNode();
            ApplicationNode.AddNode(regexNode);
            emailInfo = regexNode.GetType().GetProperty("Email");
            serviceProvider = ServiceBuilder.Build();
            hierarchy = new ConfigurationUIHierarchy(ApplicationNode, serviceProvider);
        }

        protected override void CleanupCore()
        {
            hierarchy.Dispose();
        }

        [TestMethod]
        public void RegexViolationTest()
        {
            RegexAttribute attribute = new RegexAttribute(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
            regexNode.Email = "joeblow";
            List<ValidationError> errors = new List<ValidationError>();
            attribute.Validate(regexNode, emailInfo, errors, ServiceProvider);
            Assert.AreEqual(1, errors.Count);
        }

        [TestMethod]
        public void RegexTest()
        {
            RegexAttribute attribute = new RegexAttribute(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
            regexNode.Email = "someone@microsoft.com";
            List<ValidationError> errors = new List<ValidationError>();
            attribute.Validate(regexNode, emailInfo, errors, ServiceProvider);
            Assert.AreEqual(0, errors.Count);
        }

        [TestMethod]
        public void RegexViolationTestWithValidateCommand()
        {
            ValidateNodeCommand cmd = new ValidateNodeCommand(serviceProvider);
            Assert.IsNotNull(regexNode.Site);
            regexNode.Email = "joeblow.com";
            cmd.Execute(regexNode);

            Assert.AreEqual(1, ValidationAttributeHelper.GetValidationErrorsCount(serviceProvider));
        }

        [TestMethod]
        public void RegexTestWithValidateCommand()
        {
            ValidateNodeCommand cmd = new ValidateNodeCommand(serviceProvider);
            Assert.IsNotNull(regexNode.Site);
            regexNode.Email = "someone@microsoft.com";
            cmd.Execute(regexNode);

            Assert.AreEqual(0, ValidationAttributeHelper.GetConfigurationErrorsCount(serviceProvider));
        }

        class MyRegexTestNode : ConfigurationNode
        {
            string email;

            public MyRegexTestNode()
                : base() {}

            [Regex(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$")]
            public string Email
            {
                get { return email; }
                set { email = value; }
            }
        }
    }
}