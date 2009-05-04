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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    partial class LoadAssemblyFromCacheDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoadAssemblyFromCacheDialog));
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.assembliesListView = new System.Windows.Forms.ListView();
            this.AssemblyNameColumn = new System.Windows.Forms.ColumnHeader();
            this.VersionColumn = new System.Windows.Forms.ColumnHeader();
            this.Architecture = new System.Windows.Forms.ColumnHeader();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            resources.ApplyResources(this.cancelButton, "cancelButton");
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // okButton
            // 
            resources.ApplyResources(this.okButton, "okButton");
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Name = "okButton";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // assembliesListView
            // 
            resources.ApplyResources(this.assembliesListView, "assembliesListView");
            this.assembliesListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.AssemblyNameColumn,
            this.VersionColumn,
            this.Architecture});
            this.assembliesListView.FullRowSelect = true;
            this.assembliesListView.MultiSelect = false;
            this.assembliesListView.Name = "assembliesListView";
            this.assembliesListView.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.assembliesListView.UseCompatibleStateImageBehavior = false;
            this.assembliesListView.View = System.Windows.Forms.View.Details;
            this.assembliesListView.SelectedIndexChanged += new System.EventHandler(this.assembliesListView_SelectedIndexChanged);
            // 
            // AssemblyNameColumn
            // 
            resources.ApplyResources(this.AssemblyNameColumn, "AssemblyNameColumn");
            // 
            // VersionColumn
            // 
            resources.ApplyResources(this.VersionColumn, "VersionColumn");
            // 
            // Architecture
            // 
            resources.ApplyResources(this.Architecture, "Architecture");
            // 
            // LoadAssemblyFromCacheDialog
            // 
            this.AcceptButton = this.okButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.Controls.Add(this.assembliesListView);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.cancelButton);
            this.Name = "LoadAssemblyFromCacheDialog";
            this.Load += new System.EventHandler(this.LoadAssemblyFromCacheDialog_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.ListView assembliesListView;
        private System.Windows.Forms.ColumnHeader AssemblyNameColumn;
        private System.Windows.Forms.ColumnHeader VersionColumn;
        private System.Windows.Forms.ColumnHeader Architecture;
    }
}
