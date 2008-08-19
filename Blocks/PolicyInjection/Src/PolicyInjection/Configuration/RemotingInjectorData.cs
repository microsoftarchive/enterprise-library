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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.RemotingInterception;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration
{
    /// <summary>
    /// Configuration element to specify the <see cref="RemotingPolicyInjector"/>.
    /// </summary>
    [Assembler(typeof(RemotingInjectorAssembler))]
    public class RemotingInjectorData : InjectorData
    {
        /// <summary>
        /// Create a new unintialized <see cref="RemotingInjectorData"/>.
        /// </summary>
        public RemotingInjectorData()
        {
            
        }

        /// <summary>
        /// Create a new <see cref="RemotingInjectorData"/> with the given name.
        /// </summary>
        /// <param name="name">Name of the injector in the config file.</param>
        public RemotingInjectorData(string name)
            : base(name, typeof(RemotingPolicyInjector))
        {
            
        }
    }

    /// <summary>
    /// A class used by ObjectBuilder to construct <see cref="RemotingPolicyInjector"/>
    /// instances from a <see cref="InjectorData"/> instance.
    /// </summary>
    public class RemotingInjectorAssembler : IAssembler<PolicyInjector, InjectorData>
    {
        /// <summary>
        /// Builds an instance of the subtype of <see cref="PolicyInjector"/> type the receiver knows how to build,  based on 
        /// an a configuration object.
        /// </summary>
        /// <param name="context">The <see cref="IBuilderContext"/> that represents the current building process.</param>
        /// <param name="objectConfiguration">The configuration object that describes the object to build.</param>
        /// <param name="configurationSource">The source for configuration objects.</param>
        /// <param name="reflectionCache">The cache to use retrieving reflection information.</param>
        /// <returns>A fully initialized instance of the <see cref="PolicyInjector"/> subtype.</returns>
        public PolicyInjector Assemble(
            IBuilderContext context,
            InjectorData objectConfiguration,
            IConfigurationSource configurationSource,
            ConfigurationReflectionCache reflectionCache)
        {
            PolicySetFactory policyFactory = new PolicySetFactory(configurationSource);
            PolicySet policies = policyFactory.Create();

            PolicyInjector injector = new RemotingPolicyInjector(policies);
            return injector;
        }
    }
}
