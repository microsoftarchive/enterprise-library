//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Policy Injection Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.TestSupport.ObjectsUnderTest
{
    [ConfigurationElementType(typeof(CustomMatchingRuleData))]
    public class TypeMatchingAssignmentRule : IMatchingRule
    {
        private Type matchType;

        public TypeMatchingAssignmentRule(Type matchType)
        {
            this.matchType = matchType;
        }

        public bool Matches(MethodBase member)
        {
            //if(member is Type)
            //{
            //    return member == matchType;
            //}

            return (member.DeclaringType == matchType);
        }
    }
}
