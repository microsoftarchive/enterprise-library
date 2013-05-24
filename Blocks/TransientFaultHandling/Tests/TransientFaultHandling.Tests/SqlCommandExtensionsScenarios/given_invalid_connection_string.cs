#region license
// ==============================================================================
// Microsoft patterns & practices Enterprise Library
// Transient Fault Handling Application Block
// ==============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ==============================================================================
#endregion

namespace Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling.Tests.SqlCommandExtensionsScenarios.given_invalid_connection_string
{
    using System;
    using System.Data;
    using System.Data.SqlClient;

    using Common.TestSupport.ContextBase;

    using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling.TestSupport;
    using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;

    using VisualStudio.TestTools.UnitTesting;

    public abstract class Context : ArrangeActAssert
    {
        protected TestRetryStrategy connectionStrategy;
        protected TestRetryStrategy commandStrategy;
        protected SqlCommand command;
        protected RetryPolicy connectionPolicy;
        protected RetryPolicy commandPolicy;

        protected override void Arrange()
        {
            this.command = new SqlCommand(TestSqlSupport.ValidSqlText);

            this.connectionStrategy = new TestRetryStrategy();

            this.commandStrategy = new TestRetryStrategy();

            this.connectionPolicy = new RetryPolicy<AlwaysTransientErrorDetectionStrategy>(this.connectionStrategy);

            this.commandPolicy = new RetryPolicy<AlwaysTransientErrorDetectionStrategy>(this.commandStrategy);
        }
    }

    [TestClass]
    public class when_executing_non_query_command_with_closed_connection : Context
    {
        protected override void Act()
        {
            try
            {
                this.command.Connection = new SqlConnection(TestSqlSupport.InvalidConnectionString);
                this.command.ExecuteNonQueryWithRetry(this.commandPolicy, this.connectionPolicy);
                Assert.Fail();
            }
            catch (SqlException)
            {
            }
        }

        [TestMethod]
        public void then_connection_is_closed()
        {
            Assert.IsNotNull(this.command.Connection);
            Assert.IsTrue(this.command.Connection.State == ConnectionState.Closed);
        }

        [TestMethod]
        public void then_retried()
        {
            Assert.AreEqual(2, this.connectionStrategy.ShouldRetryCount);
            Assert.AreEqual(1, this.commandStrategy.ShouldRetryCount);
        }
    }

    [TestClass]
    public class when_executing_non_query_command_with_no_connection : Context
    {
        protected override void Act()
        {
            try
            {
                this.command.ExecuteNonQueryWithRetry(this.commandPolicy, this.connectionPolicy);
                Assert.Fail();
            }
            catch (InvalidOperationException)
            {
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void then_did_not_retried()
        {
            Assert.AreEqual(0, this.connectionStrategy.ShouldRetryCount);
            Assert.AreEqual(0, this.commandStrategy.ShouldRetryCount);
        }
    }

    [TestClass]
    public class when_executing_scalar_command_with_closed_connection : Context
    {
        protected override void Act()
        {
            try
            {
                this.command.Connection = new SqlConnection(TestSqlSupport.InvalidConnectionString);
                this.command.ExecuteScalarWithRetry(this.commandPolicy, this.connectionPolicy);
                Assert.Fail();
            }
            catch (SqlException)
            {
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void then_connection_is_closed()
        {
            Assert.IsNotNull(this.command.Connection);
            Assert.IsTrue(this.command.Connection.State == ConnectionState.Closed);
        }

        [TestMethod]
        public void then_retried()
        {
            Assert.AreEqual(2, this.connectionStrategy.ShouldRetryCount);
            Assert.AreEqual(1, this.commandStrategy.ShouldRetryCount);
        }
    }

    [TestClass]
    public class when_executing_scalar_command_with_no_connection : Context
    {
        protected override void Act()
        {
            try
            {
                this.command.ExecuteScalarWithRetry(this.commandPolicy, this.connectionPolicy);
                Assert.Fail();
            }
            catch (InvalidOperationException)
            {
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void then_did_not_retried()
        {
            Assert.AreEqual(0, this.connectionStrategy.ShouldRetryCount);
            Assert.AreEqual(0, this.commandStrategy.ShouldRetryCount);
        }
    }

    [TestClass]
    public class when_executing_reader_command_with_closed_connection : Context
    {
        protected override void Act()
        {
            try
            {
                this.command.Connection = new SqlConnection(TestSqlSupport.InvalidConnectionString);
                this.command.ExecuteReaderWithRetry(this.commandPolicy, this.connectionPolicy);
                Assert.Fail();
            }
            catch (SqlException)
            {
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void then_connection_is_closed()
        {
            Assert.IsNotNull(this.command.Connection);
            Assert.IsTrue(this.command.Connection.State == ConnectionState.Closed);
        }

        [TestMethod]
        public void then_retried()
        {
            Assert.AreEqual(2, this.connectionStrategy.ShouldRetryCount);
            Assert.AreEqual(1, this.commandStrategy.ShouldRetryCount);
        }
    }
    
    [TestClass]
    public class when_executing_reader_command_with_no_connection : Context
    {
        protected override void Act()
        {
            try
            {
                this.command.ExecuteReaderWithRetry(this.commandPolicy, this.connectionPolicy);
                Assert.Fail();
            }
            catch (InvalidOperationException)
            {
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void then_did_not_retried()
        {
            Assert.AreEqual(0, this.connectionStrategy.ShouldRetryCount);
            Assert.AreEqual(0, this.commandStrategy.ShouldRetryCount);
        }
    }

    [TestClass]
    public class when_executing_xml_reader_command_with_closed_connection : Context
    {
        protected override void Act()
        {
            try
            {
                this.command.Connection = new SqlConnection(TestSqlSupport.InvalidConnectionString);
                this.command.ExecuteXmlReaderWithRetry(this.commandPolicy, this.connectionPolicy);
                Assert.Fail();
            }
            catch (SqlException)
            {
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void then_connection_is_closed()
        {
            Assert.IsNotNull(this.command.Connection);
            Assert.IsTrue(this.command.Connection.State == ConnectionState.Closed);
        }

        [TestMethod]
        public void then_retried()
        {
            Assert.AreEqual(2, this.connectionStrategy.ShouldRetryCount);
            Assert.AreEqual(1, this.commandStrategy.ShouldRetryCount);
        }
    }

    [TestClass]
    public class when_executing_xml_reader_command_with_no_connection : Context
    {
        protected override void Act()
        {
            try
            {
                this.command.ExecuteXmlReaderWithRetry(this.commandPolicy, this.connectionPolicy);
                Assert.Fail();
            }
            catch (InvalidOperationException)
            {
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void then_did_not_retried()
        {
            Assert.AreEqual(0, this.connectionStrategy.ShouldRetryCount);
            Assert.AreEqual(0, this.commandStrategy.ShouldRetryCount);
        }
    }
}