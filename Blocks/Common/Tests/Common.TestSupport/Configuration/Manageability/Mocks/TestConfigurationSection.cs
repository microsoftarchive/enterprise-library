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
using System.Configuration;

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration.Manageability.Mocks
{
	public class TestsConfigurationSection : SerializableConfigurationSection
	{
		private const String valuePropertyName = "value";

		public TestsConfigurationSection()
			: base()
		{ }

		public TestsConfigurationSection(String value)
			: base()
		{
			this.Value = value;
		}

		public override string ToString()
		{
			return this.Value.ToString();
		}

		[ConfigurationProperty(valuePropertyName)]
		public String Value
		{
			get { return (String)base[valuePropertyName]; }
			set { base[valuePropertyName] = value; }
		}
	}
}
