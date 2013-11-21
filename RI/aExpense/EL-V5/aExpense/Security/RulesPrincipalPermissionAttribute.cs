// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Permissions;
using System.Web;

namespace AExpense.Security
{
    public class RulesPrincipalPermissionAttribute : CodeAccessSecurityAttribute
    {
        public RulesPrincipalPermissionAttribute(SecurityAction action)
            : base(action)
        {            
        }

        public override IPermission CreatePermission()
        {
            return new RulesPrincipalPermission(this.Rule);
        }

        public string Rule { get; set; }
    }
}