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
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Console.Wpf.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using System.Configuration;
using Console.Wpf.ViewModel.Services;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using System.ComponentModel;

namespace Console.Wpf.Tests.VSTS.DevTests.given_extension_provider_in_view_model
{
    [TestClass]
    public class when_adding_ext_properties_to_view_model : ExceptionHandlingSettingsContext
    {
        SectionViewModel ehabModel;
        SectionViewModel sectionWithExtendedPropertyProvider;

        protected override void Act()
        {
            ehabModel = SectionViewModel.CreateSection(ServiceProvider, Section);
            sectionWithExtendedPropertyProvider = SectionViewModel.CreateSection(ServiceProvider, new ConfigurationSectionWithExtendedPropertyProvider());
        }

        [TestMethod]
        public void then_element_extended_property_provider_can_be_found()
        {
            ElementLookup lookup = (ElementLookup)ServiceProvider.GetService(typeof(ElementLookup));
            var extensionProviders = lookup.FindExtendedPropertyProviders();
            Assert.IsTrue(extensionProviders.Count() > 0);
        }

        [TestMethod]
        public void then_extended_property_provider_can_add_properties_to_element()
        {
            var aWrapHandler = ehabModel.DescendentElements(x => x.ConfigurationType == typeof(WrapHandlerData)).First();
            Assert.IsTrue(aWrapHandler.Properties.Where(x => x.PropertyName == "Extended Property").Any());
        }
    }


    [ViewModel(typeof(SectionViewModelExtension))]
    public class ConfigurationSectionWithExtendedPropertyProvider : ConfigurationSection
    {
    }


    public class SectionViewModelExtension : SectionViewModel, IElementExtendedPropertyProvider
    {
        IServiceProvider serviceProvider;
        public SectionViewModelExtension(IServiceProvider serviceProvider, ConfigurationSection section)
            : base(serviceProvider, section)
        {
            this.serviceProvider = serviceProvider;
        }

        public bool CanExtend(ElementViewModel element)
        {
            return element.ConfigurationType == typeof(WrapHandlerData);
        }

        public IEnumerable<Property> GetExtendedProperties(ElementViewModel element)
        {
            if (element.ConfigurationType == typeof(WrapHandlerData))
            {
                yield return new Property(serviceProvider, element, new ExtendedPropertyDescriptor(element));
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
