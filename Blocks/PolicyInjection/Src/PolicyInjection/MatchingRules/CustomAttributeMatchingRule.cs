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
using System.Text;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Properties;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.MatchingRules
{
    /// <summary>
    /// An implementation of <see cref="IMatchingRule"/> that checks to see if
    /// the member tested has an arbitrary attribute applied.
    /// </summary>
    [ConfigurationElementType(typeof(CustomAttributeMatchingRuleData))]
    public class CustomAttributeMatchingRule : IMatchingRule
    {
        private readonly Type attributeType;
        private readonly bool inherited;

        /// <summary>
        /// Constructs a new <see cref="CustomAttributeMatchingRule"/>.
        /// </summary>
        /// <param name="attributeType">Attribute to match.</param>
        /// <param name="inherited">If true, checks the base class for attributes as well.</param>
        public CustomAttributeMatchingRule(Type attributeType, bool inherited)
        {
            if (attributeType == null)
            {
                throw new ArgumentNullException("attributeType");
            }
            if (!attributeType.IsSubclassOf(typeof(Attribute)))
            {
                throw new ArgumentException(Resources.ExceptionAttributeNoSubclassOfAttribute, "attributeType");
            }

            this.attributeType = attributeType;
            this.inherited = inherited;
        }

        /// <summary>
        /// Checks to see if the given <paramref name="member"/> matches the rule.
        /// </summary>
        /// <param name="member">Member to check.</param>
        /// <returns>true if it matches, false if not.</returns>
        public bool Matches(MethodBase member)
        {
            object[] attribues = member.GetCustomAttributes(attributeType, inherited);

            return (attribues != null && attribues.Length > 0);
        }
    }
}
