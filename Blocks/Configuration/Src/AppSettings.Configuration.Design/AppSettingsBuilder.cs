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

using System.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.AppSettings.Configuration.Design
{
    /// <summary>
    /// Represents a builder for an <see cref="AppSettingsSection"/>.
    /// </summary>
    public class AppSettingsBuilder
    {
        readonly AppSettingsNode appSettingsNode;

        /// <summary>
        /// Initialize a new instance of the <see cref="AppSettingsBuilder"/>
        /// </summary>
        /// <param name="appSettingsNode"></param>
        public AppSettingsBuilder(AppSettingsNode appSettingsNode)
        {
            this.appSettingsNode = appSettingsNode;
        }

        /// <summary>
        /// Builds an <see cref="AppSettingsSection"/> object.
        /// </summary>
        /// <returns>An <see cref="AppSettingsSection"/> object.</returns>
        public AppSettingsSection Build()
        {
            AppSettingsSection appSettings = new AppSettingsSection();
            appSettings.File = appSettingsNode.File;

            foreach (AppSettingNode singleAppSetting in appSettingsNode.Hierarchy.FindNodesByType(appSettingsNode, typeof(AppSettingNode)))
            {
                KeyValueConfigurationElement keyValueElement = new KeyValueConfigurationElement(singleAppSetting.Name, singleAppSetting.Value);
                appSettings.Settings.Add(keyValueElement);
            }
            return appSettings;
        }
    }
}
