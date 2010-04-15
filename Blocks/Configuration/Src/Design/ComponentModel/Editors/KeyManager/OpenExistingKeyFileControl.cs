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
using System.IO;


namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design
{
    /// <summary>
    /// Visual control that allows a user to select an existing key file within the cryptographic key wizard.
    /// </summary>
    public partial class OpenExistingKeyFileControl : UserControl, IWizardValidationTarget
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="OpenExistingKeyFileControl"/> class.
        /// </summary>
        public OpenExistingKeyFileControl()
        {
            InitializeComponent();

            dlgLoadKeyfile.Filter = KeyManagerResources.KeyFileFilter;
            dlgLoadKeyfile.Title = KeyManagerResources.OpenExistingKeyFileTitle;

            lblChooseExistingKeyFileLocation.Text = KeyManagerResources.ChooseExistingKeyFileMessage;
        }

        /// <summary>
        /// The file path to the existing key file.
        /// </summary>
        public string FilePath
        {
            get { return txtChooseExistingKeyFileLocation.Text; }
            set { txtChooseExistingKeyFileLocation.Text = value; }
        }

        private void btnBrowseExistingKeyFileLocation_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == dlgLoadKeyfile.ShowDialog())
            {
                txtChooseExistingKeyFileLocation.Text = dlgLoadKeyfile.FileName;
            }
        }

        bool IWizardValidationTarget.ValidateControl()
        {
            if (string.IsNullOrEmpty(FilePath))
            {
                MessageBox.Show(KeyManagerResources.FileShouldNotBeEmpty, KeyManagerResources.CryptoKeyWizardErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                return false;
            }
            if (!File.Exists(FilePath))
            {
                MessageBox.Show(KeyManagerResources.FileDoesNotExists, KeyManagerResources.CryptoKeyWizardErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                return false;
            }

            return true;
        }
    }
}
