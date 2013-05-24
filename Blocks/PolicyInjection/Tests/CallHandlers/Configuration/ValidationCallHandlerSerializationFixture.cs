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

using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.PolicyInjection;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ValidationCallHandler = Microsoft.Practices.EnterpriseLibrary.Validation.PolicyInjection.ValidationCallHandler;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Tests.Configuration
{
    [TestClass]
    public class ValidationCallHandlerSerializationFixture : CallHandlerDataFixtureBase
    {
        [TestMethod]
        public void CanDeserializeValidationCallHandlerData()
        {
            ValidationCallHandlerData data = new ValidationCallHandlerData("Logging Handler");
            data.RuleSet = "MyRuleSet";
            data.SpecificationSource = SpecificationSource.Configuration;
            data.Order = 7;

            ValidationCallHandlerData deserialized =
                (ValidationCallHandlerData)SerializeAndDeserializeHandler(data);

            Assert.AreEqual(data.RuleSet, deserialized.RuleSet);
            Assert.AreEqual(data.SpecificationSource, deserialized.SpecificationSource);
            Assert.AreEqual(typeof(ValidationCallHandler), deserialized.Type);
            Assert.AreEqual(data.Order, deserialized.Order);
        }

        private static MethodImplementationInfo GetMethodImpl(MethodBase method)
        {
            return new MethodImplementationInfo(null, ((MethodInfo)method));
        }
    }
}
