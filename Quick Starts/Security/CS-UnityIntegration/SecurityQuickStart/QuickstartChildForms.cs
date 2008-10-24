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
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;

namespace SecurityQuickStart
{
    /// <summary>
    /// This class holds the instances of all the child forms
    /// the main quickstart form requires. This reduces the
    /// size of the main form's constructor parameter list.
    /// </summary>
    public class QuickstartChildForms
    {
        [Dependency]
        public AddUserRoleForm AddUserRoleForm { get; set; }

        [Dependency]
        public UserRoleForm UserRoleForm { get; set; }
        
        [Dependency]
        public UsersForm UsersForm { get; set; }
        
        [Dependency]
        public CredentialsForm CredentialsForm { get; set; }
        
        [Dependency]
        public ProfileForm ProfileForm { get; set; }
     
        [Dependency]
        public RoleAuthorizationForm RoleAuthForm { get; set; }
    }
}
