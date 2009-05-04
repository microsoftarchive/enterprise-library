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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Console
{
    /// <summary>
    /// TODO: ADD COMMENT
    /// </summary>
    class AppDomainBasePluginDirectoryProvider : IPluginDirectoryProvider
    {
        #region IPluginDirectoryProvider Members

        /// <summary>
        /// TODO: ADD COMMENT
        /// </summary>
        public string PluginDirectory
        {
            get { return AppDomain.CurrentDomain.BaseDirectory; }
        }

        #endregion
    }
}
