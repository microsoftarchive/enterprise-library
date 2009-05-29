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

using System.Security.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Instrumentation;
using System;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography
{
    /// <summary>
    /// <para>A symmetric provider for the Data Protection API (DPAPI).</para>
    /// </summary>
	[ConfigurationElementType(typeof(DpapiSymmetricCryptoProviderData))]	
	public class DpapiSymmetricCryptoProvider : ISymmetricCryptoProvider
    {		
		private DataProtectionScope protectionScope;
		private byte[] entropy;
        private ISymmetricAlgorithmInstrumentationProvider instrumentationProvider;

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="DpapiSymmetricCryptoProvider"/></para>
        /// </summary>
        /// <param name="scope"><para>One of the <see cref="DataProtectionScope"/> values.</para></param>
        /// <param name="entropy"><para>The entropy to salt the phrase.</para></param>
        public DpapiSymmetricCryptoProvider(DataProtectionScope scope, byte[] entropy)
            :this(scope, entropy, new NullSymmetricAlgorithmInstrumentationProvider())
        {
        }

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="DpapiSymmetricCryptoProvider"/></para>
        /// </summary>
        /// <param name="scope"><para>One of the <see cref="DataProtectionScope"/> values.</para></param>
        public DpapiSymmetricCryptoProvider(DataProtectionScope scope)
            : this(scope, null, new NullSymmetricAlgorithmInstrumentationProvider())
        {
        }

		/// <summary>
		/// <para>Initialize a new instance of the <see cref="DpapiSymmetricCryptoProvider"/></para>
		/// </summary>
		/// <param name="scope"><para>One of the <see cref="DataProtectionScope"/> values.</para></param>
		/// <param name="entropy"><para>The entropy to salt the phrase.</para></param>
        /// <param name="instrumentationProvider">Instrumentation provider to use.</param>
		public DpapiSymmetricCryptoProvider(DataProtectionScope scope, byte[] entropy, ISymmetricAlgorithmInstrumentationProvider instrumentationProvider)
		{
            if (instrumentationProvider == null) throw new ArgumentNullException("instrumentationProvider");

			this.protectionScope = scope;
			this.entropy = entropy;
            this.instrumentationProvider = instrumentationProvider;
		}

		/// <summary>
		/// <para>Initialize a new instance of the <see cref="DpapiSymmetricCryptoProvider"/></para>
		/// </summary>
		/// <param name="scope"><para>One of the <see cref="DataProtectionScope"/> values.</para></param>
        /// <param name="instrumentationProvider">Instrumentation provider to use.</param>
        public DpapiSymmetricCryptoProvider(DataProtectionScope scope, ISymmetricAlgorithmInstrumentationProvider instrumentationProvider)
            : this(scope, null, instrumentationProvider)
		{
		}

		private DpapiCryptographer DpapiCrytographer
        {
			get 
			{
                return new DpapiCryptographer(protectionScope);            
			}
        }

        /// <summary>
        /// <para>Encrypts a secret using DPAPI.</para>
        /// </summary>
        /// <param name="plaintext"><para>The input for which you want to encrypt.</para></param>
        /// <returns><para>The resulting cipher text.</para></returns>
        /// <seealso cref="ISymmetricCryptoProvider.Encrypt"/>
		public byte[] Encrypt(byte[] plaintext)
        {
            try
            {
                byte[] result = DpapiCrytographer.Encrypt(plaintext, entropy);
                instrumentationProvider.FireSymmetricEncryptionPerformed();
                return result;
            }
            catch (Exception e)
            {
                instrumentationProvider.FireCyptographicOperationFailed(Resources.EncryptionFailed, e);
                throw;
            }
        }

        /// <summary>
        /// <para>Decrypts cipher text using DPAPI.</para>
        /// </summary>
        /// <param name="ciphertext"><para>The cipher text for which you want to decrypt.</para></param>
        /// <returns><para>The resulting plain text.</para></returns>
        /// <seealso cref="ISymmetricCryptoProvider.Decrypt"/>
		public byte[] Decrypt(byte[] ciphertext)
        {
            try
            {
                byte[] result = DpapiCrytographer.Decrypt(ciphertext, entropy);
                instrumentationProvider.FireSymmetricDecryptionPerformed();
                return result;
            }
            catch (Exception e)
            {
                instrumentationProvider.FireCyptographicOperationFailed(Resources.DecryptionFailed, e);
                throw;
            }
        }
	}
}
