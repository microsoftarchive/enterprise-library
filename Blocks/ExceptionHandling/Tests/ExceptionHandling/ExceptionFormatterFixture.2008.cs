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
using System.Text;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests
{
    public partial class ExceptionFormatterFixture
    {
        [TestMethod]
        public void ReflectionFormatterReadSecurityExceptionPropertiesWithoutPermissionTest()
        {
            var evidence = new Evidence();
            evidence.AddHostEvidence(new Zone(SecurityZone.Internet));
            var set = SecurityManager.GetStandardSandbox(evidence);
            set.AddPermission(new ReflectionPermission(ReflectionPermissionFlag.MemberAccess));

            var domain = AppDomain.CreateDomain("partial trust", null, AppDomain.CurrentDomain.SetupInformation, set);

            try
            {
                var instance = ((Tester)domain.CreateInstanceAndUnwrap(typeof(Tester).Assembly.FullName, typeof(Tester).FullName));
                var exception = instance.DoTest();

                StringBuilder sb = new StringBuilder();
                StringWriter writer = new StringWriter(sb);

                var formatter = new MockTextExceptionFormatter(writer, exception, Guid.Empty);
                formatter.Format();
                Assert.AreEqual(exception.Demanded.ToString(), formatter.properties["Demanded"]);
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

        public class Tester : MarshalByRefObject
        {
            public SecurityException DoTest()
            {
                SecurityException exception = null;

                try
                {
                    var _ = Thread.CurrentPrincipal.Identity.Name;
                }
                catch (SecurityException e)
                {
                    exception = e;
                }

                StringBuilder sb = new StringBuilder();
                StringWriter writer = new StringWriter(sb);

                MockTextExceptionFormatter formatter = new MockTextExceptionFormatter(writer, exception, Guid.Empty);
                formatter.Format();

                formatter = new MockTextExceptionFormatter(writer, exception, Guid.Empty);
                formatter.Format();

                return exception;
            }
        }
    }
}
