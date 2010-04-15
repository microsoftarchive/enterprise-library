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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;

namespace Console.Wpf.Tests.VSTS.Converters
{
    public abstract class given_enumerable_to_boolean_converter : ArrangeActAssert
    {
        protected override void Arrange()
        {
            base.Arrange();

            Converter = new EnumerableToBooleanConverter();
        }

        protected EnumerableToBooleanConverter Converter { get; private set; }
    }
    
    [TestClass]
    public class when_converting_empty_enumerable : given_enumerable_to_boolean_converter
    {
        private object convertedValue;

        protected override void Act()
        {
            var enumerable = Enumerable.Empty<Object>();
            convertedValue = Converter.Convert(enumerable, typeof (bool), null, CultureInfo.CurrentCulture);
        }

        [TestMethod]
        public void then_converted_value_is_boolean()
        {
            Assert.IsInstanceOfType(convertedValue, typeof(bool));
        }

        [TestMethod]
        public void then_value_is_false()
        {
            Assert.AreEqual(false, convertedValue);
        }
    }

    [TestClass]
    public class when_converting_non_empty_enumerable : given_enumerable_to_boolean_converter
    {
        private object convertedValue;

        protected override void Act()
        {
            var enumerable = new object[] {new object(), new object(),};
            convertedValue = Converter.Convert(enumerable, typeof(bool), null, CultureInfo.CurrentCulture);
        }

        [TestMethod]
        public void then_converted_value_is_boolean()
        {
            Assert.IsInstanceOfType(convertedValue, typeof(bool));
        }

        [TestMethod]
        public void then_value_is_true()
        {
            Assert.AreEqual(true, convertedValue);
        }

    }
}
