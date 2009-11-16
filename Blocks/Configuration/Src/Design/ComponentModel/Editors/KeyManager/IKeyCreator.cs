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

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design
{
    /// <summary>
    /// <para>Represents the creator of keys for security algorithms.</para>
    /// </summary>
    public interface IKeyCreator
    {
        /// <summary>
        /// <para>When implemented by a class, gets the length of the key.</para>
        /// </summary>
        /// <value>The length of the key.</value>
        int KeyLength { get; }

        /// <summary>
        /// <para>When implemented by a class, generates a random key.</para>
        /// </summary>
        /// <returns><para>A random key.</para></returns>
        byte[] GenerateKey();

        /// <summary>
        /// <para>When implemented by a class, determines if the <paramref name="key"/> is valid.</para>
        /// </summary>
        /// <param name="key">The key to test.</param>
        /// <returns><para><see langword="true"/> if the key is valid; otherwise <see langword="false"/>.</para></returns>
        bool KeyIsValid(byte[] key);
    }
}
