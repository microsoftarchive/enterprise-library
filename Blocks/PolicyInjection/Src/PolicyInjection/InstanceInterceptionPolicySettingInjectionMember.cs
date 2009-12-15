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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection
{
    ///<summary>
    /// Sets the <see cref="IInstanceInterceptor"/> on the <see cref="IInstanceInterceptionPolicy"/> for
    /// a type in the <see cref="IUnityContainer"/>
    ///</summary>
    public class InstanceInterceptionPolicySettingInjectionMember : InjectionMember
    {
        ///<summary>
        /// Initializes the InstanceInterceptionPolicySettingInjectionMember with the provided interceptor.
        ///</summary>
        ///<param name="interceptor">The <see cref="IInstanceInterceptor"/> to set on the <see cref="IInstanceInterceptionPolicy"/></param>
        ///<exception cref="ArgumentNullException">A valid interceptor is required</exception>
        public InstanceInterceptionPolicySettingInjectionMember(IInstanceInterceptor interceptor)
        {
            if (interceptor == null)
            {
                throw new ArgumentNullException("interceptor");
            }

            this.Interceptor = interceptor;
        }

        /// <summary>
        /// Add policies to the <paramref name="policies"/> to configure the container with 
        /// an appropriate <see cref="IInstanceInterceptionPolicy"/>
        /// </summary>
        /// <param name="serviceType">Type of the interface being registered. This parameter is
        /// ignored by this class.</param>
        /// <param name="implementationType">Type to register.</param>
        /// <param name="name">Name used to resolve the type object.</param>
        /// <param name="policies">Policy list to add policies to.</param>
        public override void AddPolicies(Type serviceType, Type implementationType, string name, IPolicyList policies)
        {
            var key = new NamedTypeBuildKey(implementationType);
            policies.Set<IInstanceInterceptionPolicy>(new FixedInstanceInterceptionPolicy(Interceptor), key);

            var piabInjectionMember = new InterceptionBehavior<PolicyInjectionBehavior>();
            piabInjectionMember.AddPolicies(serviceType, implementationType, name, policies);
        }

        ///<summary>
        /// The <see cref="IInstanceInterceptor"/> set on the <see cref="IInstanceInterceptionPolicy"/>
        ///</summary>
        public IInstanceInterceptor Interceptor { get; private set; }
    }
}
