//===============================================================================
// Microsoft patterns & practices Enterprise Library
// SQL Configuration Source QuickStart
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Configuration;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Storage;
using Microsoft.Practices.EnterpriseLibrary.SqlConfigurationSource.Properties;
using Microsoft.Practices.EnterpriseLibrary.SqlConfigurationSource.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.SqlConfigurationSource
{
    /// <summary>
    /// Represents a configuration source that is based on Microsoft SQL Server.
    /// </summary>
    [ConfigurationElementType(typeof(SqlConfigurationSourceElement))]
    public class SqlConfigurationSource : IConfigurationSource
    {
        private SqlConfigurationData data = null;
        private static object lockObject = new object();
        private object lockMe = new object();
        private ConfigurationSourceWatcher configSqlWatcher = null;
        private Dictionary<string, ConfigurationSourceWatcher> watchedConfigSourceMapping;
        private Dictionary<string, ConfigurationSourceWatcher> watchedSectionMapping;
        private EventHandlerList eventHandlers = new EventHandlerList();
        private bool refresh = true;
        
        /// <summary/>
        public const string NullConfigSource = "";


        /// <summary/>
        public SqlConfigurationSource(SqlConfigurationData sqlConfigurationData, bool refresh)
            :this(sqlConfigurationData.ConnectionString, sqlConfigurationData.GetStoredProcedure, sqlConfigurationData.SetStoredProcedure, sqlConfigurationData.RefreshStoredProcedure, sqlConfigurationData.RemoveStoredProcedure)
        {
            this.refresh = refresh;
        }

        /// <summary/>
        public SqlConfigurationSource(
                        string connectString,
                        string getStoredProc,
                        string setStoredProc,
                        string refreshStoredProc,
                        string removeStoredProc
                        )
        {
            if (string.IsNullOrEmpty(connectString)) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "connectString");
            if (string.IsNullOrEmpty(getStoredProc)) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "getStoredProc");
            if (string.IsNullOrEmpty(setStoredProc)) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "setStoredProc");
            if (string.IsNullOrEmpty(refreshStoredProc)) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "refreshStoredProc");
            if (string.IsNullOrEmpty(removeStoredProc)) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "removeStoredProc");

            this.data = new SqlConfigurationData(connectString, getStoredProc, setStoredProc, refreshStoredProc, removeStoredProc);


            this.watchedConfigSourceMapping = new Dictionary<string, ConfigurationSourceWatcher>();
            this.watchedSectionMapping = new Dictionary<string, ConfigurationSourceWatcher>();
        }

        /// <summary>
		/// Retrieves a specified configuration section for the current application's default configuration.
		/// </summary>
		/// <param name="sectionName">The section name to retrieve.</param>
		/// <returns>The configuration for the section.</returns>
		public ConfigurationSection GetSection(string sectionName)
		{
            object section = SqlConfigurationManager.GetSection(sectionName, data);
            ConfigurationSection configurationSection = section as ConfigurationSection;

            if (configurationSection != null)
            {
                lock (lockMe)
                {
                    if (!IsWatchingSection(sectionName))	// should only be true sporadically
                    {
                        SetWatcherForSection(sectionName, this.data.ConnectionString);
                    }
                }
            }

            return configurationSection;
		}

		/// <summary>
		/// Adds a handler to be called when changes to section <code>sectionName</code> are detected.
		/// </summary>
		/// <param name="sectionName">The name of the section to watch for.</param>
		/// <param name="handler">The handler.</param>
		public void AddSectionChangeHandler(string sectionName, ConfigurationChangedEventHandler handler)
		{
            eventHandlers.AddHandler(sectionName, handler);
		}

		/// <summary>
		/// Remove a handler to be called when changes to section <code>sectionName</code> are detected.
		/// </summary>
		/// <param name="sectionName">The name of the section to watch for.</param>
		/// <param name="handler">The handler.</param>
		public void RemoveSectionChangeHandler(string sectionName, ConfigurationChangedEventHandler handler)
		{
            eventHandlers.RemoveHandler(sectionName, handler);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="saveParameter"></param>
        /// <param name="sectionName"></param>
        /// <param name="configurationSection"></param>
        public void Add(IConfigurationParameter saveParameter, string sectionName, ConfigurationSection configurationSection)
        {
            if (null == saveParameter) 
                throw new ArgumentException(string.Format(Resources.Culture, Resources.ExceptionUnexpectedType, typeof(SqlConfigurationParameter).Name), "saveParameter");
            if (!(configurationSection is SerializableConfigurationSection))
                throw new ArgumentException(string.Format(Resources.Culture, Resources.ExceptionUnexpectedType, typeof(SerializableConfigurationSection).Name), "configurationSection");

            SqlConfigurationParameter parameter = saveParameter as SqlConfigurationParameter;
            SerializableConfigurationSection serializableSection =
                configurationSection as SerializableConfigurationSection;

            Save(parameter.ConnectionString, parameter.SetStoredProcedure, sectionName, serializableSection);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="removeParameter"></param>
        /// <param name="sectionName"></param>
        public void Remove(IConfigurationParameter removeParameter, string sectionName)
        {
            if (null == removeParameter)
                throw new ArgumentException(string.Format(Resources.Culture, Resources.ExceptionUnexpectedType, typeof(SqlConfigurationParameter).Name), "removeParameter");

            SqlConfigurationParameter parameter = removeParameter as SqlConfigurationParameter;
            Remove(parameter.ConnectionString, parameter.RemoveStoredProcedure, sectionName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="setStoredProcedure"></param>
        /// <param name="section"></param>
        /// <param name="configurationSection"></param>
        public void Save(string connectionString, string setStoredProcedure, string section, SerializableConfigurationSection configurationSection)
        {
            ValidateArgumentsAndConnectionInfo(connectionString, setStoredProcedure, section, configurationSection);
            
            //TODO:  need to modify this to use arguments passed in -- can I make modifications to just call SqlConfigurationManager.SaveSection(connectionString, setStoredProcedure, section, configurationSection);

            SaveSection(section, configurationSection);
        }

        /// <summary>
        /// Saves a section from SqlConfiguration, and starts watching for 
        /// its changes if not watching already.
        /// </summary>
        /// <param name="sectionName">The section name.</param>
        /// <param name="section">The section.</param>
        /// <returns></returns>
        public void SaveSection(string sectionName, SerializableConfigurationSection section)
        {
            if (section == null) throw new ArgumentNullException("section");
            if ((sectionName == null) || (sectionName.Trim().Equals(String.Empty)))
                throw new ArgumentNullException(sectionName);

            SqlConfigurationManager.SaveSection(sectionName, section, data);

            lock (lockMe)
            {
                if (!IsWatchingSection(sectionName))
                {
                    SetWatcherForSection(sectionName, section.SectionInformation.ConfigSource);
                }
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="removeStoredProcedure"></param>
        /// <param name="section"></param>
        public void Remove(string connectionString, string removeStoredProcedure, string section)
        {
            if (String.IsNullOrEmpty(section))
                throw new ArgumentNullException(section);

            SqlConfigurationManager.RemoveSection(section, data);

            lock (lockMe)
            {
                if (IsWatchingSection(section))
                {
                    ConfigSourceChanged(section);
                    ConfigurationSourceWatcher watcher = this.watchedSectionMapping[section];
                    UnlinkWatcherForSection(watcher, section);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sectionName"></param>
        public void RemoveSection(string sectionName)
        {
            if ((sectionName == null) || (sectionName.Trim().Equals(String.Empty)))
                throw new ArgumentNullException(sectionName);

            SqlConfigurationManager.RemoveSection(sectionName, data);

            lock (lockMe)
            {
                if (IsWatchingSection(sectionName))
                {
                    ConfigSourceChanged(sectionName);
                    ConfigurationSourceWatcher watcher = this.watchedSectionMapping[sectionName];
                    UnlinkWatcherForSection(watcher, sectionName);
                }
            }
        }
	    

        private static void ValidateArgumentsAndConnectionInfo(string connectString, string setStoredProcedure, string section, ConfigurationSection configurationSection)
        {
            if (string.IsNullOrEmpty(connectString)) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "connectString");
            if (string.IsNullOrEmpty(setStoredProcedure)) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "setStoredProcedure");
            if (string.IsNullOrEmpty(section)) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "section");
            if (null == configurationSection) throw new ArgumentNullException("configurationSection");
            if (!(configurationSection is SerializableConfigurationSection))
                throw new ArgumentException(Resources.ExceptionConfigurationSectionNotSerializable, "configurationSection");
            
            //TODO: Ensure connectionstring and setstoredprocedure are valid           
        }


        private void OnConfigurationChanged(object sender, ConfigurationChangedEventArgs args)
        {
            ConfigSourceChanged(args.SectionName);
        }

        private void OnExternalConfigurationChanged(object sender, ConfigurationChangedEventArgs args)
        {
            ExternalConfigSourceChanged(args.SectionName);
        }

        /// <summary>
        /// <exclude/>
        /// </summary>
        /// <param name="configSource">The name of the updated configuration source.</param>
        public void ExternalConfigSourceChanged(string configSource)
        {
            string[] sectionsToNotify;

            lock (lockMe)
            {
                ConfigurationSourceWatcher watcher = null;
                this.watchedConfigSourceMapping.TryGetValue(configSource, out watcher);
                sectionsToNotify = new string[watcher.WatchedSections.Count];
                watcher.WatchedSections.CopyTo(sectionsToNotify, 0);
            }

            foreach (string sectionName in sectionsToNotify)
            {
                SqlConfigurationManager.RefreshSection(sectionName, this.data);
            }

            NotifyUpdatedSections(sectionsToNotify);
        }

        /// <summary>
        /// <exclude/>
        /// </summary>
        /// <param name="configSource">The name of the updated configuration source.</param>
        /// <remarks>
        /// </remarks>
        public void ConfigSourceChanged(string configSource)
        {
            IDictionary<string, string> localSectionsToRefresh = new Dictionary<string, string>();
            IDictionary<string, string> externalSectionsToRefresh = new Dictionary<string, string>();

            IDictionary<string, string> sectionsWithChangedConfigSource;
            ICollection<string> sectionsToNotify = new List<string>();

            // get two separate lists with the sections of interest
            lock (lockMe)
            {
                if (configSqlWatcher != null)
                {
                    AddSectionsToUpdate(configSqlWatcher, localSectionsToRefresh);
                }
                foreach (ConfigurationSourceWatcher watcher in watchedConfigSourceMapping.Values)
                {
                    if (watcher != configSqlWatcher)
                    {
                        AddSectionsToUpdate(watcher, externalSectionsToRefresh);
                    }
                }
            }

            RefreshAndValidateSections(localSectionsToRefresh, externalSectionsToRefresh, out sectionsToNotify, out sectionsWithChangedConfigSource);

            UpdateWatchersForSections(sectionsWithChangedConfigSource);

            // notify changes (out of lock)
            NotifyUpdatedSections(sectionsToNotify);
        }


        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {

        }


        // must be called inside lock
        private void UnlinkWatcherForSection(ConfigurationSourceWatcher watcher, string sectionName)
        {
            this.watchedSectionMapping.Remove(sectionName);
            watcher.WatchedSections.Remove(sectionName);
            if (watcher.WatchedSections.Count == 0 && configSqlWatcher != watcher)
            {
                RemoveConfigSourceWatcher(watcher);
            }
        }

        // must be called inside lock
        private void RemoveConfigSourceWatcher(ConfigurationSourceWatcher watcher)
        {
            this.watchedConfigSourceMapping.Remove(watcher.ConfigSource);
            if (watcher != null && watcher is IDisposable)
            {
                (watcher as IDisposable).Dispose();
            }
        }

        private void AddSectionsToUpdate(ConfigurationSourceWatcher watcher, IDictionary<string, string> sectionsToUpdate)
        {
            foreach (string section in watcher.WatchedSections)
            {
                sectionsToUpdate.Add(section, watcher.ConfigSource);
            }
        }

        // must be called outside of lock
        private void RefreshAndValidateSections(IDictionary<string, string> localSectionsToRefresh, IDictionary<string, string> externalSectionsToRefresh, out ICollection<string> sectionsToNotify, out IDictionary<string, string> sectionsWithChangedConfigSource)
        {
            sectionsToNotify = new List<string>();
            sectionsWithChangedConfigSource = new Dictionary<string, string>();

            // refresh local sections and determine what to do.
            foreach (KeyValuePair<string, string> sectionMapping in localSectionsToRefresh)
            {
                SqlConfigurationManager.RefreshSection(sectionMapping.Key, this.data);
                ConfigurationSection section = SqlConfigurationManager.GetSection(sectionMapping.Key, this.data) as ConfigurationSection;
                string refreshedConfigSource = section != null ? section.SectionInformation.ConfigSource : NullConfigSource;
                if (!sectionMapping.Value.Equals(refreshedConfigSource))
                {
                    sectionsWithChangedConfigSource.Add(sectionMapping.Key, refreshedConfigSource);
                }

                // notify anyway, since it might have been updated.
                sectionsToNotify.Add(sectionMapping.Key);
            }

            // refresh external sections and determine what to do.
            foreach (KeyValuePair<string, string> sectionMapping in externalSectionsToRefresh)
            {
                SqlConfigurationManager.RefreshSection(sectionMapping.Key, this.data);
                ConfigurationSection section = SqlConfigurationManager.GetSection(sectionMapping.Key, this.data) as ConfigurationSection;
                string refreshedConfigSource = (section != null) ? section.SectionInformation.ConfigSource : NullConfigSource;
                if (!sectionMapping.Value.Equals(refreshedConfigSource))
                {
                    sectionsWithChangedConfigSource.Add(sectionMapping.Key, refreshedConfigSource);

                    // notify only if che config source changed
                    sectionsToNotify.Add(sectionMapping.Key);
                }
            }
        }


        // must be called outside lock
        private bool IsWatchingSection(string sectionName)
        {
            return this.watchedSectionMapping.ContainsKey(sectionName);
        }

        // must be called outside lock
        private void UpdateWatchersForSections(IDictionary<string, string> sectionsChangingSource)
        {
            lock (lockMe)
            {
                foreach (KeyValuePair<string, string> sectionSourcePair in sectionsChangingSource)
                {
                    UpdateWatcherForSection(sectionSourcePair.Key, sectionSourcePair.Value);
                }
            }
        }

        // must be called outside lock
        private void UpdateWatcherForSection(string sectionName, string configSource)
        {
            ConfigurationSourceWatcher currentSectionWatcher = null;
            this.watchedSectionMapping.TryGetValue(sectionName, out currentSectionWatcher);

            if (currentSectionWatcher == null || currentSectionWatcher.ConfigSource != configSource)
            {
                if (currentSectionWatcher != null)
                {
                    UnlinkWatcherForSection(currentSectionWatcher, sectionName);
                }

                if (configSource != null)
                {
                    SetWatcherForSection(sectionName, configSource);
                }
            }
        }


        // must be called inside lock
        private void SetWatcherForSection(string sectionName, string configSource)
        {
            ConfigurationSourceWatcher currentConfigSourceWatcher = null;
            this.watchedConfigSourceMapping.TryGetValue(configSource, out currentConfigSourceWatcher);

            if (currentConfigSourceWatcher == null)
            {
                currentConfigSourceWatcher = CreateWatcherForConfigSource(configSource, sectionName);
            }
            else
            {
                currentConfigSourceWatcher.StopWatching();
            }
            LinkWatcherForSection(currentConfigSourceWatcher, sectionName);
            currentConfigSourceWatcher.StartWatching();
        }


        // must be called inside lock
        private ConfigurationSourceWatcher CreateWatcherForConfigSource(string configSource, string sectionName)
        {
            ConfigurationSourceWatcher watcher = null;

            if (string.Empty == configSource)
            {
                watcher = new ConfigurationSqlSourceWatcher(this.data.ConnectionString, this.data.GetStoredProcedure,
                        sectionName,
                    this.refresh,
                    new ConfigurationChangedEventHandler(OnConfigurationChanged));
                configSqlWatcher = watcher;
            }
            else
            {
                watcher = new ConfigurationSqlSourceWatcher(this.data.ConnectionString, this.data.GetStoredProcedure,
                    sectionName,
                    this.refresh && !NullConfigSource.Equals(configSource),
                    new ConfigurationChangedEventHandler(OnExternalConfigurationChanged));
            }

            this.watchedConfigSourceMapping.Add(configSource, watcher);

            return watcher;
        }

        // must be called outside lock
        private void NotifyUpdatedSections(IEnumerable<string> sectionsToNotify)
        {
            EventHandler<ConfigurationSourceChangedEventArgs> handler = SourceChanged;
            if (handler != null) handler(this, new ConfigurationSourceChangedEventArgs(this, sectionsToNotify));

            foreach (string sectionName in sectionsToNotify)
            {
                ConfigurationChangedEventHandler callbacks = (ConfigurationChangedEventHandler)eventHandlers[sectionName];
                if (callbacks == null)
                {
                    continue;
                }

                ConfigurationSqlChangedEventArgs eventData = new ConfigurationSqlChangedEventArgs(this.data.ConnectionString, this.data.GetStoredProcedure, sectionName);
                try
                {
                    foreach (ConfigurationChangedEventHandler callback in callbacks.GetInvocationList())
                    {
                        if (callback != null)
                        {
                            callback(this, eventData);
                        }
                    }
                }
                catch// (Exception e)
                {
                    //EventLog.WriteEntry(GetEventSourceName(), Resources.ExceptionEventRaisingFailed + GetType().FullName + " :" + e.Message);
                }
            }
        }

        // must be called inside lock
        private void LinkWatcherForSection(ConfigurationSourceWatcher watcher, string sectionName)
        {
            this.watchedSectionMapping.Add(sectionName, watcher);
            watcher.WatchedSections.Add(sectionName);
        }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<ConfigurationSourceChangedEventArgs> SourceChanged;

    }
}
