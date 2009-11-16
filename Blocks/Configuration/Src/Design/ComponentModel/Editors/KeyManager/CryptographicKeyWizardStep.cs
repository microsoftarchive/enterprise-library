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

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design
{
    /// <summary>
    /// Enumeration to represent the steps within the <see cref="CryptographicKeyWizard"/>.
    /// </summary>
    public enum CryptographicKeyWizardStep
    {
        /// <summary>
        /// Wizard step in which the user decides how to provide a key.
        /// </summary>
        SupplyKey,

        /// <summary>
        /// Wizard step in which the user specifies a new cryptographic key.
        /// </summary>
        CreateNewKey,

        /// <summary>
        /// Wizard step in which the user specifies a file, the cryptoghrapic key should be stored in.
        /// </summary>
        ChooseKeyFile,

        /// <summary>
        /// Wizard step in which the user specifies the <see cref="System.Security.Cryptography.DataProtectionScope"/> 
        /// which should be used to protect the key file.
        /// </summary>
        ChooseProtectionScope,

        /// <summary>
        /// Wizard step in which the user chooses a existing keyfile that should be used as key.
        /// </summary>
        OpenExistingKeyFile,

        /// <summary>
        /// Wizard step in which the user imports an archived keyfile as key.
        /// </summary>
        ImportArchivedKey,

        /// <summary>
        /// Wizard step that indicates the wizard is completed.
        /// </summary>
        Finished
    }
}
