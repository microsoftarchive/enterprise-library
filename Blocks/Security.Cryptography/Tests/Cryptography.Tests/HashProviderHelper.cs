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
using System.Collections.Generic;
using System.Security.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Tests
{
    public class HashProviderHelper
    {
        IHashProvider defaultHashProvider;
        MockHashInstrumentationListener defaultHashProviderListener;
        readonly byte[] plainText = new byte[] { 0, 1, 2, 3, 4, 5, 6 };
        IHashProvider saltedHashProvider;
        MockHashInstrumentationListener saltedHashProviderListener;

        public HashProviderHelper(IHashProvider defaultHashProvider,
                                  IHashProvider saltedHashProvider)
        {
            this.defaultHashProvider = defaultHashProvider;
            BindNewInstrumentationListener(this.defaultHashProvider, out defaultHashProviderListener);

            this.saltedHashProvider = saltedHashProvider;
            BindNewInstrumentationListener(this.saltedHashProvider, out saltedHashProviderListener);
        }

        public IHashProvider DefaultHashProvider
        {
            get { return defaultHashProvider; }
        }

        public IHashProvider SaltedHashProvider
        {
            get { return saltedHashProvider; }
        }

        void BindNewInstrumentationListener(IHashProvider hashProvider,
                                            out MockHashInstrumentationListener hashInstrumentationListener)
        {
            if (hashProvider is HashAlgorithmProvider)
            {
                hashInstrumentationListener = new MockHashInstrumentationListener();
                new ReflectionInstrumentationBinder().Bind((hashProvider as HashAlgorithmProvider).GetInstrumentationEventProvider(), hashInstrumentationListener);
            }
            else
            {
                hashInstrumentationListener = null;
            }
        }

        void CheckEvents(MockHashInstrumentationListener listener,
                         int failed,
                         int created,
                         int compared,
                         int mismatched)
        {
            if (listener == null)
                return;

            Assert.AreEqual(failed, listener.GetNotificationTally("CyptographicOperationFailed"), "CyptographicOperationFailed");
            Assert.AreEqual(created, listener.GetNotificationTally("HashOperationPerformed"), "HashOperationPerformed");
            Assert.AreEqual(compared, listener.GetNotificationTally("HashComparisonPerformed"), "HashComparisonPerformed");
            Assert.AreEqual(mismatched, listener.GetNotificationTally("HashMismatchDetected"), "HashMismatchDetected");
        }

        public void CompareEqualHash()
        {
            byte[] hashedText = DefaultHashProvider.CreateHash(plainText);

            Assert.IsTrue(DefaultHashProvider.CompareHash(plainText, hashedText));
            CheckEvents(defaultHashProviderListener, 0, 1, 1, 0);
        }

        public void CompareHashInvalidHashedText()
        {
            byte[] hashedText = new byte[] { 0, 1, 2, 3 };

            Assert.IsFalse(DefaultHashProvider.CompareHash(plainText, hashedText));
        }

        public void CompareHashNullHashedText()
        {
            try
            {
                DefaultHashProvider.CompareHash(plainText, null);
            }
            catch
            {
                CheckEvents(defaultHashProviderListener, 0, 0, 0, 0);
                throw;
            }
        }

        public void CompareHashNullPlainText()
        {
            byte[] hashedText = DefaultHashProvider.CreateHash(new byte[] { 1 });

            try
            {
                DefaultHashProvider.CompareHash(null, hashedText);
            }
            catch
            {
                CheckEvents(defaultHashProviderListener, 0, 1, 0, 0);
                throw;
            }
        }

        public void CompareHashOfDifferentText()
        {
            byte[] plainText1 = new byte[] { 0, 1, 0, 0 };
            byte[] plainText2 = new byte[] { 0, 0, 1, 0 };
            byte[] hashedText = DefaultHashProvider.CreateHash(plainText2);

            Assert.IsFalse(DefaultHashProvider.CompareHash(plainText1, hashedText));
            CheckEvents(defaultHashProviderListener, 0, 1, 1, 1);
        }

        public void CompareHashWithSalt()
        {
            IHashProvider hashProvider = SaltedHashProvider;

            byte[] providerHash = hashProvider.CreateHash(plainText);
            Assert.IsTrue(hashProvider.CompareHash(plainText, providerHash), "true");

            byte[] badHash = new byte[50];
            RNGCryptoServiceProvider.Create().GetBytes(badHash);
            Assert.IsFalse(hashProvider.CompareHash(plainText, badHash), "false");
        }

        public void CompareHashZeroLengthHashedText()
        {
            byte[] hashedText = new byte[] { };

            try
            {
                DefaultHashProvider.CompareHash(plainText, hashedText);
            }
            catch
            {
                CheckEvents(defaultHashProviderListener, 0, 0, 0, 0);
                throw;
            }
        }

        public void CompareHashZeroLengthPlainText()
        {
            byte[] plainTextZero = new byte[] { };
            byte[] hashedText = DefaultHashProvider.CreateHash(plainTextZero);

            Assert.IsTrue(DefaultHashProvider.CompareHash(plainTextZero, hashedText));
        }

        public void CreateHash()
        {
            byte[] providerHash = DefaultHashProvider.CreateHash(plainText);
            Assert.IsFalse(CryptographyUtility.CompareBytes(plainText, providerHash));
            CheckEvents(defaultHashProviderListener, 0, 1, 0, 0);
        }

        public void CreateHashNullPlainText()
        {
            try
            {
                DefaultHashProvider.CreateHash(null);
            }
            catch
            {
                CheckEvents(defaultHashProviderListener, 1, 0, 0, 0);
                throw;
            }
        }

        public void CreateHashZeroLengthPlainText()
        {
            byte[] hashForEmptyPlainText = DefaultHashProvider.CreateHash(new byte[] { });
            Assert.IsTrue(hashForEmptyPlainText.Length > 0);
        }

        public void HashWithSalt()
        {
            IHashProvider hashProviderWithSalt = SaltedHashProvider;
            IHashProvider hashProvider = DefaultHashProvider;

            byte[] origHash1 = hashProvider.CreateHash(plainText);
            byte[] providerHash1 = hashProviderWithSalt.CreateHash(plainText);

            Assert.IsFalse(CryptographyUtility.CompareBytes(origHash1, providerHash1), "original");
            Assert.IsFalse(CryptographyUtility.CompareBytes(plainText, providerHash1), "plain");
        }

        public void ThrowExceptionWhenByteArrayIsNull()
        {
            defaultHashProvider.CreateHash(null);
        }

        public void UniqueSaltedHashes()
        {
            IHashProvider hashProviderWithSalt = SaltedHashProvider;
            byte[] providerHash1 = hashProviderWithSalt.CreateHash(plainText);
            byte[] providerHash2 = hashProviderWithSalt.CreateHash(plainText);
            Assert.IsFalse(CryptographyUtility.CompareBytes(providerHash1, providerHash2), "compare");
        }

        public void VerifyHashAsUnique()
        {
            byte[] hash1 = SaltedHashProvider.CreateHash(plainText);
            byte[] hash2 = SaltedHashProvider.CreateHash(plainText);

            Assert.IsFalse(CryptographyUtility.CompareBytes(hash1, hash2));
        }
    }

    public class MockHashInstrumentationListener
    {
        IDictionary<string, int> notifications = new Dictionary<string, int>();

        [InstrumentationConsumer("CyptographicOperationFailed")]
        public void CyptographicOperationFailed(object sender,
                                                EventArgs e)
        {
            IncreaseNotificationTally("CyptographicOperationFailed");
        }

        internal int GetNotificationTally(string key)
        {
            return notifications.ContainsKey(key) ? notifications[key] : 0;
        }

        [InstrumentationConsumer("HashComparisonPerformed")]
        public void HashComparisonPerformed(object sender,
                                            EventArgs e)
        {
            IncreaseNotificationTally("HashComparisonPerformed");
        }

        [InstrumentationConsumer("HashMismatchDetected")]
        public void HashMismatchDetected(object sender,
                                         EventArgs e)
        {
            IncreaseNotificationTally("HashMismatchDetected");
        }

        [InstrumentationConsumer("HashOperationPerformed")]
        public void HashOperationPerformed(object sender,
                                           EventArgs e)
        {
            IncreaseNotificationTally("HashOperationPerformed");
        }

        void IncreaseNotificationTally(string key)
        {
            if (notifications.ContainsKey(key))
            {
                notifications[key] = notifications[key] + 1;
            }
            else
            {
                notifications[key] = 1;
            }
        }
    }
}
