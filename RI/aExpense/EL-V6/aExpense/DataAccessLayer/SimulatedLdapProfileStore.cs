#region license
// ==============================================================================
// Microsoft patterns & practices Enterprise Library
// aExpense Reference Implementation
// ==============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ==============================================================================
#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AExpense.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Utility;

namespace AExpense.DataAccessLayer
{
    /// <summary>
    /// This is a mock version of a simulated LDAP profile store.
    /// It uses a database as a backing store.
    /// </summary>
    public class SimulatedLdapProfileStore : IProfileStore
    {
        private readonly DataAccessor<ProfileStoreData> accessor;
        private readonly RetryPolicy retryPolicy;

        [InjectionConstructor]
        public SimulatedLdapProfileStore(Database database)
        {
            this.accessor = database.CreateSprocAccessor<ProfileStoreData>("aexpense_ProfileStore_GetProfileFromUser");
            this.retryPolicy = RetryPolicyFactory.GetRetryPolicy<SqlDatabaseTransientErrorDetectionStrategy>();

            AExpenseEvents.Log.ProfileStoreInitialized();
        }

        // this is a mock version of an LDAP query
        // (&(objectCategory=person)(objectClass=user);costCenter;manager;displayName" 
        public Dictionary<string, string> GetAttributesFor(string userName, string[] attributes)
        {
            Guard.ArgumentNotNullOrEmpty(userName, "userName");
            Guard.ArgumentNotNull(attributes, "attributes");

            var profileStore = this.retryPolicy.ExecuteAction<ProfileStoreData>(() => this.accessor.Execute(userName).FirstOrDefault());

            if (profileStore == null)
            {
                throw new ArgumentException(Properties.Resources.LdapUserNotFound);
            }

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
