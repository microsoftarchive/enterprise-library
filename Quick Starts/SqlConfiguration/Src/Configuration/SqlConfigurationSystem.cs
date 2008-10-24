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
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;
using System.Configuration.Internal;
using System.Collections.Generic;
using System.Xml;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.SqlConfigurationSource.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.SqlConfigurationSource.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public class SqlConfigurationSystem : IInternalConfigSystem
    {
        private SqlConfigurationData data = null;

        /// <summary>
        /// 
        /// </summary>
        public SqlConfigurationSystem()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public SqlConfigurationSystem(SqlConfigurationData data)
        {
            this.data = new SqlConfigurationData(data);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectString"></param>
        /// <param name="getStoredProcedure"></param>
        /// <param name="setStoredProcedure"></param>
        /// <param name="refreshStoredProcedure"></param>
        /// <param name="removeStoredProcedure"></param>
        public SqlConfigurationSystem(
                    string connectString, 
                    string getStoredProcedure, 
                    string setStoredProcedure, 
                    string refreshStoredProcedure,
                    string removeStoredProcedure)
        {
            this.data = new SqlConfigurationData(connectString, getStoredProcedure, setStoredProcedure, refreshStoredProcedure, removeStoredProcedure);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sectionName"></param>
        /// <returns></returns>
        public object GetSection(string sectionName)
        {
            string xmlData;
            string configSectionType;

            using (SqlConnection myConnection = new SqlConnection(data.ConnectionString))
            {
                try
                {
                    // Create Instance of Connection and Command Object
                    SqlCommand myCommand = new SqlCommand(data.GetStoredProcedure, myConnection);
                    myCommand.CommandType = CommandType.StoredProcedure;

                    SqlParameter parameterSectionName = new SqlParameter(@"@SectionName", SqlDbType.NVarChar);
                    parameterSectionName.Value = sectionName;
                    myCommand.Parameters.Add(parameterSectionName);

                    // Execute the command
                    myConnection.Open();
                    using (SqlDataReader sqlReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        if (!sqlReader.Read())
                        {
                            return null;
                        }
                        configSectionType = sqlReader.IsDBNull(0) ? null : sqlReader.GetString(0);
                        xmlData = sqlReader.IsDBNull(0) ? null : sqlReader.GetString(1);
                    }
                }
                catch (SqlException sqlException)
                {
                    throw new ConfigurationErrorsException(String.Format(Resources.ExceptionConfigurationSqlInvalidSection, sectionName), sqlException);
                }
            }

            if (xmlData == null || xmlData.Trim().Equals(String.Empty))
            {
                return null;               
            }

            //TODO:  If data is encrypted, decrypt it here

            SerializableConfigurationSection configSection = (SerializableConfigurationSection)Activator.CreateInstance(Type.GetType(configSectionType));

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.CloseInput = true;
            using (System.IO.StringReader stringReader = new System.IO.StringReader(xmlData))
            {
                using (XmlReader reader = XmlReader.Create(stringReader, settings))
                {
                    configSection.ReadXml(reader);
                    reader.Close();
                }
                stringReader.Close();
            }
            return configSection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sectionName"></param>
        public void RefreshConfig(string sectionName)
        {

            // Create Instance of Connection and Command Object
            using (SqlConnection myConnection = new SqlConnection(data.ConnectionString))
            {
                try
                {
                    SqlCommand myCommand = new SqlCommand(data.RefreshStoredProcedure, myConnection);
                    myCommand.CommandType = CommandType.StoredProcedure;

                    SqlParameter sectionNameParameter = new SqlParameter(@"@section_name", SqlDbType.NVarChar);
                    sectionNameParameter.Value = sectionName;
                    myCommand.Parameters.Add(sectionNameParameter);

                    // Execute the command
                    myConnection.Open();
                    myCommand.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    throw new ConfigurationErrorsException(Resources.ExceptionConfigurationCannotSet, e);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool SupportsUserConfig
        {
            get { return true; }
        }

        /// <summary>
        /// 
        /// </summary>
        public void SaveSection(string sectionName, SerializableConfigurationSection configurationSection)
        {
            //TODO: if encryption enabled, encrypt it here

            // Create Instance of Connection and Command Object
            using (SqlConnection myConnection = new SqlConnection(data.ConnectionString))
            {
                try
                {
                    SqlCommand myCommand = new SqlCommand(data.SetStoredProcedure, myConnection);
                    myCommand.CommandType = CommandType.StoredProcedure;

                    SqlParameter sectionNameParameter = new SqlParameter(@"@section_name", SqlDbType.NVarChar);
                    sectionNameParameter.Value = sectionName;
                    myCommand.Parameters.Add(sectionNameParameter);

                    SqlParameter sectionTypeParameter = new SqlParameter(@"@section_type", SqlDbType.NVarChar);
                    sectionTypeParameter.Value = configurationSection.GetType().AssemblyQualifiedName;
                    myCommand.Parameters.Add(sectionTypeParameter);

                    SqlParameter sectionValueParameter = new SqlParameter(@"@section_value", SqlDbType.NText);

                    StringBuilder output = new StringBuilder();
                    XmlWriterSettings settings = new XmlWriterSettings();
                    using (XmlWriter writer = XmlWriter.Create(output, settings))
                    {
                        configurationSection.WriteXml(writer);
                        writer.Close();
                        writer.Flush();
                    }

                    sectionValueParameter.Value = output.ToString();
                    myCommand.Parameters.Add(sectionValueParameter);

                    // Execute the command
                    myConnection.Open();
                    myCommand.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    throw new ConfigurationErrorsException(Resources.ExceptionConfigurationCannotSet, e);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void RemoveSection(string sectionName)
        {
            // Create Instance of Connection and Command Object
            using (SqlConnection myConnection = new SqlConnection(data.ConnectionString))
            {
                try
                {
                    SqlCommand myCommand = new SqlCommand(data.RemoveStoredProcedure, myConnection);
                    myCommand.CommandType = CommandType.StoredProcedure;

                    SqlParameter sectionNameParameter = new SqlParameter(@"@section_name", SqlDbType.NVarChar);
                    sectionNameParameter.Value = sectionName;
                    myCommand.Parameters.Add(sectionNameParameter);

                    // Execute the command
                    myConnection.Open();
                    myCommand.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    throw new ConfigurationErrorsException(Resources.ExceptionConfigurationCannotSet, e);
                }
            }
        }
    
    }
}
