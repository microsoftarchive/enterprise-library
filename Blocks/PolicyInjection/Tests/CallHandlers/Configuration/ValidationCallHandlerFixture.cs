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
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Tests.Configuration
{
    [TestClass]
    public class ValidationCallHandlerFixture : CallHandlerDataFixtureBase
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

    [TestClass]
    public class GivenAValidationCallHandlerDataWithAttributesSource
    {
        private CallHandlerData callHandlerData;

        [TestInitialize]
        public void Setup()
        {
            callHandlerData =
                new ValidationCallHandlerData("validation")
                {
                    Order = 300,
                    SpecificationSource = SpecificationSource.Attributes,
                    RuleSet = "ruleset"
                };
        }

        [TestMethod]
        public void WhenCreatesTypeRegistration_ThenCreatesSingleRegistration()
        {
            var registrations = callHandlerData.GetRegistrations("-suffix");

            Assert.AreEqual(1, registrations.Count());
        }


        [TestMethod]
        public void WhenCreatesTypeRegistration_ThenRegistrationHasTransientLifetime()
        {
            var registrations = callHandlerData.GetRegistrations("-suffix").First();

            Assert.AreEqual(TypeRegistrationLifetime.Transient, registrations.Lifetime);
        }

        [TestMethod]
        public void WhenCreatesTypeRegistration_ThenRegistrationIsForICallHandlerWithNameAndImplementationType()
        {
            var registrations = callHandlerData.GetRegistrations("-suffix");

            registrations.ElementAt(0)
                .AssertForServiceType(typeof(ICallHandler))
                .ForName("validation-suffix")
                .ForImplementationType(typeof(ValidationCallHandler));
        }

        [TestMethod]
        public void WhenCreatesRegistrations_ThenCallHandlerRegistrationInjectsConstructorParameters()
        {
            var registrations = callHandlerData.GetRegistrations("-suffix");

            registrations.ElementAt(0)
                .AssertConstructor()
                .WithValueConstructorParameter("ruleset")
                .WithContainerResolvedParameter<AttributeValidatorFactory>(null)
                .WithValueConstructorParameter(300)
                .VerifyConstructorParameters();
        }
    }

    [TestClass]
    public class GivenAValidationCallHandlerDataWithConfigurationSource
    {
        private CallHandlerData callHandlerData;

        [TestInitialize]
        public void Setup()
        {
            callHandlerData =
                new ValidationCallHandlerData("validation")
                {
                    Order = 400,
                    SpecificationSource = SpecificationSource.Configuration,
                    RuleSet = "ruleset"
                };
        }

        [TestMethod]
        public void WhenCreatesTypeRegistration_ThenCreatesSingleRegistration()
        {
            var registrations = callHandlerData.GetRegistrations("-suffix");

            Assert.AreEqual(1, registrations.Count());
        }

        [TestMethod]
        public void WhenCreatesTypeRegistration_ThenRegistrationIsForICallHandlerWithNameAndImplementationType()
        {
            var registrations = callHandlerData.GetRegistrations("-suffix");

            registrations.ElementAt(0)
                .AssertForServiceType(typeof(ICallHandler))
                .ForName("validation-suffix")
                .ForImplementationType(typeof(ValidationCallHandler));
        }

        [TestMethod]
        public void WhenCreatesRegistrations_ThenCallHandlerRegistrationInjectsConstructorParameters()
        {
            var registrations = callHandlerData.GetRegistrations("-suffix");

            registrations.ElementAt(0)
                .AssertConstructor()
                .WithValueConstructorParameter("ruleset")
                .WithContainerResolvedParameter<ConfigurationValidatorFactory>(null)
                .WithValueConstructorParameter(400)
                .VerifyConstructorParameters();
        }
    }

    [TestClass]
    public class GivenAValidationCallHandlerDataWithBothSource
    {
        private CallHandlerData callHandlerData;

        [TestInitialize]
        public void Setup()
        {
            callHandlerData =
                new ValidationCallHandlerData("validation")
                {
                    Order = 400,
                    SpecificationSource = SpecificationSource.Both,
                    RuleSet = "ruleset"
                };
        }

        [TestMethod]
        public void WhenCreatesTypeRegistration_ThenCreatesSingleRegistration()
        {
            var registrations = callHandlerData.GetRegistrations("-suffix");

            Assert.AreEqual(1, registrations.Count());
        }

        [TestMethod]
        public void WhenCreatesTypeRegistration_ThenRegistrationIsForICallHandlerWithNameAndImplementationType()
        {
            var registrations = callHandlerData.GetRegistrations("-suffix");

            registrations.ElementAt(0)
                .AssertForServiceType(typeof(ICallHandler))
                .ForName("validation-suffix")
                .ForImplementationType(typeof(ValidationCallHandler));
        }

        [TestMethod]
        public void WhenCreatesRegistrations_ThenCallHandlerRegistrationInjectsConstructorParameters()
        {
            var registrations = callHandlerData.GetRegistrations("-suffix");

            registrations.ElementAt(0)
                .AssertConstructor()
                .WithValueConstructorParameter("ruleset")
                .WithContainerResolvedParameter<ValidatorFactory>(null)
                .WithValueConstructorParameter(400)
                .VerifyConstructorParameters();
        }
    }

    [TestClass]
    public class GivenAValidationCallHandlerDataWithParameterAttributesOnlySource
    {
        private CallHandlerData callHandlerData;

        [TestInitialize]
        public void Setup()
        {
            callHandlerData =
                new ValidationCallHandlerData("validation")
                {
                    Order = 400,
                    SpecificationSource = SpecificationSource.ParameterAttributesOnly,
                    RuleSet = "ruleset"
                };
        }

        [TestMethod]
        public void WhenCreatesTypeRegistration_ThenCreatesSingleRegistration()
        {
            var registrations = callHandlerData.GetRegistrations("-suffix");

            Assert.AreEqual(1, registrations.Count());
        }

        [TestMethod]
        public void WhenCreatesTypeRegistration_ThenRegistrationIsForICallHandlerWithNameAndImplementationType()
        {
            var registrations = callHandlerData.GetRegistrations("-suffix");

            registrations.ElementAt(0)
                .AssertForServiceType(typeof(ICallHandler))
                .ForName("validation-suffix")
                .ForImplementationType(typeof(ValidationCallHandler));
        }

        [TestMethod]
        public void WhenCreatesRegistrations_ThenCallHandlerRegistrationInjectsConstructorParameters()
        {
            var registrations = callHandlerData.GetRegistrations("-suffix");

            registrations.ElementAt(0)
                .AssertConstructor()
                .WithValueConstructorParameter("ruleset")
                .WithValueConstructorParameter<ValidatorFactory>(null)
                .WithValueConstructorParameter(400)
                .VerifyConstructorParameters();
        }
    }

    class ValidationMock
    {
        [ValidationCallHandler(Order = 16)]
        public string ReturnSomething()
        {
            return string.Empty;
        }
    }
}
