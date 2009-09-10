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

    public abstract class Given_XmlFileListenerInConfigurationSourceBuilder : Given_LoggingCategorySourceInConfigurationSourceBuilder
    {
        protected ILoggingConfigurationSendToXmlTraceListener XmlListenerBuilder;
        private string xmlFileListenerName = "xml listener";

        protected override void Arrange()
        {
            base.Arrange();

            XmlListenerBuilder = base.CategorySourceBuilder.SendTo.XmlFile(xmlFileListenerName);
        }

        protected XmlTraceListenerData GetXmlTraceListenerData()
        {
            return base.GetLoggingConfiguration().TraceListeners.OfType<XmlTraceListenerData>()
                .Where(x => x.Name == xmlFileListenerName).First();
        }
    }

    [TestClass]
    public class When_CallingSendToXmlListenerPassingNullForName : Given_LoggingCategorySourceInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_SendToXml_ThrowsArgumentException()
        {
            CategorySourceBuilder.SendTo.XmlFile(null);
        }
    }

    [TestClass]
    public class When_CallingSendToXmlListenerOnLogToCategoryConfigurationBuilder : Given_XmlFileListenerInConfigurationSourceBuilder
    {
        protected override void Arrange()
        {
            base.Arrange();
        }

        [TestMethod]
        public void ThenDefaultFilenameIsSet()
        {
            Assert.AreEqual("trace-xml.log", GetXmlTraceListenerData().FileName);
        }

        [TestMethod]
        public void ThenTraceOptionsIsNone()
        {
            Assert.AreEqual(TraceOptions.None, GetXmlTraceListenerData().TraceOutputOptions);
        }

        [TestMethod]
        public void ThenFilterIsAll()
        {
            Assert.AreEqual(SourceLevels.All, GetXmlTraceListenerData().Filter);
        }

        [TestMethod]
        public void ThenCategortyContainsTraceListenerReference()
        {
            Assert.AreEqual(GetXmlTraceListenerData().Name, GetTraceSourceData().TraceListeners.First().Name);
        }

        [TestMethod]
        public void ThenLoggingConfigurationContainsTraceListener()
        {
            Assert.IsTrue(GetLoggingConfiguration().TraceListeners.OfType<XmlTraceListenerData>().Any());
        }
    }


    [TestClass]
    public class When_SettingFileNameForXmlTraceListener : Given_XmlFileListenerInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.XmlListenerBuilder.ToFile("file-name.xml");
        }

        [TestMethod]
        public void ThenConfigurationReflectsFileName()
        {
            Assert.AreEqual("file-name.xml", base.GetXmlTraceListenerData().FileName);
        }
    }


    [TestClass]
    public class When_SettingFileNameForXmlTraceListenerToNull : Given_XmlFileListenerInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_ToFile_ThrowsArgumentException()
        {
            XmlListenerBuilder.ToFile(null);
        }
    }

    [TestClass]
    public class When_SettingTraceOptionForXmlTraceListener : Given_XmlFileListenerInConfigurationSourceBuilder
    {
        TraceOptions trOption;
        protected override void Act()
        {
            trOption = TraceOptions.Callstack | TraceOptions.DateTime;
            base.XmlListenerBuilder.WithTraceOptions(trOption);
        }

        [TestMethod]
        public void ThenConfigurationReflectsTraceOption()
        {
            Assert.AreEqual(trOption, base.GetXmlTraceListenerData().TraceOutputOptions);
        }
    }

    [TestClass]
    public class When_SettingFilterForXmlTraceListener : Given_XmlFileListenerInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.XmlListenerBuilder.Filter(SourceLevels.Error);
        }

        [TestMethod]
        public void ThenConfigurationReflectsTraceOption()
        {
            Assert.AreEqual(SourceLevels.Error, base.GetXmlTraceListenerData().Filter);
        }
    }


}
