//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block QuickStart
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Security;

namespace SecurityQuickStart
{
    /// <summary>
    /// Extension methods for the ASP.NET <see cref="System.Web.Security.MembershipProvider"/> class
    /// to provide some of the convenience overloads that are provided by the static <see cref="System.Web.Security.Membership"/>
    /// static facade.
    /// </summary>
    static class MembershipProviderExtensions
    {
        public static MembershipUser CreateUser(this MembershipProvider provider, string username, string password)
        {
            MembershipCreateStatus status;

            MembershipUser user = provider.CreateUser(username, password, null, null, null, true, null, out status);
            if (user == null)
            {
                throw new MembershipCreateUserException(status);
            }
            return user;
        }

        public static bool DeleteUser(this MembershipProvider provider, string username)
        {
            return provider.DeleteUser(username, true);
        }

        public static MembershipUserCollection GetAllUsers(this MembershipProvider provider)
        {
            int totalRecords;
            return provider.GetAllUsers(0, Int32.MaxValue, out totalRecords);
        }
    }
}
