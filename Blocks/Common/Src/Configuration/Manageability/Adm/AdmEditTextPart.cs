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
using System.Globalization;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm
{
	/// <summary>
	/// Represents an EDITTEXT part on an ADM template.
	/// </summary>
	public class AdmEditTextPart : AdmPart
	{
		internal const String EditTextTemplate = "\t\t\tEDITTEXT";
		internal const String DefaultValueTemplate = "\t\t\tDEFAULT \"{0}\"";
		internal const String MaxLengthTemplate = "\t\t\tMAXLEN {0}";
		internal const String RequiredTemplate = "\t\t\tREQUIRED";

		private String defaultValue;
		private int maxlen;
		private bool required;

		/// <summary>
		/// Initializes a new instance of the <see cref="AdmEditTextPart"/> class.
		/// </summary>
		/// <param name="partName">The name for the part.</param>
		/// <param name="keyName">The registry key for part, or <see langword="null"/> 
		/// if no key name is required for the part.</param>
		/// <param name="valueName">The name for the registry value for the part.</param>
		/// <param name="defaultValue">The default value for the part, or <see langword="null"/> 
		/// if no default value is available.</param>
		/// <param name="maxlen">The maximum length for the part's value.</param>
		/// <param name="required">The indication of whether values for the part are required.</param>
		protected internal AdmEditTextPart(String partName, String keyName, String valueName,
			String defaultValue, int maxlen, bool required)
			: base(partName, keyName, valueName)
		{
			this.defaultValue = defaultValue;
			this.maxlen = maxlen;
			this.required = required;
		}

		/// <summary>
		/// Writes the text representing the part to the <paramref name="writer"/>.
		/// </summary>
		/// <param name="writer">The <see cref="System.IO.TextWriter"/> to where the text for the part should be written to.</param>
		protected override void WritePart(System.IO.TextWriter writer)
		{
			base.WritePart(writer);

			if (maxlen > 0)
				writer.WriteLine(String.Format(CultureInfo.InvariantCulture, MaxLengthTemplate, maxlen));

			if (required)
				writer.WriteLine(RequiredTemplate);

			if (defaultValue != null)
			{
				writer.WriteLine(String.Format(CultureInfo.InvariantCulture, DefaultValueTemplate, this.defaultValue));
			}
		}

		/// <summary>
		/// Gets the default value for the part, or <see langword="null"/> 
		/// if no default value is available.
		/// </summary>
		public String DefaultValue
		{
			get { return defaultValue; }
		}

		/// <summary>
		/// Gets the maximum length for the part's value.
		/// </summary>
		public int Maxlen
		{
			get { return this.maxlen; }
		}

		/// <summary>
		/// Gets the indication of whether values for the part are required.
		/// </summary>
		public bool Required
		{
			get { return this.required; }
		}

		/// <summary>
		/// Gest the template representing the type of the part.
		/// </summary>
		protected override String PartTypeTemplate
		{
			get { return EditTextTemplate; }
		}
	}
}
