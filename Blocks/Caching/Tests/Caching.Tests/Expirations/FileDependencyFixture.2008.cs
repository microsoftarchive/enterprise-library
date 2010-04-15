//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security;
using System.Security.Permissions;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Expirations.Tests
{
    public partial class FileDependencyFixture
    {
        [TestMethod]
        [ExpectedException(typeof(SecurityException))]
        public void NoPermissionToReadFileWhenCreated()
        {
            FileIOPermission denyPermission =
                new FileIOPermission(FileIOPermissionAccess.Read, Path.GetFullPath("TestFile"));

            PermissionSet permissions = new PermissionSet(PermissionState.None);
            permissions.AddPermission(denyPermission);
            permissions.Deny();

            try
            {
                new FileDependency("TestFile");
            }
            finally
            {
                CodeAccessPermission.RevertDeny();
            }
        }

        [TestMethod]
        [ExpectedException(typeof(SecurityException))]
        public void NoPermissionToReadWhenCheckingExpiration()
        {
            FileDependency dependency = new FileDependency("TestFile");

            FileIOPermission denyPermission =
                new FileIOPermission(FileIOPermissionAccess.Read, Path.GetFullPath("TestFile"));

            PermissionSet permissions = new PermissionSet(PermissionState.None);
            permissions.AddPermission(denyPermission);
            permissions.Deny();

            try
            {
                dependency.HasExpired();
            }
            finally
            {
                CodeAccessPermission.RevertDeny();
            }
        }
    }
}
