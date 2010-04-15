//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Diagnostics;
using System.IO;
using System.Security.Permissions;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners
{
    public partial class RollingFlatFileTraceListenerFixture
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RollingFlatFileTraceListenerReplacedEnviromentVariablesWillFallBackIfNotPrivilegesToRead()
        {
            string environmentVariable = "%USERPROFILE%";
            string fileName = Path.Combine(environmentVariable, "foo.log");

            EnvironmentPermission denyPermission = new EnvironmentPermission(PermissionState.Unrestricted);
            denyPermission.Deny();

            try
            {
                RollingFlatFileTraceListener listener = new RollingFlatFileTraceListener(fileName, "header", "footer", null, 1, "", RollFileExistsBehavior.Increment, RollInterval.Day);
                listener.TraceData(new TraceEventCache(), "source", TraceEventType.Error, 1, "This is a test");
                listener.Dispose();
            }
            finally
            {
                EnvironmentPermission.RevertAll();
            }

            Assert.Fail("Permission was not denied.");
        }
    }
}
