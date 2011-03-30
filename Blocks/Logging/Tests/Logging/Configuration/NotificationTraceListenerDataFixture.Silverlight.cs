using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.Configuration
{
    [TestClass]
    public class GivenNotificationTraceListenerData
    {
        private TraceListenerData listenerData;

        [TestInitialize]
        public void Setup()
        {
            listenerData = new NotificationTraceListenerData { Name = "listener" } ; //, NotificationTrace = "notificationTrace" };
        }

        [TestMethod]
        public void WhenCreatesRegistrations_ThenCreatesOneTypeRegistration()
        {
            Assert.AreEqual(1, listenerData.GetRegistrations().Count());
        }

        [TestMethod]
        public void WhenCreatesRegistrations_ThenCreatesATypeRegistrationForTheListenerWithTheConfiguredName()
        {
            listenerData.GetRegistrations().Where(tr => tr.Name == "listener").First()
                .AssertForServiceType(typeof(TraceListener))
                .ForName("listener")
                .ForImplementationType(typeof(NotificationTraceListener));
        }

        [TestMethod]
        public void WhenCreatesRegistrations_ThenTraceListenerRegistrationIsSingleton()
        {
            Assert.AreEqual(
                TypeRegistrationLifetime.Singleton,
                listenerData.GetRegistrations().First(tr => tr.Name == "listener").Lifetime);
        }

        [TestMethod]
        public void WhenCreatesRegistrations_ThenTraceListenerRegistrationIsInjectedWithTheNameProperty()
        {
            listenerData.GetRegistrations().First(tr => tr.Name == "listener")
                .AssertProperties()
                .WithValueProperty("Name", "listener")
                .VerifyProperties();
        }

        [TestMethod]
        public void ThenShouldProviderProperConstructorParameters()
        {
            listenerData.GetRegistrations().First(tr => tr.Name == "listener")
                .AssertConstructor()
                .WithContainerResolvedParameter<ITraceDispatcher>(null)
                .VerifyConstructorParameters();
        }
    }
}
