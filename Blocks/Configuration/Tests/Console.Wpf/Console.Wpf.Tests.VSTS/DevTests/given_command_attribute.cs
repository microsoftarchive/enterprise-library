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

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Console.Wpf.Tests.VSTS.DevTests
{
    [TestClass]
    public class when_creating_a_command : ArrangeActAssert
    {
        private CommandAttribute attrib;
        private TestableCommandModel command;
        private Mock<IUIServiceWpf> uiServiceMock;

        protected override void Arrange()
        {
            base.Arrange();

            uiServiceMock = new Mock<IUIServiceWpf>();

            attrib = new CommandAttribute(typeof(TestableCommandModel))
                             {
                                 CommandPlacement = CommandPlacement.FileMenu,
                                 Title = "TestTitle",
                                 KeyGesture = "Ctrl+A"
                             };
        }

        protected override void Act()
        {
            command = new TestableCommandModel(attrib, uiServiceMock.Object);
        }

        [TestMethod]
        public void then_title_is_set_from_attribute()
        {
            Assert.AreEqual(attrib.Title, command.Title);
        }

        [TestMethod]
        public void then_command_placement_matches_attribute()
        {
            Assert.AreEqual(attrib.CommandPlacement, command.Placement);
        }

        [TestMethod]
        public void then_gesture_matches()
        {
            Assert.AreEqual(attrib.KeyGesture, command.KeyGesture);
        }

        public class TestableCommandModel : CommandModel
        {
            public TestableCommandModel(CommandAttribute attribute, IUIServiceWpf uiService)
                : base(attribute, uiService)
            {

            }
        }
    }


}
