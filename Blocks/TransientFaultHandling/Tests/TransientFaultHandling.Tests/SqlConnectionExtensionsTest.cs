#region license
// ==============================================================================
// Microsoft patterns & practices Enterprise Library
// Transient Fault Handling Application Block
// ==============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ==============================================================================
#endregion

using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling.Tests
{
    [TestClass]
    public class SqlConnectionExtensionsTest
    {
        [TestInitialize]
        public void Setup()
        {
            RetryPolicyFactory.CreateDefault();
        }

        [TestCleanup]
        public void Cleanup()
        {
            RetryPolicyFactory.SetRetryManager(null, false);
        }

        [Description("F5.1.1")]
        [Priority(1)]
        [TestMethod]
        public void TestSqlConnectionExtensions()
        {
            using (SqlConnection connection = new SqlConnection(TestSqlSupport.SqlDatabaseConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SELECT [ProductCategoryID], [Name] FROM [SalesLT].[ProductCategory]", connection))
                {
                    connection.OpenWithRetry();

                    using (IDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string name = reader.GetString(1);

                            Trace.WriteLine(string.Format("{0}: {1}", id, name));
                        }

                        reader.Close();
                    }

                    connection.Close();
                }
            }
        }
    }
}
