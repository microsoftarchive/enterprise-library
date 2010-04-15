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

using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.TestClasses;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests.Instrumentation
{
    [TestClass]
    public class PerformanceCounterFixture
    {
        const string counterCategoryName = "Enterprise Library Validation Counters";
        NoPrefixNameFormatter formatter;
        const string instanceName = "Object";
        string formattedInstanceName;

        PerformanceCounter validationCallsCounter;
        PerformanceCounter validationCallsPerSecCounter;
        PerformanceCounter validationSucceededCounter;
        PerformanceCounter validationSucceededPerSecond;
        PerformanceCounter validationFailures;
        PerformanceCounter validationFailuresPerSecond;
        PerformanceCounter percentageValidationSuccesses;
        PerformanceCounter percentageValidationSuccessesBase;

        [TestInitialize]
        public void SetUp()
        {
            formatter = new NoPrefixNameFormatter();
            formattedInstanceName = formatter.CreateName(instanceName);

            validationCallsCounter = new PerformanceCounter(counterCategoryName, "Number of Validation Calls", formattedInstanceName, false);
            validationCallsCounter.RawValue = 0;

            validationCallsPerSecCounter = new PerformanceCounter(counterCategoryName, "Validation Calls/sec", formattedInstanceName, false);
            validationCallsPerSecCounter.RawValue = 0;

            validationSucceededCounter = new PerformanceCounter(counterCategoryName, "Number of Validation Successes", formattedInstanceName, false);
            validationSucceededCounter.RawValue = 0;

            validationSucceededPerSecond = new PerformanceCounter(counterCategoryName, "Validation Successes/sec", formattedInstanceName, false);
            validationSucceededPerSecond.RawValue = 0;

            validationFailures = new PerformanceCounter(counterCategoryName, "Number of Validation Failures", formattedInstanceName, false);
            validationFailures.RawValue = 0;

            validationFailuresPerSecond = new PerformanceCounter(counterCategoryName, "Validation Failures/sec", formattedInstanceName, false);
            validationFailuresPerSecond.RawValue = 0;

            percentageValidationSuccesses = new PerformanceCounter(counterCategoryName, "% Validation Successes", formattedInstanceName, false);
            percentageValidationSuccesses.RawValue = 0;

            percentageValidationSuccessesBase = new PerformanceCounter(counterCategoryName, "% Validation Successes Base", formattedInstanceName, false);
            percentageValidationSuccessesBase.RawValue = 0;
        }

        [TestCleanup]
        public void TearDown()
        {
            validationCallsCounter.Dispose();
            validationCallsPerSecCounter.Dispose();
            validationSucceededCounter.Dispose();
            validationSucceededPerSecond.Dispose();
            validationFailures.Dispose();
            validationFailuresPerSecond.Dispose();
            percentageValidationSuccesses.Dispose();
            percentageValidationSuccessesBase.Dispose();
        }

        [TestMethod]
        public void CallingValidateIncrementsNumberOfValidationCallsCounter()
        {
            GenericValidatorWrapper<object> validator = new GenericValidatorWrapper<object>(new MockValidator(false),
                                                                                            new ValidationInstrumentationProvider
                                                                                                (true, false, formatter));
            validator.Validate(this);
            Assert.AreEqual(1L, validationCallsCounter.RawValue);
        }

        [TestMethod]
        public void CallingValidateIncrementsValidationCalledCounterPerSecond()
        {
            ValidationInstrumentationProvider instrumentationProvider = new ValidationInstrumentationProvider(true, false, formatter);
            GenericValidatorWrapper<object> validator = new GenericValidatorWrapper<object>(new MockValidator(false), instrumentationProvider);

            validator.Validate(this);
            Assert.AreEqual(1L, validationCallsPerSecCounter.RawValue);
        }

        [TestMethod]
        public void FailedValidationIncrementsFailedValidationCounterPerSecond()
        {
            ValidationInstrumentationProvider instrumentationProvider = new ValidationInstrumentationProvider(true, false, formatter);
            GenericValidatorWrapper<object> validator = new GenericValidatorWrapper<object>(new MockValidator(true), instrumentationProvider);

            validator.Validate(this);
            Assert.AreEqual(1L, validationFailuresPerSecond.RawValue);
        }

        [TestMethod]
        public void FailedValidationIncrementsFailedValidationCounter()
        {
            ValidationInstrumentationProvider instrumentationProvider = new ValidationInstrumentationProvider(true, false, formatter);
            GenericValidatorWrapper<object> validator = new GenericValidatorWrapper<object>(new MockValidator(true), instrumentationProvider);

            validator.Validate(this);
            Assert.AreEqual(1L, validationFailures.RawValue);
        }

        [TestMethod]
        public void SuccessfulValidationIncrementsValidationSuccessCounter()
        {
            ValidationInstrumentationProvider instrumentationProvider = new ValidationInstrumentationProvider(true, false, formatter);
            GenericValidatorWrapper<object> validator = new GenericValidatorWrapper<object>(new MockValidator(false), instrumentationProvider);

            validator.Validate(this);
            Assert.AreEqual(1L, validationSucceededCounter.RawValue);
        }

        [TestMethod]
        public void SuccessfulValidationIncrementsValidationSuccessCounterPerSecond()
        {
            ValidationInstrumentationProvider instrumentationProvider = new ValidationInstrumentationProvider(true, false, formatter);
            GenericValidatorWrapper<object> validator = new GenericValidatorWrapper<object>(new MockValidator(false), instrumentationProvider);

            validator.Validate(this);
            Assert.AreEqual(1L, validationSucceededPerSecond.RawValue);
        }

        [TestMethod]
        public void PercentageSuccessIsUpdated()
        {
            ValidationInstrumentationProvider instrumentationProvider = new ValidationInstrumentationProvider(true, false, formatter);
            GenericValidatorWrapper<object> ValidValidator = new GenericValidatorWrapper<object>(new MockValidator(false), instrumentationProvider);
            GenericValidatorWrapper<object> InValidValidator = new GenericValidatorWrapper<object>(new MockValidator(true), instrumentationProvider);


            ValidValidator.Validate(this);
            ValidValidator.Validate(this);
            ValidValidator.Validate(this);

            Assert.AreEqual(100f, percentageValidationSuccesses.NextValue());

            InValidValidator.Validate(this);
            Assert.AreEqual(75f, percentageValidationSuccesses.NextValue());
        }
    }
}
