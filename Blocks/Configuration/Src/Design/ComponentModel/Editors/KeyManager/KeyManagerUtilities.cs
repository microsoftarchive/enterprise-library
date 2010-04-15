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
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using System.Globalization;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design
{
    /// <summary>
    /// The <see cref="KeyManagerUtilities"/> provides common <see cref="ICryptographicKeyProperty"/> routines
    /// used by the design-time.
    /// </summary>
    public static class KeyManagerUtilities
    {
        ///<summary>
        /// Imports a protected key from a file and updates a <see cref="ProtectedKeySettings"/> structure with the imported values.
        ///</summary>
        ///<param name="uiService">The user-interface service for displaying messages and windows to the user.</param>
        ///<param name="keySettings">The key settings structure to update.</param>
        public static void ImportProtectedKey(IUIServiceWpf uiService, ProtectedKeySettings keySettings)
        {
            string protectedKeyFilename = keySettings.FileName;
            DataProtectionScope protectedKeyScope = keySettings.Scope;

            if (!File.Exists(protectedKeyFilename))
            {
                var dialogResult = uiService.ShowMessageWpf(
                                        string.Format(CultureInfo.CurrentCulture, Resources.MessageboxMessageUnableToLocateCryptoKey, keySettings.FileName),
                                        Resources.MessageboxTitleUnableToLocateCryptoKey,
                                        System.Windows.MessageBoxButton.YesNo);
                if (dialogResult == System.Windows.MessageBoxResult.Yes)
                {

                    Microsoft.Win32.OpenFileDialog locateCryptoKeyDialog = new Microsoft.Win32.OpenFileDialog
                    {
                        Title = Resources.MessageboxTitleUnableToLocateCryptoKey
                    };

                    var locateFileDislogresult = uiService.ShowFileDialog(locateCryptoKeyDialog);
                    if (true == locateFileDislogresult.DialogResult)
                    {
                        protectedKeyFilename = locateCryptoKeyDialog.FileName;
                    }
                }
            }

            using (Stream keyFileContents = File.OpenRead(protectedKeyFilename))
            {
                var key = KeyManager.Read(keyFileContents, protectedKeyScope);
                keySettings.ProtectedKey = key;
                keySettings.Scope = protectedKeyScope;
                keySettings.FileName = protectedKeyFilename;
            }
        }
    }
}
