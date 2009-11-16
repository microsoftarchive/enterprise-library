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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;

namespace Console.Wpf.Tests.VSTS.DevTests.given_configuration_source_model
{
    [TestClass]
    public class when_removing_section : ContainerContext
    {
        ConfigurationSourceModel configurationSourceModel;

        protected override void Arrange()
        {
            base.Arrange();

            configurationSourceModel = Container.Resolve<ConfigurationSourceModel>();
            configurationSourceModel.AddSection(LoggingSettings.SectionName, new LoggingSettings());
        }

        protected override void Act()
        {
            configurationSourceModel.Sections.First().Delete();
        }

        [TestMethod]
        public void then_section_is_removed_from_source_model()
        {
            Assert.AreEqual(0, configurationSourceModel.Sections.Count);
        }
    }
}
