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
using System.Collections.ObjectModel;
using System.GACManagedAccess;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Editors
{
    /// <summary>
    /// Interaction logic for SelectGacAssembliesDialog.xaml
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Gac")]
    public partial class SelectGacAssembliesDialog : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectGacAssembliesDialog"/> class.
        /// </summary>
        public SelectGacAssembliesDialog()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var assemblyNames = (AssemblyNamesCollection)this.Resources["AssemblyNames"];
            AssemblyCacheEnum assemblyCacheEnum = null;

            Action initialize = null;
            initialize = () =>
            {
                if (assemblyCacheEnum == null)
                {
                    assemblyCacheEnum = new AssemblyCacheEnum(null);
                }

                int count = 5;
                string name = null;
                while (count-- > 0 && (name = assemblyCacheEnum.GetNextAssembly()) != null)
                {
                    assemblyNames.Add(new AssemblyName(name));
                }

                if (name != null)
                {
                    Dispatcher.BeginInvoke(initialize);
                }
            };
            Dispatcher.BeginInvoke(initialize);
        }

        /// <summary>
        /// Returns the names for the assemblies selected by the user.
        /// </summary>
        /// <returns>The names for the assemblies selected by the user.</returns>
        public IEnumerable<AssemblyName> GetAssemblyNames()
        {
            return this.Assemblies.SelectedItems.OfType<AssemblyName>();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }

    /// <summary>
    /// Non-generic ObservableCollection subclass for XAML support.
    /// </summary>
    public class AssemblyNamesCollection : ObservableCollection<AssemblyName> { }
}
