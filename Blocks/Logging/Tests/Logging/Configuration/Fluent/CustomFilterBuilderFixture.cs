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
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Specialized;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.Configuration.Fluent
{
    public abstract class Given_CustomFilterBuilder : Given_LoggingSettingsInConfigurationSourceBuilder
    {
        protected Type customFilterType = typeof(CustomLogFilter);
        protected string customFilterName=  "custom filter";

        protected override void Act()
        {
            base.ConfigureLogging.WithOptions.FilterCustom(customFilterName, customFilterType);
        }

        protected CustomLogFilterData GetCustomFilterData()
        {
            return GetLoggingConfiguration().LogFilters.OfType<CustomLogFilterData>().FirstOrDefault();
        }

        protected class CustomLogFilter : LogFilter
        {
            public CustomLogFilter() : base("")
            {
            }

            public override bool Filter(LogEntry log)
            {
                throw new NotImplementedException();
            }
        }
    }
    
    [TestClass]
    public class When_CreatingCustomFilterBuilderPassingNullForName : Given_CustomFilterBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_FilterCustom_ThrowsArgumentException()
        {
            ConfigureLogging.WithOptions.FilterCustom(null, typeof(CustomLogFilter));
        }
    }

    [TestClass]
    public class When_CreatingCustomFilterBuilderPassingNullForType : Given_LoggingSettingsInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Then_FilterCustom_ThrowsArgumentNullException()
        {
            ConfigureLogging.WithOptions.FilterCustom("filter name", null);
        }
    }

    [TestClass]
    public class When_CreatingCustomFilterBuilderPassingNonFilterType : Given_LoggingSettingsInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_FilterCustom_ThrowsArgumentException()
        {
            ConfigureLogging.WithOptions.FilterCustom("filter name", typeof(object));
        }
    }

    [TestClass]
    public class When_CreatingCustomFilterBuilderPassingNullForAttributes : Given_CustomFilterBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Then_FilterCustom_ThrowsArgumentNullException()
        {
            ConfigureLogging.WithOptions.FilterCustom("filter name", typeof(CustomLogFilter), null);
        }
    }
    
    [TestClass]
    public class When_CreatingCustomFilterBuilder : Given_CustomFilterBuilder
    {
        [TestMethod]
        public void Then_CustomFilterDataHasSpecifiedType()
        {
            Assert.AreEqual(customFilterType, GetCustomFilterData().Type);
        }

        [TestMethod]
        public void Then_CustomFilterDataHasSpecifiedName()
        {
            Assert.AreEqual(customFilterName, GetCustomFilterData().Name);
        }
        
        [TestMethod]
        public void Then_ConfigurationContainsCustomFilter()
        {
            Assert.IsTrue(GetLoggingConfiguration().LogFilters.OfType<CustomLogFilterData>().Any());
        }
    }
    
    [TestClass]
    public class When_CreatingCustomFilterBuilderGeneric : Given_CustomFilterBuilder
    {
        protected override void  Act()
        {
            base.ConfigureLogging.WithOptions.FilterCustom<CustomLogFilter>(customFilterName);
        }

        [TestMethod]
        public void Then_CustomFilterDataHasSpecifiedType()
        {
            Assert.AreEqual(customFilterType, GetCustomFilterData().Type);
        }

        [TestMethod]
        public void Then_CustomFilterDataHasSpecifiedName()
        {
            Assert.AreEqual(customFilterName, GetCustomFilterData().Name);
        }
    }

    [TestClass]
    public class When_CreatingCustomFilterBuilderPassingAttributes : Given_CustomFilterBuilder
    {
        NameValueCollection attributes = new NameValueCollection();

        protected override void Act()
        {
            attributes.Add("key1", "value1");
            attributes.Add("key2", "value2");
            base.ConfigureLogging.WithOptions.FilterCustom(customFilterName, customFilterType, attributes);
        }

        [TestMethod]
        public void Then_CustomFilterDataHasSpecifiedType()
        {
            Assert.AreEqual(customFilterType, GetCustomFilterData().Type);
        }

        [TestMethod]
        public void Then_CustomFormatterDataHasSpecifiedAttributes()
        {
            var formatterData = GetCustomFilterData();
            Assert.AreEqual(attributes.Count, formatterData.Attributes.Count);
            foreach (string attKey in attributes)
            {
                Assert.AreEqual(attributes[attKey], formatterData.Attributes[attKey]);
            }
        }

        [TestMethod]
        public void Then_CustomFilterDataHasSpecifiedName()
        {
            Assert.AreEqual(customFilterName, GetCustomFilterData().Name);
        }
    }

    [TestClass]
    public class When_CreatingCustomFilterBuilderPassingAttributesGeneric : Given_CustomFilterBuilder
    {
        NameValueCollection attributes = new NameValueCollection();

        protected override void Act()
        {
            attributes.Add("key1", "value1");
            attributes.Add("key2", "value2");
            base.ConfigureLogging.WithOptions.FilterCustom<CustomLogFilter>(customFilterName, attributes);
        }

        [TestMethod]
        public void Then_CustomFilterDataHasSpecifiedType()
        {
            Assert.AreEqual(typeof(CustomLogFilter), GetCustomFilterData().Type);
        }

        [TestMethod]
        public void Then_CustomFormatterDataHasSpecifiedAttributes()
        {
            var formatterData = GetCustomFilterData();
            Assert.AreEqual(attributes.Count, formatterData.Attributes.Count);
            foreach (string attKey in attributes)
            {
                Assert.AreEqual(attributes[attKey], formatterData.Attributes[attKey]);
            }
        }

        [TestMethod]
        public void Then_CustomFilterDataHasSpecifiedName()
        {
            Assert.AreEqual(customFilterName, GetCustomFilterData().Name);
        }
    }

}
