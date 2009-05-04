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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// Represnets a configuration source node builder.
    /// </summary>
    public sealed class ConfigurationSourceSectionNodeBuilder : NodeBuilder
    {
        ConfigurationSourceSection configurationSourceSection;
        ConfigurationSourceElementNode defaultNode;

        /// <summary>
        /// Initialize a new instance of the <see cref="ConfigurationSourceSectionNodeBuilder"/> class with a service provider and a configuration section.
        /// </summary>
        /// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
        /// <param name="configurationSourceSection">The configuration section to build.</param>
        public ConfigurationSourceSectionNodeBuilder(IServiceProvider serviceProvider,
                                                     ConfigurationSourceSection configurationSourceSection)
            : base(serviceProvider)
        {
            this.configurationSourceSection = configurationSourceSection;
        }

        /// <summary>
        /// Build a <see cref="ConfigurationSourceSectionNode"/>.
        /// </summary>
        /// <returns>A <see cref="ConfigurationSourceSectionNode"/> object.</returns>
        public ConfigurationSourceSectionNode Build()
        {
            ConfigurationSourceSectionNode rootNode = new ConfigurationSourceSectionNode();
            foreach (ConfigurationSourceElement configurationSourceElement in configurationSourceSection.Sources)
            {
                CreateConfigurationSourceElement(rootNode, configurationSourceElement);
            }
            rootNode.SelectedSource = defaultNode;
            rootNode.RequirePermission = configurationSourceSection.SectionInformation.RequirePermission;
            return rootNode;
        }

        void CreateConfigurationSourceElement(ConfigurationSourceSectionNode node,
                                              ConfigurationSourceElement configurationSourceElement)
        {
            ConfigurationNode sourceNode = NodeCreationService.CreateNodeByDataType(configurationSourceElement.GetType(), new object[] { configurationSourceElement });
            if (null == sourceNode)
            {
                LogNodeMapError(node, configurationSourceElement.GetType());
                return;
            }
            if (configurationSourceSection.SelectedSource == configurationSourceElement.Name) defaultNode = (ConfigurationSourceElementNode)sourceNode;
            node.AddNode(sourceNode);
        }
    }
}
