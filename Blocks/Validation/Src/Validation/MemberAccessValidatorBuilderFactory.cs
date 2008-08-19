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
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Validation.Properties;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace Microsoft.Practices.EnterpriseLibrary.Validation
{
	internal class MemberAccessValidatorBuilderFactory
	{
		private MemberValueAccessBuilder valueAccessBuilder;

		public MemberAccessValidatorBuilderFactory()
			: this(new ReflectionMemberValueAccessBuilder())
		{ }

		public MemberAccessValidatorBuilderFactory(MemberValueAccessBuilder valueAccessBuilder)
		{
			this.valueAccessBuilder = valueAccessBuilder;
		}

		/// <summary>
		/// Returns an object that will build a ValueAccessValidator for <paramref name="propertyInfo"/> with the supplied 
		/// validation modifiers.
		/// </summary>
		/// <returns>A <see cref="ValueAccessValidatorBuilder"/> for the supplied parameters.</returns>
		/// <exception cref="ArgumentNullException">when <paramref name="propertyInfo"/> is <see langword="null"/>.</exception>
		public virtual ValueAccessValidatorBuilder GetPropertyValueAccessValidatorBuilder(PropertyInfo propertyInfo, IValidatedElement validatedElement)
		{
			return new ValueAccessValidatorBuilder(this.valueAccessBuilder.GetPropertyValueAccess(propertyInfo),
				validatedElement);
		}

		/// <summary>
		/// Returns an object that will build a ValueAccessValidator for <paramref name="fieldInfo"/> with the supplied 
		/// validation modifiers.
		/// </summary>
		/// <returns>A <see cref="ValueAccessValidatorBuilder"/> for the supplied parameters.</returns>
		/// <exception cref="ArgumentNullException">when <paramref name="fieldInfo"/> is <see langword="null"/>.</exception>
		public virtual ValueAccessValidatorBuilder GetFieldValueAccessValidatorBuilder(FieldInfo fieldInfo, IValidatedElement validatedElement)
		{
			return new ValueAccessValidatorBuilder(this.valueAccessBuilder.GetFieldValueAccess(fieldInfo),
				validatedElement);
		}

		/// <summary>
		/// Returns an object that will build a ValueAccessValidator for <paramref name="methodInfo"/> with the supplied 
		/// validation modifiers.
		/// </summary>
		/// <returns>A <see cref="ValueAccessValidatorBuilder"/> for the supplied parameters.</returns>
		/// <exception cref="ArgumentNullException">when <paramref name="methodInfo"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException">when <paramref name="methodInfo"/> returns <see langword="void"/>.</exception>
		/// <exception cref="ArgumentException">when <paramref name="methodInfo"/> has parameters.</exception>
		public virtual ValueAccessValidatorBuilder GetMethodValueAccessValidatorBuilder(MethodInfo methodInfo, IValidatedElement validatedElement)
		{
			return new ValueAccessValidatorBuilder(this.valueAccessBuilder.GetMethodValueAccess(methodInfo),
				validatedElement);
		}

		public virtual CompositeValidatorBuilder GetTypeValidatorBuilder(Type type, IValidatedElement validatedElement)
		{
			if (null == type)
				throw new ArgumentNullException("type");

			return new CompositeValidatorBuilder(validatedElement);
		}

		public MemberValueAccessBuilder MemberValueAccessBuilder
		{
			get { return this.valueAccessBuilder; }
		}
	}
}