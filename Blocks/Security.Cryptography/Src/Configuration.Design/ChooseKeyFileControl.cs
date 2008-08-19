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
    /// Visual control that allows an user to select the file wich is used to store an cryptographic key.
    /// </summary>
    public partial class ChooseKeyFileControl : UserControl, IWizardValidationTarget
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="ChooseKeyFileControl"/>
        /// </summary>
        public ChooseKeyFileControl()
        {
            InitializeComponent();

            lblChooseKeyFileMessage.Text = Resources.ChooseKeyFileMessage;

            dlgSaveProtectedKey.Title = Resources.SaveExistingKeyFileTitle;
            dlgSaveProtectedKey.Filter = Resources.KeyFileFilter;
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
        public string Filepath
        {
            get { return txtKeyFileLocation.Text; }
            set { txtKeyFileLocation.Text = value; }
        }

        bool IWizardValidationTarget.ValidateControl()
        {
            if (string.IsNullOrEmpty(Filepath))
            {
                MessageBox.Show(Resources.FileShouldNotBeEmpty, Resources.CryptoKeyWizardErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

			if(ValidateFileName(Filepath) == false)
			{
				MessageBox.Show(String.Format(Resources.KeyFileBadNameError, Filepath), Resources.CryptoKeyWizardErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return false;
			}

            return true;
        }

		bool ValidateFileName(string filePath)
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
