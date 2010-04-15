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

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Commands;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
#pragma warning disable 1591
{

    /// <summary>
    /// This class supports block-specific configuration design-time and is not
    /// intended to be used directly from your code.
    /// </summary>
    public class AddInstrumentationBlockCommand : AddApplicationBlockCommand
    {
        public AddInstrumentationBlockCommand(ConfigurationSourceModel configurationModel, AddApplicationBlockCommandAttribute commandAttribute, IUIServiceWpf uiService)
            : base(configurationModel, commandAttribute, uiService)
        {
        }


        protected override void AfterSectionAdded()
        {
            base.AfterSectionAdded();
            AddedSection.IsExpanded = false;
        }

    }

#pragma warning restore 1591
}
