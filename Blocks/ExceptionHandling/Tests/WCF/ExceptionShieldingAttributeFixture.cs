//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Tests
{
    [TestClass]
    public class ExceptionShieldingAttributeFixture
    {
        [TestMethod]
        public void CanCreateInstance()
        {
            ExceptionShieldingAttribute instance = new ExceptionShieldingAttribute();
            Assert.IsNotNull(instance);
            Assert.AreEqual(ExceptionShielding.DefaultExceptionPolicy, instance.ExceptionPolicyName);
        }

        [TestMethod]
        public void CanCreateInstanceWithPolicyName()
        {
            ExceptionShieldingAttribute instance = new ExceptionShieldingAttribute("Policy");
            Assert.AreEqual("Policy", instance.ExceptionPolicyName);
        }

        [TestMethod]
        public void CanAssignExceptionPolicyName()
        {
            ExceptionShieldingAttribute shielding = new ExceptionShieldingAttribute();
            shielding.ExceptionPolicyName = "MyPolicy";
            Assert.AreEqual("MyPolicy", shielding.ExceptionPolicyName);
        }

        [TestMethod]
        public void ShouldGetDefaultValueOnNullExceptionPolicyName()
        {
            ExceptionShieldingAttribute shielding = new ExceptionShieldingAttribute();
            shielding.ExceptionPolicyName = null;
            Assert.AreEqual(ExceptionShielding.DefaultExceptionPolicy, shielding.ExceptionPolicyName);
        }

        [TestMethod]
        public void ShouldGetDefaultValueOnEmptyShieldingAttribute()
        {
            ExceptionShieldingAttribute shielding = new ExceptionShieldingAttribute();
            shielding.ExceptionPolicyName = "";
            Assert.AreEqual(ExceptionShielding.DefaultExceptionPolicy, shielding.ExceptionPolicyName);
        }
    }
}
