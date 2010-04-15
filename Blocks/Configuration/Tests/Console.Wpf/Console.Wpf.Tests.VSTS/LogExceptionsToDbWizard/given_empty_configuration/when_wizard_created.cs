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
using System.ComponentModel;
using System.Linq;
using Console.Wpf.Tests.VSTS.BlockSpecific.Logging;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Hosting;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Wizard;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Console.Wpf.Tests.VSTS.LogExceptionsToDbWizard.given_empty_configuration
{
    [TestClass]
    public class when_wizard_created : NewConfigurationSourceModelContext
    {
        private LogExceptionsToDatabase wizard;

        protected override void Arrange()
        {
            base.Arrange();
            GlobalResources.SetDictionary(new KeyedResourceDictionary());
        }

        protected override void Act()
        {
            wizard = Container.Resolve<LogExceptionsToDatabase>();
        }

        [TestMethod]
        public void then_steps_include_pick_exceptions()
        {
            Assert.IsInstanceOfType(wizard.Steps.ElementAt(0), typeof(PickExceptionStep));
        }

        [TestMethod]
        public void then_steps_include_select_database()
        {
            Assert.IsInstanceOfType(wizard.Steps.ElementAt(1), typeof (SelectDatabaseStep));
        }

        [TestMethod]
        public void then_title_is_retrieved_correctly()
        {
            var title = wizard.Title;
            Assert.IsFalse(string.IsNullOrEmpty(title));
        }
    }

    [TestClass]
    public class when_wizard_title_fails : ContainerContext
    {
        private WizardWithFailingTitle wizard;

        protected override void Arrange()
        {
            base.Arrange();
            GlobalResources.SetDictionary(new KeyedResourceDictionary());
        }

        protected override void Act()
        {
            wizard = Container.Resolve<WizardWithFailingTitle>();
        }

        [TestMethod]
        public void then_error_message_is_returned()
        {
            var title = wizard.Title;
            Assert.AreEqual("An error occurred retrieving the wizard's Title.", title);
        }
    }

    public class WizardWithFailingTitle : WizardModel
    {
        public WizardWithFailingTitle(IUIServiceWpf uiService) :
            base(uiService)
        { }

        protected override string GetWizardTitle()
        {
            throw new Exception();
        }
    }
}
