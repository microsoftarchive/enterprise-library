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

    public abstract class Given_RollingFileListenerInConfigurationSourceBuilder : Given_LoggingCategorySourceInConfigurationSourceBuilder
    {
        protected ILoggingConfigurationSendToRollingFileTraceListener RollingFileListenerBuilder;
        private string rollingFileListenerName = "rolling file listener";

        protected override void Arrange()
        {
            base.Arrange();

            RollingFileListenerBuilder = base.CategorySourceBuilder.SendTo.RollingFile(rollingFileListenerName);
        }

        protected RollingFlatFileTraceListenerData GetRollingFileTraceListenerData()
        {
            return base.GetLoggingConfiguration().TraceListeners.OfType<RollingFlatFileTraceListenerData>()
                .Where(x => x.Name == rollingFileListenerName).First();
        }
    }

    [TestClass]
    public class When_CallingSendToRollingFileListnerPassingNullName : Given_LoggingCategorySourceInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_SendToRollingFile_ThrowsArgumentException()
        {
            CategorySourceBuilder.SendTo.RollingFile(null);
        }
    }

    [TestClass]
    public class When_CallingSendToRollingFileListenerOnLogToCategoryConfigurationBuilder : Given_RollingFileListenerInConfigurationSourceBuilder
    {
        protected override void Arrange()
        {
            base.Arrange();
        }

        [TestMethod]
        public void ThenDefaultFileNameIsSet()
        {
            Assert.AreEqual("rolling.log", GetRollingFileTraceListenerData().FileName);
        }

        [TestMethod]
        public void ThenRollFileExistBehaviorIsOverwrite()
        {
            Assert.AreEqual(RollFileExistsBehavior.Overwrite, GetRollingFileTraceListenerData().RollFileExistsBehavior);
        }

        [TestMethod]
        public void ThenRollIntervalIsNone()
        {
            Assert.AreEqual(RollInterval.None, GetRollingFileTraceListenerData().RollInterval);
        }

        [TestMethod]
        public void ThenRollSizeIs0Kb()
        {
            Assert.AreEqual(0, GetRollingFileTraceListenerData().RollSizeKB);
        }

        [TestMethod]
        public void ThenTraceOptionsIsNone()
        {
            Assert.AreEqual(TraceOptions.None, GetRollingFileTraceListenerData().TraceOutputOptions);
        }

        [TestMethod]
        public void ThenFilterIsAll()
        {
            Assert.AreEqual(SourceLevels.All, GetRollingFileTraceListenerData().Filter);
        }

        [TestMethod]
        public void ThenCategortyContainsTraceListenerReference()
        {
            Assert.AreEqual(GetRollingFileTraceListenerData().Name, GetTraceSourceData().TraceListeners.First().Name);
        }

        [TestMethod]
        public void ThenLoggingConfigurationContainsTraceListener()
        {
            Assert.IsTrue(GetLoggingConfiguration().TraceListeners.OfType<RollingFlatFileTraceListenerData>().Any());
        }
    }

    [TestClass]
    public class When_SettingFileExistBehaviorOnRollingFileTraceListener : Given_RollingFileListenerInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.RollingFileListenerBuilder.WhenRollFileExists(RollFileExistsBehavior.Increment);
        }

        [TestMethod]
        public void ThenConfigurationReflectsNewFileExistBehavior()
        {
            Assert.AreEqual(RollFileExistsBehavior.Increment, base.GetRollingFileTraceListenerData().RollFileExistsBehavior);
        }
    }

    [TestClass]
    public class When_SettingTimeStampFmtOnRollingFileTraceListener : Given_RollingFileListenerInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.RollingFileListenerBuilder.UseTimeStampPattern("mm-dd");
        }

        [TestMethod]
        public void ThenConfigurationReflectsNewTimeStampFmt()
        {
            Assert.AreEqual("mm-dd", base.GetRollingFileTraceListenerData().TimeStampPattern);
        }
    }

    [TestClass]
    public class When_SettingRollIntervalOnRollingFileTraceListener : Given_RollingFileListenerInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.RollingFileListenerBuilder.RollEvery(RollInterval.Midnight);
        }

        [TestMethod]
        public void ThenConfigurationReflectsNewRollInterval()
        {
            Assert.AreEqual(RollInterval.Midnight, base.GetRollingFileTraceListenerData().RollInterval);
        }
    }

    [TestClass]
    public class When_SettingRollSizeOnRollingFileTraceListener : Given_RollingFileListenerInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.RollingFileListenerBuilder.RollAfterSize(42);
        }

        [TestMethod]
        public void ThenConfigurationReflectsNewRollSize()
        {
            Assert.AreEqual(42, base.GetRollingFileTraceListenerData().RollSizeKB);
        }
    }


    [TestClass]
    public class When_CallingToFileOnRollingFileTraceListener : Given_RollingFileListenerInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.RollingFileListenerBuilder.ToFile("new file name.txt");
        }

        [TestMethod]
        public void ThenConfigurationReflectsNewFileName()
        {
            Assert.AreEqual("new file name.txt", base.GetRollingFileTraceListenerData().FileName);
        }
    }

    [TestClass]
    public class When_SettingHeaderForRollingFileTraceListener : Given_RollingFileListenerInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.RollingFileListenerBuilder.WithHeader("Header");
        }

        [TestMethod]
        public void ThenConfigurationReflectsTraceOption()
        {
            Assert.AreEqual("Header", base.GetRollingFileTraceListenerData().Header);
        }
    }

    [TestClass]
    public class When_SettingFooterForRollingFileTraceListener : Given_RollingFileListenerInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.RollingFileListenerBuilder.WithFooter("Footer");
        }

        [TestMethod]
        public void ThenConfigurationReflectsTraceOption()
        {
            Assert.AreEqual("Footer", base.GetRollingFileTraceListenerData().Footer);
        }
    }

    [TestClass]
    public class When_SettingSharedFormatterForRollingFileTraceListener : Given_RollingFileListenerInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.RollingFileListenerBuilder.FormatWithSharedFormatter("formatter");
        }

        [TestMethod]
        public void ThenConfigurationReflectsTraceOption()
        {
            Assert.AreEqual("formatter", base.GetRollingFileTraceListenerData().Formatter);
        }
    }

    [TestClass]
    public class When_SettingTraceOptionForRollingFileTraceListener : Given_RollingFileListenerInConfigurationSourceBuilder
    {
        TraceOptions trOption;
        protected override void Act()
        {
            trOption = TraceOptions.Callstack | TraceOptions.DateTime;
            base.RollingFileListenerBuilder.WithTraceOptions(trOption);
        }

        [TestMethod]
        public void ThenConfigurationReflectsTraceOption()
        {
            Assert.AreEqual(trOption, base.GetRollingFileTraceListenerData().TraceOutputOptions);
        }
    }

    [TestClass]
    public class When_SettingFilterForRollingFileTraceListener : Given_RollingFileListenerInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.RollingFileListenerBuilder.Filter(SourceLevels.Error);
        }

        [TestMethod]
        public void ThenConfigurationReflectsTraceOption()
        {
            Assert.AreEqual(SourceLevels.Error, base.GetRollingFileTraceListenerData().Filter);
        }
    }

    [TestClass]
    public class When_SettingNewFormatterForRollingFileTraceListener : Given_RollingFileListenerInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.RollingFileListenerBuilder.FormatWith(new FormatterBuilder().TextFormatterNamed("Text Formatter"));
        }

        [TestMethod]
        public void ThenConfigurationReflectsTraceOption()
        {
            Assert.AreEqual("Text Formatter", base.GetRollingFileTraceListenerData().Formatter);
            Assert.IsTrue(base.GetLoggingConfiguration().Formatters.Where(x => x.Name == "Text Formatter").Any());
        }
    }

    [TestClass]
    public class When_SettingNullFormatterForRollingFileTraceListener : Given_RollingFileListenerInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Then_FormatWith_ThrowsArgumentNullException()
        {
            base.RollingFileListenerBuilder.FormatWith(null);
        }
    }


}
