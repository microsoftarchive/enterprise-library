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
using System.Data.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Data.TestSupport
{
    public class ParameterDiscoveryFixture
    {
        DbCommand storedProcedure;

        public ParameterDiscoveryFixture(DbCommand storedProcedure)
        {
            this.storedProcedure = storedProcedure;
        }

        public void CanCreateStoredProcedureCommand()
        {
            Assert.AreEqual(storedProcedure.CommandType, CommandType.StoredProcedure);
        }

        public class TestCache : ParameterCache
        {
            public bool CacheUsed = false;

            protected override void AddParametersFromCache(DbCommand command,
                                                           Database database)
            {
                CacheUsed = true;
                base.AddParametersFromCache(command, database);
            }
        }
    }
}
