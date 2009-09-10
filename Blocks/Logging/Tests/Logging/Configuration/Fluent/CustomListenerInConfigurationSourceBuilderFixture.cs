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
    public abstract class Given_CustomListenerInConfigurationSourceBuilder : Given_LoggingCategorySourceInConfigurationSourceBuilder
    {
        protected ILoggingConfigurationSendToCustomTraceListener CustomListenerBuilder;
        private string customListenerName = "custom listener";

        protected override void Arrange()
        {
            base.Arrange();

            CustomListenerBuilder = base.CategorySourceBuilder.SendTo.Custom(customListenerName, typeof(TestCustomTraceListener));
        }

        protected CustomTraceListenerData GetCustomTraceListenerData()
        {
            return base.GetLoggingConfiguration().TraceListeners.OfType<CustomTraceListenerData>()
                .Where(x => x.Name == customListenerName).First();
        }
    }

    class TestCustomTraceListener : CustomTraceListener
    {
        public override void Write(string message)
        {
            throw new NotImplementedException();
        }

        public override void WriteLine(string message)
        {
            throw new NotImplementedException();
        }
    }

    [TestClass]
    public class When_CallingSendToCustomListenerPassingNullForName : Given_LoggingCategorySourceInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_SendToCustom_ThrowsArgumentException()
        {
            base.CategorySourceBuilder.SendTo.Custom(null, typeof(TestCustomTraceListener));
        }
    }

    [TestClass]
    public class When_CallingSendToCustomListenerPassingNullForType : Given_LoggingCategorySourceInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Then_SendToCustom_ThrowsArgumentNullException()
        {
            base.CategorySourceBuilder.SendTo.Custom("listener name", null);
        }
    }

    [TestClass]
    public class When_CallingSendToCustomListenerPassingNonListenerType : Given_LoggingCategorySourceInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_SendToCustom_ThrowsArgumentException()
        {
            base.CategorySourceBuilder.SendTo.Custom("listener name", typeof(object));
        }
    }

    [TestClass]
    public class When_CallingSendToCustomListenerPassingNullForAttributes : Given_LoggingCategorySourceInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Then_SendToCustom_ThrowsArgumentNullException()
        {
            base.CategorySourceBuilder.SendTo.Custom("listener name", typeof(TestCustomTraceListener), null);
        }
    }

    [TestClass]
    public class When_CallingSendToCustomListenerOnLogToCategoryConfigurationBuilder : Given_LoggingCategorySourceInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.CategorySourceBuilder.SendTo.Custom("custom", typeof(TestCustomTraceListener));
        }

        [TestMethod]
        public void ThenCustomTraceListenerAppearsInConfiguration()
        {
            Assert.IsTrue(GetLoggingConfiguration().TraceListeners.OfType<CustomTraceListenerData>().Any());
        }


        [TestMethod]
        public void ThenCustomTraceListenerHasReferenceInCategorySource()
        {
            Assert.IsTrue(GetTraceSourceData().TraceListeners.Where(x => x.Name == "custom").Any());
        }

        [TestMethod]
        public void ThenCustomTraceListenerHasAppropriateName()
        {
            CustomTraceListenerData customTl = GetLoggingConfiguration().TraceListeners.OfType<CustomTraceListenerData>().First();
            Assert.AreEqual("custom", customTl.Name);
        }

        [TestMethod]
        public void ThenCustomTraceListenerHasAppropriateType()
        {
            CustomTraceListenerData customTl = GetLoggingConfiguration().TraceListeners.OfType<CustomTraceListenerData>().First();
            Assert.AreEqual(typeof(TestCustomTraceListener), customTl.Type);
        }
    }

    [TestClass]
    public class When_CallingSendToCustomGenericListenerOnLogToCategoryConfigurationBuilder : Given_LoggingCategorySourceInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.CategorySourceBuilder.SendTo.Custom<TestCustomTraceListener>("custom");
        }

        [TestMethod]
        public void ThenCustomTraceListenerAppearsInConfiguration()
        {
            Assert.IsTrue(GetLoggingConfiguration().TraceListeners.OfType<CustomTraceListenerData>().Any());
        }


        [TestMethod]
        public void ThenCustomTraceListenerHasReferenceInCategorySource()
        {
            Assert.IsTrue(GetTraceSourceData().TraceListeners.Where(x => x.Name == "custom").Any());
        }

        [TestMethod]
        public void ThenCustomTraceListenerHasAppropriateName()
        {
            CustomTraceListenerData customTl = GetLoggingConfiguration().TraceListeners.OfType<CustomTraceListenerData>().First();
            Assert.AreEqual("custom", customTl.Name);
        }

        [TestMethod]
        public void ThenCustomTraceListenerHasAppropriateType()
        {
            CustomTraceListenerData customTl = GetLoggingConfiguration().TraceListeners.OfType<CustomTraceListenerData>().First();
            Assert.AreEqual(typeof(TestCustomTraceListener), customTl.Type);
        }
    }

    [TestClass]
    public class When_CallingSendToCustomListenerPassingAttributesOnLogToCategoryConfigurationBuilder : Given_LoggingCategorySourceInConfigurationSourceBuilder
    {
        NameValueCollection attributes;
        protected override void Act()
        {
            attributes = new NameValueCollection();
            attributes.Add("key", "value");

            base.CategorySourceBuilder.SendTo.Custom("custom", typeof(TestCustomTraceListener), attributes);
        }

        [TestMethod]
        public void ThenCustomTraceListenerAppearsInConfiguration()
        {
            CustomTraceListenerData customTl = GetLoggingConfiguration().TraceListeners.OfType<CustomTraceListenerData>().First();
            Assert.AreEqual("custom", customTl.Name);
        }

        [TestMethod]
        public void ThenCustomTraceListenerHasReferenceInCategorySource()
        {
            Assert.IsTrue(GetTraceSourceData().TraceListeners.Where(x => x.Name == "custom").Any());
        }

        [TestMethod]
        public void ThenConfiguredTraceListenerHasAttributes()
        {
            CustomTraceListenerData customTl = GetLoggingConfiguration().TraceListeners.OfType<CustomTraceListenerData>().First();
            Assert.AreEqual("key", customTl.Attributes.Keys[0]);
            Assert.AreEqual("value", customTl.Attributes["key"]);
        }


        [TestMethod]
        public void ThenTraceOptionsIsNone()
        {
            CustomTraceListenerData customTl = GetLoggingConfiguration().TraceListeners.OfType<CustomTraceListenerData>().First();
            Assert.AreEqual(TraceOptions.None, customTl.TraceOutputOptions);
        }

        [TestMethod]
        public void ThenFilterIsAll()
        {
            CustomTraceListenerData customTl = GetLoggingConfiguration().TraceListeners.OfType<CustomTraceListenerData>().First();
            Assert.AreEqual(SourceLevels.All, customTl.Filter);
        }

        [TestMethod]
        public void ThenCustomTraceListenerHasAppropriateType()
        {
            CustomTraceListenerData customTl = GetLoggingConfiguration().TraceListeners.OfType<CustomTraceListenerData>().First();
            Assert.AreEqual(typeof(TestCustomTraceListener), customTl.Type);
        }
    }

    [TestClass]
    public class When_CallingSendToCustomGenericListenerPassingAttributesOnLogToCategoryConfigurationBuilder : Given_LoggingCategorySourceInConfigurationSourceBuilder
    {
        NameValueCollection attributes;
        protected override void Act()
        {
            attributes = new NameValueCollection();
            attributes.Add("key", "value");

            base.CategorySourceBuilder.SendTo.Custom<TestCustomTraceListener>("custom", attributes);
        }

        [TestMethod]
        public void ThenCustomTraceListenerAppearsInConfiguration()
        {
            CustomTraceListenerData customTl = GetLoggingConfiguration().TraceListeners.OfType<CustomTraceListenerData>().First();
            Assert.AreEqual("custom", customTl.Name);
        }

        [TestMethod]
        public void ThenCustomTraceListenerHasReferenceInCategorySource()
        {
            Assert.IsTrue(GetTraceSourceData().TraceListeners.Where(x => x.Name == "custom").Any());
        }

        [TestMethod]
        public void ThenConfiguredTraceListenerHasAttributes()
        {
            CustomTraceListenerData customTl = GetLoggingConfiguration().TraceListeners.OfType<CustomTraceListenerData>().First();
            Assert.AreEqual("key", customTl.Attributes.Keys[0]);
            Assert.AreEqual("value", customTl.Attributes["key"]);
        }


        [TestMethod]
        public void ThenTraceOptionsIsNone()
        {
            CustomTraceListenerData customTl = GetLoggingConfiguration().TraceListeners.OfType<CustomTraceListenerData>().First();
            Assert.AreEqual(TraceOptions.None, customTl.TraceOutputOptions);
        }

        [TestMethod]
        public void ThenFilterIsAll()
        {
            CustomTraceListenerData customTl = GetLoggingConfiguration().TraceListeners.OfType<CustomTraceListenerData>().First();
            Assert.AreEqual(SourceLevels.All, customTl.Filter);
        }

        [TestMethod]
        public void ThenCustomTraceListenerHasAppropriateType()
        {
            CustomTraceListenerData customTl = GetLoggingConfiguration().TraceListeners.OfType<CustomTraceListenerData>().First();
            Assert.AreEqual(typeof(TestCustomTraceListener), customTl.Type);
        }
    }


    [TestClass]
    public class When_SettingFormatterForCustomListener : Given_CustomListenerInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.CustomListenerBuilder =
                base.CustomListenerBuilder
                        .FormatWith(new FormatterBuilder().TextFormatterNamed("formatter name"));
        }

        [TestMethod]
        public void ThenTraceListenerHasFormatter()
        {
            var CustomTraceListener = base.GetCustomTraceListenerData();
            Assert.AreEqual("formatter name", CustomTraceListener.Formatter);
        }

        [TestMethod]
        public void ThenLogginSettingsHasFormatter()
        {
            var loggingSettings = base.GetLoggingConfiguration();
            Assert.IsTrue(loggingSettings.Formatters.Where(x=>x.Name == "formatter name").Any());
        }
    }

    [TestClass]
    public class When_SettingFilterOnCustomListenerBuilder : Given_CustomListenerInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            CustomListenerBuilder = CustomListenerBuilder.Filter(SourceLevels.Warning);
        }

        [TestMethod]
        public void ThenConfiguredCustomTracelistnerCallstackAndDateTimeOptions()
        {
            Assert.AreEqual(SourceLevels.Warning, GetCustomTraceListenerData().Filter);
        }
    }


    [TestClass]
    public class When_SettingInitDataOnCustomListenerBuilder : Given_CustomListenerInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            CustomListenerBuilder = CustomListenerBuilder.UsingInitData("init data");
        }

        [TestMethod]
        public void ThenConfiguredCustomTracelistnerHasInitData()
        {
            Assert.AreEqual("init data", GetCustomTraceListenerData().InitData);
        }
    }

    [TestClass]
    public class When_SettingSharedFormatterOnCustomListenerBuilder : Given_CustomListenerInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.CustomListenerBuilder =
                base.CustomListenerBuilder
                        .FormatWithSharedFormatter("formatter name");
        }

        [TestMethod]
        public void ThenTraceListenerHasFormatter()
        {
            var CustomTraceListener = base.GetCustomTraceListenerData();
            Assert.AreEqual("formatter name", CustomTraceListener.Formatter);
        }
    }

    [TestClass]
    public class When_SettingTraceOptionsOnCustomListenerBuilder : Given_CustomListenerInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.CustomListenerBuilder = base.CustomListenerBuilder
                                                    .WithTraceOptions(TraceOptions.Callstack);
        }

        [TestMethod]
        public void ThenConfiguredCustomTracelistnerHasTraceOptionCallstack()
        {
            Assert.AreEqual(TraceOptions.Callstack, GetCustomTraceListenerData().TraceOutputOptions);
        }
    }
}
