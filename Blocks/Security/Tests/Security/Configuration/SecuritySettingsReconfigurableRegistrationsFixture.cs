//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
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
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel.Unity;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Tests.Configuration
{
    public abstract class UpdatedConfigurationSourceContext : ArrangeActAssert
    {
        protected UnityContainerConfigurator containerConfigurator;
        protected UnityContainer container;
        protected ConfigurationSourceUpdatable updatableConfigurationSource;
        protected SecuritySettings settings;
        protected AuthorizationRuleProviderData ruleProvider;

        protected override void Arrange()
        {
            updatableConfigurationSource = new ConfigurationSourceUpdatable();

            ruleProvider = new AuthorizationRuleProviderData();
            ruleProvider.Name = "ruleProvider";

            settings = new SecuritySettings();
            settings.AuthorizationProviders.Add(ruleProvider);

            updatableConfigurationSource.Add(SecuritySettings.SectionName, settings);

            container = new UnityContainer();
            containerConfigurator = new UnityContainerConfigurator(container);
            EnterpriseLibraryContainer.ConfigureContainer(containerConfigurator, updatableConfigurationSource);
        }

        protected override void Act()
        {
            updatableConfigurationSource.DoSourceChanged(new string[] { SecuritySettings.SectionName });
        }
    }

    [TestClass]
    public class WhenAuthorizationProviderNameChanges: UpdatedConfigurationSourceContext
    {
        protected override void Act()
        {
            ruleProvider.Name = "new name";

            base.Act();
        }

        [TestMethod]
        public void ThenCanResolveproviderUsingNewName()
        {
            Assert.IsNotNull(container.Resolve<IAuthorizationProvider>("new name"));
        }

    }

}
