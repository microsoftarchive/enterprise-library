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
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Common.Properties;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity
{
	/// <summary>
	/// A <see cref="IContainerPolicyCreator"/> implementation that tries to match a target type 
	/// public constructors' parameters with an object's properties to create build plan policies.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Matching is performed by considering the target type's constructors in decreasing order by
	/// number for parameters and trying to match the parameters' names with the properties in source object.
	/// If a match is found, a policy to use the matched constructor will be created, and the values for the matched source object properties
	/// will be used as the constructor parameters.
	/// If not match is found for any constructor an exception is thrown.
	/// </para>
	/// <para>
	/// To match parameters to properties, a case-insensitive comparison is performed and the property is validated
	/// for the following conditions:
	/// <list type="bullet">
	/// <item>
	/// <description>The property is public.</description>
	/// </item>
	/// <item>
	/// <description>The property can be read.</description>
	/// </item>
	/// <item>
	/// <description>The property is not an indexer.</description>
	/// </item>
	/// <item>
	/// <description>The property has a matching type.</description>
	/// </item>
	/// </list>
	/// </para>
	/// <para>
	/// Subclasses can override how matching is performed and what kind of parameter resolution policies are created.
	/// </para>
	/// </remarks>
	public class ConstructorArgumentMatchingPolicyCreator : IContainerPolicyCreator
	{
		private Type targetType;

		/// <summary>
		/// Initializes a new instance of class <see cref="ConstructorArgumentMatchingPolicyCreator"/> for a given type.
		/// </summary>
		/// <param name="targetType">The <see cref="Type"/> for which policies will be created.</param>
		public ConstructorArgumentMatchingPolicyCreator(Type targetType)
		{
			Guard.ArgumentNotNull(targetType, "targetType");
			this.targetType = targetType;
		}

		[SuppressMessage("Microsoft.Design", "CA1033",
			Justification = "The class defines virtual methods subclasses are expected to override if needed.")]
		void IContainerPolicyCreator.CreatePolicies(
			IPolicyList policyList,
			string instanceName,
			ConfigurationElement configurationObject,
			IConfigurationSource configurationSource)
		{
			IEnumerable<ConstructorInfo> ctors
				= from ctor in this.targetType.GetConstructors(BindingFlags.Public | BindingFlags.Instance)
				  orderby ctor.GetParameters().Length descending
				  select ctor;

			foreach (ConstructorInfo ctor in ctors)
			{
				if (TryToMatchAndAddPolicies(ctor, configurationObject, policyList, instanceName))
				{
					return;
				}
			}

			// if we got this far, no match could be obtained - throw to indicate this
			throw new ArgumentException(
				string.Format(
					CultureInfo.CurrentCulture, 
					Resources.ExceptionUnableToMatchConstructorToConfigurationObject,
					configurationObject.GetType().AssemblyQualifiedName,
					this.targetType.AssemblyQualifiedName));
		}

		private bool TryToMatchAndAddPolicies(
			ConstructorInfo ctor,
			ConfigurationElement configurationObject,
			IPolicyList policyList,
			string instanceName)
		{
			Type configurationObjectType = configurationObject.GetType();

			ParameterInfo[] constructorParameters = ctor.GetParameters();
			List<PropertyInfo> matchingProperties = new List<PropertyInfo>();

			foreach (ParameterInfo parameter in constructorParameters)
			{
				// only deal with public instance properties
				PropertyInfo matchingProperty
					= configurationObjectType.GetProperty(
						parameter.Name,
						BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);

				// that are readable and not indexers
				if (!(matchingProperty != null
					&& matchingProperty.CanRead
					&& matchingProperty.GetIndexParameters().Length == 0))
				{
					return false;
				}

				// and are type compatible with the parameter (whatever that means)
				if (!MatchArgumentAndPropertyTypes(parameter, matchingProperty))
				{
					return false;
				}

				matchingProperties.Add(matchingProperty);
			}

			// we got so far, there's a match
			// set up the policies

			SelectedConstructor selectedConstructor = new SelectedConstructor(ctor);

			// first the parameter policies
			for (int i = 0; i < constructorParameters.Length; i++)
			{
				ParameterInfo parameterInfo = constructorParameters[i];
				PropertyInfo propertyInfo = matchingProperties[i];

				string parameterKey = Guid.NewGuid().ToString();

				object value = propertyInfo.GetValue(configurationObject, null);
				policyList.Set<IDependencyResolverPolicy>(CreateDependencyResolverPolicy(parameterInfo, value), parameterKey);
				selectedConstructor.AddParameterKey(parameterKey);
			}

			// next the ctor policy
			policyList.Set<IConstructorSelectorPolicy>(
				new FixedConstructorSelectorPolicy(selectedConstructor),
				new NamedTypeBuildKey(targetType, instanceName));

			return true;
		}

		/// <summary>
		/// Checks <paramref name="parameterInfo"/> and <paramref name="propertyInfo"/> for type matching.
		/// </summary>
		/// <param name="parameterInfo">The constructor parameter to match</param>
		/// <param name="propertyInfo">The property to match.</param>
		/// <returns><see langword="true"/> if types are compatible, otherwise <see langword="false"/>.</returns>.
		/// <remarks>
		/// The default behavior is to match for simple <see cref="Type.IsAssignableFrom">assignability</see>. 
		/// Subclasses can override this behavior.
		/// </remarks>
		protected virtual bool MatchArgumentAndPropertyTypes(ParameterInfo parameterInfo, PropertyInfo propertyInfo)
		{
			return parameterInfo.ParameterType.IsAssignableFrom(propertyInfo.PropertyType);
		}

		/// <summary>
		/// Returns a <see cref="IDependencyResolverPolicy"/> suitable to resolve <paramref name="value"/>
		/// for <paramref name="parameterInfo"/>.
		/// </summary>
		/// <param name="parameterInfo">The constructor parameter.</param>
		/// <param name="value">The raw value from the source object.</param>
		/// <returns>An instance of <see cref="ConstantResolverPolicy"/> that returns <paramref name="value"/>. Subclasses
		/// can override this behavior.</returns>
		protected virtual IDependencyResolverPolicy CreateDependencyResolverPolicy(ParameterInfo parameterInfo, object value)
		{
			return new ConstantResolverPolicy(value);
		}
	}
}
