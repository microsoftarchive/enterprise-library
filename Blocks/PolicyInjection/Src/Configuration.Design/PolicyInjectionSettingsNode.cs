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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design
{
    /// <summary>
    /// Represents the configuration section for the policy injection block.
    /// </summary>
    [Image(typeof(PolicyInjectionSettingsNode))]
    [SelectedImage(typeof(PolicyInjectionSettingsNode))]
    public class PolicyInjectionSettingsNode : ConfigurationSectionNode
    {

        /// <summary>
		/// Gets the name of the <see cref="PolicyInjectionSettingsNode"/> instance.
        /// </summary>
        [ReadOnly(true)]
        public override string Name
        {
            get
            {
                return Resources.PolicyInjectionSettingsNodeName;
            }
            set{}
        }

        /// <summary>
        /// Gets the indication of whether child nodes should be sorted.
        /// </summary>
		/// <value>Always <see langword="false"/> as order is relevant for policies.</value>
        public override bool SortChildren
        {
            get
            {
                return false;
            }
        }
    }
}
