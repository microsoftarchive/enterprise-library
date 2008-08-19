//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Validation Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration
{
	/// <summary>
	/// Configuration section for stored validation information.
	/// </summary>
	/// <seealso cref="ValidatedTypeReference"/>
	public class ValidationSettings : SerializableConfigurationSection
	{
		/// <summary>
		/// The name used to serialize the configuration section.
		/// </summary>
		public const string SectionName = "validation";

		private const string TypesPropertyName = "";
		/// <summary>
		/// Gets the collection of types for which validation has been configured.
		/// </summary>
		[ConfigurationProperty(TypesPropertyName, IsDefaultCollection = true)]
		public ValidatedTypeReferenceCollection Types
		{
			get { return (ValidatedTypeReferenceCollection)this[TypesPropertyName]; }
		}
	}
}