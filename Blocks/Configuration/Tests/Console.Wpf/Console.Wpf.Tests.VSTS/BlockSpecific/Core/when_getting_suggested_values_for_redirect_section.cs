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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Console.Wpf.Tests.VSTS.TestSupport;

namespace Console.Wpf.Tests.VSTS.BlockSpecific.Core
{
    [TestClass]
    public class when_getting_suggested_values_for_redirect_section : ContainerContext
    {
        ElementReferenceProperty sourceProperty;
        IEnumerable<string> suggestedValues;

        protected override void Arrange()
        {
            base.Arrange();

            ConfigurationSourceModel sourceModel = Container.Resolve<ConfigurationSourceModel>();
            var section = sourceModel.AddSection(ConfigurationSourceSection.SectionName, new ConfigurationSourceSection()
            {
                Sources = { { new FileConfigurationSourceElement("file1", "file1") },
                            { new FileConfigurationSourceElement("file2", "file1") },
                            { new SystemConfigurationSourceElement("system") }},
                RedirectedSections = {{new RedirectedSectionElement(){Name = "redirect" }}}
            });


            sourceProperty = (ElementReferenceProperty) section.GetDescendentsOfType<RedirectedSectionElement>().First().Property("SourceName");
        }

        protected override void Act()
        {
            suggestedValues = sourceProperty.SuggestedValues.Cast<string>();
        }

        [TestMethod]
        public void then_system_source_was_not_suggested()
        {
            Assert.IsFalse(suggestedValues.Contains("system"));
        }
    }
}
