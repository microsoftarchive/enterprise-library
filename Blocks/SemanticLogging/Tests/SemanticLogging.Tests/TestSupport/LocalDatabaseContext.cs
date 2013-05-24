#region license
// ==============================================================================
// Microsoft patterns & practices Enterprise Library
// Semantic Logging Application Block
// ==============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ==============================================================================
#endregion

using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Tests.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Transactions;

namespace Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Tests.TestSupport
{
    public abstract class LocalDatabaseContext : ContextBase
    {
        protected const string LocalDbConnectionString = @"Data Source=(LocalDB)\v11.0;Initial Catalog=master;Integrated Security=True";

        protected string dbFileName;
        protected string dbLogFileName;

        protected string dbName;
        protected SqlConnection LocalDbConnection = new SqlConnection(LocalDbConnectionString);

        protected abstract string GetLocalDatabaseFileName();

        protected override void Given()
        {
            this.dbName = this.GetLocalDatabaseFileName();

            if (string.IsNullOrWhiteSpace(dbName))
            {
                Assert.Inconclusive("You must specify a valid database name");
            }

            var output = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            this.dbFileName = Path.Combine(output, dbName + ".mdf");
            this.dbLogFileName = Path.Combine(output, string.Format("{0}_log.ldf", dbName));

            this.LocalDbConnection.Open();

            // Recover from failed run
            this.DetachDatabase();

            File.Delete(this.dbFileName);
            File.Delete(this.dbLogFileName);

            using (var cmd = new SqlCommand(string.Format("CREATE DATABASE {0} ON (NAME = N'{0}', FILENAME = '{1}')", this.dbName, this.dbFileName), this.LocalDbConnection))
            {
                cmd.ExecuteNonQuery();
            }
        }

        protected override void OnCleanup()
        {
            using (var cmd = new SqlCommand(string.Format("ALTER DATABASE {0} SET SINGLE_USER WITH ROLLBACK IMMEDIATE", this.dbName), this.LocalDbConnection))
            {
                cmd.ExecuteNonQuery();
            }

            this.LocalDbConnection.ChangeDatabase("master");
            this.DetachDatabase();
            this.LocalDbConnection.Dispose();

            File.Delete(this.dbFileName);
            File.Delete(this.dbLogFileName);
        }

        protected string GetSqlConnectionString()
        {
            var cs = string.Format(@"Data Source=(LocalDB)\v11.0;AttachDBFileName={1};Initial Catalog={0};Integrated Security=True;", this.dbName, this.dbFileName);

            return cs;
        }

        protected void DetachDatabase()
        {
            using (var cmd = new SqlCommand(string.Format("IF EXISTS (SELECT * FROM sys.databases WHERE Name = N'{0}') exec sp_detach_db N'{0}'", dbName), this.LocalDbConnection))
            {
                cmd.ExecuteNonQuery();
            }
        }
    }
}
