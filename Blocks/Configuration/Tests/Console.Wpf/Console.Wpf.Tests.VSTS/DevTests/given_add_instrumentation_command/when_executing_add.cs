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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;

namespace Console.Wpf.Tests.VSTS.DevTests.given_add_instrumentation_command
{
    [TestClass]
    public class when_executing_add : ContainerContext
    {
        private AddInstrumentationBlockCommand command;
        private ConfigurationSourceModel configurationSource;
        private SectionViewModel addedSection;

        protected override void Arrange()
        {
            base.Arrange();
            
            configurationSource = Container.Resolve<ConfigurationSourceModel>();
            command = Container.Resolve<AddInstrumentationBlockCommand>(
                new ParameterOverride("commandAttribute", new AddApplicationBlockCommandAttribute(
                                                            InstrumentationConfigurationSection.SectionName,
                                                            typeof (InstrumentationConfigurationSection))));
        }

        protected override void Act()
        {
            command.Execute(null);
            addedSection =
                configurationSource.Sections.Where(
                    s => s.ConfigurationType == typeof (InstrumentationConfigurationSection)).Single();

        }
        [TestMethod]
        public void then_added_section_is_not_expanded()
        {
            Assert.IsFalse(addedSection.IsExpanded);
        }
    }
}
