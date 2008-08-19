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
    public class CustomAttributesValidationAttributeFixture : ConfigurationDesignHost
    {
        readonly PropertyInfo attributeProperty = typeof(NodeWithAttributes).GetProperty("Attributes");

        [TestMethod]
        public void AttributeWithNullKeyCausesError()
        {
            NodeWithAttributes node = new NodeWithAttributes();
            List<ValidationError> errors = new List<ValidationError>();

            node.Attributes.Add(new EditableKeyValue(null, "value"));

            CustomAttributesValidationAttribute validationAttribute = new CustomAttributesValidationAttribute();
            validationAttribute.Validate(node, attributeProperty, errors, ServiceProvider);

            Assert.AreEqual(1, errors.Count);
        }

        [TestMethod]
        public void DuplicateAttributeKeysCauseError()
        {
            NodeWithAttributes node = new NodeWithAttributes();
            List<ValidationError> errors = new List<ValidationError>();

            node.Attributes.Add(new EditableKeyValue("key", "value"));
            node.Attributes.Add(new EditableKeyValue("key", "value"));

            CustomAttributesValidationAttribute validationAttribute = new CustomAttributesValidationAttribute();
            validationAttribute.Validate(node, attributeProperty, errors, ServiceProvider);

            Assert.AreEqual(1, errors.Count);
        }

        [TestMethod]
        public void EmptyCollectionPassesValidation()
        {
            NodeWithAttributes node = new NodeWithAttributes();
            List<ValidationError> errors = new List<ValidationError>();

            CustomAttributesValidationAttribute validationAttribute = new CustomAttributesValidationAttribute();
            validationAttribute.Validate(node, attributeProperty, errors, ServiceProvider);

            Assert.AreEqual(0, errors.Count);
        }

        class NodeWithAttributes : ConfigurationNode
        {
            List<EditableKeyValue> _attributes = new List<EditableKeyValue>();

            [CustomAttributesValidation]
            public List<EditableKeyValue> Attributes
            {
                get { return _attributes; }
            }
        }
    }
}