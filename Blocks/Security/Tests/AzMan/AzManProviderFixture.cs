//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Configuration;
using System.IO;
using System.Management;
using System.Security;
using System.Security.Principal;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Security.AzMan.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Instrumentation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Security.AzMan.Tests
{
    [TestClass]
    public class AzManProviderFixture
    {
        static readonly string scope = "http://entlib.com";
        static readonly string authorizedTask = "Authorize Purchase";
        static readonly string unauthorizedTask = "Fire Employee";
        IPrincipal cryptographyProviderCollection;
        AzManAuthorizationProvider azman;
        AzManAuthorizationProviderData data = new AzManAuthorizationProviderData();

        [TestInitialize]
        public void TestInitialize()
        {
            ReplaceSIDinConfigFile();
            cryptographyProviderCollection = new GenericPrincipal(
                WindowsIdentity.GetCurrent(),
                new string[] { "Guest" });
            data.Application = "Enterprise Library Unit Test";
            data.AuditIdentifierPrefix = "myAuditId";
            data.Scope = "";
            data.StoreLocation = @"msxml://{currentPath}/testAzStore.xml";
            azman = new AzManAuthorizationProvider(data.StoreLocation, data.Application, data.AuditIdentifierPrefix, data.Scope);
        }

        [TestMethod]
        public void AuthorizeTask()
        {
            bool res = azman.Authorize(cryptographyProviderCollection, authorizedTask);
            Assert.IsTrue(res);
        }

        [TestMethod]
        public void AuthorizedTaskWithoutData()
        {
            IAuthorizationProvider azManProvider = AuthorizationFactory.GetAuthorizationProvider();

            bool res = azManProvider.Authorize(cryptographyProviderCollection, authorizedTask);

            Assert.IsTrue(res);
        }

        [TestMethod]
        public void UnauthorizedTask()
        {
            bool res = azman.Authorize(cryptographyProviderCollection, unauthorizedTask);
            Assert.IsFalse(res);
        }

        [TestMethod]
        public void AuthorizedThenUnauthorizedTask()
        {
            bool res = azman.Authorize(cryptographyProviderCollection, authorizedTask);
            Assert.IsTrue(res);

            res = azman.Authorize(cryptographyProviderCollection, unauthorizedTask);
            Assert.IsFalse(res);

            res = azman.Authorize(cryptographyProviderCollection, authorizedTask);
            Assert.IsTrue(res);
        }

        [TestMethod]
        public void AuthorizedScopeTask()
        {
            data.Scope = scope;
            azman = new AzManAuthorizationProvider(data.StoreLocation, data.Application, data.AuditIdentifierPrefix, data.Scope);

            string task = "Manage Extranet";
            bool res = azman.Authorize(cryptographyProviderCollection, task);

            Assert.IsTrue(res);
        }

        [TestMethod]
        public void UnauthorizedScopeTask()
        {
            string task = "Publish Extranet";

            data.Scope = scope;
            azman = new AzManAuthorizationProvider(data.StoreLocation, data.Application, data.AuditIdentifierPrefix, data.Scope);
            bool res = azman.Authorize(cryptographyProviderCollection, task);

            Assert.IsFalse(res);
        }

        [TestMethod]
        public void AuthorizedOperationUsingPrefix()
        {
            string operation = "O:Approve Purchase Order";
            bool res = azman.Authorize(cryptographyProviderCollection, operation);

            Assert.IsTrue(res);
        }

        [TestMethod]
        [ExpectedExceptionAttribute(typeof(SecurityException))]
        public void InvalidTask()
        {
            azman.Authorize(cryptographyProviderCollection, "INVALID");
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void AuthorizeTaskWithNoOperations()
        {
            string task = "Missing Operations Task";
            azman.Authorize(cryptographyProviderCollection, task);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IdentityParameterIsNull()
        {
            WindowsIdentity ident = null;
            IPrincipal testPrincipal = new GenericPrincipal(
                ident,
                new string[] { "Guest" });

            azman.Authorize(testPrincipal, authorizedTask);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TaskParameterIsNull()
        {
            azman.Authorize(cryptographyProviderCollection, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TaskParameterLengthIsZero()
        {
            azman.Authorize(cryptographyProviderCollection, "");
        }

        [TestMethod]
        [ExpectedException(typeof(SecurityException))]
        public void InvalidApplication()
        {
            data.Application = "INVALID";
            azman = new AzManAuthorizationProvider(data.StoreLocation, data.Application, data.AuditIdentifierPrefix, data.Scope);
            azman.Authorize(cryptographyProviderCollection, authorizedTask);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void InvalidIdentityTypeThrows()
        {
            azman.Authorize(new GenericPrincipal(new GenericIdentity("Joe"), new string[] { }), authorizedTask);
        }

        [TestMethod]
        public void AuthorizationFailedFires2WmiEvents()
        {
            AzManAuthorizationProvider instrumentedAzman = new AzManAuthorizationProvider(data.StoreLocation, data.Application, data.AuditIdentifierPrefix, data.Scope);
            AuthorizationProviderInstrumentationListener listener = new AuthorizationProviderInstrumentationListener("foo", false, false, true, "fooApplicationInstanceName");

            ReflectionInstrumentationBinder binder = new ReflectionInstrumentationBinder();
            binder.Bind(instrumentedAzman.GetInstrumentationEventProvider(), listener);

            using (WmiEventWatcher eventWatcher = new WmiEventWatcher(2))
            {
                bool res = instrumentedAzman.Authorize(cryptographyProviderCollection, unauthorizedTask);

                eventWatcher.WaitForEvents();

                Assert.AreEqual(2, eventWatcher.EventsReceived.Count);

                Assert.AreEqual("AuthorizationCheckPerformedEvent", eventWatcher.EventsReceived[0].ClassPath.ClassName);
                Assert.AreEqual("foo", eventWatcher.EventsReceived[0].Properties["InstanceName"].Value);
                Assert.AreEqual(cryptographyProviderCollection.Identity.Name, eventWatcher.EventsReceived[0].Properties["UserName"].Value);
                Assert.AreEqual(unauthorizedTask, eventWatcher.EventsReceived[0].Properties["TaskName"].Value);

                Assert.AreEqual("AuthorizationCheckFailedEvent", eventWatcher.EventsReceived[1].ClassPath.ClassName);
                Assert.AreEqual("foo", eventWatcher.EventsReceived[1].Properties["InstanceName"].Value);
                Assert.AreEqual(cryptographyProviderCollection.Identity.Name, eventWatcher.EventsReceived[1].Properties["UserName"].Value);
                Assert.AreEqual(unauthorizedTask, eventWatcher.EventsReceived[1].Properties["TaskName"].Value);
            }
        }

        [TestMethod]
        public void AuthorizeFiresWmiEvent()
        {
            AzManAuthorizationProvider instrumentedAzman = new AzManAuthorizationProvider(data.StoreLocation, data.Application, data.AuditIdentifierPrefix, data.Scope);
            AuthorizationProviderInstrumentationListener listener = new AuthorizationProviderInstrumentationListener("foo", false, false, true, "fooApplicationInstanceName");

            ReflectionInstrumentationBinder binder = new ReflectionInstrumentationBinder();
            binder.Bind(instrumentedAzman.GetInstrumentationEventProvider(), listener);

            using (WmiEventWatcher eventWatcher = new WmiEventWatcher(1))
            {
                bool res = instrumentedAzman.Authorize(cryptographyProviderCollection, authorizedTask);

                eventWatcher.WaitForEvents();
                Thread.Sleep(500);

                Assert.AreEqual(1, eventWatcher.EventsReceived.Count);
                Assert.AreEqual("foo", eventWatcher.EventsReceived[0].Properties["InstanceName"].Value);
                Assert.AreEqual(cryptographyProviderCollection.Identity.Name, eventWatcher.EventsReceived[0].Properties["UserName"].Value);
                Assert.AreEqual(authorizedTask, eventWatcher.EventsReceived[0].Properties["TaskName"].Value);
            }
        }

        [TestMethod]
        public void PerformsReplacementsInStoreLocation()
        {
            // uses a new AppDomain to avoid issues with AppDomain.BaseDirectory and the mstest runner in VS 2008
            AppDomainSetup info = new AppDomainSetup();
            info.ApplicationBase = Environment.CurrentDirectory;

            AppDomain domain = null;
            try
            {
                domain = AppDomain.CreateDomain("azman test", null, info);
                domain.DoCallBack(PerformsReplacementsInStoreLocation_Imp);
            }
            finally
            {
                if (domain != null)
                {
                    AppDomain.Unload(domain);
                }
            }
        }

        private static void PerformsReplacementsInStoreLocation_Imp()
        {
            string location;

            location = @"msxml://ignored/testAzStore.xml";
            Assert.AreEqual(location, AzManAuthorizationProvider.GetStoreLocationPath(location));

            location = @"msxml://{currentPath}/testAzStore.xml";
            Assert.AreNotEqual(location, AzManAuthorizationProvider.GetStoreLocationPath(location));
            Assert.IsTrue(File.Exists((new Uri(AzManAuthorizationProvider.GetStoreLocationPath(location))).LocalPath));

            string currentDirectory = Environment.CurrentDirectory;
            try
            {
                Environment.CurrentDirectory = Environment.SystemDirectory;
                Assert.IsFalse(File.Exists((new Uri(AzManAuthorizationProvider.GetStoreLocationPath(location))).LocalPath));

                location = @"msxml://{baseDirectory}/testAzStore.xml";
                Assert.AreNotEqual(location, AzManAuthorizationProvider.GetStoreLocationPath(location));
                Assert.IsTrue(File.Exists((new Uri(AzManAuthorizationProvider.GetStoreLocationPath(location))).LocalPath));
            }
            finally
            {
                Environment.CurrentDirectory = currentDirectory;
            }
        }

        static void ReplaceSIDinConfigFile()
        {
            string sidToReplace = "S-1-5-21-839522115-764733703-1343024091-500";
            string currentSid = GetUserSID();

            // read current config file
            File.SetAttributes("testAzStore.xml", FileAttributes.Archive);
            FileStream f = new FileStream("testAzStore.xml", FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            StreamReader r = new StreamReader(f);
            string contents = r.ReadToEnd();
            f.Close();

            // write updated config file
            f = new FileStream("testAzStore.xml", FileMode.Truncate, FileAccess.ReadWrite, FileShare.ReadWrite);
            StreamWriter w = new StreamWriter(f);
            contents = contents.Replace(sidToReplace, currentSid);
            w.Write(contents);
            w.Flush();
            f.Close();
        }

        static string GetUserSID()
        {
            string currentSid = "";
            string ident = WindowsIdentity.GetCurrent().Name;
            string domain = ident.Substring(0, ident.IndexOf(@"\"));
            string user = ident.Substring(domain.Length + 1);

            // get current users sid
            string pGroup = "ROOT\\CIMV2:Win32_Account.Domain=\"" + domain + "\",Name=\"" + user + "\"";
            ManagementPath path = new ManagementPath(pGroup);
            using (ManagementObject o = new ManagementObject(path))
            {
                currentSid = o["SID"].ToString();
            }

            return currentSid;
        }
    }
}
