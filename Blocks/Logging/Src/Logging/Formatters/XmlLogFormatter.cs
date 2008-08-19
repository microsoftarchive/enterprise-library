//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
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
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using System.Reflection;
using System.Collections;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Logging.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Formatters
{
	/// <summary>
	/// Formats a <see cref="LogEntry"/> and any subclass of it to an XML string representation.
	/// </summary>
	public class XmlLogFormatter : LogFormatter
	{
		private const string DefaultValue = "";

		/// <summary>
		/// Formats the <see cref="LogEntry"/> into an XML String representation.
		/// </summary>
		/// <param name="log">A LogEntry or any sub class of it</param>
		/// <returns></returns>
		public override string Format(LogEntry log)
		{
			StringBuilder result = new StringBuilder();
			Format(log, result);
			return result.ToString();
		}

		private void Format(object obj, StringBuilder result)
		{
			if (obj == null) return;
			result.Append(CreateOpenElement(CreateRootName(obj)));
			if (Type.GetTypeCode(obj.GetType()) == TypeCode.Object)
			{
				foreach (PropertyInfo propertyInfo in obj.GetType().GetProperties())
				{
					// skip write only properties and indexers
					if (!propertyInfo.CanRead || propertyInfo.GetIndexParameters().Length > 0)
						continue;

					result.Append(CreateOpenElement(propertyInfo.Name));
					if (typeof(IEnumerable).IsAssignableFrom(propertyInfo.PropertyType) && Type.GetTypeCode(propertyInfo.PropertyType) == TypeCode.Object)
					{
						IEnumerable values = (IEnumerable)propertyInfo.GetValue(obj, null);
						if (values != null)
						{
							foreach (object value in values)
							{
								Format(value, result);
							}
						}
					}
					else
					{
						result.Append(ConvertToString(propertyInfo, obj));
					}
					result.Append(CreateCloseElement(propertyInfo.Name));
				}
			}
			else
			{
				result.Append(obj.ToString());
			}
			result.Append(CreateCloseElement(CreateRootName(obj)));
		}

		private string CreateRootName(object obj)
		{
			string name = obj.GetType().Name;
			name = name.Replace('`', '_');
			name = name.Replace('[', '_');
			name = name.Replace(']', '_');
			name = name.Replace(',', '_');
			return name;
		}

		private string CreateOpenElement(string name)
		{
			return string.Format(Resources.Culture, "<{0}>", name);
		}
		private string CreateCloseElement(string name)
		{
			return string.Format(Resources.Culture, "</{0}>", name);
		}

		private string ConvertToString(PropertyInfo propertyInfo, object obj)
		{
			object value = propertyInfo.GetValue(obj, null);
			return value != null ? value.ToString() : DefaultValue;
		}
	}
}
