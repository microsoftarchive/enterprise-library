#region license
// ==============================================================================
// Microsoft patterns & practices Enterprise Library
// aExpense Reference Implementation
// ==============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ==============================================================================
#endregion

using System;
using System.Configuration;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using AExpense.DataAccessLayer;
using AExpense.FunctionalTests.Properties;
using AExpense.FunctionalTests.Util;
using AExpense.Instrumentation;
using AExpense.Model;
using AExpense.Tests.Util;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Net;

namespace AExpense.Tests.Functional
{
    [TestClass]
    public class ExceptionHandlingAppBlockFixture
    {
        private IProfileStore ldapStore;
        private ExceptionManager exceptionMgr;
        private IUnityContainer container;
        private ObservableEventListener obsListener;
        private static readonly string TracingDatabaseConnectionString = ConfigurationManager.ConnectionStrings["Tracing"].ConnectionString;
        private static readonly string aExpenseDatabaseConnectionString = ConfigurationManager.ConnectionStrings["aExpense"].ConnectionString.Replace(@"\", @"\\");


        [TestInitialize]
        public void TestInitialize()
        {
            obsListener = new ObservableEventListener();
            obsListener.EnableEvents(AExpenseEvents.Log, EventLevel.LogAlways, Keywords.All);

            DatabaseHelper.CleanLoggingDB(TracingDatabaseConnectionString);

            this.container = new UnityContainer();
            ContainerBootstrapper.Configure(container);

            this.ldapStore = container.Resolve<IProfileStore>();
            this.exceptionMgr = container.Resolve<ExceptionManager>();

        }

        [TestCleanup]
        public void TestCleanup()
        {
            DatabaseHelper.CleanLoggingDB(TracingDatabaseConnectionString);

            this.obsListener.DisableEvents(AExpenseEvents.Log);
            this.obsListener.Dispose();

            this.container.Dispose();

        }

        [TestMethod]
        public void GetWrongUserReplacesException()
        {
            using (var subscription = obsListener.LogToSqlDatabase("test", TracingDatabaseConnectionString, bufferingCount: 1))
            {
                string username = "ADATUM\\WrongUser";
                var roles = new[] { "Employee" };
                Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(username), roles);

                var repository = new UserRepository(ldapStore, exceptionMgr, container);

                //assert that the friendly excetpion is thrown to the UI
                var ex = ExceptionAssertHelper.Throws<NotifyException>(() => repository.GetUser(username));

                subscription.Sink.FlushAsync().Wait();

                var entries = DatabaseHelper.GetAllLogEntries(TracingDatabaseConnectionString);
                Assert.AreEqual(2, entries.Count);
                Assert.IsTrue(entries.Any(e => e.Level == (int)EventLevel.Error));
                Assert.IsTrue(entries.Any(e => e.EventId == 1000));
                Assert.IsTrue(entries.Any(e => e.Payload.Contains(Resources.UserNotRegisteredMessage)));

                Assert.IsNotNull(ex, "Exception not thrown");
                StringAssert.Contains(ex.Message, Resources.FriendlyMessage);
            }
        }

        [TestMethod]
        public void InvalidUserInUpdateReplacesException()
        {
            using (var subscription = obsListener.LogToSqlDatabase("test", TracingDatabaseConnectionString, bufferingCount: 1))
            {
                var repository = new UserRepository(ldapStore, exceptionMgr, container);

                var ex = ExceptionAssertHelper.Throws<NotifyException>(
                  () => repository.UpdateUserPreferredReimbursementMethod(null));

                subscription.Sink.FlushAsync().Wait();

                var entries = DatabaseHelper.GetAllLogEntries(TracingDatabaseConnectionString);
                Assert.AreEqual(2, entries.Count);
                Assert.AreEqual((int)EventLevel.Error, entries.Last().Level);
                Assert.AreEqual(1000, entries.Last().EventId);
                StringAssert.Contains(entries.Last().Payload, "System.ArgumentNullException");

                //assert that the friendly execption is thrown to the UI

                Assert.IsNotNull(ex, "Exception not thrown");
                StringAssert.Contains(ex.Message, Resources.FriendlyMessage);
            }
        }
    }
}
