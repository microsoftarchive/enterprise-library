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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
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
        private static Dictionary<string, SqlConfigurationSourceImplementation> implementationByConnectionString = new Dictionary<string, SqlConfigurationSourceImplementation>(StringComparer.OrdinalIgnoreCase);
        private SqlConfigurationData data = null;
        private static object lockObject = new object();

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="connectString"></param>
        ///// <param name="getStoredProc"></param>
        ///// <param name="setStoredProc"></param>
        ///// <param name="refreshStoredProc"></param>
        ///// <param name="removeStoredProc"></param>
        //[TargetConstructor]
        //public SqlConfigurationSource(
        //                [Value("ConnectionString")]string connectString,
        //                [Value("GetStoredProcedure")]string getStoredProc,
        //                [Value("SetStoredProcedure")]string setStoredProc,
        //                [Value("RefreshStoredProcedure")]string refreshStoredProc,
        //                [Value("RemoveStoredProcedure")]string removeStoredProc
        //                )
        //{
        //    if (string.IsNullOrEmpty(connectString)) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "connectString");
        //    if (string.IsNullOrEmpty(getStoredProc)) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "getStoredProc");
        //    if (string.IsNullOrEmpty(setStoredProc)) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "setStoredProc");
        //    if (string.IsNullOrEmpty(refreshStoredProc)) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "refreshStoredProc");
        //    if (string.IsNullOrEmpty(removeStoredProc)) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "removeStoredProc");

        //    this.data = new SqlConfigurationData(connectString, getStoredProc, setStoredProc, refreshStoredProc, removeStoredProc);
        
        //    EnsureImplementation(data);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectString"></param>
        /// <param name="getStoredProc"></param>
        /// <param name="setStoredProc"></param>
        /// <param name="refreshStoredProc"></param>
        /// <param name="removeStoredProc"></param>
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

            EnsureImplementation(data);
        }

        /// <summary>
		/// Retrieves a specified configuration section for the current application's default configuration.
		/// </summary>
		/// <param name="sectionName">The section name to retrieve.</param>
		/// <returns>The configuration for the section.</returns>
		public ConfigurationSection GetSection(string sectionName)
		{
            return implementationByConnectionString[data.ConnectionString].GetSection(sectionName);
		}

		/// <summary>
		/// Adds a handler to be called when changes to section <code>sectionName</code> are detected.
		/// </summary>
		/// <param name="sectionName">The name of the section to watch for.</param>
		/// <param name="handler">The handler.</param>
		public void AddSectionChangeHandler(string sectionName, ConfigurationChangedEventHandler handler)
		{
            implementationByConnectionString[data.ConnectionString].AddSectionChangeHandler(sectionName, handler);
		}

		/// <summary>
		/// Remove a handler to be called when changes to section <code>sectionName</code> are detected.
		/// </summary>
		/// <param name="sectionName">The name of the section to watch for.</param>
		/// <param name="handler">The handler.</param>
		public void RemoveSectionChangeHandler(string sectionName, ConfigurationChangedEventHandler handler)
		{
            implementationByConnectionString[data.ConnectionString].RemoveSectionChangeHandler(sectionName, handler);
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
            
            implementationByConnectionString[connectionString].SaveSection(section, configurationSection);
        }
        
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="removeStoredProcedure"></param>
        /// <param name="section"></param>
        public void Remove(string connectionString, string removeStoredProcedure, string section)
        {
           implementationByConnectionString[connectionString].RemoveSection(section);
        }
        
        /// <summary>
		/// <exclude>For unit testing purposes.</exclude>
		/// </summary>
		public static void ResetImplementation(SqlConfigurationData data, bool refreshing)
		{
            SqlConfigurationSourceImplementation currentImplementation = null;
            implementationByConnectionString.TryGetValue(data.ConnectionString, out currentImplementation);
            implementationByConnectionString[data.ConnectionString] =
                new SqlConfigurationSourceImplementation(data.ConnectionString, data.GetStoredProcedure, data.SetStoredProcedure, data.RefreshStoredProcedure, data.RemoveStoredProcedure, refreshing);

            if (currentImplementation != null)
            {
                currentImplementation.Dispose();
            }
		}

		/// <summary>
		/// <exclude>For unit testing purposes.</exclude>
		/// </summary>
		public SqlConfigurationSourceImplementation Implementation
		{
            get { return implementationByConnectionString[data.ConnectionString]; }
		}

        /// <summary>
        /// <exclude>For unit testing purposes.</exclude>
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static SqlConfigurationSourceImplementation GetImplementation(SqlConfigurationData data)
        {
            EnsureImplementation(data);
            string connectString = data.ConnectionString;
            return (implementationByConnectionString.ContainsKey(connectString)) ? implementationByConnectionString[connectString] : null;
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

        private static void EnsureImplementation(SqlConfigurationData data)
        {
            if (!implementationByConnectionString.ContainsKey(data.ConnectionString))
            {
                lock (lockObject)
                {
                    SqlConfigurationSourceImplementation implementation = new SqlConfigurationSourceImplementation(data);
                    implementationByConnectionString.Add(data.ConnectionString, implementation);
                }
            }
        }
    }
}
