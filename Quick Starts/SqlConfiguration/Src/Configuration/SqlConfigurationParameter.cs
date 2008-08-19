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
using System.Collections.Generic;
using System.Text;

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.SqlConfigurationSource.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public class SqlConfigurationParameter : IConfigurationParameter
    {
        private SqlConfigurationData data;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="getStoredProc"></param>
        /// <param name="saveStoredProc"></param>
        /// <param name="refreshStoredProc"></param>
        /// <param name="removeStoredProc"></param>
        public SqlConfigurationParameter(
                    string connectionString, 
                    string getStoredProc, 
                    string saveStoredProc, 
                    string refreshStoredProc, 
                    string removeStoredProc)
        {
            this.data = new SqlConfigurationData(connectionString, getStoredProc, saveStoredProc, refreshStoredProc, removeStoredProc);
        }

        /// <summary>
        /// 
        /// </summary>
        public string ConnectionString
        {
            get { return this.data.ConnectionString; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public string GetStoredProcedure
        {
            get { return this.data.GetStoredProcedure; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string SetStoredProcedure
        {
            get { return this.data.SetStoredProcedure; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string RefreshStoredProcedure
        {
            get { return this.data.RefreshStoredProcedure; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string RemoveStoredProcedure
        {
            get { return this.data.RemoveStoredProcedure; }
        }
    }
}