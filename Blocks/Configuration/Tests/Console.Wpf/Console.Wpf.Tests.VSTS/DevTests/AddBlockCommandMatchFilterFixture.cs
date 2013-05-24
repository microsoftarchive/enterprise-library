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
    public class given_new_add_block_command_filter : ContainerContext
    {
        protected AddBlockCommandMatchFilter filter;

        protected override void Arrange()
        {
            base.Arrange();

            this.filter = new AddBlockCommandMatchFilter { };
        }

        [TestClass]
        public class when_filtering_type : given_new_add_block_command_filter
        {
            private bool match = false;

            protected override void Act()
            {
                var command = new AddApplicationBlockCommand(Container.Resolve<ConfigurationSourceModel>(),
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

        [TestClass]
        public class when_command_is_command_model_instance : given_new_add_block_command_filter
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

    public class given_add_block_command_filter_configured_with_type_name_and_no_section_name : ContainerContext
    {
        protected AddBlockCommandMatchFilter filter;
        protected AddBlockCommandMatchFilter filterOnCommandModel;

        protected override void Arrange()
        {
            base.Arrange();

            this.filter = new AddBlockCommandMatchFilter { Name = typeof(AddApplicationBlockCommand).AssemblyQualifiedName };
            this.filterOnCommandModel = new AddBlockCommandMatchFilter { Name = typeof(TestCommandModel).AssemblyQualifiedName };
        }

        [TestClass]
        public class when_filtering_type : given_add_block_command_filter_configured_with_type_name_and_no_section_name
        {
            private bool match = false;

            protected override void Act()
            {
                var command = new AddApplicationBlockCommand(Container.Resolve<ConfigurationSourceModel>(),
                                         new AddApplicationBlockCommandAttribute("sectionName", typeof(LoggingSettings)),
                                         Container.Resolve<IUIServiceWpf>());

                this.match = this.filter.Match(command);
            }

            [TestMethod]
            public void then_matches()
            {
                Assert.IsTrue(match);
            }
        }

        [TestClass]
        public class when_filtering_derived_type : given_add_block_command_filter_configured_with_type_name_and_no_section_name
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

        [TestClass]
        public class when_command_is_command_model_instance : given_add_block_command_filter_configured_with_type_name_and_no_section_name
        {
            private bool match = false;

            protected override void Act()
            {
                var command = new TestCommandModel(Container.Resolve<IUIServiceWpf>());

                this.match = this.filterOnCommandModel.Match(command);
            }

            [TestMethod]
            public void then_matches()
            {
                Assert.IsTrue(match);
            }
        }
    }

    public class given_add_block_command_filter_configured_with_no_type_name_and_section_name : ContainerContext
    {
        protected AddBlockCommandMatchFilter filter;
        protected string TestSectionName = "sectionName";

        protected override void Arrange()
        {
            base.Arrange();

            this.filter = new AddBlockCommandMatchFilter { SectionName = TestSectionName  };
        }

        [TestClass]
        public class when_section_name_is_equal : given_add_block_command_filter_configured_with_no_type_name_and_section_name
        {
            private bool match = false;

            protected override void Act()
            {
                var command = new AddLoggingBlockCommand(Container.Resolve<ConfigurationSourceModel>(),
                     new AddApplicationBlockCommandAttribute(TestSectionName, typeof(LoggingSettings)),
                     Container.Resolve<IUIServiceWpf>());

                this.match = this.filter.Match(command);
            }

            [TestMethod]
            public void then_matches()
            {
                Assert.IsTrue(match);
            }
        }

        [TestClass]
        public class when_section_name_is_different : given_add_block_command_filter_configured_with_no_type_name_and_section_name
        {
            private bool match = false;

            protected override void Act()
            {
                var command = new AddLoggingBlockCommand(Container.Resolve<ConfigurationSourceModel>(),
                     new AddApplicationBlockCommandAttribute("otherSectionName", typeof(LoggingSettings)),
                     Container.Resolve<IUIServiceWpf>());

                this.match = this.filter.Match(command);
            }

            [TestMethod]
            public void then_does_not_match()
            {
                Assert.IsFalse(match);
            }
        }

        [TestClass]
        public class when_command_is_command_model_instance : given_add_block_command_filter_configured_with_no_type_name_and_section_name
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

    public class given_add_block_command_filter_configured_with_type_name_and_section_name : ContainerContext
    {
        protected AddBlockCommandMatchFilter filter;
        protected AddBlockCommandMatchFilter filterOnCommandModel;
        protected const string SectionName1 = "sectionName1";
        protected const string SectionName2 = "sectionName2";

        protected override void Arrange()
        {
            base.Arrange();

            this.filter = new AddBlockCommandMatchFilter { SectionName = SectionName1, Name = typeof(AddLoggingBlockCommand).AssemblyQualifiedName };
            this.filterOnCommandModel = new AddBlockCommandMatchFilter { SectionName = SectionName1, Name = typeof(TestCommandModel).AssemblyQualifiedName };
        }

        [TestClass]
        public class when_section_name_is_equal_but_type_different : given_add_block_command_filter_configured_with_type_name_and_section_name
        {
            private bool match = false;

            protected override void Act()
            {
                var command = new AddApplicationBlockCommand(Container.Resolve<ConfigurationSourceModel>(),
                                                         new AddApplicationBlockCommandAttribute(SectionName1, typeof(LoggingSettings)),
                                                         Container.Resolve<IUIServiceWpf>());

                this.match = this.filter.Match(command);
            }

            [TestMethod]
            public void then_does_not_matches()
            {
                Assert.IsFalse(match);
            }
        }

        [TestClass]
        public class when_section_name_is_different_but_type_is_same : given_add_block_command_filter_configured_with_type_name_and_section_name
        {
            private bool match = false;

            protected override void Act()
            {
                var command = new AddLoggingBlockCommand(Container.Resolve<ConfigurationSourceModel>(),
                                                         new AddApplicationBlockCommandAttribute(SectionName2, typeof(LoggingSettings)),
                                                         Container.Resolve<IUIServiceWpf>());

                this.match = this.filter.Match(command);
            }

            [TestMethod]
            public void then_does_not_matches()
            {
                Assert.IsFalse(match);
            }
        }


        [TestClass]
        public class when_section_name_and_type_are_same : given_add_block_command_filter_configured_with_type_name_and_section_name
        {
            private bool match = false;

            protected override void Act()
            {
                var command = new AddApplicationBlockCommand(Container.Resolve<ConfigurationSourceModel>(),
                                         new AddApplicationBlockCommandAttribute(SectionName1, typeof(LoggingSettings)),
                                         Container.Resolve<IUIServiceWpf>());
                
                this.match = this.filter.Match(command);
            }

            [TestMethod]
            public void then_matches()
            {
                Assert.IsFalse(match);
            }
        }

        [TestClass]
        public class when_command_is_command_model_instance : given_add_block_command_filter_configured_with_type_name_and_section_name
        {
            private bool match = false;

            protected override void Act()
            {
                var command = new TestCommandModel(Container.Resolve<IUIServiceWpf>());

                this.match = this.filterOnCommandModel.Match(command);
            }

            [TestMethod]
            public void then_matches()
            {
                Assert.IsTrue(match);
            }
        }
    }
}
