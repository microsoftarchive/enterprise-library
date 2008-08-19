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
    public class RangeValidatorNodeFixture
    {
        [TestMethod]
        public void CreatedNodeHasAppropriateDefaultValues()
        {
            RangeValidatorNode node = new RangeValidatorNode();

            RangeValidatorData validatorData = node.CreateValidatorData() as RangeValidatorData;

            Assert.IsNotNull(validatorData);
            Assert.AreEqual(false, validatorData.Negated);
            Assert.AreEqual(string.Empty, validatorData.LowerBound);
            Assert.AreEqual(RangeBoundaryType.Ignore, validatorData.LowerBoundType);
            Assert.AreEqual(string.Empty, validatorData.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Inclusive, validatorData.UpperBoundType);

            Assert.AreEqual(false, node.Negated);
            Assert.AreEqual(string.Empty, node.LowerBound);
            Assert.AreEqual(RangeBoundaryType.Ignore, node.LowerBoundType);
            Assert.AreEqual(string.Empty, node.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Inclusive, node.UpperBoundType);
        }

        [TestMethod]
        public void CreatedNodeWithValidatorDataHasAppropriateValuesFromData()
        {
            RangeValidatorData validatorData = new RangeValidatorData("name");
            validatorData.Negated = true;
            validatorData.LowerBound = "2";
            validatorData.LowerBoundType = RangeBoundaryType.Exclusive;
            validatorData.UpperBound = "3";
            validatorData.UpperBoundType = RangeBoundaryType.Ignore;
            validatorData.MessageTemplate = "message template";

            RangeValidatorNode node = new RangeValidatorNode(validatorData);

            Assert.AreEqual("name", node.Name);
            Assert.AreEqual(true, node.Negated);
            Assert.AreEqual("2", node.LowerBound);
            Assert.AreEqual(RangeBoundaryType.Exclusive, node.LowerBoundType);
            Assert.AreEqual("3", node.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Ignore, node.UpperBoundType);
            Assert.AreEqual("message template", node.MessageTemplate);
        }

        [TestMethod]
        public void NodeCreatesValidatorDataWithValues()
        {
            RangeValidatorNode node = new RangeValidatorNode();
            node.Name = "validator";
            node.Negated = true;
            node.LowerBound = "2";
            node.LowerBoundType = RangeBoundaryType.Exclusive;
            node.UpperBound = "3";
            node.UpperBoundType = RangeBoundaryType.Ignore;
            node.MessageTemplate = "message template";

            RangeValidatorData validatorData = node.CreateValidatorData() as RangeValidatorData;

            Assert.IsNotNull(validatorData);
            Assert.AreEqual("validator", validatorData.Name);
            Assert.AreEqual(true, validatorData.Negated);
            Assert.AreEqual("2", validatorData.LowerBound);
            Assert.AreEqual(RangeBoundaryType.Exclusive, validatorData.LowerBoundType);
            Assert.AreEqual("3", validatorData.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Ignore, validatorData.UpperBoundType);
            Assert.AreEqual("message template", validatorData.MessageTemplate);
        }
    }
}