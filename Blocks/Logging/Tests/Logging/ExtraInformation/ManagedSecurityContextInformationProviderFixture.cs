//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tests.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation.Tests
{
    [TestClass]
    public class ManagedSecurityContextInformationProviderFixture
    {
        Dictionary<string, object> dictionary;
        ManagedSecurityContextInformationProvider provider;

        [TestInitialize]
        public void SetUp()
        {
            dictionary = new Dictionary<string, object>();
        }

        [TestMethod]
        public void PopulateDictionaryFilledCorrectly()
        {
            provider = new ManagedSecurityContextInformationProvider();
            provider.PopulateDictionary(dictionary);

            Assert.IsTrue(dictionary.Count > 0, "Dictionary contains no items");
            AssertUtilities.AssertStringDoesNotContain(dictionary[Resources.ManagedSecurity_AuthenticationType] as string, string.Format(Resources.ExtendedPropertyError, ""), "Authentication Type");
            AssertUtilities.AssertStringDoesNotContain(dictionary[Resources.ManagedSecurity_IdentityName] as string, string.Format(Resources.ExtendedPropertyError, ""), "Identity Name");
            AssertUtilities.AssertStringDoesNotContain(dictionary[Resources.ManagedSecurity_IsAuthenticated] as string, string.Format(Resources.ExtendedPropertyError, ""), "Is Authenticated");
        }
    }
}
