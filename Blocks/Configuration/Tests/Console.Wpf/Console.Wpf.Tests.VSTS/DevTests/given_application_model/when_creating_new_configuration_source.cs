using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Console.Wpf.Tests.VSTS.DevTests.given_shell_service;
using Moq;
using System.Windows;

namespace Console.Wpf.Tests.VSTS.DevTests.given_application_model
{

    [TestClass]
    public class when_creating_new_configuration_source : given_dirty_application_model
    {
        protected override void Arrange()
        {
            base.Arrange();

            base.UIServiceMock.Setup(x=> x.ShowMessageWpf(It.IsAny<string>(), It.IsAny<string>(), MessageBoxButton.YesNoCancel))
                              .Returns( MessageBoxResult.Yes)
                              .Verifiable();
        }

        protected override void Act()
        {
            ApplicationModel.New();
        }

        [TestMethod]
        public void then_confirmation_message_was_shown()
        {
            UIServiceMock.Verify();
        }

        [TestMethod]
        public void then_ui_is_not_dirty()
        {
            Assert.IsFalse(ApplicationModel.IsDirty);
        }
    }
}
