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

namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration.TestObjects
{
	public class PolymorphicConfigurationElementCollectionTestSection : ConfigurationSection
	{
		[ConfigurationProperty("withOverrides")]
		public TestNameTypeConfigurationElementCollectionWithOverridenAddAndClearNames<BasePolymorphicObjectData, CustomPolymorphicObjectData> WithOverrides
		{
            get { return (TestNameTypeConfigurationElementCollectionWithOverridenAddAndClearNames<BasePolymorphicObjectData, CustomPolymorphicObjectData>)this["withOverrides"]; }
			set { this["withOverrides"] = value; }
		}

		[ConfigurationProperty("withoutOverrides")]
        public NameTypeConfigurationElementCollection<BasePolymorphicObjectData, CustomPolymorphicObjectData> WithoutOverrides
		{
            get { return (NameTypeConfigurationElementCollection<BasePolymorphicObjectData, CustomPolymorphicObjectData>)this["withoutOverrides"]; }
			set { this["withoutOverrides"] = value; }
		}
	}
}
