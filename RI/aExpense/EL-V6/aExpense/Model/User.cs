// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

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