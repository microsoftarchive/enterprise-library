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

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests.Mocks
{
	public class MockCustomProviderSetting : NamedConfigurationSetting
	{
		public String providerTypeName;
		public String[] attributes;

		public MockCustomProviderSetting(String name, String providerTypeName, String[] attributes)
			: base(name)
		{
			this.providerTypeName = providerTypeName;
			this.attributes = attributes;
		}
	}
}
