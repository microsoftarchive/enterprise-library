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

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests.Mocks
{
	internal class MockManageabilityHelper : IManageabilityHelper
	{
		public bool updateCalled = false;
		public bool addHandlerCalled = false;
		public ICollection<String> notifiedSections = new List<String>();
		private IEnumerable<String> sectionNames;

		public MockManageabilityHelper()
			: this(new String[0])
		{ }

		public MockManageabilityHelper(params String[] sectionNames)
		{
			this.sectionNames = sectionNames;
		}

		public void UpdateConfigurationManageability(IConfigurationAccessor configurationAccessor)
		{
			updateCalled = true;
			foreach (String sectionName in sectionNames)
			{
				configurationAccessor.GetSection(sectionName);
				notifiedSections.Add(sectionName);
			}
		}

		public void UpdateConfigurationSectionManageability(IConfigurationAccessor configurationAccessor, string sectionName)
		{
			throw new NotImplementedException();
		}

		public event EventHandler<ConfigurationSettingChangedEventArgs> ConfigurationSettingChanged
		{
			add { this.addHandlerCalled = true; }
			remove { }
		}
	}
}
