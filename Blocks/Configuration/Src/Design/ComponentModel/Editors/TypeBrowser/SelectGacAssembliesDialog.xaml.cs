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
    public partial class SelectGacAssembliesDialog : Window
    {
        public SelectGacAssembliesDialog()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var assemblyNames = (AssemblyNamesCollection)this.Resources["AssemblyNames"];
            var assemblyCacheEnum = new AssemblyCacheEnum(null);

            Action initialize = null;
            initialize = () =>
            {
                int count = 25;
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

        public IEnumerable<Assembly> GetAssemblies()
        {
            return this.Assemblies.SelectedItems.Cast<AssemblyName>()
                .Select(an =>
                    {
                        try
                        {
                            return Assembly.ReflectionOnlyLoad(an.FullName);
                        }
                        catch (Exception)
                        {
                            return null;
                        }
                    })
                .Where(a => a != null);
        }
    }

    public class AssemblyNamesCollection : ObservableCollection<AssemblyName> { }
}
