//===============================================================================
// Microsoft patterns & practices Enterprise Library
// SQL Configuration Source QuickStart
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Storage;
using Microsoft.Practices.EnterpriseLibrary.SqlConfigurationSource.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.SqlConfigurationSource.Tests
{
    [TestClass]
    public class ConfigurationSourceFactoryFixture
    {
        const string localSection = "dummy.local";

        static void ClearConfig()
        {
            string connectionString = @"server=(local)\SQLExpress;database=Northwind;Integrated Security=true";
            //clear the configuration sections from the database
            using (SqlConnection myConnection = new SqlConnection(connectionString))
            {
                // Create Instance of Connection and Command Object
                SqlCommand myCommand = new SqlCommand("DELETE FROM Configuration_Parameter", myConnection);
                myCommand.CommandType = CommandType.Text;

                // Execute the command
                myConnection.Open();
                myCommand.ExecuteNonQuery();
            }
        }

        static SqlConfigurationParameter CreateParameter()
        {
            string connectString = @"server=(local)\SQLExpress;database=Northwind;Integrated Security=true";
            string getStoredProc = @"EntLib_GetConfig";
            string setStoredProc = @"EntLib_SetConfig";
            string refreshStoredProc = @"UpdateSectionDate";
            string removeStoredProc = @"EntLib_RemoveSection";

            SqlConfigurationParameter parameter = new SqlConfigurationParameter(connectString, getStoredProc, setStoredProc, refreshStoredProc, removeStoredProc);
            return parameter;
        }

        /// <summary>
        /// 
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            ClearConfig();
            ConfigurationChangeWatcher.ResetDefaultPollDelay();
        }

        [TestMethod]
        [Ignore] //change notification seems to be broken...
        public void GetsNotificationWhenUpdatingAndRemovingSections()
        {
            ConfigurationChangeWatcher.SetDefaultPollDelayInMilliseconds(50);
            IConfigurationSource source = ConfigurationSourceFactory.Create("sqlSource");
            Assert.AreEqual(typeof(SqlConfigurationSource), source.GetType());

            DummySection dummySection1 = new DummySection();
            dummySection1.Value = 10;

            source.Add(CreateParameter(), localSection, dummySection1);
            bool sourceChanged = false;
            SqlConfigurationSource sqlSource = source as SqlConfigurationSource;

            sqlSource.AddSectionChangeHandler(localSection, delegate(object o,
                                                                     ConfigurationChangedEventArgs args)
                                                            {
                                                                sourceChanged = true;
                                                            });

            ConfigurationSection newSection = source.GetSection(localSection);
            Assert.AreEqual(typeof(DummySection), newSection.GetType());

            DummySection dummySection2 = newSection as DummySection;
            Assert.AreEqual(dummySection1, dummySection2);

            //update the section
            dummySection2.Value = 15;
            sqlSource.Add(CreateParameter(), localSection, dummySection2);

            Thread.Sleep(1500);

            Assert.IsTrue(sourceChanged);
            sourceChanged = false;

            //remove the section
            sqlSource.Remove(CreateParameter(), localSection);

            Thread.Sleep(1500);

            Assert.IsTrue(sourceChanged);
            sourceChanged = false;

            newSection = sqlSource.GetSection(localSection);
            Assert.AreEqual(null, newSection);
        }

        [TestMethod]
        public void CanCreateAConfigurationSourceThatExistsInConfig()
        {
            IConfigurationSource source = ConfigurationSourceFactory.Create("sqlSource");

            Assert.AreEqual(typeof(SqlConfigurationSource), source.GetType());
        }

        [TestMethod]
        public void AddAndGetASection()
        {
            IConfigurationSource source = ConfigurationSourceFactory.Create("sqlSource");
            Assert.AreEqual(typeof(SqlConfigurationSource), source.GetType());

            DummySection dummySection1 = new DummySection();
            dummySection1.Value = 10;

            source.Add(CreateParameter(), localSection, dummySection1);

            ConfigurationSection newSection = source.GetSection(localSection);
            Assert.AreEqual(typeof(DummySection), newSection.GetType());

            DummySection dummySection2 = newSection as DummySection;

            Assert.AreEqual(dummySection1, dummySection2);
        }

        [TestMethod]
        public void AddGetAndRemoveASection()
        {
            IConfigurationSource source = ConfigurationSourceFactory.Create("sqlSource");
            Assert.AreEqual(typeof(SqlConfigurationSource), source.GetType());

            DummySection dummySection1 = new DummySection();
            dummySection1.Value = 10;

            source.Add(CreateParameter(), localSection, dummySection1);

            ConfigurationSection newSection = source.GetSection(localSection);
            Assert.AreEqual(typeof(DummySection), newSection.GetType());

            DummySection dummySection2 = newSection as DummySection;
            Assert.AreEqual(dummySection1, dummySection2);

            source.Remove(CreateParameter(), localSection);

            newSection = source.GetSection(localSection);
            Assert.AreEqual(null, newSection);
        }
    }
}
