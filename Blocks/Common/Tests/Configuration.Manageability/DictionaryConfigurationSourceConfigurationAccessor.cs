//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Configuration;

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests
{
	public class DictionaryConfigurationSourceConfigurationAccessor : IConfigurationAccessor
	{
		private DictionaryConfigurationSource configurationSource;

		public DictionaryConfigurationSourceConfigurationAccessor(DictionaryConfigurationSource configurationSource)
		{
			this.configurationSource = configurationSource;
		}

		public ConfigurationSection GetSection(String sectionName)
		{
			return configurationSource.GetSection(sectionName);
		}

		public void RemoveSection(String sectionName)
		{
			configurationSource.Remove(sectionName);
		}

		public IEnumerable<String> GetRequestedSectionNames()
		{
			throw new Exception("The method or operation is not implemented.");
		}
	}
}
