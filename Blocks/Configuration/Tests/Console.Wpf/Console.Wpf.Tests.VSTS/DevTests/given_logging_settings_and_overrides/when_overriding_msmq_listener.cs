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
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;

namespace Console.Wpf.Tests.VSTS.DevTests.given_logging_settings_and_overrides
{
    [TestClass]
    public class when_overriding_msmq_listener : given_logging_settings_and_overrides
    {
        ElementViewModel msmqTracelistenerElement;
        Property overridesProperty;

        protected override void Act()
        {
            msmqTracelistenerElement =  base.LoggingSectionViewModel.GetDescendentsOfType<MsmqTraceListenerData>().First();
            overridesProperty = GetOverridesProperty(msmqTracelistenerElement);
            overridesProperty.Value = true;
        }

        [TestMethod]
        public void then_time_to_receive_can_be_overwritten()
        {
            Assert.IsTrue(overridesProperty.ChildProperties.Any(x => x.PropertyName == "TimeToBeReceived"));
        }

        [TestMethod]
        public void then_time_to_reach_queue_can_be_overwritten()
        {
            Assert.IsTrue(overridesProperty.ChildProperties.Any(x => x.PropertyName == "TimeToReachQueue"));
        }
    }
}
