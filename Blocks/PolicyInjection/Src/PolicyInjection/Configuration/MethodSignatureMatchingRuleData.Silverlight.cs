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

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration
{
    /// <summary>
    /// Configuration element that stores the configuration information for an instance
    /// of <see cref="MethodSignatureMatchingRule"/>.
    /// </summary>
    public partial class MethodSignatureMatchingRuleData
    {
        private readonly NamedElementCollection<ParameterTypeElement> parameters = new NamedElementCollection<ParameterTypeElement>();

        /// <summary>
        /// The collection of parameters that make up the matching method signature.
        /// </summary>
        public NamedElementCollection<ParameterTypeElement> Parameters
        {
            get { return this.parameters; }
        }
    }

    /// <summary>
    /// A configuration element representing a single parameter in a method signature.
    /// </summary>
    public partial class ParameterTypeElement : NamedConfigurationElement
    {
        /// <summary>
        /// The parameter type required.
        /// </summary>
        public string ParameterTypeName
        {
            get; 
            set; 
        }
    }
}
