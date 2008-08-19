using System;

using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Configuration;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Tests.Configuration
{
    /// <summary>
    /// Summary description for CachingCallHandlerAssemblerFixture
    /// </summary>
    [TestClass]
    public class CachingCallHandlerAssemblerFixture
    {
        [TestMethod]
        public void CanCreateCallHandlerViaAssemberWithProperData()
        {
            CachingCallHandlerData data = new CachingCallHandlerData()
            {
                Name = "cache me",
                Order = 37,
                ExpirationTime = new TimeSpan(0, 30, 0)
            };

            CachingCallHandlerAssembler assembler = new CachingCallHandlerAssembler();

            CachingCallHandler handler = (CachingCallHandler)assembler.Assemble(null, data, null, null);

            Assert.AreEqual(data.Order, handler.Order);
            Assert.AreEqual(data.ExpirationTime, handler.ExpirationTime);
        }
    }
}
