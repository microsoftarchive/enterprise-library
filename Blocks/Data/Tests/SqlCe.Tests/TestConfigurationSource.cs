//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
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
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using System.Configuration;

namespace Data.SqlCe.Tests.VSTS
{
	public class TestConfigurationSource
	{
		public static DictionaryConfigurationSource CreateConfigurationSource()
		{
			DictionaryConfigurationSource source = new DictionaryConfigurationSource();

			DatabaseSettings settings = new DatabaseSettings();
			settings.DefaultDatabase = "SqlCeTestConnection";

			ConnectionStringsSection section = new ConnectionStringsSection();
            section.ConnectionStrings.Add(new ConnectionStringSettings("SqlCeTestConnection", "Data Source='testdb.sdf'", "System.Data.SqlServerCe.4.0"));

			source.Add(DatabaseSettings.SectionName, settings);
			source.Add("connectionStrings", section);

			return source;
		}
	}
}
