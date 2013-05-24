//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Linq;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Console.Wpf.Tests.VSTS.DevTests.given_invalid_model
{
    public abstract class LoggingElementModelContext : ContainerContext
    {
        private DictionaryConfigurationSource configSource;

        protected const string DefaultCategoryNameName = TestConfigurationBuilder.DefaultCategoryName;

        protected LoggingSettings LoggingConfiguration
        {
            get
            {
                return (LoggingSettings)configSource.GetSection(BlockSectionNames.Logging);
            }
        }

        protected override void Arrange()
        {
            base.Arrange();

            var builder = new TestConfigurationBuilder();
            configSource = new DictionaryConfigurationSource();
            builder.AddLoggingSettings()
                .Build(configSource);

            var configurationSourceModel = Container.Resolve<ConfigurationSourceModel>();
            var source = new DesignDictionaryConfigurationSource();
            source.Add(BlockSectionNames.Logging, LoggingConfiguration);
            configurationSourceModel.Load(source);

            LoggingSettingsViewModel =
                configurationSourceModel.Sections
                    .Where(x => x.ConfigurationType == typeof(LoggingSettings)).Single();
        }

        protected SectionViewModel LoggingSettingsViewModel { get; private set; }
    }

    [TestClass]
    public class when_invalidating_logging_section : LoggingElementModelContext
    {
        private ValidationModel validationModel;
        private ElementViewModel traceListener;

        protected override void Arrange()
        {
            base.Arrange();
            validationModel = Container.Resolve<ValidationModel>();
            traceListener = LoggingSettingsViewModel.DescendentElements(x => x.ConfigurationType == typeof(FormattedEventLogTraceListenerData)).First();
        }

        protected override void Act()
        {
            var bindableProperty = traceListener.Property("TraceOutputOptions").BindableProperty;
            bindableProperty.BindableValue = "Invalid";
        }

        [TestMethod]
        public void then_only_one_validation_error_message()
        {
            Assert.AreEqual(1, validationModel.ValidationResults.Count());
        }
    }

    [TestClass]
    public class when_adding_additional_elements_to_invalid_mdoel : LoggingElementModelContext
    {
        private ValidationModel validationModel;
        private ElementCollectionViewModel cacheManagerCollection;
        private int originalCount;

        protected override void Arrange()
        {
            base.Arrange();
            validationModel = Container.Resolve<ValidationModel>();

            cacheManagerCollection = LoggingSettingsViewModel.GetDescendentsOfType<TraceListenerDataCollection>().OfType<ElementCollectionViewModel>().Single();
            var newItem = cacheManagerCollection.AddNewCollectionElement(typeof(FlatFileTraceListenerData));
            newItem.Property("Name").BindableProperty.BindableValue = "";

            originalCount = validationModel.ValidationResults.Count();
        }

        protected override void Act()
        {
            var newItem = cacheManagerCollection.AddNewCollectionElement(typeof(FlatFileTraceListenerData));
        }

        [TestMethod]
        public void then_validation_model_retains_errors()
        {
            Assert.AreEqual(originalCount, validationModel.ValidationResults.Count());
        }
    }
}
