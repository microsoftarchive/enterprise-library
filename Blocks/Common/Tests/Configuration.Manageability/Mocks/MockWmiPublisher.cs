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

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests.Mocks
{
	public class MockWmiPublisher : IWmiPublisher
	{
		private IDictionary<ConfigurationSetting, ConfigurationSetting> publishedInstances;

		public MockWmiPublisher()
		{
			publishedInstances = new Dictionary<ConfigurationSetting, ConfigurationSetting>();
		}

		public ICollection<ConfigurationSetting> GetPublishedInstances()
		{
			return publishedInstances.Keys;
		}

		public void Publish(ConfigurationSetting instance)
		{
			publishedInstances[instance] = instance;
		}

		public void Revoke(ConfigurationSetting instance)
		{
			publishedInstances.Remove(instance);
		}
	}
}
