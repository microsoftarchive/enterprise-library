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
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows;
using Microsoft.Win32;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Editors
{
    /// <summary>
    /// Interaction logic for TypeBrowser.xaml
    /// </summary>
    public partial class TypeBrowser : Window
    {
        private const string AssembliesFilter = "Assemblies|*.dll;*.exe";

        public TypeBrowser()
        {
            InitializeComponent();
        }

        public TypeBrowser(TypeBrowserViewModel viewModel)
            : this()
        {
            this.DataContext = viewModel;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.SelectedType = ((TypeBrowserViewModel)this.DataContext).ResolveType();
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Properties.Resources.ErrorResolvingType,
                        exception.Message),
                    Properties.Resources.ErrorResolvingTypeCaption);
            }

            if (this.SelectedType != null)
            {
                this.DialogResult = true;
            }
        }

        private void AddFromGac_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SelectGacAssembliesDialog();
            dialog.Owner = this;
            dialog.ShowDialog();

            if (dialog.DialogResult.HasValue && dialog.DialogResult.Value)
            {
            }
        }

        private void AddFromFile_Click(object sender, RoutedEventArgs e)
        {
            var dialog =
                new OpenFileDialog()
                {
                    AddExtension = true,
                    CheckFileExists = true,
                    CheckPathExists = true,
                    Filter = AssembliesFilter,
                    Multiselect = true,
                    ValidateNames = true
                };
            var dialogResult = dialog.ShowDialog(this);

            if (dialogResult.HasValue && dialogResult.Value)
            {
                var viewModel = (TypeBrowserViewModel)this.DataContext;

                foreach (var assemblyFileName in dialog.FileNames)
                {
                    string originalAssemblyFileName = assemblyFileName;
                    string referenceDirectory = Path.GetDirectoryName(originalAssemblyFileName);

                    using (AssemblyLoader loaderHook = new AssemblyLoader(originalAssemblyFileName, referenceDirectory))
                    {
                        Assembly newAssembly = null;

                        try
                        {
                            newAssembly = Assembly.LoadFrom(loaderHook.CopiedAssemblyPath);
                        }
                        catch (Exception)
                        {
                        }

                        if (newAssembly != null)
                        {
                        }
                    }
                }
            }
        }

        public Type SelectedType { get; private set; }
    }
}
