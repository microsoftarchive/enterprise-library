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

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity.Fluent
{
	/// <summary>
	/// Supports the fluent API for the <see cref="PolicyBuilder{TTarget,TSource}"/>.
	/// </summary>
	public interface IPropertyAndFinishPolicyBuilding<TTarget, TSource> :
		IPropertyPolicyBuilding<TTarget, TSource>,
		IFinishPoliciesBuilding
	{ }
}
