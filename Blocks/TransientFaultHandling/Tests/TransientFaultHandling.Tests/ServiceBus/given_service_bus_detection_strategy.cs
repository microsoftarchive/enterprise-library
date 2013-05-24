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

namespace Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling.Tests.ServiceBus.given_service_bus_detection_strategy
{
    using System;
    using System.ServiceModel;
    using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
    using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.ServiceBus.Messaging;
    using System.Net;

    public class Context : ArrangeActAssert
    {
        protected ServiceBusTransientErrorDetectionStrategy strategy;

        protected override void Arrange()
        {
            this.strategy = new ServiceBusTransientErrorDetectionStrategy();
        }
    }

    [TestClass]
    public class when_checking_exceptions : Context
    {
        [TestMethod]
        public void then_determines_null_failure_is_non_transient()
        {
            Assert.IsFalse(this.strategy.IsTransient(null));
        }

        [TestMethod]
        public void then_determines_transient_failure_is_transient()
        {
            Assert.IsTrue(this.strategy.IsTransient(new TimeoutException()));
        }

        [TestMethod]
        public void then_determines_non_transient_failure_is_non_transient()
        {
            Assert.IsFalse(this.strategy.IsTransient(new CommunicationObjectFaultedException()));
        }

        [TestMethod]
        public void then_determines_transient_failure_inner_of_non_transient_failure_is_transient()
        {
            Assert.IsTrue(this.strategy.IsTransient(new CommunicationObjectFaultedException("transient", new TimeoutException())));
        }

        [TestMethod]
        public void then_determines_transient_failure_inner_of_non_transient_failure_inner_of_non_transient_failure_is_transient()
        {
            Assert.IsFalse(this.strategy.IsTransient((new CommunicationObjectFaultedException("transient", new CommunicationObjectFaultedException("transient", new TimeoutException())))));
        }

        [TestMethod]
        public void then_determines_non_transient_failure__with_inner_web_exception_is_non_transient()
        {
            Assert.IsFalse(this.strategy.IsTransient((new MessagingEntityAlreadyExistsException("non transient", new WebException("Conflict", WebExceptionStatus.ProtocolError)))));
        }

        [TestMethod]
        public void then_determines_transient_unknown_messagingexception_is_transient()
        {
            Assert.IsTrue(this.strategy.IsTransient(new MessagingExceptionSubClass(true)));
        }

        [TestMethod]
        public void then_determines_nontransient_unknown_messagingexception_is_transient()
        {
            Assert.IsFalse(this.strategy.IsTransient(new MessagingExceptionSubClass(false)));
        }

        internal class MessagingExceptionSubClass : MessagingException
        {
            public MessagingExceptionSubClass(bool isTransient)
                : base("IsTransient=" + isTransient.ToString())
            {
                base.IsTransient = isTransient;
            }
        }
    }
}
