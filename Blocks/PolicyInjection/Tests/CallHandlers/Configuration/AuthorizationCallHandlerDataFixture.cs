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
    public class AuthorizationCallHandlerDataFixture : CallHandlerDataFixtureBase
    {
        [TestMethod]
        public void CanDeserializeAuthorizationCallHandlerData()
        {
            AuthorizationCallHandlerData data = new AuthorizationCallHandlerData("Authorization Handler");
            data.AuthorizationProvider = "auhtorizationProvider";
            data.OperationName = "operationName";
            data.Order = 5;

            AuthorizationCallHandlerData deserialized =
                (AuthorizationCallHandlerData)SerializeAndDeserializeHandler(data);

            Assert.AreEqual(data.AuthorizationProvider, deserialized.AuthorizationProvider);
            Assert.AreEqual(data.OperationName, deserialized.OperationName);
            Assert.AreEqual(typeof(AuthorizationCallHandler), deserialized.Type);
            Assert.AreEqual(data.Order, deserialized.Order);
        }

        [TestMethod]
        public void CanCreateHandlerFromDataWithCorrectProperties()
        {
            AuthorizationCallHandlerData data = new AuthorizationCallHandlerData("Auth handler");
            data.AuthorizationProvider = "authorizationProvider";
            data.OperationName = "op";
            data.Order = 7;

            AuthorizationCallHandlerAssembler assembler = new AuthorizationCallHandlerAssembler();
            AuthorizationCallHandler handler = (AuthorizationCallHandler)assembler.Assemble(null, data, null, null);

            Assert.AreEqual(data.AuthorizationProvider, handler.ProviderName);
            Assert.AreEqual(data.OperationName, handler.OperationName);
            Assert.AreEqual(data.Order, handler.Order);
        }
    }
}