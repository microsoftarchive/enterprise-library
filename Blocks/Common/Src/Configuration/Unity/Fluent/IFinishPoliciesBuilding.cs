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

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity.Fluent
{
	/// <summary>
	/// Supports the fluent API for the <see cref="PolicyBuilder{TTarget,TSource}"/>.
	/// </summary>
	public interface IFinishPoliciesBuilding
	{
		/// <summary>
		/// Finish the creation process by adding all the collected policies to <paramref name="policyList"/>.
		/// </summary>
		/// <param name="policyList">The destination <see cref="IPolicyList"/>.</param>
		void AddPoliciesToPolicyList(IPolicyList policyList);
	}
}
