using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Console.Wpf.Tests.VSTS.DevTests.given_host_adapter
{
    [TestClass]
    public class when_editing_property : given_host_adapter_and_type_descriptor
    {
        Property expirationPollFrequency;
        PropertyDescriptor componentModelExpirationPollFrequency;
        bool expirationPollFrequencyChanged;
        protected override void Arrange()
        {
            base.Arrange();

            expirationPollFrequency = CacheManager.Property("ExpirationPollFrequencyInSeconds");
            componentModelExpirationPollFrequency = CacheManagerTypeDescriptor.GetProperties().OfType<PropertyDescriptor>().Where(x => x.Name == "ExpirationPollFrequencyInSeconds").First();

            expirationPollFrequencyChanged = false;

            componentModelExpirationPollFrequency.AddValueChanged(CacheManagerTypeDescriptor, new EventHandler((sender, args) => expirationPollFrequencyChanged = true));
        }

        protected override void Act()
        {
            expirationPollFrequency.BindableProperty.BindableValue = "123";
        }

        [TestMethod]
        public void then_property_descriptor_raises_changed_event()
        {
            Assert.IsTrue(expirationPollFrequencyChanged);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void then_property_descriptor_throws_exception_on_validation_error()
        {
            componentModelExpirationPollFrequency.SetValue(null, "invalidvalue");
        }
    }
}
