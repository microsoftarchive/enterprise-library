//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Security.Cryptography;
using System.Security.Permissions;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design
{
    /// <summary>
    /// <para>Creates keys for a <see cref="KeyedHashAlgorithm"/>.</para>
    /// </summary>
    public class KeyedHashAlgorithmKeyCreator : IKeyCreator
    {
        private KeyedHashAlgorithm algorithm;
        private Type algorithmType;


        /// <summary>
        /// Initialize a new instance of the <see cref="KeyedHashAlgorithmKeyCreator"/> class.
        /// </summary>
        /// <param name="algorithmType">The <see cref="HashAlgorithm"/> type that should be used.</param>
        public KeyedHashAlgorithmKeyCreator(Type algorithmType)
        {
            if (algorithmType == null) 
            {
                throw new ArgumentNullException("algorithmType");
            }

            if (!typeof(HashAlgorithm).IsAssignableFrom(algorithmType))
            {
                throw new ArgumentException(KeyManagerResources.TypeShouldDeriveFromHashAlgorithm, "algorithmType");
            }
            
            this.algorithm = CreateAlgorithm(algorithmType);
            this.algorithmType = algorithmType;
        }

        /// <summary>
        /// <para>Gets the length of the key.</para>
        /// </summary>
        /// <value>The length of the key.</value>
        public int KeyLength
        {
            get { return algorithm.Key.Length; }
        }

        /// <summary>
        /// <para>Generates a random key.</para>
        /// </summary>
        /// <returns><para>A random key.</para></returns>
        public byte[] GenerateKey()
        {
            return KeyManager.GenerateKeyedHashKey(algorithmType, DataProtectionScope.CurrentUser).Unprotect();
        }

        /// <summary>
        /// <para>Determines if the <paramref name="key"/> is valid.</para>
        /// </summary>
        /// <param name="key">The key to test.</param>
        /// <returns><para><see langword="true"/> if the key is valid; otherwise <see langword="false"/>.</para></returns>
        public bool KeyIsValid(byte[] key)
        {
            return (key != null) && (key.Length != 0);
        }

        [ReflectionPermission(SecurityAction.Demand)]
        private static KeyedHashAlgorithm CreateAlgorithm(Type algorithmType)
        {
            return (KeyedHashAlgorithm)Activator.CreateInstance(algorithmType);
        }
    }
}
