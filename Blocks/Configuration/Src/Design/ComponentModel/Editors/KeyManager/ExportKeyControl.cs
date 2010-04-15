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
    /// Visual control that allows a user to export an protected key to an archived key file.
    /// </summary>
    public partial class ExportKeyControl : UserControl, IWizardValidationTarget
    {
        private const int MinPasswordLength = 6;

        /// <summary>
        /// Initialize a new instance of the <see cref="ExportKeyControl"/> class.
        /// </summary>
        public ExportKeyControl()
        {
            InitializeComponent();

            browseDialog.Filter = KeyManagerResources.KeyArchiveFileFilter;
            browseDialog.Title = KeyManagerResources.ExportDialogTitle;

            lblPasswordExportKey.Text = KeyManagerResources.ExportKeyPasswordLabel;
            lblConfirmPasswordExportKey.Text = KeyManagerResources.ExportKeyPasswordConfirmLabel;
            lblExportKeyFileMessage.Text = KeyManagerResources.ExportKeyFileMessage;
        }

        private bool OverwriteFile()
        {
            if (!IsFileReadOnly(txtExportFileLocation.Text)) return true;

            DialogResult result = MessageBox.Show(string.Format(CultureInfo.CurrentCulture, KeyManagerResources.OverwriteExportFileMessage, txtExportFileLocation.Text), KeyManagerResources.OverwriteExportFileCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

            if (DialogResult.No == result) return false;
            ChangeFileAttributesToWritable(txtExportFileLocation.Text);
            return true;
        }

        private bool ValidatePath()
        {
            bool result = true;

            if (txtExportFileLocation.Text.Length == 0)
            {
                result = false;
            }
            else
            {
                string directory = Path.GetDirectoryName(txtExportFileLocation.Text);
                if (!Directory.Exists(directory))
                {
                    result = false;
                }
            }

            if (!result)
            {
                MessageBox.Show(KeyManagerResources.ExportDirectoryInvalid, KeyManagerResources.ExportDialogErrorTitle,
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            }

            return result;
        }

        private bool ConfirmPassword()
        {
            bool result = true;

            result = (txtPassword1.Text == txtPassword2.Text);
            if (!result)
            {
                MessageBox.Show(KeyManagerResources.ExportPasswordsDoNotMatch, KeyManagerResources.ExportDialogErrorTitle,
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
				result = false;
            }
            else
            {
                if (txtPassword1.Text.Length < MinPasswordLength)
                {
                    MessageBox.Show(string.Format(CultureInfo.CurrentCulture, KeyManagerResources.ExportPasswordMinLength, MinPasswordLength.ToString(CultureInfo.CurrentCulture)),
                                    KeyManagerResources.ExportDialogErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    result = false;
                }
            }
            return result;
        }

        private static bool IsFileReadOnly(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return false;
            }
            FileAttributes attributes = File.GetAttributes(filePath);
            FileAttributes attr = attributes | FileAttributes.ReadOnly;
            if (attr == attributes)
            {
                return true;
            }
            return false;
        }

        private static void ChangeFileAttributesToWritable(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException("filePath");
            }

            if (!File.Exists(filePath))
            {
                return;
            }
            FileAttributes attributes = File.GetAttributes(filePath);
            FileAttributes attr = attributes | FileAttributes.ReadOnly;
            if (attr == attributes)
            {
                attributes ^= FileAttributes.ReadOnly;
                File.SetAttributes(filePath, attributes);
            }
        }

        private void btnBrowseExportKeyLocation_Click(object sender, EventArgs e)
        {
            if (browseDialog.ShowDialog() == DialogResult.OK)
            {
                txtExportFileLocation.Text = browseDialog.FileName;
            }
        }

        /// <summary>
        /// Gets or sets the file path to which the key should be exported.
        /// </summary>
        public string FileName
        {
            get { return txtExportFileLocation.Text; }
            set { txtExportFileLocation.Text = value; }
        }

        /// <summary>
        /// Gets the password which should be used to export the key.
        /// </summary>
        public string Password
        {
            get { return txtPassword1.Text; }
        }

		/// <summary>
		/// Validates the fields on this form
		/// </summary>
		/// <returns>True if all fields on form are valid.</returns>
        public bool ValidateControl()
        {
            if (!ConfirmPassword())
            {
                return false;
            }
            if (!ValidatePath())
            {
                return false;
            }

            if (!OverwriteFile())
            {
                return false;
            }
            return true;
        }
    }
}
