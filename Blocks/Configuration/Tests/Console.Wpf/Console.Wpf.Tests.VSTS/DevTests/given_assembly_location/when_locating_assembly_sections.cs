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
using System.Reflection;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Moq;

namespace Console.Wpf.Tests.VSTS.DevTests.given_assembly_location
{
    [TestClass]
    public class WhenLocatingAssemblySections : ArrangeActAssert
    {
        private AssemblyAttributeSectionLocator locator;
        

        protected override void Act()
        {
            var mockAssemblyLocator = new Mock<AssemblyLocator>();
            mockAssemblyLocator.Setup(x => x.Assemblies).Returns(new[] {this.GetType().Assembly});

            locator = new AssemblyAttributeSectionLocator(mockAssemblyLocator.Object);
        }


        [TestMethod]
        public void then_should_find_attributed_assemblies()
        {
            Assert.AreEqual(1, locator.ConfigurationSectionNames.Count(s => s == "testSection"));
        }

        [TestMethod]
        public void then_should_not_return_clear_only_sections()
        {
            Assert.IsFalse(locator.ConfigurationSectionNames.Any(s => s == "clearOnlySection"));
        }

        [TestMethod]
        public void then_should_return_all_sections()
        {
            Assert.AreEqual(2, locator.ClearableConfigurationSectionNames.Count());
        }
    }
}
