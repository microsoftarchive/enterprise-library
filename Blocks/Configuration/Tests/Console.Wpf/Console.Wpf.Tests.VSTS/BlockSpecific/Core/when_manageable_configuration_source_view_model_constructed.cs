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
using System.IO;
using System.Linq;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Console.Wpf.Tests.VSTS.BlockSpecific.Core
{
    [TestClass]
    public class when_manageable_configuration_source_view_model_constructed : ContainerContext
    {
        private string targetFilePath;

        protected override void Arrange()
        {
            base.Arrange();

            targetFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "manageableSource.config");
            File.Create(targetFilePath).Dispose();
            File.WriteAllText(targetFilePath, "<configuration/>");

            var source = new DesignConfigurationSource(targetFilePath);

            var section =
                new ConfigurationSourceSection
                {
                    SelectedSource = "manageable",
                    Sources =
                    {
                        new ManageableConfigurationSourceElement
                        {
                            Name = "manageable",
                            FilePath = "file.config",
                        }
                    }
                };

            source.AddLocalSection(ConfigurationSourceSection.SectionName, section);


            var sourceModel = this.Container.Resolve<ConfigurationSourceModel>();
            sourceModel.Load(source);

            SectionViewModel =
                sourceModel.Sections.Where(x => x.ConfigurationType == typeof(ConfigurationSourceSection)).Single();

            ConfigurationSourceViewModel = SectionViewModel.GetDescendentsOfType<ManageableConfigurationSourceElement>().FirstOrDefault();
        }

        protected SectionViewModel SectionViewModel { get; private set; }
        protected ElementViewModel ConfigurationSourceViewModel { get; private set; }

        [TestMethod]
        public void then_source_model_is_custom_view_model()
        {
            Assert.IsInstanceOfType(ConfigurationSourceViewModel, typeof(ManageableConfigurationSourceViewModel));
        }

        protected override void Teardown()
        {
            if (File.Exists(targetFilePath)) File.Delete(targetFilePath);
        }
    }
}
