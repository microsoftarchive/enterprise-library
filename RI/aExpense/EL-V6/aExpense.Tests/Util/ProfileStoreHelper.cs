// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using AExpense.DataAccessLayer;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace AExpense.Tests.Util
{
    public static class ProfileStoreHelper
    {
        private static readonly DatabaseProviderFactory dbFactory = new DatabaseProviderFactory(ConfigurationSourceFactory.Create());

        public static SimulatedLdapProfileStore GetProfileStore(string database)
        {
            var db = dbFactory.Create(database);

            return new SimulatedLdapProfileStore(db);
        }
    }
}
