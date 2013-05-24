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

using AExpense.DataAccessLayer;
using AExpense.FunctionalTests.Properties;
using AExpense.Model;
using AExpense.Tests.Util;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Configuration;
using System.Linq;
using System.Security.Principal;
using System.Threading;

namespace AExpense.Tests.Functional
{
    [TestClass]
    public class ExceptionHandlingAppBlockFixture
    {
        private static readonly SimulatedLdapProfileStore LDAPStore = ProfileStoreHelper.GetProfileStore("aExpense");
        private static readonly string LoggingDatabaseConnectionString = ConfigurationManager.ConnectionStrings["Logging"].ConnectionString;
        private static readonly ExceptionManager exceptionMgr = EnterpriseLibraryContainer.Current.GetInstance<ExceptionManager>();
        private IUnityContainer container;

        [TestInitialize]
        public void TestInitialize()
        {
            DatabaseHelper.CleanLoggingDB(LoggingDatabaseConnectionString);
            container = new UnityContainer();
            ContainerBootstrapper.Configure(container);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            container.Dispose();
        }

        [TestMethod]
        public void GetWrongUserReplacesException()
        {
            string username = "ADATUM\\WrongUser";
            var roles = new[] { "Employee" };
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(username), roles);

            var repository = new UserRepository(LDAPStore, exceptionMgr, container);

            //assert that the friendly excetpion is thrown to the UI
            var ex = ExceptionAssertHelper.Throws<NotifyException>(
              () => repository.GetUser(username));

            Assert.IsNotNull(ex, "Exception not thrown");
            StringAssert.Contains(ex.Message, Resources.FriendlyMessage);

            //assert that the inner execption is logged to the database

            var errors = DatabaseHelper.GetExceptionsFromDB(LoggingDatabaseConnectionString);
            var error = errors.FirstOrDefault();
            Assert.IsNotNull(error, "Inner exception not logged to the db");
            StringAssert.Contains(error.FormattedMessage, Resources.UserNotRegisteredMessage);
            StringAssert.Contains(error.Title, Resources.NotifyExceptionTitle);

        }

        [TestMethod]
        public void InvalidUserInUpdateReplacesException()
        {

            var repository = new UserRepository(LDAPStore, exceptionMgr, container);

            var ex = ExceptionAssertHelper.Throws<NotifyException>(
              () => repository.UpdateUserPreferredReimbursementMethod(null));

            //assert that the friendly execption is thrown to the UI

            Assert.IsNotNull(ex, "Exception not thrown");
            StringAssert.Contains(ex.Message, Resources.FriendlyMessage);

            //assert that the inner execption is logged to the database

            var errors = DatabaseHelper.GetExceptionsFromDB(LoggingDatabaseConnectionString);
            var error = errors.FirstOrDefault();
            Assert.IsNotNull(error, "Inner exception not logged to the db");
            StringAssert.Contains(error.FormattedMessage, "System.ArgumentNullException");
            StringAssert.Contains(error.Title, Resources.NotifyExceptionTitle);
        }
    }
}
