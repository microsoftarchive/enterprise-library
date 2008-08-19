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

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.ValidatorNodes
{
    /// <summary>
    /// Represents the designtime configuration node for any <see cref="ValidatorData"/>.
    /// </summary>
    [Image(typeof(ValidatorNodeBase))]
    [SelectedImage(typeof(ValidatorNodeBase))]
    public abstract class ValidatorNodeBase : ConfigurationNode
    {
        /// <summary>
        /// Creates an instance of <see cref="ValidatorNodeBase"/> based on a name for the configuration node.
        /// </summary>
        /// <param name="name">The name that should be used to display this configuration node.</param>
        protected ValidatorNodeBase(string name)
            : base(name)
        {
        }

        /// <summary>
        /// When overriden in a derived class, this method returns the runtime configuration data represented by this node.
        /// </summary>
        /// <returns>The runtime configuration data represented by this node, that can de persisted to a configuration file.</returns>
        public abstract ValidatorData CreateValidatorData();
    }
}
