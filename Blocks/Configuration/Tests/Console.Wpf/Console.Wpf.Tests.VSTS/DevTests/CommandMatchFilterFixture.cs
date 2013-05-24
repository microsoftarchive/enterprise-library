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

using System.Diagnostics;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Commands;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services.PlatformProfile;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.Practices.Unity;

namespace Console.Wpf.Tests.VSTS.DevTests
{
    public class given_new_command_filter : ContainerContext
    {
        protected CommandMatchFilter filter;

        protected override void Arrange()
        {
            base.Arrange();

            this.filter = new CommandMatchFilter { };
        }

        [TestClass]
        public class when_filtering_type : given_new_command_filter
        {
            private bool match = false;

            protected override void Act()
            {
                var command = new TestCommandModel(Container.Resolve<IUIServiceWpf>());

                this.match = this.filter.Match(command);
            }

            [TestMethod]
            public void then_does_not_match()
            {
                Assert.IsFalse(match);
            }
        }
    }

    public class given_command_filter_configured_with_type_name : ContainerContext
    {
        protected CommandMatchFilter filter;

        protected override void Arrange()
        {
            base.Arrange();

            this.filter = new CommandMatchFilter { Name = typeof(TestCommandModel).AssemblyQualifiedName };
        }

        [TestClass]
        public class when_filtering_type : given_command_filter_configured_with_type_name
        {
            private bool match = false;

            protected override void Act()
            {
                var command = new TestCommandModel(Container.Resolve<IUIServiceWpf>());

                this.match = this.filter.Match(command);
            }

            [TestMethod]
            public void then_matches()
            {
                Assert.IsTrue(match);
           } 
        }

        [TestClass]
        public class when_filtering_derived_type : given_command_filter_configured_with_type_name
        {
            private bool match = false;

            protected override void Act()
            {
                var command = new AddLoggingBlockCommand(Container.Resolve<ConfigurationSourceModel>(),
                         new AddApplicationBlockCommandAttribute("sectionName", typeof(LoggingSettings)),
                         Container.Resolve<IUIServiceWpf>());

                this.match = this.filter.Match(command);
            }

            [TestMethod]
            public void then_does_not_match()
            {
                Assert.IsFalse(match);
            }
        }
    }
}
