//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.ComponentModel;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design
{
    internal class ProviderEditorUI : UserControl
    {
        private Container components = null;                
        private ListBox referencesListBox;
		private IWindowsFormsEditorService service;
		private string currentProvider;

		public ProviderEditorUI(IWindowsFormsEditorService service, string currentProvider)
        {
            InitializeComponent();
			this.service = service;
            referencesListBox.Click += new EventHandler(OnListBoxClick);
			this.currentProvider = currentProvider;
			DisplayList();
        }

		public string SelectedProvider
        {
            get
            {
                string selectedProvider = (string)referencesListBox.SelectedItem;
				return selectedProvider;
            }
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        private void OnListBoxClick(object sender, EventArgs args)
        {
            service.CloseDropDown();
        }

        private void DisplayList()
        {			
            referencesListBox.Items.Clear();
			DataSet dbProviderConfig = (DataSet)ConfigurationManager.GetSection("system.data");
			DataTable table = dbProviderConfig.Tables["DbProviderFactories"];
			foreach (DataRow row in table.Rows)
			{
				string invariantRow = (string)row["InvariantName"];
				referencesListBox.Items.Add(invariantRow);
				if (currentProvider == invariantRow)
				{
					referencesListBox.SelectedItem = invariantRow;
				}
			}

        }

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProviderEditorUI));
			this.referencesListBox = new System.Windows.Forms.ListBox();
			this.SuspendLayout();
			// 
			// referencesListBox
			// 
			resources.ApplyResources(this.referencesListBox, "referencesListBox");
			this.referencesListBox.Name = "referencesListBox";
			// 
			// ReferenceEditorUI
			// 
			this.Controls.Add(this.referencesListBox);
			this.Name = "ReferenceEditorUI";
			resources.ApplyResources(this, "$this");
			this.ResumeLayout(false);

        }
        #endregion
    }
}
