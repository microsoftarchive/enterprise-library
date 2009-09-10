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

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Tests
{
    [TestClass]
    public class DictionarySourceFixture
    {
        [TestMethod]
        public void CanRetrieveSectionFromSource()
        {
            DictionaryConfigurationSource source = LocalConfigurationSource.Create();

            Assert.IsTrue(source.Contains("test"));
            Assert.AreEqual(source.GetSection("test").GetType(), typeof(LocalConfigurationSection));
            source.Remove("test");
            Assert.IsNull(source.GetSection("random"));
        }

        class LocalConfigurationSection : SerializableConfigurationSection {}

        static class LocalConfigurationSource
        {
            public static DictionaryConfigurationSource Create()
            {
                DictionaryConfigurationSource source = new DictionaryConfigurationSource();
                source.Add("test", new LocalConfigurationSection());
                return source;
            }
        }
    }
}
