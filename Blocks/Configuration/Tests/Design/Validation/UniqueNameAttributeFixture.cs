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

using System.Collections.Generic;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation.Tests
{
    [TestClass]
    public class UniqueNameAttributeFixture : ConfigurationDesignHost
    {
        UniqueNameTestNode childOfParent;
        UniqueNameTestNode otherChildOfParent;

        PropertyInfo uniqueNameInfo;
        UniqueNameAttribute attribute;

        protected override void InitializeCore()
        {
            NonUniqueNameTestNode parent = new NonUniqueNameTestNode("Parent");
            NonUniqueNameTestNode nonUniqueNameChildOfParent = new NonUniqueNameTestNode("uniqueName");
            childOfParent = new UniqueNameTestNode("uniqueName");
            otherChildOfParent = new UniqueNameTestNode("other name");
            UniqueNameTestNode siblingOfParent = new UniqueNameTestNode("uniqueName");

            ApplicationNode.AddNode(parent);
            ApplicationNode.AddNode(siblingOfParent);
            parent.AddNode(childOfParent);
            parent.AddNode(otherChildOfParent);
            parent.AddNode(nonUniqueNameChildOfParent);

            uniqueNameInfo = typeof(UniqueNameTestNode).GetProperty("Name");
            attribute = new UniqueNameAttribute(typeof(UniqueNameTestNode), typeof(ConfigurationApplicationNode));
        }

        [TestMethod]
        public void UniqueNameFailureTest()
        {
            List<ValidationError> errors = new List<ValidationError>();
            attribute.Validate(childOfParent, uniqueNameInfo, errors, ServiceProvider);
            Assert.AreEqual(1, errors.Count);
        }

        [TestMethod]
        public void UniqueNameValidTest()
        {
            List<ValidationError> errors = new List<ValidationError>();
            attribute.Validate(otherChildOfParent, uniqueNameInfo, errors, ServiceProvider);
            Assert.AreEqual(0, errors.Count);
        }

        class UniqueNameTestNode : ConfigurationNode
        {
            public UniqueNameTestNode(string name)
                : base(name) {}

            [UniqueName(typeof(UniqueNameTestNode), typeof(ConfigurationApplicationNode))]
            public override string Name
            {
                get { return base.Name; }
                set { base.Name = value; }
            }
        }

        class NonUniqueNameTestNode : ConfigurationNode
        {
            public NonUniqueNameTestNode(string name)
                : base(name) {}
        }
    }
}
