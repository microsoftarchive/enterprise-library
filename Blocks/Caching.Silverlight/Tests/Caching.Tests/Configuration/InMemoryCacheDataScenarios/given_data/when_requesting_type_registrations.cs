using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Scheduling;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.Configuration.InMemoryCacheDataScenarios.given_data
{
    [TestClass]
    public class when_requesting_type_registrations : Context
    {
        private IEnumerable<TypeRegistration> registrations;

        protected override void Act()
        {
            base.Act();

            this.registrations = this.data.GetRegistrations(null);
        }

        [TestMethod]
        public void then_has_two_registrations()
        {
            Assert.AreEqual(2, this.registrations.Count());
        }

        [TestMethod]
        public void then_first_registration_has_name()
        {
            Assert.AreEqual("test name", this.registrations.First().Name);
            Assert.IsTrue(this.registrations.First().IsPublicName);
        }

        [TestMethod]
        public void then_first_registration_is_not_default()
        {
            Assert.IsFalse(this.registrations.First().IsDefault);
        }

        [TestMethod]
        public void then_first_registration_is_for_ObjectCache_service()
        {
            Assert.AreSame(typeof(ObjectCache), this.registrations.First().ServiceType);
        }

        [TestMethod]
        public void then_first_registration_is_for_IsolatedStorageCache_implementation()
        {
            Assert.AreSame(typeof(InMemoryCache), this.registrations.First().ImplementationType);
        }

        [TestMethod]
        public void then_first_registration_has_expected_parameters()
        {
            Assert.AreEqual(5, this.registrations.First().ConstructorParameters.Count());
            Assert.AreEqual("test name", ((ConstantParameterValue)this.registrations.First().ConstructorParameters.ElementAt(0)).Value);
            Assert.AreEqual(500, ((ConstantParameterValue)this.registrations.First().ConstructorParameters.ElementAt(1)).Value);
            Assert.AreEqual(300, ((ConstantParameterValue)this.registrations.First().ConstructorParameters.ElementAt(2)).Value);
            Assert.AreEqual(typeof(ScavengingScheduler), ((ContainerResolvedParameter)this.registrations.First().ConstructorParameters.ElementAt(3)).Type);
            Assert.IsNull(((ContainerResolvedParameter)this.registrations.First().ConstructorParameters.ElementAt(3)).Name);
            Assert.AreEqual(typeof(ExpirationScheduler), ((ContainerResolvedParameter)this.registrations.First().ConstructorParameters.ElementAt(4)).Type);
            Assert.AreEqual(this.registrations.ElementAt(1).Name, ((ContainerResolvedParameter)this.registrations.First().ConstructorParameters.ElementAt(4)).Name);
        }

        [TestMethod]
        public void then_first_registration_has_no_properties()
        {
            Assert.AreEqual(0, this.registrations.First().InjectedProperties.Count());
        }


        [TestMethod]
        public void then_second_registration_has_name()
        {
            Assert.IsTrue(this.registrations.ElementAt(1).Name.StartsWith("test name"));
            Assert.IsFalse(this.registrations.ElementAt(1).IsPublicName);
        }

        [TestMethod]
        public void then_second_registration_is_not_default()
        {
            Assert.IsFalse(this.registrations.ElementAt(1).IsDefault);
        }

        [TestMethod]
        public void then_second_registration_is_for_IRecurringScheduledWork_service()
        {
            Assert.AreSame(typeof(IRecurringScheduledWork), this.registrations.ElementAt(1).ServiceType);
        }

        [TestMethod]
        public void then_second_registration_is_for_ExpirationScheduler_implementation()
        {
            Assert.AreSame(typeof(ExpirationScheduler), this.registrations.ElementAt(1).ImplementationType);
        }

        [TestMethod]
        public void then_second_registration_has_expected_parameters()
        {
            Assert.AreEqual(1, this.registrations.ElementAt(1).ConstructorParameters.Count());
            Assert.AreEqual(TimeSpan.FromSeconds(15), ((ConstantParameterValue)this.registrations.ElementAt(1).ConstructorParameters.ElementAt(0)).Value);
        }

        [TestMethod]
        public void then_second_registration_has_no_properties()
        {
            Assert.AreEqual(0, this.registrations.ElementAt(1).InjectedProperties.Count());
        }
    }
}
