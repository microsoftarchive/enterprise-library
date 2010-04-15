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
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Converters;

namespace Console.Wpf.Tests.VSTS.DevTests.given_system_data_provider_converter
{
    [TestClass]
    public class when_getting_standard_values : ContainerContext
    {
        System.ComponentModel.TypeConverter.StandardValuesCollection providers;
        SystemDataProviderConverter dataProvider;
        protected override void Arrange()
        {
            base.Arrange();

            dataProvider = new SystemDataProviderConverter();
        }

        protected override void Act()
        {
            providers = (System.ComponentModel.TypeConverter.StandardValuesCollection)dataProvider.GetStandardValues();
        }

        [TestMethod]
        public void then_providers_should_be_returned()
        {
            Assert.IsTrue(providers.Count > 0);
        }

        [TestMethod]
        public void then_returned_providers_contains_sql_client()
        {
            Assert.IsTrue(providers.OfType<string>().Contains("System.Data.SqlClient"));
        }
    }
}
