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

    public abstract class Given_FlatFileListenerInConfigurationSourceBuilder : Given_LoggingCategorySourceInConfigurationSourceBuilder
    {
        protected ILoggingConfigurationSendToFlatFileTraceListener FlatFileListenerBuilder;
        private string flatFileDiagnosticsListenerName = "file listener";

        protected override void Arrange()
        {
            base.Arrange();

            FlatFileListenerBuilder = base.CategorySourceBuilder.SendTo.FlatFile(flatFileDiagnosticsListenerName);
        }

        protected FlatFileTraceListenerData GetFlatFileTraceListenerData()
        {
            return base.GetLoggingConfiguration().TraceListeners.OfType<FlatFileTraceListenerData>()
                .Where(x => x.Name == flatFileDiagnosticsListenerName).First();
        }
    }

    [TestClass]
    public class When_CallingSendToFlatFileListenerOnLogToCategoryConfigurationBuilder : Given_FlatFileListenerInConfigurationSourceBuilder
    {
        [TestMethod]
        public void ThenDefaultFilenameIsSet()
        {
            Assert.AreEqual("trace.log", GetFlatFileTraceListenerData().FileName);
        }

        [TestMethod]
        public void ThenTraceOptionsIsNone()
        {
            Assert.AreEqual(TraceOptions.None, GetFlatFileTraceListenerData().TraceOutputOptions);
        }

        [TestMethod]
        public void ThenFilterIsAll()
        {
            Assert.AreEqual(SourceLevels.All, GetFlatFileTraceListenerData().Filter);
        }

        [TestMethod]
        public void ThenCategortyContainsTraceListenerReference()
        {
            Assert.AreEqual(GetFlatFileTraceListenerData().Name, GetTraceSourceData().TraceListeners.First().Name);
        }

        [TestMethod]
        public void ThenLoggingConfigurationContainsTraceListener()
        {
            Assert.IsTrue(GetLoggingConfiguration().TraceListeners.OfType<FlatFileTraceListenerData>().Any());
        }
    }

    [TestClass]
    public class When_CallingSendToFlatFileListenerPassingNullForName : Given_FlatFileListenerInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_SendToFlatFile_ThrowsArgumentException()
        {
            CategorySourceBuilder.SendTo.FlatFile( null );
        }
    }

    [TestClass]
    public class When_CallingToFileOnFlatFileTraceListener : Given_FlatFileListenerInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.FlatFileListenerBuilder.ToFile("new file name.txt");
        }

        [TestMethod]
        public void ThenConfigurationReflectsNewFileName()
        {
            Assert.AreEqual("new file name.txt", base.GetFlatFileTraceListenerData().FileName);
        }
    }

    [TestClass]
    public class When_CallingToFileOnFlatFileTraceListenerPassingNull : Given_FlatFileListenerInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_ToFile_ThrowsArgumentException()
        {
            base.FlatFileListenerBuilder.ToFile(null);
        }
    }

    [TestClass]
    public class When_SettingHeaderForFlatFileTraceListener : Given_FlatFileListenerInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.FlatFileListenerBuilder.WithHeader("Header");
        }

        [TestMethod]
        public void ThenConfigurationReflectsTraceOption()
        {
            Assert.AreEqual("Header", base.GetFlatFileTraceListenerData().Header);
        }
    }

    [TestClass]
    public class When_SettingFooterForFlatFileTraceListener : Given_FlatFileListenerInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.FlatFileListenerBuilder.WithFooter("Footer");
        }

        [TestMethod]
        public void ThenConfigurationReflectsTraceOption()
        {
            Assert.AreEqual("Footer", base.GetFlatFileTraceListenerData().Footer);
        }
    }

    [TestClass]
    public class When_SettingSharedFormatterForFlatFileTraceListener : Given_FlatFileListenerInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.FlatFileListenerBuilder.FormatWithSharedFormatter("formatter");
        }

        [TestMethod]
        public void ThenConfigurationReflectsTraceOption()
        {
            Assert.AreEqual("formatter", base.GetFlatFileTraceListenerData().Formatter);
        }
    }

    [TestClass]
    public class When_SettingTraceOptionForFlatFileTraceListener : Given_FlatFileListenerInConfigurationSourceBuilder
    {
        TraceOptions trOption;
        protected override void Act()
        {
            trOption = TraceOptions.Callstack | TraceOptions.DateTime;
            base.FlatFileListenerBuilder.WithTraceOptions(trOption);
        }

        [TestMethod]
        public void ThenConfigurationReflectsTraceOption()
        {
            Assert.AreEqual(trOption, base.GetFlatFileTraceListenerData().TraceOutputOptions);
        }
    }

    [TestClass]
    public class When_SettingFilterForFlatFileTraceListener : Given_FlatFileListenerInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.FlatFileListenerBuilder.Filter(SourceLevels.Error);
        }

        [TestMethod]
        public void ThenConfigurationReflectsTraceOption()
        {
            Assert.AreEqual(SourceLevels.Error, base.GetFlatFileTraceListenerData().Filter);
        }
    }

    [TestClass]
    public class When_SettingNewFormatterForFlatFileTraceListener : Given_FlatFileListenerInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.FlatFileListenerBuilder.FormatWith(new FormatterBuilder().TextFormatterNamed("Text Formatter"));
        }

        [TestMethod]
        public void ThenConfigurationReflectsTraceOption()
        {
            Assert.AreEqual("Text Formatter", base.GetFlatFileTraceListenerData().Formatter);
            Assert.IsTrue(base.GetLoggingConfiguration().Formatters.Where(x => x.Name == "Text Formatter").Any());
        }
    }


    [TestClass]
    public class When_SettingNullFormatterForFlatFileTraceListener : Given_FlatFileListenerInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Then_FormatWith_ThrowsArgumentNullException()
        {
            base.FlatFileListenerBuilder.FormatWith(null);
        }
    }
}
