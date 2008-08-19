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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.SqlConfigurationSource.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.SqlConfigurationSource.Configuration;


namespace Microsoft.Practices.EnterpriseLibrary.SqlConfigurationSource.Design
{
    /// <summary>
    /// 
    /// </summary>
    public class SqlConfigurationSourceDesignManager : ConfigurationDesignManager 
    {
		/// <summary>
		/// 
		/// </summary>
        public SqlConfigurationSourceDesignManager()
        {
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="serviceProvider"></param>
        public override void Register(IServiceProvider serviceProvider)
        {
			SqlConfigurationSourceCommandRegistrar commandRegistrar = new SqlConfigurationSourceCommandRegistrar(serviceProvider);
			commandRegistrar.Register();

			SqlConfigurationSourceNodeMapRegistrar nodeMapRegistrar = new SqlConfigurationSourceNodeMapRegistrar(serviceProvider);
			nodeMapRegistrar.Register();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="rootNode"></param>
        /// <param name="section"></param>
        protected override void OpenCore(IServiceProvider serviceProvider, ConfigurationApplicationNode rootNode, ConfigurationSection section)
        {
        }
    }
}