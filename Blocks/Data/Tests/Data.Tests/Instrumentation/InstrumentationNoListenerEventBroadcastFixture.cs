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
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Tests.Instrumentation
{
    [TestClass]
    public class InstrumentationNoListenerEventBroadcastFixture
    {
        [TestMethod]
        public void NoEventBroadcastIfNoEventRegistered()
        {
            string connectionString = @"server=(local)\sqlexpress;database=northwind;integrated security=true;";
            SqlDatabase db = new SqlDatabase(connectionString);

            db.ExecuteNonQuery(CommandType.Text, "Select count(*) from Region");
        }

        [TestMethod]
        public void NoConnectionFailedEventBroadcastWithNoListener()
        {
            string connectionString = @"null;";
            SqlDatabase db = new SqlDatabase(connectionString);
            try
            {
                db.ExecuteNonQuery(CommandType.Text, "Select count(*) from Region");
            }
            catch (ArgumentException) {}
        }
    }
}