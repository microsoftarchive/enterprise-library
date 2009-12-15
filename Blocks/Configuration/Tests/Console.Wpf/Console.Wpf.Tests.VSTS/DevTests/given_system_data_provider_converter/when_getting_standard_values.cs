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
    public class  when_getting_standard_values : ContainerContext
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
    }
}
