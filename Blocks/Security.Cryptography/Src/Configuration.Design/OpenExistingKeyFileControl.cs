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
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design
{
    /// <summary>
    /// Visual control that allows an user to select an existing key file within the cryptographic key wizard.
    /// </summary>
    public partial class OpenExistingKeyFileControl : UserControl, IWizardValidationTarget
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="OpenExistingKeyFileControl"/> class.
        /// </summary>
        public OpenExistingKeyFileControl()
        {
            InitializeComponent();

            dlgLoadKeyfile.Filter = Resources.KeyFileFilter;
            dlgLoadKeyfile.Title = Resources.OpenExistingKeyFileTitle;

            lblChooseExistingKeyFileLocation.Text = Resources.ChooseExistingKeyFileMessage;
        }

        /// <summary>
        /// The file path to the existing key file.
        /// </summary>
        public string Filepath
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
            if (string.IsNullOrEmpty(Filepath))
            {
                MessageBox.Show(Resources.FileShouldNotBeEmpty, Resources.CryptoKeyWizardErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            if (!File.Exists(Filepath))
            {
                MessageBox.Show(Resources.FileDoesNotExists, Resources.CryptoKeyWizardErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            return true;
        }
    }
}
