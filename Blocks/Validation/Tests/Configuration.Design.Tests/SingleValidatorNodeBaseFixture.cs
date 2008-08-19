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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.ValidatorNodes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.Tests
{
    [TestClass]
    public class SingleValidatorNodeBaseFixture
    {
        [TestMethod]
        public void ValidatorNodeHasValidatorNameAsNodeName()
        {
            MyValidatorData validatorData = new MyValidatorData("ValidatorName");
            MyValidatorNode validatorNode = new MyValidatorNode(validatorData);

            Assert.AreEqual("ValidatorName", validatorNode.Name);
        }

        [TestMethod]
        public void MessageTemplatePropertiesAreCopiedToNode()
        {
            MyValidatorData validatorData = new MyValidatorData("bla");
            validatorData.MessageTemplate = "MessageTemplate";
            validatorData.MessageTemplateResourceName = "MessageTemplateResourceName";
            validatorData.MessageTemplateResourceTypeName = "MessageTemplateResourceType";

            MyValidatorNode validatorNode = new MyValidatorNode(validatorData);

            Assert.AreEqual("MessageTemplate", validatorNode.MessageTemplate);
            Assert.AreEqual("MessageTemplateResourceName", validatorNode.MessageTemplateResourceName);
            Assert.AreEqual("MessageTemplateResourceType", validatorNode.MessageTemplateResourceTypeName);
        }

        [TestMethod]
        public void MessageTemplatePropertiesAreCopiedToConfigurationData()
        {
            MyValidatorData validatorData = new MyValidatorData("bla");
            MyValidatorNode validatorNode = new MyValidatorNode(validatorData);

            validatorNode.MessageTemplate = "MessageTemplate";
            validatorNode.MessageTemplateResourceName = "MessageTemplateResourceName";
            validatorNode.MessageTemplateResourceTypeName = "MessageTemplateResourceType";

            validatorData = validatorNode.CreateValidatorData() as MyValidatorData;

            Assert.IsNotNull(validatorData);
            Assert.AreEqual("MessageTemplate", validatorData.MessageTemplate);
            Assert.AreEqual("MessageTemplateResourceName", validatorData.MessageTemplateResourceName);
            Assert.AreEqual("MessageTemplateResourceType", validatorData.MessageTemplateResourceTypeName);
        }

        [TestMethod]
        public void SpecifyingBothMessageTempalateAndResourceNameIsInvalid()
        {
            MyValidatorData validatorData = new MyValidatorData("bla");
            validatorData.MessageTemplate = "MessageTemplate";
            validatorData.MessageTemplateResourceName = "MessageTemplateResourceName";

            MyValidatorNode validatorNode = new MyValidatorNode(validatorData);

            List<ValidationError> errors = new List<ValidationError>();
            validatorNode.Validate(errors);

            Assert.AreEqual(1, errors.Count);
        }

        [TestMethod]
        public void TagPropertyIsCopiedToNode()
        {
            MyValidatorData validatorData = new MyValidatorData("bla");
            validatorData.Tag = "tag";

            MyValidatorNode validatorNode = new MyValidatorNode(validatorData);

            Assert.AreEqual("tag", validatorNode.Tag);
        }

        [TestMethod]
        public void TagPropertyIsCopiedToConfigurationData()
        {
            MyValidatorData validatorData = new MyValidatorData("bla");
            MyValidatorNode validatorNode = new MyValidatorNode(validatorData);

            validatorNode.Tag = "tag";

            validatorData = validatorNode.CreateValidatorData() as MyValidatorData;

            Assert.IsNotNull(validatorData);
            Assert.AreEqual("tag", validatorData.Tag);
        }

        class MyValidatorNode : SingleValidatorNodeBase
        {
            public MyValidatorNode(MyValidatorData validatorData)
                : base(validatorData) {}

            public override ValidatorData CreateValidatorData()
            {
                MyValidatorData validatorData = new MyValidatorData(Name);
                SetValidatorBaseProperties(validatorData);
                return validatorData;
            }
        }

        class MyValidatorData : ValidatorData
        {
            public MyValidatorData(string name)
                : base(name, typeof(MyValidatorData)) {}
        }
    }
}