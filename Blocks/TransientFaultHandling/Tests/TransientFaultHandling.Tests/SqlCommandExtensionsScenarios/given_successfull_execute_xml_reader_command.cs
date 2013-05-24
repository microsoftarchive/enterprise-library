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

namespace Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling.Tests.SqlCommandExtensionsScenarios.given_successfull_execute_xml_reader_command
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Xml;

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
            this.command = new SqlCommand(TestSqlSupport.ValidForXmlSqlQuery);

            this.connectionStrategy = new TestRetryStrategy();

            this.commandStrategy = new TestRetryStrategy();

            this.connectionPolicy = new RetryPolicy<AlwaysTransientErrorDetectionStrategy>(this.connectionStrategy);

            this.commandPolicy = new RetryPolicy<AlwaysTransientErrorDetectionStrategy>(this.commandStrategy);
        }
    }

    [TestClass]
    public class when_executing_command_with_closed_connection : Context
    {
        private XmlReader reader;

        protected override void Act()
        {
            this.command.Connection = new SqlConnection(TestSqlSupport.SqlDatabaseConnectionString);

            this.reader = this.command.ExecuteXmlReaderWithRetry(this.commandPolicy, this.connectionPolicy);
        }

        [TestMethod]
        public void then_connection_is_opened()
        {
            Assert.IsTrue(this.command.Connection.State == ConnectionState.Open);
        }

        [TestMethod]
        public void then_can_read_results()
        {
            Assert.IsTrue(this.reader.Read());
        }

        [TestMethod]
        public void then_retried()
        {
            Assert.AreEqual(0, this.connectionStrategy.ShouldRetryCount);
            Assert.AreEqual(0, this.commandStrategy.ShouldRetryCount);
        }
    }

    [TestClass]
    public class when_executing_command_with_opened_connection : Context
    {
        private XmlReader reader;

        protected override void Act()
        {
            this.command.Connection = new SqlConnection(TestSqlSupport.SqlDatabaseConnectionString);
            this.command.Connection.Open();

            this.reader = this.command.ExecuteXmlReaderWithRetry(this.commandPolicy, this.connectionPolicy);
        }

        [TestMethod]
        public void then_connection_is_opened()
        {
            Assert.IsTrue(this.command.Connection.State == ConnectionState.Open);
        }

        [TestMethod]
        public void then_can_read_results()
        {
            Assert.IsTrue(this.reader.Read());
        }

        [TestMethod]
        public void then_retried()
        {
            Assert.AreEqual(0, this.connectionStrategy.ShouldRetryCount);
            Assert.AreEqual(0, this.commandStrategy.ShouldRetryCount);
        }
    }
}