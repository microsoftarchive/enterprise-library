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

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using System.ComponentModel;
using Microsoft.Practices.Unity;

namespace Console.Wpf.Tests.VSTS.DevTests.given_extension_provider_in_view_model
{
    [TestClass]
    public class when_adding_ext_properties_to_view_model : ExceptionHandlingSettingsContext
    {
        SectionViewModel ehabModel;
        SectionViewModel sectionWithExtendedPropertyProvider;

        protected override void Act()
        {
            var configurationSourceModel = Container.Resolve<ConfigurationSourceModel>();
            ehabModel = configurationSourceModel.AddSection(ExceptionHandlingSettings.SectionName, Section);
            sectionWithExtendedPropertyProvider = configurationSourceModel.AddSection("extended", new ConfigurationSectionWithExtendedPropertyProvider());
        }

        [TestMethod]
        public void then_element_extended_property_provider_can_be_found()
        {
            ElementLookup lookup = (ElementLookup)Container.Resolve<ElementLookup>();
            var extensionProviders = lookup.FindExtendedPropertyProviders();
            Assert.IsTrue(extensionProviders.Count() > 0);
        }

        [TestMethod]
        public void then_extended_property_provider_can_add_properties_to_element()
        {
            var aWrapHandler = ehabModel.DescendentElements(x => x.ConfigurationType == typeof(WrapHandlerData)).First();
            Assert.IsTrue(aWrapHandler.Properties.Where(x => x.PropertyName == "Extended Property").Any());
        }

        [TestMethod]
        public void then_extended_properties_are_discovered_after_properties_collection_was_accessed()
        {
            var aWrapHandler = ehabModel.DescendentElements(x => x.ConfigurationType == typeof(WrapHandlerData)).First();

            var sourceModel = Container.Resolve<ConfigurationSourceModel>();
            sectionWithExtendedPropertyProvider = sourceModel.AddSection("mock section", new ConfigurationSectionWithExtendedPropertyProvider());
            Assert.AreEqual(2, aWrapHandler.Properties.Where(x => x.PropertyName == "Extended Property").Count());
        }

        [TestMethod]
        public void then_extended_properties_value_can_be_accessed()
        {
            var aWrapHandler = ehabModel.DescendentElements(x => x.ConfigurationType == typeof(WrapHandlerData)).First();
            var properties = aWrapHandler.Properties.Where(x => x.PropertyName == "Extended Property").First();

            properties.Value = "SomeValue";
            Assert.AreEqual("SomeValue", properties.Value);
        }
    }


    [ViewModel(typeof(SectionViewModelExtension))]
    public class ConfigurationSectionWithExtendedPropertyProvider : ConfigurationSection
    {
    }

    public class SectionViewModelExtension : SectionViewModel, IElementExtendedPropertyProvider
    {
        IUnityContainer builder;
        public SectionViewModelExtension(IUnityContainer builder, ConfigurationSection section)
            : base(builder, "sectionName", section)
        {
            this.builder = builder;
        }

        public bool CanExtend(ElementViewModel element)
        {
            return element.ConfigurationType == typeof(WrapHandlerData);
        }

        public IEnumerable<Property> GetExtendedProperties(ElementViewModel element)
        {
            if (element.ConfigurationType == typeof(WrapHandlerData))
            {
                yield return new Property(builder.Resolve<IServiceProvider>(), this, new ExtendedPropertyDescriptor(element));
            }
        }

        private Dictionary<ElementViewModel, string> ExtendedProperties = new Dictionary<ElementViewModel, string>();

        private class ExtendedPropertyDescriptor : PropertyDescriptor
        {
            ElementViewModel subject;
            public ExtendedPropertyDescriptor(ElementViewModel subject)
                :base("Extended Property", new Attribute[0])
            {
                this.subject = subject;
            }

            public override bool CanResetValue(object component)
            {
                return false;
            }

            public override Type ComponentType
            {
                get { return typeof(ElementViewModel); }
            }

            public override object GetValue(object component)
            {
                string value= string.Empty;
                ((SectionViewModelExtension)component).ExtendedProperties.TryGetValue(subject, out value);
                return value;
            }

            public override bool IsReadOnly
            {
                get { return false; }
            }

            public override Type PropertyType
            {
                get { return typeof(string); }
            }

            public override void ResetValue(object component)
            {
                
            }

            public override void SetValue(object component, object value)
            {
                ((SectionViewModelExtension)component).ExtendedProperties[subject] = (string)value;
            }

            public override bool ShouldSerializeValue(object component)
            {
                return false;
            }
        }
    }
}
