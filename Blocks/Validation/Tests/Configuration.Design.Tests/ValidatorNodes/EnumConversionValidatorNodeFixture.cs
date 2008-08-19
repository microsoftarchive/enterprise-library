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
    enum MockEnumValidator
    {
        MyEnumValue,
        AnotherEnumValue
    }

    [TestClass]
    public class EnumConversionValidatorNodeFixture
    {
        [TestMethod]
        public void CreatedNodeHasAppropriateDefaultValues()
        {
            EnumConversionValidatorNode node = new EnumConversionValidatorNode();

            EnumConversionValidatorData validatorData = node.CreateValidatorData() as EnumConversionValidatorData;

            Assert.IsNotNull(validatorData);
            Assert.AreEqual(false, validatorData.Negated);
            Assert.IsNull(validatorData.EnumType);

            Assert.AreEqual(false, node.Negated);
            Assert.AreEqual(string.Empty, node.EnumType);
        }

        [TestMethod]
        public void CreatedNodeWithValidatorDataHasAppropriateValuesFromData()
        {
            EnumConversionValidatorData validatorData = new EnumConversionValidatorData("name");
            validatorData.Negated = true;
            validatorData.EnumType = typeof(MockEnumValidator);

            EnumConversionValidatorNode node = new EnumConversionValidatorNode(validatorData);

            Assert.AreEqual("name", node.Name);
            Assert.AreEqual(true, node.Negated);
            Assert.AreEqual(validatorData.EnumTypeName, node.EnumType);
        }

        [TestMethod]
        public void NodeCreatesValidatorDataWithValues()
        {
            EnumConversionValidatorNode node = new EnumConversionValidatorNode();
            node.Name = "validator";
            node.Negated = true;
            node.EnumType = typeof(MockEnumValidator).AssemblyQualifiedName;

            EnumConversionValidatorData validatorData = node.CreateValidatorData() as EnumConversionValidatorData;

            Assert.IsNotNull(validatorData);
            Assert.AreEqual("validator", validatorData.Name);
            Assert.AreEqual(true, validatorData.Negated);
            Assert.AreEqual(typeof(MockEnumValidator), validatorData.EnumType);
        }
    }
}