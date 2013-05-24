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
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.HostAdapter;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

            elementViewModel = LoggingViewModel.GetDescendentsOfType<TraceListenerData>().Single();
            HostAdapter.TasksChanged += (sender, args) => validationError = args.Tasks.First();

            try
            {
                elementViewModel.Property("Filter").BindableProperty.BindableValue = "abc";
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
