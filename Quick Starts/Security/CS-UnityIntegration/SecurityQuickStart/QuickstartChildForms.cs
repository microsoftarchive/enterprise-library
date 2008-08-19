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
