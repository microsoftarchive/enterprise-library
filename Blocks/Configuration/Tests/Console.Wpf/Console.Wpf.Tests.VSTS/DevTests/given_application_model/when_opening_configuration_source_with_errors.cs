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
using System.Configuration;
using System.IO;
using System.Windows;
using Console.Wpf.Tests.VSTS.DevTests.given_shell_service;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Win32;
using Moq;


namespace Console.Wpf.Tests.VSTS.DevTests.given_application_model
{
    [TestClass]
    public class when_opening_configuration_source_with_errors : given_clean_application_model
    {
        private bool waitClosed;

        protected override void Arrange()
        {
            base.Arrange();
            string errorSectionConfigurationPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "configuration_error.config");

            UIServiceMock.Setup(x => x.ShowFileDialog(It.IsAny<OpenFileDialog>()))
                         .Returns(new FileDialogResult { DialogResult = true, FileName = errorSectionConfigurationPath });
            UIServiceMock.Setup(x => x.ShowWindow(It.IsAny<Window>())).Callback<Window>((w) => w.Closed += (o, e) => waitClosed = true);

            UIServiceMock.Setup(x => x.ShowError(It.IsAny<ConfigurationErrorsException>(), It.IsAny<string>()))
                .Verifiable();

        }

        protected override void Act()
        {
            ApplicationModel.OpenConfigurationSource();
        }

        [TestMethod]
        public void then_error_was_shown()
        {
            UIServiceMock.Verify();
        }

        [TestMethod]
        public void then_wiat_window_shown()
        {
            UIServiceMock.Verify(x => x.ShowWindow(It.IsAny<Window>()));
        }

        [TestMethod]
        public void then_wait_window_closed()
        {
            Assert.IsTrue(waitClosed);
        }

    }
}
