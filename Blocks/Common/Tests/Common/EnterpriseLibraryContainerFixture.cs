//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using Microsoft.Practices.ServiceLocation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests
{
    [TestClass]
    public class GivenALocatorPassedIntoEnterpriseLibraryContainer
    {
        private EnterpriseLibraryContainer container;
        private MockLocatorContext context;

        [TestInitialize]
        public void Given()
        {
            context = new MockLocatorContext();
            container = new EnterpriseLibraryContainer(context.MockLocator);
        }

        [TestMethod]
        public void WhenResolvingAnObject_ThenTheLocatorIsUsed()
        {
            var dummy = container.GetInstance<IDummyEntlibObject>();

            Assert.IsNotNull(dummy);
            context.Verify();
        }
    }

    [TestClass]
    public class GivenALocatorSetThroughCurrentProperty
    {
        private MockLocatorContext context;

        [TestInitialize]
        public void Given()
        {
            context = new MockLocatorContext();
            EnterpriseLibraryContainer.SetCurrentLocator(context.MockLocator);
        }

        [TestMethod]
        public void WhenResolvingAnObjectThroughCurrentContainer_ThenTheGivenLocatorIsUsed()
        {
            var dummy = EnterpriseLibraryContainer.Current.GetInstance<IDummyEntlibObject>();

            Assert.IsNotNull(dummy);
            context.Verify();
        }
    }

    internal class MockLocatorContext
    {
        private readonly Mock<ServiceLocatorImplBase> mock;

        public IServiceLocator MockLocator { get; private set; }

        public MockLocatorContext()
        {
            mock = new Mock<ServiceLocatorImplBase>(MockBehavior.Strict);
            IDummyEntlibObject mockDummy = new Mock<IDummyEntlibObject>().Object;

            mock.Setup(l => l.GetInstance(typeof (IDummyEntlibObject), null))
                .Returns(mockDummy)
                .AtMostOnce().Verifiable();
            MockLocator = mock.Object;
        }

        public void Verify()
        {
            mock.Verify();
        }
    }

    public interface IDummyEntlibObject
    {
    }

    internal class EnterpriseLibraryContainer : ServiceLocatorImplBase
    {
        private static readonly object globalContainerLock = new object();
        private static volatile EnterpriseLibraryContainer globalContainer;

        private readonly IServiceLocator locator;

        public EnterpriseLibraryContainer(IServiceLocator locator)
        {
            this.locator = locator;
        }

        public static EnterpriseLibraryContainer Current
        {
            get
            {
                lock (globalContainerLock)
                {
                    return globalContainer;
                }
            }
        }

        public static void SetCurrentLocator(IServiceLocator locator)
        {
                var newContainer = new EnterpriseLibraryContainer(locator);
                lock (globalContainerLock)
                {
                    globalContainer = newContainer;
                }
        }

        protected override object DoGetInstance(Type serviceType, string key)
        {
            return locator.GetInstance(serviceType, key);
        }

        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            return locator.GetAllInstances(serviceType);
        }
    }
}
