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

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design
{
    /// <summary>
    /// Represents a configuration node that contains cryptographic key infromation.
    /// </summary>
    public interface ICryptographicKeyConfigurationNode
    {
        /// <summary>
        /// Gets the cryptographic key infromation.
        /// </summary>
        ProtectedKeySettings KeySettings { get; }

        /// <summary>
        /// Gets the <see cref="IKeyCreator"/> that should be used to generate and validate the cryptographic key information.
        /// </summary>
        IKeyCreator KeyCreator { get; }
    }
}
