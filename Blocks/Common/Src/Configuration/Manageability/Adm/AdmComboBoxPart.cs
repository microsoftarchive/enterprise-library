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
	/// Represents a COMBOBOX part on an ADM template.
	/// </summary>
	public class AdmComboBoxPart : AdmEditTextPart
	{
		internal const String ComboBoxTemplate = "\t\t\tCOMBOBOX";
		internal const String SuggestionsStartTemplate = "\t\t\tSUGGESTIONS";
		internal const String SuggestionsEndTemplate = "\t\t\tEND SUGGESTIONS";

		IEnumerable<String> suggestions;

		internal AdmComboBoxPart(String partName, String keyName, String valueName,
			String defaultValue, IEnumerable<String> suggestions, int maxlen, bool required)
			: base(partName, keyName, valueName, defaultValue, maxlen, required)
		{
			this.suggestions = suggestions;
		}

		/// <summary>
		/// Writes the text representing the part to the <paramref name="writer"/>.
		/// </summary>
		/// <param name="writer">The <see cref="System.IO.TextWriter"/> to where the text for the part should be written to.</param>
		protected override void WritePart(System.IO.TextWriter writer)
		{
			base.WritePart(writer);

			writer.Write(SuggestionsStartTemplate);
			foreach (String suggestion in this.suggestions)
			{
				writer.Write(String.Format(CultureInfo.InvariantCulture, " \"{0}\"", suggestion));
			}
			writer.Write(writer.NewLine);
			writer.WriteLine(SuggestionsEndTemplate);
		}

		/// <summary>
		/// Gets the list of suggested values for the part.
		/// </summary>
		public IEnumerable<String> Suggestions
		{
			get { return suggestions; }
		}

		/// <summary>
		/// Gest the template representing the type of the part.
		/// </summary>
		protected override string PartTypeTemplate
		{
			get
			{
				return ComboBoxTemplate;
			}
		}
	}
}
