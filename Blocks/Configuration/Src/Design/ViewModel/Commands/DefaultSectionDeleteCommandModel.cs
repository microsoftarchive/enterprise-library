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
using System.Windows;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Commands
{
    /// <summary>
    /// Deletes a section after prompting the user to confirm deletion.
    /// </summary>
    public class DefaultSectionDeleteCommandModel : DefaultDeleteCommandModel
    {
        private readonly IUIServiceWpf uiService;

        /// <summary>
        /// Initializes a new instance of <see cref="DefaultSectionDeleteCommandModel"/>
        /// </summary>
        /// <param name="section">The section element to apply this command to.</param>
        /// <param name="uiService">The UI service used to display messages.</param>
        public DefaultSectionDeleteCommandModel(ElementViewModel section, IUIServiceWpf uiService) 
            : base(section, uiService)
        {
            this.uiService = uiService;
        }

        /// <summary>
        /// Deletes the section after prompting the user for confirmation.
        /// </summary>
        /// <param name="parameter">Not used.</param>
        protected override void InnerExecute(object parameter)
        {
            MessageBoxResult result = uiService.ShowMessageWpf(
                Resources.DeleteSectionConfirmationDialogMessage, 
                Resources.DeleteSectionConfirmationDialogTitle, 
                MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                base.InnerExecute(parameter);
            }
        }
    }
}
