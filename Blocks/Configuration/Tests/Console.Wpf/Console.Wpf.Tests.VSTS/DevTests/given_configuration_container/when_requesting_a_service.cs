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
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services.PlatformProfile;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;

namespace Console.Wpf.Tests.VSTS.DevTests.given_configuration_container
{
    public class given_configuration_container_with_no_parent_service_provider : ArrangeActAssert
    {
        protected ConfigurationContainer container;

        protected override void Arrange()
        {
            this.container = new ConfigurationContainer();
        }
    }

    public class given_configuration_container_with_parent_service_provider : ArrangeActAssert, IServiceProvider
    {
        protected ConfigurationContainer container;

        protected override void Arrange()
        {
            this.container = new ConfigurationContainer(this);
        }

        object IServiceProvider.GetService(Type serviceType)
        {
            if (serviceType == typeof(IFormatProvider)) return this;
            return null;
        }
    }

    [TestClass]
    public class when_requesting_existing_service : given_configuration_container_with_no_parent_service_provider
    {
        private object service;

        protected override void Act()
        {
            service = container.GetService(typeof(IServiceProvider));
        }

        [TestMethod]
        public void then_service_is_not_null()
        {
            Assert.IsNotNull(service);
        }
    }

    [TestClass]
    public class when_requesting_non_existing_service : given_configuration_container_with_no_parent_service_provider
    {
        private object service;

        protected override void Act()
        {
            service = container.GetService(typeof(IEnumerable));
        }

        [TestMethod]
        public void then_service_is_null()
        {
            Assert.IsNull(service);
        }
    }

    [TestClass]
    public class when_requesting_service_supplied_by_the_container : given_configuration_container_with_parent_service_provider
    {
        private object service;

        protected override void Act()
        {
            service = container.GetService(typeof(IServiceProvider));
        }

        [TestMethod]
        public void then_service_is_not_null()
        {
            Assert.IsNotNull(service);
        }
    }

    [TestClass]
    public class when_requesting_service_provider_by_the_parent : given_configuration_container_with_parent_service_provider
    {
        private object service;

        protected override void Act()
        {
            service = container.GetService(typeof(IFormatProvider));
        }

        [TestMethod]
        public void then_service_not_null()
        {
            Assert.IsNotNull(service);
        }


        [TestMethod]
        public void then_service_instance_is_provided_by_parent()
        {
            Assert.AreSame(this, service);
        }
    }

    [TestClass]
    public class when_requesting_service_not_supplied_by_container_or_parent : given_configuration_container_with_parent_service_provider
    {
        private object service;

        protected override void Act()
        {
            service = container.GetService(typeof(IEnumerable));
        }

        [TestMethod]
        public void then_service_is_null()
        {
            Assert.IsNull(service);
        }
    }

    [TestClass]
    public class when_requesting_default_profile : given_configuration_container_with_parent_service_provider
    {
        private object service;

        protected override void Act()
        {
            service = container.GetService(typeof(Profile));
        }

        [TestMethod]
        public void then_service_is_null()
        {
            Assert.IsNotNull(service);
        }
    }
}
