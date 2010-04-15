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
using Console.Wpf.Tests.VSTS.Mocks;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.TestSupport;

namespace Console.Wpf.Tests.VSTS.DevTests.given_configuration_source_model
{
    [TestClass]
    public class when_loading_configuration : ContainerContext
    {
        bool refreshed;
        ConfigurationSourceDependency dependency;

        protected override void Arrange()
        {
            base.Arrange();

            dependency = Container.Resolve<ConfigurationSourceDependency>();
            dependency.Cleared += (sender, args) => refreshed = true;

        }

        protected override void Act()
        {
            var sourceModel = Container.Resolve<ConfigurationSourceModel>();
            sourceModel.Load(new DesignDictionaryConfigurationSource());
        }

        [TestMethod]
        public void then_dependency_is_refreshed()
        {
            Assert.IsTrue(refreshed);
        }
    }
}
