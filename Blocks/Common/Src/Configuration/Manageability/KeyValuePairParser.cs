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
using System.Text;
using System.Text.RegularExpressions;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability
{
	/// <summary>
	/// Helper class to extract a key/value pair collection from an encoded string of semicolon
	/// separated key/value pairs.
	/// </summary>
	/// <seealso cref="KeyValuePairEncoder"/>
	public static class KeyValuePairParser
	{
		private static Regex KeyValueEntryRegex
			= new Regex(@"
						(?<name>[^;=]+)= 		# match the value name - anything but ; or =, followed by =
						(?<value>.*?)			# followed by anything as the value, but non greedy
						(						# until either
							$					# the string ends
								|				# or
							(?<!;);$			# the string ends after a non escaped ;
								|				# or
							(?<!;);(?!;)) 		# a ; that is not before or after another ; (;;) is the escaped ;"
						, RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace | RegexOptions.ExplicitCapture);

		/// <summary>
		/// Extracts the key/value pairs encoded in <paramref name="attributes"/>,
		/// adding them to <paramref name="attributesDictionary"/>.
		/// </summary>
		/// <param name="attributes">The string where the key/value pairs are encoded.</param>
		/// <param name="attributesDictionary">The dictionary where the extracted key/value pairs should be added.</param>
		public static void ExtractKeyValueEntries(String attributes, IDictionary<String, String> attributesDictionary)
		{
			foreach (Match match in KeyValueEntryRegex.Matches(attributes))
			{
				attributesDictionary.Add(match.Groups["name"].Value.Trim(), match.Groups["value"].Value.Replace(";;", ";"));
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="keyValue"></param>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public static void DecodeKeyValuePair(string keyValue, out string key, out string value)
		{
			key = string.Empty;
			value = string.Empty;

			if (keyValue != null)
			{
				Match match = KeyValueEntryRegex.Match(keyValue);
				if (match.Success)
				{
					key = match.Groups["name"].Value.Trim();
					value = match.Groups["value"].Value.Replace(";;", ";");
				}
				else
				{
					key = keyValue;
				}
			}
		}
	}
}
