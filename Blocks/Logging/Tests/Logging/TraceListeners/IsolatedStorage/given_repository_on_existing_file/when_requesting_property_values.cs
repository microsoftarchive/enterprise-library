using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.IsolatedStorage.given_repository_on_existing_file
{
    [TestClass]
    [Tag("IsolatedStorage")]
    public class when_requesting_property_values : Context
    {
        protected override void Act()
        {
            base.Act();
        }

        [TestMethod]
        public void then_max_size_is_read()
        {
            Assert.AreEqual(1, this.repository.MaxSizeInKilobytes);
        }

        [TestMethod]
        public void then_effective_max_size_is_read()
        {
            Assert.AreEqual(1, this.repository.ActualMaxSizeInKilobytes);
        }
    }
}
