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
using Console.Wpf.Tests.VSTS.Contexts;
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.Logging;

namespace Console.Wpf.Tests.VSTS.BlockSpecific.Logging.given_logging_configuration
{
    public abstract class given_email_trace_listener : LoggingConfigurationContext
    {
        protected ElementViewModel EmailTraceListener;
        protected override void Arrange()
        {
            base.Arrange();

            LoggingSectionViewModel = SectionViewModel.CreateSection(Container, LoggingSettings.SectionName, base.LoggingSection);
            EmailTraceListener = LoggingSectionViewModel.GetDescendentsOfType<EmailTraceListenerData>().First();
        }

        protected SectionViewModel LoggingSectionViewModel { get; private set; }
    }

    [TestClass]
    public class when_retrieving_password_bindable  : given_email_trace_listener
    {
        private Property passwordProperty;

        protected override void Act()
        {
            passwordProperty = EmailTraceListener.Property("Password");
        }

        [TestMethod]
        public void then_uses_custom_viewmodel()
        {
            Assert.AreEqual(typeof (EmailTraceListenerPasswordProperty), passwordProperty.GetType());
        }

        [TestMethod]
        public void then_bindable_is_masked_password_bindable()
        {
            Assert.AreEqual(typeof (MaskedPasswordBindable), passwordProperty.BindableProperty.GetType());
        }
    }

    [TestClass]
    public class when_validated_with_auth_mode_usernameandpassword_and_no_credentials : given_email_trace_listener
    {
        private Property authenticationMode;
        private ValidationResult result;

        protected override void Arrange()
        {
            base.Arrange();

            authenticationMode = EmailTraceListener.Property("AuthenticationMode");
            authenticationMode.Value = EmailAuthenticationMode.UserNameAndPassword;
        }

        protected override void Act()
        {
            EmailTraceListener.Validate();
            result = EmailTraceListener.ValidationResults.Where(e => e.Message.Contains("Supply a user name")).FirstOrDefault();
        }

        [TestMethod]
        public void then_has_error_to_supply_userandpassword()
        {
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void then_error_level_is_error()
        {
            Assert.IsTrue(result.IsError);
        }

        [TestMethod]
        public void then_only_one_error_appears()
        {
            Assert.AreEqual(1, EmailTraceListener.ValidationResults.Count());
        }
    }

    [TestClass]
    public class when_validated_with_auth_mode_usernameandpassword_and_credentials : given_email_trace_listener
    {
        private Property authenticationMode;
        private ValidationResult result;

        protected override void Arrange()
        {
            base.Arrange();

            authenticationMode = EmailTraceListener.Property("AuthenticationMode");
            authenticationMode.Value = EmailAuthenticationMode.UserNameAndPassword;
            EmailTraceListener.Property("UserName").Value = "TestUser";
        }

        protected override void Act()
        {
            EmailTraceListener.Validate();
            result = EmailTraceListener.ValidationResults.Where(e => e.Message.Contains("Supply a user name")).FirstOrDefault();
        }

        [TestMethod]
        public void then_has_no_error_to_supply_userandpassword()
        {
            Assert.IsNull(result);
        }

        [TestMethod]
        public void then_supplies_warning_to_encrypt()
        {
            Assert.IsTrue(EmailTraceListener.ValidationResults.Any(e => e.Message.Contains("Be sure to encrypt")));
        }

    }

    [TestClass]
    public class when_validated_with_auth_mode_usernameandpassword_and_credentials_and_encryption : given_email_trace_listener
    {
        private Property authenticationMode;

        protected override void Arrange()
        {
            base.Arrange();

            authenticationMode = EmailTraceListener.Property("AuthenticationMode");
            authenticationMode.Value = EmailAuthenticationMode.UserNameAndPassword;
            EmailTraceListener.Property("UserName").Value = "TestUser";

            LoggingSectionViewModel.ProtectionProviderProperty.Value =
                LoggingSectionViewModel.ProtectionProviderProperty.SuggestedValues.Last();
            
        }

        protected override void Act()
        {
            EmailTraceListener.Validate();
        }

        [TestMethod]
        public void then_encryption_warning_not_displayed()
        {
            Assert.IsFalse(EmailTraceListener.ValidationResults.Any(e => e.Message.Contains("Be sure to encrypt")));
        }
    }

    [TestClass]
    public class when_validated_without_usernameandpassword_and_credentials_supplied : given_email_trace_listener
    {
        private Property authenticationMode;
        private ValidationResult result;

        protected override void Arrange()
        {
            base.Arrange();

            authenticationMode = EmailTraceListener.Property("AuthenticationMode");
            authenticationMode.Value = EmailAuthenticationMode.WindowsCredentials;
            EmailTraceListener.Property("UserName").Value = "TestUser";
        }

        protected override void Act()
        {
            EmailTraceListener.Validate();
            result = EmailTraceListener.ValidationResults.Where(e => e.Message.Contains("user name and password are not needed")).FirstOrDefault();
        }

        [TestMethod]
        public void then_shows_error_that_you_do_not_need_username_and_assword()
        {
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void then_error_level_is_warning()
        {
            Assert.IsTrue(result.IsWarning);
        }

        [TestMethod]
        public void then_only_one_error_appears()
        {
            Assert.AreEqual(1, EmailTraceListener.ValidationResults.Count());
        }
    }
}
