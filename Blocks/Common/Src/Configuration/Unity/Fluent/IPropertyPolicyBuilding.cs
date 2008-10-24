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
	public interface IPropertyPolicyBuilding<TTarget, TSource>
	{
		/// <summary>
		/// Specify a property mapping.
		/// </summary>
		/// <typeparam name="TProperty">The type of the target property.</typeparam>
		/// <param name="propertyAccessExpression">The expresion that specifies the property to map to. Should look like "o => o.Property".</param>
		[SuppressMessage("Microsoft.Design", "CA1006",
			Justification = "Signatures dealing with Expression trees use nested generics (see http://msdn2.microsoft.com/en-us/library/bb534754.aspx).")]
		IMapProperty<TTarget, TSource, TProperty> SetProperty<TProperty>(Expression<Func<TTarget, TProperty>> propertyAccessExpression);
	}
}
