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
using System.Configuration;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using Moq;
using System.Windows;
using Microsoft.Practices.Unity;

namespace Console.Wpf.Tests.VSTS.DevTests.given_configuration_element_with_properties
{
    public abstract class given_configuration_properties_decorated_with_component_model_uielement_editor : given_configuration_properties_decorated_with_component_model_attributes
	{
        protected class ConfigurationElementWithComponentModelEditor : ConfigurationElementWithComponentModelAttributes
        {
            public const string Prop3Name = "prop3";

            [ConfigurationProperty(Prop3Name)]
            [System.ComponentModel.Editor(typeof(System.Windows.Controls.Button), typeof(FrameworkElement))]
            public string Prop3
            {
                get { return (string)base[Prop3Name]; }
                set { base[Prop3Name] = value; }
            }
        }
	}


    [TestClass]
    public class when_discovering_properties_with_uielement_editor : given_configuration_properties_decorated_with_component_model_uielement_editor
    {
        private static object editorInstance;
        ElementProperty property;

        protected override void Act()
        {
            Container.RegisterInstance<IWindowsFormsEditorService>(new Mock<IWindowsFormsEditorService>().Object);
            Container.RegisterInstance<IUIService>(new Mock<IUIService>().Object);

            var sectionModel = SectionViewModel.CreateSection(Container,"mock section", new ConfigurationElementWithComponentModelEditor());

            var originalPropertyDescriptor = TypeDescriptor.GetProperties(typeof(ConfigurationElementWithComponentModelEditor)).OfType<PropertyDescriptor>().Where(x => x.Name == "Prop3").First();
            property = sectionModel.CreateElementProperty(sectionModel, new PropertyDescriptorReturnEditor(originalPropertyDescriptor));
        }

        [TestMethod]
        public void then_editor_specified_in_editor_attributes_sets_has_editor()
        {
            Assert.IsInstanceOfType(property.BindableProperty, typeof(FrameworkEditorBindableProperty));

            FrameworkEditorBindableProperty frameworkEditorBindable = (FrameworkEditorBindableProperty)property.BindableProperty;
            Assert.AreNotSame(editorInstance, frameworkEditorBindable.CreateEditorInstance());
            editorInstance = frameworkEditorBindable.CreateEditorInstance();
        }

        [TestMethod]
        public void then_editor_is_returned_as_ui_element()
        {
            FrameworkEditorBindableProperty frameworkEditorBindable = (FrameworkEditorBindableProperty)property.BindableProperty;
            
            Assert.IsNotNull(frameworkEditorBindable.CreateEditorInstance());
            Assert.AreNotSame(editorInstance, frameworkEditorBindable.CreateEditorInstance());
            editorInstance = frameworkEditorBindable.CreateEditorInstance();
        }


        [TestMethod]
        public void then_editor_data_context_is_BindableProperty()
        {
            FrameworkEditorBindableProperty frameworkEditorBindable = (FrameworkEditorBindableProperty)property.BindableProperty;

            Assert.IsInstanceOfType(frameworkEditorBindable.CreateEditorInstance().DataContext, typeof(BindableProperty));
        }

        private class PropertyDescriptorReturnEditor : PropertyDescriptor
        {
            PropertyDescriptor actual;

            public PropertyDescriptorReturnEditor(PropertyDescriptor actual)
                :base(actual)
            {
                this.actual = actual;
            }

            public override object GetEditor(Type editorBaseType)
            {
                if (editorBaseType == typeof(FrameworkElement))
                {
                    return new System.Windows.Controls.Button();
                }
                return base.GetEditor(editorBaseType);
            }

            public override bool CanResetValue(object component)
            {
                return actual.CanResetValue(component);
            }

            public override Type ComponentType
            {
                get { return actual.ComponentType; }
            }

            public override object GetValue(object component)
            {
                return actual.GetValue(component);
            }

            public override bool IsReadOnly
            {
                get { return actual.IsReadOnly; }
            }

            public override Type PropertyType
            {
                get { return actual.PropertyType; }
            }

            public override void ResetValue(object component)
            {
                actual.ResetValue(component);
            }

            public override void SetValue(object component, object value)
            {
                actual.SetValue(component, value);
            }

            public override bool ShouldSerializeValue(object component)
            {
                return actual.ShouldSerializeValue(component);
            }
        }
    }
}
