using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.IsolatedStorage.given_repository_on_existing_locked_file
{
    [TestClass]
    [Tag("IsolatedStorage")]
    public class when : Context
    {
        [TestMethod]
        public void then_is_not_available()
        {
            Assert.IsFalse(this.repository.IsAvailable);
        }
    }
}
