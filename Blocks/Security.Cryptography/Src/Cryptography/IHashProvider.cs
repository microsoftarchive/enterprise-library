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

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography
{
    /// <summary>
    /// A contract for any provider for configurable hash implementations.
    /// </summary>
	[ConfigurationNameMapper(typeof(HashProviderDataRetriever))]
	[CustomFactory(typeof(HashProviderCustomFactory))]
	public interface IHashProvider
    {
        /// <summary>
        /// Computes the hash value of plain text.
        /// </summary>
        /// <param name="plaintext">The input for which to compute the hash.</param>
        /// <returns>The computed hash code.</returns>
        byte[] CreateHash(byte[] plaintext);

        /// <summary>
        /// Compares plain text input with a computed hash.
        /// </summary>
        /// <param name="plaintext">The input for which you want to compare the hash to.</param>
        /// <param name="hashedtext">The hash value for which you want to compare the input to.</param>
        /// <returns><c>true</c> if plainText hashed is equal to the hashedText. Otherwise, <c>false</c>.</returns>
        bool CompareHash(byte[] plaintext, byte[] hashedtext);
    }
}