//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Tests
{
    /// <summary>
    /// Summary description for OracleDatabaseDataExtraInformationFixture
    /// </summary>
    [TestClass]
    public class OracleDatabaseDataExtraInformationFixture
    {
        [TestInitialize]
        public void SetUp()
        {
            AppDomain.CurrentDomain.SetData("APPBASE", Environment.CurrentDirectory);
        }

        [TestMethod]
        public void CanDeserializeSerializedConfiguration()
        {
            OracleConnectionSettings rwSettings = new OracleConnectionSettings();

            OracleConnectionData rwOracleConnectionData = new OracleConnectionData();
            rwOracleConnectionData.Name = "name0";
            rwOracleConnectionData.Packages.Add(new OraclePackageData("package1", "pref1"));
            rwOracleConnectionData.Packages.Add(new OraclePackageData("package2", "pref2"));
            rwSettings.OracleConnectionsData.Add(rwOracleConnectionData);
            rwOracleConnectionData = new OracleConnectionData();
            rwOracleConnectionData.Name = "name1";
            rwOracleConnectionData.Packages.Add(new OraclePackageData("package3", "pref3"));
            rwOracleConnectionData.Packages.Add(new OraclePackageData("package4", "pref4"));
            rwSettings.OracleConnectionsData.Add(rwOracleConnectionData);

            IDictionary<string, ConfigurationSection> sections = new Dictionary<string, ConfigurationSection>();
            sections[OracleConnectionSettings.SectionName] = rwSettings;
            IConfigurationSource configurationSource
                = ConfigurationTestHelper.SaveSectionsInFileAndReturnConfigurationSource(sections);

            OracleConnectionSettings roSettings = (OracleConnectionSettings)configurationSource.GetSection(OracleConnectionSettings.SectionName);
            Assert.AreEqual(2, roSettings.OracleConnectionsData.Count);
            Assert.AreEqual("name0", roSettings.OracleConnectionsData.Get(0).Name);
            Assert.AreEqual(2, roSettings.OracleConnectionsData.Get(0).Packages.Count);
        }
    }
}
