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
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.EnvironmentalOverrides;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Win32;
using System.Globalization;
using System.Windows;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{
#pragma warning disable 1591
    /// <summary>
    /// This class supports block-specific configuration design-time and is not
    /// intended to be used directly from your code.
    /// </summary>
    public class SaveEnvironmentConfigurationDeltaCommand : CommandModel
    {
        readonly EnvironmentSourceViewModel overridesViewModel;

        public SaveEnvironmentConfigurationDeltaCommand(IUIServiceWpf uiService, ElementViewModel overridesViewModel)
            : base(uiService)
        {
            this.overridesViewModel = (EnvironmentSourceViewModel)overridesViewModel;
        }

        protected override bool InnerCanExecute(object parameter)
        {
            return true;
        }

        public override string Title
        {
            get
            {
                return DesignResources.SaveEnvironmentDeltaCommandTitle;
            }
        }

        protected override void InnerExecute(object parameter)
        {
            overridesViewModel.SaveDelta();
        }
    }
#pragma warning restore 1591
}
