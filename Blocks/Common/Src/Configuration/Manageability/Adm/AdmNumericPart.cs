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
	/// Represents a NUMERIC part in an ADM template.
	/// </summary>
	public class AdmNumericPart : AdmPart
	{
		internal const String NumericTemplate = "\t\t\tNUMERIC";
		internal const String DefaultValueTemplate = "\t\t\tDEFAULT {0}";
		internal const String MinValueTemplate = "\t\t\tMIN {0}";
		internal const String MaxValueTemplate = "\t\t\tMAX {0}";

		private int? defaultValue;
		private int? maxValue;
		private int? minValue;

		internal AdmNumericPart(String partName, String keyName, String valueName,
			int? defaultValue, int? minValue, int? maxValue)
			: base(partName, keyName, valueName)
		{
			this.defaultValue = defaultValue;
			this.minValue = minValue;
			this.maxValue = maxValue;
		}

		/// <summary>
		/// Writes the text representing the part to the <paramref name="writer"/>.
		/// </summary>
		/// <param name="writer">The <see cref="System.IO.TextWriter"/> to where the text for the part should be written to.</param>
		protected override void WritePart(System.IO.TextWriter writer)
		{
			base.WritePart(writer);

			if (defaultValue != null)
			{
				writer.WriteLine(String.Format(CultureInfo.InvariantCulture, DefaultValueTemplate, this.defaultValue.Value));
			}
			if (minValue != null)
			{
				writer.WriteLine(String.Format(CultureInfo.InvariantCulture, MinValueTemplate, this.minValue.Value));
			}
			if (maxValue != null)
			{
				writer.WriteLine(String.Format(CultureInfo.InvariantCulture, MaxValueTemplate, this.maxValue.Value));
			}
		}

		/// <summary>
		/// Gets the default value for the part, or <see langword="null"/> 
		/// if no default value is available.
		/// </summary>
		public int? DefaultValue
		{
			get { return defaultValue; }
		}

		/// <summary>
		/// Gets the maximum value for the part, or <see langword="null"/> 
		/// if no maximum value is available.
		/// </summary>
		public int? MaxValue
		{
			get { return this.maxValue; }
		}

		/// <summary>
		/// Gets the minimum value for the part, or <see langword="null"/> 
		/// if no minimum value is available.
		/// </summary>
		public int? MinValue
		{
			get { return this.minValue; }
		}

		/// <summary>
		/// Gest the template representing the type of the part.
		/// </summary>
		protected override string PartTypeTemplate
		{
			get { return NumericTemplate; }
		}
	}
}
