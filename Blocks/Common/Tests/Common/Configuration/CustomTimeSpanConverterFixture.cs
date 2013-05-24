//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Globalization;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration
{
    [TestClass]
    public class CustomTimeSpanConverterFixture
    {
        [TestMethod]
        public void ValidatorAttributeReturnsExpectedValidatorInstance()
        {
            Assert.IsInstanceOfType(new NonNegativeOrInfiniteTimeSpanValidatorAttribute().ValidatorInstance, typeof(NonNegativeOrInfiniteTimeSpanValidator));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ValidatingNegativeTimeSpanThrows()
        {
            var validator = new NonNegativeOrInfiniteTimeSpanValidator();
            validator.Validate(TimeSpan.FromSeconds(-1));
        }

        [TestMethod]
        public void ValidatingZeroTimeSpanSucceeds()
        {
            var validator = new NonNegativeOrInfiniteTimeSpanValidator();
            validator.Validate(TimeSpan.Zero);
        }

        [TestMethod]
        public void ValidatingPositiveTimeSpanSucceeds()
        {
            var validator = new NonNegativeOrInfiniteTimeSpanValidator();
            validator.Validate(TimeSpan.FromSeconds(1));
        }

        [TestMethod]
        public void ValidatingInfiniteTimeoutSucceeds()
        {
            var validator = new NonNegativeOrInfiniteTimeSpanValidator();
            validator.Validate(Timeout.InfiniteTimeSpan);
        }

        [TestMethod]
        public void ConvertingFromFormattedTimeSpanSucceeds()
        {
            var converter = new TimeSpanOrInfiniteConverter();

            var result = converter.ConvertFrom(null, CultureInfo.InvariantCulture, "1.23:45:56.789");

            Assert.AreEqual(new TimeSpan(1, 23, 45, 56, 789), result);
        }

        [TestMethod]
        public void ConvertingFromInfiniteTimeSpanSucceeds()
        {
            var converter = new TimeSpanOrInfiniteConverter();

            var result = converter.ConvertFrom(null, CultureInfo.InvariantCulture, "infinite");

            Assert.AreEqual(Timeout.InfiniteTimeSpan, result);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void ConvertingFromStringFails()
        {
            var converter = new TimeSpanOrInfiniteConverter();

            var result = converter.ConvertFrom(null, CultureInfo.InvariantCulture, "invalid");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ConvertingFromNullStringFails()
        {
            var converter = new TimeSpanOrInfiniteConverter();

            var result = converter.ConvertFrom(null, CultureInfo.InvariantCulture, 4);
        }

        [TestMethod]
        public void ConvertingInfiniteTimeoutToStringReturnsCustomString()
        {
            var converter = new TimeSpanOrInfiniteConverter();

            var result = converter.ConvertTo(null, CultureInfo.InvariantCulture, Timeout.InfiniteTimeSpan, typeof(string));

            Assert.AreEqual("infinite", result);
        }

        [TestMethod]
        public void ConvertingNonInfiniteTimeoutToStringReturnsFormattedString()
        {
            var converter = new TimeSpanOrInfiniteConverter();

            var result = converter.ConvertTo(null, CultureInfo.InvariantCulture, new TimeSpan(1, 23, 45, 56, 789), typeof(string));

            Assert.AreEqual("1.23:45:56.7890000", result);
        }
    }
}
