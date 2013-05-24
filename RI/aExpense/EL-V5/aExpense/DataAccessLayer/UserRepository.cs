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

using AExpense.Model;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Utility;
using System;
using System.Collections.Generic;
using System.Web.Profile;
using System.Web.Security;

namespace AExpense.DataAccessLayer
{
    public class UserRepository : IUserRepository
    {
        private IProfileStore profileStore;        
        private ExceptionManager exManager;
        private IUnityContainer container;

        public UserRepository(IProfileStore profileStore, ExceptionManager exManager, IUnityContainer container)
        {
            this.profileStore = profileStore;
            this.exManager = exManager;
            this.container = container;
        }

        public User GetUser(string userName, bool throwOnError = true)
        {
            Guard.ArgumentNotNullOrEmpty(userName, "userName");

            return exManager.Process(() =>
                {
                    var membershipUser = Membership.GetUser(userName);
                    if (membershipUser == null)
                    {
                        if (throwOnError)
                        {
                            throw new InvalidOperationException(Properties.Resources.MembershipUserNotFound);
                        }
                        return null;
                    }

                    var attributes = profileStore.GetAttributesFor(userName, new[] { "costCenter", "manager", "displayName" });

                    var user = this.container.Resolve<User>();
                    user.Roles = Roles.GetRolesForUser(userName);
                    user.CostCenter = attributes["costCenter"];
                    user.FullName = attributes["displayName"];
                    user.Manager = attributes["manager"];
                    // TracingBehavior should trigger here
                    user.UserName = membershipUser.UserName;

                    var profile = ProfileBase.Create(userName);
                    user.PreferredReimbursementMethod = string.IsNullOrEmpty(profile.GetProperty<string>("PreferredReimbursementMethod")) ?
                                                         ReimbursementMethod.NotSet : 
                                                         (ReimbursementMethod)Enum.Parse(typeof(ReimbursementMethod), profile.GetProperty<string>("PreferredReimbursementMethod"));
                    return user;
                }, 
                Constants.NotifyPolicy);
        }

        public void UpdateUserPreferredReimbursementMethod(User user)
        {
            exManager.Process(() =>
                {
                    Guard.ArgumentNotNull(user, "user");

                    var profile = ProfileBase.Create(user.UserName);

                    profile.SetPropertyValue("PreferredReimbursementMethod", Enum.GetName(typeof(ReimbursementMethod), user.PreferredReimbursementMethod));
                    profile.Save();
                },
                Constants.NotifyPolicy);
        }                
    }
}