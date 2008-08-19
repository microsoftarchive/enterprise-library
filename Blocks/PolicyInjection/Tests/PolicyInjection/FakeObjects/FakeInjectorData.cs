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

using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Tests.FakeObjects
{
    [Assembler(typeof(FakeInjectorAssembler))]
    public class FakeInjectorData : InjectorData
    {
        private const string extraValuePropertyName = "extraValue";
        public FakeInjectorData()
        {
            
        }

        public FakeInjectorData(string name)
            : base(name, typeof(FakeInjector))
        {
            
        }

        [ConfigurationProperty(extraValuePropertyName, DefaultValue = -1, IsRequired = false)]
        public int ExtraValue
        {
            get { return (int)( base[extraValuePropertyName] ); }
            set { base[extraValuePropertyName] = value; }
        }

    }

    public class FakeInjectorAssembler : IAssembler<PolicyInjector, InjectorData>
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
            FakeInjectorData injectorData = (FakeInjectorData)objectConfiguration;
            FakeInjector injector = new FakeInjector();
            injector.ExtraValue = injectorData.ExtraValue;
            return injector;
        }
    }
}
