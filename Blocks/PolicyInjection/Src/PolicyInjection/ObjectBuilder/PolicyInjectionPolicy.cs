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
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.RemotingInterception;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.ObjectBuilder
{
    /// <summary>
    /// An implementation of the <see cref="IPolicyInjectionPolicy"/> interface that
    /// uses the remoting interception mechanism.
    /// </summary>
    public class PolicyInjectionPolicy : IPolicyInjectionPolicy
    {
        private readonly bool applyPolicies;
        private volatile IConfigurationSource configSource;
        private PolicyInjector injector;
        private object policiesLock = new object();

        /// <summary>
        /// Constructs a new <see cref="PolicyInjectionPolicy"/> with the
        /// given flag to determine if policies should be applied.
        /// </summary>
        /// <param name="applyPolicies">if true, policies should be applied. If false, they should not be.</param>
        public PolicyInjectionPolicy(bool applyPolicies)
        {
            this.applyPolicies = applyPolicies;
        }

        /// <summary>
        /// Gets a flag indicating if the strategy should apply policies or not.
        /// </summary>
        /// <value>the flag.</value>
        public bool ApplyPolicies
        {
            get { return applyPolicies; }
        }

        #region IPolicyInjectionPolicy Members

        /// <summary>
        /// Stores the configuration source that should be used to derive the
        /// <see cref="PolicySet"/> for interception.
        /// </summary>
        /// <param name="configSource">The configuration source.</param>
        public void SetPolicyConfigurationSource(IConfigurationSource configSource)
        {
            if (this.configSource == null || this.configSource != configSource)
            {
                lock(policiesLock)
                {
                    if(this.configSource == null || this.configSource != configSource)
                    {
                        this.configSource = configSource;
                        PolicyInjectorFactory injectorFactory =
                            new PolicyInjectorFactory(configSource);
                        injector = injectorFactory.Create();
                    }
                }
            }
        }

        #endregion

        /// <summary>
        /// Applies interception to the given object.
        /// </summary>
        /// <param name="instanceToProxy">The object to intercept.</param>
        /// <param name="typeToProxy">The interceptable interface or type or return.</param>
        /// <returns>The proxy to the object.</returns>
        public object ApplyProxy(object instanceToProxy, Type typeToProxy)
        {
            return injector.Wrap(instanceToProxy, typeToProxy);
        }
    }
}
