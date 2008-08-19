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

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    /// <summary>
    /// TODO: Add Comment
    /// </summary>
    public interface IProtectedConfigurationSource
    {
        /// <summary>
        /// TODO: Add Comment
        /// </summary>
        /// <param name="saveParameter"></param>
        /// <param name="sectionName"></param>
        /// <param name="configurationSection"></param>
        /// <param name="protectionProviderName"></param>
        void Add(IConfigurationParameter saveParameter, string sectionName, ConfigurationSection configurationSection, string protectionProviderName);


    }
}
