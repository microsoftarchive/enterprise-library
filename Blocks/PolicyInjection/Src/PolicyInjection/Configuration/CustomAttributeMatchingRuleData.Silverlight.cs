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
    partial class CustomAttributeMatchingRuleData
    {
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
