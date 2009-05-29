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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.Unity;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Unity
{
    /// <summary>
    /// A <see cref="UnityContainerExtension"/> that registers the policies necessary
    /// to create <see cref="IHashProvider"/> and <see cref="ISymmetricCryptoProvider"/> instances described in the standard
    /// configuration file.
    /// </summary>
    /// <remarks>This function is now performed by the <see cref="EnterpriseLibraryCoreExtension"/>.
    /// This extension is no longer necessary.</remarks>
    [Obsolete]
    public class CryptographyBlockExtension : EnterpriseLibraryBlockExtension
    {
    }
}
