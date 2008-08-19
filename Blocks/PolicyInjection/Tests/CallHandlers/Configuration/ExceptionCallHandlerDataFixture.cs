//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Policy Injection Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Tests.Configuration
{
    [TestClass]
    public class ExceptionCallHandlerDataFixture : CallHandlerDataFixtureBase
    {
        [TestMethod]
        public void CanSerializeExceptionCallHandler()
        {
            ExceptionCallHandlerData handlerData =
                new ExceptionCallHandlerData("CallHandler", "Swallow Exceptions");

            ExceptionCallHandlerData deserializedHandler =
                SerializeAndDeserializeHandler(handlerData) as ExceptionCallHandlerData;

            Assert.IsNotNull(deserializedHandler);
            Assert.AreEqual(handlerData.Name, deserializedHandler.Name);
            Assert.AreEqual(handlerData.ExceptionPolicyName, deserializedHandler.ExceptionPolicyName);
        }

        [TestMethod]
        public void CanCreateHandlerViaAssemblerWithProperData()
        {
            ExceptionCallHandlerData data =
                new ExceptionCallHandlerData("handler", "Swallow Exceptions");
            data.Order = 5;

            ExceptionCallHandlerAssembler assembler = new ExceptionCallHandlerAssembler();

            ExceptionCallHandler handler = (ExceptionCallHandler)assembler.Assemble(null, data, null, null);

            Assert.AreEqual(data.ExceptionPolicyName, handler.ExceptionPolicyName);
            Assert.AreEqual(data.Order, handler.Order);
        }
    }
}