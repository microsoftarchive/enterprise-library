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
using System.GACManagedAccess;
using System.Reflection;
using System.Windows.Forms;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Editors
{
    internal partial class LoadAssemblyFromCacheDialog : Form
    {
        public LoadAssemblyFromCacheDialog()
        {
            InitializeComponent();
        }

        private void LoadAssemblyFromCacheDialog_Load(object sender, EventArgs e)
        {
            AssemblyCacheEnum assemblyCacheEnum = new AssemblyCacheEnum(null);

            string name;
            while ((name = assemblyCacheEnum.GetNextAssembly()) != null)
            {
                AssemblyName assemblyName = new AssemblyName(name);

                ListViewItem item = this.assembliesListView.Items.Add(assemblyName.Name);
                item.SubItems.Add(assemblyName.Version.ToString());
                item.SubItems.Add(assemblyName.ProcessorArchitecture.ToString());
                item.Tag = assemblyName;
            }

            this.assembliesListView.Focus();
        }

        private void assembliesListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.assembliesListView.SelectedItems.Count > 0)
            {
                this.okButton.Enabled = true;
                this.AssemblyName = (AssemblyName)this.assembliesListView.SelectedItems[0].Tag;
            }
            else
            {
                this.okButton.Enabled = false;
                this.AssemblyName = null;
            }
        }

        public AssemblyName AssemblyName { get; private set; }
    }
}
