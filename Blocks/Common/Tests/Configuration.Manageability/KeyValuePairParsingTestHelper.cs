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

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests
{
	public static class KeyValuePairParsingTestHelper
	{
		public static void ExtractKeyValueEntries(String attributes, IDictionary<String, String> attributesDictionary)
		{
			KeyValuePairParser.ExtractKeyValueEntries(attributes, attributesDictionary);
		}

		public static String EncodeKeyValueEntry(String key, String value)
		{
			return KeyValuePairEncoder.EncodeKeyValuePair(key, value);
		}
	}
}
