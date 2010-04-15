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
using System.Globalization;


namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design
{
    /// <summary>
    /// Visual control that allows a user to select the file which is used to store an cryptographic key.
    /// </summary>
    public partial class ChooseKeyFileControl : UserControl, IWizardValidationTarget
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="ChooseKeyFileControl"/>
        /// </summary>
        public ChooseKeyFileControl()
        {
            InitializeComponent();

            lblChooseKeyFileMessage.Text = KeyManagerResources.ChooseKeyFileMessage;

            dlgSaveProtectedKey.Title = KeyManagerResources.SaveExistingKeyFileTitle;
            dlgSaveProtectedKey.Filter = KeyManagerResources.KeyFileFilter;
        }

        private void btnBrowseKeyFileLocation_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == dlgSaveProtectedKey.ShowDialog())
            {
                txtKeyFileLocation.Text = dlgSaveProtectedKey.FileName;
            }
        }

        /// <summary>
        /// Gets or sets the filepath which is used to store the cryptographic key.
        /// </summary>
        public string FilePath
        {
            get { return txtKeyFileLocation.Text; }
            set { txtKeyFileLocation.Text = value; }
        }

        bool IWizardValidationTarget.ValidateControl()
        {
            if (string.IsNullOrEmpty(FilePath))
            {
                MessageBox.Show(KeyManagerResources.FileShouldNotBeEmpty, KeyManagerResources.CryptoKeyWizardErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                return false;
            }

			if(ValidateFileName(FilePath) == false)
			{
                MessageBox.Show(String.Format(CultureInfo.CurrentCulture, KeyManagerResources.KeyFileBadNameError, FilePath), KeyManagerResources.CryptoKeyWizardErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
				return false;
			}

            return true;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        static bool ValidateFileName(string filePath)
		{
			try
			{
				Path.GetFullPath(filePath);
				return true;
			}
			catch (Exception)
			{
				return false;
			}

		}
    }
}
