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
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Console.Wpf.Tests.VSTS.DevTests.given_host_adapter
{
    [TestClass]
    public class when_editing_property : given_host_adapter_and_type_descriptor
    {
        Property traceOutputOptions;
        PropertyDescriptor componentModelTraceOutputOptions;
        bool expirationPollFrequencyChanged;

        protected override void Arrange()
        {
            base.Arrange();

            traceOutputOptions = TraceListener.Property("TraceOutputOptions");
            componentModelTraceOutputOptions = TraceListenerTypeDescriptor.GetProperties().OfType<PropertyDescriptor>().Where(x => x.Name == "TraceOutputOptions").First();

            expirationPollFrequencyChanged = false;

            componentModelTraceOutputOptions.AddValueChanged(TraceListenerTypeDescriptor, new EventHandler((sender, args) => expirationPollFrequencyChanged = true));
        }

        protected override void Act()
        {
            traceOutputOptions.BindableProperty.BindableValue = "Callstack";
        }

        [TestMethod]
        public void then_property_descriptor_raises_changed_event()
        {
            Assert.IsTrue(expirationPollFrequencyChanged);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void then_property_descriptor_throws_exception_on_validation_error()
        {
            componentModelTraceOutputOptions.SetValue(null, "invalidvalue");
        }

        [TestMethod]
        public void then_propery_descriptor_type_is_property_type()
        {
            Assert.AreEqual(typeof(TraceOptions), componentModelTraceOutputOptions.PropertyType);
            Assert.IsInstanceOfType(componentModelTraceOutputOptions.GetValue(null), typeof(TraceOptions));
        }
    }
}
