using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Asserts
{
    public static class DateTimeAssert
    {
        public static void AreEqual(DateTime expected, DateTime actual, int precision)
        {
            var expectedOffset = new DateTimeOffset(expected);
            var actualOffset = new DateTimeOffset(actual);

            Assert.AreEqual(expectedOffset.Offset, actualOffset.Offset);
            Assert.AreEqual(
                expectedOffset.UtcTicks / precision,
                actualOffset.UtcTicks / precision,
                string.Format("Dates do not match: '{0}' / '{1}'", expected, actual));
        }
    }
}
