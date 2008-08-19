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
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography
{
    /// <summary>
    /// Allows caller to a read a cryptographic key from a DPAPI-protected key file
	/// or from a password-protected key archive file.
    /// </summary>
    public interface IKeyReader
    {

        /// <summary>
        /// Reads a DPAPI protected key from the given <see cref="Stream"></see>.
        /// </summary>
		/// <param name="protectedKeyContents"><see cref="Stream"></see> containing the key to be read.</param>
		/// <param name="protectionScope"><see cref="DataProtectionScope"></see> used to encrypt the generated key.</param>
		/// <returns>Key read from stream, encapsulated in a <see cref="ProtectedKey"></see>.</returns>
		ProtectedKey Read(Stream protectedKeyContents, DataProtectionScope protectionScope);

		/// <summary>
		/// Restores a key from an encrypted key archive <see cref="Stream"></see>.
		/// </summary>
		/// <param name="protectedKeyContents"><see cref="Stream"></see> containing the key to be read.</param>
		/// <param name="passphrase">User-provided passphrase used to encrypt the key in the archive for safekeeping. </param>
		/// <param name="protectionScope"><see cref="DataProtectionScope"></see> used to encrypt the generated key.</param>
		/// <returns>Key read from stream, encapsulated in a <see cref="ProtectedKey"></see>.</returns>
		ProtectedKey Restore(Stream protectedKeyContents, string passphrase, DataProtectionScope protectionScope);

		/// <summary>
		/// Restores a key from an encrypted key archive <see cref="Stream"></see>.
		/// </summary>
		/// <param name="protectedKeyContents"><see cref="Stream"></see> containing the key to be read.</param>
		/// <param name="passphrase">User-provided passphrase used to encrypt the key in the archive for safekeeping. </param>
		/// <returns>Unencrypted key read from stream.
		/// The caller of this method is responsible for clearing the contents of this byte array.</returns>
		byte[] Restore(Stream protectedKeyContents, string passphrase);
	}
}
