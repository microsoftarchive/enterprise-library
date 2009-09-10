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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using System.Messaging;
using System.Collections.Specialized;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.Configuration
{

    public abstract class Given_MsmqListenerInConfigurationSourceBuilder : Given_LoggingCategorySourceInConfigurationSourceBuilder
    {
        protected ILoggingConfigurationSendToMsmqTraceListener MsmqListenerBuilder;
        private string msmqDiagnosticsListenerName = "msmq listener";

        protected override void Arrange()
        {
            base.Arrange();

            MsmqListenerBuilder = base.CategorySourceBuilder.SendTo.Msmq(msmqDiagnosticsListenerName);
        }

        protected MsmqTraceListenerData GetMsmqTraceListenerData()
        {
            return base.GetLoggingConfiguration().TraceListeners.OfType<MsmqTraceListenerData>()
                .Where(x => x.Name == msmqDiagnosticsListenerName).First();
        }
    }

    [TestClass]
    public class When_CallingSendToMsmqListenerOnLogToCategoryConfigurationBuilder : Given_MsmqListenerInConfigurationSourceBuilder
    {
        protected override void Arrange()
        {
            base.Arrange();
        }

        [TestMethod]
        public void ThenRecoverableIsFalse()
        {
            Assert.IsFalse(GetMsmqTraceListenerData().Recoverable);
        }

        [TestMethod]
        public void ThenUseDeadLetterQueueIsFalse()
        {
            Assert.IsFalse(GetMsmqTraceListenerData().UseDeadLetterQueue);
        }

        [TestMethod]
        public void ThenUseEncryptionIsFalse()
        {
            Assert.IsFalse(GetMsmqTraceListenerData().UseEncryption);
        }

        [TestMethod]
        public void ThenUseAuthenticationIsFalse()
        {
            Assert.IsFalse(GetMsmqTraceListenerData().UseAuthentication);
        }

        [TestMethod]
        public void ThenTimeToReadQueueIsInfinite()
        {
            Assert.AreEqual(Message.InfiniteTimeout, GetMsmqTraceListenerData().TimeToReachQueue);
        }

        [TestMethod]
        public void ThenTimeToBeReceivedIsInfinite()
        {
            Assert.AreEqual(Message.InfiniteTimeout, GetMsmqTraceListenerData().TimeToBeReceived);
        }

        public void ThenMessagePriorityIsNormal()
        {
            Assert.AreEqual(MessagePriority.Normal, GetMsmqTraceListenerData().MessagePriority);
        }

        [TestMethod]
        public void ThenTraceOptionsIsNone()
        {
            Assert.AreEqual(TraceOptions.None, GetMsmqTraceListenerData().TraceOutputOptions);
        }

        [TestMethod]
        public void ThenFilterIsAll()
        {
            Assert.AreEqual(SourceLevels.All, GetMsmqTraceListenerData().Filter);
        }

        [TestMethod]
        public void ThenCategortyContainsTraceListenerReference()
        {
            Assert.AreEqual(GetMsmqTraceListenerData().Name, GetTraceSourceData().TraceListeners.First().Name);
        }

        [TestMethod]
        public void ThenLoggingConfigurationContainsTraceListener()
        {
            Assert.IsTrue(GetLoggingConfiguration().TraceListeners.OfType<MsmqTraceListenerData>().Any());
        }
    }

    [TestClass]
    public class When_CallingSendToMsmqListenerPassingNullForName : Given_LoggingCategorySourceInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_SendToMsmq_TrowsArgumentException()
        {
            base.CategorySourceBuilder.SendTo.Msmq(null);
        }
    }


    [TestClass]
    public class When_CallingUseEncryptionOnMsmqTraceListener : Given_MsmqListenerInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.MsmqListenerBuilder.UseEncryption();
        }

        [TestMethod]
        public void ThenUseEncryptionIsTrue()
        {
            Assert.IsTrue(base.GetMsmqTraceListenerData().UseEncryption);
        }
    }

    [TestClass]
    public class When_CallingUseAuthenticationOnMsmqTraceListener : Given_MsmqListenerInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.MsmqListenerBuilder.UseAuthentication();
        }

        [TestMethod]
        public void ThenUseAuthenticationIsTrue()
        {
            Assert.IsTrue(base.GetMsmqTraceListenerData().UseAuthentication);
        }
    }

    [TestClass]
    public class When_CallingUseDeadLetterQueueOnMsmqTraceListener : Given_MsmqListenerInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.MsmqListenerBuilder.UseDeadLetterQueue();
        }

        [TestMethod]
        public void ThenUseDeadLetterQueueIsTrue()
        {
            Assert.IsTrue(base.GetMsmqTraceListenerData().UseDeadLetterQueue);
        }
    }

    [TestClass]
    public class When_CallingAsRecoverableOnMsmqTraceListener : Given_MsmqListenerInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.MsmqListenerBuilder.AsRecoverable();
        }

        [TestMethod]
        public void ThenRecoverableIsTrue()
        {
            Assert.IsTrue(base.GetMsmqTraceListenerData().Recoverable);
        }
    }

    [TestClass]
    public class When_SettingTimeToBeReceivedOnMsmqTraceListener : Given_MsmqListenerInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.MsmqListenerBuilder.SetTimeToBeReceived(TimeSpan.FromDays(1));
        }

        [TestMethod]
        public void ThenTimeToBeReceivedIsSet()
        {
            Assert.AreEqual(TimeSpan.FromDays(1), base.GetMsmqTraceListenerData().TimeToBeReceived);
        }
    }

    [TestClass]
    public class When_SettingTimeToReachQueueOnMsmqTraceListener : Given_MsmqListenerInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.MsmqListenerBuilder.SetTimeToReachQueue(TimeSpan.FromDays(1));
        }

        [TestMethod]
        public void ThenTimeToReachQueueIsSet()
        {
            Assert.AreEqual(TimeSpan.FromDays(1), base.GetMsmqTraceListenerData().TimeToReachQueue);
        }
    }

    [TestClass]
    public class When_SpecifyingTransactionTypenMsmqTraceListener : Given_MsmqListenerInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.MsmqListenerBuilder.WithTransactionType(MessageQueueTransactionType.Single);
        }

        [TestMethod]
        public void ThenTransactionTypeIsSet()
        {
            Assert.AreEqual(MessageQueueTransactionType.Single, base.GetMsmqTraceListenerData().TransactionType);
        }
    }

    [TestClass]
    public class When_SettingSharedFormatterForMsmqTraceListener : Given_MsmqListenerInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.MsmqListenerBuilder.FormatWithSharedFormatter("formatter");
        }

        [TestMethod]
        public void ThenConfigurationReflectsTraceOption()
        {
            Assert.AreEqual("formatter", base.GetMsmqTraceListenerData().Formatter);
        }
    }

    [TestClass]
    public class When_SettingSharedFormatterForMsmqTraceListenerToNull : Given_MsmqListenerInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_FormatWithSharedFormatter_ThrowsArgumentException()
        {
            base.MsmqListenerBuilder.FormatWithSharedFormatter(null);
        }
    }

    [TestClass]
    public class When_SettingTraceOptionForMsmqTraceListener : Given_MsmqListenerInConfigurationSourceBuilder
    {
        TraceOptions trOption;
        protected override void Act()
        {
            trOption = TraceOptions.Callstack | TraceOptions.DateTime;
            base.MsmqListenerBuilder.WithTraceOptions(trOption);
        }

        [TestMethod]
        public void ThenConfigurationReflectsTraceOption()
        {
            Assert.AreEqual(trOption, base.GetMsmqTraceListenerData().TraceOutputOptions);
        }
    }

    [TestClass]
    public class When_SettingFilterForMsmqTraceListener : Given_MsmqListenerInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.MsmqListenerBuilder.Filter(SourceLevels.Error);
        }

        [TestMethod]
        public void ThenConfigurationReflectsTraceOption()
        {
            Assert.AreEqual(SourceLevels.Error, base.GetMsmqTraceListenerData().Filter);
        }
    }


    [TestClass]
    public class When_SettingNullFormatterForMsmqTraceListener : Given_MsmqListenerInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Then_FormatWith_ThrowsArgumentNullException()
        {
            base.MsmqListenerBuilder.FormatWith(null);
        }
    }

    [TestClass]
    public class When_SettingNewFormatterForMsmqTraceListener : Given_MsmqListenerInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.MsmqListenerBuilder.FormatWith(new FormatterBuilder().TextFormatterNamed("Text Formatter"));
        }

        [TestMethod]
        public void ThenConfigurationReflectsTraceOption()
        {
            Assert.AreEqual("Text Formatter", base.GetMsmqTraceListenerData().Formatter);
            Assert.IsTrue(base.GetLoggingConfiguration().Formatters.Where(x => x.Name == "Text Formatter").Any());
        }
    }
}
