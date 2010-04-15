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
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;


namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design
{
    /// <summary>
    /// UI wizard that allows a user to manipulate a <see cref="ProtectedKeySettings"/> class, from within the configuration console.
    /// </summary>
    public partial class CryptographicKeyWizard : Form
    {
        private ProtectedKeySettings protectedKeySettings = new ProtectedKeySettings();
        private UserControl currentControl = null;
        private CryptographicKeyWizardStep currentWizardStep = CryptographicKeyWizardStep.SupplyKey;
        private Stack<CryptographicKeyWizardStep> previousWizardSteps = new Stack<CryptographicKeyWizardStep>();
        private Dictionary<CryptographicKeyWizardStep, UserControl> controlByCryptographicKeyWizardStep = new Dictionary<CryptographicKeyWizardStep, UserControl>();


        /// <summary>
        /// Initializes a new instance of the <see cref="CryptographicKeyWizard"/> class with a <see cref="IKeyCreator"/>.
        /// </summary>
        /// <param name="keyCreator">The <see cref="IKeyCreator"/> that should be used to generate and validate an input key.</param>
        public CryptographicKeyWizard(IKeyCreator keyCreator)
            : this(CryptographicKeyWizardStep.SupplyKey, keyCreator)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CryptographicKeyWizard"/> class with a <see cref="IKeyCreator"/> and 
        /// for a specific <see cref="CryptographicKeyWizardStep"/>.
        /// </summary>
        /// <param name="keyCreator">The <see cref="IKeyCreator"/> that should be used to generate and validate an input key.</param>
        /// <param name="step">The <see cref="CryptographicKeyWizardStep"/> which should be shown within the wizard.</param>
        public CryptographicKeyWizard(CryptographicKeyWizardStep step, IKeyCreator keyCreator)
        {
            InitializeComponent();

            controlByCryptographicKeyWizardStep.Add(CryptographicKeyWizardStep.SupplyKey, supplyKeyControl);
            controlByCryptographicKeyWizardStep.Add(CryptographicKeyWizardStep.CreateNewKey, createNewKeyControl);
            controlByCryptographicKeyWizardStep.Add(CryptographicKeyWizardStep.OpenExistingKeyFile, openExistingKeyFileControl);
            controlByCryptographicKeyWizardStep.Add(CryptographicKeyWizardStep.ImportArchivedKey, importArchivedKeyControl);
            controlByCryptographicKeyWizardStep.Add(CryptographicKeyWizardStep.ChooseProtectionScope, chooseDpapiScopeControl);
            controlByCryptographicKeyWizardStep.Add(CryptographicKeyWizardStep.ChooseKeyFile, chooseProtectionScopeControl);

            btnCancel.Text = KeyManagerResources.CryptographicKeyWizardCancelButton;
            btnPrevious.Text = KeyManagerResources.CryptographicKeyWizardPreviousButton;
            btnNext.Text = KeyManagerResources.CryptographicKeyWizardNextButton;
            btnFinish.Text = KeyManagerResources.CryptographicKeyWizardFinishButton;
            Text = KeyManagerResources.CryptographicKeyWizardTitle;


            createNewKeyControl.KeyCreator = keyCreator;

            currentWizardStep = step;
            RefreshWizardControls();
        }

        /// <summary>
        /// Gets or sets the <see cref="ProtectedKeySettings"/> for this wizard.
        /// </summary>
        public ProtectedKeySettings KeySettings
        {
            get { return protectedKeySettings; }
            set
            {
                if (value == null) throw new ArgumentNullException("value");

                createNewKeyControl.Key = value.ProtectedKey.DecryptedKey;
                chooseDpapiScopeControl.Scope = value.Scope;
                chooseProtectionScopeControl.FilePath = value.FileName;
            }
        }

        /// <summary>
        /// Sets an unencrypted key to be used within the wizard.    
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1044:PropertiesShouldNotBeWriteOnly")]
        public Byte[] Key
        {
            set { createNewKeyControl.Key = value; }
        }

        /// <summary>
        /// Sets an <see cref="IKeyCreator"/> instance, that should be used to validate and generate keys.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1044:PropertiesShouldNotBeWriteOnly")]
        public IKeyCreator KeyCreator
        {
            set { createNewKeyControl.KeyCreator = value; }
        }

        private void UpdateState()
        {
            switch (currentWizardStep)
            {
                case CryptographicKeyWizardStep.ChooseKeyFile:
                    protectedKeySettings.FileName = chooseProtectionScopeControl.FilePath;
                    break;

                case CryptographicKeyWizardStep.OpenExistingKeyFile:
                    protectedKeySettings.FileName = openExistingKeyFileControl.FilePath;
                    break;

                case CryptographicKeyWizardStep.ChooseProtectionScope:
                    protectedKeySettings.Scope = chooseDpapiScopeControl.Scope;
                    break;
            }
        }

        private CryptographicKeyWizardStep NextStep
        {
            get
            {
                switch (currentWizardStep)
                {
                    case CryptographicKeyWizardStep.SupplyKey:
                        switch (supplyKeyControl.Method)
                        {
                            case SupplyKeyMethod.CreateNew:
                                return CryptographicKeyWizardStep.CreateNewKey;

                            case SupplyKeyMethod.ImportKey:
                                return CryptographicKeyWizardStep.ImportArchivedKey;

                            case SupplyKeyMethod.OpenExisting:
                                return CryptographicKeyWizardStep.OpenExistingKeyFile;
                        }

                        return CryptographicKeyWizardStep.CreateNewKey;

                    case CryptographicKeyWizardStep.CreateNewKey:
                        return CryptographicKeyWizardStep.ChooseKeyFile;

                    case CryptographicKeyWizardStep.ChooseKeyFile:
                        return CryptographicKeyWizardStep.ChooseProtectionScope;

                    case CryptographicKeyWizardStep.OpenExistingKeyFile:
                        return CryptographicKeyWizardStep.ChooseProtectionScope;

                    case CryptographicKeyWizardStep.ImportArchivedKey:
                        return CryptographicKeyWizardStep.ChooseKeyFile;

                    default:
                        return CryptographicKeyWizardStep.Finished;
                }
            }
        }

        private void RefreshWizardControls()
        {
            btnNext.Visible = (NextStep != CryptographicKeyWizardStep.Finished);
            btnFinish.Visible = (NextStep == CryptographicKeyWizardStep.Finished);
            btnPrevious.Visible = (previousWizardSteps.Count != 0);

            if (currentControl != null)
            {
                currentControl.Visible = false;
            }

            currentControl = controlByCryptographicKeyWizardStep[currentWizardStep];
            currentControl.Visible = true;
            currentControl.Dock = DockStyle.Fill;
        }

        private void DoProceed()
        {
            IWizardValidationTarget validationTarget = currentControl as IWizardValidationTarget;
            if (validationTarget != null && !validationTarget.ValidateControl())
            {
                return;
            }

            UpdateState();

            previousWizardSteps.Push(currentWizardStep);
            currentWizardStep = NextStep;

            RefreshWizardControls();
        }

        private void DoGoBack()
        {
            currentWizardStep = previousWizardSteps.Pop();

            RefreshWizardControls();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (currentControl != null)
            {
                DoProceed();
            }
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            DoGoBack();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private void btnFinish_Click(object sender, EventArgs e)
        {
            switch (supplyKeyControl.Method)
            {
                case SupplyKeyMethod.OpenExisting:
                    using (Stream importedKeyReader = File.OpenRead(openExistingKeyFileControl.FilePath))
                    {
                        try
                        {
                            protectedKeySettings.ProtectedKey = KeyManager.Read(importedKeyReader, chooseDpapiScopeControl.Scope);
                            protectedKeySettings.Scope = chooseDpapiScopeControl.Scope;
                        }
                        catch (Exception)
                        {
                            MessageBox.Show(KeyManagerResources.ErrorImportingKey, KeyManagerResources.CryptoKeyWizardErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                        }
                    }
                    break;

                case SupplyKeyMethod.CreateNew:
                    protectedKeySettings.ProtectedKey = ProtectedKey.CreateFromPlaintextKey(createNewKeyControl.Key, chooseDpapiScopeControl.Scope);
                    protectedKeySettings.Scope = chooseDpapiScopeControl.Scope;
                    break;

                case SupplyKeyMethod.ImportKey:
                    using (Stream archivedKeyReader = File.OpenRead(importArchivedKeyControl.FileName))
                    {
                        try
                        {
                            protectedKeySettings.ProtectedKey = KeyManager.RestoreKey(archivedKeyReader, importArchivedKeyControl.PassPhrase, chooseDpapiScopeControl.Scope);
                            protectedKeySettings.Scope = chooseDpapiScopeControl.Scope;
                        }
                        catch (CryptographicException)
                        {
                            MessageBox.Show(KeyManagerResources.KeyCouldNotBeRead, KeyManagerResources.CryptoKeyWizardErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                            this.DialogResult = DialogResult.None;
                        }
                    }
                    break;
            }
        }
    }
}
