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
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.Policies.Tests.ObjectsUnderTest
{
    class AttributeMatchingRule : IMatchingRule
    {
        private string tagToMatch;

        public AttributeMatchingRule(string tagToMatch)
        {
            this.tagToMatch = tagToMatch;
        }


        public bool Matches(MemberInfo member)
        {
            foreach( Attribute attr in member.GetCustomAttributes(typeof(TagAttribute), true))
            {
                TagAttribute tagAttr = (TagAttribute)attr;
                if( tagAttr.Tag == tagToMatch)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
