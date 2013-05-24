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