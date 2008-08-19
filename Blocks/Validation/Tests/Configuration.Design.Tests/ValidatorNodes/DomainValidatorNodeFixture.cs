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

using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.ValidatorNodes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.Tests.ValidatorNodes
{
    [TestClass]
    public class DomainValidatorNodeFixture
    {
        [TestMethod]
        public void CreatedNodeHasAppropriateDefaultValues()
        {
            DomainValidatorNode node = new DomainValidatorNode();

            DomainValidatorData validatorData = node.CreateValidatorData() as DomainValidatorData;

            Assert.IsNotNull(validatorData);
            Assert.AreEqual(false, validatorData.Negated);
            Assert.AreEqual(0, validatorData.Domain.Count);

            Assert.AreEqual(false, node.Negated);
            Assert.AreEqual(0, node.Domain.Count);
        }

        [TestMethod]
        public void CreatedNodeWithValidatorDataHasAppropriateValuesFromData()
        {
            DomainValidatorData validatorData = new DomainValidatorData("name");
            validatorData.Negated = true;
            validatorData.Domain.Add(new DomainConfigurationElement("1"));
            validatorData.Domain.Add(new DomainConfigurationElement("2"));
            validatorData.Domain.Add(new DomainConfigurationElement("3"));

            DomainValidatorNode node = new DomainValidatorNode(validatorData);

            Assert.AreEqual("name", node.Name);
            Assert.AreEqual(true, node.Negated);
            Assert.AreEqual(3, node.Domain.Count);
        }

        [TestMethod]
        public void NodeCreatesValidatorDataWithValues()
        {
            DomainValidatorNode node = new DomainValidatorNode();
            node.Name = "validator";
            node.Negated = true;
            node.Domain.Add(new DomainValue("1"));
            node.Domain.Add(new DomainValue("2"));
            node.Domain.Add(new DomainValue("3"));

            DomainValidatorData validatorData = node.CreateValidatorData() as DomainValidatorData;

            Assert.IsNotNull(validatorData);
            Assert.AreEqual("validator", validatorData.Name);
            Assert.AreEqual(true, validatorData.Negated);
            Assert.AreEqual(3, validatorData.Domain.Count);
        }
    }
}