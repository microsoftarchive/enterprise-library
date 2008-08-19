//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Unity
{
	/// <summary>
	/// Container extension to the policies required to create the Exception Handling Application Block's
	/// objects described in the configuration file.
	/// </summary>
	public class ExceptionHandlingBlockExtension : EnterpriseLibraryBlockExtension
	{
		/// <summary>
		/// Adds the policies describing the Exception Handling Application Block's objects.
		/// </summary>
		protected override void Initialize()
		{
			ExceptionHandlingSettings settings
				= (ExceptionHandlingSettings)ConfigurationSource.GetSection(ExceptionHandlingSettings.SectionName);

			if (settings == null)
			{
				return;
			}

			CreateExceptionPoliciesPolicies(Context.Policies, settings.ExceptionPolicies);
			CreateExceptionManagerPolicy(Context.Policies, settings);
            Container.RegisterType<ExceptionManager, ExceptionManagerImpl>();
		}

		private void CreateExceptionPoliciesPolicies(
			IPolicyList policyList,
			IEnumerable<ExceptionPolicyData> policies)
		{
			foreach (ExceptionPolicyData policyData in policies)
			{
				NamedTypeBuildKey instanceKey = NamedTypeBuildKey.Make<ExceptionPolicyImpl>(policyData.Name);
				string parentPrefix = Guid.NewGuid().ToString();

				new PolicyBuilder<ExceptionPolicyImpl, ExceptionPolicyData>(instanceKey, policyData,
						c => new ExceptionPolicyImpl(
							c.Name,
							Resolve.ReferenceDictionary<Dictionary<Type, ExceptionPolicyEntry>, ExceptionPolicyEntry, Type>(
								from t in c.ExceptionTypes select new KeyValuePair<string, Type>(parentPrefix + t.Name, t.Type))))
					.AddPoliciesToPolicyList(policyList);

				CreateExceptionTypePolicies(policyList, policyData.ExceptionTypes, parentPrefix);
			}
		}

		private void CreateExceptionTypePolicies(
			IPolicyList policyList,
			IEnumerable<ExceptionTypeData> types,
			string policyParentPrefix)
		{
			foreach (ExceptionTypeData policyTypeData in types)
			{
				NamedTypeBuildKey instanceKey = NamedTypeBuildKey.Make<ExceptionPolicyEntry>(policyParentPrefix + policyTypeData.Name);
				string parentPrefix = Guid.NewGuid().ToString();

				new PolicyBuilder<ExceptionPolicyEntry, ExceptionTypeData>(instanceKey, policyTypeData,
						c => new ExceptionPolicyEntry(
							c.PostHandlingAction,
							Resolve.ReferenceCollection<List<IExceptionHandler>, IExceptionHandler>(
								from h in c.ExceptionHandlers select parentPrefix + h.Name)))
					.AddPoliciesToPolicyList(policyList);

				CreateProvidersPolicies<IExceptionHandler, ExceptionHandlerData>(
					policyList,
					null,
					policyTypeData.ExceptionHandlers,
					ConfigurationSource,
					h => parentPrefix + h.Name);
			}
		}

		private void CreateExceptionManagerPolicy(
			IPolicyList policyList, 
			ExceptionHandlingSettings settings)
		{
			new PolicyBuilder<ExceptionManagerImpl, ExceptionHandlingSettings>(
				NamedTypeBuildKey.Make<ExceptionManagerImpl>(),
				settings,
				c => new ExceptionManagerImpl(
					Resolve.ReferenceDictionary<Dictionary<string, ExceptionPolicyImpl>, ExceptionPolicyImpl, string>(
						from p in c.ExceptionPolicies select new KeyValuePair<string, string>(p.Name, p.Name))))
				.AddPoliciesToPolicyList(policyList);
		}
	}
}
