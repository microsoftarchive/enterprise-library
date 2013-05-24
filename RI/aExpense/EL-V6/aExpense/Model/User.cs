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

using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AExpense.Model
{
    [Serializable]
    public class User
    {
        public virtual string UserName { get; set; }

        public string FullName { get; set; }

        public string Manager { get; set; }

        public ICollection<string> Roles { get; set; }

        public string CostCenter { get; set; }

        public ReimbursementMethod PreferredReimbursementMethod { get; set; }
    }
}