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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.ObjectBuilder
{
    /// <summary>
    /// An ObjectBuilder strategy class that runs objects through PIAB as part of construction.
    /// </summary>
    public class PolicyInjectionStrategy : EnterpriseLibraryBuilderStrategy
    {
		/// <summary>
		/// Wraps the <paramref name="context"/>'s existing object 
		/// running it through PIAB if the policy set applies.
		/// </summary>
		/// <param name="context">ObjectBuilder context.</param>
		public override void PostBuildUp(IBuilderContext context)
		{
			base.PostBuildUp(context);

			IPolicyInjectionPolicy policyInjectionPolicy = context.Policies.Get<IPolicyInjectionPolicy>(context.BuildKey);
			if (policyInjectionPolicy != null && policyInjectionPolicy.ApplyPolicies)
			{
				policyInjectionPolicy.SetPolicyConfigurationSource(GetConfigurationSource(context));
				context.Existing = policyInjectionPolicy.ApplyProxy(context.Existing, BuildKey.GetType(context.OriginalBuildKey));
			}
		}
    }
}
