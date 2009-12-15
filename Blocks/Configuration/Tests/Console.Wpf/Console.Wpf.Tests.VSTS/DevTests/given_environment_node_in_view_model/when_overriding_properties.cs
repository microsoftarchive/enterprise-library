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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides.Configuration;
using Console.Wpf.Tests.VSTS.TestSupport;

namespace Console.Wpf.Tests.VSTS.DevTests.given_environment_node_in_view_model
{
    [TestClass]
    public class when_overriding_properties : given_environmental_overrides_and_ehab
    {
        Property overridenWrapTypeProperty;
        PropertyChangedListener overridenWrapTypePropertyChangedListener;
        protected override void Act()
        {
            overridenWrapTypeProperty = base.OverridesProperty.ChildProperties.Where(x => x.PropertyName == "WrapExceptionTypeName").FirstOrDefault();

            overridenWrapTypePropertyChangedListener = new PropertyChangedListener(overridenWrapTypeProperty.BindableProperty);

            OverridesProperty.Value = OverridesProperty.Converter.ConvertFromString(OverridesProperty, "Override Properties");
            
        }

        [TestMethod]
        public void then_child_properties_are_read_write()
        {
            Assert.IsFalse(OverriddenExceptionMessage.BindableProperty.ReadOnly);
        }

        [TestMethod]
        public void then_child_property_readonly_changed()
        {
            Assert.IsTrue(overridenWrapTypePropertyChangedListener.ChangedProperties.Contains("ReadOnly"));
        }


        [TestMethod]
        public void then_environmental_merge_node_doesnt_have_overrides()
        {
            Assert.IsFalse(base.EnvironmentViewModel.Properties.Where(x => x.PropertyName.Contains("Overrides")).Any());
        }

        [TestMethod]
        public void then_overridden_property_has_value_from_main_property()
        {
            Assert.AreEqual(MainExceptionMessage.Value, OverriddenExceptionMessage.Value);
        }

        [TestMethod]
        public void then_setting_main_property_doesnt_update_overridden_property()
        {
            string oldValue = (string) MainExceptionMessage.Value;
            MainExceptionMessage.Value = "New Root Value";

            Assert.AreEqual(oldValue, OverriddenExceptionMessage.Value);
        }

        [TestMethod]
        public void then_base_class_attribute_can_be_accessed_through_overriden_property()
        {
            Assert.IsTrue(overridenWrapTypeProperty.Attributes.OfType<BaseTypeAttribute>().Any());
        }

        [TestMethod]
        public void then_overridden_value_is_set_on_environmet_section()
        {
            OverriddenExceptionMessage.Value = "new exception message";

            var mergeElements = EnvironmentSection.MergeElements.OfType<EnvironmentNodeMergeElement>();
            var mergeRecord = mergeElements.Where(x => x.ConfigurationNodePath == base.WrapHandler.Path).FirstOrDefault();

            Assert.IsNotNull(mergeRecord);
            Assert.IsTrue(mergeRecord.OverriddenProperties.AllKeys.Contains("exceptionMessage"));
            Assert.AreEqual("new exception message", mergeRecord.OverriddenProperties["exceptionMessage"].Value);
        }

        [TestMethod]
        public void then_merge_record_has_appropriate_path_after_name_change()
        {
            OverriddenExceptionMessage.Value = "new exception message";
            WrapHandler.Property("Name").Value = "New Name";

            var mergeElements = EnvironmentSection.MergeElements.OfType<EnvironmentNodeMergeElement>();
            var mergeRecord = mergeElements.Where(x => x.ConfigurationNodePath == base.WrapHandler.Path).FirstOrDefault();

            Assert.IsNotNull(mergeRecord);
        }

        [TestMethod]
        public void then_merge_record_is_deleted_after_element_is_deleted()
        {
            OverriddenExceptionMessage.Value = "new exception message";
            ((ElementCollectionViewModel)WrapHandler.ParentElement).Delete((CollectionElementViewModel)WrapHandler);

            var mergeElements = EnvironmentSection.MergeElements.OfType<EnvironmentNodeMergeElement>();
            var mergeRecord = mergeElements.Where(x => x.ConfigurationNodePath == base.WrapHandler.Path).FirstOrDefault();

            Assert.IsNull(mergeRecord);
        }

    }
}
