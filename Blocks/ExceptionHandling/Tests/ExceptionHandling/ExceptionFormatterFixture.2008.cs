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
using System.Security.Principal;
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
            SecurityPermission denyPermission
                = new SecurityPermission(SecurityPermissionFlag.ControlPolicy | SecurityPermissionFlag.ControlEvidence);
            PermissionSet permissions = new PermissionSet(PermissionState.None);
            permissions.AddPermission(denyPermission);
            permissions.Deny();

            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);

            SecurityException exception = null;
            try
            {
                DemandException(denyPermission);
            }
            catch (SecurityException e)
            {
                exception = e;
            }

            MockTextExceptionFormatter formatter = new MockTextExceptionFormatter(writer, exception, Guid.Empty);
            formatter.Format();

            CodeAccessPermission.RevertDeny();

            formatter = new MockTextExceptionFormatter(writer, exception, Guid.Empty);
            formatter.Format();
            Assert.AreEqual(exception.Demanded.ToString(), formatter.properties["Demanded"]);
        }

        static void DemandException(SecurityPermission denyPermission)
        {
            denyPermission.Demand();
        }
    }
}
