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
using Microsoft.Practices.Unity;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity
{
    /// <summary>
    /// 
    /// </summary>
    public class TransientPolicyBuildUpExtension : UnityContainerExtension
    {
        /// <summary>
        /// 
        /// </summary>
        protected override void Initialize()
        {
            // does not require initialization
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <param name="existing"></param>
        /// <param name="name"></param>
        /// <param name="injectionMembers"></param>
        /// <returns></returns>
        public object BuildUp(Type t, object existing, string name, params InjectionMember[] injectionMembers)
        {
            return DoBuildUp(t, existing, name, injectionMembers);
        }

        private object DoBuildUp(Type t, object existing, string name, InjectionMember[] injectionMembers)
        {
            var transientPolicies = new PolicyList(this.Context.Policies);
            foreach (var member in injectionMembers)
            {
                member.AddPolicies(t, name, transientPolicies);
            }

            object result;
            try
            {
                result =
                    new Builder()
                        .BuildUp(
                            this.Context.Locator,
                            this.Context.Lifetime,
                            transientPolicies,
                            this.Context.Strategies.MakeStrategyChain(),
                            new NamedTypeBuildKey(t, name),
                            existing);
            }
            catch (BuildFailedException exception)
            {
                throw new ResolutionFailedException(t, name, exception);
            }
            return result;
        }
    }
}
