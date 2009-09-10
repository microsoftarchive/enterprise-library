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
using System.ComponentModel;

namespace Console.Wpf.Tests.VSTS.DevTests.given_property_grid_consuming_view_model
{
    [TestClass]
    public class when_property_grid_consumes_model_element : ExceptionHandlingSettingsContext
    {
        SectionViewModel model;

        protected override void Act()
        {
            model = SectionViewModel.CreateSection(ServiceProvider, Section);
        }

        private void AssertForAllProperties(Action<PropertyDescriptor, Property> assertion)
        {
            var allProperties = model.DescendentElements().Select(x => new { Element = x, Properties = x.Properties });
            foreach (var propertyContext in allProperties)
            {
                var PropertiesForGrid = TypeDescriptor.GetProperties(propertyContext.Element).OfType<PropertyDescriptor>();

                foreach (Property propertyFromModel in propertyContext.Properties)
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
        public void then_all_converters_match()
        {
            AssertForAllProperties(
                (propertyForGrid, propertyFromModel)
                    => Assert.AreEqual(propertyFromModel.Converter.GetType(), propertyForGrid.Converter.GetType()));
        }

        //[TestMethod]
        //public void then_all_converters_match()
        //{
        //    AssertForAllProperties(
        //        (propertyForGrid, propertyFromModel)
        //            => Assert.AreEqual(propertyFromModel.Converter.GetType(), propertyForGrid.Converter.GetType()));
        //}


        //[TestMethod]
        //public void then_all_properties_with_child_properties_have_child_properties()
        //{
        //    AssertForAllProperties(
        //        (propertyForGrid, propertyFromModel) => 
        //        {
        //            if (propertyFromModel.HasChildProperties)
        //            {
        //                Assert.IsNotNull(propertyForGrid.GetChildProperties());

        //                Assert.AreEqual(propertyFromModel.ChildProperties.Count(), propertyForGrid.GetChildProperties().Count);
        //            }
        //        });
        //}
    }
}
