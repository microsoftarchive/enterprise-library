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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.Unity.Configuration.Design.Tests.given_populated_unity_section
{
    [TestClass]
    public class when_cloning_section : given_populated_unity_section
    {
        private UnityConfigurationSection clonedSection;

        protected override void Act()
        {
            base.Act();

            var cloner = new ConfigurationSectionCloner();
            clonedSection = (UnityConfigurationSection)cloner.Clone(Section);
        }

        [TestMethod]
        public void then_container_is_cloned()
        {
            Assert.AreEqual(1, clonedSection.Containers.Count);
            Assert.AreEqual("", clonedSection.Containers.Default.Name);
        }

        [TestMethod]
        public void then_optional_value_is_cloned()
        {
            var ctor = (ConstructorElement) clonedSection.Containers.Default.Registrations[0].InjectionMembers[0];
            var optionalParam = ctor.Parameters.Where(p => p.Name == "optionalParameter").First();

            Assert.IsInstanceOfType(optionalParam.Value, typeof(OptionalElement));
        }
    }
}
