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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Instrumentation;
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
            var registrations = settings.GetRegistrations(null).Where(r => r.ServiceType == typeof(ExceptionManager));

            Assert.AreEqual(1, registrations.Count());

            var registration = registrations.ElementAt(0);

            registration.AssertForServiceType(typeof(ExceptionManager))
                .IsDefault()
                .ForImplementationType(typeof(ExceptionManagerImpl))
                .IsPublicName();
        }

#if !SILVERLIGHT
        [TestMethod]
        public void WhenRegistrationsRequested_ThenTheDefaultExceptionHandlingEventLoggerIsRegistered()
        {
            var registrations =
                settings.GetRegistrations(null).Where(r => r.ServiceType == typeof(DefaultExceptionHandlingEventLogger));

            Assert.AreEqual(1, registrations.Count());

            var registration = registrations.ElementAt(0);
            registration.AssertForServiceType(typeof(DefaultExceptionHandlingEventLogger))
                .IsDefault()
                .ForImplementationType(typeof(DefaultExceptionHandlingEventLogger))
                .IsNotPublicName();
        }
#endif
    }

    [TestClass]
    public class GivenConfigurationSettingsWithASinglePolicy
    {
        private ExceptionHandlingSettings settings;

        [TestInitialize]
        public void Setup()
        {
            settings = new ExceptionHandlingSettings();
            var exceptionPolicyData =
                new ExceptionPolicyData
                {
                    Name = "aPolicy",
                    ExceptionTypes =
                    {
                        new ExceptionTypeData
                        {
                            Name = "ExceptionType", 
                            Type = typeof(ArgumentNullException),
                            PostHandlingAction = PostHandlingAction.None,
                            ExceptionHandlers =
                            {
                                new WrapHandlerData
                                {
                                    Name = "aWrapHandler",
                                    ExceptionMessage = "exception", 
                                    WrapExceptionType = typeof(Exception)
                                }
                            }
                        }
                    }
                };

            settings.ExceptionPolicies.Add(exceptionPolicyData);
        }

        [TestMethod]
        public void WhenRegistrationsAreRequested_ThenReturnsARegistrationForExceptionPolicyEntry()
        {
            var registrations = settings.GetRegistrations(null);

            TypeRegistration registration = registrations.First(r => r.ImplementationType == typeof(ExceptionPolicyImpl));
            registration.AssertForServiceType(typeof(ExceptionPolicyImpl))
                .ForName("aPolicy")
                .ForImplementationType(typeof(ExceptionPolicyImpl))
                .IsNotPublicName();
        }

        [TestMethod]
        public void WhenRegistrationsAreRequested_ThenExcpetionPolicyImplHasTransientLifetime()
        {
            var registrations = settings.GetRegistrations(null);
            TypeRegistration registration = registrations.First(r => r.ImplementationType == typeof(ExceptionPolicyImpl));

            Assert.AreEqual(TypeRegistrationLifetime.Transient, registration.Lifetime);
        }

#if !SILVERLIGHT
        [TestMethod]
        public void WhenRegistrationsAreRequested_ThenExcpetionInstrumentationProviderHasTransientLifetime()
        {
            var registrations = settings.GetRegistrations(null);
            TypeRegistration registration = registrations.First(r => r.ImplementationType == typeof(ExceptionHandlingInstrumentationProvider));

            Assert.AreEqual(TypeRegistrationLifetime.Transient, registration.Lifetime);
        }
#else
        [TestMethod]
        public void WhenRegistrationsAreRequested_ThenExcpetionInstrumentationProviderHasTransientLifetime()
        {
            var registrations = settings.GetRegistrations(null);
            TypeRegistration registration = registrations.First(r => r.ImplementationType == typeof(NullExceptionHandlingInstrumentationProvider));

            Assert.AreEqual(TypeRegistrationLifetime.Transient, registration.Lifetime);
        }
#endif

        [TestMethod]
        public void WhenRegistrationsAreRequested_ThenReturnsExceptionTypeRegistrationEntryWithCorrectName()
        {
            var registrations = settings.GetRegistrations(null);

            TypeRegistration registration = registrations.First(r => r.ImplementationType == typeof(ExceptionPolicyEntry));

            registration.AssertForServiceType(typeof(ExceptionPolicyEntry))
                .ForName("aPolicy.ExceptionType")
                .ForImplementationType(typeof(ExceptionPolicyEntry))
                .IsNotPublicName();
        }

        [TestMethod]
        public void WhenRegistrationsAreRequested_ThenReturnsExceptionTypeRegistrationEntryWithTransientLifetime()
        {
            var registrations = settings.GetRegistrations(null);

            TypeRegistration registration = registrations.First(r => r.ImplementationType == typeof(ExceptionPolicyEntry));
            Assert.AreEqual(TypeRegistrationLifetime.Transient, registration.Lifetime);
        }

        [TestMethod]
        public void WhenRegistrationsAreRequested_ThenReturnsWrapHandlerRegistrationWithCorrectName()
        {
            var registrations = settings.GetRegistrations(null);

            TypeRegistration registration = registrations.First(r => r.ImplementationType == typeof(WrapHandler));

            registration.AssertForServiceType(typeof(IExceptionHandler))
                .ForName("aPolicy.ExceptionType.aWrapHandler")
                .ForImplementationType(typeof(WrapHandler))
                .IsNotPublicName();
        }

        [TestMethod]
        public void WhenRegistrationsAreRequested_ThenReturnsWrapHandlerWithTransientLifetime()
        {
            var registrations = settings.GetRegistrations(null);

            TypeRegistration registration = registrations.First(r => r.ImplementationType == typeof(WrapHandler));
            Assert.AreEqual(TypeRegistrationLifetime.Transient, registration.Lifetime);
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
            var exceptionPolicyData = new ExceptionPolicyData { Name = "aPolicy" };
            var exceptionType =
                new ExceptionTypeData
                {
                    Name = "ExceptionType",
                    Type = typeof(ArgumentNullException),
                    PostHandlingAction = PostHandlingAction.None
                };
            exceptionType.ExceptionHandlers.Add(
                new WrapHandlerData
                {
                    Name = "aHandler",
                    ExceptionMessage = "exception",
                    WrapExceptionType = typeof(Exception)
                });

            exceptionPolicyData.ExceptionTypes.Add(exceptionType);
            settings.ExceptionPolicies.Add(exceptionPolicyData);

            var exceptionType2 =
                new ExceptionTypeData
                {
                    Name = "AnotherExceptionType",
                    Type = typeof(ArgumentNullException),
                    PostHandlingAction = PostHandlingAction.None
                };

            exceptionType2.ExceptionHandlers.Add(
                new ReplaceHandlerData
                {
                    Name = "aHandler",
                    ExceptionMessage = "exception",
                    ReplaceExceptionType = typeof(Exception)
                }
                );
            exceptionPolicyData.ExceptionTypes.Add(exceptionType2);
        }

        [TestMethod]
        public void WhenRegistrationTypesRequested_ThenWillProduceUniqueNamesForExceptionHandlersAtTheSameLevel()
        {
            IEnumerable<TypeRegistration> registrations = settings.GetRegistrations(null);

            registrations.First(r => r.Name == "aPolicy.ExceptionType.aHandler");
            registrations.First(r => r.Name == "aPolicy.AnotherExceptionType.aHandler");
        }

        [TestMethod]
        public void WhenRegistrationTypesRequested_ThenReturnsReplaceHandlerWithTransientLifetime()
        {
            IEnumerable<TypeRegistration> registrations = settings.GetRegistrations(null);
            TypeRegistration registration = registrations.First(r => r.ImplementationType == typeof(ReplaceHandler));

            Assert.AreEqual(TypeRegistrationLifetime.Transient, registration.Lifetime);
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

            var exceptionPolicyData = new ExceptionPolicyData { Name = "aPolicy" };
            var exceptionType =
                new ExceptionTypeData
                {
                    Name = "ExceptionType",
                    Type = typeof(ArgumentNullException),
                    PostHandlingAction = PostHandlingAction.None
                };
            exceptionPolicyData.ExceptionTypes.Add(exceptionType);
            settings.ExceptionPolicies.Add(exceptionPolicyData);

            var exceptionPolicyData2 = new ExceptionPolicyData { Name = "anotherPolicy" };
            var exceptionType2 =
                new ExceptionTypeData
                {
                    Name = "ExceptionType",
                    Type = typeof(ArgumentNullException),
                    PostHandlingAction = PostHandlingAction.None
                };
            exceptionPolicyData2.ExceptionTypes.Add(exceptionType2);
            settings.ExceptionPolicies.Add(exceptionPolicyData2);
        }

        [TestMethod]
        public void WhenRegistrationTypesRequested_ThenWillProduceUniqueNamesForExceptionHandlersAtTheSameLevel()
        {
            IEnumerable<TypeRegistration> registrations = settings.GetRegistrations(null);

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

            var exceptionPolicyData1 = new ExceptionPolicyData { Name = "policy1" };
            settings.ExceptionPolicies.Add(exceptionPolicyData1);
            var exceptionType11 =
                new ExceptionTypeData
                {
                    Name = "ExceptionType1",
                    Type = typeof(ArgumentNullException),
                    PostHandlingAction = PostHandlingAction.NotifyRethrow
                };
            exceptionPolicyData1.ExceptionTypes.Add(exceptionType11);
            exceptionType11.ExceptionHandlers.Add(
                new WrapHandlerData
                {
                    Name = "handler1",
                    ExceptionMessage = "message",
                    WrapExceptionType = typeof(Exception)
                });
            var exceptionType12 =
                new ExceptionTypeData
                {
                    Name = "ExceptionType2",
                    Type = typeof(ArgumentNullException),
                    PostHandlingAction = PostHandlingAction.NotifyRethrow
                };
            exceptionPolicyData1.ExceptionTypes.Add(exceptionType12);
            exceptionType12.ExceptionHandlers.Add(
                new WrapHandlerData
                {
                    Name = "handler1",
                    ExceptionMessage = "message",
                    WrapExceptionType = typeof(Exception)
                });
            exceptionType12.ExceptionHandlers.Add(
                new WrapHandlerData
                {
                    Name = "handler2",
                    ExceptionMessage = "message",
                    WrapExceptionType = typeof(Exception)
                });


            var exceptionPolicyData2 = new ExceptionPolicyData { Name = "policy2" };
            settings.ExceptionPolicies.Add(exceptionPolicyData2);
            var exceptionType21 =
                new ExceptionTypeData
                {
                    Name = "ExceptionType1",
                    Type = typeof(ArgumentNullException),
                    PostHandlingAction = PostHandlingAction.NotifyRethrow
                };
            exceptionPolicyData2.ExceptionTypes.Add(exceptionType21);
            exceptionType21.ExceptionHandlers.Add(
                new WrapHandlerData
                {
                    Name = "handler1",
                    ExceptionMessage = "message",
                    WrapExceptionType = typeof(Exception)
                });
            exceptionType21.ExceptionHandlers.Add(
                new WrapHandlerData
                {
                    Name = "handler3",
                    ExceptionMessage = "message",
                    WrapExceptionType = typeof(Exception)
                });

            registrations = settings.GetRegistrations(null);
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
            handlerData =
                new WrapHandlerData
                {
                    Name = "wrap",
                    ExceptionMessage = "exception",
                    WrapExceptionType = typeof(Exception)
                };
        }

        [TestMethod]
        public void WhenAskedForTypeRegistration_ThenReturnsTypeRegistrationConfiguringMessageAndExceptionType()
        {
            TypeRegistration typeRegistration = handlerData.GetRegistrations("prefix").First();

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
            handlerData =
                new WrapHandlerData
            {
                Name = "wrap",
                ExceptionMessage = "exception",
                WrapExceptionType = typeof(Exception),
                ExceptionMessageResourceName = "ExceptionMessage",
                ExceptionMessageResourceType = typeof(Resources).AssemblyQualifiedName
            };
        }

        [TestMethod]
        public void WhenAskedForTypeRegistration_ThenReturnsTypeRegistrationConfiguringMessageAndExceptionType()
        {
            TypeRegistration typeRegistration = handlerData.GetRegistrations("prefix").First();

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
            handlerData =
                new ReplaceHandlerData
                {
                    Name = "replace",
                    ExceptionMessage = "exception",
                    ReplaceExceptionType = typeof(Exception)
                };
        }

        [TestMethod]
        public void WhenAskedForTypeRegitration_ThenReturnsTypeRegistrationConfiguringReplaceHandler()
        {

            TypeRegistration typeRegistration = handlerData.GetRegistrations("prefix").First();
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
                new ReplaceHandlerData
                {
                    Name = "replace",
                    ExceptionMessage = "exception",
                    ReplaceExceptionType = typeof(Exception),
                    ExceptionMessageResourceType = typeof(Resources).AssemblyQualifiedName,
                    ExceptionMessageResourceName = "ExceptionMessage"
                };
        }

        [TestMethod]
        public void WhenAskedForTypeRegitration_ThenReturnsTypeRegistrationConfiguringReplaceHandler()
        {
            TypeRegistration typeRegistration = handlerData.GetRegistrations("prefix").First();
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

#if !SILVERLIGHT
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
            TypeRegistration typeRegistration = handlerData.GetRegistrations("prefix").First();

            typeRegistration
                .AssertForServiceType(typeof(IExceptionHandler))
                .ForName("prefix.custom")
                .ForImplementationType(typeof(MockExceptionHandler));

            typeRegistration.AssertConstructor()
                .WithValueConstructorParameter(handlerData.Attributes)
                .VerifyConstructorParameters();

        }


        [TestMethod]
        public void WhenRegistrationTypesRequested_ThenReturnsCustomHandlerWithTransientLifetime()
        {
            TypeRegistration registration = handlerData.GetRegistrations("prefix").First();

            Assert.AreEqual(TypeRegistrationLifetime.Transient, registration.Lifetime);
        }
    }
