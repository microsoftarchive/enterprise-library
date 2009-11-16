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
    /// Represents the validation logic for a control within the <see cref="CryptographicKeyWizard"/>.
    /// </summary>
    public interface IWizardValidationTarget
    {
        /// <summary>
        /// Validates the control
        /// </summary>
        /// <returns><see langword="true"/> if controls state is valid; otherwise <see langword="false"/>.</returns>
        bool ValidateControl();
    }
}
