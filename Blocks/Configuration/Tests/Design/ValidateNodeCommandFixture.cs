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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests
{
    [TestClass]
    public class ValidateNodeCommandFixture
    {
        [TestMethod]
        public void TestThatAValidationThatFailsReportsErrorsToUI()
        {
            TestNode node = new TestNode();
            IServiceProvider provider = ServiceBuilder.Build();
            ValidateNodeCommand cmd = new ValidateNodeCommand(provider, true, false);
            cmd.Execute(node);
            MockUIService uiService = provider.GetService(typeof(IUIService)) as MockUIService;

            Assert.AreEqual(1, uiService.ValidationErrorsCount);
        }

        class TestNode : ConfigurationNode
        {
            public TestNode()
                : base("node") {}

            [Required]
            public string LastName
            {
                get { return null; }
            }
        }
    }
}
