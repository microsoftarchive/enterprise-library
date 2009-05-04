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
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.AppSettings.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.AppSettings.Configuration.Design
{
    /// <summary>
    /// Represents the design time node for a single <see cref="KeyValueConfigurationElement"/>, 
    /// inside the <see cref="AppSettingsSection"/>.
    /// </summary>
    [Image(typeof(AppSettingNode))]
    [SelectedImage(typeof(AppSettingNode))]
    public class AppSettingNode : ConfigurationNode
    {
        private string value;
        
        /// <summary>
        /// Initializes a new <see cref="AppSettingNode"/>.
        /// </summary>
        public AppSettingNode()
            : this(Resources.AppSettingNodeDefaultKey, string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new <see cref="AppSettingNode"/> with a specific key and value.
        /// </summary>
        /// <param name="key">The key for this <see cref="AppSettingNode"/>.</param>
        /// <param name="value">The value for this <see cref="AppSettingNode"/>.</param>
        public AppSettingNode(string key, string value)
            :base(key)
        {
            this.value = value;
        }

        /// <summary>
        /// Gets or sets the value for this setting.
        /// </summary>
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("AppSettingNodeValueDescription", typeof(Resources))]
        public string Value
        {
            get { return value; }
            set { this.value = value; }
        }
    }
}
