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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.HostAdapter;

namespace Console.Wpf.Tests.VSTS.DevTests.given_host_adapter
{
    [TestClass]
    public class when_navigating_validation_error : given_host_adapter
    {
        ElementViewModel elementViewModel;
        Task validationError;

        protected override void Arrange()
        {
            base.Arrange();

            elementViewModel = CachingViewModel.GetDescendentsOfType<CacheManagerData>().Single();
            HostAdapter.TasksChanged += (sender, args) => validationError = args.Tasks.First();

            try
            {
                elementViewModel.Property("ExpirationPollFrequencyInSeconds").BindableProperty.BindableValue = "abc";
            }
            catch { }
        }

        protected override void Act()
        {
            HostAdapter.NavigateTask(validationError);
        }

        [TestMethod]
        public void then_element_view_model_is_selected()
        {
            Assert.IsTrue(elementViewModel.IsSelected);
        }

        [TestMethod]
        public void then_element_view_model_section_is_expanded()
        {
            Assert.IsTrue(elementViewModel.ContainingSection.IsExpanded);
        }

        [TestMethod]
        public void then_element_view_model_properties_are_shown()
        {
            Assert.IsTrue(elementViewModel.PropertiesShown);
        }
    }
}
