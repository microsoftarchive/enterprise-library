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
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests.ExceptionPolicyFactoryFixture
{
    [TestClass]
    public class when_creating_factory_with_null_configuration_access : ArrangeActAssert
    {
        private ArgumentNullException exception;

        protected override void Act()
        {
            try
            {
                new ExceptionPolicyFactory((Func<string, ConfigurationSection>)null);
                Assert.Fail("should have thrown");
            }
            catch (ArgumentNullException e)
            {
                this.exception = e;
            }
        }

        [TestMethod]
        public void then_throws()
        {
            Assert.IsNotNull(this.exception);
            Assert.AreEqual("configurationAccessor", this.exception.ParamName);
        }
    }

    [TestClass]
    public class when_creating_factory_with_null_configuration_source : ArrangeActAssert
    {
        private ArgumentNullException exception;

        protected override void Act()
        {
            try
            {
                new ExceptionPolicyFactory((IConfigurationSource)null);
                Assert.Fail("should have thrown");
            }
            catch (ArgumentNullException e)
            {
                this.exception = e;
            }
        }

        [TestMethod]
        public void then_throws()
        {
            Assert.IsNotNull(this.exception);
            Assert.AreEqual("configurationSource", this.exception.ParamName);
        }
    }

    public class given_factory_with_null_configuration_section : ArrangeActAssert
    {
        protected ExceptionPolicyFactory factory;

        protected override void Arrange()
        {
            this.factory = new ExceptionPolicyFactory(n => null);
        }

        [TestClass]
        public class when_creating_policy_with_null_name : given_factory_with_null_configuration_section
        {
            private ArgumentNullException exception;

            protected override void Act()
            {
                try
                {
                    this.factory.CreatePolicy(null);
                    Assert.Fail("should have thrown");
                }
                catch (ArgumentNullException e)
                {
                    this.exception = e;
                }
            }

            [TestMethod]
            public void then_throws()
            {
                Assert.IsNotNull(this.exception);
            }
        }

        [TestClass]
        public class when_creating_policy_with_valid_name : given_factory_with_null_configuration_section
        {
            private InvalidOperationException exception;

            protected override void Act()
            {
                try
                {
                    this.factory.CreatePolicy("policy");
                    Assert.Fail("should have thrown");
                }
                catch (InvalidOperationException e)
                {
                    this.exception = e;
                }
            }

            [TestMethod]
            public void then_throws()
            {
                Assert.IsNotNull(this.exception);
            }
        }

        [TestClass]
        public class when_creating_manager : given_factory_with_null_configuration_section
        {
            private InvalidOperationException exception;

            protected override void Act()
            {
                try
                {
                    this.factory.CreateManager();
                    Assert.Fail("should have thrown");
                }
                catch (InvalidOperationException e)
                {
                    this.exception = e;
                }
            }

            [TestMethod]
            public void then_throws()
            {
                Assert.IsNotNull(this.exception);
            }
        }
    }

    public class given_factory_with_error_throwing_configuration_section : ArrangeActAssert
    {
        protected ExceptionPolicyFactory factory;

        protected override void Arrange()
        {
            this.factory = new ExceptionPolicyFactory(n => { throw new ConfigurationErrorsException(); });
        }

        [TestClass]
        public class when_creating_policy_with_null_name : given_factory_with_error_throwing_configuration_section
        {
            private ArgumentNullException exception;

            protected override void Act()
            {
                try
                {
                    this.factory.CreatePolicy(null);
                    Assert.Fail("should have thrown");
                }
                catch (ArgumentNullException e)
                {
                    this.exception = e;
                }
            }

            [TestMethod]
            public void then_throws()
            {
                Assert.IsNotNull(this.exception);
            }
        }

        [TestClass]
        public class when_creating_policy_with_valid_name : given_factory_with_error_throwing_configuration_section
        {
            private InvalidOperationException exception;

            protected override void Act()
            {
                try
                {
                    this.factory.CreatePolicy("policy");
                    Assert.Fail("should have thrown");
                }
                catch (InvalidOperationException e)
                {
                    this.exception = e;
                }
            }

            [TestMethod]
            public void then_throws()
            {
                Assert.IsNotNull(this.exception);
            }
        }

        [TestClass]
        public class when_creating_manager : given_factory_with_error_throwing_configuration_section
        {
            private InvalidOperationException exception;

            protected override void Act()
            {
                try
                {
                    this.factory.CreateManager();
                    Assert.Fail("should have thrown");
                }
                catch (InvalidOperationException e)
                {
                    this.exception = e;
                }
            }

            [TestMethod]
            public void then_throws()
            {
                Assert.IsNotNull(this.exception);
            }
        }
    }

    public class given_factory_with_empty_configuration_section : ArrangeActAssert
    {
        protected ExceptionPolicyFactory factory;

        protected override void Arrange()
        {
            this.factory = new ExceptionPolicyFactory(n => new ExceptionHandlingSettings());
        }

        [TestClass]
        public class when_creating_policy_with_null_name : given_factory_with_empty_configuration_section
        {
            private ArgumentNullException exception;

            protected override void Act()
            {
                try
                {
                    this.factory.CreatePolicy(null);
                    Assert.Fail("should have thrown");
                }
                catch (ArgumentNullException e)
                {
                    this.exception = e;
                }
            }

            [TestMethod]
            public void then_throws()
            {
                Assert.IsNotNull(this.exception);
            }
        }

        [TestClass]
        public class when_creating_policy_with_valid_name : given_factory_with_empty_configuration_section
        {
            private InvalidOperationException exception;

            protected override void Act()
            {
                try
                {
                    this.factory.CreatePolicy("policy");
                    Assert.Fail("should have thrown");
                }
                catch (InvalidOperationException e)
                {
                    this.exception = e;
                }
            }

            [TestMethod]
            public void then_throws()
            {
                Assert.IsNotNull(this.exception);
            }
        }

        [TestClass]
        public class when_creating_manager : given_factory_with_empty_configuration_section
        {
            private ExceptionManager manager;

            protected override void Act()
            {
                this.manager = this.factory.CreateManager();
            }

            [TestMethod]
            public void then_creates_manager()
            {
                Assert.IsNotNull(this.manager);
            }

            [TestMethod]
            public void then_manager_has_configured_policies()
            {
                var exception = new Exception("probe exception");

                try
                {
                    this.manager.HandleException(exception, "non existing");
                    Assert.Fail("should have thrown");
                }
                catch (ExceptionHandlingException)
                {
                    // expected
                }
            }
        }
    }

    public class given_factory_with_non_empty_configuration_section : ArrangeActAssert
    {
        protected ExceptionPolicyFactory factory;

        protected override void Arrange()
        {
            this.factory =
                new ExceptionPolicyFactory(n =>
                    new ExceptionHandlingSettings
                    {
                        ExceptionPolicies =
                        {
                            new ExceptionPolicyData
                            { 
                                Name = "existing", 
                                ExceptionTypes = 
                                {
                                    new ExceptionTypeData
                                    {
                                        Type = typeof(Exception),
                                        ExceptionHandlers = 
                                        {
                                            new CustomHandlerData{ Type = typeof(MockExceptionHandler), Name = "mock" }
                                        }
                                    }
                                }
                            }
                        }
                    });
        }

        [TestClass]
        public class when_creating_policy_with_null_name : given_factory_with_non_empty_configuration_section
        {
            private ArgumentNullException exception;

            protected override void Act()
            {
                try
                {
                    this.factory.CreatePolicy(null);
                    Assert.Fail("should have thrown");
                }
                catch (ArgumentNullException e)
                {
                    this.exception = e;
                }
            }

            [TestMethod]
            public void then_throws()
            {
                Assert.IsNotNull(this.exception);
            }
        }

        [TestClass]
        public class when_creating_policy_with_non_existing_name : given_factory_with_non_empty_configuration_section
        {
            private InvalidOperationException exception;

            protected override void Act()
            {
                try
                {
                    this.factory.CreatePolicy("non existing");
                    Assert.Fail("should have thrown");
                }
                catch (InvalidOperationException e)
                {
                    this.exception = e;
                }
            }

            [TestMethod]
            public void then_throws()
            {
                Assert.IsNotNull(this.exception);
            }
        }

        [TestClass]
        public class when_creating_policy_with_existing_name : given_factory_with_non_empty_configuration_section
        {
            private ExceptionPolicyDefinition policy;

            protected override void Act()
            {
                this.policy = this.factory.CreatePolicy("existing");
            }

            [TestMethod]
            public void then_creates_policy()
            {
                Assert.IsNotNull(this.policy);
            }

            [TestMethod]
            public void then_policy_is_created_as_configured()
            {
                var exception = new Exception("probe exception");

                this.policy.HandleException(exception);
                Assert.AreEqual(exception.Message, MockExceptionHandler.lastMessage);
            }
        }

        [TestClass]
        public class when_creating_manager : given_factory_with_non_empty_configuration_section
        {
            private ExceptionManager manager;

            protected override void Act()
            {
                this.manager = this.factory.CreateManager();
            }

            [TestMethod]
            public void then_creates_manager()
            {
                Assert.IsNotNull(this.manager);
            }

            [TestMethod]
            public void then_manager_has_configured_policies()
            {
                var exception = new Exception("probe exception");

                this.manager.HandleException(exception, "existing");
                Assert.AreEqual(exception.Message, MockExceptionHandler.lastMessage);

                try
                {
                    this.manager.HandleException(exception, "non existing");
                    Assert.Fail("should have thrown");
                }
                catch (ExceptionHandlingException)
                {
                    // expected
                }

            }
        }
    }
}