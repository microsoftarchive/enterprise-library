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
    public class SqlConfigurationManagerFixture
    {
        SqlConfigurationData data = null;
        SqlConfigurationSystem configSystem;

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

        void UpdateSection(string sectionName,
                           string sectionType,
                           object sectionValue)
        {
            using (SqlConnection myConnection = new SqlConnection(data.ConnectionString))
            {
                // Create Instance of Connection and Command Object
                SqlCommand myCommand = new SqlCommand("EntLib_SetConfig", myConnection);
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.Parameters.AddWithValue("@section_name", sectionName);
                myCommand.Parameters.AddWithValue("@section_type", sectionType);
                if (sectionValue == null)
                    sectionValue = String.Empty;
                myCommand.Parameters.AddWithValue("@section_value", sectionValue);

                // Execute the command
                myConnection.Open();
                myCommand.ExecuteNonQuery();
            }
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
            string removeStoredProc = @"EntLib_RemoveSection";
            string refreshStoredProc = @"UpdateSectionDate";

            data = new SqlConfigurationData(connectString, getStoredProc, setStoredProc, refreshStoredProc, removeStoredProc);

            configSystem = new SqlConfigurationSystem(connectString, getStoredProc, setStoredProc, refreshStoredProc, removeStoredProc);
            if (SqlConfigurationManager.InitializationState == SqlConfigurationManager.InitState.NotStarted)
                SqlConfigurationManager.SetConfigurationSystem(configSystem, true);

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
            SqlConfigurationManager.SaveSection("TestSection", configSection, data);

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
            SqlConfigurationManager.SaveSection("TestSection", configSection, data);

            DummySection configSection2 = SqlConfigurationManager.GetSection("TestSection", data) as DummySection;

            Assert.AreEqual(configSection2.Value, configSection.Value);
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void InvalidSectionNameReturnsNull()
        {
            DummySection configSection2 = SqlConfigurationManager.GetSection("BadSection", data) as DummySection;

            Assert.IsNull(configSection2);
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void EmptySectionValueFieldReturnsNull()
        {
            UpdateSection("TestSection", typeof(DummySection).FullName, null);
            object configSection = SqlConfigurationManager.GetSection("TestSection", data);

            Assert.IsNull(configSection);
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
            SqlConfigurationManager.SaveSection("TestSection", configSection, data);

            DateTime lastModDate1 = GetLastModDate(sectionName);
            Thread.Sleep(1000);
            SqlConfigurationManager.RefreshSection(sectionName, data);
            DateTime lastModDate2 = GetLastModDate(sectionName);

            Assert.IsFalse(lastModDate2.Equals(lastModDate1));
        }
    }
}