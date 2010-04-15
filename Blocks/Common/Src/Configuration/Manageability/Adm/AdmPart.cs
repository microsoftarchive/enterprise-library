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
using System.IO;
using System.Globalization;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm
{
	/// <summary>
	/// Represents a part in an ADM template.
	/// </summary>
	public abstract class AdmPart
	{
        /// <summary>
        /// The part start template.
        /// </summary>
		public const String PartStartTemplate = "\t\tPART \"{0}\"";
		
        /// <summary>
        /// The part end template.
        /// </summary>
        public const String PartEndTemplate = "\t\tEND PART";
		
        /// <summary>
        /// The part value name.
        /// </summary>
        public const String ValueNameTemplate = "\t\t\tVALUENAME \"{0}\"";
		
        /// <summary>
        /// The part key name.
        /// </summary>
        public const String KeyNameTemplate = "\t\t\tKEYNAME \"Software\\Policies\\{0}\"";

		private String keyName;
		private String partName;
		private String valueName;

		/// <summary>
		/// Initializes a new instance of the <see cref="AdmPart"/> class.
		/// </summary>
		/// <param name="partName">The name for the part.</param>
		/// <param name="keyName">The registry key for part, or <see langword="null"/> 
		/// if no key name is required for the part.</param>
		/// <param name="valueName">The name for the registry value for the part.</param>
		protected AdmPart(String partName, String keyName, String valueName)
		{
			this.keyName = keyName;
			this.partName = partName;
			this.valueName = valueName;
		}

        /// <summary>
        /// Writes the part the <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">
        /// The <see cref="TextWriter"/> to write.
        /// </param>
		public void Write(TextWriter writer)
		{
            if (writer == null) throw new ArgumentNullException("writer");

			writer.WriteLine(String.Format(CultureInfo.InvariantCulture, PartStartTemplate, partName));

			WritePart(writer);

			writer.WriteLine(PartEndTemplate);
		}

		/// <summary>
		/// Writes the text representing the part to the <paramref name="writer"/>.
		/// </summary>
		/// <param name="writer">The <see cref="TextWriter"/> to where the text for the part should be written to.</param>
		protected virtual void WritePart(TextWriter writer)
		{
			writer.WriteLine(PartTypeTemplate);

			if (valueName != null)
			{
				writer.WriteLine(String.Format(CultureInfo.InvariantCulture, ValueNameTemplate, valueName));
			}
			if (keyName != null)
			{
				writer.WriteLine(String.Format(CultureInfo.InvariantCulture, KeyNameTemplate, keyName));
			}
		}

		/// <summary>
		/// Gets the registry key for the part, or <see langword="null"/> if no key name is required for the part.
		/// </summary>
		public String KeyName
		{
			get { return keyName; }
		}

		/// <summary>
		/// Gets the name for the part.
		/// </summary>
		public String PartName
		{
			get { return partName; }
		}

		/// <summary>
		/// Gets the name for the registry value for the part.
		/// </summary>
		public String ValueName
		{
			get { return valueName; }
		}

		/// <summary>
		/// Gest the template representing the type of the part.
		/// </summary>
		protected abstract String PartTypeTemplate
		{
			get;
		}
	}
}
