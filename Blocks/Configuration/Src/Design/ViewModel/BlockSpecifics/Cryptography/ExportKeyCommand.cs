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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.Cryptography
{
#pragma warning disable 1591
    /// <summary>
    /// This class supports block-specific configuration design-time and is not
    /// intended to be used directly from your code.
    /// </summary>
    public class ExportKeyCommand : CommandModel
    {
        ICryptographicKeyProperty keyProperty;
        IUIServiceWpf uiService;

        public ExportKeyCommand(CommandAttribute commandAttribute, ElementViewModel subject, IUIServiceWpf uiService)
            :base(commandAttribute, uiService)
        {
            this.uiService = uiService;
            this.keyProperty = subject.Properties.OfType<ICryptographicKeyProperty>().FirstOrDefault();
        }

        protected override bool InnerCanExecute(object parameter)
        {
            return keyProperty != null;
        }

        protected override void InnerExecute(object parameter)
        {
            var key = keyProperty.KeySettings;
            KeyManagerUtilities.ImportProtectedKey(uiService, key);

            using (var exportKeyUI = new ExportKeyUI(key.ProtectedKey))
            {
                uiService.ShowDialog(exportKeyUI);
            }
        }
    }

#pragma warning restore 1591
}
