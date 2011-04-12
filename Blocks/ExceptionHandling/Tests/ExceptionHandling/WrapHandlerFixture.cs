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
using System.Globalization;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
#if SILVERLIGHT
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel.Unity;
using Microsoft.Practices.Unity;
#endif

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests
{
    [TestClass]
    public class WrapHandlerFixture
    {
        const string message = "message";

#if SILVERLIGHT
        private IUnityContainer container;
        private ExceptionManager ExceptionPolicy;

        [TestInitialize]
        public void TestInitialize()
        {
            this.container = new UnityContainer();

            var configurationSource =
                ResourceDictionaryConfigurationSource.FromXaml(
                    new Uri(
                        "/Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Silverlight.Tests;component/Configuration.xaml",
                        UriKind.Relative));

            EnterpriseLibraryContainer.ConfigureContainer(new UnityContainerConfigurator(this.container), configurationSource);

            this.ExceptionPolicy = this.container.Resolve<ExceptionManager>();
        }


        [TestCleanup]
        public void TestCleanup()
        {
            this.container.Dispose();
        }
#endif

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void HandlerThrowsWhenNotWrappingAnException()
        {
            WrapHandler handler = new WrapHandler(message, typeof(object));
            handler.HandleException(new ApplicationException(), Guid.NewGuid());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructingWithNullExceptionTypeThrows()
        {
            WrapHandler handler = new WrapHandler(message, null);
        }

        [TestMethod]
        public void CanWrapException()
        {
            WrapHandler handler = new WrapHandler(message, typeof(ApplicationException));
            Exception ex = handler.HandleException(new InvalidOperationException(), Guid.NewGuid());

            Assert.AreEqual(typeof(ApplicationException), ex.GetType());
            Assert.AreEqual(typeof(ApplicationException), handler.WrapExceptionType);
            Assert.AreEqual(message, ex.Message);
            Assert.AreEqual(typeof(InvalidOperationException), ex.InnerException.GetType());
        }

        [TestMethod]
        public void WrapExceptionReturnsLocalizedMessage()
        {
            Exception exceptionToWrap = new Exception();
            Exception thrownException;
            ExceptionPolicy.HandleException(exceptionToWrap, "LocalizedWrapPolicy", out thrownException);

            Assert.AreEqual(Resources.ExceptionMessage, thrownException.Message);
        }

        [TestMethod]
#if !SILVERLIGHT
        [DeploymentItem(@"es\Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests.resources.dll", "es")]
#endif
        public void WrapExceptionReturnsLocalizedMessageBasedOnCurentUICulture()
        {
            CultureInfo originalCultureInfo = Thread.CurrentThread.CurrentUICulture;
            try
            {
                Exception exceptionToWrap = new Exception();
                Exception thrownException;

                Thread.CurrentThread.CurrentUICulture = new CultureInfo("es-es");

                ExceptionPolicy.HandleException(exceptionToWrap, "LocalizedWrapPolicy", out thrownException);

                Assert.AreEqual("caramba!", thrownException.Message);

                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-us");

                ExceptionPolicy.HandleException(exceptionToWrap, "LocalizedWrapPolicy", out thrownException);

                Assert.AreEqual("ooops!", thrownException.Message);
            }
            finally
            {
                Thread.CurrentThread.CurrentUICulture = originalCultureInfo;
            }
        }
    }
}
