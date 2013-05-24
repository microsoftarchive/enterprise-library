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
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Tests.ExceptionHandlingObjectCreationFixture
{
    [TestClass]
    public class given_configuration_settings_with_no_data
    {
        private ExceptionHandlingSettings settings;

        [TestInitialize]
        public void Setup()
        {
            settings = new ExceptionHandlingSettings();
        }

        [TestMethod]
        public void when_building_manager_then_builds_empty_manager()
        {
            var manager = (ExceptionManager)this.settings.BuildExceptionManager();

            Assert.AreEqual(0, manager.Policies.Count());
        }
    }

    [TestClass]
    public class given_configuration_settings_with_a_single_policy
    {
        private ExceptionHandlingSettings settings;

        [TestInitialize]
        public void Setup()
        {
            settings = new ExceptionHandlingSettings();
            var exceptionPolicyData = new ExceptionPolicyData("aPolicy");
            var exceptionType = new ExceptionTypeData("ExceptionType", typeof(ArgumentNullException),
                                                      PostHandlingAction.ThrowNewException);
            exceptionType.ExceptionHandlers.Add(
                new WrapHandlerData("aWrapHandler", "exception", typeof(Exception).AssemblyQualifiedName)
                );

            exceptionPolicyData.ExceptionTypes.Add(exceptionType);
            settings.ExceptionPolicies.Add(exceptionPolicyData);
        }

        [TestMethod]
        public void when_building_manager_then_builds_manager_with_single_policy()
        {
            var manager = (ExceptionManager)this.settings.BuildExceptionManager();

            var policy = manager.Policies.Single();

            Assert.AreEqual("aPolicy", policy.PolicyName);
        }

        [TestMethod]
        public void when_using_built_manager_then_wraps_exception()
        {
            var manager = this.settings.BuildExceptionManager();

            var exception = new ArgumentNullException();
            Exception newException;

            var rethrow = manager.HandleException(exception, "aPolicy", out newException);

            Assert.IsTrue(rethrow);
            Assert.IsNotNull(newException);
            Assert.IsInstanceOfType(newException, typeof(Exception));
            Assert.AreEqual("exception", newException.Message);
        }
    }

    [TestClass]
    public class given_configuration_settings_with_multiple_types_with_same_handler_names
    {
        private ExceptionHandlingSettings settings;

        [TestInitialize]
        public void Setup()
        {
            settings = new ExceptionHandlingSettings();
            var exceptionPolicyData = new ExceptionPolicyData("aPolicy");
            var exceptionType = new ExceptionTypeData("ArgumentNullException", typeof(ArgumentNullException),
                                                      PostHandlingAction.None);
            exceptionType.ExceptionHandlers.Add(
                new WrapHandlerData("aHandler", "exception", typeof(Exception).AssemblyQualifiedName)
                );

            exceptionPolicyData.ExceptionTypes.Add(exceptionType);
            settings.ExceptionPolicies.Add(exceptionPolicyData);

            var exceptionType2 = new ExceptionTypeData("ArgumentException", typeof(ArgumentException),
                                                       PostHandlingAction.None);

            exceptionType2.ExceptionHandlers.Add(
                new ReplaceHandlerData("aHandler", "exception", typeof(Exception).AssemblyQualifiedName)
                );
            exceptionPolicyData.ExceptionTypes.Add(exceptionType2);
        }

        [TestMethod]
        public void when_building_manager_then_builds_manager_with_single_policy()
        {
            var manager = (ExceptionManager)this.settings.BuildExceptionManager();

            var policy = manager.Policies.Single();

            Assert.AreEqual("aPolicy", policy.PolicyName);
        }

        [TestMethod]
        public void when_building_policy_with_existing_name_then_gets_policy()
        {
            var policy = this.settings.BuildExceptionPolicy("aPolicy");

            Assert.IsNotNull(policy);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void when_building_policy_with_non_existing_name_then_throws()
        {
            this.settings.BuildExceptionPolicy("missing policy");
        }
    }

    [TestClass]
    public class given_configuration_settings_with_multiple_policies_with_same_type_names
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
        public void when_building_manager_then_builds_manager_with_multiple_policies()
        {
            var manager = (ExceptionManager)this.settings.BuildExceptionManager();

            var policies = manager.Policies.ToArray();

            Assert.AreEqual(2, policies.Length);
            Assert.IsTrue(policies.Any(p => p.PolicyName == "aPolicy"));
            Assert.IsTrue(policies.Any(p => p.PolicyName == "anotherPolicy"));
        }
    }

    [TestClass]
    public class given_configuration_settings_with_multiple_policies_with_duplicate_type_and_handler_names
    {
        private ExceptionHandlingSettings settings;
        private ExceptionManager manager;

        [TestInitialize]
        public void Setup()
        {
            settings = new ExceptionHandlingSettings();

            var exceptionPolicyData1 = new ExceptionPolicyData("policy1");
            settings.ExceptionPolicies.Add(exceptionPolicyData1);
            var exceptionType11 = new ExceptionTypeData("ArgumentNullException", typeof(ArgumentNullException),
                                                        PostHandlingAction.NotifyRethrow);
            exceptionPolicyData1.ExceptionTypes.Add(exceptionType11);
            exceptionType11.ExceptionHandlers.Add(
                new WrapHandlerData("handler1", "message", typeof(Exception).AssemblyQualifiedName));
            var exceptionType12 = new ExceptionTypeData("ArgumentException", typeof(ArgumentException),
                                                        PostHandlingAction.NotifyRethrow);
            exceptionPolicyData1.ExceptionTypes.Add(exceptionType12);
            exceptionType12.ExceptionHandlers.Add(
                new WrapHandlerData("handler1", "message", typeof(Exception).AssemblyQualifiedName));
            exceptionType12.ExceptionHandlers.Add(
                new WrapHandlerData("handler2", "message", typeof(Exception).AssemblyQualifiedName));


            var exceptionPolicyData2 = new ExceptionPolicyData("policy2");
            settings.ExceptionPolicies.Add(exceptionPolicyData2);
            var exceptionType21 = new ExceptionTypeData("ArgumentNullException", typeof(ArgumentNullException),
                                                        PostHandlingAction.NotifyRethrow);
            exceptionPolicyData2.ExceptionTypes.Add(exceptionType21);
            exceptionType21.ExceptionHandlers.Add(
                new WrapHandlerData("handler1", "message", typeof(Exception).AssemblyQualifiedName));
            exceptionType21.ExceptionHandlers.Add(
                new WrapHandlerData("handler3", "message", typeof(Exception).AssemblyQualifiedName));

            this.manager = (ExceptionManager)this.settings.BuildExceptionManager();
        }

        [TestMethod]
        public void when_building_manager_then_builds_manager_with_multiple_policies()
        {
            var manager = (ExceptionManager)this.settings.BuildExceptionManager();

            var policies = manager.Policies.ToArray();

            Assert.AreEqual(2, policies.Length);
            Assert.IsTrue(policies.Any(p => p.PolicyName == "policy1"));
            Assert.IsTrue(policies.Any(p => p.PolicyName == "policy2"));
        }
    }

    [TestClass]
    public class given_wrap_handler_configuration_object_with_message
    {
        private WrapHandlerData handlerData;

        [TestInitialize]
        public void Setup()
        {
            handlerData = new WrapHandlerData("wrap", "exception", typeof(Exception).AssemblyQualifiedName);
        }

        [TestMethod]
        public void when_creating_handler_then_handler_has_literal_message_string()
        {
            var handler = this.handlerData.BuildExceptionHandler();

            var exception = new ArgumentException();

            var newException = handler.HandleException(exception, Guid.NewGuid());
            Assert.IsInstanceOfType(newException, typeof(Exception));
            Assert.AreEqual("exception", newException.Message);
        }
    }

    [TestClass]
    public class given_wrap_handler_configuration_object_with_message_resource_type
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
        public void when_creating_handler_then_handler_has_resource_message_string()
        {
            var handler = this.handlerData.BuildExceptionHandler();

            var exception = new ArgumentException();

            var newException = handler.HandleException(exception, Guid.NewGuid());
            Assert.IsInstanceOfType(newException, typeof(Exception));
            Assert.AreEqual(Resources.ExceptionMessage, newException.Message);
        }
    }

    [TestClass]
    public class given_replace_handler_configuration_object_with_message
    {
        private ReplaceHandlerData handlerData;

        [TestInitialize]
        public void Setup()
        {
            handlerData = new ReplaceHandlerData("replace", "exception", typeof(Exception).AssemblyQualifiedName);
        }

        [TestMethod]
        public void when_creating_handler_then_handler_has_literal_message_string()
        {
            var handler = this.handlerData.BuildExceptionHandler();

            var exception = new ArgumentException();

            var newException = handler.HandleException(exception, Guid.NewGuid());
            Assert.IsInstanceOfType(newException, typeof(Exception));
            Assert.AreEqual("exception", newException.Message);
        }
    }

    [TestClass]
    public class given_replace_handler_configuration_object_with_message_resource_type
    {
        private ReplaceHandlerData handlerData;

        [TestInitialize]
        public void Setup()
        {
            handlerData = new ReplaceHandlerData("replace", "exception", typeof(Exception).AssemblyQualifiedName)
            {
                ExceptionMessageResourceName = "ExceptionMessage",
                ExceptionMessageResourceType = typeof(Resources).AssemblyQualifiedName
            };
        }

        [TestMethod]
        public void when_creating_handler_then_handler_has_resource_message_string()
        {
            var handler = this.handlerData.BuildExceptionHandler();

            var exception = new ArgumentException();

            var newException = handler.HandleException(exception, Guid.NewGuid());
            Assert.IsInstanceOfType(newException, typeof(Exception));
            Assert.AreEqual(Resources.ExceptionMessage, newException.Message);
        }
    }

    [TestClass]
    public class given_custom_handler_configuration_object_for_mock_handler
    {
        private CustomHandlerData handlerData;

        [TestInitialize]
        public void Setup()
        {
            MockExceptionHandler.Clear();

            handlerData =
                new CustomHandlerData("custom", typeof(MockExceptionHandler));
            handlerData.Attributes["foo"] = "bar";
        }

        [TestMethod]
        public void when_building_handler_then_provides_constructor_parameters()
        {
            var handler = this.handlerData.BuildExceptionHandler();

            Assert.IsInstanceOfType(handler, typeof(MockExceptionHandler));
            Assert.AreEqual(1, MockExceptionHandler.attributes.Count);
            Assert.AreEqual("bar", MockExceptionHandler.attributes["foo"]);
        }
    }

    [TestClass]
    public class given_custom_handler_configuration_object_for_non_handler_type
    {
        private CustomHandlerData handlerData;

        [TestInitialize]
        public void Setup()
        {
            MockExceptionHandler.Clear();

            handlerData =
                new CustomHandlerData("custom", typeof(string));
            handlerData.Attributes["foo"] = "bar";
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void when_building_handler_then_throws()
        {
            this.handlerData.BuildExceptionHandler();
        }
    }

    [TestClass]
    public class given_custom_handler_configuration_object_for_missing_handler_type
    {
        private CustomHandlerData handlerData;

        [TestInitialize]
        public void Setup()
        {
            MockExceptionHandler.Clear();

            handlerData = new CustomHandlerData();
            handlerData.Attributes["foo"] = "bar";
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void when_building_handler_then_throws()
        {
            this.handlerData.BuildExceptionHandler();
        }
    }

    [TestClass]
    public class given_custom_handler_configuration_object_for_invalid_handler_type
    {
        private CustomHandlerData handlerData;

        [TestInitialize]
        public void Setup()
        {
            MockExceptionHandler.Clear();

            handlerData = new CustomHandlerData("custom", "an invalid type name");
            handlerData.Attributes["foo"] = "bar";
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void when_building_handler_then_throws()
        {
            this.handlerData.BuildExceptionHandler();
        }
    }

    [TestClass]
    public class given_custom_handler_configuration_object_for_handler_type_with_missing_constructor
    {
        private CustomHandlerData handlerData;

        [TestInitialize]
        public void Setup()
        {
            MockExceptionHandler.Clear();

            handlerData = new CustomHandlerData("custom", typeof(TestHandlerWithMissingConstructor));
            handlerData.Attributes["foo"] = "bar";
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void when_building_handler_then_throws()
        {
            this.handlerData.BuildExceptionHandler();
        }

        public class TestHandlerWithMissingConstructor : IExceptionHandler
        {
            public Exception HandleException(Exception exception, Guid handlingInstanceId)
            {
                throw new NotImplementedException();
            }
        }
    }
}
