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

    public abstract class Given_WmiListenerInConfigurationSourceBuilder : Given_LoggingCategorySourceInConfigurationSourceBuilder
    {
        protected ILoggingConfigurationSendToWmiTraceListener WmiListenerBuilder;
        private string wmiFileListenerName = "wmi listener";

        protected override void Arrange()
        {
            base.Arrange();

            WmiListenerBuilder = base.CategorySourceBuilder.SendTo.Wmi(wmiFileListenerName);
        }

        protected WmiTraceListenerData GetWmiTraceListenerData()
        {
            return base.GetLoggingConfiguration().TraceListeners.OfType<WmiTraceListenerData>()
                .Where(x => x.Name == wmiFileListenerName).First();
        }
    }

    [TestClass]
    public class When_callingSendToWmiListenerPassingNullForListenerName : Given_LoggingCategorySourceInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_SendToWmi_ThrowsArgumentException()
        {
            CategorySourceBuilder.SendTo.Wmi(null);
        }
    }

    [TestClass]
    public class When_CallingSendToWmiListenerOnLogToCategoryConfigurationBuilder : Given_WmiListenerInConfigurationSourceBuilder
    {
        protected override void Arrange()
        {
            base.Arrange();
        }

        [TestMethod]
        public void ThenTraceOptionsIsNone()
        {
            Assert.AreEqual(TraceOptions.None, GetWmiTraceListenerData().TraceOutputOptions);
        }

        [TestMethod]
        public void ThenFilterIsAll()
        {
            Assert.AreEqual(SourceLevels.All, GetWmiTraceListenerData().Filter);
        }

        [TestMethod]
        public void ThenCategortyContainsTraceListenerReference()
        {
            Assert.AreEqual(GetWmiTraceListenerData().Name, GetTraceSourceData().TraceListeners.First().Name);
        }

        [TestMethod]
        public void ThenLoggingConfigurationContainsTraceListener()
        {
            Assert.IsTrue(GetLoggingConfiguration().TraceListeners.OfType<WmiTraceListenerData>().Any());
        }
    }

    [TestClass]
    public class When_SettingTraceOptionForWmiTraceListener : Given_WmiListenerInConfigurationSourceBuilder
    {
        TraceOptions trOption;
        protected override void Act()
        {
            trOption = TraceOptions.Callstack | TraceOptions.DateTime;
            base.WmiListenerBuilder.WithTraceOptions(trOption);

        }

        [TestMethod]
        public void ThenConfigurationReflectsTraceOption()
        {
            Assert.AreEqual(trOption, base.GetWmiTraceListenerData().TraceOutputOptions);
        }
    }

    [TestClass]
    public class When_SettingFilterForWmiTraceListener : Given_WmiListenerInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.WmiListenerBuilder.Filter(SourceLevels.Error);
        }

        [TestMethod]
        public void ThenConfigurationReflectsTraceOption()
        {
            Assert.AreEqual(SourceLevels.Error, base.GetWmiTraceListenerData().Filter);
        }
    }

}
