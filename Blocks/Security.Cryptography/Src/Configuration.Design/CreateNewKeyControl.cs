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
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design
{
    /// <summary>
    /// Visual control that allows an user to specify a key for an cryptography algorithm.
    /// </summary>
    public partial class CreateNewKeyControl : UserControl, IWizardValidationTarget
    {

        private Regex hexRegex = new Regex("^[0-9a-fA-F]+$", RegexOptions.Compiled | RegexOptions.Singleline);
        private string lastValidationMessage = String.Empty;
        private IKeyCreator keyCreator;

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="CreateNewKeyControl"/> class.</para>
        /// </summary>
        public CreateNewKeyControl()
        {
            InitializeComponent();

            generateKeyButton.Text = Resources.GenerateKeyButtonText;
            lblCreateKeyMessage.Text = Resources.CreateKeyMessage;
        }

        /// <summary>
        /// Sets the <see cref="IKeyCreator"/> that should be used to generate a new random key.
        /// </summary>
        public IKeyCreator KeyCreator
        {
            set { keyCreator = value; }
        }

        /// <summary>
        /// <para>Gets or sets the key for the algorithm.</para>
        /// </summary>
        /// <value>
        /// <para>The key for the algorithm</para>
        /// </value>
        public byte[] Key
        {
            get { return GetKeyBox(); }
            set { SetKeyBox(value); }
        }

        /// <summary>
        /// <para>Gets the length of the current key.</para>
        /// </summary>
        /// <value>
        /// <para>The length of the current key.</para>
        /// </value>
        public int KeyLength
        {
            get
            {
                int length = 0;
                if (keyBox.Text.Length != 0)
                {
                    length = GetKeyBox().Length * 8;
                }
                return length;
            }
        }

        private byte[] GetKeyBox()
        {
            if (keyBox.Text.Length > 0)
            {
                byte [] hexBytes = CryptographyUtility.GetBytesFromHexString(keyBox.Text);
                return hexBytes;
            }
            else
            {
                return new byte[0];
            }
        }

        private void SetKeyBox(byte[] key)
        {
            if (key.Length > 0)
            {
                keyBox.Text = CryptographyUtility.GetHexStringFromBytes(key);
                keyBox.Update();
            }
        }

        private void generateKeyButton_Click(object sender, EventArgs e)
        {
            SetKeyBox(this.keyCreator.GenerateKey());
        }

        bool IWizardValidationTarget.ValidateControl()
        {
            if (string.IsNullOrEmpty(keyBox.Text))
            {
                MessageBox.Show(Resources.KeyShouldNotBeEmpty, Resources.CryptoKeyWizardErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            if (keyBox.Text.Length % 2 != 0 || !this.hexRegex.IsMatch(keyBox.Text))
            {
                MessageBox.Show(Resources.KeyManagerUIInvalidKeyCharactersErrorMessage, Resources.CryptoKeyWizardErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            else if (!this.keyCreator.KeyIsValid(GetKeyBox()))
            {
                MessageBox.Show(Resources.KeyManagerUIInvalidKeyLengthErrorMessage, Resources.CryptoKeyWizardErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            return true;
        }
    }
}
