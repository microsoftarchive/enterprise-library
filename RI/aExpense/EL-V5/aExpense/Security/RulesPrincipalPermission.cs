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
using System.Security;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security;

namespace AExpense.Security
{
    public class RulesPrincipalPermission : IPermission
    {
        private string rule;

        public RulesPrincipalPermission(string rule)
        {
            this.rule = rule;
        }

        public IPermission Copy()
        {
            throw new NotImplementedException();
        }

        public void Demand()
        {
            var provider  = EnterpriseLibraryContainer.Current.GetInstance<IAuthorizationProvider>("Authorization Rule Provider");
            if(!provider.Authorize(Thread.CurrentPrincipal, this.rule))
            {
                throw new SecurityException(Properties.Resources.ApproverNotAuthorized);
            }
        }

        public IPermission Intersect(IPermission target)
        {
            throw new NotImplementedException();
        }

        public bool IsSubsetOf(IPermission target)
        {
            return false;
        }

        public IPermission Union(IPermission target)
        {
            throw new NotImplementedException();
        }

        public void FromXml(SecurityElement e)
        {
            throw new NotImplementedException();
        }

        public SecurityElement ToXml()
        {
            throw new NotImplementedException();
        }
    }
}