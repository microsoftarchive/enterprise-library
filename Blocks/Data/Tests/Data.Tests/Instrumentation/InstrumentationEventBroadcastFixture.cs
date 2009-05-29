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
using System.Data;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Data.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Tests.Instrumentation
{
    [TestClass]
    public class InstrumentationEventBroadcastFixture
    {
        const string connectionString = @"server=(local)\sqlexpress;database=northwind;integrated security=true;";

        [TestMethod]
        public void ConnectionOpenedEventBroadcast()
        {
            var mockProvider = new Mock<IDataInstrumentationProvider>();
            mockProvider.Setup(p => p.FireConnectionOpenedEvent())
                .Verifiable();

            var sqlDb = new SqlDatabase(connectionString, mockProvider.Object);
            sqlDb.ExecuteNonQuery(CommandType.Text, "Select count(*) from Region");

            mockProvider.Verify();
        }

        [TestMethod]
        public void ConnectionFailedEventBroadcast()
        {
            var mockProvider = new Mock<IDataInstrumentationProvider>();
            mockProvider.Setup(p => p.FireConnectionFailedEvent("invalid;", It.IsAny<Exception>()))
                .Verifiable();

            var sqlDb = new SqlDatabase("invalid;", mockProvider.Object);

            try
            {
                sqlDb.ExecuteNonQuery(CommandType.Text, "Select count(*) from Region");
            }
            catch (ArgumentException) {}

            mockProvider.Verify();
        }

        [TestMethod]
        public void CommandExecutedEventBroadcast()
        {
            DateTime commandStartedTime = DateTime.MinValue;

            var mockProvider = new Mock<IDataInstrumentationProvider>();
            mockProvider.Setup(p => p.FireCommandExecutedEvent(It.IsAny<DateTime>()))
                .Callback<DateTime>(d => commandStartedTime = d)
                .Verifiable();

            var sqlDb = new SqlDatabase(connectionString, mockProvider.Object);

            sqlDb.ExecuteNonQuery(CommandType.Text, "Select count(*) from Region");

            mockProvider.Verify();
            AssertDateIsWithinBounds(DateTime.Now, commandStartedTime, 2);
        }

        [TestMethod]
        public void CommandFailedEventBroadcast()
        {
            string commandText = null;

            var mockProvider = new Mock<IDataInstrumentationProvider>();
            mockProvider.Setup(
                p => p.FireCommandFailedEvent(It.IsAny<string>(), connectionString, It.IsAny<Exception>()))
                .Callback<string, string, Exception>((c, cs, ex) => commandText = c)
                .Verifiable();

            var sqlDb = new SqlDatabase(connectionString, mockProvider.Object);

            try
            {
                sqlDb.ExecuteNonQuery(CommandType.StoredProcedure, "NonExistentStoredProcedure");
            }
            catch (SqlException) {}

            mockProvider.Verify();
            Assert.AreEqual("NonExistentStoredProcedure", commandText);
        }

        static void AssertDateIsWithinBounds(DateTime expectedTime,
                                      DateTime actualTime,
                                      int maxDifference)
        {
            int diff = (expectedTime - actualTime).Seconds;
            Assert.IsTrue(diff <= maxDifference);
        }
    }
}
