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

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Tests
{
    public class SymmetricProviderHelper
    {
        ISymmetricCryptoProvider symmProvider;

        public SymmetricProviderHelper(ISymmetricCryptoProvider symmProvider)
        {
            this.symmProvider = symmProvider;
        }

        public ISymmetricCryptoProvider DefaultSymmProvider
        {
            get { return symmProvider; }
        }

        public void DecryptNull()
        {
            DefaultSymmProvider.Decrypt(null);
        }

        public void DecryptZeroLength()
        {
            DefaultSymmProvider.Decrypt(new byte[] { });
        }

        public void EncryptAndDecrypt()
        {
            byte[] plainText = new byte[50];
            EncryptAndDecryptTest(plainText);
        }

        public void EncryptAndDecryptOneByte()
        {
            byte[] plainText = new byte[1];
            EncryptAndDecryptTest(plainText);
        }

        public void EncryptAndDecryptOneKilobyte()
        {
            byte[] plainText = new byte[1024];
            EncryptAndDecryptTest(plainText);
        }

        public void EncryptAndDecryptOneMegabyte()
        {
            byte[] plainText = new byte[1024 * 1024];
            EncryptAndDecryptTest(plainText);
        }

        public void EncryptAndDecryptTest(byte[] plainText)
        {
            plainText = CryptographyUtility.GetRandomBytes(plainText.Length);

            byte[] cipherText = DefaultSymmProvider.Encrypt(plainText);
            Assert.IsFalse(CryptographyUtility.CompareBytes(cipherText, plainText), "encrypted");

            byte[] decryptedText = DefaultSymmProvider.Decrypt(cipherText);
            Assert.IsTrue(CryptographyUtility.CompareBytes(plainText, decryptedText), "decrypted");
        }

        public void EncryptNull()
        {
            DefaultSymmProvider.Encrypt(null);
        }

        public void EncryptZeroLength()
        {
            DefaultSymmProvider.Encrypt(new byte[] { });
        }
    }
}
