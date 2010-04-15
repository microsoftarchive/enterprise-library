//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Validation Application Block
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
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.PolicyInjection;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests.Configuration
{
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
}
