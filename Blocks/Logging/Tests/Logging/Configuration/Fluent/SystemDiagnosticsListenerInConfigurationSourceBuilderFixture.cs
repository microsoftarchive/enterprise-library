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
    public abstract class Given_SystemDiagnosticsListenerInConfigurationSourceBuilder : Given_LoggingCategorySourceInConfigurationSourceBuilder
    {
        protected ILoggingConfigurationSendToSystemDiagnosticsTraceListener SystemDiagnosticsListenerBuilder;
        private string sysDiagnosticsListenerName = "system diagnostics listener";

        protected override void Arrange()
        {
            base.Arrange();

            SystemDiagnosticsListenerBuilder = base.CategorySourceBuilder.SendTo.SystemDiagnosticsListener(sysDiagnosticsListenerName);
        }

        protected SystemDiagnosticsTraceListenerData GetSystemDiagnosticsTraceListenerData()
        {
            return base.GetLoggingConfiguration().TraceListeners.OfType<SystemDiagnosticsTraceListenerData>()
                .Where(x => x.Name == sysDiagnosticsListenerName).First();
        }
    }

    [TestClass]
    public class When_CallingSendToSystemDiagnosticsListenerPassingNullForName : Given_LoggingCategorySourceInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_SendToSystemDiagnostics_ThrowsArgumentException()
        {
            CategorySourceBuilder.SendTo.SystemDiagnosticsListener(null);
        }
    }


    [TestClass]
    public class When_CallingSendToSystemDiagnosticsListenerOnLogToCategoryConfigurationBuilder : Given_SystemDiagnosticsListenerInConfigurationSourceBuilder
    {
        protected override void Arrange()
        {
            base.Arrange();
        }

        [TestMethod]
        public void ThenTraceOptionsIsNone()
        {
            Assert.AreEqual(TraceOptions.None, GetSystemDiagnosticsTraceListenerData().TraceOutputOptions);
        }

        [TestMethod]
        public void ThenFilterIsAll()
        {
            Assert.AreEqual(SourceLevels.All, GetSystemDiagnosticsTraceListenerData().Filter);
        }

        [TestMethod]
        public void ThenCategortyContainsTraceListenerReference()
        {
            Assert.AreEqual(GetSystemDiagnosticsTraceListenerData().Name, GetTraceSourceData().TraceListeners.First().Name);
        }

        [TestMethod]
        public void ThenLoggingConfigurationContainsTraceListener()
        {
            Assert.IsTrue(GetLoggingConfiguration().TraceListeners.OfType<SystemDiagnosticsTraceListenerData>().Any());
        }
    }

    [TestClass]
    public class When_SpecifyingTraceListenerTypeForSystemDiagnosticsTraceListener : Given_SystemDiagnosticsListenerInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.SystemDiagnosticsListenerBuilder.ForTraceListenerType(typeof(ConsoleTraceListener));
        }

        [TestMethod]
        public void ThenTraceListenerTypeIsSetInConfiguration()
        {
            Assert.AreEqual(typeof(ConsoleTraceListener), GetSystemDiagnosticsTraceListenerData().Type);
        }
    }

    [TestClass]
    public class When_SpecifyingNullTraceListenerTypeForSystemDiagnosticsTraceListener : Given_SystemDiagnosticsListenerInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Then_ForTraceListenerType_ThrowsArgumentNullException()
        {
            base.SystemDiagnosticsListenerBuilder.ForTraceListenerType(null);
        }
    }

    [TestClass]
    public class When_SpecifyingNonTraceListenerTypeForSystemDiagnosticsTraceListener : Given_SystemDiagnosticsListenerInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_ForTraceListenerType_ThrowsArgumentException()
        {
            base.SystemDiagnosticsListenerBuilder.ForTraceListenerType(typeof(object));
        }
    }

    [TestClass]
    public class When_SpecifyingTraceListenerTypeForSystemDiagnosticsTraceListenerGeneric : Given_SystemDiagnosticsListenerInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.SystemDiagnosticsListenerBuilder.ForTraceListenerType<ConsoleTraceListener>();
        }

        [TestMethod]
        public void ThenTraceListenerTypeIsSetInConfiguration()
        {
            Assert.AreEqual(typeof(ConsoleTraceListener), GetSystemDiagnosticsTraceListenerData().Type);
        }
    }

    [TestClass]
    public class When_SpecifyingInitDataForSystemDiagnosticsTraceListenerGeneric : Given_SystemDiagnosticsListenerInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.SystemDiagnosticsListenerBuilder.UsingInitData("intialization data");
        }

        [TestMethod]
        public void ThenInitDataIsSetInConfiguration()
        {
            Assert.AreEqual("intialization data", GetSystemDiagnosticsTraceListenerData().InitData);
        }
    }

    [TestClass]
    public class When_SettingTraceOptionForSystemDiagnosticsTraceListener : Given_SystemDiagnosticsListenerInConfigurationSourceBuilder
    {
        TraceOptions trOption;
        protected override void Act()
        {
            trOption = TraceOptions.Callstack | TraceOptions.DateTime;
            base.SystemDiagnosticsListenerBuilder.WithTraceOptions(trOption);
        }

        [TestMethod]
        public void ThenConfigurationReflectsTraceOption()
        {
            Assert.AreEqual(trOption, base.GetSystemDiagnosticsTraceListenerData().TraceOutputOptions);
        }
    }

    [TestClass]
    public class When_SettingFilterForSystemDiagnosticsTraceListener : Given_SystemDiagnosticsListenerInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.SystemDiagnosticsListenerBuilder.Filter(SourceLevels.Error);
        }

        [TestMethod]
        public void ThenConfigurationReflectsTraceOption()
        {
            Assert.AreEqual(SourceLevels.Error, base.GetSystemDiagnosticsTraceListenerData().Filter);
        }
    }

}
