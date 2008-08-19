//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation. All rights reserved.
// Adapted from ACA.NET with permission from Avanade Inc.
// ACA.NET copyright © Avanade Inc. All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Console
{
    class ConfigurationErrorListViewItem : ConfigurationNodeListViewItem
    {
        private ConfigurationError error;

        public ConfigurationErrorListViewItem(ConfigurationError error) : base()
        {
            this.error = error;
            if (error.ConfigurationNode != null)
            {
                Text = error.ConfigurationNode.Name;
            }
            StateImageIndex = 0;
            SubItems.Add(String.Empty);
            SubItems.Add(error.Message);
            if (error.ConfigurationNode != null)
            {
                SubItems.Add(error.ConfigurationNode.Path);    
            }
        }

        public override ConfigurationNode ConfigurationNode
        {
            get { return error.ConfigurationNode; }
        }
    }
}