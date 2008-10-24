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
using Microsoft.Practices.EnterpriseLibrary.SqlConfigurationSource.Storage;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.SqlConfigurationSource
{
    /// <summary>
    /// 
    /// </summary>
    public class ConfigurationSqlSourceWatcher : ConfigurationSourceWatcher
    {
        /// <summary>
        /// 
        /// </summary>
        private string connectString;
        private string getStoredProc;
    
        /// <summary>
        /// 
        /// </summary>
        public ConfigurationSqlSourceWatcher(string connectString, string getStoredProc, string configSource, bool refresh, ConfigurationChangedEventHandler changed)
            : base(configSource, refresh, changed)
        {

            this.connectString = connectString;
            this.getStoredProc = getStoredProc;

            if (refresh)
            {
                SetUpWatcher(changed);
            }

		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="changed"></param>
		private void SetUpWatcher(ConfigurationChangedEventHandler changed)
		{
            this.configWatcher = new ConfigurationChangeSqlWatcher(connectString, getStoredProc, this.ConfigSource);
			this.configWatcher.ConfigurationChanged += changed;
		}
        
	}
}
