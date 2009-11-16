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
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using System.Reflection;

namespace Console.Wpf.Tests.VSTS.DevTests.given_assembly_location
{
    
    [TestClass]
    public class when_locating_assemblies_from_current_directory : ArrangeActAssert
    {
        private BinPathProbingAssemblyLocator locator;

        protected override void Act()
        {
            locator = new BinPathProbingAssemblyLocator();
        }

        [TestMethod]
        public void then_will_find_test_assembly()
        {
            Assert.AreEqual(1, locator.Assemblies.Count(a => a.FullName == this.GetType().Assembly.FullName));
        }

        [TestMethod]
        public void then_will_find_exception_settings()
        {
            Assert.AreEqual(1, locator.Assemblies.Count(a => a.GetName().Name == "Microsoft.Practices.EnterpriseLibrary.ExceptionHandling"));
        }
    }

}
