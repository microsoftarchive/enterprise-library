using System;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.IsolatedStorage.given_repository_on_existing_locked_file
{
    [TestClass]
    [Tag("IsolatedStorage")]
    public class when_adding_an_entry : Context
    {
        private Exception exception;

        protected override void Act()
        {
            try
            {
                this.repository.Add(new LogEntry { });
            }
            catch (InvalidOperationException e)
            {
                this.exception = e;
            }
        }

        [TestMethod]
        public void then_exception_is_thrown()
        {
            Assert.IsNotNull(this.exception);
        }
    }
}
