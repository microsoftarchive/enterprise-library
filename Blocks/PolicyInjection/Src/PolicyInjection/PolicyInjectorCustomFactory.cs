//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Policy Injection Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Properties;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.RemotingInterception;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection
{
	/// <summary>
	/// A custom factory used by ObjectBuilder to construct the policy injectors
	/// based on configuration.
	/// </summary>
	public class PolicyInjectorCustomFactory : ICustomFactory
	{
		/// <summary>
		/// Returns an new instance of the type the receiver knows how to build.
		/// </summary>
		/// <param name="context">The <see cref="IBuilderContext"/> that represents the current building process.</param>
		/// <param name="name">The name of the instance to build, or null.</param>
		/// <param name="configurationSource">The source for configuration objects.</param>
		/// <param name="reflectionCache">The cache to use retrieving reflection information.</param>
		/// <returns>The new instance.</returns>
		public object CreateObject(
			IBuilderContext context,
			string name,
			IConfigurationSource configurationSource,
			ConfigurationReflectionCache reflectionCache)
		{
			PolicyInjectionSettings settings =
				configurationSource.GetSection(PolicyInjectionSettings.SectionName) as
				PolicyInjectionSettings;
			if (settings != null)
			{
				if (InjectorsAreDefined(settings))
				{
					InjectorData injectorData;
					if (!string.IsNullOrEmpty(name))
					{
						injectorData = settings.Injectors[name];
					}
					else if (!string.IsNullOrEmpty(settings.Injectors.DefaultInjector))
					{
						injectorData = settings.Injectors[settings.Injectors.DefaultInjector];
					}
					else
					{
						throw new ArgumentException(
							Resources.NoDefaultInjectorConfigured, "name");
					}

					return
						AssemblerDrivenInjectorFactory.Instance.Create(
							context,
							injectorData,
							configurationSource,
							reflectionCache);
				}
				else
				{
					if (!string.IsNullOrEmpty(name))
					{
						throw new ArgumentException(
							string.Format(Resources.InvalidInjectorNameConfiguration, name),
							"name");
					}

					return
						new RemotingPolicyInjector(CreatePolicySetFromSettings(settings, context));
				}
			}
			else
			{
				return new RemotingPolicyInjector();
			}
		}

		private static bool InjectorsAreDefined(PolicyInjectionSettings settings)
		{
			return settings.Injectors != null && settings.Injectors.Count > 0;
		}

		private bool ValidInjectorNameConfigured(PolicyInjectionSettings settings)
		{
			return !string.IsNullOrEmpty(settings.Injectors.DefaultInjector) &&
			       settings.Injectors.Contains(settings.Injectors.DefaultInjector);
		}

		private PolicyInjector CreateDefaultInjectorWithGivenPolicySet(PolicySet policies)
		{
			return new RemotingPolicyInjector(policies);
		}

		private static PolicySet CreatePolicySetFromSettings(PolicyInjectionSettings settings, IBuilderContext context)
		{
			IBuilderContext policySetContext = context.CloneForNewBuild(NamedTypeBuildKey.Make<PolicySet>(), null);

			return (PolicySet) policySetContext.Strategies.ExecuteBuildUp(policySetContext);
		}

		private class AssemblerDrivenInjectorFactory : AssemblerBasedObjectFactory<PolicyInjector, InjectorData>
		{
			public static readonly AssemblerDrivenInjectorFactory Instance = new AssemblerDrivenInjectorFactory();
		}
	}
}
