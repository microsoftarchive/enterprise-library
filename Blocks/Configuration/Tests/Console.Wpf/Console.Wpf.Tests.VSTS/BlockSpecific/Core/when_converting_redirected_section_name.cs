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
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Converters;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Console.Wpf.Tests.VSTS.BlockSpecific.Core
{
    [TestClass]
    public class when_converting_redirected_section_name : ArrangeActAssert
    {
        RedirectedSectionNameConverter sectionNameConverter;

        protected override void Arrange()
        {
            sectionNameConverter = new RedirectedSectionNameConverter();
        }

        [TestMethod]
        public void then_unknown_value_is_unconverted()
        {
            Assert.AreEqual("unknown", sectionNameConverter.ConvertFrom("unknown"));
            Assert.AreEqual("unknown", sectionNameConverter.ConvertTo("unknown", typeof(String)));
        }

        [TestMethod]
        public void then_known_section_are_converted_to_display_names()
        {
            Assert.AreNotEqual(DataAccessDesignTime.ConnectionStringSettingsSectionName, sectionNameConverter.ConvertTo(DataAccessDesignTime.ConnectionStringSettingsSectionName, typeof(string)));
            Assert.AreNotEqual(OracleConnectionSettings.SectionName, sectionNameConverter.ConvertTo(OracleConnectionSettings.SectionName, typeof(string)));
            Assert.AreNotEqual(DatabaseSettings.SectionName, sectionNameConverter.ConvertTo(DatabaseSettings.SectionName, typeof(string)));
            Assert.AreNotEqual(ExceptionHandlingSettings.SectionName, sectionNameConverter.ConvertTo(ExceptionHandlingSettings.SectionName, typeof(string)));
            Assert.AreNotEqual(LoggingSettings.SectionName, sectionNameConverter.ConvertTo(LoggingSettings.SectionName, typeof(string)));
            Assert.AreNotEqual(PolicyInjectionSettings.SectionName, sectionNameConverter.ConvertTo(PolicyInjectionSettings.SectionName, typeof(string)));
            Assert.AreNotEqual(ValidationSettings.SectionName, sectionNameConverter.ConvertTo(ValidationSettings.SectionName, typeof(string)));
        }
    }
}
