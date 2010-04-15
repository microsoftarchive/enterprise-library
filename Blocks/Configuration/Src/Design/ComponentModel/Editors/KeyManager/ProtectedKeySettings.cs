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
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design
{
    /// <summary>
    /// <para>Represents the settings to store and open a <see cref="ProtectedKey"/>.</para>
    /// </summary>
    public class ProtectedKeySettings
    {
        private ProtectedKey protectedKey;
        private string fileName;
        private DataProtectionScope scope;

        /// <summary>
        /// Initialize a new instance of the <see cref="ProtectedKeySettings"/> class
        /// </summary>
        public ProtectedKeySettings()
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="ProtectedKeySettings"/> class, given a <paramref name="fileName"/> and <see cref="DataProtectionScope"/>.
        /// </summary>
        /// <param name="fileName">The file path to the protected keyfile.</param>
        /// <param name="scope">The <see cref="DataProtectionScope"/> used to protect the keyfile.</param>
        public ProtectedKeySettings(string fileName, DataProtectionScope scope)
        {
            this.fileName = fileName;
            this.scope = scope;
        }

        /// <summary>
        /// Gets or sets the file path to the protected keyfile.
        /// </summary>
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        /// <summary>
        /// Gets or sets the <see cref="DataProtectionScope"/> used to protect the keyfile.
        /// </summary>
        public DataProtectionScope Scope
        {
            get { return scope; }
            set { scope = value; }
        }

        /// <summary>
        /// Gets or sets the actual <see cref="ProtectedKey"/> represented by this class.
        /// </summary>
        public ProtectedKey ProtectedKey
        {
            get { return protectedKey; }
            set { protectedKey = value; }
        }

        /// <summary>
        /// Returns a string representation of this class.
        /// </summary>
        /// <returns>A string representation of this class.</returns>
        public override string ToString()
        {
            return KeyManagerResources.ProtectedKeySettingsToString;
        }
    }
}
