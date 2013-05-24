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
    /// Container extension that supports supplying additional, transient policies when building up an object
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
        /// Runs an existing object through the container and performs injection on it.
        /// </summary>
        /// <param name="t">The <see cref="Type"/> of object to perform injection on.</param>
        /// <param name="existing">The instance to build up.</param>
        /// <param name="name">The name to use when looking up the type mappings and other configurations.</param>
        /// <param name="injectionMembers">The providers of transient policies.</param>
        /// <returns>The resulting object. By default, this will be the object supplied in the <paramref name="existing"/> 
        /// parameter. However, container extensions may add things such as automatic proxy creation, which would cause this method to 
        /// return a different object that is still type-compatible with the t parameter.</returns>
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
