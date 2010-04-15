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

namespace Console.Wpf.Tests.VSTS.DevTests.given_selected_source_and_redirected_sections
{
    [TestClass]
    public class when_validating_sources_section : given_selected_source_and_redirects
    {
        protected override void Act()
        {
            base.SourcesSectionViewModel.Validate();
        }

        [TestMethod]
        public void then_selected_source_property_has_validation_warning()
        {
            var selectedSourceProperty = base.SourcesSectionViewModel.Properties.Where(x=>x.PropertyName == "SelectedSource").Single();
            var warning = selectedSourceProperty.ValidationResults.Where(x=>x.IsWarning).Single();
            Assert.IsTrue(warning.Message.Contains("Redirect"));
        }
    }
}
