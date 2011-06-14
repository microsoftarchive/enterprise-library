//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
#if !SILVERLIGHT
    using System.Diagnostics;
#else
    using Microsoft.Practices.EnterpriseLibrary.Logging.Diagnostics;
#endif


namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Tests.Configuration
{
    [TestClass]
    public class GivenLoggingExceptionHandlerRegistrations : ArrangeActAssert
    {
        private IEnumerable<TypeRegistration> registrations;
        private LoggingExceptionHandlerData configuration;

        protected override void Arrange()
        {
            configuration = new LoggingExceptionHandlerData() { LogCategory = "General" };
        }

        protected override void Act()
        {
            registrations = configuration.GetRegistrations("");
        }

        [TestMethod]
        public void ThenLoggingExceptionHandlerHasTransientLifetime()
        {
            TypeRegistration loggingExceptionHandlerRegistration = registrations.Where(x => x.ServiceType == typeof(IExceptionHandler)).First();
            Assert.AreEqual(TypeRegistrationLifetime.Transient, loggingExceptionHandlerRegistration.Lifetime);
        }
        
        [TestMethod]
        public void ThenDefaultPropertyAreProperlySetted()
        {
            Assert.AreEqual(100, configuration.EventId);
            Assert.AreEqual(TraceEventType.Error, configuration.Severity);
            Assert.AreEqual("Enterprise Library Exception Handling", configuration.Title);
        }

        [TestMethod]
        public void ThenThrowsIfRequiredValueAreMissing()
        {
            try { new LoggingExceptionHandlerData { Title = null }.GetRegistrations("").ToArray(); Assert.Fail(); }
            catch (InvalidOperationException) { }
            catch (Exception) { Assert.Fail();}

            try { new LoggingExceptionHandlerData { LogCategory = null }.GetRegistrations("").ToArray(); Assert.Fail(); }
            catch (InvalidOperationException) { }
            catch (Exception) { Assert.Fail(); }

            try { new LoggingExceptionHandlerData { FormatterTypeName = null }.GetRegistrations("").ToArray(); Assert.Fail(); }
            catch (InvalidOperationException) { }
            catch (Exception) { Assert.Fail(); }
        }
    }
}
