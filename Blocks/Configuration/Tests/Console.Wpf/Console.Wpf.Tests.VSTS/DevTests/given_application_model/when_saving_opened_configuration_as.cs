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
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport;
using Console.Wpf.Tests.VSTS.ConfigFiles;
using System.Configuration;
using System.Xml;

namespace Console.Wpf.Tests.VSTS.DevTests.given_application_model
{
    [TestClass]
    public class when_saving_opened_configuration_as : ContainerContext
    {
        IApplicationModel applicationModel;
        string saveAsTargetFileName;

        protected override void Arrange()
        {
            base.Arrange();

            var resources = new ResourceHelper<ConfigFileLocator>();
            var localFilename = resources.DumpResourceFileToDisk("systemweb_and_el.config");
            
            applicationModel = Container.Resolve<IApplicationModel>();
            applicationModel.Load(localFilename);

            saveAsTargetFileName = localFilename.Replace(".config", ".saveas.config");
        }

        protected override void Act()
        {
            applicationModel.Save(saveAsTargetFileName);
        }

        [TestMethod]
        public void then_non_enterprise_library_sections_are_also_saved_as()
        {
            XmlDocument configurationAsXml = new XmlDocument();
            configurationAsXml.Load(saveAsTargetFileName);

            Assert.IsNotNull(configurationAsXml.SelectSingleNode("/configuration/system.web"));
        }
    }
}
