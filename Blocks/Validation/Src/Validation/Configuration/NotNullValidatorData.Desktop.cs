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
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration
{
	/// <summary>
	/// Configuration object to describe an instance of class <see cref="NotNullValidator"/>.
	/// </summary>
    [ResourceDescription(typeof(DesignResources), "NotNullValidatorDataDescription")]
    [ResourceDisplayName(typeof(DesignResources), "NotNullValidatorDataDisplayName")]
	partial class NotNullValidatorData
	{
		/// <summary>
		/// <para>Initializes a new instance of the <see cref="NotNullValidatorData"/> class.</para>
		/// </summary>
        public NotNullValidatorData() { Type = typeof(NotNullValidator); }

		/// <summary>
		/// <para>Initializes a new instance of the <see cref="NotNullValidatorData"/> class with a name.</para>
		/// </summary>
		/// <param name="name">The name for the instance.</param>
		public NotNullValidatorData(string name)
			: base(name, typeof(NotNullValidator))
		{ }
	}
}
