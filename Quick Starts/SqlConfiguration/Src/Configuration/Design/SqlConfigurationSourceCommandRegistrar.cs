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
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.SqlConfigurationSource.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.SqlConfigurationSource.Design
{
    /// <summary>
    /// 
    /// </summary>
    public class SqlConfigurationSourceCommandRegistrar : CommandRegistrar 
    {
		/// <summary>
		/// 
		/// </summary>
		/// <param name="serviceProvider"></param>
        public SqlConfigurationSourceCommandRegistrar(IServiceProvider serviceProvider)
            : base(serviceProvider)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		public override void Register()
		{
			AddSqlConfigurationSourceCommand();
			AddDefaultCommands(typeof(SqlConfigurationSourceElementNode));
		}


		private void AddSqlConfigurationSourceCommand()
		{
			AddMultipleChildNodeCommand(Resources.SqlConfigurationSourceUICommandText,
				Resources.SqlConfigurationSourceUICommandLongText, typeof(SqlConfigurationSourceElementNode),
				typeof(ConfigurationSourceSectionNode));
		}

	}
}
