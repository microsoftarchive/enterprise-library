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
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Console.Wpf.Tests.VSTS.TestSupport;

namespace Console.Wpf.Tests.VSTS.BlockSpecific.Data.given_data_configuration
{
    [TestClass]
    public class when_executing_add_database_section_command : ContainerContext
    {

        protected override void Act()
        {
            AddDatabaseBlockCommand addDataBlockCommand = Container.Resolve<AddDatabaseBlockCommand>(
                new DependencyOverride<AddApplicationBlockCommandAttribute>(
                    new AddApplicationBlockCommandAttribute("connectionStrings", typeof(ConnectionStringSettings))));

            addDataBlockCommand.Execute(null);
        }

        [TestMethod]
        public void then_database_block_has_default_connection_string()
        {
            var configurationSourceModel = Container.Resolve<ConfigurationSourceModel>();
            SectionViewModel databaseSection = configurationSourceModel.Sections.Where(x=>x.SectionName == "connectionStrings").First();

            ElementViewModel addedConnectionString = databaseSection.GetDescendentsOfType<ConnectionStringSettings>().FirstOrDefault();

            string defaultDatabase = (string)databaseSection.Property("DefaultDatabase").Value;
            Assert.AreEqual(addedConnectionString.Property("Name").Value, defaultDatabase);
        }
    }
}
