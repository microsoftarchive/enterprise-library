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

using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Security;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.PolicyInjection;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Tests.Configuration
{
    [TestClass]
    [DeploymentItem("test.exe.config")]
    public class AuthorizationCallHandlerDataSerializationFixture : CallHandlerDataFixtureBase
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

    }

}
