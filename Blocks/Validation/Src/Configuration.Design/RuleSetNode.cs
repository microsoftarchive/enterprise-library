//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Validation Application Block
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
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design
{
    /// <summary>
    /// Respresents the designtime configuration node for a <see cref="ValidationRulesetData"/>.
    /// </summary>
    [Image(typeof(RuleSetNode))]
    [SelectedImage(typeof(RuleSetNode))]
    public class RuleSetNode : ConfigurationNode
    {

        /// <summary>
        /// Creates an instance of <see cref="RuleSetNode"/> based on default values.
        /// </summary>
        public RuleSetNode()
            : this(new ValidationRulesetData(Resources.RuleSetNodeName))
        {
        }

        /// <summary>
        /// Creates an instance of <see cref="RuleSetNode"/> based on runtime configuration data.
        /// </summary>
        /// <param name="ruleData">The corresponding runtime configuration data.</param>
        public RuleSetNode(ValidationRulesetData ruleData)
            : base(ruleData.Name)
        {
        }

    }
}
