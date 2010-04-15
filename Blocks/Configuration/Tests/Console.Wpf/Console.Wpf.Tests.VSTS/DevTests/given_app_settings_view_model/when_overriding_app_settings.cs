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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics;

namespace Console.Wpf.Tests.VSTS.DevTests.given_app_settings_view_model
{
    [TestClass]
    public class when_overriding_app_settings : given_application_settings
    {

        protected override void Act()
        {
            var mergeSection = new EnvironmentalOverridesSection
                                   {
                                       EnvironmentName = "TestEnvironment",
                                       MergeElements =
                                           {    
                                               new EnvironmentalOverridesElement
                                                   {
                                                       LogicalParentElementPath = base.settings.Path
                                                   }       
                                           }
                                   };

            var appModel = base.Container.Resolve<ApplicationViewModel>();
            appModel.LoadEnvironment(mergeSection, string.Empty);
        }

        [TestMethod]
        public void then_appSettings_has_override_property()
        {
            Assert.IsTrue(settings.Properties.OfType<EnvironmentOverriddenElementProperty>().Any());
        }


        [TestMethod]
        public void then_appSettings_is_set_to_override()
        {
            var overridesProperty = settings.Properties.OfType<EnvironmentOverriddenElementProperty>().Single();
            Assert.IsTrue((bool)overridesProperty.Value);
        }

    }
}
