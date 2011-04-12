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

using System.Xml;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration
{
    /// <summary>
    /// A settings class that stores the policy set in configuration.
    /// </summary>
    public partial class PolicyInjectionSettings : ConfigurationSection
    {
        private NamedElementCollection<PolicyData> policies = new NamedElementCollection<PolicyData>();

        /// <summary>
        /// Gets the collection of Policies.
        /// </summary>
        public NamedElementCollection<PolicyData> Policies
        {
            get { return policies; }
        }
    }
}
