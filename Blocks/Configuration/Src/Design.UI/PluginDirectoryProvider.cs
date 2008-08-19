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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.UI
{
    internal class PluginDirectoryProvider : IPluginDirectoryProvider
    {
        string pluginDirectory;

        public PluginDirectoryProvider(string pluginDirectory)
        {
            this.pluginDirectory = pluginDirectory;
        }

        #region IPluginDirectoryProvider Members

        public string PluginDirectory
        {
            get { return pluginDirectory; }
        }

        #endregion
    }
}
