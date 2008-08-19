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

using System.Configuration;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SysConfig=System.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Tests
{
    [TestClass]
    public class DatabaseFactoryOldFixture
    {
        [TestMethod]
        public void CanCreateDefaultDatabase()
        {
            DatabaseProviderFactory factory = new DatabaseProviderFactory(TestConfigurationSource.CreateConfigurationSource());
            Database db = factory.CreateDefault();
            Assert.IsNotNull(db);
        }

        [TestMethod]
        public void CanGetDatabaseByName()
        {
            DatabaseProviderFactory factory = new DatabaseProviderFactory(TestConfigurationSource.CreateConfigurationSource());
            Database db = factory.Create("NewDatabase");
            Assert.IsNotNull(db);
        }

        [TestMethod]
        public void CallingTwiceReturnsDifferenceDatabaseInstances()
        {
            DatabaseProviderFactory factory = new DatabaseProviderFactory(TestConfigurationSource.CreateConfigurationSource());
            Database firstDb = factory.Create("NewDatabase");
            Database secondDb = factory.Create("NewDatabase");

            Assert.IsFalse(firstDb == secondDb);
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void ExceptionThrownWhenAskingForDatabaseWithUnknownName()
        {
            Database db = DatabaseFactory.CreateDatabase("ThisIsAnUnknownKey");
            Assert.IsNotNull(db);
        }

        //[TestMethod]
        //public void WmiEventFiredWhenAskingForDatabaseWithUnknownName()
        //{
        //    using (WmiEventWatcher eventListener = new WmiEventWatcher(1))
        //    {
        //        try
        //        {
        //            Database db = DatabaseFactory.CreateDatabase("ThisIsAnUnknownKey");
        //        }
        //        catch (ConfigurationErrorsException)
        //        {
        //            eventListener.WaitForEvents();
        //            Assert.AreEqual(1, eventListener.EventsReceived.Count);
        //            Assert.AreEqual("DataConfigurationFailureEvent", eventListener.EventsReceived[0].ClassPath.ClassName);
        //            Assert.AreEqual("ThisIsAnUnknownKey", eventListener.EventsReceived[0].GetPropertyValue("InstanceName"));
        //            string exceptionMessage = (string)eventListener.EventsReceived[0].GetPropertyValue("ExceptionMessage");

        //            Assert.IsFalse(-1 == exceptionMessage.IndexOf("ThisIsAnUnknownKey"));

        //            return;
        //        }

        //        Assert.Fail("ConfigurationErrorsException expected");
        //    }
        //}

        [TestMethod]
        public void CreatingDatabaseWithUnknownInstanceNameWritesToEventLog()
        {
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ThisIsAnUnknownKey");
            }
            catch (ConfigurationErrorsException)
            {
                using (EventLog applicationLog = new EventLog("Application"))
                {
                    EventLogEntry lastEntry = applicationLog.Entries[applicationLog.Entries.Count - 1];

                    Assert.AreEqual("Enterprise Library Data", lastEntry.Source);
                    Assert.IsTrue(lastEntry.Message.Contains("ThisIsAnUnknownKey"));
                }
                return;
            }

            Assert.Fail("ConfigurationErrorsException expected");
        }

        [TestMethod]
        public void CreateDatabaseDefaultDatabaseWithDatabaseFactory()
        {
            Database db = DatabaseFactory.CreateDatabase();
            Assert.IsNotNull(db);
        }

        [TestMethod]
        public void CreateNamedDatabaseWithDatabaseFactory()
        {
            Database db = DatabaseFactory.CreateDatabase("OracleTest");
            Assert.IsNotNull(db);
        }
    }
}