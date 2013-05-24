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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Tests.TestSupport
{
    internal class EnvironmentHelper
    {
        private static bool? oracleClientIsInstalled;
        private static string oracleClientNotInstalledErrorMessage;

        public static void AssertOracleClientIsInstalled()
        {
            if (!oracleClientIsInstalled.HasValue)
            {
                try
                {
                    var factory = new DatabaseProviderFactory(OracleTestConfigurationSource.CreateConfigurationSource());
                    var db = factory.Create("OracleTest");
                    var conn = db.CreateConnection();
                    conn.Open();
                    conn.Close();
                }
                catch (Exception ex)
                {
                    if (ex.Message != null && ex.Message.Contains("System.Data.OracleClient")
                        && ex.Message.Contains("8.1.7"))
                    {
                        oracleClientIsInstalled = false;
                        oracleClientNotInstalledErrorMessage = ex.Message;
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            if (oracleClientIsInstalled.HasValue && oracleClientIsInstalled.Value == false)
            {
                Assert.Inconclusive(oracleClientNotInstalledErrorMessage);
            }
        }
    }
}
