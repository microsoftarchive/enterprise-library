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

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm
{
	/// <summary>
	/// Represents a CHECKBOX part on an ADM template.
	/// </summary>
	public class AdmCheckboxPart : AdmPart
	{
		internal const String CheckBoxTemplate = "\t\t\tCHECKBOX";
		internal const String CheckBoxCheckedTemplate = "\t\t\tCHECKBOX DEFCHECKED";
		internal const String DefaultCheckBoxOnTemplate = "\t\t\tVALUEON NUMERIC 1";
		internal const String DefaultCheckBoxOffTemplate = "\t\t\tVALUEOFF NUMERIC 0";

		private bool checkedByDefault;
		private bool valueForOn;
		private bool valueForOff;

		internal AdmCheckboxPart(String partName, String keyName, String valueName,
			bool checkedByDefault, bool valueForOn, bool valueForOff)
			: base(partName, keyName, valueName)
		{
			this.checkedByDefault = checkedByDefault;
			this.valueForOn = valueForOn;
			this.valueForOff = valueForOff;
		}

		/// <summary>
		/// Writes the text representing the part to the <paramref name="writer"/>.
		/// </summary>
		/// <param name="writer">The <see cref="System.IO.TextWriter"/> to where the text for the part should be written to.</param>
		protected override void WritePart(System.IO.TextWriter writer)
		{
			base.WritePart(writer);

			if (valueForOn)
			{
				writer.WriteLine(DefaultCheckBoxOnTemplate);
			}
			if (valueForOff)
			{
				writer.WriteLine(DefaultCheckBoxOffTemplate);
			}
		}

		/// <summary>
		/// Gets the indication of whether the checkbox should be checked by default.
		/// </summary>
		public bool CheckedByDefault
		{
			get { return checkedByDefault; }
		}

		/// <summary>
		/// Gets the indication of whether a value for the checked state should be added.
		/// </summary>
		public bool ValueForOn
		{
			get { return valueForOn; }
		}

		/// <summary>
		/// Gets the indication of whether a value for the unchecked state should be added.
		/// </summary>
		public bool ValueForOff
		{
			get { return valueForOff; }
		}

		/// <summary>
		/// Gest the template representing the type of the part.
		/// </summary>
		protected override string PartTypeTemplate
		{
			get
			{
				return checkedByDefault ? CheckBoxCheckedTemplate : CheckBoxTemplate;
			}
		}
	}
}
