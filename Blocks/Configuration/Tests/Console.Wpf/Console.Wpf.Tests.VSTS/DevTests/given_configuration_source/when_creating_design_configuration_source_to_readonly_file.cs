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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport;
using System.Configuration;

namespace Console.Wpf.Tests.VSTS.DevTests.given_configuration_source
{
    [TestClass]
    public class when_creating_design_configuration_source_to_readonly_file : ArrangeActAssert
    {
        private FileConfigurationSourceElement configurationSourceElement;
        private DesignConfigurationSource mainConfigurationSource;
        private IDesignConfigurationSource desingConfigurationSource;
        private string expectedFilePath;
        private string mainFilePath;
            
        protected override void Arrange()
        {
            base.Arrange();

            var resourceHelper = new ResourceHelper<ConfigFiles.ConfigFileLocator>();
            mainFilePath = resourceHelper.DumpResourceFileToDisk("empty.config", "ds_abs_ro_path");
            mainConfigurationSource = new DesignConfigurationSource(mainFilePath);

            var mainFileDirectory = Path.GetDirectoryName(mainFilePath);
            expectedFilePath = Path.Combine(mainFileDirectory, "absolutefile.config");

            configurationSourceElement = new FileConfigurationSourceElement("absolutefile", expectedFilePath);
            File.WriteAllText(expectedFilePath, "<configuration/>");
            File.SetAttributes(expectedFilePath, FileAttributes.ReadOnly);
        }

        protected override void Act()
        {
            desingConfigurationSource = configurationSourceElement.CreateDesignSource(mainConfigurationSource);
        }


        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void throws_when_adding_a_section()
        {
            desingConfigurationSource.AddLocalSection(ExceptionHandlingSettings.SectionName, new ExceptionHandlingSettings());
        }

        protected override void Teardown()
        {
            File.SetAttributes(expectedFilePath, FileAttributes.Normal);
            RemoveFiles();
        }

        private void RemoveFiles()
        {
            if (File.Exists(expectedFilePath)) File.Delete(expectedFilePath);
            if (File.Exists(mainFilePath)) File.Delete(mainFilePath);
        }

    }
}
