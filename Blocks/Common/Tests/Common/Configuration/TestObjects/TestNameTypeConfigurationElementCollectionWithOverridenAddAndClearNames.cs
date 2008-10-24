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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration.TestObjects
{
	public class TestNameTypeConfigurationElementCollectionWithOverridenAddAndClearNames<T, TCustomElementData>
        : NameTypeConfigurationElementCollection<T, TCustomElementData>
		where T : NameTypeConfigurationElement, new()
        where TCustomElementData : T, new()
	{
		public TestNameTypeConfigurationElementCollectionWithOverridenAddAndClearNames()
		{
			this.AddElementName = "overridenAdd";
			this.ClearElementName = "overridenClear";
		}
	}
}
