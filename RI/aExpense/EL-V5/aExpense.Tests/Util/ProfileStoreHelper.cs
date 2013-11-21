// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using AExpense.DataAccessLayer;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AExpense.Tests.Util
{
    public static class ProfileStoreHelper
    {
        public static SimulatedLdapProfileStore GetProfileStore(string database)
        {
            var db = DatabaseFactory.CreateDatabase(database);

            return new SimulatedLdapProfileStore(db);
        }
    }
}
