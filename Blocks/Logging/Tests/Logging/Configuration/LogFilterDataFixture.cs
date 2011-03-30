//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.Configuration
{
    [TestClass]
    public class GivenPriorityFilterData
    {
        private LogFilterData filterData;

        [TestInitialize]
        public void Setup()
        {
            filterData = new PriorityFilterData { Name = "filter", MinimumPriority = 100, MaximumPriority = 200 };
        }

        [TestMethod]
        public void WhenCreatesRegistration_ThenCreatedRegistrationMapsLogFilterToPriorityFilterForTheSuppliedName()
        {
            filterData.GetRegistrations().First()
                .AssertForServiceType(typeof(ILogFilter))
                .ForName("filter")
                .ForImplementationType(typeof(PriorityFilter));
        }

        [TestMethod]
        public void WhenCreatesRegistration_ThenCreatedRegistrationHasTheExpectedConstructorParameters()
        {
            filterData.GetRegistrations().First()
                .AssertConstructor()
                .WithValueConstructorParameter("filter")
                .WithValueConstructorParameter(100)
                .WithValueConstructorParameter(200)
                .VerifyConstructorParameters();
        }

        [TestMethod]
        public void WhenCreatesRegistrations_ThenCreatedRegistrationsAreTransient()
        {
            Assert.AreEqual(
                0,
                filterData.GetRegistrations().Where(tr => tr.Lifetime != TypeRegistrationLifetime.Transient).Count());
        }
    }

    [TestClass]
    public class GivenCategoryFilterData
    {
        private LogFilterData filterData;

        [TestInitialize]
        public void Setup()
        {
            filterData =
                new CategoryFilterData
                {
                    Name = "filter",
                    CategoryFilters = 
                    { 
                        new CategoryFilterEntry{ Name = "category 1" }, 
                        new CategoryFilterEntry{ Name = "category 2" } 
                    },
                    CategoryFilterMode = CategoryFilterMode.DenyAllExceptAllowed
                };
        }

        [TestMethod]
        public void WhenCreatesRegistration_ThenCreatedRegistrationMapsLogFilterToCategoryFilterForTheSuppliedName()
        {
            filterData.GetRegistrations().First()
                .AssertForServiceType(typeof(ILogFilter))
                .ForName("filter")
                .ForImplementationType(typeof(CategoryFilter));
        }

        [TestMethod]
        public void WhenCreatesRegistration_ThenCreatedRegistrationHasTheExpectedConstructorParameters()
        {
            ICollection<string> categories;

            filterData.GetRegistrations().First()
                .AssertConstructor()
                .WithValueConstructorParameter("filter")
                .WithValueConstructorParameter(out categories)
                .WithValueConstructorParameter(CategoryFilterMode.DenyAllExceptAllowed)
                .VerifyConstructorParameters();

            CollectionAssert.AreEqual(new[] { "category 1", "category 2" }, (ICollection)categories);
        }

        [TestMethod]
        public void WhenCreatesRegistrations_ThenCreatedRegistrationsAreTransient()
        {
            Assert.AreEqual(
                0,
                filterData.GetRegistrations().Where(tr => tr.Lifetime != TypeRegistrationLifetime.Transient).Count());
        }
    }

    [TestClass]
    public class GivenLogEnabledFilterData
    {
        private LogFilterData filterData;

        [TestInitialize]
        public void Setup()
        {
            filterData = new LogEnabledFilterData { Name = "filter", Enabled = true };
        }

        [TestMethod]
        public void WhenCreatesRegistration_ThenCreatedRegistrationMapsLogFilterToCategoryFilterForTheSuppliedName()
        {
            filterData.GetRegistrations().First()
                .AssertForServiceType(typeof(ILogFilter))
                .ForName("filter")
                .ForImplementationType(typeof(LogEnabledFilter));
        }

        [TestMethod]
        public void WhenCreatesRegistration_ThenCreatedRegistrationHasTheExpectedConstructorParameters()
        {
            filterData.GetRegistrations().First()
                .AssertConstructor()
                .WithValueConstructorParameter("filter")
                .WithValueConstructorParameter(true)
                .VerifyConstructorParameters();
        }

        [TestMethod]
        public void WhenCreatesRegistrations_ThenCreatedRegistrationsAreTransient()
        {
            Assert.AreEqual(
                0,
                filterData.GetRegistrations().Where(tr => tr.Lifetime != TypeRegistrationLifetime.Transient).Count());
        }
    }

#if !SILVERLIGHT
    [TestClass]
    public class GivenCustomFilterDataForLogFilterType
    {
        private LogFilterData filterData;

        [TestInitialize]
        public void Setup()
        {
            filterData =
                new CustomLogFilterData("filter", typeof(MockCustomLogFilter))
                {
                    Attributes = { { "foo", "bar" } }
                };
        }

        [TestMethod]
        public void WhenCreatesRegistration_ThenCreatedRegistrationMapsLogFilterToCustomFilterForTheSuppliedName()
        {
            filterData.GetRegistrations().First()
                .AssertForServiceType(typeof(ILogFilter))
                .ForName("filter")
                .ForImplementationType(typeof(MockCustomLogFilter));
        }

        [TestMethod]
        public void WhenCreatesRegistration_ThenCreatedRegistrationHasTheExpectedConstructorParameters()
        {
            filterData.GetRegistrations().First()
                .AssertConstructor()
                .WithValueConstructorParameter(((CustomLogFilterData)filterData).Attributes)
                .VerifyConstructorParameters();
        }

        [TestMethod]
        public void WhenCreatesRegistrations_ThenCreatedRegistrationsAreTransient()
        {
            Assert.AreEqual(
                0,
                filterData.GetRegistrations().Where(tr => tr.Lifetime != TypeRegistrationLifetime.Transient).Count());
        }
    }
#endif
}
