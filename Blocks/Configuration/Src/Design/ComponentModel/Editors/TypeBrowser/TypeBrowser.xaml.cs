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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Win32;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Editors
{
    /// <summary>
    /// Interaction logic for TypeBrowser.xaml
    /// </summary>
    public partial class TypeBrowser : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeBrowser"/> class.
        /// </summary>
        public TypeBrowser()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeBrowser"/> class with a view model and an
        /// assembly discovery service.
        /// </summary>
        /// <param name="viewModel">The <see cref="TypeBrowserViewModel"/>.</param>
        /// <param name="assemblyDiscoveryService">The assembly discovery service.</param>
        public TypeBrowser(TypeBrowserViewModel viewModel, IAssemblyDiscoveryService assemblyDiscoveryService)
            : this()
        {
            this.assemblyDiscoveryService = assemblyDiscoveryService;
            viewModel.UpdateAssemblyGroups(GetAssemblyGroups());
            this.DataContext = viewModel;

            // Consider rearranging this
            this.AddFromFile.GetBindingExpression(Button.VisibilityProperty).UpdateTarget();
            this.AddFromGac.GetBindingExpression(Button.VisibilityProperty).UpdateTarget();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            OnDialogClose();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private void OnDialogClose()
        {
            try
            {
                this.SelectedType = ViewModel.ResolveType();
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Properties.Resources.ErrorResolvingType,
                        exception.Message),
                    Properties.Resources.ErrorResolvingTypeCaption,
                    MessageBoxButton.OK);
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
                var loaded = false;
                foreach (var assemblyName in dialog.GetAssemblyNames())
                {
                    try
                    {
                        Assembly.Load(assemblyName);
                        loaded = true;
                    }
                    catch (FileNotFoundException ex)
                    {
                        ReportErrorLoadingAssembly(ex);
                        continue;
                    }
                    catch (FileLoadException ex)
                    {
                        ReportErrorLoadingAssembly(ex);
                        continue;
                    }
                    catch (BadImageFormatException ex)
                    {
                        ReportErrorLoadingAssembly(ex);
                        continue;
                    }
                }

                if (loaded)
                {
                    UpdateViewModelAfterLoad();
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods", MessageId = "System.Reflection.Assembly.LoadFrom")]
        private void AddFromFile_Click(object sender, RoutedEventArgs e)
        {
            var dialog =
                new OpenFileDialog()
                {
                    AddExtension = true,
                    CheckFileExists = true,
                    CheckPathExists = true,
                    Filter = Properties.Resources.AssembliesFilter,
                    Multiselect = true,
                    ValidateNames = true
                };
            var dialogResult = dialog.ShowDialog(this);

            if (dialogResult.HasValue && dialogResult.Value && dialog.FileNames.Length > 0)
            {
                var folder = Path.GetDirectoryName(dialog.FileNames[0]);
                var loaded = false;

                using (AssemblyLoader loader = new AssemblyLoader(folder))
                {
                    foreach (var fullAssemblyFileName in dialog.FileNames)
                    {
                        try
                        {
                            Assembly.LoadFrom(AssemblyLoader.CreateCopyOfAssembly(fullAssemblyFileName));
                            loaded = true;
                        }
                        catch (FileNotFoundException ex)
                        {
                            ReportErrorLoadingAssembly(ex);
                            continue;
                        }
                        catch (FileLoadException ex)
                        {
                            ReportErrorLoadingAssembly(ex);
                            continue;
                        }
                        catch (BadImageFormatException ex)
                        {
                            ReportBadImageFormatErrorLoadingAssembly(ex);
                            continue;
                        }
                    }

                    if (loaded)
                    {
                        UpdateViewModelAfterLoad();
                    }
                }
            }
        }

        private static void ReportErrorLoadingAssembly(Exception ex)
        {
            MessageBox.Show(
                string.Format(
                    CultureInfo.CurrentCulture,
                    Properties.Resources.AssemblyLoadFailedErrorMessage,
                    ex.Message),
                Properties.Resources.ErrorLoadingAssembly,
                MessageBoxButton.OK);
        }

        private static void ReportBadImageFormatErrorLoadingAssembly(BadImageFormatException ex)
        {
            MessageBox.Show(
                string.Format(
                    CultureInfo.CurrentCulture,
                    Properties.Resources.BadImageExceptionWhenLoading,
                    ex.Message),
                Properties.Resources.ErrorLoadingAssembly,
                MessageBoxButton.OK);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private void UpdateViewModelAfterLoad()
        {
            try
            {
                this.ViewModel.UpdateAssemblyGroups(GetAssemblyGroups());
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Properties.Resources.LoadTypesErrorMessage,
                        ex.Message),
                    Properties.Resources.ErrorLoadingTypes,
                    MessageBoxButton.OK);
            }
        }

        /// <summary>
        /// Gets the type selected by the user.
        /// </summary>
        public Type SelectedType { get; private set; }

        /// <summary>
        /// Gets an indication of whether the browser supports assembly loading.
        /// </summary>
        public bool SupportsAssemblyLoading
        {
            get { return this.assemblyDiscoveryService == null || this.assemblyDiscoveryService.SupportsAssemblyLoading; }
        }

        private TypeBrowserViewModel ViewModel
        {
            get { return (TypeBrowserViewModel)this.DataContext; }
        }

        private IEnumerable<AssemblyGroup> GetAssemblyGroups()
        {
            return this.assemblyDiscoveryService.GetAvailableAssemblies()
                    .Select(kvp => new AssemblyGroup(kvp.Key, kvp.Value));
        }

        private readonly IAssemblyDiscoveryService assemblyDiscoveryService;

        private void TypesTree_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                var header = ((TreeViewItem)sender).Header as TypeNode;
                if ((header != null) && (header.Data != null))
                {
                    e.Handled = true;
                    this.OnDialogClose();
                }
            }
        }
    }
}
