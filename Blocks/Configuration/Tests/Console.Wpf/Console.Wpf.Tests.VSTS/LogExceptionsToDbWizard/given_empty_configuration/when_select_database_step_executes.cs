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
using System.Configuration;
using System.Linq;
using System.Text;
using Console.Wpf.Tests.VSTS.BlockSpecific.Logging;
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Wizard;
using Microsoft.Practices.Unity;

namespace Console.Wpf.Tests.VSTS.LogExceptionsToDbWizard.given_empty_configuration
{
    [TestClass]
    public class when_select_database_step_executes : NewConfigurationSourceModelContext
    {
        private SelectDatabaseStep step;

        protected override void Arrange()
        {
            base.Arrange();

            step = Container.Resolve<SelectDatabaseStep>();

            step.Name.Value = "MyConnection";
            step.ConnectionString.Value = "SomeConnectionString";
            step.DatabaseProvider.Value = "ADatabaseProvider";
        }

        protected override void Act()
        {
            step.Execute();
        }

        [TestMethod]
        public void then_database_connection_string_matches()
        {
            ConfigurationSourceModel
                .DatabaseConfiguration()
                .WithConnectionString(step.Name.Value.ToString())
                .MatchesConnectionString(step.ConnectionString.Value.ToString())
                .MatchesProvider(step.DatabaseProvider.Value.ToString());
        }
    }
}
