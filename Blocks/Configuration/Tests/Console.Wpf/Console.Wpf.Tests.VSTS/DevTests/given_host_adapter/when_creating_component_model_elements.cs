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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Hosting;
using Microsoft.Practices.Unity;
using System.Windows;
using System.Drawing.Design;

namespace Console.Wpf.Tests.VSTS.DevTests.given_property_grid_consuming_view_model
{
    [TestClass]
    public class when_property_grid_consumes_model_element : ExceptionHandlingSettingsContext
    {
        SectionViewModel model;

        protected override void Act()
        {
            model = SectionViewModel.CreateSection(Container, "section", Section);
        }

        private void AssertForAllProperties(Action<PropertyDescriptor, Property> assertion)
        {
            var allProperties = model.DescendentElements().Select(x => new { Element = x, Properties = x.Properties });
            foreach (var propertyContext in allProperties)
            {
                var PropertiesForGrid = TypeDescriptor.GetProperties(new ComponentModelElement( propertyContext.Element, Container.Resolve<IServiceProvider>())).OfType<PropertyDescriptor>();

                foreach (Property propertyFromModel in propertyContext.Properties.Where(x=>!x.Hidden))
                {
                    var propertyforGrid = PropertiesForGrid.Where(x => x.Name == propertyFromModel.PropertyName).FirstOrDefault();

                    assertion(propertyforGrid, propertyFromModel);
                }
            }
        }

        [TestMethod]
        public void then_every_model_property_is_displayed_in_grid()
        {
            AssertForAllProperties(
                (propertyForGrid, propertyFromModel)
                    => Assert.IsNotNull(propertyForGrid));
        }

        [TestMethod]
        public void then_all_display_names_match()
        {
            AssertForAllProperties(
                (propertyForGrid, propertyFromModel)
                    => Assert.AreEqual(propertyFromModel.DisplayName, propertyForGrid.DisplayName));
        }

        [TestMethod]
        public void then_all_categories_match()
        {
            AssertForAllProperties(
                (propertyForGrid, propertyFromModel)
                    => Assert.AreEqual(propertyFromModel.Category, propertyForGrid.Category));
        }


        [TestMethod]
        public void then_all_descriptions_match()
        {
            AssertForAllProperties(
                (propertyForGrid, propertyFromModel)
                    => Assert.AreEqual(propertyFromModel.Description, propertyForGrid.Description));
        }



        [TestMethod]
        public void then_properties_with_framework_element_are_wrapped_in_ui_type_editor()
        {
            bool propertyFound = false;
            AssertForAllProperties(
                (propertyForGrid, propertyFromModel)
                    =>
                {
                    if (propertyFromModel.Attributes.OfType<EditorAttribute>().Where(x => Type.GetType(x.EditorBaseTypeName) == typeof(FrameworkElement)).Any())
                    {
                        propertyFound = true;

                        var editor = (UITypeEditor) propertyForGrid.GetEditor(typeof(UITypeEditor));
                        Assert.IsNotNull(editor);
                        Assert.IsTrue(editor.IsDropDownResizable);
                        Assert.AreEqual(UITypeEditorEditStyle.DropDown, editor.GetEditStyle());
                    }
                });

            Assert.IsTrue(propertyFound);
        }
    }
}
