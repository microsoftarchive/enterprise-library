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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.SqlConfigurationSource;

namespace Microsoft.Practices.EnterpriseLibrary.SqlConfigurationSource.Design
{
    /// <summary>
    /// 
    /// </summary>
    public class SqlConfigurationSourceNodeMapRegistrar : NodeMapRegistrar
    {
		/// <summary>
		/// 
		/// </summary>
		/// <param name="serviceProvider"></param>
        public SqlConfigurationSourceNodeMapRegistrar(IServiceProvider serviceProvider)
            : base(serviceProvider)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		public override void Register()
		{
			AddMultipleNodeMap(Resources.SqlConfigurationSourceUICommandText,
				typeof(SqlConfigurationSourceElementNode),
				typeof(SqlConfigurationSourceElement));
		}
	}
}
