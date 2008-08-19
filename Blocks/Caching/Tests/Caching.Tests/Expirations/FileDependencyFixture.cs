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
    [TestClass]
    public class FileDependencyFixture
    {
        [TestInitialize]
        public void InitializeTestFile()
        {
            if (File.Exists("TestFile"))
            {
                File.Delete("TestFile");
            }
            FileStream newFile = File.Create("TestFile");
            newFile.Close();
        }

        [TestCleanup]
        public void RemoveTestFile()
        {
            if (File.Exists("TestFile"))
            {
                File.Delete("TestFile");
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructingWithANullFileThrowsException()
        {
            FileDependency fileDependency = new FileDependency(null);
        }

        [TestMethod]
        public void ExpiresReturnsTrueIfFileDisappears()
        {
            FileDependency dependency = new FileDependency("TestFile");

            File.Delete("TestFile");

            Assert.IsTrue(dependency.HasExpired(), "Deleted files should always be considered expired");
        }

        [TestMethod]
        public void ExpiresIfTouched()
        {
            FileDependency dependency = new FileDependency("TestFile");
            Thread.Sleep(1500);
            using (FileStream outputStream = File.OpenWrite("TestFile"))
            {
                outputStream.WriteByte(0x00);
            }

            Assert.IsTrue(dependency.HasExpired(), "File was touched, so it should be considered expired");
        }

        [TestMethod]
        public void ClassCanSerializeCorrectly()
        {
            FileDependency dependency = new FileDependency("TestFile");

            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            formatter.Serialize(stream, dependency);
            stream.Position = 0;
            FileDependency dependency2 = (FileDependency)formatter.Deserialize(stream);

            Assert.AreEqual(dependency.FileName, dependency2.FileName);
            Assert.AreEqual(dependency.LastModifiedTime, dependency2.LastModifiedTime);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SecurityPermissionsDoNotCauseExceptionIfFileNotPresent()
        {
            new FileDependency("shouldNeverExist");
        }

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