//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Silverlight Design-Time Configuration
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.ComponentModel;
using System;

namespace Microsoft.Practices.EnterpriseLibrary.Silverlight.Configuration.Tests.VSTS.BlockSpecific.Logging.given_logging_configuraton
{
    [TestClass]
    public class when_adding_notification_trace_listener : Context
    {
        protected override void Act()
        {
            Assert.AreEqual(0, TestLoggingSettings.TraceListeners.Count);
            AddNewNotificationTraceListener.Execute(null);
        }

        [TestMethod]
        public void then_newly_notification_trace_listener_added()
        {
            Assert.AreEqual(1, TestLoggingSettings.TraceListeners.Count);
            Assert.IsFalse(string.IsNullOrEmpty((string)AddNewNotificationTraceListener.AddedElementViewModel.Property("Name").Value));
        }

        [TestMethod]
        public void then_unsupported_properties_are_not_browsable()
        {
            var unsupportedAttributes = new [] { "TraceOutputOptions", "Filter" };

            foreach (var unsupportedAttribute in unsupportedAttributes)
            {
                Attribute att = AddNewNotificationTraceListener.AddedElementViewModel.Property(unsupportedAttribute).Attributes.First(x => x.GetType() == typeof(BrowsableAttribute));
                Assert.IsFalse(((BrowsableAttribute)att).Browsable);
            }
        }
    }
}
