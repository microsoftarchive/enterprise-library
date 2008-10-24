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
using System.Configuration.Internal;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.SqlConfigurationSource.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.SqlConfigurationSource
{
    /// <summary>
    /// 
    /// </summary>
    public class SqlConfigurationManager
    {
        // Nested Types
        /// <summary>
        /// 
        /// </summary>
        public enum InitState
        {
            // Fields
            /// <summary>
            /// 
            /// </summary>
            Completed = 3,
            /// <summary>
            /// 
            /// </summary>
            NotStarted = 0,
            /// <summary>
            /// 
            /// </summary>
            Started = 1,
            /// <summary>
            /// 
            /// </summary>
            Usable = 2
        }
        
        private static SqlConfigurationSystem configSystem;
        private static object initLock;
        private static InitState initState;

        /// <summary>
        /// 
        /// </summary>
        static SqlConfigurationManager()
        {
            SqlConfigurationManager.initState = SqlConfigurationManager.InitState.NotStarted;
            SqlConfigurationManager.initLock = new object();
        }
 
        /// <summary>
        /// Retrieves a specified configuration section for the current application's configuration
        /// </summary>
        /// <param name="sectionName">The configuration section name.</param>
        /// <param name="data">The configuration data for this connection.</param>
        /// <returns>The specified System.Configuration.ConfigurationSection object, or null if the section does not exist.</returns>
        public static object GetSection(string sectionName, SqlConfigurationData data)
        {
            if (string.IsNullOrEmpty(sectionName))
            {
                return null;
            }
            SqlConfigurationManager.PrepareConfigSystem(data);
            return SqlConfigurationManager.configSystem.GetSection(sectionName);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sectionName"></param>
        /// <param name="section"></param>
        /// <param name="data"></param>
        public static void SaveSection(string sectionName, SerializableConfigurationSection section, SqlConfigurationData data)
        {
            if (!string.IsNullOrEmpty(sectionName))
            {
                SqlConfigurationManager.PrepareConfigSystem(data);
                SqlConfigurationManager.configSystem.SaveSection(sectionName, section);
            }
        }

        /// <summary>
        /// Refreshes the named section so the next time it is retrieved it will be re-read
        /// </summary>
        /// <param name="sectionName"> The configuration section name of the section to refresh.</param>
        /// <param name="data"></param>
        public static void RefreshSection(string sectionName, SqlConfigurationData data)
        {
            if (!string.IsNullOrEmpty(sectionName))
            {
                SqlConfigurationManager.PrepareConfigSystem(data);
                SqlConfigurationManager.configSystem.RefreshConfig(sectionName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sectionName"></param>
        /// <param name="data"></param>
        public static void RemoveSection(string sectionName, SqlConfigurationData data)
        {
            if (!string.IsNullOrEmpty(sectionName))
            {
                SqlConfigurationManager.PrepareConfigSystem(data);
                SqlConfigurationManager.configSystem.RemoveSection(sectionName);
            }
        }

        
        /// <summary>
        /// 
        /// </summary>
        public static InitState InitializationState
        {
            get
            {
                return SqlConfigurationManager.initState;
            }
        }
        
        internal static void CompleteConfigInit()
        {
            lock (SqlConfigurationManager.initLock)
            {
                SqlConfigurationManager.initState = SqlConfigurationManager.InitState.Completed;
            }
        }


        private static void EnsureConfigurationSystem(SqlConfigurationData data)
        {
            lock (SqlConfigurationManager.initLock)
            {
                if (SqlConfigurationManager.initState >= SqlConfigurationManager.InitState.Usable)
                {
                    return;
                }
                SqlConfigurationManager.initState = SqlConfigurationManager.InitState.Started;
                try
                {
                    try
                    {
                        SqlConfigurationManager.configSystem = new SqlConfigurationSystem(data);
                        SqlConfigurationManager.initState = SqlConfigurationManager.InitState.Usable;
                    }
                    catch (Exception exception1)
                    {
                        throw new ConfigurationErrorsException(Properties.Resources.ExceptionConfigurationInitialization, exception1);
                    }
                }
                catch
                {
                    SqlConfigurationManager.initState = SqlConfigurationManager.InitState.Completed;
                    throw;
                }
            }
        }

        private static void PrepareConfigSystem(SqlConfigurationData data)
        {
            if (SqlConfigurationManager.initState < SqlConfigurationManager.InitState.Usable)
            {
                SqlConfigurationManager.EnsureConfigurationSystem(data);
            }
        }

        //set public for tests!
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configSystem"></param>
        /// <param name="initComplete"></param>
        public static void SetConfigurationSystem(SqlConfigurationSystem configSystem, bool initComplete)
        {
            lock (SqlConfigurationManager.initLock)
            {
                if (SqlConfigurationManager.initState != SqlConfigurationManager.InitState.NotStarted)
                {
                    throw new InvalidOperationException(Properties.Resources.ExceptionConfigurationAlreadySet);
                }
                SqlConfigurationManager.configSystem = configSystem;
                if (initComplete)
                {
                    SqlConfigurationManager.initState = SqlConfigurationManager.InitState.Completed;
                }
                else
                {
                    SqlConfigurationManager.initState = SqlConfigurationManager.InitState.Usable;
                }
            }
        }
    }
}
