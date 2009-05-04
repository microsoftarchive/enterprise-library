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
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Common.Properties;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity
{
	/// <summary>
	/// Standard policy creator for custom providers.
	/// </summary>
	/// <typeparam name="T">The type of the custom provider data class representing a custom provider.</typeparam>
	public sealed class CustomProviderPolicyCreator<T> : IContainerPolicyCreator
		where T : NameTypeConfigurationElement, IHelperAssistedCustomConfigurationData<T>
	{
		private readonly Type[] constructorTypes = new Type[] { typeof(NameValueCollection) };

		void IContainerPolicyCreator.CreatePolicies(
			IPolicyList policyList,
			string instanceName,
			ConfigurationElement configurationObject,
			IConfigurationSource configurationSource)
		{
			T castConfigurationObject = (T)configurationObject;
			
			// we expect custom providers to have a constructor with a specific signature
			ConstructorInfo ctor = castConfigurationObject.Type.GetConstructor(
				BindingFlags.Public | BindingFlags.Instance, null, constructorTypes, null);

			if (ctor == null)
			{
				throw new ArgumentException(
					string.Format(
						CultureInfo.CurrentCulture, 
						Resources.ExceptionCustomProviderTypeDoesNotHaveTheRequiredConstructor,
						castConfigurationObject.Type.AssemblyQualifiedName),
					"configurationObject");
			}

			SelectedConstructor selectedCtor = new SelectedConstructor(ctor);
			string parameterKey = Guid.NewGuid().ToString();
			selectedCtor.AddParameterKey(parameterKey);
			policyList.Set<IConstructorSelectorPolicy>(
				new FixedConstructorSelectorPolicy(selectedCtor),
				new NamedTypeBuildKey(castConfigurationObject.Type, instanceName));
			policyList.Set<IDependencyResolverPolicy>(
				new ConstantResolverPolicy(new NameValueCollection(castConfigurationObject.Attributes)),
				parameterKey);
		}
	}
}
