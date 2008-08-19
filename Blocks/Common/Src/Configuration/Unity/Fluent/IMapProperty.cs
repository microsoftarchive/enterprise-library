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
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity.Fluent
{
	/// <summary>
	/// Supports the fluent API for the <see cref="PolicyBuilder{TTarget,TSource}"/>.
	/// </summary>
	/// <typeparam name="TTarget">The type on which a property will be set.</typeparam>
	/// <typeparam name="TSource">The type for which the values for the property will be calculated.</typeparam>
	/// <typeparam name="TProperty">The type of the property to set.</typeparam>
	[SuppressMessage("Microsoft.Design", "CA1005",
		Justification = "Three types are needed here to achieve type safety.")]
	public interface IMapProperty<TTarget, TSource, TProperty>
	{
		/// <summary>
		/// Indicate the expression that specifies the value for the property.
		/// </summary>
		/// <param name="mappingExpression">The expression specifying the value.</param>
		[SuppressMessage("Microsoft.Design", "CA1006",
			Justification = "Signatures dealing with Expression trees use nested generics (see http://msdn2.microsoft.com/en-us/library/bb534754.aspx).")]
		IPropertyAndFinishPolicyBuilding<TTarget, TSource> To(
			Expression<Func<TSource, TProperty>> mappingExpression);
	}
}
