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

using System.Collections.Generic;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity
{
	/// <summary>
	/// Property selector policy that returns a fixed set of selected properties previously specified.
	/// </summary>
	public sealed class FixedPropertySelectorPolicy : IPropertySelectorPolicy
	{
		private readonly IEnumerable<SelectedProperty> selectedProperties;

		/// <summary>
		/// Initializes a new instance of the class <see cref="FixedPropertySelectorPolicy"/> with the
		/// supplied <see cref="SelectedProperty"/> instances.
		/// </summary>
		/// <param name="selectedProperties">The selected properties to return on request.</param>
		public FixedPropertySelectorPolicy(IEnumerable<SelectedProperty> selectedProperties)
		{
			this.selectedProperties = selectedProperties;
		}

		IEnumerable<SelectedProperty> IPropertySelectorPolicy.SelectProperties(IBuilderContext context)
		{
			return selectedProperties;
		}
	}
}
