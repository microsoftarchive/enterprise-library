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

using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;

namespace Common.ContainerInfrastructure.Tests.VSTS.TestSupport
{
    /// <summary>
    /// Helper class to build up <see cref="PolicyInjectionSettings"/> objects
    /// programatically using a fluent builder pattern.
    /// </summary>
    /// <remarks>This class is just a sketch right now, just enough to
    /// implement the default settings we need in this current test project.
    /// It will need to grow before being actually useful.</remarks>
    public class PiabSectionBuilder
    {
        private readonly List<PolicyBuilder> builders = new List<PolicyBuilder>();

        public PiabSectionBuilder AddPolicy(string name)
        {
            builders.Add(new PolicyBuilder(name));
            return this;
        }

        public void AddTo(IConfigurationSource configurationSource)
        {
            var section = new PolicyInjectionSettings();
            foreach(var builder in builders)
            {
                builder.AddTo(section);
            }
            configurationSource.Add(BlockSectionNames.PolicyInjection, section);
        }

        private class PolicyBuilder
        {
            private readonly PolicyData policySetting = new PolicyData();

            public PolicyBuilder(string name)
            {
                policySetting.Name = name;
            }

            public void AddTo(PolicyInjectionSettings settings)
            {
                settings.Policies.Add(new PolicyData(policySetting.Name));
            }
        }
    }
}
