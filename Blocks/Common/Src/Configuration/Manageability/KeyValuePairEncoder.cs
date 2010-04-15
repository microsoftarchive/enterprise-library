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
using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability
{
	/// <summary>
	/// Represents the logic to encode key/value pairs into a string of semicolon separated entries.
	/// </summary>
	public class KeyValuePairEncoder
	{
		private StringBuilder builder;

		/// <summary>
		/// Initializes a new instance of the <see cref="KeyValuePairEncoder"/> class.
		/// </summary>
		public KeyValuePairEncoder()
		{
			this.builder = new StringBuilder();
		}

		/// <summary>
		/// Adds a key/value pair to the encoded string being built.
		/// </summary>
		/// <param name="key">The key of the pair.</param>
		/// <param name="value">The value of the pair.</param>
		public void AppendKeyValuePair(String key, String value)
        {
            if (value == null) throw new ArgumentNullException("value");

			builder.Append(EncodeKeyValuePair(key, value, true));
			builder.Append(';');
		}

		/// <summary>
		/// Gets the encoded key/value pairs string built.
		/// </summary>
		/// <returns></returns>
		public String GetEncodedKeyValuePairs()
		{
			return builder.ToString();
		}

		/// <summary>
		/// Returns a string representing a single encoded key/value pair.
		/// </summary>
		/// <param name="key">The key of the pair.</param>
		/// <param name="value">The value of the pair.</param>
		/// <returns>The encoded key/value pair.</returns>
		public static String EncodeKeyValuePair(String key, String value)
        {
            if (value == null) throw new ArgumentNullException("value");

			return EncodeKeyValuePair(key, value, false);
		}

		/// <summary>
		/// Returns a string representing a single encoded key/value pair with semicolons escaped if
		/// appropriate.
		/// </summary>
		/// <param name="key">The key of the pair.</param>
		/// <param name="value">The value of the pair.</param>
		/// <param name="escapeSemicolons"><see langword="true"/> if semicolons should be escaped;
		/// otherwise <see langword="false"/>.</param>
		/// <returns>The encoded key/value pair.</returns>
		public static String EncodeKeyValuePair(String key, String value, bool escapeSemicolons)
		{
            if (value == null) throw new ArgumentNullException("value");

			return key
				+ "="
				+ (escapeSemicolons ? value.Replace(";", ";;") : value);
		}
	}
}
