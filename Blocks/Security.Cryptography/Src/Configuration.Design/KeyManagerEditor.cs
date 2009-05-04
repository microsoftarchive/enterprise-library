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
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design.Properties;
using System.IO;
using System.Security.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design
{
    /// <summary>
    /// Provides a user interface for managing cryptogrpahic key information.
    /// </summary>
    internal class KeyManagerEditor : UITypeEditor
    {

        /// <summary>
        /// Edits the specified object using the editor style provided by the GetEditStyle method.
        /// </summary>
        /// <param name="context">
        /// An ITypeDescriptorContext that can be used to gain additional context information.
        /// </param>
        /// <param name="provider">
        /// A service provider object through which editing services may be obtained.
        /// </param>
        /// <param name="value">
        /// An instance of the value being edited.
        /// </param>
        /// <returns>
        /// The new value of the object. If the value of the object hasn't changed, this should return the same object it was passed.
        /// </returns>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            Debug.Assert(provider != null, "No service provider; we cannot edit the value");
            if (provider != null)
            {
                IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

                Debug.Assert(edSvc != null, "No editor service; we cannot edit the value");
                if (edSvc != null)
                {
                    IWindowsFormsEditorService service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

                    ProtectedKeySettings keySettings = value as ProtectedKeySettings;
                    if (keySettings == null)
                    {
                        throw new ArgumentException(Resources.KeyManagerEditorRequiresKeyInfoProperty);
                    }

					ICryptographicKeyConfigurationNode cryptographicKeyContainer = context.Instance as ICryptographicKeyConfigurationNode;

					try
					{
						ImportProtectedKey(cryptographicKeyContainer);
					}
					catch (IOException ioe)
					{
						MessageBox.Show(String.Format(Resources.OpenExistingKeyFileFailureErrorMessage, keySettings.Filename, ioe.Message), Resources.OpenExistingKeyFileFailureTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
						return value;
					}
					
					CryptographicKeyWizard dialog = new CryptographicKeyWizard(CryptographicKeyWizardStep.CreateNewKey, cryptographicKeyContainer.KeyCreator);
                    dialog.KeySettings = keySettings;

                    DialogResult dialogResult = service.ShowDialog(dialog);

                    if (dialogResult == DialogResult.Cancel)
                    {
                        return keySettings;
                    }
                    else
                    {
                        return dialog.KeySettings;
                    }
                }
            }
            return value;
        }

        /// <summary>
        /// See <see cref="UITypeEditor.GetEditStyle(ITypeDescriptorContext)"/>.
        /// </summary>
        /// <param name="context">See <see cref="UITypeEditor.GetEditStyle(ITypeDescriptorContext)"/>.</param>
        /// <returns>See <see cref="UITypeEditor.GetEditStyle(ITypeDescriptorContext)"/>.</returns>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

		private static void ImportProtectedKey(ICryptographicKeyConfigurationNode keyContainer)
		{
			string protectedKeyFilename = keyContainer.KeySettings.Filename;
			DataProtectionScope protectedKeyScope = keyContainer.KeySettings.Scope;

			using (Stream keyFileContents = File.OpenRead(protectedKeyFilename))
			{
				keyContainer.KeySettings.ProtectedKey = KeyManager.Read(keyFileContents, protectedKeyScope);
				keyContainer.KeySettings.Scope = protectedKeyScope;
			}
		}
    }
}
