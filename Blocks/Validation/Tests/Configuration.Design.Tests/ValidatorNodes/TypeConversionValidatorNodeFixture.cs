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
    public class TypeConversionValidatorNodeFixture
    {
        [TestMethod]
        public void CreatedNodeWithValidatorDataHasAppropriateValuesFromData()
        {
            TypeConversionValidatorData validatorData = new TypeConversionValidatorData("name");
            validatorData.Negated = true;
            validatorData.TargetType = typeof(double);

            TypeConversionValidatorNode node = new TypeConversionValidatorNode(validatorData);

            Assert.AreEqual("name", node.Name);
            Assert.AreEqual(true, node.Negated);
            Assert.AreEqual(typeof(double).AssemblyQualifiedName, node.TargetType);
        }

        [TestMethod]
        public void NodeCreatesValidatorDataWithValues()
        {
            TypeConversionValidatorNode node = new TypeConversionValidatorNode();
            node.Name = "validator";
            node.Negated = true;
            node.TargetType = typeof(double).AssemblyQualifiedName;

            TypeConversionValidatorData validatorData = node.CreateValidatorData() as TypeConversionValidatorData;

            Assert.IsNotNull(validatorData);
            Assert.AreEqual("validator", validatorData.Name);
            Assert.AreEqual(true, validatorData.Negated);
            Assert.AreEqual(typeof(double), validatorData.TargetType);
        }
    }
}
