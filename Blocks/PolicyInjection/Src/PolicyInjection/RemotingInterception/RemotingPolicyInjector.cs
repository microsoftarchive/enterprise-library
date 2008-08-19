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
using System.Runtime.Remoting;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.RemotingInterception
{
    /// <summary>
    /// This class holds onto a <see cref="PolicySet" /> and can
    /// inject that policy via a remoting proxy into either a newly created object or
    /// an existing object.
    /// </summary>
    [ConfigurationElementType(typeof(RemotingInjectorData))]
    public class RemotingPolicyInjector : PolicyInjector
    {
        /// <summary>
        /// Creates a new <see cref="RemotingPolicyInjector" /> with an 
        /// empty <see cref="PolicySet" />.
        /// </summary>
        public RemotingPolicyInjector()
        {
        }

        /// <summary>
        /// Creates a new <see cref="RemotingPolicyInjector" /> with the
        /// given <see cref="PolicySet"/>.
        /// </summary>
        /// <param name="policies"><see cref="PolicySet"/> to use when 
        /// creating object or wrapping existing ones.</param>
        public RemotingPolicyInjector(PolicySet policies) : base(policies)
        {
        }

        /// <summary>
        /// Checks to see if the given type can be intercepted.
        /// </summary>
        /// <remarks>In this implementation, only interfaces and types derived from MarshalByRefObject
        /// can have policies applied.</remarks>
        /// <param name="t">Type to check.</param>
        /// <returns>True if this type can be intercepted, false if it cannot.</returns>
        public override bool TypeSupportsInterception(Type t)
        {
            return ( typeof(MarshalByRefObject).IsAssignableFrom(t) || t.IsInterface );
        }

        /// <summary>
        /// Wraps the given instance in a proxy with interception hooked up if it
        /// is required by policy. If not required, returns the unwrapped instance.
        /// </summary>
        /// <param name="instance">object to wrap.</param>
        /// <param name="typeToReturn">Type of the reference to return.</param>
        /// <param name="policiesForThisType">Policy set specific to typeToReturn.</param>
        /// <returns>The object with policy added.</returns>
        protected override object DoWrap(object instance, Type typeToReturn, PolicySet policiesForThisType)
        {
            if (PolicyRequiresInterception(policiesForThisType))
            {
                InterceptingRealProxy proxy =
                    new InterceptingRealProxy(UnwrapTarget(instance), typeToReturn, policiesForThisType);
                return proxy.GetTransparentProxy();
            }
            return instance;
        }

        private object UnwrapTarget(object target)
        {
            if( RemotingServices.IsTransparentProxy(target))
            {
                InterceptingRealProxy realProxy =
                    RemotingServices.GetRealProxy(target) as InterceptingRealProxy;
                if( realProxy != null )
                {
                    return realProxy.Target;
                }
            }
            return target;
        }
    }
}
