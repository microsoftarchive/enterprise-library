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
using System.IO;
using System.Text;
using System.Globalization;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm
{
	/// <summary>
	/// Represents a policy in an ADM template.
	/// </summary>
	public class AdmPolicy
	{
		internal const String PolicyStartTemplate = "\tPOLICY \"{0}\"";
		internal const String KeyNameTemplate = "\t\t\tKEYNAME \"Software\\Policies\\{0}\"";
		internal const String ValueNameTemplate = "\t\t\tVALUENAME \"{0}\" VALUEON NUMERIC 1 VALUEOFF NUMERIC 0";
		internal const String PolicyEndTemplate = "\tEND POLICY";

		private List<AdmPart> parts;
		private String keyName;
		private String name;
		private String valueName;

		internal AdmPolicy(String name, String keyName, String valueName)
		{
			this.keyName = keyName;
			this.name = name;
			this.valueName = valueName;

			this.parts = new List<AdmPart>();
		}

		internal void AddPart(AdmPart part)
		{
			this.parts.Add(part);
		}

		internal void Write(TextWriter writer)
		{
			writer.WriteLine(String.Format(CultureInfo.InvariantCulture, PolicyStartTemplate, name));
			writer.WriteLine(String.Format(CultureInfo.InvariantCulture, KeyNameTemplate, keyName));
			if (valueName != null)
			{
				writer.WriteLine(String.Format(CultureInfo.InvariantCulture, ValueNameTemplate, valueName));
			}
			foreach (AdmPart part in parts)
			{
				part.Write(writer);
			}
			writer.WriteLine(PolicyEndTemplate);
		}

		/// <summary>
		/// Gets the registry key for the policy.
		/// </summary>
		public String KeyName
		{
			get { return keyName; }
		}

		/// <summary>
		/// Gets the parts for the policy.
		/// </summary>
		public IEnumerable<AdmPart> Parts
		{
			get { return parts; }
		}

		/// <summary>
		/// Gets the name for the policy.
		/// </summary>
		public String Name
		{
			get { return name; }
		}

		/// <summary>
		/// Gets the registry value name for the policy, or <see langword="null"/> if no value name is required for the policy.
		/// </summary>
		public String ValueName
		{
			get { return valueName; }
		}
	}
}
