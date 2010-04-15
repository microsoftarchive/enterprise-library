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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.Unity;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;

namespace Console.Wpf.Tests.VSTS.DevTests.given_selected_source_and_parent_source
{
    public abstract class given_selected_source_and_parent : ContainerContext
    {
        protected SectionViewModel SourcesSectionViewModel;

        protected override void Arrange()
        {
            base.Arrange();

            ConfigurationSourceSection sourcesSection = new ConfigurationSourceSection
            {
                SelectedSource = "selected",
                ParentSource = "parent",
                Sources = { { new FileConfigurationSourceElement { Name = "selected" } }, { new FileConfigurationSourceElement { Name = "parent" } } }
            };

            ConfigurationSourceModel sourceModel = Container.Resolve<ConfigurationSourceModel>();
            sourceModel.AddSection(ConfigurationSourceSection.SectionName, sourcesSection);

            SourcesSectionViewModel = sourceModel.Sections.Where(x => x.SectionName == ConfigurationSourceSection.SectionName).Single();
        }
    }
}
