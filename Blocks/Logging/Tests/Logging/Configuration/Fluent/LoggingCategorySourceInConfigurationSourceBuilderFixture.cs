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
    public abstract class Given_LoggingCategorySourceInConfigurationSourceBuilder : Given_LoggingSettingsInConfigurationSourceBuilder
    {
        private string categoryName = "category";
        protected ILoggingConfigurationCustomCategoryStart CategorySourceBuilder;

        protected override void Arrange()
        {
            base.Arrange();

            CategorySourceBuilder = ConfigureLogging.LogToCategoryNamed(categoryName);
        }

        protected TraceSourceData GetTraceSourceData()
        {
            return base.GetLoggingConfiguration().TraceSources.Where(x => x.Name == categoryName).First();
        }
    }


    [TestClass]
    public class When_CallingLogToCategoryNamedPassingNullCategoryName : Given_LoggingSettingsInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_LogToCategoryNamed_ThrowsArgumentException()
        {
            ConfigureLogging.LogToCategoryNamed(null);
        }
    }

    [TestClass]
    public class When_CallingSetAsDefaultOnCategorySource : Given_LoggingCategorySourceInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            CategorySourceBuilder.WithOptions.SetAsDefaultCategory();
        }

        [TestMethod]
        public void Then_LoggingConfigurationHasCategoryAsDefaultCategory()
        {
            Assert.AreEqual(GetTraceSourceData().Name, GetLoggingConfiguration().DefaultCategory);
        }
    }

    [TestClass]
    public class When_CallingdoNotAutoFlushOnCategorySource : Given_LoggingCategorySourceInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            CategorySourceBuilder.WithOptions.DoNotAutoFlushEntries();
        }

        [TestMethod]
        public void Then_LoggingConfigurationHasCategoryAsDefaultCategory()
        {
            Assert.IsFalse(GetTraceSourceData().AutoFlush);
        }
    }

    [TestClass]
    public class When_CallingSendToSharedListenerOnLogToCategoryConfigurationBuilder : Given_LoggingCategorySourceInConfigurationSourceBuilder
    {
        protected override void Arrange()
        {
            base.Arrange();
            CategorySourceBuilder.SendTo.SharedListenerNamed("shared listener");
        }

        [TestMethod]
        public void Then_CategorySourceContainsTraceListenerReference()
        {
            Assert.AreEqual(1, GetTraceSourceData().TraceListeners.Count);
        }

        [TestMethod]
        public void Then_TraceListenerReferenceHasAppropriateName()
        {
            Assert.AreEqual("shared listener", GetTraceSourceData().TraceListeners.First().Name);
        }
    }

}
