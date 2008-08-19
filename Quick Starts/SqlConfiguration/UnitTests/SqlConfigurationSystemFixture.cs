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

using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.SqlConfigurationSource.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.SqlConfigurationSource.Tests
{
    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class SqlConfigurationSystemSectionFixture
    {
        SqlConfigurationData data = null;

        void ClearConfigs()
        {
            //clear the configuration sections from the database
            using (SqlConnection myConnection = new SqlConnection(data.ConnectionString))
            {
                // Create Instance of Connection and Command Object
                SqlCommand myCommand = new SqlCommand("DELETE FROM Configuration_Parameter", myConnection);
                myCommand.CommandType = CommandType.Text;

                // Execute the command
                myConnection.Open();
                myCommand.ExecuteNonQuery();
            }
        }

        int GetNumberofConfigSections()
        {
            int numConfigSections = 0;
            using (SqlConnection myConnection = new SqlConnection(data.ConnectionString))
            {
                // Create Instance of Connection and Command Object
                SqlCommand myCommand = new SqlCommand("SELECT COUNT(*) FROM Configuration_Parameter", myConnection);
                myCommand.CommandType = CommandType.Text;

                // Execute the command
                myConnection.Open();
                numConfigSections = (int)myCommand.ExecuteScalar();
            }
            return numConfigSections;
        }

        DateTime GetLastModDate(string sectionName)
        {
            DateTime lastmoddate = DateTime.MinValue;
            using (SqlConnection myConnection = new SqlConnection(data.ConnectionString))
            {
                // Create Instance of Connection and Command Object
                SqlCommand myCommand = new SqlCommand("SELECT lastmoddate FROM Configuration_Parameter", myConnection);
                myCommand.CommandType = CommandType.Text;

                // Execute the command
                myConnection.Open();
                lastmoddate = (DateTime)myCommand.ExecuteScalar();
            }
            return lastmoddate;
        }

        /// <summary>
        /// 
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            string connectString = @"server=(local)\SQLExpress;database=Northwind;Integrated Security=true";
            string getStoredProc = @"EntLib_GetConfig";
            string setStoredProc = @"EntLib_SetConfig";
            string refreshStoredProc = @"UpdateSectionDate";
            string removeStoredProc = @"EntLib_RemoveSection";

            data = new SqlConfigurationData(connectString, getStoredProc, setStoredProc, refreshStoredProc, removeStoredProc);

            ClearConfigs();
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void SaveTestConfiguration()
        {
            DummySection configSection = new DummySection();
            configSection.Value = 20;
            SqlConfigurationSystem configSystem = new SqlConfigurationSystem(data);
            configSystem.SaveSection("TestSection", configSection);

            //Assert that the database now has one record
            Assert.IsTrue(GetNumberofConfigSections() == 1);
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void SaveAndGetTestConfiguration()
        {
            DummySection configSection = new DummySection();
            configSection.Value = 20;
            SqlConfigurationSystem configSystem = new SqlConfigurationSystem(data);
            configSystem.SaveSection("TestSection", configSection);

            DummySection configSection2 = configSystem.GetSection("TestSection") as DummySection;

            Assert.AreEqual(configSection2.Value, configSection.Value);
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void RefreshConfiguration()
        {
            string sectionName = "TestSection";

            DummySection configSection = new DummySection();
            configSection.Value = 20;
            SqlConfigurationSystem configSystem = new SqlConfigurationSystem(data);
            configSystem.SaveSection(sectionName, configSection);

            DateTime lastModDate1 = GetLastModDate(sectionName);
            Thread.Sleep(1000);
            configSystem.RefreshConfig(sectionName);
            DateTime lastModDate2 = GetLastModDate(sectionName);

            Assert.IsFalse(lastModDate2.Equals(lastModDate1));
        }
    }
}