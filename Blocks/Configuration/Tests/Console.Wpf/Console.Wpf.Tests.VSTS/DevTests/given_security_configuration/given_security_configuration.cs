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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Security;

namespace Console.Wpf.Tests.VSTS.DevTests.given_security_configuration
{
    [TestClass]
    public class given_security_configuration : ContainerContext
    {
        SecuritySettings securitySettings;

        protected override void Arrange()
        {
            base.Arrange();

            ConfigurationSourceBuilder builder = new ConfigurationSourceBuilder();
            builder.ConfigureSecurity()
                 .AuthorizeUsingCustomProviderNamed("custom authz", typeof(IAuthorizationProvider))
                .AuthorizeUsingRuleProviderNamed("ruleProvider")
                        .SpecifyRule("rule1", "true")
                        .SpecifyRule("rule2", "false")
                .CacheSecurityInCacheStoreNamed("cache Storage").WithOptions.UseSharedCacheManager("cache");

            DictionaryConfigurationSource source = new DictionaryConfigurationSource();
            builder.UpdateConfigurationWithReplace(source);


            securitySettings = (SecuritySettings)source.GetSection(SecuritySettings.SectionName);
        }

        SectionViewModel viewModel;

        protected override void Act()
        {
            viewModel = SectionViewModel.CreateSection(Container, SecuritySettings.SectionName, securitySettings);
        }

    }
}
