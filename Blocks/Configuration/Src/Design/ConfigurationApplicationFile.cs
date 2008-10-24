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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// Represents the file for a application's configuration.
    /// </summary>
    [Serializable]
    public sealed class ConfigurationApplicationFile
    {
        private string baseDirectory;
        private string configurationFilePath;

        /// <summary>
        /// Initialize a new instance of the <see cref="ConfigurationApplicationFile"/> class.
        /// </summary>
        public ConfigurationApplicationFile() : this(string.Empty, string.Empty)
        {            
        }

		/// <summary>
		/// Initialize a new instance of the <see cref="ConfigurationApplicationFile"/> class with a base directory and file path.
		/// </summary>
        /// <param name="baseDirectory">The base directory of the application.</param>
        /// <param name="configurationFilePath">The configuration file.</param>
        public ConfigurationApplicationFile(string baseDirectory, string configurationFilePath)
        {            
            this.baseDirectory = baseDirectory;
            this.configurationFilePath = configurationFilePath;
        }

        /// <summary>
        /// Gets or sets the base directory of the application.
        /// </summary>
		/// <value>
		/// The base directory of the application.
		/// </value>
        public string BaseDirectory
        {
            get { return baseDirectory; }
            set { baseDirectory = value; }
        }

        /// <summary>
        /// Gets or sets the configuration file.
        /// </summary>
		/// <value>
		/// The configuration file.
		/// </value>
        public string ConfigurationFilePath
        {
            get { return configurationFilePath; }
            set { configurationFilePath = value; }
        }

        /// <summary>
        /// Creates a <see cref="ConfigurationApplicationFile"/> from the current <see cref="AppDomain"/>.
        /// </summary>
		/// <returns>A <see cref="ConfigurationApplicationFile"/> from the current <see cref="AppDomain"/>.</returns>
        public static ConfigurationApplicationFile FromCurrentAppDomain()
        {
            ConfigurationApplicationFile appData = new ConfigurationApplicationFile();
            AppDomain current = AppDomain.CurrentDomain;            
            appData.BaseDirectory = current.BaseDirectory;
            appData.ConfigurationFilePath = current.SetupInformation.ConfigurationFile;
            return appData;
        }

    }

}
