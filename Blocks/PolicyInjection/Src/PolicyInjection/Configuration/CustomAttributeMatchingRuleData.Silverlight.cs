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

using Microsoft.Practices.Unity.InterceptionExtension;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration
{
    /// <summary>
    /// Configuration element for the <see cref="CustomAttributeMatchingRule"/> configuration.
    /// </summary>
    public partial class CustomAttributeMatchingRuleData : MatchingRuleData
    {
        /// <summary>
        /// Creates a new <see cref="CustomAttributeMatchingRuleData"/>.
        /// </summary>
        public CustomAttributeMatchingRuleData()
        {
        }

        /// <summary>
        /// Creates a new <see cref="CustomAttributeMatchingRuleData"/> instance.
        /// </summary>
        /// <param name="name">Name of the matching rule.</param>
        /// <param name="attributeTypeName">Name of the attribute type to match on the target.</param>
        /// <param name="searchInheritanceChain">Should we search the inheritance chain to find the attribute?</param>
        public CustomAttributeMatchingRuleData(string name, string attributeTypeName, bool searchInheritanceChain)
        {
            Name = name;
            SearchInheritanceChain = searchInheritanceChain;
            AttributeTypeName = attributeTypeName;
        }

        /// <summary>
        /// Should we search the inheritance chain to find the attribute?
        /// </summary>
        /// <value>The "searchInheritanceChain" config attribute.</value>
        public bool SearchInheritanceChain
        {
            get;
            set;
        }

        /// <summary>
        /// Name of attribute type to match.
        /// </summary>
        /// <value>The "attributeType" config attribute.</value>
        public string AttributeTypeName
        {
            get; 
            set;
        }
    }
}
