// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

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