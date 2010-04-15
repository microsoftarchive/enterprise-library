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
using System.Windows;
using Console.Wpf.Tests.VSTS.BlockSpecific.Logging;
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Hosting;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Wizard;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Database.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;

namespace Console.Wpf.Tests.VSTS.LogExceptionsToDbWizard.given_empty_configuration
{
    public abstract class WizardCompleteContext : NewConfigurationSourceModelContext
    {
        protected LogExceptionsToDatabase wizard;
        protected PickExceptionStep pickExceptionStep;
        protected SelectDatabaseStep selectDatabaseStep;

        protected override void Arrange()
        {
            base.Arrange();

            GlobalResources.SetDictionary(new KeyedResourceDictionary());

            wizard = Container.Resolve<LogExceptionsToDatabase>();
            pickExceptionStep = wizard.Steps.OfType<PickExceptionStep>().First();

            pickExceptionStep.Policy.Value = "MyPolicy";
            pickExceptionStep.ExceptionType.Value = typeof(System.Exception).AssemblyQualifiedName;

            selectDatabaseStep = wizard.Steps.OfType<SelectDatabaseStep>().First();
            selectDatabaseStep.Name.Value = "MyConnection";
            selectDatabaseStep.ConnectionString.Value = "AConnectionString";
            selectDatabaseStep.DatabaseProvider.Value = "ADatabaseProvider";
        }
    }


    [TestClass]
    public class when_wizard_executes_all_steps : WizardCompleteContext
    {
        protected override void Arrange()
        {
            base.Arrange();

            wizard = Container.Resolve<LogExceptionsToDatabase>();
            pickExceptionStep = wizard.Steps.OfType<PickExceptionStep>().First();

            pickExceptionStep.Policy.Value = "MyPolicy";
            pickExceptionStep.ExceptionType.Value = typeof(System.Exception).AssemblyQualifiedName;

            selectDatabaseStep = wizard.Steps.OfType<SelectDatabaseStep>().First();
            selectDatabaseStep.Name.Value = "MyConnection";
            selectDatabaseStep.ConnectionString.Value = "AConnectionString";
            selectDatabaseStep.DatabaseProvider.Value = "ADatabaseProvider";
        }

        protected override void Act()
        {
            wizard.Finish();
        }

        [TestMethod]
        public void then_database_trace_listener_connects_to_selected_database()
        {
            ConfigurationSourceModel.Logging()
                .HasListener("Database Trace Listener")
                .OfConfigurationType<FormattedDatabaseTraceListenerData>()
                .WithProperty(x => x.DatabaseInstanceName == selectDatabaseStep.Name.Value.ToString());
        }
    }
}
