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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Storage;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability
{
    /// <summary>
    /// Manages the configuration file watchers for a collection of configuration sections.
    /// </summary>
    public class ConfigurationChangeWatcherCoordinator : IDisposable
    {
        /// <summary>
        /// The main configuration file source.
        /// </summary>
        public const String MainConfigurationFileSource = "";

        readonly Dictionary<String, ConfigurationChangeFileWatcher> configSourceWatcherMapping;

        readonly String mainConfigurationFileName;
        readonly String mainConfigurationFilePath;
        readonly bool refresh;

        /// <summary>
        /// Initialize a new instance of the <see cref="ConfigurationChangeNotificationCoordinator"/> class.
        /// </summary>
        /// <param name="mainConfigurationFileName">The main configuration file.</param>
        /// <param name="refresh">true to refresh configuration; otherwise, false.</param>
        public ConfigurationChangeWatcherCoordinator(String mainConfigurationFileName,
                                                     bool refresh)
        {
            this.mainConfigurationFileName = mainConfigurationFileName;
            mainConfigurationFilePath = Path.GetDirectoryName(mainConfigurationFileName);
            this.refresh = refresh;

            configSourceWatcherMapping = new Dictionary<String, ConfigurationChangeFileWatcher>();

            CreateWatcherForConfigSource(MainConfigurationFileSource);
        }

        ///// <summary>
        ///// Gets the mapings of sections to configuration watchers. 
        ///// </summary>
        ///// <value>
        ///// The mapings of sections to configuration watchers.
        ///// </value>
        //public IDictionary<String, ConfigurationChangeFileWatcher> ConfigSourceWatcherMapping
        //{
        //    get { return configSourceWatcherMapping; }
        //}

        /// <summary>
        /// Gets a collection of watch configuration sources.
        /// </summary>
        /// <value>
        /// A collection of watch configuration sources.
        /// </value>
        public ICollection<String> WatchedConfigSources
        {
            get { return configSourceWatcherMapping.Keys; }
        }

        /// <summary>
        /// Event to notify when configuration changes.
        /// </summary>
        public event ConfigurationChangedEventHandler ConfigurationChanged;

        void CreateWatcherForConfigSource(String configSource)
        {
            ConfigurationChangeFileWatcher watcher;

            if (MainConfigurationFileSource.Equals(configSource))
            {
                watcher = new ConfigurationChangeFileWatcher(mainConfigurationFileName,
                                                             configSource);
            }
            else
            {
                watcher = new ConfigurationChangeFileWatcher(Path.Combine(mainConfigurationFilePath, configSource),
                                                             configSource);
            }
            watcher.ConfigurationChanged += OnConfigurationChanged;

            configSourceWatcherMapping.Add(configSource, watcher);

            if (refresh)
            {
                watcher.StartWatching();
            }

            return;
        }

        ///<summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        ///</summary>
        public void Dispose()
        {
            foreach (IDisposable watcher in configSourceWatcherMapping.Values)
            {
                watcher.Dispose();
            }
        }

        ///<summary>
        /// Determines if the configuration source is being watched.
        ///</summary>
        ///<param name="configSource">
        /// The configuration source.
        /// </param>
        ///<returns>
        /// true if the source is being watched; otherwise, false.
        /// </returns>
        public bool IsWatchingConfigSource(String configSource)
        {
            return configSourceWatcherMapping.ContainsKey(configSource);
        }

        /// <summary>
        /// Raises the <see cref="ConfigurationChanged"/> event.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="args">The event arguments.</param>
        public void OnConfigurationChanged(object sender,
                                           ConfigurationChangedEventArgs args)
        {
            if (ConfigurationChanged != null)
            {
                ConfigurationChanged(this, args);
            }
        }

        /// <summary>
        /// Removes a watcher for the configuration source.
        /// </summary>
        /// <param name="configSource">
        /// The source to remove the watcher.
        /// </param>
        public void RemoveWatcherForConfigSource(String configSource)
        {
            ConfigurationChangeFileWatcher watcher;
            configSourceWatcherMapping.TryGetValue(configSource, out watcher);

            if (watcher != null)
            {
                configSourceWatcherMapping.Remove(configSource);
                watcher.Dispose();
            }
        }

        /// <summary>
        /// Sets a watcher for a configuration source.
        /// </summary>
        /// <param name="configSource">
        /// The configuration source to watch.
        /// </param>
        public void SetWatcherForConfigSource(String configSource)
        {
            if (!IsWatchingConfigSource(configSource))
            {
                CreateWatcherForConfigSource(configSource);
            }
        }
    }
}
