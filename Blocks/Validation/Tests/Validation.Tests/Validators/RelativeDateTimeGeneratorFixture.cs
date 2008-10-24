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
    public class RelativeDateTimeGeneratorFixture
    {
        RelativeDateTimeGenerator generator;
        DateTime dateTime;

        [TestInitialize]
        public void SetUp()
        {
            generator = new RelativeDateTimeGenerator();
            dateTime = DateTime.Now;
        }

        [TestMethod]
        public void GenerateBoundDateTimeUsingNoneUnit()
        {
            DateTime newDateTime = generator.GenerateBoundDateTime(0, DateTimeUnit.None, dateTime);

            Assert.AreEqual(newDateTime, dateTime);
        }

        [TestMethod]
        public void GenerateBoundDateTimeUsingSecondUnit()
        {
            DateTime lowerDateTime = generator.GenerateBoundDateTime(-3, DateTimeUnit.Second, dateTime);
            DateTime upperDateTime = generator.GenerateBoundDateTime(5, DateTimeUnit.Second, dateTime);

            Assert.AreEqual(dateTime.AddSeconds(-3), lowerDateTime);
            Assert.AreEqual(dateTime.AddSeconds(5), upperDateTime);
            Assert.AreNotEqual(dateTime.AddSeconds(-2), lowerDateTime);
            Assert.AreNotEqual(dateTime.AddSeconds(4), upperDateTime);
        }

        [TestMethod]
        public void GenerateBoundDateTimeUsingMinuteUnit()
        {
            DateTime lowerDateTime = generator.GenerateBoundDateTime(-3, DateTimeUnit.Minute, dateTime);
            DateTime upperDateTime = generator.GenerateBoundDateTime(5, DateTimeUnit.Minute, dateTime);

            Assert.AreEqual(dateTime.AddMinutes(-3), lowerDateTime);
            Assert.AreEqual(dateTime.AddMinutes(5), upperDateTime);
            Assert.AreNotEqual(dateTime.AddMinutes(-2), lowerDateTime);
            Assert.AreNotEqual(dateTime.AddMinutes(4), upperDateTime);
        }

        [TestMethod]
        public void GenerateBoundDateTimeUsingHourUnit()
        {
            DateTime lowerDateTime = generator.GenerateBoundDateTime(-3, DateTimeUnit.Hour, dateTime);
            DateTime upperDateTime = generator.GenerateBoundDateTime(5, DateTimeUnit.Hour, dateTime);

            Assert.AreEqual(dateTime.AddHours(-3), lowerDateTime);
            Assert.AreEqual(dateTime.AddHours(5), upperDateTime);
            Assert.AreNotEqual(dateTime.AddHours(-2), lowerDateTime);
            Assert.AreNotEqual(dateTime.AddHours(4), upperDateTime);
        }

        [TestMethod]
        public void GenerateBoundDateTimeUsingDayUnit()
        {
            DateTime lowerDateTime = generator.GenerateBoundDateTime(-3, DateTimeUnit.Day, dateTime);
            DateTime upperDateTime = generator.GenerateBoundDateTime(5, DateTimeUnit.Day, dateTime);

            Assert.AreEqual(dateTime.AddDays(-3), lowerDateTime);
            Assert.AreEqual(dateTime.AddDays(5), upperDateTime);
            Assert.AreNotEqual(dateTime.AddDays(-2), lowerDateTime);
            Assert.AreNotEqual(dateTime.AddDays(4), upperDateTime);
        }

        [TestMethod]
        public void GenerateBoundDateTimeUsingMonthUnit()
        {
            DateTime lowerDateTime = generator.GenerateBoundDateTime(-3, DateTimeUnit.Month, dateTime);
            DateTime upperDateTime = generator.GenerateBoundDateTime(5, DateTimeUnit.Month, dateTime);

            Assert.AreEqual(dateTime.AddMonths(-3), lowerDateTime);
            Assert.AreEqual(dateTime.AddMonths(5), upperDateTime);
            Assert.AreNotEqual(dateTime.AddMonths(-2), lowerDateTime);
            Assert.AreNotEqual(dateTime.AddMonths(4), upperDateTime);
        }

        [TestMethod]
        public void GenerateBoundDateTimeUsingYearUnit()
        {
            DateTime lowerDateTime = generator.GenerateBoundDateTime(-3, DateTimeUnit.Year, dateTime);
            DateTime upperDateTime = generator.GenerateBoundDateTime(5, DateTimeUnit.Year, dateTime);

            Assert.AreEqual(dateTime.AddYears(-3), lowerDateTime);
            Assert.AreEqual(dateTime.AddYears(5), upperDateTime);
            Assert.AreNotEqual(dateTime.AddYears(-2), lowerDateTime);
            Assert.AreNotEqual(dateTime.AddYears(4), upperDateTime);
        }
    }
}
