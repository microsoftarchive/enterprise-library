//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block QuickStart
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.ComponentModel;
using System.Windows.Forms;
using CachingQuickStart.Properties;

namespace CachingQuickStart
{
    /// <summary>
    /// Summary description for RetrieveItemForm.
    /// </summary>
    public class SelectItemForm : Form
    {
        private GroupBox groupBox1;
        private Button okButton;
        private Button cancelButton;
        private Label label1;
        private TextBox keyTextBox;
        private Label instructionLabel;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private Container components = null;

        public SelectItemForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(SelectItemForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.keyTextBox = new System.Windows.Forms.TextBox();
            this.instructionLabel = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.AccessibleDescription = resources.GetString("groupBox1.AccessibleDescription");
            this.groupBox1.AccessibleName = resources.GetString("groupBox1.AccessibleName");
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("groupBox1.Anchor")));
            this.groupBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("groupBox1.BackgroundImage")));
            this.groupBox1.Controls.Add(this.cancelButton);
            this.groupBox1.Controls.Add(this.okButton);
            this.groupBox1.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("groupBox1.Dock")));
            this.groupBox1.Enabled = ((bool)(resources.GetObject("groupBox1.Enabled")));
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Font = ((System.Drawing.Font)(resources.GetObject("groupBox1.Font")));
            this.groupBox1.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("groupBox1.ImeMode")));
            this.groupBox1.Location = ((System.Drawing.Point)(resources.GetObject("groupBox1.Location")));
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("groupBox1.RightToLeft")));
            this.groupBox1.Size = ((System.Drawing.Size)(resources.GetObject("groupBox1.Size")));
            this.groupBox1.TabIndex = ((int)(resources.GetObject("groupBox1.TabIndex")));
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = resources.GetString("groupBox1.Text");
            this.groupBox1.Visible = ((bool)(resources.GetObject("groupBox1.Visible")));
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
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // okButton
            // 
            this.okButton.AccessibleDescription = resources.GetString("okButton.AccessibleDescription");
            this.okButton.AccessibleName = resources.GetString("okButton.AccessibleName");
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("okButton.Anchor")));
            this.okButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("okButton.BackgroundImage")));
            this.okButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("okButton.Dock")));
            this.okButton.Enabled = ((bool)(resources.GetObject("okButton.Enabled")));
            this.okButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("okButton.FlatStyle")));
            this.okButton.Font = ((System.Drawing.Font)(resources.GetObject("okButton.Font")));
            this.okButton.Image = ((System.Drawing.Image)(resources.GetObject("okButton.Image")));
            this.okButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("okButton.ImageAlign")));
            this.okButton.ImageIndex = ((int)(resources.GetObject("okButton.ImageIndex")));
            this.okButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("okButton.ImeMode")));
            this.okButton.Location = ((System.Drawing.Point)(resources.GetObject("okButton.Location")));
            this.okButton.Name = "okButton";
            this.okButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("okButton.RightToLeft")));
            this.okButton.Size = ((System.Drawing.Size)(resources.GetObject("okButton.Size")));
            this.okButton.TabIndex = ((int)(resources.GetObject("okButton.TabIndex")));
            this.okButton.Text = resources.GetString("okButton.Text");
            this.okButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("okButton.TextAlign")));
            this.okButton.Visible = ((bool)(resources.GetObject("okButton.Visible")));
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // label1
            // 
            this.label1.AccessibleDescription = resources.GetString("label1.AccessibleDescription");
            this.label1.AccessibleName = resources.GetString("label1.AccessibleName");
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("label1.Anchor")));
            this.label1.AutoSize = ((bool)(resources.GetObject("label1.AutoSize")));
            this.label1.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("label1.Dock")));
            this.label1.Enabled = ((bool)(resources.GetObject("label1.Enabled")));
            this.label1.Font = ((System.Drawing.Font)(resources.GetObject("label1.Font")));
            this.label1.Image = ((System.Drawing.Image)(resources.GetObject("label1.Image")));
            this.label1.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label1.ImageAlign")));
            this.label1.ImageIndex = ((int)(resources.GetObject("label1.ImageIndex")));
            this.label1.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("label1.ImeMode")));
            this.label1.Location = ((System.Drawing.Point)(resources.GetObject("label1.Location")));
            this.label1.Name = "label1";
            this.label1.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("label1.RightToLeft")));
            this.label1.Size = ((System.Drawing.Size)(resources.GetObject("label1.Size")));
            this.label1.TabIndex = ((int)(resources.GetObject("label1.TabIndex")));
            this.label1.Text = resources.GetString("label1.Text");
            this.label1.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label1.TextAlign")));
            this.label1.Visible = ((bool)(resources.GetObject("label1.Visible")));
            // 
            // keyTextBox
            // 
            this.keyTextBox.AccessibleDescription = resources.GetString("keyTextBox.AccessibleDescription");
            this.keyTextBox.AccessibleName = resources.GetString("keyTextBox.AccessibleName");
            this.keyTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("keyTextBox.Anchor")));
            this.keyTextBox.AutoSize = ((bool)(resources.GetObject("keyTextBox.AutoSize")));
            this.keyTextBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("keyTextBox.BackgroundImage")));
            this.keyTextBox.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("keyTextBox.Dock")));
            this.keyTextBox.Enabled = ((bool)(resources.GetObject("keyTextBox.Enabled")));
            this.keyTextBox.Font = ((System.Drawing.Font)(resources.GetObject("keyTextBox.Font")));
            this.keyTextBox.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("keyTextBox.ImeMode")));
            this.keyTextBox.Location = ((System.Drawing.Point)(resources.GetObject("keyTextBox.Location")));
            this.keyTextBox.MaxLength = ((int)(resources.GetObject("keyTextBox.MaxLength")));
            this.keyTextBox.Multiline = ((bool)(resources.GetObject("keyTextBox.Multiline")));
            this.keyTextBox.Name = "keyTextBox";
            this.keyTextBox.PasswordChar = ((char)(resources.GetObject("keyTextBox.PasswordChar")));
            this.keyTextBox.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("keyTextBox.RightToLeft")));
            this.keyTextBox.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("keyTextBox.ScrollBars")));
            this.keyTextBox.Size = ((System.Drawing.Size)(resources.GetObject("keyTextBox.Size")));
            this.keyTextBox.TabIndex = ((int)(resources.GetObject("keyTextBox.TabIndex")));
            this.keyTextBox.Text = resources.GetString("keyTextBox.Text");
            this.keyTextBox.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("keyTextBox.TextAlign")));
            this.keyTextBox.Visible = ((bool)(resources.GetObject("keyTextBox.Visible")));
            this.keyTextBox.WordWrap = ((bool)(resources.GetObject("keyTextBox.WordWrap")));
            // 
            // instructionLabel
            // 
            this.instructionLabel.AccessibleDescription = resources.GetString("instructionLabel.AccessibleDescription");
            this.instructionLabel.AccessibleName = resources.GetString("instructionLabel.AccessibleName");
            this.instructionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("instructionLabel.Anchor")));
            this.instructionLabel.AutoSize = ((bool)(resources.GetObject("instructionLabel.AutoSize")));
            this.instructionLabel.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("instructionLabel.Dock")));
            this.instructionLabel.Enabled = ((bool)(resources.GetObject("instructionLabel.Enabled")));
            this.instructionLabel.Font = ((System.Drawing.Font)(resources.GetObject("instructionLabel.Font")));
            this.instructionLabel.Image = ((System.Drawing.Image)(resources.GetObject("instructionLabel.Image")));
            this.instructionLabel.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("instructionLabel.ImageAlign")));
            this.instructionLabel.ImageIndex = ((int)(resources.GetObject("instructionLabel.ImageIndex")));
            this.instructionLabel.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("instructionLabel.ImeMode")));
            this.instructionLabel.Location = ((System.Drawing.Point)(resources.GetObject("instructionLabel.Location")));
            this.instructionLabel.Name = "instructionLabel";
            this.instructionLabel.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("instructionLabel.RightToLeft")));
            this.instructionLabel.Size = ((System.Drawing.Size)(resources.GetObject("instructionLabel.Size")));
            this.instructionLabel.TabIndex = ((int)(resources.GetObject("instructionLabel.TabIndex")));
            this.instructionLabel.Text = resources.GetString("instructionLabel.Text");
            this.instructionLabel.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("instructionLabel.TextAlign")));
            this.instructionLabel.Visible = ((bool)(resources.GetObject("instructionLabel.Visible")));
            // 
            // SelectItemForm
            // 
            this.AcceptButton = this.okButton;
            this.AccessibleDescription = resources.GetString("$this.AccessibleDescription");
            this.AccessibleName = resources.GetString("$this.AccessibleName");
            this.AutoScroll = ((bool)(resources.GetObject("$this.AutoScroll")));
            this.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMargin")));
            this.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMinSize")));
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.CancelButton = this.cancelButton;
            this.ClientSize = ((System.Drawing.Size)(resources.GetObject("$this.ClientSize")));
            this.Controls.Add(this.instructionLabel);
            this.Controls.Add(this.keyTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
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
            this.Name = "SelectItemForm";
            this.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("$this.RightToLeft")));
            this.ShowInTaskbar = false;
            this.StartPosition = ((System.Windows.Forms.FormStartPosition)(resources.GetObject("$this.StartPosition")));
            this.Text = resources.GetString("$this.Text");
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            this.keyTextBox.Clear();
            this.keyTextBox.Focus();
        }

        /// <summary>
        /// Key of item selected by user to be read from cache.
        /// </summary>
        public string ItemKey
        {
            get { return this.keyTextBox.Text; }
        }

        public void SetInstructionLabelText(string text)
        {
            this.instructionLabel.Text = text;
        }

        public void ClearInputTextBox()
        {
            this.keyTextBox.Clear();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (this.keyTextBox.Text.CompareTo("") != 0)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show(Resources.InvalidKeyMessage, Resources.QuickStartTitleMessage, MessageBoxButtons.OK);
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }



    }
}