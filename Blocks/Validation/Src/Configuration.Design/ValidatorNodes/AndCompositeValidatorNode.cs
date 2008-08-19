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
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.ValidatorNodes
{
    /// <summary>
    /// Respresents the designtime configuration node for an <see cref="AndCompositeValidatorData"/>.
    /// </summary>
    public class AndCompositeValidatorNode : CompositeValidatorNodeBase
    {
        /// <summary>
        /// Creates an instance of <see cref="AndCompositeValidatorNode"/> based on default values.
        /// </summary>
        public AndCompositeValidatorNode()
            : this(new AndCompositeValidatorData(Resources.AndCompositeValidatorNodeName))
        {
        }

        /// <summary>
        /// Creates an instance of <see cref="AndCompositeValidatorNode"/> based on runtime configuration data.
        /// </summary>
        /// <param name="validatorData">The corresponding runtime configuration data.</param>
        public AndCompositeValidatorNode(AndCompositeValidatorData validatorData)
            : base(validatorData.Name)
        {
        }

        /// <summary>
        /// Returns the runtime configuration data that is represented by this node.
        /// </summary>
        /// <returns>An instance of <see cref="AndCompositeValidatorData"/> that can be persisted to a configuration file.</returns>
        public override ValidatorData CreateValidatorData()
        {
            AndCompositeValidatorData validatorData = new AndCompositeValidatorData(Name);

            foreach (ConfigurationNode childNode in Nodes)
            {
                ValidatorNodeBase validatorNode = childNode as ValidatorNodeBase;
                if (validatorNode != null)
                {
                    validatorData.Validators.Add(validatorNode.CreateValidatorData());
                }
            }
            return validatorData;
        }
    }
}
