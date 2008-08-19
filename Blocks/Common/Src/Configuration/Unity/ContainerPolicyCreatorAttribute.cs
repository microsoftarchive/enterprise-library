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
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Common.Properties;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity
{
	/// <summary>
	/// Specifies the type that can create the policies necessary to create an object
	/// as described by configuration objects.
	/// </summary>
	/// <remarks>
	/// The specified type must implement the <see cref="IContainerPolicyCreator"/> interface.
	/// </remarks>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public sealed class ContainerPolicyCreatorAttribute : Attribute
	{
		/// <summary>
		/// Initializes a new instance of the class <see cref="ContainerPolicyCreatorAttribute"/>.
		/// </summary>
		/// <param name="policyCreatorType">The type implementing <see cref="IContainerPolicyCreator"/>.</param>
		/// <exception cref="ArgumentNullException">when <paramref name="policyCreatorType"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException">when <paramref name="policyCreatorType"/> does not implement 
		/// <see cref="IContainerPolicyCreator"/>.</exception>
		/// <exception cref="ArgumentException">when <paramref name="policyCreatorType"/> does not have a zero-arguments 
		/// constructor.</exception>
		public ContainerPolicyCreatorAttribute(Type policyCreatorType)
		{
			Guard.ArgumentNotNull(policyCreatorType, "policyCreatorType");

			if (!typeof (IContainerPolicyCreator).IsAssignableFrom(policyCreatorType))
			{
				throw new ArgumentException(
					string.Format(
						CultureInfo.CurrentCulture,
						Resources.ExceptionMustImplementIContainerPolicyCreator, policyCreatorType.AssemblyQualifiedName),
					"policyCreatorType");
			}

			if (policyCreatorType.GetConstructor(Type.EmptyTypes) == null)
			{
				throw new ArgumentException(
					string.Format(
						CultureInfo.CurrentCulture, 
						Resources.ExceptionMustHaveNoArgsConstructor, policyCreatorType.AssemblyQualifiedName),
					"policyCreatorType");
			}

			this.PolicyCreatorType = policyCreatorType;
		}

		/// <summary>
		/// Gets the <see cref="Type"/> specified by the attribute.
		/// </summary>
		public Type PolicyCreatorType
		{
			get;
			private set;
		}
	}
}