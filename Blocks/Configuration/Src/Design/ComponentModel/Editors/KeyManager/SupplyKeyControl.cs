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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;


namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design
{
    /// <summary>
    /// Visual control that allows a user to select an to select a method of proving a key.
    /// </summary>
    public partial class SupplyKeyControl : UserControl
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="SupplyKeyControl"/> class.
        /// </summary>
        public SupplyKeyControl()
        {
            InitializeComponent();

            rbImportKey.Text = KeyManagerResources.ImportKeyLabel;
            rbUseExistingKey.Text = KeyManagerResources.ExistingKeyLabel;
            rbCreateNewKey.Text = KeyManagerResources.CreateNewKeyLabel;
            lblSupplyKeyMessage.Text = KeyManagerResources.SupplyKeyMessage;
        }

        /// <summary>
        /// Gets the <see cref="SupplyKeyMethod"/> which should be used inside the cryptographic key wizard.
        /// </summary>
        public SupplyKeyMethod Method
        {
            get
            {
                if (rbImportKey.Checked) return SupplyKeyMethod.ImportKey;
                if (rbUseExistingKey.Checked) return SupplyKeyMethod.OpenExisting;
                return SupplyKeyMethod.CreateNew;
            }
        }
    }

    /// <summary>
    /// Enumeration to represent the ways a user can provide a cryptographic key.
    /// </summary>
    public enum SupplyKeyMethod
    {
        /// <summary>
        /// A new key should be created.
        /// </summary>
        CreateNew,

        /// <summary>
        /// An existing protected key file should be used.
        /// </summary>
        OpenExisting,

        /// <summary>
        /// An archived keyfile should be used.
        /// </summary>
        ImportKey
    }
}
