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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using System.Configuration;

namespace Console.Wpf.Tests.VSTS.BlockSpecific.Data
{
    [TestClass]
    public class when_creating_new_database_section : ContainerContext
    {
        SectionViewModel connectionStringsSection;
        protected override void Act()
        {
            ConfigurationSourceModel sourceModel = Container.Resolve<ConfigurationSourceModel>();
            connectionStringsSection = sourceModel.AddSection(DataAccessDesignTime.ConnectionStringSettingsSectionName, new ConnectionStringsSection());
        }

        [TestMethod]
        public void then_default_database_property_has_validation_warning()
        {
            Property defaultDatabaseProperty = connectionStringsSection.Property("DefaultDatabase");

            Assert.AreEqual(1, defaultDatabaseProperty.ValidationResults.Where(x => x.IsWarning).Count());
        }

        [TestMethod]
        public void then_logical_parent_element_for_default_database_is_connectionstrings_section()
        {
            Property defaultDatabaseProperty = connectionStringsSection.Property("DefaultDatabase");

            Assert.AreEqual(typeof(ConnectionStringsSection), ((ILogicalPropertyContainerElement)defaultDatabaseProperty).ContainingElement.ConfigurationType);
        }
    }
}
