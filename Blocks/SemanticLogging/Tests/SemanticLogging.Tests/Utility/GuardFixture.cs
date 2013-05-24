#region license
// ==============================================================================
// Microsoft patterns & practices Enterprise Library
// Semantic Logging Application Block
// ==============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ==============================================================================
#endregion

using System;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Tests.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Tests.Utility
{
    [TestClass]
    public class GuardFixture
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ThrowOnInvalidDateTimeFormat()
        {
            Guard.ValidDateTimeFormat("0", "dtf");
        }

        [TestMethod]
        public void CheckForInvalidFileNameChars()
        {
            AssertEx.Throws<ArgumentException>(() => Guard.ValidateTimestampPattern("MM/dd/yyyy", "timestampPattern"));
            AssertEx.Throws<ArgumentException>(() => Guard.ValidateTimestampPattern("MM:dd:yyyy", "timestampPattern"));
        }
    }
}
