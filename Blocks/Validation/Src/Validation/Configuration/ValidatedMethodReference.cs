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
	/// Represents the validation information for a method.
	/// </summary>
	/// <seealso cref="ValidatedTypeReference"/>
	/// <seealso cref="ValidationRulesetData"/>
	/// <seealso cref="ValidatedMemberReference"/>
	public sealed class ValidatedMethodReference : ValidatedMemberReference
	{
		/// <summary>
		/// <para>Initializes a new instance of the <see cref="ValidatedMethodReference"/> class.</para>
		/// </summary>
		public ValidatedMethodReference()
		{ }

		/// <summary>
		/// <para>Initializes a new instance of the <see cref="ValidatedMethodReference"/> class with a name.</para>
		/// </summary>
		/// <param name="name">The name for the instance.</param>
		public ValidatedMethodReference(string name)
			: base(name)
		{ }
	}
}