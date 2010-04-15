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
using System.ComponentModel;
using System.Security.Principal;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Security;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Editors.RuleEditor;
using System.Globalization;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design
{
    /// <summary>
    /// Provides a dialog box for editing identity
    /// role rule expressions.
    /// </summary>
    /// <summary>
    /// Provides a dialog box for editing identity
    /// role rule expressions.
    /// </summary>
    internal class ExpressionEditorFormUI : Form
    {
        private ToolBarButton addButton;
        private ToolBarButton toolBarButton1;
        private Button okButton;
        private Button cancelButton;
        private TextBox roleTextBox;
        private Label label1;
        private Label identityLabel;
        private CheckBox authenticationCheckBox;
        private TextBox identityTextBox;
        private Button evaluateButton;
        private Button roleButton;
        private Button identityButton;
        private Button closeGroupButton;
        private Button openGroupButton;
        private Button notButton;
        private Button orButton;
        private Button andButton;
        private ExpressionTextBox expressionTextBox;
        private GroupBox groupBox2;
        private GroupBox groupBox3;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private Container components = null;

        private TextBox validationResultBox;
        private GroupBox groupBox4;
        private TextBox ruleNameBox;
        private StatusBar statusBar;
        private StatusBarPanel statusPanel;
        private Button anonymousButton;
        private Parser parser = new Parser();

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="ExpressionEditorFormUI"/> class.
        /// </summary>
        public ExpressionEditorFormUI()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the expression that 
        /// is currently being edited.
        /// </summary>
        /// <value>An expression</value>
        public string Expression
        {
            get { return this.expressionTextBox.Text; }
            set
            {
                if (value == null)
                {
                    this.expressionTextBox.Text = String.Empty;
                }
                else
                {
                    this.expressionTextBox.Text = value;
                    this.expressionTextBox.HighlightText();
                }
            }
        }

        public string RuleName
        {
            get { return ruleNameBox.Text; }
            set { ruleNameBox.Text = value; }
        }

        protected override void OnLoad(EventArgs e)
        {
            this.expressionTextBox.InitParser();
            base.OnLoad(e);
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true to release both 
        /// managed and unmanaged resources; 
        /// false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.components != null)
                {
                    this.components.Dispose();
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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ExpressionEditorFormUI));
            this.addButton = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton1 = new System.Windows.Forms.ToolBarButton();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.roleTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.authenticationCheckBox = new System.Windows.Forms.CheckBox();
            this.identityTextBox = new System.Windows.Forms.TextBox();
            this.identityLabel = new System.Windows.Forms.Label();
            this.evaluateButton = new System.Windows.Forms.Button();
            this.roleButton = new System.Windows.Forms.Button();
            this.identityButton = new System.Windows.Forms.Button();
            this.closeGroupButton = new System.Windows.Forms.Button();
            this.openGroupButton = new System.Windows.Forms.Button();
            this.notButton = new System.Windows.Forms.Button();
            this.orButton = new System.Windows.Forms.Button();
            this.andButton = new System.Windows.Forms.Button();
            this.expressionTextBox = new Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design.ExpressionTextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.anonymousButton = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.validationResultBox = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.ruleNameBox = new System.Windows.Forms.TextBox();
            this.statusBar = new System.Windows.Forms.StatusBar();
            this.statusPanel = new System.Windows.Forms.StatusBarPanel();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.statusPanel)).BeginInit();
            this.SuspendLayout();
            // 
            // addButton
            // 
            this.addButton.Enabled = ((bool)(resources.GetObject("addButton.Enabled")));
            this.addButton.ImageIndex = ((int)(resources.GetObject("addButton.ImageIndex")));
            this.addButton.Text = resources.GetString("addButton.Text");
            this.addButton.ToolTipText = resources.GetString("addButton.ToolTipText");
            this.addButton.Visible = ((bool)(resources.GetObject("addButton.Visible")));
            // 
            // toolBarButton1
            // 
            this.toolBarButton1.Enabled = ((bool)(resources.GetObject("toolBarButton1.Enabled")));
            this.toolBarButton1.ImageIndex = ((int)(resources.GetObject("toolBarButton1.ImageIndex")));
            this.toolBarButton1.Text = resources.GetString("toolBarButton1.Text");
            this.toolBarButton1.ToolTipText = resources.GetString("toolBarButton1.ToolTipText");
            this.toolBarButton1.Visible = ((bool)(resources.GetObject("toolBarButton1.Visible")));
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
            this.okButton.Click += new System.EventHandler(this.HandleOkButtonClick);
            // 
            // cancelButton
            // 
            this.cancelButton.AccessibleDescription = resources.GetString("cancelButton.AccessibleDescription");
            this.cancelButton.AccessibleName = resources.GetString("cancelButton.AccessibleName");
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("cancelButton.Anchor")));
            this.cancelButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("cancelButton.BackgroundImage")));
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
            this.cancelButton.Click += new System.EventHandler(this.HandleCancelButtonClick);
            // 
            // roleTextBox
            // 
            this.roleTextBox.AcceptsReturn = true;
            this.roleTextBox.AccessibleDescription = resources.GetString("roleTextBox.AccessibleDescription");
            this.roleTextBox.AccessibleName = resources.GetString("roleTextBox.AccessibleName");
            this.roleTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("roleTextBox.Anchor")));
            this.roleTextBox.AutoSize = ((bool)(resources.GetObject("roleTextBox.AutoSize")));
            this.roleTextBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("roleTextBox.BackgroundImage")));
            this.roleTextBox.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("roleTextBox.Dock")));
            this.roleTextBox.Enabled = ((bool)(resources.GetObject("roleTextBox.Enabled")));
            this.roleTextBox.Font = ((System.Drawing.Font)(resources.GetObject("roleTextBox.Font")));
            this.roleTextBox.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("roleTextBox.ImeMode")));
            this.roleTextBox.Location = ((System.Drawing.Point)(resources.GetObject("roleTextBox.Location")));
            this.roleTextBox.MaxLength = ((int)(resources.GetObject("roleTextBox.MaxLength")));
            this.roleTextBox.Multiline = ((bool)(resources.GetObject("roleTextBox.Multiline")));
            this.roleTextBox.Name = "roleTextBox";
            this.roleTextBox.PasswordChar = ((char)(resources.GetObject("roleTextBox.PasswordChar")));
            this.roleTextBox.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("roleTextBox.RightToLeft")));
            this.roleTextBox.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("roleTextBox.ScrollBars")));
            this.roleTextBox.Size = ((System.Drawing.Size)(resources.GetObject("roleTextBox.Size")));
            this.roleTextBox.TabIndex = ((int)(resources.GetObject("roleTextBox.TabIndex")));
            this.roleTextBox.Text = resources.GetString("roleTextBox.Text");
            this.roleTextBox.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("roleTextBox.TextAlign")));
            this.roleTextBox.Visible = ((bool)(resources.GetObject("roleTextBox.Visible")));
            this.roleTextBox.WordWrap = ((bool)(resources.GetObject("roleTextBox.WordWrap")));
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
            // authenticationCheckBox
            // 
            this.authenticationCheckBox.AccessibleDescription = resources.GetString("authenticationCheckBox.AccessibleDescription");
            this.authenticationCheckBox.AccessibleName = resources.GetString("authenticationCheckBox.AccessibleName");
            this.authenticationCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("authenticationCheckBox.Anchor")));
            this.authenticationCheckBox.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("authenticationCheckBox.Appearance")));
            this.authenticationCheckBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("authenticationCheckBox.BackgroundImage")));
            this.authenticationCheckBox.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("authenticationCheckBox.CheckAlign")));
            this.authenticationCheckBox.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("authenticationCheckBox.Dock")));
            this.authenticationCheckBox.Enabled = ((bool)(resources.GetObject("authenticationCheckBox.Enabled")));
            this.authenticationCheckBox.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("authenticationCheckBox.FlatStyle")));
            this.authenticationCheckBox.Font = ((System.Drawing.Font)(resources.GetObject("authenticationCheckBox.Font")));
            this.authenticationCheckBox.Image = ((System.Drawing.Image)(resources.GetObject("authenticationCheckBox.Image")));
            this.authenticationCheckBox.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("authenticationCheckBox.ImageAlign")));
            this.authenticationCheckBox.ImageIndex = ((int)(resources.GetObject("authenticationCheckBox.ImageIndex")));
            this.authenticationCheckBox.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("authenticationCheckBox.ImeMode")));
            this.authenticationCheckBox.Location = ((System.Drawing.Point)(resources.GetObject("authenticationCheckBox.Location")));
            this.authenticationCheckBox.Name = "authenticationCheckBox";
            this.authenticationCheckBox.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("authenticationCheckBox.RightToLeft")));
            this.authenticationCheckBox.Size = ((System.Drawing.Size)(resources.GetObject("authenticationCheckBox.Size")));
            this.authenticationCheckBox.TabIndex = ((int)(resources.GetObject("authenticationCheckBox.TabIndex")));
            this.authenticationCheckBox.Text = resources.GetString("authenticationCheckBox.Text");
            this.authenticationCheckBox.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("authenticationCheckBox.TextAlign")));
            this.authenticationCheckBox.Visible = ((bool)(resources.GetObject("authenticationCheckBox.Visible")));
            // 
            // identityTextBox
            // 
            this.identityTextBox.AccessibleDescription = resources.GetString("identityTextBox.AccessibleDescription");
            this.identityTextBox.AccessibleName = resources.GetString("identityTextBox.AccessibleName");
            this.identityTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("identityTextBox.Anchor")));
            this.identityTextBox.AutoSize = ((bool)(resources.GetObject("identityTextBox.AutoSize")));
            this.identityTextBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("identityTextBox.BackgroundImage")));
            this.identityTextBox.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("identityTextBox.Dock")));
            this.identityTextBox.Enabled = ((bool)(resources.GetObject("identityTextBox.Enabled")));
            this.identityTextBox.Font = ((System.Drawing.Font)(resources.GetObject("identityTextBox.Font")));
            this.identityTextBox.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("identityTextBox.ImeMode")));
            this.identityTextBox.Location = ((System.Drawing.Point)(resources.GetObject("identityTextBox.Location")));
            this.identityTextBox.MaxLength = ((int)(resources.GetObject("identityTextBox.MaxLength")));
            this.identityTextBox.Multiline = ((bool)(resources.GetObject("identityTextBox.Multiline")));
            this.identityTextBox.Name = "identityTextBox";
            this.identityTextBox.PasswordChar = ((char)(resources.GetObject("identityTextBox.PasswordChar")));
            this.identityTextBox.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("identityTextBox.RightToLeft")));
            this.identityTextBox.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("identityTextBox.ScrollBars")));
            this.identityTextBox.Size = ((System.Drawing.Size)(resources.GetObject("identityTextBox.Size")));
            this.identityTextBox.TabIndex = ((int)(resources.GetObject("identityTextBox.TabIndex")));
            this.identityTextBox.Text = resources.GetString("identityTextBox.Text");
            this.identityTextBox.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("identityTextBox.TextAlign")));
            this.identityTextBox.Visible = ((bool)(resources.GetObject("identityTextBox.Visible")));
            this.identityTextBox.WordWrap = ((bool)(resources.GetObject("identityTextBox.WordWrap")));
            // 
            // identityLabel
            // 
            this.identityLabel.AccessibleDescription = resources.GetString("identityLabel.AccessibleDescription");
            this.identityLabel.AccessibleName = resources.GetString("identityLabel.AccessibleName");
            this.identityLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("identityLabel.Anchor")));
            this.identityLabel.AutoSize = ((bool)(resources.GetObject("identityLabel.AutoSize")));
            this.identityLabel.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("identityLabel.Dock")));
            this.identityLabel.Enabled = ((bool)(resources.GetObject("identityLabel.Enabled")));
            this.identityLabel.Font = ((System.Drawing.Font)(resources.GetObject("identityLabel.Font")));
            this.identityLabel.Image = ((System.Drawing.Image)(resources.GetObject("identityLabel.Image")));
            this.identityLabel.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("identityLabel.ImageAlign")));
            this.identityLabel.ImageIndex = ((int)(resources.GetObject("identityLabel.ImageIndex")));
            this.identityLabel.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("identityLabel.ImeMode")));
            this.identityLabel.Location = ((System.Drawing.Point)(resources.GetObject("identityLabel.Location")));
            this.identityLabel.Name = "identityLabel";
            this.identityLabel.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("identityLabel.RightToLeft")));
            this.identityLabel.Size = ((System.Drawing.Size)(resources.GetObject("identityLabel.Size")));
            this.identityLabel.TabIndex = ((int)(resources.GetObject("identityLabel.TabIndex")));
            this.identityLabel.Text = resources.GetString("identityLabel.Text");
            this.identityLabel.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("identityLabel.TextAlign")));
            this.identityLabel.Visible = ((bool)(resources.GetObject("identityLabel.Visible")));
            // 
            // evaluateButton
            // 
            this.evaluateButton.AccessibleDescription = resources.GetString("evaluateButton.AccessibleDescription");
            this.evaluateButton.AccessibleName = resources.GetString("evaluateButton.AccessibleName");
            this.evaluateButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("evaluateButton.Anchor")));
            this.evaluateButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("evaluateButton.BackgroundImage")));
            this.evaluateButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("evaluateButton.Dock")));
            this.evaluateButton.Enabled = ((bool)(resources.GetObject("evaluateButton.Enabled")));
            this.evaluateButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("evaluateButton.FlatStyle")));
            this.evaluateButton.Font = ((System.Drawing.Font)(resources.GetObject("evaluateButton.Font")));
            this.evaluateButton.Image = ((System.Drawing.Image)(resources.GetObject("evaluateButton.Image")));
            this.evaluateButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("evaluateButton.ImageAlign")));
            this.evaluateButton.ImageIndex = ((int)(resources.GetObject("evaluateButton.ImageIndex")));
            this.evaluateButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("evaluateButton.ImeMode")));
            this.evaluateButton.Location = ((System.Drawing.Point)(resources.GetObject("evaluateButton.Location")));
            this.evaluateButton.Name = "evaluateButton";
            this.evaluateButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("evaluateButton.RightToLeft")));
            this.evaluateButton.Size = ((System.Drawing.Size)(resources.GetObject("evaluateButton.Size")));
            this.evaluateButton.TabIndex = ((int)(resources.GetObject("evaluateButton.TabIndex")));
            this.evaluateButton.Text = resources.GetString("evaluateButton.Text");
            this.evaluateButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("evaluateButton.TextAlign")));
            this.evaluateButton.Visible = ((bool)(resources.GetObject("evaluateButton.Visible")));
            this.evaluateButton.Click += new System.EventHandler(this.HandleEvaluateButtonClick);
            // 
            // roleButton
            // 
            this.roleButton.AccessibleDescription = resources.GetString("roleButton.AccessibleDescription");
            this.roleButton.AccessibleName = resources.GetString("roleButton.AccessibleName");
            this.roleButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("roleButton.Anchor")));
            this.roleButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("roleButton.BackgroundImage")));
            this.roleButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("roleButton.Dock")));
            this.roleButton.Enabled = ((bool)(resources.GetObject("roleButton.Enabled")));
            this.roleButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("roleButton.FlatStyle")));
            this.roleButton.Font = ((System.Drawing.Font)(resources.GetObject("roleButton.Font")));
            this.roleButton.Image = ((System.Drawing.Image)(resources.GetObject("roleButton.Image")));
            this.roleButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("roleButton.ImageAlign")));
            this.roleButton.ImageIndex = ((int)(resources.GetObject("roleButton.ImageIndex")));
            this.roleButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("roleButton.ImeMode")));
            this.roleButton.Location = ((System.Drawing.Point)(resources.GetObject("roleButton.Location")));
            this.roleButton.Name = "roleButton";
            this.roleButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("roleButton.RightToLeft")));
            this.roleButton.Size = ((System.Drawing.Size)(resources.GetObject("roleButton.Size")));
            this.roleButton.TabIndex = ((int)(resources.GetObject("roleButton.TabIndex")));
            this.roleButton.Text = resources.GetString("roleButton.Text");
            this.roleButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("roleButton.TextAlign")));
            this.roleButton.Visible = ((bool)(resources.GetObject("roleButton.Visible")));
            this.roleButton.Click += new System.EventHandler(this.HandleRoleButtonClick);
            // 
            // identityButton
            // 
            this.identityButton.AccessibleDescription = resources.GetString("identityButton.AccessibleDescription");
            this.identityButton.AccessibleName = resources.GetString("identityButton.AccessibleName");
            this.identityButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("identityButton.Anchor")));
            this.identityButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("identityButton.BackgroundImage")));
            this.identityButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("identityButton.Dock")));
            this.identityButton.Enabled = ((bool)(resources.GetObject("identityButton.Enabled")));
            this.identityButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("identityButton.FlatStyle")));
            this.identityButton.Font = ((System.Drawing.Font)(resources.GetObject("identityButton.Font")));
            this.identityButton.Image = ((System.Drawing.Image)(resources.GetObject("identityButton.Image")));
            this.identityButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("identityButton.ImageAlign")));
            this.identityButton.ImageIndex = ((int)(resources.GetObject("identityButton.ImageIndex")));
            this.identityButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("identityButton.ImeMode")));
            this.identityButton.Location = ((System.Drawing.Point)(resources.GetObject("identityButton.Location")));
            this.identityButton.Name = "identityButton";
            this.identityButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("identityButton.RightToLeft")));
            this.identityButton.Size = ((System.Drawing.Size)(resources.GetObject("identityButton.Size")));
            this.identityButton.TabIndex = ((int)(resources.GetObject("identityButton.TabIndex")));
            this.identityButton.Text = resources.GetString("identityButton.Text");
            this.identityButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("identityButton.TextAlign")));
            this.identityButton.Visible = ((bool)(resources.GetObject("identityButton.Visible")));
            this.identityButton.Click += new System.EventHandler(this.HandleIdentityButtonClick);
            // 
            // closeGroupButton
            // 
            this.closeGroupButton.AccessibleDescription = resources.GetString("closeGroupButton.AccessibleDescription");
            this.closeGroupButton.AccessibleName = resources.GetString("closeGroupButton.AccessibleName");
            this.closeGroupButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("closeGroupButton.Anchor")));
            this.closeGroupButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("closeGroupButton.BackgroundImage")));
            this.closeGroupButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("closeGroupButton.Dock")));
            this.closeGroupButton.Enabled = ((bool)(resources.GetObject("closeGroupButton.Enabled")));
            this.closeGroupButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("closeGroupButton.FlatStyle")));
            this.closeGroupButton.Font = ((System.Drawing.Font)(resources.GetObject("closeGroupButton.Font")));
            this.closeGroupButton.Image = ((System.Drawing.Image)(resources.GetObject("closeGroupButton.Image")));
            this.closeGroupButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("closeGroupButton.ImageAlign")));
            this.closeGroupButton.ImageIndex = ((int)(resources.GetObject("closeGroupButton.ImageIndex")));
            this.closeGroupButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("closeGroupButton.ImeMode")));
            this.closeGroupButton.Location = ((System.Drawing.Point)(resources.GetObject("closeGroupButton.Location")));
            this.closeGroupButton.Name = "closeGroupButton";
            this.closeGroupButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("closeGroupButton.RightToLeft")));
            this.closeGroupButton.Size = ((System.Drawing.Size)(resources.GetObject("closeGroupButton.Size")));
            this.closeGroupButton.TabIndex = ((int)(resources.GetObject("closeGroupButton.TabIndex")));
            this.closeGroupButton.Text = resources.GetString("closeGroupButton.Text");
            this.closeGroupButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("closeGroupButton.TextAlign")));
            this.closeGroupButton.Visible = ((bool)(resources.GetObject("closeGroupButton.Visible")));
            this.closeGroupButton.Click += new System.EventHandler(this.HandleCloseGroupButtonClick);
            // 
            // openGroupButton
            // 
            this.openGroupButton.AccessibleDescription = resources.GetString("openGroupButton.AccessibleDescription");
            this.openGroupButton.AccessibleName = resources.GetString("openGroupButton.AccessibleName");
            this.openGroupButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("openGroupButton.Anchor")));
            this.openGroupButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("openGroupButton.BackgroundImage")));
            this.openGroupButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("openGroupButton.Dock")));
            this.openGroupButton.Enabled = ((bool)(resources.GetObject("openGroupButton.Enabled")));
            this.openGroupButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("openGroupButton.FlatStyle")));
            this.openGroupButton.Font = ((System.Drawing.Font)(resources.GetObject("openGroupButton.Font")));
            this.openGroupButton.Image = ((System.Drawing.Image)(resources.GetObject("openGroupButton.Image")));
            this.openGroupButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("openGroupButton.ImageAlign")));
            this.openGroupButton.ImageIndex = ((int)(resources.GetObject("openGroupButton.ImageIndex")));
            this.openGroupButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("openGroupButton.ImeMode")));
            this.openGroupButton.Location = ((System.Drawing.Point)(resources.GetObject("openGroupButton.Location")));
            this.openGroupButton.Name = "openGroupButton";
            this.openGroupButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("openGroupButton.RightToLeft")));
            this.openGroupButton.Size = ((System.Drawing.Size)(resources.GetObject("openGroupButton.Size")));
            this.openGroupButton.TabIndex = ((int)(resources.GetObject("openGroupButton.TabIndex")));
            this.openGroupButton.Text = resources.GetString("openGroupButton.Text");
            this.openGroupButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("openGroupButton.TextAlign")));
            this.openGroupButton.Visible = ((bool)(resources.GetObject("openGroupButton.Visible")));
            this.openGroupButton.Click += new System.EventHandler(this.HandleOpenGroupButtonClick);
            // 
            // notButton
            // 
            this.notButton.AccessibleDescription = resources.GetString("notButton.AccessibleDescription");
            this.notButton.AccessibleName = resources.GetString("notButton.AccessibleName");
            this.notButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("notButton.Anchor")));
            this.notButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("notButton.BackgroundImage")));
            this.notButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("notButton.Dock")));
            this.notButton.Enabled = ((bool)(resources.GetObject("notButton.Enabled")));
            this.notButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("notButton.FlatStyle")));
            this.notButton.Font = ((System.Drawing.Font)(resources.GetObject("notButton.Font")));
            this.notButton.Image = ((System.Drawing.Image)(resources.GetObject("notButton.Image")));
            this.notButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("notButton.ImageAlign")));
            this.notButton.ImageIndex = ((int)(resources.GetObject("notButton.ImageIndex")));
            this.notButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("notButton.ImeMode")));
            this.notButton.Location = ((System.Drawing.Point)(resources.GetObject("notButton.Location")));
            this.notButton.Name = "notButton";
            this.notButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("notButton.RightToLeft")));
            this.notButton.Size = ((System.Drawing.Size)(resources.GetObject("notButton.Size")));
            this.notButton.TabIndex = ((int)(resources.GetObject("notButton.TabIndex")));
            this.notButton.Text = resources.GetString("notButton.Text");
            this.notButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("notButton.TextAlign")));
            this.notButton.Visible = ((bool)(resources.GetObject("notButton.Visible")));
            this.notButton.Click += new System.EventHandler(this.HandleNotButtonClick);
            // 
            // orButton
            // 
            this.orButton.AccessibleDescription = resources.GetString("orButton.AccessibleDescription");
            this.orButton.AccessibleName = resources.GetString("orButton.AccessibleName");
            this.orButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("orButton.Anchor")));
            this.orButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("orButton.BackgroundImage")));
            this.orButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("orButton.Dock")));
            this.orButton.Enabled = ((bool)(resources.GetObject("orButton.Enabled")));
            this.orButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("orButton.FlatStyle")));
            this.orButton.Font = ((System.Drawing.Font)(resources.GetObject("orButton.Font")));
            this.orButton.Image = ((System.Drawing.Image)(resources.GetObject("orButton.Image")));
            this.orButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("orButton.ImageAlign")));
            this.orButton.ImageIndex = ((int)(resources.GetObject("orButton.ImageIndex")));
            this.orButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("orButton.ImeMode")));
            this.orButton.Location = ((System.Drawing.Point)(resources.GetObject("orButton.Location")));
            this.orButton.Name = "orButton";
            this.orButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("orButton.RightToLeft")));
            this.orButton.Size = ((System.Drawing.Size)(resources.GetObject("orButton.Size")));
            this.orButton.TabIndex = ((int)(resources.GetObject("orButton.TabIndex")));
            this.orButton.Text = resources.GetString("orButton.Text");
            this.orButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("orButton.TextAlign")));
            this.orButton.Visible = ((bool)(resources.GetObject("orButton.Visible")));
            this.orButton.Click += new System.EventHandler(this.HandleOrButtonClick);
            // 
            // andButton
            // 
            this.andButton.AccessibleDescription = resources.GetString("andButton.AccessibleDescription");
            this.andButton.AccessibleName = resources.GetString("andButton.AccessibleName");
            this.andButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("andButton.Anchor")));
            this.andButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("andButton.BackgroundImage")));
            this.andButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("andButton.Dock")));
            this.andButton.Enabled = ((bool)(resources.GetObject("andButton.Enabled")));
            this.andButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("andButton.FlatStyle")));
            this.andButton.Font = ((System.Drawing.Font)(resources.GetObject("andButton.Font")));
            this.andButton.Image = ((System.Drawing.Image)(resources.GetObject("andButton.Image")));
            this.andButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("andButton.ImageAlign")));
            this.andButton.ImageIndex = ((int)(resources.GetObject("andButton.ImageIndex")));
            this.andButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("andButton.ImeMode")));
            this.andButton.Location = ((System.Drawing.Point)(resources.GetObject("andButton.Location")));
            this.andButton.Name = "andButton";
            this.andButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("andButton.RightToLeft")));
            this.andButton.Size = ((System.Drawing.Size)(resources.GetObject("andButton.Size")));
            this.andButton.TabIndex = ((int)(resources.GetObject("andButton.TabIndex")));
            this.andButton.Text = resources.GetString("andButton.Text");
            this.andButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("andButton.TextAlign")));
            this.andButton.Visible = ((bool)(resources.GetObject("andButton.Visible")));
            this.andButton.Click += new System.EventHandler(this.HandleAndButtonClick);
            // 
            // expressionTextBox
            // 
            this.expressionTextBox.AcceptsTab = true;
            this.expressionTextBox.AccessibleDescription = resources.GetString("expressionTextBox.AccessibleDescription");
            this.expressionTextBox.AccessibleName = resources.GetString("expressionTextBox.AccessibleName");
            this.expressionTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("expressionTextBox.Anchor")));
            this.expressionTextBox.AutoSize = ((bool)(resources.GetObject("expressionTextBox.AutoSize")));
            this.expressionTextBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("expressionTextBox.BackgroundImage")));
            this.expressionTextBox.BulletIndent = ((int)(resources.GetObject("expressionTextBox.BulletIndent")));
            this.expressionTextBox.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("expressionTextBox.Dock")));
            this.expressionTextBox.Enabled = ((bool)(resources.GetObject("expressionTextBox.Enabled")));
            this.expressionTextBox.Font = ((System.Drawing.Font)(resources.GetObject("expressionTextBox.Font")));
            this.expressionTextBox.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("expressionTextBox.ImeMode")));
            this.expressionTextBox.Location = ((System.Drawing.Point)(resources.GetObject("expressionTextBox.Location")));
            this.expressionTextBox.MaxLength = ((int)(resources.GetObject("expressionTextBox.MaxLength")));
            this.expressionTextBox.Multiline = ((bool)(resources.GetObject("expressionTextBox.Multiline")));
            this.expressionTextBox.Name = "expressionTextBox";
            this.expressionTextBox.RightMargin = ((int)(resources.GetObject("expressionTextBox.RightMargin")));
            this.expressionTextBox.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("expressionTextBox.RightToLeft")));
            this.expressionTextBox.ScrollBars = ((System.Windows.Forms.RichTextBoxScrollBars)(resources.GetObject("expressionTextBox.ScrollBars")));
            this.expressionTextBox.Size = ((System.Drawing.Size)(resources.GetObject("expressionTextBox.Size")));
            this.expressionTextBox.TabIndex = ((int)(resources.GetObject("expressionTextBox.TabIndex")));
            this.expressionTextBox.Text = resources.GetString("expressionTextBox.Text");
            this.expressionTextBox.Visible = ((bool)(resources.GetObject("expressionTextBox.Visible")));
            this.expressionTextBox.WordWrap = ((bool)(resources.GetObject("expressionTextBox.WordWrap")));
            this.expressionTextBox.ZoomFactor = ((System.Single)(resources.GetObject("expressionTextBox.ZoomFactor")));
            this.expressionTextBox.TextChanged += new System.EventHandler(this.expressionTextBox_TextChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.AccessibleDescription = resources.GetString("groupBox2.AccessibleDescription");
            this.groupBox2.AccessibleName = resources.GetString("groupBox2.AccessibleName");
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("groupBox2.Anchor")));
            this.groupBox2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("groupBox2.BackgroundImage")));
            this.groupBox2.Controls.Add(this.anonymousButton);
            this.groupBox2.Controls.Add(this.expressionTextBox);
            this.groupBox2.Controls.Add(this.identityButton);
            this.groupBox2.Controls.Add(this.closeGroupButton);
            this.groupBox2.Controls.Add(this.openGroupButton);
            this.groupBox2.Controls.Add(this.notButton);
            this.groupBox2.Controls.Add(this.orButton);
            this.groupBox2.Controls.Add(this.andButton);
            this.groupBox2.Controls.Add(this.roleButton);
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
            // anonymousButton
            // 
            this.anonymousButton.AccessibleDescription = resources.GetString("anonymousButton.AccessibleDescription");
            this.anonymousButton.AccessibleName = resources.GetString("anonymousButton.AccessibleName");
            this.anonymousButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("anonymousButton.Anchor")));
            this.anonymousButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("anonymousButton.BackgroundImage")));
            this.anonymousButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("anonymousButton.Dock")));
            this.anonymousButton.Enabled = ((bool)(resources.GetObject("anonymousButton.Enabled")));
            this.anonymousButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("anonymousButton.FlatStyle")));
            this.anonymousButton.Font = ((System.Drawing.Font)(resources.GetObject("anonymousButton.Font")));
            this.anonymousButton.Image = ((System.Drawing.Image)(resources.GetObject("anonymousButton.Image")));
            this.anonymousButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("anonymousButton.ImageAlign")));
            this.anonymousButton.ImageIndex = ((int)(resources.GetObject("anonymousButton.ImageIndex")));
            this.anonymousButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("anonymousButton.ImeMode")));
            this.anonymousButton.Location = ((System.Drawing.Point)(resources.GetObject("anonymousButton.Location")));
            this.anonymousButton.Name = "anonymousButton";
            this.anonymousButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("anonymousButton.RightToLeft")));
            this.anonymousButton.Size = ((System.Drawing.Size)(resources.GetObject("anonymousButton.Size")));
            this.anonymousButton.TabIndex = ((int)(resources.GetObject("anonymousButton.TabIndex")));
            this.anonymousButton.Text = resources.GetString("anonymousButton.Text");
            this.anonymousButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("anonymousButton.TextAlign")));
            this.anonymousButton.Visible = ((bool)(resources.GetObject("anonymousButton.Visible")));
            this.anonymousButton.Click += new System.EventHandler(this.anonymousButton_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.AccessibleDescription = resources.GetString("groupBox3.AccessibleDescription");
            this.groupBox3.AccessibleName = resources.GetString("groupBox3.AccessibleName");
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("groupBox3.Anchor")));
            this.groupBox3.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("groupBox3.BackgroundImage")));
            this.groupBox3.Controls.Add(this.validationResultBox);
            this.groupBox3.Controls.Add(this.identityTextBox);
            this.groupBox3.Controls.Add(this.roleTextBox);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.identityLabel);
            this.groupBox3.Controls.Add(this.authenticationCheckBox);
            this.groupBox3.Controls.Add(this.evaluateButton);
            this.groupBox3.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("groupBox3.Dock")));
            this.groupBox3.Enabled = ((bool)(resources.GetObject("groupBox3.Enabled")));
            this.groupBox3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox3.Font = ((System.Drawing.Font)(resources.GetObject("groupBox3.Font")));
            this.groupBox3.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("groupBox3.ImeMode")));
            this.groupBox3.Location = ((System.Drawing.Point)(resources.GetObject("groupBox3.Location")));
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("groupBox3.RightToLeft")));
            this.groupBox3.Size = ((System.Drawing.Size)(resources.GetObject("groupBox3.Size")));
            this.groupBox3.TabIndex = ((int)(resources.GetObject("groupBox3.TabIndex")));
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = resources.GetString("groupBox3.Text");
            this.groupBox3.Visible = ((bool)(resources.GetObject("groupBox3.Visible")));
            // 
            // validationResultBox
            // 
            this.validationResultBox.AccessibleDescription = resources.GetString("validationResultBox.AccessibleDescription");
            this.validationResultBox.AccessibleName = resources.GetString("validationResultBox.AccessibleName");
            this.validationResultBox.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("validationResultBox.Anchor")));
            this.validationResultBox.AutoSize = ((bool)(resources.GetObject("validationResultBox.AutoSize")));
            this.validationResultBox.BackColor = System.Drawing.SystemColors.Control;
            this.validationResultBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("validationResultBox.BackgroundImage")));
            this.validationResultBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.validationResultBox.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.validationResultBox.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("validationResultBox.Dock")));
            this.validationResultBox.Enabled = ((bool)(resources.GetObject("validationResultBox.Enabled")));
            this.validationResultBox.Font = ((System.Drawing.Font)(resources.GetObject("validationResultBox.Font")));
            this.validationResultBox.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("validationResultBox.ImeMode")));
            this.validationResultBox.Location = ((System.Drawing.Point)(resources.GetObject("validationResultBox.Location")));
            this.validationResultBox.MaxLength = ((int)(resources.GetObject("validationResultBox.MaxLength")));
            this.validationResultBox.Multiline = ((bool)(resources.GetObject("validationResultBox.Multiline")));
            this.validationResultBox.Name = "validationResultBox";
            this.validationResultBox.PasswordChar = ((char)(resources.GetObject("validationResultBox.PasswordChar")));
            this.validationResultBox.ReadOnly = true;
            this.validationResultBox.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("validationResultBox.RightToLeft")));
            this.validationResultBox.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("validationResultBox.ScrollBars")));
            this.validationResultBox.Size = ((System.Drawing.Size)(resources.GetObject("validationResultBox.Size")));
            this.validationResultBox.TabIndex = ((int)(resources.GetObject("validationResultBox.TabIndex")));
            this.validationResultBox.TabStop = false;
            this.validationResultBox.Text = resources.GetString("validationResultBox.Text");
            this.validationResultBox.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("validationResultBox.TextAlign")));
            this.validationResultBox.Visible = ((bool)(resources.GetObject("validationResultBox.Visible")));
            this.validationResultBox.WordWrap = ((bool)(resources.GetObject("validationResultBox.WordWrap")));
            // 
            // groupBox4
            // 
            this.groupBox4.AccessibleDescription = resources.GetString("groupBox4.AccessibleDescription");
            this.groupBox4.AccessibleName = resources.GetString("groupBox4.AccessibleName");
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("groupBox4.Anchor")));
            this.groupBox4.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("groupBox4.BackgroundImage")));
            this.groupBox4.Controls.Add(this.ruleNameBox);
            this.groupBox4.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("groupBox4.Dock")));
            this.groupBox4.Enabled = ((bool)(resources.GetObject("groupBox4.Enabled")));
            this.groupBox4.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox4.Font = ((System.Drawing.Font)(resources.GetObject("groupBox4.Font")));
            this.groupBox4.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("groupBox4.ImeMode")));
            this.groupBox4.Location = ((System.Drawing.Point)(resources.GetObject("groupBox4.Location")));
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("groupBox4.RightToLeft")));
            this.groupBox4.Size = ((System.Drawing.Size)(resources.GetObject("groupBox4.Size")));
            this.groupBox4.TabIndex = ((int)(resources.GetObject("groupBox4.TabIndex")));
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = resources.GetString("groupBox4.Text");
            this.groupBox4.Visible = ((bool)(resources.GetObject("groupBox4.Visible")));
            // 
            // ruleNameBox
            // 
            this.ruleNameBox.AccessibleDescription = resources.GetString("ruleNameBox.AccessibleDescription");
            this.ruleNameBox.AccessibleName = resources.GetString("ruleNameBox.AccessibleName");
            this.ruleNameBox.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("ruleNameBox.Anchor")));
            this.ruleNameBox.AutoSize = ((bool)(resources.GetObject("ruleNameBox.AutoSize")));
            this.ruleNameBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ruleNameBox.BackgroundImage")));
            this.ruleNameBox.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("ruleNameBox.Dock")));
            this.ruleNameBox.Enabled = ((bool)(resources.GetObject("ruleNameBox.Enabled")));
            this.ruleNameBox.Font = ((System.Drawing.Font)(resources.GetObject("ruleNameBox.Font")));
            this.ruleNameBox.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("ruleNameBox.ImeMode")));
            this.ruleNameBox.Location = ((System.Drawing.Point)(resources.GetObject("ruleNameBox.Location")));
            this.ruleNameBox.MaxLength = ((int)(resources.GetObject("ruleNameBox.MaxLength")));
            this.ruleNameBox.Multiline = ((bool)(resources.GetObject("ruleNameBox.Multiline")));
            this.ruleNameBox.Name = "ruleNameBox";
            this.ruleNameBox.PasswordChar = ((char)(resources.GetObject("ruleNameBox.PasswordChar")));
            this.ruleNameBox.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("ruleNameBox.RightToLeft")));
            this.ruleNameBox.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("ruleNameBox.ScrollBars")));
            this.ruleNameBox.Size = ((System.Drawing.Size)(resources.GetObject("ruleNameBox.Size")));
            this.ruleNameBox.TabIndex = ((int)(resources.GetObject("ruleNameBox.TabIndex")));
            this.ruleNameBox.Text = resources.GetString("ruleNameBox.Text");
            this.ruleNameBox.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("ruleNameBox.TextAlign")));
            this.ruleNameBox.Visible = ((bool)(resources.GetObject("ruleNameBox.Visible")));
            this.ruleNameBox.WordWrap = ((bool)(resources.GetObject("ruleNameBox.WordWrap")));
            // 
            // statusBar
            // 
            this.statusBar.AccessibleDescription = resources.GetString("statusBar.AccessibleDescription");
            this.statusBar.AccessibleName = resources.GetString("statusBar.AccessibleName");
            this.statusBar.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("statusBar.Anchor")));
            this.statusBar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("statusBar.BackgroundImage")));
            this.statusBar.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("statusBar.Dock")));
            this.statusBar.Enabled = ((bool)(resources.GetObject("statusBar.Enabled")));
            this.statusBar.Font = ((System.Drawing.Font)(resources.GetObject("statusBar.Font")));
            this.statusBar.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("statusBar.ImeMode")));
            this.statusBar.Location = ((System.Drawing.Point)(resources.GetObject("statusBar.Location")));
            this.statusBar.Name = "statusBar";
            this.statusBar.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[]
                {
                    this.statusPanel
                });
            this.statusBar.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("statusBar.RightToLeft")));
            this.statusBar.ShowPanels = true;
            this.statusBar.Size = ((System.Drawing.Size)(resources.GetObject("statusBar.Size")));
            this.statusBar.SizingGrip = false;
            this.statusBar.TabIndex = ((int)(resources.GetObject("statusBar.TabIndex")));
            this.statusBar.Text = resources.GetString("statusBar.Text");
            this.statusBar.Visible = ((bool)(resources.GetObject("statusBar.Visible")));
            // 
            // statusPanel
            // 
            this.statusPanel.Alignment = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("statusPanel.Alignment")));
            this.statusPanel.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
            this.statusPanel.Icon = ((System.Drawing.Icon)(resources.GetObject("statusPanel.Icon")));
            this.statusPanel.MinWidth = ((int)(resources.GetObject("statusPanel.MinWidth")));
            this.statusPanel.Text = resources.GetString("statusPanel.Text");
            this.statusPanel.ToolTipText = resources.GetString("statusPanel.ToolTipText");
            this.statusPanel.Width = ((int)(resources.GetObject("statusPanel.Width")));
            // 
            // ExpressionEditorForm
            // 
            this.AccessibleDescription = resources.GetString("$this.AccessibleDescription");
            this.AccessibleName = resources.GetString("$this.AccessibleName");
            this.AutoScaleBaseSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScaleBaseSize")));
            this.AutoScroll = ((bool)(resources.GetObject("$this.AutoScroll")));
            this.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMargin")));
            this.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMinSize")));
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = ((System.Drawing.Size)(resources.GetObject("$this.ClientSize")));
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Enabled = ((bool)(resources.GetObject("$this.Enabled")));
            this.Font = ((System.Drawing.Font)(resources.GetObject("$this.Font")));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("$this.ImeMode")));
            this.Location = ((System.Drawing.Point)(resources.GetObject("$this.Location")));
            this.MaximizeBox = false;
            this.MaximumSize = ((System.Drawing.Size)(resources.GetObject("$this.MaximumSize")));
            this.MinimizeBox = false;
            this.MinimumSize = ((System.Drawing.Size)(resources.GetObject("$this.MinimumSize")));
            this.Name = "ExpressionEditorForm";
            this.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("$this.RightToLeft")));
            this.StartPosition = ((System.Windows.Forms.FormStartPosition)(resources.GetObject("$this.StartPosition")));
            this.Text = resources.GetString("$this.Text");
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.statusPanel)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private void HandleParseButtonClick(object sender, EventArgs e)
        {
            BooleanExpression expression = this.Parse();
            if (expression != null)
            {
				MessageBoxOptions options = (MessageBoxOptions)0;

				if (RightToLeft == RightToLeft.Yes)
				{
					options = MessageBoxOptions.RtlReading |
					   MessageBoxOptions.RightAlign;
				}

                MessageBox.Show(
                    Resources.ParseSucceededMessageBox,
                    Resources.ParseSucceededMessageBoxCaption,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
					MessageBoxDefaultButton.Button1,
					options);
            }
        }

        private BooleanExpression Parse()
        {
            parser = new Parser();
            try
            {
                return parser.Parse(this.expressionTextBox.Text);
            }
            catch (SyntaxException ex)
            {
				MessageBoxOptions options = (MessageBoxOptions)0;

				if (RightToLeft == RightToLeft.Yes)
				{
					options = MessageBoxOptions.RtlReading |
					   MessageBoxOptions.RightAlign;
				}

                MessageBox.Show(
                    string.Format(CultureInfo.CurrentCulture, Resources.ParseFailedMessage, ex.Message),
                    Resources.ParseFailedCaption,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
					MessageBoxDefaultButton.Button1,
					options);
                return null;
            }
        }

        private void HandleEvaluateButtonClick(object sender, EventArgs e)
        {
            this.Evaluate();
        }

        private void Evaluate()
        {
            Identity identity = new Identity(
                this.identityTextBox.Text,
                this.authenticationCheckBox.Checked);
            GenericPrincipal principal = new GenericPrincipal(identity, this.roleTextBox.Lines);
            BooleanExpression expression = this.Parse();
            if (expression != null)
            {
                bool result = expression.Evaluate(principal);
                this.validationResultBox.Text = result ? Resources.AccessGranted : Resources.AccessDenied;
                //this.validationResultBox.BackColor = result ? Color.LightGreen : Color.Red;
            }
        }

        private void HandleOkButtonClick(object sender, EventArgs e)
        {
            if (ruleNameBox.Text.Length == 0)
            {
				MessageBoxOptions options = (MessageBoxOptions)0;

				if (RightToLeft == RightToLeft.Yes)
				{
					options = MessageBoxOptions.RtlReading |
					   MessageBoxOptions.RightAlign;
				}

                MessageBox.Show(Resources.RuleNameRequired, Resources.RuleNameRequired, MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, options);
            }
            else if (ContinueWhenUnableToParse())
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void HandleCancelButtonClick(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void InsertToken(string token)
        {
            string expression = this.expressionTextBox.Text;
            int startIndex = this.expressionTextBox.SelectionStart;
            expression = expression.Remove(
                startIndex,
                this.expressionTextBox.SelectionLength);
            this.expressionTextBox.Text = expression.Insert(startIndex, token);
            this.expressionTextBox.SelectionStart = startIndex + token.Length;
            this.expressionTextBox.SelectionLength = 0;
            this.expressionTextBox.Focus();
        }

        private void HandleAndButtonClick(object sender, EventArgs e)
        {
            this.InsertToken("AND ");
        }

        private void HandleOrButtonClick(object sender, EventArgs e)
        {
            this.InsertToken("OR ");
        }

        private void HandleNotButtonClick(object sender, EventArgs e)
        {
            this.InsertToken("NOT ");
        }

        private void HandleOpenGroupButtonClick(object sender, EventArgs e)
        {
            this.InsertToken("(");
        }

        private void HandleCloseGroupButtonClick(object sender, EventArgs e)
        {
            this.InsertToken(") ");
        }

        private void HandleIdentityButtonClick(object sender, EventArgs e)
        {
            this.InsertToken("I:");
        }

        private void HandleRoleButtonClick(object sender, EventArgs e)
        {
            this.InsertToken("R:");
        }

        private void anonymousButton_Click(object sender, EventArgs e)
        {
            this.InsertToken("I:? ");
        }

        private bool ContinueWhenUnableToParse()
        {
            bool cont = true;

            if (!expressionTextBox.ExpressionIsValid)
            {
				MessageBoxOptions options = (MessageBoxOptions)0;

				if (RightToLeft == RightToLeft.Yes)
				{
					options = MessageBoxOptions.RtlReading |
					   MessageBoxOptions.RightAlign;
				}

                DialogResult expressionDialog = MessageBox.Show(
                    Resources.UnableToParseDialogMessage,
                    Resources.UnableToParseDialogCaption,
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
					MessageBoxDefaultButton.Button1,
					options
                    );

                cont = expressionDialog == DialogResult.Yes ? true : false;
            }

            return cont;
        }

        private void expressionTextBox_TextChanged(object sender, EventArgs e)
        {
            statusPanel.Text = expressionTextBox.ParseStatus;
        }

        private class Identity : IIdentity
        {
            private string name;
            private bool authenticated;

            public Identity(string name, bool authenticated)
            {
                this.name = name;
                this.authenticated = authenticated;
            }

            public bool IsAuthenticated
            {
                get { return this.authenticated; }
            }

            public string Name
            {
                get { return this.name; }
            }

            public string AuthenticationType
            {
                get { return null; }
            }
        }
    }
}
