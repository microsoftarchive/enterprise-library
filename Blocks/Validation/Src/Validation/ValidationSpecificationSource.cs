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

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Validation
{
	/// <summary>
	/// Specifies the required source for validation information when invoking <see cref="Validator"/> creation methods.
	/// </summary>
	/// <seealso cref="ValidationFactory.CreateValidator{T}(string, IConfigurationSource)"/>
	public enum ValidationSpecificationSource
	{
		/// <summary>
		/// Validation information is to be retrieved from attributes.
		/// </summary>
		Attributes,

		/// <summary>
		/// Validation information is to be retrieved from configuration.
		/// </summary>
		Configuration,

		/// <summary>
		/// Validation information is to be retrieved from both attributes and configuration.
		/// </summary>
		Both
	}
}