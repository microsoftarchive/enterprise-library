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
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.Tests.ValidatorNodes
{
    [TestClass]
    public class ContainsCharactersValidatorNodeFixture
    {
        [TestMethod]
        public void CreatedNodeHasAppropriateDefaultValues()
        {
            ContainsCharactersValidatorNode node = new ContainsCharactersValidatorNode();

            ContainsCharactersValidatorData validatorData = node.CreateValidatorData() as ContainsCharactersValidatorData;

            Assert.IsNotNull(validatorData);
            Assert.AreEqual(string.Empty, validatorData.CharacterSet);
            Assert.AreEqual(false, validatorData.Negated);
            Assert.AreEqual(ContainsCharacters.Any, validatorData.ContainsCharacters);

            Assert.AreEqual(string.Empty, node.CharacterSet);
            Assert.AreEqual(false, node.Negated);
            Assert.AreEqual(ContainsCharacters.Any, node.ContainsCharacters);
        }

        [TestMethod]
        public void CreatedNodeWithValidatorDataHasAppropriateValuesFromData()
        {
            ContainsCharactersValidatorData validatorData = new ContainsCharactersValidatorData("name");
            validatorData.CharacterSet = "abc";
            validatorData.ContainsCharacters = ContainsCharacters.All;
            validatorData.Negated = true;

            ContainsCharactersValidatorNode node = new ContainsCharactersValidatorNode(validatorData);

            Assert.AreEqual("name", node.Name);
            Assert.AreEqual("abc", node.CharacterSet);
            Assert.AreEqual(true, node.Negated);
            Assert.AreEqual(ContainsCharacters.All, node.ContainsCharacters);
        }

        [TestMethod]
        public void NodeCreatesValidatorDataWithValues()
        {
            ContainsCharactersValidatorNode node = new ContainsCharactersValidatorNode();
            node.Name = "validator";
            node.CharacterSet = "abc";
            node.Negated = true;
            node.ContainsCharacters = ContainsCharacters.All;

            ContainsCharactersValidatorData validatorData = node.CreateValidatorData() as ContainsCharactersValidatorData;

            Assert.IsNotNull(validatorData);
            Assert.AreEqual("validator", node.Name);
            Assert.AreEqual("validator", validatorData.Name);
            Assert.AreEqual("abc", validatorData.CharacterSet);
            Assert.AreEqual(true, validatorData.Negated);
            Assert.AreEqual(ContainsCharacters.All, validatorData.ContainsCharacters);
        }
    }
}