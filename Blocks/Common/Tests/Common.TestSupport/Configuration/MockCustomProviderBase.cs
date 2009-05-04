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

using System.Collections.Specialized;

namespace Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration
{
	public abstract class MockCustomProviderBase
	{
		public const string AttributeKey = "attributeKey";

		public string customValue;

		public MockCustomProviderBase(NameValueCollection attributes)
		{
			this.customValue = attributes[AttributeKey];
		}	
	}
}
