//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation. All rights reserved.
// Adapted from ACA.NET with permission from Avanade Inc.
// ACA.NET copyright © Avanade Inc. All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Console
{
	/// <devdoc>
	/// Allows users to pick which hierarchies to save
	/// </devdoc>
	internal class SaveChangesDialog : Form
	{
        private Button cancelButton;
        private Button noButton;
        private Button yesButton;
        private ListBox saveItemsListBox;
        private Label descriptionLabel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private Container components = null;

		public SaveChangesDialog(ArrayList hierarchies)
		{
            InitializeComponent();
            int index = 0;
		    foreach (IConfigurationUIHierarchy hierarchy in hierarchies)
		    {
		        index = saveItemsListBox.Items.Add(hierarchy.RootNode);
                saveItemsListBox.SelectedItem = saveItemsListBox.Items[index];
		    }
		}

        public ArrayList SelectedHieraries
        {
            get
            {
                ArrayList selectedHierarchies = new ArrayList(5);
                foreach (object item in saveItemsListBox.SelectedItems)
                {
                    selectedHierarchies.Add(((ConfigurationNode)item).Hierarchy);
                }
                return selectedHierarchies;
            }
        }

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

        private void noButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
        }

        private void yesButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Yes;
            Close();
        }

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(SaveChangesDialog));
            this.cancelButton = new System.Windows.Forms.Button();
            this.noButton = new System.Windows.Forms.Button();
            this.yesButton = new System.Windows.Forms.Button();
            this.saveItemsListBox = new System.Windows.Forms.ListBox();
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.AccessibleDescription = resources.GetString("cancelButton.AccessibleDescription");
            this.cancelButton.AccessibleName = resources.GetString("cancelButton.AccessibleName");
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("cancelButton.Anchor")));
            this.cancelButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("cancelButton.BackgroundImage")));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("cancelButton.Dock")));
            this.cancelButton.Enabled = ((bool)(resources.GetObject("cancelButton.Enabled")));
            this.cancelButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("cancelButton.FlatStyle")));
            this.cancelButton.Font = ((System.Drawing.Font)(resources.GetObject("cancelButton.Font")));
            this.cancelButton.Image = ((System.Drawing.Image)(resources.GetObject("cancelButton.Image")));
            this.cancelButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("cancelButton.ImageAlign")));
            this.cancelButton.ImageIndex = ((int)(resources.GetObject("cancelButton.ImageIndex")));
            this.cancelButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("cancelButton.ImeMode")));
            this.cancelButton.Location = ((System.Drawing.Point)(resources.GetObject("cancelButton.Location")));
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("cancelButton.RightToLeft")));
            this.cancelButton.Size = ((System.Drawing.Size)(resources.GetObject("cancelButton.Size")));
            this.cancelButton.TabIndex = ((int)(resources.GetObject("cancelButton.TabIndex")));
            this.cancelButton.Text = resources.GetString("cancelButton.Text");
            this.cancelButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("cancelButton.TextAlign")));
            this.cancelButton.Visible = ((bool)(resources.GetObject("cancelButton.Visible")));
            // 
            // noButton
            // 
            this.noButton.AccessibleDescription = resources.GetString("noButton.AccessibleDescription");
            this.noButton.AccessibleName = resources.GetString("noButton.AccessibleName");
            this.noButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("noButton.Anchor")));
            this.noButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("noButton.BackgroundImage")));
            this.noButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("noButton.Dock")));
            this.noButton.Enabled = ((bool)(resources.GetObject("noButton.Enabled")));
            this.noButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("noButton.FlatStyle")));
            this.noButton.Font = ((System.Drawing.Font)(resources.GetObject("noButton.Font")));
            this.noButton.Image = ((System.Drawing.Image)(resources.GetObject("noButton.Image")));
            this.noButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("noButton.ImageAlign")));
            this.noButton.ImageIndex = ((int)(resources.GetObject("noButton.ImageIndex")));
            this.noButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("noButton.ImeMode")));
            this.noButton.Location = ((System.Drawing.Point)(resources.GetObject("noButton.Location")));
            this.noButton.Name = "noButton";
            this.noButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("noButton.RightToLeft")));
            this.noButton.Size = ((System.Drawing.Size)(resources.GetObject("noButton.Size")));
            this.noButton.TabIndex = ((int)(resources.GetObject("noButton.TabIndex")));
            this.noButton.Text = resources.GetString("noButton.Text");
            this.noButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("noButton.TextAlign")));
            this.noButton.Visible = ((bool)(resources.GetObject("noButton.Visible")));
            this.noButton.Click += new System.EventHandler(this.noButton_Click);
            // 
            // yesButton
            // 
            this.yesButton.AccessibleDescription = resources.GetString("yesButton.AccessibleDescription");
            this.yesButton.AccessibleName = resources.GetString("yesButton.AccessibleName");
            this.yesButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("yesButton.Anchor")));
            this.yesButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("yesButton.BackgroundImage")));
            this.yesButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("yesButton.Dock")));
            this.yesButton.Enabled = ((bool)(resources.GetObject("yesButton.Enabled")));
            this.yesButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("yesButton.FlatStyle")));
            this.yesButton.Font = ((System.Drawing.Font)(resources.GetObject("yesButton.Font")));
            this.yesButton.Image = ((System.Drawing.Image)(resources.GetObject("yesButton.Image")));
            this.yesButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("yesButton.ImageAlign")));
            this.yesButton.ImageIndex = ((int)(resources.GetObject("yesButton.ImageIndex")));
            this.yesButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("yesButton.ImeMode")));
            this.yesButton.Location = ((System.Drawing.Point)(resources.GetObject("yesButton.Location")));
            this.yesButton.Name = "yesButton";
            this.yesButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("yesButton.RightToLeft")));
            this.yesButton.Size = ((System.Drawing.Size)(resources.GetObject("yesButton.Size")));
            this.yesButton.TabIndex = ((int)(resources.GetObject("yesButton.TabIndex")));
            this.yesButton.Text = resources.GetString("yesButton.Text");
            this.yesButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("yesButton.TextAlign")));
            this.yesButton.Visible = ((bool)(resources.GetObject("yesButton.Visible")));
            this.yesButton.Click += new System.EventHandler(this.yesButton_Click);
            // 
            // saveItemsListBox
            // 
            this.saveItemsListBox.AccessibleDescription = resources.GetString("saveItemsListBox.AccessibleDescription");
            this.saveItemsListBox.AccessibleName = resources.GetString("saveItemsListBox.AccessibleName");
            this.saveItemsListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("saveItemsListBox.Anchor")));
            this.saveItemsListBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("saveItemsListBox.BackgroundImage")));
            this.saveItemsListBox.ColumnWidth = ((int)(resources.GetObject("saveItemsListBox.ColumnWidth")));
            this.saveItemsListBox.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("saveItemsListBox.Dock")));
            this.saveItemsListBox.Enabled = ((bool)(resources.GetObject("saveItemsListBox.Enabled")));
            this.saveItemsListBox.Font = ((System.Drawing.Font)(resources.GetObject("saveItemsListBox.Font")));
            this.saveItemsListBox.HorizontalExtent = ((int)(resources.GetObject("saveItemsListBox.HorizontalExtent")));
            this.saveItemsListBox.HorizontalScrollbar = ((bool)(resources.GetObject("saveItemsListBox.HorizontalScrollbar")));
            this.saveItemsListBox.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("saveItemsListBox.ImeMode")));
            this.saveItemsListBox.IntegralHeight = ((bool)(resources.GetObject("saveItemsListBox.IntegralHeight")));
            this.saveItemsListBox.ItemHeight = ((int)(resources.GetObject("saveItemsListBox.ItemHeight")));
            this.saveItemsListBox.Location = ((System.Drawing.Point)(resources.GetObject("saveItemsListBox.Location")));
            this.saveItemsListBox.Name = "saveItemsListBox";
            this.saveItemsListBox.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("saveItemsListBox.RightToLeft")));
            this.saveItemsListBox.ScrollAlwaysVisible = ((bool)(resources.GetObject("saveItemsListBox.ScrollAlwaysVisible")));
            this.saveItemsListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.saveItemsListBox.Size = ((System.Drawing.Size)(resources.GetObject("saveItemsListBox.Size")));
            this.saveItemsListBox.Sorted = true;
            this.saveItemsListBox.TabIndex = ((int)(resources.GetObject("saveItemsListBox.TabIndex")));
            this.saveItemsListBox.Visible = ((bool)(resources.GetObject("saveItemsListBox.Visible")));
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.AccessibleDescription = resources.GetString("descriptionLabel.AccessibleDescription");
            this.descriptionLabel.AccessibleName = resources.GetString("descriptionLabel.AccessibleName");
            this.descriptionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("descriptionLabel.Anchor")));
            this.descriptionLabel.AutoSize = ((bool)(resources.GetObject("descriptionLabel.AutoSize")));
            this.descriptionLabel.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("descriptionLabel.Dock")));
            this.descriptionLabel.Enabled = ((bool)(resources.GetObject("descriptionLabel.Enabled")));
            this.descriptionLabel.Font = ((System.Drawing.Font)(resources.GetObject("descriptionLabel.Font")));
            this.descriptionLabel.Image = ((System.Drawing.Image)(resources.GetObject("descriptionLabel.Image")));
            this.descriptionLabel.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("descriptionLabel.ImageAlign")));
            this.descriptionLabel.ImageIndex = ((int)(resources.GetObject("descriptionLabel.ImageIndex")));
            this.descriptionLabel.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("descriptionLabel.ImeMode")));
            this.descriptionLabel.Location = ((System.Drawing.Point)(resources.GetObject("descriptionLabel.Location")));
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("descriptionLabel.RightToLeft")));
            this.descriptionLabel.Size = ((System.Drawing.Size)(resources.GetObject("descriptionLabel.Size")));
            this.descriptionLabel.TabIndex = ((int)(resources.GetObject("descriptionLabel.TabIndex")));
            this.descriptionLabel.Text = resources.GetString("descriptionLabel.Text");
            this.descriptionLabel.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("descriptionLabel.TextAlign")));
            this.descriptionLabel.Visible = ((bool)(resources.GetObject("descriptionLabel.Visible")));
            // 
            // SaveChangesDialog
            // 
            this.AcceptButton = this.yesButton;
            this.AccessibleDescription = resources.GetString("$this.AccessibleDescription");
            this.AccessibleName = resources.GetString("$this.AccessibleName");
            this.AutoScaleBaseSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScaleBaseSize")));
            this.AutoScroll = ((bool)(resources.GetObject("$this.AutoScroll")));
            this.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMargin")));
            this.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMinSize")));
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.CancelButton = this.cancelButton;
            this.ClientSize = ((System.Drawing.Size)(resources.GetObject("$this.ClientSize")));
            this.Controls.Add(this.descriptionLabel);
            this.Controls.Add(this.saveItemsListBox);
            this.Controls.Add(this.yesButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.noButton);
            this.Enabled = ((bool)(resources.GetObject("$this.Enabled")));
            this.Font = ((System.Drawing.Font)(resources.GetObject("$this.Font")));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("$this.ImeMode")));
            this.Location = ((System.Drawing.Point)(resources.GetObject("$this.Location")));
            this.MaximizeBox = false;
            this.MaximumSize = ((System.Drawing.Size)(resources.GetObject("$this.MaximumSize")));
            this.MinimizeBox = false;
            this.MinimumSize = ((System.Drawing.Size)(resources.GetObject("$this.MinimumSize")));
            this.Name = "SaveChangesDialog";
            this.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("$this.RightToLeft")));
            this.ShowInTaskbar = false;
            this.StartPosition = ((System.Windows.Forms.FormStartPosition)(resources.GetObject("$this.StartPosition")));
            this.Text = resources.GetString("$this.Text");
            this.ResumeLayout(false);

        }
		#endregion
    }
}
