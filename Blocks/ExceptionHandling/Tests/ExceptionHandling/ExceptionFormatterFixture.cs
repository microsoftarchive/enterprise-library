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
using System.IO;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;
using System.Security.Principal;
using System.Text;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests
{
    [TestClass]
    public partial class ExceptionFormatterFixture
    {
        const string fileNotFoundMessage = "The file can't be found";
        const string theFile = "theFile";
        const string loggedTimeStampFailMessage = "Logged TimeStamp is not within a one minute time window";
        const string machineName = "MachineName";
        const string timeStamp = "TimeStamp";
        const string appDomainName = "AppDomainName";
        const string threadIdentity = "ThreadIdentity";
        const string windowsIdentity = "WindowsIdentity";
        const string fieldString = "FieldString";
        const string mockFieldString = "MockFieldString";
        const string propertyString = "PropertyString";
        const string mockPropertyString = "MockPropertyString";
        const string message = "Message";
        const string computerName = "COMPUTERNAME";
        const string permissionDenied = "Permission Denied";

        [TestMethod]
        public void AdditionalInfoTest()
        {
            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);

            Exception exception = new FileNotFoundException(fileNotFoundMessage, theFile);
            TextExceptionFormatter formatter = new TextExceptionFormatter(writer, exception);

            formatter.Format();

            if (string.Compare(permissionDenied, formatter.AdditionalInfo[machineName]) != 0)
            {
                Assert.AreEqual(Environment.MachineName, formatter.AdditionalInfo[machineName]);
            }

            DateTime minimumTime = DateTime.UtcNow.AddMinutes(-1);
            DateTime loggedTime = DateTime.Parse(formatter.AdditionalInfo[timeStamp]);
            if (DateTime.Compare(minimumTime, loggedTime) > 0)
            {
                Assert.Fail(loggedTimeStampFailMessage);
            }

            Assert.AreEqual(AppDomain.CurrentDomain.FriendlyName, formatter.AdditionalInfo[appDomainName]);
            Assert.AreEqual(Thread.CurrentPrincipal.Identity.Name, formatter.AdditionalInfo[threadIdentity]);

            if (string.Compare(permissionDenied, formatter.AdditionalInfo[windowsIdentity]) != 0)
            {
                Assert.AreEqual(WindowsIdentity.GetCurrent().Name, formatter.AdditionalInfo[windowsIdentity]);
            }
        }

        [TestMethod]
        public void ReflectionFormatterReadTest()
        {
            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);

            MockTextExceptionFormatter formatter
                = new MockTextExceptionFormatter(writer, new MockException(), Guid.Empty);

            formatter.Format();

            Assert.AreEqual(formatter.fields[fieldString], mockFieldString);
            Assert.AreEqual(formatter.properties[propertyString], mockPropertyString);
            // The message should be null because the reflection formatter should ignore this property
            Assert.AreEqual(null, formatter.properties[message]);
        }

        [TestMethod]
        public void CanGetMachineNameWithoutSecurity()
        {
            var evidence = new Evidence();
            evidence.AddHostEvidence(new Zone(SecurityZone.Internet));
            var set = SecurityManager.GetStandardSandbox(evidence);
            set.AddPermission(new ReflectionPermission(ReflectionPermissionFlag.MemberAccess));

            var domain = AppDomain.CreateDomain("partial trust", null, AppDomain.CurrentDomain.SetupInformation, set);

            try
            {
                var instance = ((ExceptionFormatterTester)domain.CreateInstanceAndUnwrap(typeof(ExceptionFormatterTester).Assembly.FullName, typeof(ExceptionFormatterTester).FullName));
                var formattedMessage = instance.DoTest();

                Assert.IsTrue(formattedMessage.Contains(threadIdentity + " : " + permissionDenied));
            }
            catch
            {
                throw;
            }
            finally
            {
                AppDomain.Unload(domain);
            }
        }

        [TestMethod]
        public void CanGetWindowsIdentityWithoutSecurity()
        {
            var evidence = new Evidence();
            evidence.AddHostEvidence(new Zone(SecurityZone.Internet));
            var set = SecurityManager.GetStandardSandbox(evidence);
            set.AddPermission(new ReflectionPermission(ReflectionPermissionFlag.MemberAccess));

            var domain = AppDomain.CreateDomain("partial trust", null, AppDomain.CurrentDomain.SetupInformation, set);

            try
            {
                var instance = ((ExceptionFormatterTester)domain.CreateInstanceAndUnwrap(typeof(ExceptionFormatterTester).Assembly.FullName, typeof(ExceptionFormatterTester).FullName));
                var formattedMessage = instance.DoTest();

                Assert.IsTrue(formattedMessage.Contains(windowsIdentity + " : " + permissionDenied));
            }
            catch
            {
                throw;
            }
            finally
            {
                AppDomain.Unload(domain);
            }
        }

        public class ExceptionFormatterTester : MarshalByRefObject
        {
            public string DoTest()
            {
                StringBuilder sb = new StringBuilder();
                StringWriter writer = new StringWriter(sb);
                Exception exception = new MockException();
                TextExceptionFormatter formatter = new TextExceptionFormatter(writer, exception);
                formatter.Format();

                return sb.ToString();
            }
        }

        [TestMethod]
        public void SkipsIndexerProperties()
        {
            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);

            Exception exception = new FileNotFoundExceptionWithIndexer(fileNotFoundMessage, theFile);
            TextExceptionFormatter formatter = new TextExceptionFormatter(writer, exception);

            formatter.Format();

            if (string.Compare(permissionDenied, formatter.AdditionalInfo[machineName]) != 0)
            {
                Assert.AreEqual(Environment.MachineName, formatter.AdditionalInfo[machineName]);
            }

            DateTime minimumTime = DateTime.UtcNow.AddMinutes(-1);
            DateTime loggedTime = DateTime.Parse(formatter.AdditionalInfo[timeStamp]);
            if (DateTime.Compare(minimumTime, loggedTime) > 0)
            {
                Assert.Fail(loggedTimeStampFailMessage);
            }

            Assert.AreEqual(AppDomain.CurrentDomain.FriendlyName, formatter.AdditionalInfo[appDomainName]);
            Assert.AreEqual(Thread.CurrentPrincipal.Identity.Name, formatter.AdditionalInfo[threadIdentity]);

            if (string.Compare(permissionDenied, formatter.AdditionalInfo[windowsIdentity]) != 0)
            {
                Assert.AreEqual(WindowsIdentity.GetCurrent().Name, formatter.AdditionalInfo[windowsIdentity]);
            }
        }

        public class FileNotFoundExceptionWithIndexer : FileNotFoundException
        {
            public FileNotFoundExceptionWithIndexer(string message,
                                                    string fileName)
                : base(message, fileName) { }

            public string this[int index]
            {
                get { return null; }
            }
        }
    }
}
