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
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Database.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Database.Tests.Configuration
{

    public abstract class Given_LoggingCategorySourceInConfigurationSourceBuilder : ArrangeActAssert
    {
        private string categoryName = "category";
        protected ILoggingConfigurationCustomCategoryStart CategorySourceBuilder;

        protected ILoggingConfigurationStart ConfigureLogging;
        protected ConfigurationSourceBuilder ConfigurationSourceBuilder;

        protected override void Arrange()
        {
            ConfigurationSourceBuilder = new ConfigurationSourceBuilder();
            ConfigureLogging = ConfigurationSourceBuilder.ConfigureLogging();
            CategorySourceBuilder = ConfigureLogging.LogToCategoryNamed(categoryName);
        }


        protected LoggingSettings GetLoggingConfiguration()
        {
            var configurationSource = GetConfigurationSource();
            var loggingSettings = (LoggingSettings)configurationSource.GetSection(LoggingSettings.SectionName);

            return loggingSettings;
        }

        protected IConfigurationSource GetConfigurationSource()
        {
            DictionaryConfigurationSource configSource = new DictionaryConfigurationSource();
            ConfigurationSourceBuilder.UpdateConfigurationWithReplace(configSource);

            return configSource;
        }

        protected TraceSourceData GetTraceSourceData()
        {
            return GetLoggingConfiguration().TraceSources.Where(x => x.Name == categoryName).First();
        }
    }

    public abstract class Given_DatabaseListenerInConfigurationSourceBuilder : Given_LoggingCategorySourceInConfigurationSourceBuilder
    {
        protected ILoggingConfigurationSendToDatabaseTraceListener databaseListenerBuilder;
        private string databaseFileListenerName = "Database listener";

        protected override void Arrange()
        {
            base.Arrange();

            databaseListenerBuilder = base.CategorySourceBuilder.SendTo.Database(databaseFileListenerName);
        }

        protected FormattedDatabaseTraceListenerData GetDatabaseTraceListenerData()
        {
            return base.GetLoggingConfiguration().TraceListeners.OfType<FormattedDatabaseTraceListenerData>()
                .Where(x => x.Name == databaseFileListenerName).First();
        }
    }

    [TestClass]
    public class When_CallingSendToDatabaseListenerPassingNullForName : Given_LoggingCategorySourceInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_SendToDatabase_ThrowsArgumentException()
        {
            base.CategorySourceBuilder.SendTo.Database(null);
        }
    }

    [TestClass]
    public class When_CallingSendToDatabaseListenerOnLogToCategoryConfigurationBuilder : Given_DatabaseListenerInConfigurationSourceBuilder
    {
        [TestMethod]
        public void ThenTraceOptionsIsNone()
        {
            Assert.AreEqual(TraceOptions.None, GetDatabaseTraceListenerData().TraceOutputOptions);
        }

        [TestMethod]
        public void ThenFilterIsAll()
        {
            Assert.AreEqual(SourceLevels.All, GetDatabaseTraceListenerData().Filter);
        }


        [TestMethod]
        public void ThenDatabaseTraceListenerDataHasAppropriateType()
        {
            Assert.AreEqual(typeof(FormattedDatabaseTraceListener), GetDatabaseTraceListenerData().Type);
        }

        [TestMethod]
        public void ThenCategortyContainsTraceListenerReference()
        {
            Assert.AreEqual(GetDatabaseTraceListenerData().Name, GetTraceSourceData().TraceListeners.First().Name);
        }

        [TestMethod]
        public void ThenLoggingConfigurationContainsTraceListener()
        {
            Assert.IsTrue(GetLoggingConfiguration().TraceListeners.OfType<FormattedDatabaseTraceListenerData>().Any());
        }

        [TestMethod]
        public void ThenAddCategoryStoredProcIsAddCategory()
        {
            Assert.AreEqual("AddCategory", GetDatabaseTraceListenerData().AddCategoryStoredProcName);
        }

        [TestMethod]
        public void ThenWriteLogStoredProcIsWriteLog()
        {
            Assert.AreEqual("WriteLog", GetDatabaseTraceListenerData().WriteLogStoredProcName);
        }
    }

    [TestClass]
    public class When_SettingTraceOptionForDatabaseTraceListener : Given_DatabaseListenerInConfigurationSourceBuilder
    {
        TraceOptions trOption;
        protected override void Act()
        {
            trOption = TraceOptions.Callstack | TraceOptions.DateTime;
            base.databaseListenerBuilder.WithTraceOptions(trOption);

        }

        [TestMethod]
        public void ThenConfigurationReflectsTraceOption()
        {
            Assert.AreEqual(trOption, base.GetDatabaseTraceListenerData().TraceOutputOptions);
        }
    }

    [TestClass]
    public class When_SettingFilterForDatabaseTraceListener : Given_DatabaseListenerInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.databaseListenerBuilder.Filter(SourceLevels.Error);
        }

        [TestMethod]
        public void ThenConfigurationReflectsTraceOption()
        {
            Assert.AreEqual(SourceLevels.Error, base.GetDatabaseTraceListenerData().Filter);
        }
    }

    [TestClass]
    public class When_SettingAddCategorySprocForDatabaseTraceListener : Given_DatabaseListenerInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.databaseListenerBuilder.WithAddCategoryStoredProcedure("addCat");
        }

        [TestMethod]
        public void ThenConfigurationHasAddCategorySproc()
        {
            Assert.AreEqual("addCat", base.GetDatabaseTraceListenerData().AddCategoryStoredProcName);
        }
    }

    [TestClass]
    public class When_SettingCategorySprocForDatabaseTraceListenerToNull : Given_DatabaseListenerInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_WithAddCategoryStoredProcedure_ThrowsArgumentException()
        {
            base.databaseListenerBuilder.WithAddCategoryStoredProcedure(null);
        }
    }

    [TestClass]
    public class When_SettingWriteLogSprocForDatabaseTraceListener : Given_DatabaseListenerInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.databaseListenerBuilder.WithWriteLogStoredProcedure("WriteL");
        }

        [TestMethod]
        public void ThenConfigurationHasWriteLogSproc()
        {
            Assert.AreEqual("WriteL", base.GetDatabaseTraceListenerData().WriteLogStoredProcName);
        }
    }

    [TestClass]
    public class When_SettingWriteLogSprocForDatabaseTraceListenerToNull : Given_DatabaseListenerInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_WithWriteLogStoredProcedure_ThrowsArgumentException()
        {
            base.databaseListenerBuilder.WithWriteLogStoredProcedure(null);
        }
    }


    [TestClass]
    public class When_CallingFormatWithOnSendToDatabaseListenerPassingNull : Given_DatabaseListenerInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Then_FormatWith_ThrowsArgumentNullException()
        {
            base.databaseListenerBuilder.FormatWith(null);
        }
    }

    [TestClass]
    public class When_SettingDatabaseInstanceForDatabaseTraceListener : Given_DatabaseListenerInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.databaseListenerBuilder.UseDatabase("db12");
        }

        [TestMethod]
        public void ThenConfigurationHasDatabaseInstance()
        {
            Assert.AreEqual("db12", base.GetDatabaseTraceListenerData().DatabaseInstanceName);
        }
    }
}
