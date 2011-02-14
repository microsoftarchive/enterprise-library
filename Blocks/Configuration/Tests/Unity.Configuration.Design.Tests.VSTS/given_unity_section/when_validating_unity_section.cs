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
using Console.Wpf.Tests.VSTS.BlockSpecific.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.Unity.Configuration.Design.Tests
{
    [TestClass]
    public class when_validating_unity_section : given_unity_section
    {
        protected override void Act()
        {
            base.UnitySectionViewModel.Validate();
        }

        [TestMethod]
        public void then_unity_section_has_no_errors()
        {
            var numberOfPropertiesWithErrors = base.UnitySectionViewModel.Properties.Where(x => x.ValidationResults.Count() > 0).Count();
            
            Assert.AreEqual(0, numberOfPropertiesWithErrors);
        }
    }
}
