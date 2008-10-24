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
    public class RelativeDateTimeValidatorNodeFixture
    {
        [TestMethod]
        public void CreatedNodeHasAppropriateDefaultValues()
        {
            RelativeDateTimeValidatorNode node = new RelativeDateTimeValidatorNode();

            RelativeDateTimeValidatorData validatorData = node.CreateValidatorData() as RelativeDateTimeValidatorData;

            Assert.IsNotNull(validatorData);
            Assert.AreEqual(false, validatorData.Negated);
            Assert.AreEqual(0, validatorData.LowerBound);
            Assert.AreEqual(RangeBoundaryType.Ignore, validatorData.LowerBoundType);
            Assert.AreEqual(DateTimeUnit.None, validatorData.LowerUnit);
            Assert.AreEqual(0, validatorData.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Inclusive, validatorData.UpperBoundType);
            Assert.AreEqual(DateTimeUnit.None, validatorData.UpperUnit);

            Assert.AreEqual(false, node.Negated);
            Assert.AreEqual(0, node.LowerBound);
            Assert.AreEqual(RangeBoundaryType.Ignore, node.LowerBoundType);
            Assert.AreEqual(DateTimeUnit.None, node.LowerUnit);
            Assert.AreEqual(0, node.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Inclusive, node.UpperBoundType);
            Assert.AreEqual(DateTimeUnit.None, node.UpperUnit);
        }

        [TestMethod]
        public void CreatedNodeWithValidatorDataHasAppropriateValuesFromData()
        {
            RelativeDateTimeValidatorData validatorData = new RelativeDateTimeValidatorData("name");
            validatorData.Negated = true;
            validatorData.LowerBound = 2;
            validatorData.LowerBoundType = RangeBoundaryType.Exclusive;
            validatorData.LowerUnit = DateTimeUnit.Year;
            validatorData.UpperBound = 3;
            validatorData.UpperBoundType = RangeBoundaryType.Ignore;
            validatorData.UpperUnit = DateTimeUnit.Month;

            RelativeDateTimeValidatorNode node = new RelativeDateTimeValidatorNode(validatorData);

            Assert.AreEqual("name", node.Name);
            Assert.AreEqual(true, node.Negated);
            Assert.AreEqual(2, node.LowerBound);
            Assert.AreEqual(RangeBoundaryType.Exclusive, node.LowerBoundType);
            Assert.AreEqual(DateTimeUnit.Year, node.LowerUnit);
            Assert.AreEqual(3, node.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Ignore, node.UpperBoundType);
            Assert.AreEqual(DateTimeUnit.Month, node.UpperUnit);
        }

        [TestMethod]
        public void NodeCreatesValidatorDataWithValues()
        {
            RelativeDateTimeValidatorNode node = new RelativeDateTimeValidatorNode();
            node.Name = "validator";
            node.Negated = true;
            node.LowerBound = 2;
            node.LowerBoundType = RangeBoundaryType.Exclusive;
            node.LowerUnit = DateTimeUnit.Year;
            node.UpperBound = 3;
            node.UpperBoundType = RangeBoundaryType.Ignore;
            node.UpperUnit = DateTimeUnit.Month;

            RelativeDateTimeValidatorData validatorData = node.CreateValidatorData() as RelativeDateTimeValidatorData;

            Assert.IsNotNull(validatorData);
            Assert.AreEqual("validator", validatorData.Name);
            Assert.AreEqual(true, validatorData.Negated);
            Assert.AreEqual(2, validatorData.LowerBound);
            Assert.AreEqual(RangeBoundaryType.Exclusive, validatorData.LowerBoundType);
            Assert.AreEqual(DateTimeUnit.Year, validatorData.LowerUnit);
            Assert.AreEqual(3, validatorData.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Ignore, validatorData.UpperBoundType);
            Assert.AreEqual(DateTimeUnit.Month, validatorData.UpperUnit);
        }
    }
}
