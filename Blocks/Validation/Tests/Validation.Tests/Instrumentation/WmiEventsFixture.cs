//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Validation Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.TestClasses;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests.Instrumentation
{
    [TestClass]
    public class WmiEventsFixture
    {
        [TestMethod]
        public void SuccessfulValidateFiresWmiEvent()
        {
            ValidatorWrapper validator = new ValidatorWrapper(new MockValidator(false));
            ValidationInstrumentationListener instrumentationListener = new ValidationInstrumentationListener(false, false, true, "fooApplicationInstanceName");

            new ReflectionInstrumentationBinder().Bind(validator.GetInstrumentationEventProvider(), instrumentationListener);

            using (WmiEventWatcher eventWatcher = new WmiEventWatcher(1))
            {
                validator.Validate(this);

                eventWatcher.WaitForEvents();

                Assert.AreEqual(1, eventWatcher.EventsReceived.Count);
                Assert.AreEqual("ValidationSucceededEvent", eventWatcher.EventsReceived[0].ClassPath.ClassName);
            }
        }

        [TestMethod]
        public void FailedValidateFiresWmiEvent()
        {
            ValidatorWrapper validator = new ValidatorWrapper(new MockValidator(true));
            ValidationInstrumentationListener instrumentationListener = new ValidationInstrumentationListener(false, false, true, "fooApplicationInstanceName");

            new ReflectionInstrumentationBinder().Bind(validator.GetInstrumentationEventProvider(), instrumentationListener);

            using (WmiEventWatcher eventWatcher = new WmiEventWatcher(1))
            {
                validator.Validate(this);

                eventWatcher.WaitForEvents();

                Assert.AreEqual(1, eventWatcher.EventsReceived.Count);
                Assert.AreEqual("ValidationFailedEvent", eventWatcher.EventsReceived[0].ClassPath.ClassName);
            }
        }

        [TestMethod]
        public void ExceptionDuringValidateFiresWmiEvent()
        {
            ValidatorWrapper validator = new ValidatorWrapper(new MockValidator(new Exception()));
            ValidationInstrumentationListener instrumentationListener = new ValidationInstrumentationListener(false, false, true, "fooApplicationInstanceName");

            new ReflectionInstrumentationBinder().Bind(validator.GetInstrumentationEventProvider(), instrumentationListener);

            using (WmiEventWatcher eventWatcher = new WmiEventWatcher(1))
            {
                try
                {
                    validator.Validate(this);
                    Assert.Fail();
                }
                catch { }
                eventWatcher.WaitForEvents();

                Assert.AreEqual(1, eventWatcher.EventsReceived.Count);
                Assert.AreEqual("ValidationExceptionEvent", eventWatcher.EventsReceived[0].ClassPath.ClassName);
            }
        }
    }
}
