//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.Configuration
{

    public abstract class Given_EmailListenerInConfigurationSourceBuilder : Given_LoggingCategorySourceInConfigurationSourceBuilder
    {
        protected ILoggingConfigurationSendToEmailTraceListener EmailTraceListenerBuilder;
        private string emailListenerName = "email trace listener";

        protected override void Arrange()
        {
            base.Arrange();

            EmailTraceListenerBuilder = base.CategorySourceBuilder.SendTo.Email(emailListenerName);
        }

        protected EmailTraceListenerData GetEmailTraceListenerData()
        {
            return base.GetLoggingConfiguration().TraceListeners.OfType<EmailTraceListenerData>()
                .Where(x => x.Name == emailListenerName).First();
        }
    }

    [TestClass]
    public class When_CallingSendToEmailListenerPassingEmptyName : Given_LoggingCategorySourceInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_SendToEmail_ThrowsArgumentException()
        {
            CategorySourceBuilder.SendTo.Email(null);
        }
    }

    [TestClass]
    public class When_CallingSendToEmailListenerOnLogToCategoryConfigurationBuilder : Given_EmailListenerInConfigurationSourceBuilder
    {
        protected override void Arrange()
        {
            base.Arrange();
        }

        [TestMethod]
        public void ThenSmtpServerPortIs25()
        {
            Assert.AreEqual(25, GetEmailTraceListenerData().SmtpPort);
        }

        [TestMethod]
        public void ThenSmtpServerIsLocalhost()
        {
            Assert.AreEqual("127.0.0.1", GetEmailTraceListenerData().SmtpServer);
        }

        [TestMethod]
        public void ThenTraceOptionsIsNone()
        {
            Assert.AreEqual(TraceOptions.None, GetEmailTraceListenerData().TraceOutputOptions);
        }

        [TestMethod]
        public void ThenFilterIsAll()
        {
            Assert.AreEqual(SourceLevels.All, GetEmailTraceListenerData().Filter);
        }

        [TestMethod]
        public void ThenCategortyContainsTraceListenerReference()
        {
            Assert.AreEqual(GetEmailTraceListenerData().Name, GetTraceSourceData().TraceListeners.First().Name);
        }

        [TestMethod]
        public void ThenLoggingConfigurationContainsTraceListener()
        {
            Assert.IsTrue(GetLoggingConfiguration().TraceListeners.OfType<EmailTraceListenerData>().Any());
        }

        [TestMethod]
        public void ThenProviderTypeisSetToEmailTraceListener()
        {
            Assert.AreEqual(typeof(EmailTraceListener), GetEmailTraceListenerData().Type);
        }
    }


    [TestClass]
    public class When_SettingSubjectLineEnderForEmailTraceListener : Given_EmailListenerInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.EmailTraceListenerBuilder.WithSubjectEnd("-log");
        }

        [TestMethod]
        public void ThenConfigurationReflectsSubjectLineEnder()
        {
            Assert.AreEqual("-log", base.GetEmailTraceListenerData().SubjectLineEnder);
        }
    }

    [TestClass]
    public class When_SettingSubjectLineStarterForEmailTraceListener : Given_EmailListenerInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.EmailTraceListenerBuilder.WithSubjectStart("log-");
        }

        [TestMethod]
        public void ThenConfigurationReflectsSubjectLineStarter()
        {
            Assert.AreEqual("log-", base.GetEmailTraceListenerData().SubjectLineStarter);
        }
    }

    [TestClass]
    public class When_SettingFromEmailForEmailTraceListener : Given_EmailListenerInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.EmailTraceListenerBuilder.From("from@test.nl");
        }

        [TestMethod]
        public void ThenConfigurationReflectsFromAddress()
        {
            Assert.AreEqual("from@test.nl", base.GetEmailTraceListenerData().FromAddress);
        }
    }

    [TestClass]
    public class When_SettingFromEmailForEmailTraceListenerToNull : Given_EmailListenerInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_From_ThrowsArgumentException()
        {
            base.EmailTraceListenerBuilder.From(null);
        }
    }

    [TestClass]
    public class When_SettingToEmailForEmailTraceListener : Given_EmailListenerInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.EmailTraceListenerBuilder.To("to@test.nl");
        }

        [TestMethod]
        public void ThenConfigurationReflectsRecipient()
        {
            Assert.AreEqual("to@test.nl", base.GetEmailTraceListenerData().ToAddress);
        }
    }


    [TestClass]
    public class When_SettingToEmailForEmailTraceListenerToNull : Given_EmailListenerInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_To_ThrowsArgumentException()
        {
            base.EmailTraceListenerBuilder.To(null);
        }
    }

    [TestClass]
    public class When_SettingSmtpServerPortForEmailTraceListener : Given_EmailListenerInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.EmailTraceListenerBuilder.UsingSmtpServerPort(52);
        }

        [TestMethod]
        public void ThenConfigurationReflectsSmtpServerPort()
        {
            Assert.AreEqual(52, base.GetEmailTraceListenerData().SmtpPort);
        }
    }

    [TestClass]
    public class When_SettingSmtpServerForEmailTraceListener : Given_EmailListenerInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.EmailTraceListenerBuilder.UsingSmtpServer("smtpServer");
        }

        [TestMethod]
        public void ThenConfigurationReflectsSmtpServer()
        {
            Assert.AreEqual("smtpServer", base.GetEmailTraceListenerData().SmtpServer);
        }
    }

    [TestClass]
    public class When_SettingSharedFormatterForEmailTraceListener : Given_EmailListenerInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.EmailTraceListenerBuilder.FormatWithSharedFormatter("formatter");
        }

        [TestMethod]
        public void ThenConfigurationReflectsTraceOption()
        {
            Assert.AreEqual("formatter", base.GetEmailTraceListenerData().Formatter);
        }
    }

    [TestClass]
    public class When_SettingTraceOptionForEmailTraceListener : Given_EmailListenerInConfigurationSourceBuilder
    {
        TraceOptions trOption;
        protected override void Act()
        {
            trOption = TraceOptions.Callstack | TraceOptions.DateTime;
            base.EmailTraceListenerBuilder.WithTraceOptions(trOption);
        }

        [TestMethod]
        public void ThenConfigurationReflectsTraceOption()
        {
            Assert.AreEqual(trOption, base.GetEmailTraceListenerData().TraceOutputOptions);
        }
    }

    [TestClass]
    public class When_SettingFilterForEmailTraceListener : Given_EmailListenerInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.EmailTraceListenerBuilder.Filter(SourceLevels.Error);
        }

        [TestMethod]
        public void ThenConfigurationReflectsTraceOption()
        {
            Assert.AreEqual(SourceLevels.Error, base.GetEmailTraceListenerData().Filter);
        }
    }

    [TestClass]
    public class When_SettingNullFormatterBuilderForEmailTraceListener : Given_EmailListenerInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Then_FormatWith_ThrowsArgumentNullException()
        {
            base.EmailTraceListenerBuilder.FormatWith(null);
        }
    }

    [TestClass]
    public class When_SettingNewFormatterForEmailTraceListener : Given_EmailListenerInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.EmailTraceListenerBuilder.FormatWith(new FormatterBuilder().TextFormatterNamed("Text Formatter"));
        }

        [TestMethod]
        public void ThenConfigurationReflectsTraceOption()
        {
            Assert.AreEqual("Text Formatter", base.GetEmailTraceListenerData().Formatter);
            Assert.IsTrue(base.GetLoggingConfiguration().Formatters.Where(x => x.Name == "Text Formatter").Any());
        }
    }

    [TestClass]
    public class When_SettingUseSSLForEmailTraceListener : Given_EmailListenerInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            EmailTraceListenerBuilder.UseSSL(true);
        }

        [TestMethod]
        public void ThenConfigurationHasSSLSet()
        {
            Assert.IsTrue(GetEmailTraceListenerData().UseSSL);
        }
    }

    [TestClass]
    public class When_SettingWindowsAuthForEmailTraceListener : Given_EmailListenerInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            EmailTraceListenerBuilder.WithWindowsCredentials();
        }

        [TestMethod]
        public void ThenConfigurationHasWindowsCredentialsSet()
        {
            Assert.AreEqual(EmailAuthenticationMode.WindowsCredentials, GetEmailTraceListenerData().AuthenticationMode);
        }
    }

    [TestClass]
    public class When_SettingUserNameAndPasswordForEmailTraceListener : Given_EmailListenerInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            EmailTraceListenerBuilder.WithUserNameAndPassword("user", "secret");
        }

        [TestMethod]
        public void ThenConfigurationHasCorrectModeUserNameAndPasswordSet()
        {
            Assert.AreEqual(EmailAuthenticationMode.UserNameAndPassword, GetEmailTraceListenerData().AuthenticationMode);
            Assert.AreEqual("user", GetEmailTraceListenerData().UserName);
            Assert.AreEqual("secret", GetEmailTraceListenerData().Password);
        }
    }


    
}
