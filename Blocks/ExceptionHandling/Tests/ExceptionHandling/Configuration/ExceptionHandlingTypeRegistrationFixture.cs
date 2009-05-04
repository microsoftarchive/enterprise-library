//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Tests
{
    /// <summary>
    /// Summary description for ExceptionHandlingRegistrationProvider
    /// </summary>
    [TestClass]
    public class GivenConfigurationSettingsWithNoData
    {
        private ExceptionHandlingSettings settings;

        [TestInitialize]
        public void Setup()
        {
            settings = new ExceptionHandlingSettings();
        }


        [TestMethod]
        public void WhenRegistrationsRequested_ResultsInExceptionManagerImplementationRegistration()
        {
            var registrations = settings.CreateRegistrations();
            Assert.AreEqual(1, registrations.Count());

            var registration = registrations.ElementAt(0);

            registration.AssertForServiceType(typeof(ExceptionManager))
                .ForName(null)
                .ForImplementationType(typeof(ExceptionManagerImpl));
        }
    }

    [TestClass]
    public class GivenConfigurationSettingsWithASinglePolicy
    {
        private ExceptionHandlingSettings settings;

        [TestInitialize]
        public void Setup()
        {
            settings = new ExceptionHandlingSettings();
            var exceptionPolicyData = new ExceptionPolicyData("aPolicy");
            var exceptionType = new ExceptionTypeData("ExceptionType", typeof(ArgumentNullException),
                                                      PostHandlingAction.None);
            exceptionType.ExceptionHandlers.Add(
                new WrapHandlerData("aWrapHandler", "exception", typeof(Exception).AssemblyQualifiedName)
                );

            exceptionPolicyData.ExceptionTypes.Add(exceptionType);
            settings.ExceptionPolicies.Add(exceptionPolicyData);
        }

        [TestMethod]
        public void WhenRegistrationsAreRequested_ThenReturnsARegistrationForExceptionPolicyEntry()
        {
            var registrations = settings.CreateRegistrations();

            TypeRegistration registration = registrations.First(r => r.ImplementationType == typeof(ExceptionPolicyImpl));
            registration.AssertForServiceType(typeof(ExceptionPolicyImpl))
                .ForName("aPolicy")
                .ForImplementationType(typeof(ExceptionPolicyImpl));
        }

        [TestMethod]
        public void WhenRegistrationsAreRequested_ThenReturnsExceptionTypeRegistrationEntryWithCorrectName()
        {
            var registrations = settings.CreateRegistrations();

            TypeRegistration registration = registrations.First(r => r.ImplementationType == typeof(ExceptionPolicyEntry));

            registration.AssertForServiceType(typeof(ExceptionPolicyEntry))
                .ForName("aPolicy.ExceptionType")
                .ForImplementationType(typeof(ExceptionPolicyEntry));

        }

        [TestMethod]
        public void WhenRegistrationsAreRequested_ThenReturnsWrapHandlerRegistrationWithCorrectName()
        {
            var registrations = settings.CreateRegistrations();

            TypeRegistration registration = registrations.First(r => r.ImplementationType == typeof(WrapHandler));

            registration.AssertForServiceType(typeof(IExceptionHandler))
                .ForName("aPolicy.ExceptionType.aWrapHandler")
                .ForImplementationType(typeof(WrapHandler));

        }

    }

    [TestClass]
    public class GivenConfigurationSettingsWithMultipleTypesWithSameHandlerNamesOnDifferentTypes
    {
        private ExceptionHandlingSettings settings;

        [TestInitialize]
        public void Setup()
        {
            settings = new ExceptionHandlingSettings();
            var exceptionPolicyData = new ExceptionPolicyData("aPolicy");
            var exceptionType = new ExceptionTypeData("ExceptionType", typeof(ArgumentNullException),
                                                      PostHandlingAction.None);
            exceptionType.ExceptionHandlers.Add(
                new WrapHandlerData("aHandler", "exception", typeof(Exception).AssemblyQualifiedName)
                );

            exceptionPolicyData.ExceptionTypes.Add(exceptionType);
            settings.ExceptionPolicies.Add(exceptionPolicyData);

            var exceptionType2 = new ExceptionTypeData("AnotherExceptionType", typeof(ArgumentNullException),
                                                       PostHandlingAction.None);

            exceptionType2.ExceptionHandlers.Add(
                new ReplaceHandlerData("aHandler", "exception", typeof(Exception).AssemblyQualifiedName)
                );
            exceptionPolicyData.ExceptionTypes.Add(exceptionType2);
        }

        [TestMethod]
        public void WhenRegistrationTypesRequested_ThenWillProduceUniqueNamesForExceptionHandlersAtTheSameLevel()
        {
            IEnumerable<TypeRegistration> registrations = settings.CreateRegistrations();

            registrations.First(r => r.Name == "aPolicy.ExceptionType.aHandler");
            registrations.First(r => r.Name == "aPolicy.AnotherExceptionType.aHandler");
        }
    }

    [TestClass]
    public class GivenConfigurationSettingsWithMultiplePoliciesWithSameTypeNames
    {
        private ExceptionHandlingSettings settings;

        [TestInitialize]
        public void Setup()
        {
            settings = new ExceptionHandlingSettings();

            var exceptionPolicyData = new ExceptionPolicyData("aPolicy");
            var exceptionType = new ExceptionTypeData("ExceptionType", typeof(ArgumentNullException),
                                                      PostHandlingAction.None);
            exceptionPolicyData.ExceptionTypes.Add(exceptionType);
            settings.ExceptionPolicies.Add(exceptionPolicyData);

            var exceptionPolicyData2 = new ExceptionPolicyData("anotherPolicy");
            var exceptionType2 = new ExceptionTypeData("ExceptionType", typeof(ArgumentNullException),
                                                       PostHandlingAction.None);
            exceptionPolicyData2.ExceptionTypes.Add(exceptionType2);
            settings.ExceptionPolicies.Add(exceptionPolicyData2);
        }

        [TestMethod]
        public void WhenRegistrationTypesRequested_ThenWillProduceUniqueNamesForExceptionHandlersAtTheSameLevel()
        {
            IEnumerable<TypeRegistration> registrations = settings.CreateRegistrations();

            registrations.First(r => r.Name == "aPolicy.ExceptionType");
            registrations.First(r => r.Name == "anotherPolicy.ExceptionType");
        }
    }

    [TestClass]
    public class GivenConfigurationSettingsWithMultiplePoliciesWithDuplicateTypeAndHandlerNames
    {
        private ExceptionHandlingSettings settings;
        private IEnumerable<TypeRegistration> registrations;

        [TestInitialize]
        public void Setup()
        {
            settings = new ExceptionHandlingSettings();

            var exceptionPolicyData1 = new ExceptionPolicyData("policy1");
            settings.ExceptionPolicies.Add(exceptionPolicyData1);
            var exceptionType11 = new ExceptionTypeData("ExceptionType1", typeof(ArgumentNullException),
                                                        PostHandlingAction.NotifyRethrow);
            exceptionPolicyData1.ExceptionTypes.Add(exceptionType11);
            exceptionType11.ExceptionHandlers.Add(
                new WrapHandlerData("handler1", "message", typeof(Exception).AssemblyQualifiedName));
            var exceptionType12 = new ExceptionTypeData("ExceptionType2", typeof(ArgumentNullException),
                                                        PostHandlingAction.NotifyRethrow);
            exceptionPolicyData1.ExceptionTypes.Add(exceptionType12);
            exceptionType12.ExceptionHandlers.Add(
                new WrapHandlerData("handler1", "message", typeof(Exception).AssemblyQualifiedName));
            exceptionType12.ExceptionHandlers.Add(
                new WrapHandlerData("handler2", "message", typeof(Exception).AssemblyQualifiedName));


            var exceptionPolicyData2 = new ExceptionPolicyData("policy2");
            settings.ExceptionPolicies.Add(exceptionPolicyData2);
            var exceptionType21 = new ExceptionTypeData("ExceptionType1", typeof(ArgumentNullException),
                                                        PostHandlingAction.NotifyRethrow);
            exceptionPolicyData2.ExceptionTypes.Add(exceptionType21);
            exceptionType21.ExceptionHandlers.Add(
                new WrapHandlerData("handler1", "message", typeof(Exception).AssemblyQualifiedName));
            exceptionType21.ExceptionHandlers.Add(
                new WrapHandlerData("handler3", "message", typeof(Exception).AssemblyQualifiedName));

            registrations = settings.CreateRegistrations();
        }

        [TestMethod]
        public void ThenWillProduceRegistrationForPolicies()
        {
            registrations.First(r => r.Name == "policy1");
            registrations.First(r => r.Name == "policy2");

            Assert.AreEqual(2, registrations.Count(r => r.ImplementationType == typeof(ExceptionPolicyImpl)));
        }

        [TestMethod]
        public void ThenWillProduceRegistrationExceptionEntries()
        {
            registrations.First(r => r.Name == "policy1.ExceptionType1");
            registrations.First(r => r.Name == "policy1.ExceptionType2");
            registrations.First(r => r.Name == "policy2.ExceptionType1");

            Assert.AreEqual(3, registrations.Count(r => r.ImplementationType == typeof(ExceptionPolicyEntry)));
        }

        [TestMethod]
        public void ThenWillProduceRegistrationHandlerEntries()
        {
            registrations.First(r => r.Name == "policy1.ExceptionType1.handler1");
            registrations.First(r => r.Name == "policy1.ExceptionType2.handler1");
            registrations.First(r => r.Name == "policy1.ExceptionType2.handler2");
            registrations.First(r => r.Name == "policy2.ExceptionType1.handler1");
            registrations.First(r => r.Name == "policy2.ExceptionType1.handler3");

            Assert.AreEqual(5, registrations.Count(r => r.ServiceType == typeof(IExceptionHandler)));
        }
    }

    [TestClass]
    public class GivenWrapHandlerConfigurationObjectWithMessage
    {
        private WrapHandlerData handlerData;

        [TestInitialize]
        public void Setup()
        {
            handlerData = new WrapHandlerData("wrap", "exception", typeof(Exception).AssemblyQualifiedName);
        }

        [TestMethod]
        public void WhenAskedForTypeRegistration_ThenReturnsTypeRegistrationConfiguringMessageAndExceptionType()
        {
            TypeRegistration typeRegistration = handlerData.GetContainerConfigurationModel("prefix");

            IStringResolver resolver;
            typeRegistration
                .AssertForServiceType(typeof(IExceptionHandler))
                .ForName("prefix.wrap")
                .ForImplementationType(typeof(WrapHandler));

            typeRegistration.AssertConstructor()
                .WithValueConstructorParameter(out resolver)
                .WithValueConstructorParameter(typeof(Exception))
                .VerifyConstructorParameters();

            Assert.AreEqual("exception", resolver.GetString());
        }
    }

    [TestClass]
    public class GivenWrapHandlerConfigurationObjectWithMessageResourceTypeAndResourceName
    {
        private WrapHandlerData handlerData;

        [TestInitialize]
        public void Setup()
        {
            handlerData = new WrapHandlerData("wrap", "exception", typeof(Exception).AssemblyQualifiedName)
                              {
                                  ExceptionMessageResourceName = "ExceptionMessage",
                                  ExceptionMessageResourceType = typeof(Resources).AssemblyQualifiedName
                              };
        }

        [TestMethod]
        public void WhenAskedForTypeRegistration_ThenReturnsTypeRegistrationConfiguringMessageAndExceptionType()
        {
            TypeRegistration typeRegistration = handlerData.GetContainerConfigurationModel("prefix");

            IStringResolver resolver;
            typeRegistration
                .AssertForServiceType(typeof(IExceptionHandler))
                .ForName("prefix.wrap")
                .ForImplementationType(typeof(WrapHandler));

            typeRegistration.AssertConstructor()
                .WithValueConstructorParameter(out resolver)
                .WithValueConstructorParameter(typeof(Exception))
                .VerifyConstructorParameters();

            Assert.AreEqual(Resources.ExceptionMessage, resolver.GetString());
        }
    }

    [TestClass]
    public class GivenReplaceHandlerConfigurationObject
    {
        private ReplaceHandlerData handlerData;

        [TestInitialize]
        public void Setup()
        {
            handlerData = new ReplaceHandlerData(
                "replace",
                "exception",
                typeof(Exception).AssemblyQualifiedName);
        }

        [TestMethod]
        public void WhenAskedForTypeRegitration_ThenReturnsTypeRegistrationConfiguringReplaceHandler()
        {

            TypeRegistration typeRegistration = handlerData.GetContainerConfigurationModel("prefix");
            IStringResolver resolver;

            typeRegistration
                .AssertForServiceType(typeof(IExceptionHandler))
                .ForName("prefix.replace")
                .ForImplementationType(typeof(ReplaceHandler));

            typeRegistration.AssertConstructor()
                .WithValueConstructorParameter(out resolver)
                .WithValueConstructorParameter(typeof(Exception))
                .VerifyConstructorParameters();

            Assert.AreEqual("exception", resolver.GetString());

        }

    }

    [TestClass]
    public class GivenReplaceHandlerConfigurationObjectWithResourceBasedMessages
    {
        private ReplaceHandlerData handlerData;

        [TestInitialize]
        public void Setup()
        {
            handlerData =
                new ReplaceHandlerData("replace", "exception", typeof(Exception).AssemblyQualifiedName)
                    {
                        ExceptionMessageResourceType = typeof(Resources).AssemblyQualifiedName,
                        ExceptionMessageResourceName = "ExceptionMessage"
                    };
        }

        [TestMethod]
        public void WhenAskedForTypeRegitration_ThenReturnsTypeRegistrationConfiguringReplaceHandler()
        {
            TypeRegistration typeRegistration = handlerData.GetContainerConfigurationModel("prefix");
            IStringResolver resolver;

            typeRegistration
                .AssertForServiceType(typeof(IExceptionHandler))
                .ForName("prefix.replace")
                .ForImplementationType(typeof(ReplaceHandler));

            typeRegistration.AssertConstructor()
                .WithValueConstructorParameter(out resolver)
                .WithValueConstructorParameter(typeof(Exception))
                .VerifyConstructorParameters();

            Assert.AreEqual(Resources.ExceptionMessage, resolver.GetString());
        }
    }

    [TestClass]
    public class GivenCustomHandlerConfigurationObjectForMockExceptionHandler
    {
        private CustomHandlerData handlerData;

        [TestInitialize]
        public void Setup()
        {
            handlerData =
                new CustomHandlerData("custom", typeof(MockExceptionHandler));
            handlerData.Attributes["foo"] = "bar";
        }

        [TestMethod]
        public void WhenAskedForTypeRegistration_ThenReturnsTypeRegistrationConfiguringMockExceptionHandlerWithTheAttributesCollection()
        {
            TypeRegistration typeRegistration = handlerData.GetContainerConfigurationModel("prefix");

            typeRegistration
                .AssertForServiceType(typeof(IExceptionHandler))
                .ForName("prefix.custom")
                .ForImplementationType(typeof(MockExceptionHandler));

            typeRegistration.AssertConstructor()
                .WithValueConstructorParameter(handlerData.Attributes)
                .VerifyConstructorParameters();

        }
    }

    [TestClass]
    public class GivenExceptionPolicyTypeDataWithNoHandlers
    {
        ExceptionTypeData exceptionTypeData;
        TypeRegistration registration;

        [TestInitialize]
        public void Setup()
        {
            exceptionTypeData = new ExceptionTypeData("name", typeof(ArgumentException), PostHandlingAction.ThrowNewException);

            registration = exceptionTypeData.GetContainerConfigurationModel("prefix");
        }

        [TestMethod]
        public void ThenServiceTypeIsNull()
        {
            registration
                .AssertForServiceType(typeof(ExceptionPolicyEntry))
                .ForName("prefix.name")
                .ForImplementationType(typeof(ExceptionPolicyEntry));
        }

        [TestMethod]
        public void ThenProviderAllConstructorParameters()
        {
            registration
                .AssertConstructor()
                .WithValueConstructorParameter(typeof(ArgumentException))
                .WithValueConstructorParameter(PostHandlingAction.ThrowNewException)
                .WithContainerResolvedEnumerableConstructorParameter<IExceptionHandler>(new string[0])
                .VerifyConstructorParameters();
        }
    }

    [TestClass]
    public class GivenExceptionPolicyTypeData
    {
        ExceptionTypeData exceptionTypeData;
        TypeRegistration registration;

        [TestInitialize]
        public void Setup()
        {
            exceptionTypeData = new ExceptionTypeData("name", typeof(ArgumentNullException), PostHandlingAction.None);
            exceptionTypeData.ExceptionHandlers.Add(
                new ReplaceHandlerData("replace", "except", typeof(Exception).AssemblyQualifiedName)
                );

            exceptionTypeData.ExceptionHandlers.Add(
                new WrapHandlerData("wrap", "except", typeof(Exception).AssemblyQualifiedName)
                );

            registration = exceptionTypeData.GetContainerConfigurationModel("prefix");
        }

        [TestMethod]
        public void ThenServiceTypeIsNull()
        {
            registration
                .AssertForServiceType(typeof(ExceptionPolicyEntry))
                .ForName("prefix.name")
                .ForImplementationType(typeof(ExceptionPolicyEntry));
        }

        [TestMethod]
        public void ThenProviderAllConstructorParameters()
        {
            registration
                .AssertConstructor()
                .WithValueConstructorParameter(typeof(ArgumentNullException))
                .WithValueConstructorParameter(PostHandlingAction.None)
                .WithContainerResolvedEnumerableConstructorParameter<IExceptionHandler>(new[] { "prefix.name.replace", "prefix.name.wrap" })
                .VerifyConstructorParameters();
        }
    }

    [TestClass]
    public class GivenExceptionPolicyDataWithNoTypes
    {
        private ExceptionPolicyData exceptionPolicyData;
        private TypeRegistration registration;

        [TestInitialize]
        public void Setup()
        {
            exceptionPolicyData = new ExceptionPolicyData("policy");

            registration = exceptionPolicyData.GetContainerConfigurationModel();
        }

        [TestMethod]
        public void ThenServiceTypeIsNull()
        {
            registration
                .AssertForServiceType(typeof(ExceptionPolicyImpl))
                .ForName("policy")
                .ForImplementationType(typeof(ExceptionPolicyImpl));
        }

        [TestMethod]
        public void ThenProviderAllConstructorParameter()
        {
            registration
                .AssertConstructor()
                .WithValueConstructorParameter("policy")
                .WithContainerResolvedEnumerableConstructorParameter<ExceptionPolicyEntry>(new string[0])
                .VerifyConstructorParameters();
        }
    }

    [TestClass]
    public class GivenExceptionPolicyDataWithExceptionType
    {
        private ExceptionPolicyData exceptionPolicyData;
        private TypeRegistration registration;

        [TestInitialize]
        public void Setup()
        {
            exceptionPolicyData = new ExceptionPolicyData("policy");
            exceptionPolicyData.ExceptionTypes.Add(new ExceptionTypeData("type", typeof(ArgumentException), PostHandlingAction.None));

            registration = exceptionPolicyData.GetContainerConfigurationModel();
        }

        [TestMethod]
        public void ThenModelContainsProperlyPrefixedNameForTheExceptionType()
        {
            registration.AssertConstructor()
                .WithValueConstructorParameter("policy")
                .WithContainerResolvedEnumerableConstructorParameter<ExceptionPolicyEntry>(new[] { "policy.type" })
                .VerifyConstructorParameters();
        }
    }
}
