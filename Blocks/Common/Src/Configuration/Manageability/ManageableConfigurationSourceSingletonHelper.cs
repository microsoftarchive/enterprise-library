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
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Properties;
using System.Globalization;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability
{
    /// <summary>
    /// Manages the singleton <see cref="ManageableConfigurationSourceImplementation"/> instance for a given 
    /// file name, application name and Group Policy enablement combination.
    /// </summary>
    public class ManageableConfigurationSourceSingletonHelper : IDisposable
    {
        private readonly IDictionary<ImplementationKey, ManageableConfigurationSourceImplementation> instances;
        private readonly object lockObject = new object();
        internal bool refresh;

        /// <summary>
        /// Initialzie a new instance of the <see cref="ManageableConfigurationSourceSingletonHelper"/> class.
        /// </summary>
        public ManageableConfigurationSourceSingletonHelper()
            : this(true)
        {
        }

        /// <summary>
        /// Initialize a new instace of the <see cref="ManageableConfigurationSourceSingletonHelper"/> class.
        /// </summary>
        /// <param name="refresh">
        /// true to support refreshing; otherwise, false.
        /// </param>
        public ManageableConfigurationSourceSingletonHelper(bool refresh)
        {
            this.refresh = refresh;
            instances
                = new Dictionary<ImplementationKey, ManageableConfigurationSourceImplementation>(new ImplementationKeyComparer());
        }

        ///<summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        ///</summary>
        public void Dispose()
        {
            foreach (ManageableConfigurationSourceImplementation instance in instances.Values)
            {
                instance.Dispose();
            }
        }

        /// <summary>
        /// Gets a <see cref="ManageableConfigurationSourceImplementation"/> for a configuration.
        /// </summary>
        /// <param name="configurationFilePath">The path to a configuration file.</param>
        /// <param name="manageabilityProviders">The list of managment providers.</param>
        /// <param name="readGroupPolicies">true to read Group Policies; otherwise, false.</param>
        /// <param name="applicationName">The name of the application.</param>
        /// <returns>A <see cref="ManageableConfigurationSourceImplementation"/> object.</returns>
        public ManageableConfigurationSourceImplementation GetInstance(String configurationFilePath,
                                                                       IDictionary<String, ConfigurationSectionManageabilityProvider> manageabilityProviders,
                                                                       bool readGroupPolicies,
                                                                       String applicationName)
        {
            if (String.IsNullOrEmpty(configurationFilePath))
                throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "configurationFilePath");

            String rootedConfigurationFilePath = RootConfigurationFilePath(configurationFilePath);

            if (!File.Exists(rootedConfigurationFilePath))
                throw new FileNotFoundException(
                    String.Format(CultureInfo.CurrentCulture, Resources.ExceptionConfigurationLoadFileNotFound, rootedConfigurationFilePath));

            ImplementationKey key = new ImplementationKey(rootedConfigurationFilePath, applicationName, readGroupPolicies);
            ManageableConfigurationSourceImplementation instance;

            lock (lockObject)
            {
                instances.TryGetValue(key, out instance);
                if (instance == null)
                {
                    instance = new ManageableConfigurationSourceImplementation(rootedConfigurationFilePath,
                                                                               refresh,
                                                                               manageabilityProviders,
                                                                               readGroupPolicies,
                                                                               applicationName);
                    instances.Add(key, instance);
                }
            }

            return instance;
        }

        private static String RootConfigurationFilePath(String configurationFilePath)
        {
            String rootedConfigurationFile = configurationFilePath;
            if (!Path.IsPathRooted(rootedConfigurationFile))
            {
                rootedConfigurationFile
                    = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, rootedConfigurationFile));
            }
            return rootedConfigurationFile;
        }
    }
}
