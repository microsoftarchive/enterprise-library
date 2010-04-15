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
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Console.Wpf.Tests.VSTS.DevTests.given_throwing_command_model
{
    public abstract class exception_throwing_command_context : ArrangeActAssert
    {
        protected CommandModel command;
        protected Mock<IUIServiceWpf> uiService;

        protected override void Arrange()
        {
            uiService = new Mock<IUIServiceWpf>();
            uiService.Setup(
                x =>
                x.ShowError(It.IsAny<Exception>(), It.IsAny<string>()))
             .Verifiable("Dialog not invoked.");


            command = new ExceptionThrowingCommand(uiService.Object);
        }

        private class ExceptionThrowingCommand : CommandModel
        {
            public ExceptionThrowingCommand(IUIServiceWpf uiService)
                : base(uiService)
            {
            }

            protected override bool InnerCanExecute(object parameter)
            {
                throw new Exception("CanExecute Exception");
            }

            protected override void InnerExecute(object parameter)
            {
                throw new Exception("TestCommandException");
            }
        }
    }

    [TestClass]
    public class when_exception_is_thrown_during_execute : exception_throwing_command_context
    {

        protected override void Arrange()
        {
            base.Arrange();

            uiService.Setup(
                x =>
                x.ShowError(It.IsAny<Exception>(), It.IsAny<string>()))
             .Verifiable("Dialog not invoked.");
        }

        protected override void Act()
        {
            command.Execute(null);
        }

        [TestMethod]
        public void then_dialog_invoked()
        {
            uiService.Verify();
        }
    }


    [TestClass]
    public class when_exception_is_thrown_during_canexecute : exception_throwing_command_context
    {
        private bool canExecuteResult;

        protected override void Arrange()
        {
            base.Arrange();

            uiService.Setup(
                x =>
                x.ShowError(It.IsAny<Exception>(), It.IsAny<string>()))
             .Verifiable("Dialog not invoked.");
        }

        protected override void Act()
        {
            canExecuteResult = command.CanExecute(null);
        }

        [TestMethod]
        public void then_dialog_invoked()
        {
            uiService.Verify();
        }

        [TestMethod]
        public void then_can_execute_returns_false()
        {
            Assert.IsFalse(canExecuteResult);
        }
    }
}
