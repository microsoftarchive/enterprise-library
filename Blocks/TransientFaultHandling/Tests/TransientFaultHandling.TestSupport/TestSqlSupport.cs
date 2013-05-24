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

namespace Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling.TestSupport
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Configuration;

    public class TestSqlSupport
    {
        public const string ValidSqlText = "SELECT [ProductCategoryID], [Name] FROM [SalesLT].[ProductCategory]";

        public const string InvalidSqlText = "invalid sql";

        public const string ValidSqlScalarQuery = "SELECT count(*) FROM [SalesLT].[ProductCategory]";

        public const string ValidForXmlSqlQuery = "SELECT count(*) FROM [SalesLT].[ProductCategory] FOR XML AUTO, ELEMENTS";

        public const string InvalidConnectionString = "Data Source=tcp:invalidserver.database.windows.net;Initial Catalog=invalidcatalog;User ID=invaliduserid;Password=invalidpassword;";

        private static string sqlDatabaseConnectionStringValue = ConfigurationHelper.GetSetting("SqlDatabaseAdventureWorksLT") ?? ConfigurationManager.ConnectionStrings["SqlDatabaseAdventureWorksLT"].ConnectionString;

        public static string SqlDatabaseConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(sqlDatabaseConnectionStringValue)
                    || sqlDatabaseConnectionStringValue.Contains("[INSERT SERVER NAME HERE]")
                    || sqlDatabaseConnectionStringValue.Contains("[INSERT USER ID HERE]")
                    || sqlDatabaseConnectionStringValue.Contains("[INSERT PASSWORD HERE]"))
                {
                    Assert.Inconclusive("Cannot run tests because the Windows Azure SQL Database credentials are not configured in app.config. Please configure it to point to a SQL Database where the AdventureWorks Light (LT) sample database is installed.");
                }

                return sqlDatabaseConnectionStringValue;
            }
        }
    }
}
