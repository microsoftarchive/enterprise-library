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

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography
{
	/// <summary>
	/// Allows caller to a write a cryptographic key to a DPAPI-protected key file
	/// or to a password-protected key archive file.
	/// </summary>
	public interface IKeyWriter
	{
		/// <summary>
		/// Writes a cryptographic key to a DPAPI-protected <see cref="Stream"/>.
		/// </summary>
		/// <param name="outputStream"><see cref="Stream"/> to which key is to be written.</param>
		/// <param name="key">Key to be written.</param>
		void Write(Stream outputStream, ProtectedKey key);

		/// <summary>
		/// Creates an archived version of the given <paramref name="key"/>	written to the <paramref name="outputStream"/>.
		/// </summary>
		/// <param name="outputStream"><see cref="Stream"/> to which key is to be written.</param>
		/// <param name="key">Key to be written.</param>
		/// <param name="passphrase">User-provided passphrase used to encrypt the archive in the <see cref="Stream"/>.</param>
		void Archive(Stream outputStream, ProtectedKey key, string passphrase);
	}
}
