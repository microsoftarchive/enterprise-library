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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Win32;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Console.Hosting
{
    /// <summary>
    /// Interaction logic for ConfigurationEditor.xaml
    /// </summary>
    public partial class ConfigurationEditorUI : UserControl
    {
        /// <summary>
        /// Creates a new instance of <see cref="ConfigurationEditorUI"/>.
        /// </summary>
        public ConfigurationEditorUI()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Updates the sice of this <see cref="ConfigurationEditorUI"/>.
        /// </summary>
        public void UpdateSize(double height, double width)
        {
            this.Height = height;
            this.Width = width;
        }
    }
}
