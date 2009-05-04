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

using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity
{
	/// <summary>
	/// Constructor Selector Policy that return a fixed constructor, specified during creation.
	/// </summary>
	public sealed class FixedConstructorSelectorPolicy : IConstructorSelectorPolicy
	{
		private readonly SelectedConstructor selectedConstructor;

		/// <summary>
		/// Initializes a new instance of the class <see cref="FixedConstructorSelectorPolicy"/>
		/// with the supplied constructor.
		/// </summary>
		/// <param name="selectedConstructor">The fixed constructor to select.</param>
		public FixedConstructorSelectorPolicy(SelectedConstructor selectedConstructor)
		{
			Guard.ArgumentNotNull(selectedConstructor, "selectedConstructor");

			this.selectedConstructor = selectedConstructor;
		}

		SelectedConstructor IConstructorSelectorPolicy.SelectConstructor(IBuilderContext context)
		{
			return this.selectedConstructor;
		}
	}
}
