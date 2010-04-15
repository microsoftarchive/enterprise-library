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
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity
{
    /// <summary>
    /// Container extension that allows for supplying additional, transient policies while building up an object
    /// through a container.
    /// </summary>
    public class TransientPolicyBuildUpExtension : UnityContainerExtension
    {
        /// <summary>
        /// Initializes the container with this extension's functionality.
        /// </summary>
        /// <remarks>
        /// This extension does not permfom any initialization.
        /// </remarks>
        protected override void Initialize()
        {
            // does not require initialization
        }

        /// <summary>
        /// Run an existing object through the container and perform injection on it.
        /// </summary>
        /// <param name="t"><see cref="Type"/> of object to perform injection on.</param>
        /// <param name="existing">Instance to build up.</param>
        /// <param name="name">Name to use when looking up the typemappings and other configurations.</param>
        /// <param name="injectionMembers">Providers for transient policies to use.</param>
        /// <returns>The resulting object. By default, this will be object supplied in the <paramref name="existing"/> 
        /// parameter, but container extensions may add things like automatic proxy creation which would cause this to 
        /// return a different object (but still type compatible with t).</returns>
        public object BuildUp(Type t, object existing, string name, params InjectionMember[] injectionMembers)
        {
            return DoBuildUp(t, existing, name, injectionMembers);
        }

        private object DoBuildUp(Type t, object existing, string name, InjectionMember[] injectionMembers)
        {
            var transientPolicies = new PolicyList(this.Context.Policies);
            foreach (var member in injectionMembers)
            {
                member.AddPolicies(null, t, name, transientPolicies);
            }

            object result;
            var buildContext = new BuilderContext(Context.Strategies.MakeStrategyChain(),
                Context.Lifetime, Context.Policies, transientPolicies, new NamedTypeBuildKey(t, name),
                existing);
            try
            {
                result = buildContext.Strategies.ExecuteBuildUp(buildContext);
            }
            catch (Exception exception)
            {
                throw new ResolutionFailedException(t, name, exception, buildContext);
            }
            return result;
        }
    }
}
