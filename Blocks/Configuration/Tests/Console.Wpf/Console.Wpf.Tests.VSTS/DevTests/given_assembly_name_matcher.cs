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
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Console.Wpf.Tests.VSTS.DevTests
{
    [TestClass]
    public class given_assembly_name_matcher
    {
        [TestMethod]
        public void WhenAssemblyIsNull_TheReturnsFalse()
        {
            var result = AssemblyNameMatcher.Matches(null, new AssemblyName());

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void WhenAssemblyNameIsNull_TheReturnsNull()
        {
            var result = AssemblyNameMatcher.Matches(Assembly.GetExecutingAssembly(), null);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void WhenOnlyAssemblyNameIsIncluded_ThenReturnsTrue()
        {
            var assembly = typeof(Uri).Assembly;
            var assemblyName = typeof(Uri).Assembly.GetName();
            assemblyName.Version = null;
            assemblyName.ProcessorArchitecture = ProcessorArchitecture.None;
            assemblyName.CultureInfo = null;
            assemblyName.SetPublicKeyToken(null);

            var result = AssemblyNameMatcher.Matches(assembly, assemblyName);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void WhenAssemblyDetailsAreIncluded_ThenReturnsTrue()
        {
            var assembly = typeof(Uri).Assembly;
            var assemblyName = typeof(Uri).Assembly.GetName();

            var result = AssemblyNameMatcher.Matches(assembly, assemblyName);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void WhenAssemblyDetailsDoNotMatch_ThenReturnsFalse()
        {
            var assembly = typeof(Uri).Assembly;
            var assemblyName = typeof(Uri).Assembly.GetName();
            assemblyName.SetPublicKeyToken(new byte[8]);

            var result = AssemblyNameMatcher.Matches(assembly, assemblyName);

            Assert.IsFalse(result);
        }
    }
}
