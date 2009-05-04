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
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.AppSettings.Configuration.Design
{
    /// <summary>
    /// Represents a a builder for <see cref="AppSettingsNode"/> objects.
    /// </summary>
    public class AppSettingsNodeBuilder : NodeBuilder
    {
        readonly AppSettingsSection appSettingsSection;

        /// <summary>
        /// Initialize a new instance of the <see cref="NodeBuilder"/> class with a <see cref="IServiceProvider"/> and the <see cref="AppSettingsSection"/> to build the node.
        /// </summary>
        /// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
        /// <param name="appSettingsSection">The <see cref="AppSettingsSection"/> to build the node.</param>
        public AppSettingsNodeBuilder(IServiceProvider serviceProvider,
                                      AppSettingsSection appSettingsSection)
            : base(serviceProvider)
        {
            this.appSettingsSection = appSettingsSection;
        }

        /// <summary>
        /// Builds an <see cref="AppSettingsNode"/>.
        /// </summary>
        /// <returns>An <see cref="AppSettingsNode"/> object.</returns>
        public AppSettingsNode Build()
        {
            AppSettingsNode rootNode = new AppSettingsNode();
            rootNode.File = appSettingsSection.File;
            foreach (KeyValueConfigurationElement singleSetting in appSettingsSection.Settings)
            {
                AppSettingNode singleSettingNode = new AppSettingNode(singleSetting.Key, singleSetting.Value);
                rootNode.AddNode(singleSettingNode);
            }
            return rootNode;
        }
    }
}
