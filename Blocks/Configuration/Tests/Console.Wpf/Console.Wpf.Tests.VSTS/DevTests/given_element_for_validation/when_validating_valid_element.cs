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
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Console.Wpf.Tests.VSTS.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Specialized;

namespace Console.Wpf.Tests.VSTS.DevTests.given_element_for_validation
{
    [TestClass]
    public class when_validating_valid_element : ContainerContext
    {
        private SectionViewModel sectionModel;
        private bool elementValidationErrorsCollectionChanged;
        private bool propertyValidationErrorsCollectionChanged;

        protected override void Arrange()
        {
            base.Arrange();

            var section = new MockSectionWithSingleChild();

            sectionModel = SectionViewModel.CreateSection(Container, "mock section", section);
            
            elementValidationErrorsCollectionChanged = false;
            INotifyCollectionChanged elementValidationErrorsChanged = sectionModel.ValidationResults as INotifyCollectionChanged;
            elementValidationErrorsChanged.CollectionChanged += (sender, args) => elementValidationErrorsCollectionChanged = true;

            propertyValidationErrorsCollectionChanged = false;
            INotifyCollectionChanged propertyValidationErrorsChanged = sectionModel.Properties.First().ValidationResults as INotifyCollectionChanged;
            propertyValidationErrorsChanged.CollectionChanged += (sender, args) => propertyValidationErrorsCollectionChanged = true;

        }

        protected override void  Act()
        {
            sectionModel.Validate();         	
        }

        [TestMethod]
        public void then_element_has_no_validation_errors()
        {
            Assert.AreEqual(0, sectionModel.ValidationResults.Count());
        }

        [TestMethod]
        public void then_element_validation_errors_collection_didnt_change()
        {
            Assert.IsFalse(elementValidationErrorsCollectionChanged);
        }

        [TestMethod]
        public void then_property_validation_errors_collection_didnt_change()
        {
            Assert.IsFalse(propertyValidationErrorsCollectionChanged);
        }
    }
}
