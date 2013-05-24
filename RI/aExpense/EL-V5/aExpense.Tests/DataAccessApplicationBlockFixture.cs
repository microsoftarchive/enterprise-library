#region license
// ==============================================================================
// Microsoft patterns & practices Enterprise Library
// aExpense Reference Implementation
// ==============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ==============================================================================
#endregion

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AExpense.DataAccessLayer;
using AExpense.Tests.Util;
using AExpense.FunctionalTests.Properties;

namespace AExpense.Tests.Functional
{
    [TestClass]
    public class DataAccessApplicationBlockFixture
    {
        private static readonly SimulatedLdapProfileStore LDAPStore = ProfileStoreHelper.GetProfileStore("aExpense");

        [TestMethod]
        public void CanGetAttributesForUser()
        {
            string username = "ADATUM\\johndoe";


            var attributes = LDAPStore.GetAttributesFor(username, new[] { "costCenter", "manager", "displayName" });

            Assert.IsNotNull(attributes);
            Assert.AreEqual(3, attributes.Keys.Count);
            Assert.AreEqual("31023", attributes["costCenter"]);
            Assert.AreEqual("ADATUM\\mary", attributes["manager"]);
            Assert.AreEqual("John Doe", attributes["displayName"]);


        }

        [TestMethod]
        public void WrongUserThrows()
        {
            string username = "ADATUM\\wrongUser";

            var ex = ExceptionAssertHelper.Throws<ArgumentException>(
                () => LDAPStore.GetAttributesFor(username, new[] { "costCenter", "manager", "displayName" }));

            Assert.AreEqual(Resources.UserDoesNotExistInLDAPMessage, ex.Message);


        }
    }
}
