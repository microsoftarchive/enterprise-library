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

using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Instrumentation.Tests
{
    [TestClass]
    public class DataInstrumentationProviderWmiFixture
    {
        [TestMethod]
        public void DoLotsOfConnectionFailures()
        {
            int numberOfEvents = 50;
            using (WmiEventWatcher eventListener = new WmiEventWatcher(numberOfEvents))
            {
                var instrumentationProvider = new NewDataInstrumentationProvider("foo", true, true, true,
                                                                                 "ApplicationInstanceName");
                SqlDatabase db = new SqlDatabase("BadConnectionString", instrumentationProvider);

                for (int i = 0; i < numberOfEvents; i++)
                {
                    try
                    {
                        db.ExecuteScalar(CommandType.Text, "Select count(*) from Region");
                    }
                    catch { }
                }

                eventListener.WaitForEvents();

                Assert.AreEqual(numberOfEvents, eventListener.EventsReceived.Count);
                Assert.AreEqual("ConnectionFailedEvent", eventListener.EventsReceived[0].ClassPath.ClassName);
                Assert.AreEqual("foo", eventListener.EventsReceived[0].GetPropertyValue("InstanceName"));
                Assert.AreEqual(db.ConnectionStringWithoutCredentials, eventListener.EventsReceived[0].GetPropertyValue("ConnectionString"));
            }
        }

        [TestMethod]
        public void CommandFailureFiresWmiEvent()
        {
            using (WmiEventWatcher eventListener = new WmiEventWatcher(1))
            {
                Database db = DatabaseFactory.CreateDatabase();

                try
                {
                    db.ExecuteNonQuery(CommandType.StoredProcedure, "BadCommandText");
                }
                catch { }

                eventListener.WaitForEvents();
                Assert.AreEqual(1, eventListener.EventsReceived.Count);
                Assert.AreEqual("CommandFailedEvent", eventListener.EventsReceived[0].ClassPath.ClassName);
                Assert.AreEqual("Service_Dflt", eventListener.EventsReceived[0].GetPropertyValue("InstanceName"));
                Assert.AreEqual(db.ConnectionStringWithoutCredentials, eventListener.EventsReceived[0].GetPropertyValue("ConnectionString"));
                Assert.AreEqual("BadCommandText", eventListener.EventsReceived[0].GetPropertyValue("CommandText"));
            }
        }
    }

    public class FixedPrefixNameFormatter : IPerformanceCounterNameFormatter
    {
        string prefix;

        public FixedPrefixNameFormatter(string prefix)
        {
            this.prefix = prefix;
        }

        public string CreateName(string nameSuffix)
        {
            return prefix + nameSuffix;
        }
    }
}
