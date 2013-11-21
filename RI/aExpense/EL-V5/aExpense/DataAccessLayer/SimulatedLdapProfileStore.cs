// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.Unity.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace AExpense.DataAccessLayer
{
    /// <summary>
    /// This is a mock version of a simulated LDAP profile store.
    /// It uses a database as a backing store.
    /// </summary>
    public class SimulatedLdapProfileStore : IProfileStore
    {
        private DataAccessor<ProfileStoreData> accessor;

        public SimulatedLdapProfileStore(Database database)
        {
            this.accessor = database.CreateSprocAccessor<ProfileStoreData>("aexpense_ProfileStore_GetProfileFromUser");
        }

        // this is a mock version of an LDAP query
        // (&(objectCategory=person)(objectClass=user);costCenter;manager;displayName" 
        public Dictionary<string, string> GetAttributesFor(string userName, string[] attributes)
        {
            Guard.ArgumentNotNullOrEmpty(userName, "userName");
            Guard.ArgumentNotNull(attributes, "attributes");

            var profileStore = this.accessor.Execute(userName).FirstOrDefault();

            if (profileStore == null)
                throw new ArgumentException(Properties.Resources.LdapUserNotFound);

            return attributes.ToDictionary(k => k, v =>
            {
                switch (v)
                {
                    case "costCenter":
                        return profileStore.CostCenter;
                    case "manager":
                        return profileStore.Manager;
                    case "displayName":
                        return profileStore.DisplayName;
                    default:
                        return null;
                }
            });
        }

        [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
        private class ProfileStoreData
        {
            public string CostCenter { get; set; }
            public string Manager { get; set; }
            public string DisplayName { get; set; }
        }
    }
}
