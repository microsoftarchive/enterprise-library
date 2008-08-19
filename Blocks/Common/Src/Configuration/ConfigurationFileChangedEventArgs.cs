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

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    /// <summary>
    /// </summary>
    [Serializable]
    public class ConfigurationFileChangedEventArgs : ConfigurationChangedEventArgs
    {
        private readonly string configurationFile;


        /// <summary>
        /// <para>Initialize a new instance of the <see cref="ConfigurationChangingEventArgs"/> class with the configuration file, the section name, the old value, and the new value of the changes.</para>
        /// </summary>
        /// <param name="configurationFile"><para>The configuration file where the change occured.</para></param>
        /// <param name="sectionName"><para>The section name of the changes.</para></param>
        public ConfigurationFileChangedEventArgs(string configurationFile, string sectionName) : base(sectionName)
        {
            this.configurationFile = configurationFile;
        }

        /// <summary>
        /// <para>Gets the configuration file of the data that is changing.</para>
        /// </summary>
        /// <value>
        /// <para>The configuration file of the data that is changing.</para>
        /// </value>
        public string ConfigurationFile
        {
            get { return configurationFile; }
        }
    }
}