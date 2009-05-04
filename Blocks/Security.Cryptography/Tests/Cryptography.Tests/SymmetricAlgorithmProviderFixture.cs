//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Cryptography Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.IO;
using System.Security.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Instrumentation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Tests
{
    [TestClass]
    public class SymmetricAlgorithmProviderFixture
    {
        byte[] plaintext = new byte[] { 0x01, 0x02, 0x03, 0x04 };
        MemoryStream stream;
        SymmetricAlgorithmProvider provider;
        EnterpriseLibraryPerformanceCounter totalSymmetricEncryptionPerformedCounter;
        EnterpriseLibraryPerformanceCounter totalSymmetricDecryptionPerformedCounter;
        AppDomainNameFormatter nameFormatter;
        SymmetricAlgorithmInstrumentationListener listener;

        const string applicationInstanceName = "applicationInstanceName";
        const string instanceName = "Foo";
        string formattedInstanceName;

        SymmetricProviderHelper SymmetricProviderHelper
        {
            get { return new SymmetricProviderHelper(provider); }
        }

        [TestInitialize]
        public void SetUp()
        {
            nameFormatter = new AppDomainNameFormatter(applicationInstanceName);
            listener = new SymmetricAlgorithmInstrumentationListener(instanceName, true, true, true, nameFormatter);
            formattedInstanceName = nameFormatter.CreateName(instanceName);
            totalSymmetricDecryptionPerformedCounter = new EnterpriseLibraryPerformanceCounter(SymmetricAlgorithmInstrumentationListener.counterCategoryName, SymmetricAlgorithmInstrumentationListener.TotalSymmetricDecryptionPerformedCounterName, formattedInstanceName);
            totalSymmetricEncryptionPerformedCounter = new EnterpriseLibraryPerformanceCounter(SymmetricAlgorithmInstrumentationListener.counterCategoryName, SymmetricAlgorithmInstrumentationListener.TotalSymmetricEncryptionPerformedCounterName, formattedInstanceName);

            stream = CreateSymmetricKey();
            provider = new SymmetricAlgorithmProvider(typeof(RijndaelManaged), stream, DataProtectionScope.CurrentUser);
        }

        [TestMethod]
        public void TotalSymmetricDecryptionPerformedCounterIncremented()
        {
            listener.SymmetricDecryptionPerformed(this, EventArgs.Empty);

            long expected = 1;
            Assert.AreEqual(expected, totalSymmetricDecryptionPerformedCounter.Value);
        }

        [TestMethod]
        public void TotalSymmetricEncryptionPerformedCounterIncremented()
        {
            listener.SymmetricEncryptionPerformed(this, EventArgs.Empty);

            long expected = 1;
            Assert.AreEqual(expected, totalSymmetricEncryptionPerformedCounter.Value);
        }

        [TestCleanup]
        public void TearDown()
        {
            stream.Dispose();
        }

        [TestMethod]
        public void CanEncryptAndDecryptMessage()
        {
            byte[] encryptedData = provider.Encrypt(plaintext);
            byte[] decryptedData = provider.Decrypt(encryptedData);

            AssertHelpers.AssertArraysEqual(plaintext, decryptedData);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullAlgorithmTypeThrows()
        {
            new SymmetricAlgorithmProvider(null, ProtectedKey.CreateFromPlaintextKey(new byte[] { 0x00, 0x01 }, DataProtectionScope.CurrentUser));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullKeyFileNameThrowsException()
        {
            new SymmetricAlgorithmProvider(typeof(RijndaelManaged), (string)null, DataProtectionScope.CurrentUser);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructWithEmptyKeyFileNameThrowsException()
        {
            new SymmetricAlgorithmProvider(typeof(RijndaelManaged), String.Empty, DataProtectionScope.CurrentUser);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void ConstructWithBadFileNameThrowsException()
        {
            new SymmetricAlgorithmProvider(typeof(RijndaelManaged), "BadFileName", DataProtectionScope.CurrentUser);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructWithNonSymmetricAlgorithmTypeThrows()
        {
            new SymmetricAlgorithmProvider(typeof(object), ProtectedKey.CreateFromPlaintextKey(new byte[] { 0x00, 0x01 }, DataProtectionScope.CurrentUser));
        }

        [TestMethod]
        [ExpectedException(typeof(CryptographicException))]
        public void DecryptBadTextThrows()
        {
            SymmetricProviderHelper.DefaultSymmProvider.Decrypt(new byte[] { 0, 1, 2, 3, 4 });
        }

        [TestMethod]
        public void EncryptAndDecrypt()
        {
            SymmetricProviderHelper.EncryptAndDecrypt();
            ;
        }

        [TestMethod]
        public void EncryptAndDecryptOneByte()
        {
            SymmetricProviderHelper.EncryptAndDecryptOneByte();
        }

        [TestMethod]
        public void EncryptAndDecryptOneKilobyte()
        {
            SymmetricProviderHelper.EncryptAndDecryptOneKilobyte();
        }

        [TestMethod]
        public void EncryptAndDecryptOneMegabyte()
        {
            SymmetricProviderHelper.EncryptAndDecryptOneMegabyte();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EncryptNullThrows()
        {
            SymmetricProviderHelper.EncryptNull();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void EncryptZeroLengthThrows()
        {
            SymmetricProviderHelper.EncryptZeroLength();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DecryptNullThrows()
        {
            SymmetricProviderHelper.DecryptNull();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DecryptZeroLengthThrows()
        {
            SymmetricProviderHelper.DecryptZeroLength();
        }

        [TestMethod]
        public void InstrumentationUsesExplicitBinder()
        {
            byte[] key = Guid.NewGuid().ToByteArray();

            InstrumentationAttacherFactory attacherFactory = new InstrumentationAttacherFactory();
            IInstrumentationAttacher binder = attacherFactory.CreateBinder(provider.GetInstrumentationEventProvider(), new object[] { "foo", true, true, true, "fooApplicationInstanceName" }, new ConfigurationReflectionCache());
            binder.BindInstrumentation();

            Assert.AreSame(typeof(ExplicitInstrumentationAttacher), binder.GetType());
        }

        [TestMethod]
        [ExpectedException(typeof(CryptographicException))]
        public void CryptoFailureThrowsWithInstrumentationEnabled()
        {
            byte[] key = Guid.NewGuid().ToByteArray();

            ReflectionInstrumentationBinder binder = new ReflectionInstrumentationBinder();
            binder.Bind(provider.GetInstrumentationEventProvider(), new SymmetricAlgorithmInstrumentationListener("foo", true, true, true, "fooApplicationInstanceName"));

            provider.Decrypt(new byte[] { 0, 1, 2, 3, 4 });
        }

        static MemoryStream CreateSymmetricKey()
        {
            MemoryStream stream = new MemoryStream();

            RijndaelManaged algo = (RijndaelManaged)RijndaelManaged.Create();
            algo.GenerateKey();
            KeyManager.Write(stream, ProtectedKey.CreateFromPlaintextKey(algo.Key, DataProtectionScope.CurrentUser));
            stream.Seek(0, SeekOrigin.Begin);

            return stream;
        }
    }
}
