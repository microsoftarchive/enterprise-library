//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Validation Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Collections.Generic;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.ValidatorNodes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.Tests
{
    [TestClass]
    public class ValidRegexAttributeFixture : ConfigurationDesignHost
    {
        PropertyInfo regexProperty;

        protected override void InitializeCore()
        {
            regexProperty = typeof(TestNode).GetProperty("RegularExpression");
            base.InitializeCore();
        }

        [TestMethod]
        public void EmptyRegexIsValid()
        {
            ValidRegexAttribute validRegexAttribute = new ValidRegexAttribute();
            TestNode testClass = new TestNode(string.Empty);
            List<ValidationError> errors = new List<ValidationError>();

            validRegexAttribute.Validate(testClass, regexProperty, errors, ServiceProvider);
            Assert.AreEqual(0, errors.Count);
        }

        [TestMethod]
        public void ValidRegexDoesnReturnErrors()
        {
            ValidRegexAttribute validRegexAttribute = new ValidRegexAttribute();
            TestNode testClass = new TestNode("abc");
            List<ValidationError> errors = new List<ValidationError>();

            validRegexAttribute.Validate(testClass, regexProperty, errors, ServiceProvider);
            Assert.AreEqual(0, errors.Count);
        }

        [TestMethod]
        public void InvalidRegexReturnsErrors()
        {
            ValidRegexAttribute validRegexAttribute = new ValidRegexAttribute();
            TestNode testClass = new TestNode("\\");
            List<ValidationError> errors = new List<ValidationError>();

            validRegexAttribute.Validate(testClass, regexProperty, errors, ServiceProvider);
            Assert.AreEqual(1, errors.Count);
        }

        class TestNode : ConfigurationNode
        {
            string regularExpression;

            public TestNode(string regularExpression)
            {
                this.regularExpression = regularExpression;
            }

            public string RegularExpression
            {
                get { return regularExpression; }
                set { regularExpression = value; }
            }
        }
    }
}