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
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.IO;
using System.Security.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Editors;

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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
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
                        throw new ArgumentException(KeyManagerResources.KeyManagerEditorRequiresKeyInfoProperty);
                    }

                    var bindableProperty = EditorUtility.GetBindableProperty(context);
                    ICryptographicKeyProperty cryptographicKeyContainer = bindableProperty.Property as ICryptographicKeyProperty;
                    CryptographicKeyWizard dialog = new CryptographicKeyWizard(CryptographicKeyWizardStep.CreateNewKey, cryptographicKeyContainer.KeyCreator);

                    try
                    {
                        var uiService = (IUIServiceWpf)provider.GetService(typeof(IUIServiceWpf));
                        KeyManagerUtilities.ImportProtectedKey(uiService, keySettings);
                        dialog.KeySettings = keySettings;
                    }
                    catch (FileNotFoundException)
                    {
                        //this is handled by the KeyManager utilites;
                        return value;
                    }
                    catch (IOException ioe)
                    {
                        MessageBox.Show(String.Format(CultureInfo.CurrentCulture, KeyManagerResources.OpenExistingKeyFileFailureErrorMessage, keySettings.FileName, ioe.Message), KeyManagerResources.OpenExistingKeyFileFailureTitle, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        return value;
                    }
                    catch (Exception e)
                    {
                        IUIServiceWpf uiService = (IUIServiceWpf)provider.GetService(typeof(IUIServiceWpf));
                        string unableToLoadKeyMessage = string.Format(CultureInfo.CurrentCulture, KeyManagerResources.LoadExistingKeyFileFailureErrorMessage, keySettings.FileName, e.Message);
                        var unableToLoadKeyDialogResult = uiService.ShowMessageWpf(unableToLoadKeyMessage, KeyManagerResources.OpenExistingKeyFileFailureTitle, System.Windows.MessageBoxButton.YesNo);
                        if (unableToLoadKeyDialogResult == System.Windows.MessageBoxResult.No)
                        {
                            return value;
                        }
                    }
					
                    DialogResult editorDialogResult = service.ShowDialog(dialog);

                    if (editorDialogResult == DialogResult.Cancel)
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
    }
}
