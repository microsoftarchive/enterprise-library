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
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests.Validators
{
    [TestClass]
    public class RangeCheckerFixture
    {
        [TestMethod]
        public void ReturnsSuccessForValueShorterThanLowerBoundIfLowerBoundIsIgnored()
        {
            RangeChecker<int> rangeChecker = new RangeChecker<int>(5, RangeBoundaryType.Ignore, 10, RangeBoundaryType.Inclusive);

            Assert.IsTrue(rangeChecker.IsInRange(2));
        }

        [TestMethod]
        public void ReturnsSuccessForValueLongerThanLowerBound()
        {
            RangeChecker<int> rangeChecker = new RangeChecker<int>(5, RangeBoundaryType.Inclusive, 10, RangeBoundaryType.Inclusive);

            Assert.IsTrue(rangeChecker.IsInRange(7));
        }

        [TestMethod]
        public void ReturnsFailureForValueShorterThanLowerBound()
        {
            RangeChecker<int> rangeChecker = new RangeChecker<int>(5, RangeBoundaryType.Inclusive, 10, RangeBoundaryType.Inclusive);

            Assert.IsFalse(rangeChecker.IsInRange(2));
        }

        [TestMethod]
        public void ReturnsSuccessForValueWithLengthEqualToLowerBoundIfLowerBoundIsInclusive()
        {
            RangeChecker<int> rangeChecker = new RangeChecker<int>(5, RangeBoundaryType.Inclusive, 10, RangeBoundaryType.Inclusive);

            Assert.IsTrue(rangeChecker.IsInRange(5));
        }

        [TestMethod]
        public void ReturnsFailureForValueWithLengthEqualToLowerBoundIfLowerBoundIsExclusive()
        {
            RangeChecker<int> rangeChecker = new RangeChecker<int>(5, RangeBoundaryType.Exclusive, 10, RangeBoundaryType.Inclusive);

            Assert.IsFalse(rangeChecker.IsInRange(5));
        }

        [TestMethod]
        public void ReturnsFailureForValueWithLengthOneShoterThanLowerBoundIfLowerBoundIsInclusive()
        {
            RangeChecker<int> rangeChecker = new RangeChecker<int>(5, RangeBoundaryType.Inclusive, 10, RangeBoundaryType.Inclusive);

            Assert.IsFalse(rangeChecker.IsInRange(4));
        }

        [TestMethod]
        public void ReturnsFailureForValueWithLengthOneShorterThanLowerBoundIfLowerBoundIsExclusive()
        {
            RangeChecker<int> rangeChecker = new RangeChecker<int>(5, RangeBoundaryType.Exclusive, 10, RangeBoundaryType.Inclusive);

            Assert.IsFalse(rangeChecker.IsInRange(4));
        }

        [TestMethod]
        public void ReturnsSuccessForValueWithLengthOneLongerThanLowerBoundIfLowerBoundIsInclusive()
        {
            RangeChecker<int> rangeChecker = new RangeChecker<int>(5, RangeBoundaryType.Inclusive, 10, RangeBoundaryType.Inclusive);

            Assert.IsTrue(rangeChecker.IsInRange(6));
        }

        [TestMethod]
        public void ReturnsSuccessForValueWithLengthOneLongerThanLowerBoundIfLowerBoundIsExclusive()
        {
            RangeChecker<int> rangeChecker = new RangeChecker<int>(5, RangeBoundaryType.Exclusive, 10, RangeBoundaryType.Inclusive);

            Assert.IsTrue(rangeChecker.IsInRange(6));
        }

        [TestMethod]
        public void ReturnsSuccessForValueLongerThanUpperBoundIfUpperBoundIsIgnored()
        {
            RangeChecker<int> rangeChecker = new RangeChecker<int>(5, RangeBoundaryType.Inclusive, 10, RangeBoundaryType.Ignore);

            Assert.IsTrue(rangeChecker.IsInRange(12));
        }

        [TestMethod]
        public void ReturnsSuccessForValueShorterThanUpperBound()
        {
            RangeChecker<int> rangeChecker = new RangeChecker<int>(5, RangeBoundaryType.Inclusive, 10, RangeBoundaryType.Inclusive);

            Assert.IsTrue(rangeChecker.IsInRange(7));
        }

        [TestMethod]
        public void ReturnsFailureForValueLongerThanUpperBound()
        {
            RangeChecker<int> rangeChecker = new RangeChecker<int>(5, RangeBoundaryType.Inclusive, 10, RangeBoundaryType.Inclusive);

            Assert.IsFalse(rangeChecker.IsInRange(12));
        }

        [TestMethod]
        public void ReturnsSuccessForValueWithLengthEqualToUpperBoundIfUpperBoundIsInclusive()
        {
            RangeChecker<int> rangeChecker = new RangeChecker<int>(5, RangeBoundaryType.Inclusive, 10, RangeBoundaryType.Inclusive);

            Assert.IsTrue(rangeChecker.IsInRange(10));
        }

        [TestMethod]
        public void ReturnsFailureForValueWithLengthEqualToUpperBoundIfUpperBoundIsExclusive()
        {
            RangeChecker<int> rangeChecker = new RangeChecker<int>(5, RangeBoundaryType.Inclusive, 10, RangeBoundaryType.Exclusive);

            Assert.IsFalse(rangeChecker.IsInRange(10));
        }

        [TestMethod]
        public void ReturnsSuccessForValueWithLengthOneShoterThanLowerBoundIfLUpperBoundIsInclusive()
        {
            RangeChecker<int> rangeChecker = new RangeChecker<int>(5, RangeBoundaryType.Inclusive, 10, RangeBoundaryType.Inclusive);

            Assert.IsTrue(rangeChecker.IsInRange(9));
        }

        [TestMethod]
        public void ReturnsSuccessForValueWithLengthOneShorterThanUpperBoundIfUpperBoundIsExclusive()
        {
            RangeChecker<int> rangeChecker = new RangeChecker<int>(5, RangeBoundaryType.Inclusive, 10, RangeBoundaryType.Exclusive);

            Assert.IsTrue(rangeChecker.IsInRange(9));
        }

        [TestMethod]
        public void ReturnsFailureForValueWithLengthOneLongerThanUpperBoundIfUpperBoundIsInclusive()
        {
            RangeChecker<int> rangeChecker = new RangeChecker<int>(5, RangeBoundaryType.Inclusive, 10, RangeBoundaryType.Inclusive);

            Assert.IsFalse(rangeChecker.IsInRange(11));
        }

        [TestMethod]
        public void ReturnsFailureForValueWithLengthOneLongerThanUpperBoundIfUpperBoundIsExclusive()
        {
            RangeChecker<int> rangeChecker = new RangeChecker<int>(5, RangeBoundaryType.Inclusive, 10, RangeBoundaryType.Exclusive);

            Assert.IsFalse(rangeChecker.IsInRange(11));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreationWithUpperBoundIsLowerThanLowerBoundAndNeitherBoundIsIgnoredThrows()
        {
            new RangeChecker<int>(15, RangeBoundaryType.Inclusive, 10, RangeBoundaryType.Exclusive);
        }

        [TestMethod]
        public void CreationWithUpperBoundIsLowerThanLowerBoundDoesNotThrowIfAnyBoundIsIgnored()
        {
            new RangeChecker<int>(15, RangeBoundaryType.Ignore, 10, RangeBoundaryType.Exclusive);
            new RangeChecker<int>(15, RangeBoundaryType.Exclusive, 10, RangeBoundaryType.Ignore);
            new RangeChecker<int>(15, RangeBoundaryType.Ignore, 10, RangeBoundaryType.Ignore);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreationWithNullUpperBoundForInclusiveUpperBoundThrows()
        {
            new RangeChecker<string>("", RangeBoundaryType.Inclusive, null, RangeBoundaryType.Inclusive);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreationWithNullUpperBoundForExclusiveUpperBoundThrows()
        {
            new RangeChecker<string>("", RangeBoundaryType.Inclusive, null, RangeBoundaryType.Exclusive);
        }

        [TestMethod]
        public void CreationWithNullUpperBoundForIgnoreUpperBoundDoesNotThrowAndPerformsAppropriateCheck()
        {
            RangeChecker<string> rangeChecker = new RangeChecker<string>("aaaa", RangeBoundaryType.Inclusive, null, RangeBoundaryType.Ignore);

            Assert.IsTrue(rangeChecker.IsInRange("bbbbbb"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreationWithNullLowerBoundForInclusiveLowerBoundThrows()
        {
            new RangeChecker<string>(null, RangeBoundaryType.Inclusive, "zzzz", RangeBoundaryType.Inclusive);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreationWithNullLowerBoundForExclusiveLowerBoundThrows()
        {
            new RangeChecker<string>(null, RangeBoundaryType.Exclusive, "zzzz", RangeBoundaryType.Inclusive);
        }

        [TestMethod]
        public void CreationWithNullLowerBoundForIgnoreLowerBoundDoesNotThrowAndPerformsAppropriateCheck()
        {
            RangeChecker<string> rangeChecker = new RangeChecker<string>(null, RangeBoundaryType.Ignore, "zzzz", RangeBoundaryType.Inclusive);

            Assert.IsTrue(rangeChecker.IsInRange("bbbbbb"));
        }
    }
}
