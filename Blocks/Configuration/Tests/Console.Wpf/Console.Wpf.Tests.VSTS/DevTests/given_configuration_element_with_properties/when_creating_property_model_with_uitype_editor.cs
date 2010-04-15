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
using Microsoft.Practices.Unity;


namespace Console.Wpf.Tests.VSTS.DevTests.given_configuration_element_with_properties
{
    public abstract class given_configuration_properties_decorated_with_component_model_uitype_editor : given_configuration_properties_decorated_with_component_model_attributes
	{
        protected class ConfigurationElementWithComponentModelEditor : ConfigurationElementWithComponentModelAttributes
        {
            public const string Prop3Name = "prop3";

            [ConfigurationProperty(Prop3Name)]
            [System.ComponentModel.Editor(typeof(Prop3Editor), typeof(UITypeEditor))]
            public string Prop3
            {
                get { return (string)base[Prop3Name]; }
                set { base[Prop3Name] = value; }
            }
        }

        protected class Prop3Editor : UITypeEditor
        {
            public static object NewValue;
            public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
            {
                return UITypeEditorEditStyle.Modal;
            }

            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                Assert.IsNotNull(context);
                Assert.IsNotNull(provider);

                Assert.IsNotNull(provider.GetService(typeof(IWindowsFormsEditorService)));
                Assert.IsNotNull(provider.GetService(typeof(IUIService)));

                if (NewValue != null)
                {
                    return NewValue;
                }
                return value;
            }
        }
	}


    [TestClass]
    public class when_discovering_properties_with_component_model_editor : given_configuration_properties_decorated_with_component_model_uitype_editor
    {
        IEnumerable<Property> properties;

        protected override void Act()
        {
            Container.RegisterInstance<IWindowsFormsEditorService>(new Mock<IWindowsFormsEditorService>().Object);
            Container.RegisterInstance<IUIService>(new Mock<IUIService>().Object);

            var sectionModel = SectionViewModel.CreateSection(Container, "mockSection", new ConfigurationElementWithComponentModelEditor());

            properties = sectionModel.Properties;
        }


        [TestMethod]
        public void then_editor_specified_in_editor_attributes_sets_has_editor()
        {
            var Prop3 = properties.Where(x => x.PropertyName == "Prop3").FirstOrDefault();
            Assert.IsInstanceOfType(Prop3.BindableProperty, typeof(PopupEditorBindableProperty));

        }

        [TestMethod]
        public void then_editor_is_invoked_when_edit_value_is_called()
        {
            var Prop3 = properties.Where(x => x.PropertyName == "Prop3").FirstOrDefault();
            ((PopupEditorBindableProperty)Prop3.BindableProperty).LaunchEditor.Execute(null);
        }


        [TestMethod]
        public void then_show_ui_type_editor_sets_value()
        {
            Prop3Editor.NewValue = "NewValue";

            var Prop3 = properties.Where(x => x.PropertyName == "Prop3").FirstOrDefault();
            ((PopupEditorBindableProperty)Prop3.BindableProperty).LaunchEditor.Execute(null);

            Assert.AreEqual("NewValue", Prop3.Value);
        }

    }
}
