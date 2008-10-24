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
    public class StringLengthValidatorNodeFixture
    {
        [TestMethod]
        public void CreatedNodeHasAppropriateDefaultValues()
        {
            StringLengthValidatorNode node = new StringLengthValidatorNode();

            Assert.AreEqual(0, node.LowerBound);
            Assert.AreEqual(RangeBoundaryType.Ignore, node.LowerBoundType);
            Assert.AreEqual(0, node.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Inclusive, node.UpperBoundType);
            Assert.AreEqual(false, node.Negated);
        }

        [TestMethod]
        public void CreatedNodeWithValidatorDataHasAppropriateValuesFromData()
        {
            StringLengthValidatorData validatorData = new StringLengthValidatorData("name");
            validatorData.Negated = true;
            validatorData.LowerBound = 10;
            validatorData.LowerBoundType = RangeBoundaryType.Exclusive;
            validatorData.UpperBound = 20;
            validatorData.UpperBoundType = RangeBoundaryType.Inclusive;

            StringLengthValidatorNode node = new StringLengthValidatorNode(validatorData);

            Assert.AreEqual("name", node.Name);
            Assert.AreEqual(true, node.Negated);
            Assert.AreEqual(10, node.LowerBound);
            Assert.AreEqual(RangeBoundaryType.Exclusive, node.LowerBoundType);
            Assert.AreEqual(20, node.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Inclusive, node.UpperBoundType);
        }

        [TestMethod]
        public void NodeCreatesValidatorDataWithValues()
        {
            StringLengthValidatorNode node = new StringLengthValidatorNode();
            node.Name = "validator";
            node.Negated = true;
            node.LowerBound = 10;
            node.LowerBoundType = RangeBoundaryType.Exclusive;
            node.UpperBound = 20;
            node.UpperBoundType = RangeBoundaryType.Inclusive;

            StringLengthValidatorData validatorData = node.CreateValidatorData() as StringLengthValidatorData;

            Assert.IsNotNull(validatorData);
            Assert.AreEqual("validator", validatorData.Name);
            Assert.AreEqual(true, validatorData.Negated);
            Assert.AreEqual(10, validatorData.LowerBound);
            Assert.AreEqual(RangeBoundaryType.Exclusive, validatorData.LowerBoundType);
            Assert.AreEqual(20, validatorData.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Inclusive, validatorData.UpperBoundType);
        }
    }
}
