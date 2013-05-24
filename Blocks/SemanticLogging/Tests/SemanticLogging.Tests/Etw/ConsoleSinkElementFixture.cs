#region license
// ==============================================================================
// Microsoft patterns & practices Enterprise Library
// Semantic Logging Application Block
// ==============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ==============================================================================
#endregion

using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Etw.Configuration;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Tests.TestObjects;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Tests.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Tests.Etw
{
    public abstract class given_consoleSinkElement : ContextBase
    {
        protected ISinkElement Sut;
        private XElement element;

        protected override void Given()
        {
            this.element = new XElement(XName.Get("consoleSink", Constants.Namespace));

            this.Sut = new ConsoleSinkElement();
        }

        [TestClass]
        public class when_query_for_canCreateSink : given_consoleSinkElement
        {
            [TestMethod]
            public void then_instance_can_be_created()
            {
                Assert.IsTrue(this.Sut.CanCreateSink(this.element));
            }
        }

        [TestClass]
        public class when_createSink_with_default_parameters : given_consoleSinkElement
        {
            private IObserver<EventEntry> observer;

            protected override void When()
            {
                this.observer = this.Sut.CreateSink(this.element);
            }

            [TestMethod]
            public void then_sink_is_created()
            {
                Assert.IsNotNull(this.observer);
            }
        }

        [TestClass]
        public class when_createSink_with_colorMapper_parameter : given_consoleSinkElement
        {
            private IObserver<EventEntry> observer;

            protected override void When()
            {
                this.element.Add(new XAttribute("colorMapperType", typeof(MockColorMapper).AssemblyQualifiedName));
                this.observer = this.Sut.CreateSink(this.element);
            }

            [TestMethod]
            public void then_sink_is_created()
            {
                Assert.IsNotNull(MockColorMapper.Instance);
            }
        }
    }
}
