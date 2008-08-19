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

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration
{
	/// <summary>
	/// Represents an individual domain element for a <see cref="Microsoft.Practices.EnterpriseLibrary.Validation.Validators.DomainValidator{T}"/>.
	/// </summary>
	public class DomainConfigurationElement : NamedConfigurationElement
	{
		/// <summary>
		/// Initialize a new instance of a <see cref="DomainConfigurationElement"/> class.
		/// </summary>
		public DomainConfigurationElement()
		{ }

		/// <summary>
		/// Intialize a new instance of a <see cref="DomainConfigurationElement"/> class with a name.
		/// </summary>
		/// <param name="name">The name of the element.</param>
		public DomainConfigurationElement(string name)
			: base(name)
		{ }
	}
}
