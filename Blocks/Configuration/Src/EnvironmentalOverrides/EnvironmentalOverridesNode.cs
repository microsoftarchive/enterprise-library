//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
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
using Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides
{
    /// <summary>
    /// Represents the design time node container for <see cref="EnvironmentNode"/> nodes.
    /// </summary>
    [Image(typeof(EnvironmentalOverridesNode))]
    [SelectedImage(typeof(EnvironmentalOverridesNode))]
    public class EnvironmentalOverridesNode : ConfigurationNode
    {
        /// <summary>
        /// Initialize a new instance of <see cref="EnvironmentalOverridesNode"/>.
        /// </summary>
        public EnvironmentalOverridesNode()
            : base(Resources.EnvironmentalOverridesNodeName)
        {
        }

        /// <summary>
        /// Gets the name of the node.
        /// </summary>
        [ReadOnly(true)]
        public override string Name
        {
            get
            {
                return base.Name;
            }
        }

        /// <summary>
        /// Performs validation on the <see cref="EnvironmentNode"/> instances contained by this node.
        /// </summary>
        /// <param name="errors">A list of <see cref="ValidationError"/> that is udpated with errors found during validation.</param>
        public override void Validate(IList<ValidationError> errors)
        {
            List<string> environmentDeltaFileNames = new List<string>();
            foreach (EnvironmentNode childNode in Hierarchy.FindNodesByType(this, typeof(EnvironmentNode)))
            {
                string path = childNode.EnvironmentDeltaFile;
                if (!environmentDeltaFileNames.Contains(path))
                {
                    environmentDeltaFileNames.Add(path);
                }
                else
                {
                    errors.Add(new ValidationError(childNode, "EnvironmentDeltaFile", Resources.DuplicateDeltaFilePath));
                }
            }
        }
    }
}
