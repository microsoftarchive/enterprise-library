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
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.AppSettings.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.AppSettings.Configuration.Design
{
    /// <summary>
    /// Represents an appSetting inside the <see cref="System.Configuration.AppSettingsSection"/> that is not defined in the 
    /// opened configuration file. These settings should not be saved in the opened configuration file and therefore the properties are readonly.
    /// </summary>
    [Image(typeof(ReadOnlyAppSettingNode))]
    [SelectedImage(typeof(ReadOnlyAppSettingNode))]
    public class ReadOnlyAppSettingNode : ConfigurationNode
    {
        private string value;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyAppSettingNode"/>.
        /// </summary>
        /// <param name="key">The key for this <see cref="ReadOnlyAppSettingNode"/>.</param>
        /// <param name="value">The value for this <see cref="ReadOnlyAppSettingNode"/>.</param>
        public ReadOnlyAppSettingNode(string key, string value)
            :base(key)
        {
            this.value = value;
        }

        /// <summary>
        /// Gets the name for this node, which will also be used as the key for the setting in the configurationfile.
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
        /// Gets the value for this setting.
        /// </summary>
        [ReadOnly(true)]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("ReadOnlyAppSettingNodeValueDescription", typeof(Resources))]
        public string Value
        {
            get
            {
                return value;
            }
        }
    }
}
