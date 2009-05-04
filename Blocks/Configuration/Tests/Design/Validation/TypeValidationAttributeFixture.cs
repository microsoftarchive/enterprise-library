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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation.Tests
{
    [TestClass]
    public class TypeValidationAttributeFixture : ConfigurationDesignHost
    {
        MyTypeTestNode typeNode;
        PropertyInfo valueInfo1;
        IConfigurationUIHierarchy hierarchy;
        IServiceProvider serviceProvider;

        protected override void InitializeCore()
        {
            ConfigurationApplicationNode appNode = new ConfigurationApplicationNode();
            typeNode = new MyTypeTestNode();
            valueInfo1 = typeNode.GetType().GetProperty("TypeName");
            serviceProvider = ServiceBuilder.Build();
            appNode.AddNode(typeNode);
            hierarchy = new ConfigurationUIHierarchy(appNode, serviceProvider);
        }

        protected override void CleanupCore()
        {
            hierarchy.Dispose();
        }

        [TestMethod]
        public void ValidTypeProducesNoErrors()
        {
            TypeValidationAttribute attribute = new TypeValidationAttribute();
            List<ValidationError> errors = new List<ValidationError>();
            typeNode.TypeName = GetType().AssemblyQualifiedName;
            attribute.Validate(typeNode, valueInfo1, errors, ServiceProvider);
            Assert.AreEqual(0, errors.Count);
        }

        [TestMethod]
        public void InvalidTypeProducesAnError()
        {
            TypeValidationAttribute attribute = new TypeValidationAttribute();
            typeNode.TypeName = "MyTest";
            List<ValidationError> errors = new List<ValidationError>();
            attribute.Validate(typeNode, valueInfo1, errors, ServiceProvider);
            Assert.AreEqual(1, errors.Count);
        }

        class MyTypeTestNode : ConfigurationNode
        {
            string value1;

            public MyTypeTestNode()
                : base() { }

            [TypeValidation]
            public string TypeName
            {
                get { return value1; }
                set { value1 = value; }
            }
        }
    }
}
