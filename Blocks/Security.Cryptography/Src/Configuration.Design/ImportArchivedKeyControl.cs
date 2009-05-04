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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design
{
    /// <summary>
    /// Visual control that allows an user to import an protected key from an archived key file, from within the cryptographic key wizard.
    /// </summary>
    public partial class ImportArchivedKeyControl : UserControl, IWizardValidationTarget
    {

        /// <summary>
        /// Initialize a new instance of the <see cref="ImportArchivedKeyControl"/> class.
        /// </summary>
        public ImportArchivedKeyControl()
        {
            InitializeComponent();

            dlgOpenKeyArchive.Filter = Resources.KeyArchiveFileFilter;
            dlgOpenKeyArchive.Title = Resources.ImportDialogTitle;

            lblChooseImportFileMessage.Text = Resources.ImportArchivedKeyMessage;
            lblImportFilePassword.Text = Resources.ImportArchivedKeyPasswordLabel;
        }

        /// <summary>
        /// Gets the file path to archived key file.
        /// </summary>
        public string Filename
        {
            get { return txtChooseImportFileLocation.Text; }
        }

        /// <summary>
        /// Gets the password that should be used reading information from the archived key file.
        /// </summary>
        public string Passphrase
        {
            get { return txtPasswordImportFile.Text; }
        }

        private void btnBrowseImportFileLocation_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == dlgOpenKeyArchive.ShowDialog())
            {
                txtChooseImportFileLocation.Text = dlgOpenKeyArchive.FileName;
            }
        }

        bool IWizardValidationTarget.ValidateControl()
        {
            if (String.IsNullOrEmpty(Filename))
            {
                MessageBox.Show(Resources.FileShouldNotBeEmpty, Resources.CryptoKeyWizardErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            if (!File.Exists(Filename))
            {
                MessageBox.Show(Resources.FileDoesNotExists, Resources.CryptoKeyWizardErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            return true;
        }
    }
}
