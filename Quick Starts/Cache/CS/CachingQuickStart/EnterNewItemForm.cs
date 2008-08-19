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
using Microsoft.Practices.EnterpriseLibrary.Caching;
using CachingQuickStart.Properties;

namespace CachingQuickStart
{
	// The list of expiration options the user is allowed to select
    public enum ExpirationType
    {
        AbsoluteTime = 0,
        FileDependency,
        SlidingTime,
        ExtendedFormat,
    }

    /// <summary>
    /// Summary description for EnterNewItemForm.
    /// </summary>
    public class EnterNewItemForm : Form
    {
        private GroupBox groupBox2;
        private Button cancelButton;
        private Button okButton;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private Container components = null;

        private ExpirationType expiration;
        private Label label10;
        private Label label9;
        private TextBox priceTextBox;
        private TextBox nameTextBox;
        private TextBox keyTextBox;
        private Label label7;
        private Label label6;
        private Label label5;
        private ComboBox priorityComboBox;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label label1;
        private ComboBox expirationComboBox;
        private DateTime absoluteTime;

        public EnterNewItemForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            InitializeControls();
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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(EnterNewItemForm));
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.priceTextBox = new System.Windows.Forms.TextBox();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.keyTextBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.priorityComboBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.expirationComboBox = new System.Windows.Forms.ComboBox();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.AccessibleDescription = resources.GetString("groupBox2.AccessibleDescription");
            this.groupBox2.AccessibleName = resources.GetString("groupBox2.AccessibleName");
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("groupBox2.Anchor")));
            this.groupBox2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("groupBox2.BackgroundImage")));
            this.groupBox2.Controls.Add(this.cancelButton);
            this.groupBox2.Controls.Add(this.okButton);
            this.groupBox2.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("groupBox2.Dock")));
            this.groupBox2.Enabled = ((bool)(resources.GetObject("groupBox2.Enabled")));
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Font = ((System.Drawing.Font)(resources.GetObject("groupBox2.Font")));
            this.groupBox2.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("groupBox2.ImeMode")));
            this.groupBox2.Location = ((System.Drawing.Point)(resources.GetObject("groupBox2.Location")));
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("groupBox2.RightToLeft")));
            this.groupBox2.Size = ((System.Drawing.Size)(resources.GetObject("groupBox2.Size")));
            this.groupBox2.TabIndex = ((int)(resources.GetObject("groupBox2.TabIndex")));
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = resources.GetString("groupBox2.Text");
            this.groupBox2.Visible = ((bool)(resources.GetObject("groupBox2.Visible")));
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
            // label10
            // 
            this.label10.AccessibleDescription = resources.GetString("label10.AccessibleDescription");
            this.label10.AccessibleName = resources.GetString("label10.AccessibleName");
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("label10.Anchor")));
            this.label10.AutoSize = ((bool)(resources.GetObject("label10.AutoSize")));
            this.label10.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("label10.Dock")));
            this.label10.Enabled = ((bool)(resources.GetObject("label10.Enabled")));
            this.label10.Font = ((System.Drawing.Font)(resources.GetObject("label10.Font")));
            this.label10.Image = ((System.Drawing.Image)(resources.GetObject("label10.Image")));
            this.label10.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label10.ImageAlign")));
            this.label10.ImageIndex = ((int)(resources.GetObject("label10.ImageIndex")));
            this.label10.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("label10.ImeMode")));
            this.label10.Location = ((System.Drawing.Point)(resources.GetObject("label10.Location")));
            this.label10.Name = "label10";
            this.label10.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("label10.RightToLeft")));
            this.label10.Size = ((System.Drawing.Size)(resources.GetObject("label10.Size")));
            this.label10.TabIndex = ((int)(resources.GetObject("label10.TabIndex")));
            this.label10.Text = resources.GetString("label10.Text");
            this.label10.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label10.TextAlign")));
            this.label10.Visible = ((bool)(resources.GetObject("label10.Visible")));
            // 
            // label9
            // 
            this.label9.AccessibleDescription = resources.GetString("label9.AccessibleDescription");
            this.label9.AccessibleName = resources.GetString("label9.AccessibleName");
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("label9.Anchor")));
            this.label9.AutoSize = ((bool)(resources.GetObject("label9.AutoSize")));
            this.label9.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("label9.Dock")));
            this.label9.Enabled = ((bool)(resources.GetObject("label9.Enabled")));
            this.label9.Font = ((System.Drawing.Font)(resources.GetObject("label9.Font")));
            this.label9.Image = ((System.Drawing.Image)(resources.GetObject("label9.Image")));
            this.label9.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label9.ImageAlign")));
            this.label9.ImageIndex = ((int)(resources.GetObject("label9.ImageIndex")));
            this.label9.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("label9.ImeMode")));
            this.label9.Location = ((System.Drawing.Point)(resources.GetObject("label9.Location")));
            this.label9.Name = "label9";
            this.label9.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("label9.RightToLeft")));
            this.label9.Size = ((System.Drawing.Size)(resources.GetObject("label9.Size")));
            this.label9.TabIndex = ((int)(resources.GetObject("label9.TabIndex")));
            this.label9.Text = resources.GetString("label9.Text");
            this.label9.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label9.TextAlign")));
            this.label9.Visible = ((bool)(resources.GetObject("label9.Visible")));
            // 
            // priceTextBox
            // 
            this.priceTextBox.AccessibleDescription = resources.GetString("priceTextBox.AccessibleDescription");
            this.priceTextBox.AccessibleName = resources.GetString("priceTextBox.AccessibleName");
            this.priceTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("priceTextBox.Anchor")));
            this.priceTextBox.AutoSize = ((bool)(resources.GetObject("priceTextBox.AutoSize")));
            this.priceTextBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("priceTextBox.BackgroundImage")));
            this.priceTextBox.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("priceTextBox.Dock")));
            this.priceTextBox.Enabled = ((bool)(resources.GetObject("priceTextBox.Enabled")));
            this.priceTextBox.Font = ((System.Drawing.Font)(resources.GetObject("priceTextBox.Font")));
            this.priceTextBox.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("priceTextBox.ImeMode")));
            this.priceTextBox.Location = ((System.Drawing.Point)(resources.GetObject("priceTextBox.Location")));
            this.priceTextBox.MaxLength = ((int)(resources.GetObject("priceTextBox.MaxLength")));
            this.priceTextBox.Multiline = ((bool)(resources.GetObject("priceTextBox.Multiline")));
            this.priceTextBox.Name = "priceTextBox";
            this.priceTextBox.PasswordChar = ((char)(resources.GetObject("priceTextBox.PasswordChar")));
            this.priceTextBox.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("priceTextBox.RightToLeft")));
            this.priceTextBox.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("priceTextBox.ScrollBars")));
            this.priceTextBox.Size = ((System.Drawing.Size)(resources.GetObject("priceTextBox.Size")));
            this.priceTextBox.TabIndex = ((int)(resources.GetObject("priceTextBox.TabIndex")));
            this.priceTextBox.Text = resources.GetString("priceTextBox.Text");
            this.priceTextBox.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("priceTextBox.TextAlign")));
            this.priceTextBox.Visible = ((bool)(resources.GetObject("priceTextBox.Visible")));
            this.priceTextBox.WordWrap = ((bool)(resources.GetObject("priceTextBox.WordWrap")));
            // 
            // nameTextBox
            // 
            this.nameTextBox.AccessibleDescription = resources.GetString("nameTextBox.AccessibleDescription");
            this.nameTextBox.AccessibleName = resources.GetString("nameTextBox.AccessibleName");
            this.nameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("nameTextBox.Anchor")));
            this.nameTextBox.AutoSize = ((bool)(resources.GetObject("nameTextBox.AutoSize")));
            this.nameTextBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("nameTextBox.BackgroundImage")));
            this.nameTextBox.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("nameTextBox.Dock")));
            this.nameTextBox.Enabled = ((bool)(resources.GetObject("nameTextBox.Enabled")));
            this.nameTextBox.Font = ((System.Drawing.Font)(resources.GetObject("nameTextBox.Font")));
            this.nameTextBox.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("nameTextBox.ImeMode")));
            this.nameTextBox.Location = ((System.Drawing.Point)(resources.GetObject("nameTextBox.Location")));
            this.nameTextBox.MaxLength = ((int)(resources.GetObject("nameTextBox.MaxLength")));
            this.nameTextBox.Multiline = ((bool)(resources.GetObject("nameTextBox.Multiline")));
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.PasswordChar = ((char)(resources.GetObject("nameTextBox.PasswordChar")));
            this.nameTextBox.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("nameTextBox.RightToLeft")));
            this.nameTextBox.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("nameTextBox.ScrollBars")));
            this.nameTextBox.Size = ((System.Drawing.Size)(resources.GetObject("nameTextBox.Size")));
            this.nameTextBox.TabIndex = ((int)(resources.GetObject("nameTextBox.TabIndex")));
            this.nameTextBox.Text = resources.GetString("nameTextBox.Text");
            this.nameTextBox.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("nameTextBox.TextAlign")));
            this.nameTextBox.Visible = ((bool)(resources.GetObject("nameTextBox.Visible")));
            this.nameTextBox.WordWrap = ((bool)(resources.GetObject("nameTextBox.WordWrap")));
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
            // label7
            // 
            this.label7.AccessibleDescription = resources.GetString("label7.AccessibleDescription");
            this.label7.AccessibleName = resources.GetString("label7.AccessibleName");
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("label7.Anchor")));
            this.label7.AutoSize = ((bool)(resources.GetObject("label7.AutoSize")));
            this.label7.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("label7.Dock")));
            this.label7.Enabled = ((bool)(resources.GetObject("label7.Enabled")));
            this.label7.Font = ((System.Drawing.Font)(resources.GetObject("label7.Font")));
            this.label7.Image = ((System.Drawing.Image)(resources.GetObject("label7.Image")));
            this.label7.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label7.ImageAlign")));
            this.label7.ImageIndex = ((int)(resources.GetObject("label7.ImageIndex")));
            this.label7.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("label7.ImeMode")));
            this.label7.Location = ((System.Drawing.Point)(resources.GetObject("label7.Location")));
            this.label7.Name = "label7";
            this.label7.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("label7.RightToLeft")));
            this.label7.Size = ((System.Drawing.Size)(resources.GetObject("label7.Size")));
            this.label7.TabIndex = ((int)(resources.GetObject("label7.TabIndex")));
            this.label7.Text = resources.GetString("label7.Text");
            this.label7.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label7.TextAlign")));
            this.label7.Visible = ((bool)(resources.GetObject("label7.Visible")));
            // 
            // label6
            // 
            this.label6.AccessibleDescription = resources.GetString("label6.AccessibleDescription");
            this.label6.AccessibleName = resources.GetString("label6.AccessibleName");
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("label6.Anchor")));
            this.label6.AutoSize = ((bool)(resources.GetObject("label6.AutoSize")));
            this.label6.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("label6.Dock")));
            this.label6.Enabled = ((bool)(resources.GetObject("label6.Enabled")));
            this.label6.Font = ((System.Drawing.Font)(resources.GetObject("label6.Font")));
            this.label6.Image = ((System.Drawing.Image)(resources.GetObject("label6.Image")));
            this.label6.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label6.ImageAlign")));
            this.label6.ImageIndex = ((int)(resources.GetObject("label6.ImageIndex")));
            this.label6.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("label6.ImeMode")));
            this.label6.Location = ((System.Drawing.Point)(resources.GetObject("label6.Location")));
            this.label6.Name = "label6";
            this.label6.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("label6.RightToLeft")));
            this.label6.Size = ((System.Drawing.Size)(resources.GetObject("label6.Size")));
            this.label6.TabIndex = ((int)(resources.GetObject("label6.TabIndex")));
            this.label6.Text = resources.GetString("label6.Text");
            this.label6.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label6.TextAlign")));
            this.label6.Visible = ((bool)(resources.GetObject("label6.Visible")));
            // 
            // label5
            // 
            this.label5.AccessibleDescription = resources.GetString("label5.AccessibleDescription");
            this.label5.AccessibleName = resources.GetString("label5.AccessibleName");
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("label5.Anchor")));
            this.label5.AutoSize = ((bool)(resources.GetObject("label5.AutoSize")));
            this.label5.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("label5.Dock")));
            this.label5.Enabled = ((bool)(resources.GetObject("label5.Enabled")));
            this.label5.Font = ((System.Drawing.Font)(resources.GetObject("label5.Font")));
            this.label5.Image = ((System.Drawing.Image)(resources.GetObject("label5.Image")));
            this.label5.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label5.ImageAlign")));
            this.label5.ImageIndex = ((int)(resources.GetObject("label5.ImageIndex")));
            this.label5.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("label5.ImeMode")));
            this.label5.Location = ((System.Drawing.Point)(resources.GetObject("label5.Location")));
            this.label5.Name = "label5";
            this.label5.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("label5.RightToLeft")));
            this.label5.Size = ((System.Drawing.Size)(resources.GetObject("label5.Size")));
            this.label5.TabIndex = ((int)(resources.GetObject("label5.TabIndex")));
            this.label5.Text = resources.GetString("label5.Text");
            this.label5.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label5.TextAlign")));
            this.label5.Visible = ((bool)(resources.GetObject("label5.Visible")));
            // 
            // priorityComboBox
            // 
            this.priorityComboBox.AccessibleDescription = resources.GetString("priorityComboBox.AccessibleDescription");
            this.priorityComboBox.AccessibleName = resources.GetString("priorityComboBox.AccessibleName");
            this.priorityComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("priorityComboBox.Anchor")));
            this.priorityComboBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("priorityComboBox.BackgroundImage")));
            this.priorityComboBox.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("priorityComboBox.Dock")));
            this.priorityComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.priorityComboBox.Enabled = ((bool)(resources.GetObject("priorityComboBox.Enabled")));
            this.priorityComboBox.Font = ((System.Drawing.Font)(resources.GetObject("priorityComboBox.Font")));
            this.priorityComboBox.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("priorityComboBox.ImeMode")));
            this.priorityComboBox.IntegralHeight = ((bool)(resources.GetObject("priorityComboBox.IntegralHeight")));
            this.priorityComboBox.ItemHeight = ((int)(resources.GetObject("priorityComboBox.ItemHeight")));
            this.priorityComboBox.Items.AddRange(new object[] {
                                                                  resources.GetString("priorityComboBox.Items"),
                                                                  resources.GetString("priorityComboBox.Items1"),
                                                                  resources.GetString("priorityComboBox.Items2"),
                                                                  resources.GetString("priorityComboBox.Items3")});
            this.priorityComboBox.Location = ((System.Drawing.Point)(resources.GetObject("priorityComboBox.Location")));
            this.priorityComboBox.MaxDropDownItems = ((int)(resources.GetObject("priorityComboBox.MaxDropDownItems")));
            this.priorityComboBox.MaxLength = ((int)(resources.GetObject("priorityComboBox.MaxLength")));
            this.priorityComboBox.Name = "priorityComboBox";
            this.priorityComboBox.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("priorityComboBox.RightToLeft")));
            this.priorityComboBox.Size = ((System.Drawing.Size)(resources.GetObject("priorityComboBox.Size")));
            this.priorityComboBox.TabIndex = ((int)(resources.GetObject("priorityComboBox.TabIndex")));
            this.priorityComboBox.Text = resources.GetString("priorityComboBox.Text");
            this.priorityComboBox.Visible = ((bool)(resources.GetObject("priorityComboBox.Visible")));
            // 
            // label4
            // 
            this.label4.AccessibleDescription = resources.GetString("label4.AccessibleDescription");
            this.label4.AccessibleName = resources.GetString("label4.AccessibleName");
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("label4.Anchor")));
            this.label4.AutoSize = ((bool)(resources.GetObject("label4.AutoSize")));
            this.label4.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("label4.Dock")));
            this.label4.Enabled = ((bool)(resources.GetObject("label4.Enabled")));
            this.label4.Font = ((System.Drawing.Font)(resources.GetObject("label4.Font")));
            this.label4.Image = ((System.Drawing.Image)(resources.GetObject("label4.Image")));
            this.label4.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label4.ImageAlign")));
            this.label4.ImageIndex = ((int)(resources.GetObject("label4.ImageIndex")));
            this.label4.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("label4.ImeMode")));
            this.label4.Location = ((System.Drawing.Point)(resources.GetObject("label4.Location")));
            this.label4.Name = "label4";
            this.label4.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("label4.RightToLeft")));
            this.label4.Size = ((System.Drawing.Size)(resources.GetObject("label4.Size")));
            this.label4.TabIndex = ((int)(resources.GetObject("label4.TabIndex")));
            this.label4.Text = resources.GetString("label4.Text");
            this.label4.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label4.TextAlign")));
            this.label4.Visible = ((bool)(resources.GetObject("label4.Visible")));
            // 
            // label3
            // 
            this.label3.AccessibleDescription = resources.GetString("label3.AccessibleDescription");
            this.label3.AccessibleName = resources.GetString("label3.AccessibleName");
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("label3.Anchor")));
            this.label3.AutoSize = ((bool)(resources.GetObject("label3.AutoSize")));
            this.label3.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("label3.Dock")));
            this.label3.Enabled = ((bool)(resources.GetObject("label3.Enabled")));
            this.label3.Font = ((System.Drawing.Font)(resources.GetObject("label3.Font")));
            this.label3.Image = ((System.Drawing.Image)(resources.GetObject("label3.Image")));
            this.label3.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label3.ImageAlign")));
            this.label3.ImageIndex = ((int)(resources.GetObject("label3.ImageIndex")));
            this.label3.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("label3.ImeMode")));
            this.label3.Location = ((System.Drawing.Point)(resources.GetObject("label3.Location")));
            this.label3.Name = "label3";
            this.label3.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("label3.RightToLeft")));
            this.label3.Size = ((System.Drawing.Size)(resources.GetObject("label3.Size")));
            this.label3.TabIndex = ((int)(resources.GetObject("label3.TabIndex")));
            this.label3.Text = resources.GetString("label3.Text");
            this.label3.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label3.TextAlign")));
            this.label3.Visible = ((bool)(resources.GetObject("label3.Visible")));
            // 
            // label2
            // 
            this.label2.AccessibleDescription = resources.GetString("label2.AccessibleDescription");
            this.label2.AccessibleName = resources.GetString("label2.AccessibleName");
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("label2.Anchor")));
            this.label2.AutoSize = ((bool)(resources.GetObject("label2.AutoSize")));
            this.label2.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("label2.Dock")));
            this.label2.Enabled = ((bool)(resources.GetObject("label2.Enabled")));
            this.label2.Font = ((System.Drawing.Font)(resources.GetObject("label2.Font")));
            this.label2.Image = ((System.Drawing.Image)(resources.GetObject("label2.Image")));
            this.label2.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label2.ImageAlign")));
            this.label2.ImageIndex = ((int)(resources.GetObject("label2.ImageIndex")));
            this.label2.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("label2.ImeMode")));
            this.label2.Location = ((System.Drawing.Point)(resources.GetObject("label2.Location")));
            this.label2.Name = "label2";
            this.label2.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("label2.RightToLeft")));
            this.label2.Size = ((System.Drawing.Size)(resources.GetObject("label2.Size")));
            this.label2.TabIndex = ((int)(resources.GetObject("label2.TabIndex")));
            this.label2.Text = resources.GetString("label2.Text");
            this.label2.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label2.TextAlign")));
            this.label2.Visible = ((bool)(resources.GetObject("label2.Visible")));
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
            // expirationComboBox
            // 
            this.expirationComboBox.AccessibleDescription = resources.GetString("expirationComboBox.AccessibleDescription");
            this.expirationComboBox.AccessibleName = resources.GetString("expirationComboBox.AccessibleName");
            this.expirationComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("expirationComboBox.Anchor")));
            this.expirationComboBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("expirationComboBox.BackgroundImage")));
            this.expirationComboBox.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("expirationComboBox.Dock")));
            this.expirationComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.expirationComboBox.Enabled = ((bool)(resources.GetObject("expirationComboBox.Enabled")));
            this.expirationComboBox.Font = ((System.Drawing.Font)(resources.GetObject("expirationComboBox.Font")));
            this.expirationComboBox.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("expirationComboBox.ImeMode")));
            this.expirationComboBox.IntegralHeight = ((bool)(resources.GetObject("expirationComboBox.IntegralHeight")));
            this.expirationComboBox.ItemHeight = ((int)(resources.GetObject("expirationComboBox.ItemHeight")));
            this.expirationComboBox.Items.AddRange(new object[] {
                                                                    resources.GetString("expirationComboBox.Items"),
                                                                    resources.GetString("expirationComboBox.Items1"),
                                                                    resources.GetString("expirationComboBox.Items2"),
                                                                    resources.GetString("expirationComboBox.Items3")});
            this.expirationComboBox.Location = ((System.Drawing.Point)(resources.GetObject("expirationComboBox.Location")));
            this.expirationComboBox.MaxDropDownItems = ((int)(resources.GetObject("expirationComboBox.MaxDropDownItems")));
            this.expirationComboBox.MaxLength = ((int)(resources.GetObject("expirationComboBox.MaxLength")));
            this.expirationComboBox.Name = "expirationComboBox";
            this.expirationComboBox.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("expirationComboBox.RightToLeft")));
            this.expirationComboBox.Size = ((System.Drawing.Size)(resources.GetObject("expirationComboBox.Size")));
            this.expirationComboBox.TabIndex = ((int)(resources.GetObject("expirationComboBox.TabIndex")));
            this.expirationComboBox.Text = resources.GetString("expirationComboBox.Text");
            this.expirationComboBox.Visible = ((bool)(resources.GetObject("expirationComboBox.Visible")));
            this.expirationComboBox.SelectedIndexChanged += new System.EventHandler(this.expirationComboBox_SelectedIndexChanged);
            // 
            // EnterNewItemForm
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
            this.Controls.Add(this.expirationComboBox);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.priceTextBox);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.keyTextBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.priorityComboBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox2);
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
            this.Name = "EnterNewItemForm";
            this.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("$this.RightToLeft")));
            this.ShowInTaskbar = false;
            this.StartPosition = ((System.Windows.Forms.FormStartPosition)(resources.GetObject("$this.StartPosition")));
            this.Text = resources.GetString("$this.Text");
            this.Load += new System.EventHandler(this.EnterNewItemForm_Load);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public ExpirationType Expiration
        {
            get { return this.expiration; }
        }

        public CacheItemPriority Priority
        {
            get { return (CacheItemPriority) (this.priorityComboBox.SelectedIndex + 1); }
        }

        public DateTime AbsoluteTime
        {
            get { return this.absoluteTime; }
        }

        private void InitializeControls()
        {
            this.priorityComboBox.SelectedIndex = 1;

			// The form defaults to AbsoluteTime as the expiration for new items
			this.expirationComboBox.SelectedIndex = 0;
			this.expiration = ExpirationType.AbsoluteTime;
        }

        protected override void OnActivated(EventArgs e)
        {
			// Clear the fields and set focus to the cache key text box
            base.OnActivated(e);
         }

        public string ProductID
        {
            get { return this.keyTextBox.Text; }
        }

		public string ProductShortName
        {
            get { return this.nameTextBox.Text; }
        }

        public double ProductPrice
        {
            get { return Convert.ToDouble(this.priceTextBox.Text); }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (this.ValidateInput())
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show(Resources.InvalidInputMessage, Resources.QuickStartTitleMessage, MessageBoxButtons.OK);
            }
        }

        private bool ValidateInput()
        {
            bool result = (this.keyTextBox.Text.CompareTo("") != 0);
            result &= (this.nameTextBox.Text.CompareTo("") != 0);
            result &= (this.priceTextBox.Text.CompareTo("") != 0);
            return result;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

		/// <summary>
		/// Set the expirate enumeration as the user selects different expiration options
		/// </summary>
        private void expirationComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.expirationComboBox.SelectedIndex)
            {
                case 0:
                    this.expiration = ExpirationType.AbsoluteTime;
                    break;
                case 1:
                    this.expiration = ExpirationType.SlidingTime;
                    break;
                case 2:
                    this.expiration = ExpirationType.ExtendedFormat;
                    break;
                case 3:
                    this.expiration = ExpirationType.FileDependency;
                    break;
            }
        }

		private void EnterNewItemForm_Load(object sender, System.EventArgs e)
		{
			// Pick a pre-selected absolute expiration time, one day from the current
			// current date and time.
			this.absoluteTime = DateTime.Now + TimeSpan.FromMinutes(1);
 
			// Set the description in the combobox to display the resulting absolute time
			this.expirationComboBox.Items[0] = "AbsoluteTime - " + 
				this.absoluteTime.ToShortDateString() + " " + 
				this.absoluteTime.ToShortTimeString();

			this.keyTextBox.Focus();
			this.keyTextBox.Clear();
			this.nameTextBox.Clear();
			this.priceTextBox.Clear();
		}
    }
}