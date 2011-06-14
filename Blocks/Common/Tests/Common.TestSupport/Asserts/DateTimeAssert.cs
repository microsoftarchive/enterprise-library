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
