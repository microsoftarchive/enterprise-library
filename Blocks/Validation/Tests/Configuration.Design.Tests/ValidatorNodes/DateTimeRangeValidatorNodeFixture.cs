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

using System;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.ValidatorNodes;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.Tests.ValidatorNodes
{
    [TestClass]
    public class DateTimeRangeValidatorNodeFixture
    {
        [TestMethod]
        public void CreatedNodeHasAppropriateDefaultValues()
        {
            DateRangeValidatorNode node = new DateRangeValidatorNode();

            DateTimeRangeValidatorData validatorData = node.CreateValidatorData() as DateTimeRangeValidatorData;

            Assert.IsNotNull(validatorData);
            Assert.AreEqual(false, validatorData.Negated);
            Assert.AreEqual(DateTime.MinValue, validatorData.LowerBound);
            Assert.AreEqual(RangeBoundaryType.Ignore, validatorData.LowerBoundType);
            Assert.AreEqual(DateTime.MinValue, validatorData.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Inclusive, validatorData.UpperBoundType);

            Assert.AreEqual(false, node.Negated);
            Assert.AreEqual(DateTime.MinValue, node.LowerBound);
            Assert.AreEqual(RangeBoundaryType.Ignore, node.LowerBoundType);
            Assert.AreEqual(DateTime.MinValue, node.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Inclusive, node.UpperBoundType);
        }

        [TestMethod]
        public void CreatedNodeWithValidatorDataHasAppropriateValuesFromData()
        {
            DateTime lowerBound = new DateTime(2006, 1, 1);
            DateTime upperBound = new DateTime(2006, 1, 10);

            DateTimeRangeValidatorData validatorData = new DateTimeRangeValidatorData("name");
            validatorData.Negated = true;
            validatorData.LowerBound = lowerBound;
            validatorData.LowerBoundType = RangeBoundaryType.Exclusive;
            validatorData.UpperBound = upperBound;
            validatorData.UpperBoundType = RangeBoundaryType.Ignore;
            validatorData.MessageTemplate = "message template";

            DateRangeValidatorNode node = new DateRangeValidatorNode(validatorData);

            Assert.AreEqual("name", node.Name);
            Assert.AreEqual(true, node.Negated);
            Assert.AreEqual(lowerBound, node.LowerBound);
            Assert.AreEqual(RangeBoundaryType.Exclusive, node.LowerBoundType);
            Assert.AreEqual(upperBound, node.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Ignore, node.UpperBoundType);
            Assert.AreEqual("message template", node.MessageTemplate);
        }

        [TestMethod]
        public void NodeCreatesValidatorDataWithValues()
        {
            DateTime lowerBound = new DateTime(2006, 1, 1);
            DateTime upperBound = new DateTime(2006, 1, 10);

            DateRangeValidatorNode node = new DateRangeValidatorNode();
            node.Name = "validator";
            node.Negated = true;
            node.LowerBound = lowerBound;
            node.LowerBoundType = RangeBoundaryType.Exclusive;
            node.UpperBound = upperBound;
            node.UpperBoundType = RangeBoundaryType.Ignore;
            node.MessageTemplate = "message template";

            DateTimeRangeValidatorData validatorData = node.CreateValidatorData() as DateTimeRangeValidatorData;

            Assert.IsNotNull(validatorData);
            Assert.AreEqual("validator", validatorData.Name);
            Assert.AreEqual(true, validatorData.Negated);
            Assert.AreEqual(lowerBound, validatorData.LowerBound);
            Assert.AreEqual(RangeBoundaryType.Exclusive, validatorData.LowerBoundType);
            Assert.AreEqual(upperBound, validatorData.UpperBound);
            Assert.AreEqual(RangeBoundaryType.Ignore, validatorData.UpperBoundType);
            Assert.AreEqual("message template", validatorData.MessageTemplate);
        }
    }
}