#endif

    [TestClass]
    public class GivenExceptionPolicyTypeDataWithNoHandlers
    {
        ExceptionTypeData exceptionTypeData;
        TypeRegistration registration;

        [TestInitialize]
        public void Setup()
        {
            exceptionTypeData =
                new ExceptionTypeData
                {
                    Name = "name",
                    Type = typeof(ArgumentException),
                    PostHandlingAction = PostHandlingAction.ThrowNewException
                };

            registration = exceptionTypeData.GetRegistration("prefix");
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
                .WithContainerResolvedParameter<IExceptionHandlingInstrumentationProvider>("prefix")
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
            exceptionTypeData =
                new ExceptionTypeData
                {
                    Name = "name",
                    Type = typeof(ArgumentNullException),
                    PostHandlingAction = PostHandlingAction.None
                };
            exceptionTypeData.ExceptionHandlers.Add(
                new ReplaceHandlerData
                {
                    Name = "replace",
                    ExceptionMessage = "except",
                    ReplaceExceptionType = typeof(Exception)
                });

            exceptionTypeData.ExceptionHandlers.Add(
                new WrapHandlerData
                {
                    Name = "wrap",
                    ExceptionMessage = "except",
                    WrapExceptionType = typeof(Exception)
                });

            registration = exceptionTypeData.GetRegistration("prefix");
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
                .WithContainerResolvedParameter<IExceptionHandlingInstrumentationProvider>("prefix")
                .VerifyConstructorParameters();
        }
    }

    [TestClass]
    public class GivenExceptionPolicyDataWithNoTypes
    {
        private ExceptionPolicyData exceptionPolicyData;
        private List<TypeRegistration> registrations;
        private TypeRegistration registration;
        [TestInitialize]
        public void Setup()
        {
            exceptionPolicyData = new ExceptionPolicyData { Name = "policy" };

            registrations = exceptionPolicyData.GetRegistration(new DictionaryConfigurationSource()).ToList();

            registration = registrations.Where(r => r.ServiceType == typeof(ExceptionPolicyImpl)).ElementAt(0);
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
            exceptionPolicyData = new ExceptionPolicyData { Name = "policy" };
            exceptionPolicyData.ExceptionTypes.Add(
                new ExceptionTypeData
                {
                    Name = "type",
                    Type = typeof(ArgumentException),
                    PostHandlingAction = PostHandlingAction.None
                });

            registration = exceptionPolicyData.GetRegistration(new DictionaryConfigurationSource())
                .Where(r => r.ServiceType == typeof(ExceptionPolicyImpl))
                .ElementAt(0);
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
