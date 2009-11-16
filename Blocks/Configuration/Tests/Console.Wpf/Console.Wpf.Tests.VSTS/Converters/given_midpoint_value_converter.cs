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
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;

namespace Console.Wpf.Tests.VSTS.Converters
{
    [TestClass]
    public class when_converting_positive_value : ArrangeActAssert
    {
        private object convertedValue;
        private const double value = 413.313;

        protected override void Act()
        {
            var converter = new MidpointConverter();
            convertedValue = converter.Convert(value, typeof (double), null, CultureInfo.CurrentCulture);
        }

        [TestMethod]
        public void then_converted_value_is_half()
        {
            Assert.AreEqual(206.6565, convertedValue);
        }
    }

   
}
