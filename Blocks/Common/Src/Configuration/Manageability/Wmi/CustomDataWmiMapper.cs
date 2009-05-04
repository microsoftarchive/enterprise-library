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
using System.Collections.Specialized;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability
{
	/// <summary>
	/// Custom WMI Mapper.
	/// </summary>
	public static class CustomDataWmiMapperHelper
	{
		/// <summary>
		/// Generates a string array filled with encoded kay/value pair.
		/// </summary>
        /// <param name="attributes"><see cref="NameValueCollection"/> with attributes values.</param>
		/// <returns><see cref="System.Array"/></returns>
		public static String[] GenerateAttributesArray(NameValueCollection attributes)
		{
			String[] attributesArray = new String[attributes.Count];
			int i = 0;
			foreach (String key in attributes.AllKeys)
			{
				attributesArray[i++] = KeyValuePairEncoder.EncodeKeyValuePair(key, attributes.Get(key));
			}
			return attributesArray;
		}

		/// <summary>
        /// Creates a <see cref="NameValueCollection"/> filled with provided attributes.
		/// </summary>
		/// <param name="attributes">Encoded key/value pair.</param>
        /// <returns><see cref="NameValueCollection"/></returns>
		public static NameValueCollection GenerateAttributesCollection(string[] attributes)
		{
			NameValueCollection attributeCollection = new NameValueCollection(attributes.Length);

			foreach (string attribute in attributes)
			{
				string key;
				string value;
				KeyValuePairParser.DecodeKeyValuePair(attribute, out key, out value);
				attributeCollection.Add(key, value);
			}
			return attributeCollection;
		}

		/// <summary>
		/// Update the attributes array with the new values.
		/// </summary>
		/// <param name="encodedAttributes">Encoded key/value pair.</param>
        /// <param name="attributes">A <see cref="NameValueCollection"/> with new attributes.</param>
		public static void UpdateAttributes(string[] encodedAttributes, NameValueCollection attributes)
		{
			attributes.Clear();
			NameValueCollection generatedAttributes = GenerateAttributesCollection(encodedAttributes);
			foreach (string key in generatedAttributes.Keys)
			{
				attributes.Add(key, generatedAttributes[key]);
			}
		}
	}
}
