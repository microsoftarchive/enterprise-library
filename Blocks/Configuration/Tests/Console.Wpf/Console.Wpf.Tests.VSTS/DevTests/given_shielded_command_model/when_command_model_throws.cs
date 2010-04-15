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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Console.Wpf.Tests.VSTS.DevTests.given_command_model
{
    public abstract class exeption_throwing_command_context : ArrangeActAssert
    {
        protected ExceptionThrowingCommandModel command;
        protected Mock<IUIServiceWpf> uiService;

        protected override void Arrange()
        {
            base.Arrange();

            uiService = new Mock<IUIServiceWpf>();
            uiService.Setup(x => x.ShowError(It.IsAny<Exception>(), It.IsAny<string>()))
                .Verifiable("ShowError not displayed.");

            command = new ExceptionThrowingCommandModel(uiService.Object);
        }
    }

    [TestClass]
    public class when_command_execute_throws : exeption_throwing_command_context
    {
        protected override void Act()
        {
            command.Execute(null);
        }

        [TestMethod]
        public void then_shows_dialog()
        {
            uiService.Verify();
        }
    }

    [TestClass]
    public class when_command_canexecute_throws : exeption_throwing_command_context
    {
        private bool canExecuteResult;

        protected override void Act()
        {
            canExecuteResult = command.CanExecute(null);
        }

        [TestMethod]
        public void then_error_message_is_shown()
        {
            uiService.Verify();
        }

        [TestMethod]
        public void then_canexecute_returns_false()
        {
            Assert.IsFalse(canExecuteResult);
        }
    }
    public class ExceptionThrowingCommandModel : CommandModel
    {
        public ExceptionThrowingCommandModel(IUIServiceWpf uiService) :
            base(
            new CommandAttribute(typeof(ExceptionThrowingCommandModel))
                {
                    CommandPlacement = CommandPlacement.ContextCustom,
                    Title = "ExceptionThrowingCommand"
                },
            uiService)
        {
        }

        protected override bool InnerCanExecute(object parameter)
        {
            throw new InvalidOperationException("TestCanExecuteException");
        }
        protected override void InnerExecute(object parameter)
        {
            throw new InvalidOperationException("TestException");
        }
    }
}
