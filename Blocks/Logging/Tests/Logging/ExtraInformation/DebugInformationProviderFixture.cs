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

using System;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tests.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation.Tests
{
    [TestClass]
    public class DebugInformationProviderFixture
    {
        Dictionary<string, object> dictionary;
        DebugInformationProvider provider;

        [TestInitialize]
        public void SetUp()
        {
            dictionary = new Dictionary<string, object>();
        }

        [TestMethod]
        public void PopulateDictionaryFilledCorrectly()
        {
            provider = new DebugInformationProvider();
            provider.PopulateDictionary(dictionary);

            Assert.IsTrue(dictionary.Count > 0, "Dictionary contains no items");
            AssertUtilities.AssertStringDoesNotContain(dictionary[Resources.DebugInfo_StackTrace] as string, String.Format(Resources.ExtendedPropertyError, ""), "Stack trace");
        }

        [TestMethod]
        public void PopulateDictionaryFilledWithSecurityException()
        {
            provider = new DebugInformationProvider(new MockDebugUtilsThrowsSecurityException());
            provider.PopulateDictionary(dictionary);

            Assert.IsTrue(dictionary.Count > 0, "Dictionary contains no items");
            Assert.AreEqual(dictionary[Resources.DebugInfo_StackTrace], String.Format(Resources.ExtendedPropertyError, Resources.DebugInfo_StackTraceSecurityException));
        }

        [TestMethod]
        public void PopulateDictionaryFilledWithGenericException()
        {
            provider = new DebugInformationProvider(new MockDebugUtilsThrowsNonSecurityException());
            provider.PopulateDictionary(dictionary);

            Assert.IsTrue(dictionary.Count > 0, "Dictionary contains no items");
            Assert.AreEqual(dictionary[Resources.DebugInfo_StackTrace], String.Format(Resources.ExtendedPropertyError, Resources.DebugInfo_StackTraceException));
        }
    }
}
