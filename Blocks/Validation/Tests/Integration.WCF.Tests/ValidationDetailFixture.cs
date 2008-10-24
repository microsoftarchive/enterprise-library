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

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF.Tests.VSTS
{
    /// <summary>
    /// Summary description for ValidationDetailFixture
    /// </summary>
    [TestClass]
    public class ValidationDetailFixture
    {
        [TestMethod]
        public void ShouldDefaultConstructToValid()
        {
            ValidationFault fault = new ValidationFault();
            Assert.IsTrue(fault.IsValid);
            Assert.AreEqual(0, fault.Details.Count);
        }

        [TestMethod]
        public void ShouldBeInvalidAfterAddingDetail()
        {
            ValidationFault fault = new ValidationFault();
            fault.Add(new ValidationDetail("message", "key", "tag"));
            Assert.IsFalse(fault.IsValid);
            Assert.AreEqual(1, fault.Details.Count);
        }

        [TestMethod]
        public void ShouldBeInvalidWhenConstructedWithDetails()
        {
            ValidationDetail[] details = {
                                             new ValidationDetail("m1", "k1", "t1"),
                                             new ValidationDetail("m2", "k2", "t2")
                                         };

            ValidationFault fault = new ValidationFault(details);
            Assert.IsFalse(fault.IsValid);
            Assert.AreEqual(2, fault.Details.Count);
        }
    }
}
