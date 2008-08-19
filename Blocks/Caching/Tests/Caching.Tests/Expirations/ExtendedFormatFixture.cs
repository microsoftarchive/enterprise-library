//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Expirations.Tests
{
    [TestClass]
    public class ExtendedFormatFixture
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullFormatThrowsException()
        {
            ExtendedFormat format = new ExtendedFormat(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CreateBadFormTest()
        {
            ExtendedFormat format = new ExtendedFormat("5 * * *");
            Assert.IsNull(format);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void NegativeFormatTest()
        {
            ExtendedFormat format = new ExtendedFormat("* * -3 * *");
            Assert.IsNull(format);
        }

        [TestMethod]
        public void MinutesTest()
        {
            ExtendedFormat format = new ExtendedFormat("5 * * * *");
            int[] minutes = format.GetMinutes();
            Assert.AreEqual(1, minutes.Length);
            Assert.AreEqual(5, minutes[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void MinutesOutOfRangeTest()
        {
            ExtendedFormat format = new ExtendedFormat("61 * * * *");
            Assert.IsNull(format);
        }

        [TestMethod]
        public void MultiMinutesTest()
        {
            ExtendedFormat format = new ExtendedFormat("5,1 * * * *");
            int[] minutes = format.GetMinutes();
            Assert.AreEqual(2, minutes.Length);
            Assert.AreEqual(5, minutes[0]);
            Assert.AreEqual(1, minutes[1]);
        }

        [TestMethod]
        public void HoursTest()
        {
            ExtendedFormat format = new ExtendedFormat("* 5 * * *");
            int[] hours = format.GetHours();
            Assert.AreEqual(1, hours.Length);
            Assert.AreEqual(5, hours[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void HoursOutOfRangeTest()
        {
            ExtendedFormat format = new ExtendedFormat("* 25 * * *");
            Assert.IsNull(format);
        }

        [TestMethod]
        public void MultiHoursTest()
        {
            ExtendedFormat format = new ExtendedFormat("* 5,1 * * *");
            int[] hours = format.GetHours();
            Assert.AreEqual(2, hours.Length);
            Assert.AreEqual(5, hours[0]);
            Assert.AreEqual(1, hours[1]);
        }

        [TestMethod]
        public void DaysTest()
        {
            ExtendedFormat format = new ExtendedFormat("* * 5 * *");
            int[] days = format.GetDays();
            Assert.AreEqual(1, days.Length);
            Assert.AreEqual(5, days[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DaysOutOfRangeTest()
        {
            ExtendedFormat format = new ExtendedFormat("* * 32 * *");
            Assert.IsNull(format);
        }

        [TestMethod]
        public void MultiDaysTest()
        {
            ExtendedFormat format = new ExtendedFormat("* * 5,1 * *");
            int[] days = format.GetDays();
            Assert.AreEqual(2, days.Length);
            Assert.AreEqual(5, days[0]);
            Assert.AreEqual(1, days[1]);
        }

        [TestMethod]
        public void MonthsTest()
        {
            ExtendedFormat format = new ExtendedFormat("* * * 5 *");
            int[] months = format.GetMonths();
            Assert.AreEqual(1, months.Length);
            Assert.AreEqual(5, months[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void MonthsOutOfRangeTest()
        {
            ExtendedFormat format = new ExtendedFormat("* * * 13 *");
            Assert.IsNull(format);
        }

        [TestMethod]
        public void MultiMonthsTest()
        {
            ExtendedFormat format = new ExtendedFormat("* * * 5,1 *");
            int[] months = format.GetMonths();
            Assert.AreEqual(2, months.Length);
            Assert.AreEqual(5, months[0]);
            Assert.AreEqual(1, months[1]);
        }

        [TestMethod]
        public void DaysOfWeekTest()
        {
            ExtendedFormat format = new ExtendedFormat("* * * * 5");
            int[] daysOfWeek = format.GetDaysOfWeek();
            Assert.AreEqual(1, daysOfWeek.Length);
            Assert.AreEqual(5, daysOfWeek[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DaysOfWeekOutOfRangeTest()
        {
            ExtendedFormat format = new ExtendedFormat("* * * * 8");
            Assert.IsNull(format);
        }

        [TestMethod]
        public void MultiDaysOfWeekTest()
        {
            ExtendedFormat format = new ExtendedFormat("* * * * 5,1");
            int[] daysOfWeek = format.GetDaysOfWeek();
            Assert.AreEqual(2, daysOfWeek.Length);
            Assert.AreEqual(5, daysOfWeek[0]);
            Assert.AreEqual(1, daysOfWeek[1]);
        }

        [TestMethod]
        public void IsExpiredTest()
        {
            ExtendedFormat format = new ExtendedFormat("1 * * * *");
            bool expired = format.IsExpired(DateTime.Now.Subtract(new TimeSpan(1, 1, 2)), DateTime.Now);
            Assert.IsTrue(expired);
        }

        [TestMethod]
        public void TestForBug353()
        {
            ExtendedFormat format = new ExtendedFormat("* * 29 * *");
            bool expired = format.IsExpired(new DateTime(2003, 2, 10, 10, 10, 0), new DateTime(2003, 3, 10, 10, 10, 0));
            Assert.IsTrue(expired, "Should have expired at end of month, even though there is no Feb. 29th");
        }

        [TestMethod]
        public void TestForBug352()
        {
            ExtendedFormat format = new ExtendedFormat("0 0 1 4 *");
            bool expired = format.IsExpired(new DateTime(2001, 4, 01, 00, 05, 0), new DateTime(2002, 3, 31, 23, 59, 0));
            Assert.IsFalse(expired, "Has not hit midnight, April 1st of next year yet");
        }

        [TestMethod]
        public void ExpirationWithAllWildCardsWillExpireAfterOneMinute()
        {
            ExtendedFormat oneMinuteExpiration = new ExtendedFormat("* * * * *");
            DateTime baseTime = DateTime.Parse("5/1/2004 12:00:00");
            DateTime expirationTime = DateTime.Parse("5/1/2004 12:01:00");
            Assert.IsTrue(oneMinuteExpiration.IsExpired(baseTime, expirationTime));
        }

        [TestMethod]
        public void SecondsAreRoundedOutBeforePerformingPerMinuteExpirations()
        {
            ExtendedFormat oneMinuteExpiration = new ExtendedFormat("* * * * *");
            DateTime baseTime = DateTime.Parse("5/1/2004 12:00:59");
            DateTime expirationTime = DateTime.Parse("5/1/2004 12:01:01");
            Assert.IsTrue(oneMinuteExpiration.IsExpired(baseTime, expirationTime));
        }
    }
}