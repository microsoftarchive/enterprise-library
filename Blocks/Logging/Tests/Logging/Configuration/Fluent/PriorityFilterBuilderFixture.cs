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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.Configuration.Fluent
{
    public abstract class Given_PriorityFilterBuilder : Given_LoggingSettingsInConfigurationSourceBuilder
    {
        protected ILoggingConfigurationFilterOnPriority priorityFilterBuilder;

        protected override void Arrange()
        {
            base.Arrange();

            priorityFilterBuilder = base.ConfigureLogging.WithOptions.FilterOnPriority("prio filter");
        }

        protected PriorityFilterData GetPriorityFilterData()
        {
            return GetLoggingConfiguration().LogFilters.OfType<PriorityFilterData>().FirstOrDefault();
        }
    }

    [TestClass]
    public class When_CreatingPriorityFilterBuidlerPassingNullForName : Given_LoggingSettingsInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_FilterOnPriority_ThrowsArgumentException()
        {
            ConfigureLogging.WithOptions.FilterOnPriority(null);
        }
    }

    [TestClass]
    public class When_CreatinigPriorityFilterBuilder : Given_PriorityFilterBuilder
    {
        [TestMethod]
        public void Then_PriorityFilterHasApproriateName()
        {
            Assert.AreEqual("prio filter", GetPriorityFilterData().Name);
        }

        [TestMethod]
        public void Then_PriorityFilterHasCorrectType()
        {
            Assert.AreEqual(typeof(PriorityFilter), GetPriorityFilterData().Type);
        }

        [TestMethod]
        public void Then_PriorityFilterIsContainedInSettings()
        {
            Assert.IsTrue(GetLoggingConfiguration().LogFilters.OfType<PriorityFilterData>().Any());
        }

        [TestMethod]
        public void Then_MinimumPriorityIs0()
        {
            Assert.AreEqual(0, GetPriorityFilterData().MinimumPriority);
        }

        [TestMethod]
        public void Then_MaximumPriorityIsMaxInt()
        {
            Assert.AreEqual(Int32.MaxValue, GetPriorityFilterData().MaximumPriority);
        }
    }

    [TestClass]
    public class When_SettingMinimumPriorityOnPriorityFilterBuilder : Given_PriorityFilterBuilder
    {
        protected override void Act()
        {
            priorityFilterBuilder.StartingWithPriority(50);
        }

        [TestMethod]
        public void Then_MinimumPriorityIsSet()
        {
            Assert.AreEqual(50, GetPriorityFilterData().MinimumPriority);
        }
    }

    [TestClass]
    public class When_SettingMaximumPriorityOnPriorityFilterBuilder : Given_PriorityFilterBuilder
    {
        protected override void Act()
        {
            priorityFilterBuilder.UpToPriority(150);
        }

        [TestMethod]
        public void Then_MinimumPriorityIsSet()
        {
            Assert.AreEqual(150, GetPriorityFilterData().MaximumPriority);
        }
    }


    [TestClass]
    public class When_SettingMaximumAndMinimumPriorityOnPriorityFilterBuilder : Given_PriorityFilterBuilder
    {
        protected override void Act()
        {
            priorityFilterBuilder.StartingWithPriority(50).UpToPriority(150);
        }

        [TestMethod]
        public void Then_MinimumPriorityIsSet()
        {
            Assert.AreEqual(50, GetPriorityFilterData().MinimumPriority);
            Assert.AreEqual(150, GetPriorityFilterData().MaximumPriority);
        }
    }
}
