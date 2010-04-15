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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.Unity;

namespace Console.Wpf.Tests.VSTS.DevTests.given_selected_source_and_redirected_sections
{
    public abstract class given_selected_source_and_redirects : ContainerContext
    {
        protected SectionViewModel SourcesSectionViewModel;

        protected override void Arrange()
        {
            base.Arrange();

            ConfigurationSourceSection sourcesSection = new ConfigurationSourceSection
            {
                SelectedSource = "selected",
                RedirectedSections = { { new RedirectedSectionElement { SourceName = "redirected", Name="loggingConfiguration" } } },
                Sources = { { new FileConfigurationSourceElement { Name = "selected" } }, { new FileConfigurationSourceElement { Name = "redirected" } } }
            };

            ConfigurationSourceModel sourceModel = Container.Resolve<ConfigurationSourceModel>();
            sourceModel.AddSection(ConfigurationSourceSection.SectionName, sourcesSection);

            SourcesSectionViewModel = sourceModel.Sections.Where(x => x.SectionName == ConfigurationSourceSection.SectionName).Single();
        }
    }
}
