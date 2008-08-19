//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block QuickStart
//===============================================================================
// Copyright ? Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using DataAccessQuickStart.Properties;

namespace DataAccessQuickStart
{
    /// <summary>
    /// Enterprise Library Data Access Block Quick Start Sample.
    /// Please run SetupQuickStartsDB.bat to create database objects 
    /// used by this sample.
    /// </summary>
    public class QuickStartForm : Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private Container components = null;

        private Label label4;
        private GroupBox groupBox1;
        private Label label2;
        private GroupBox groupBox;

        private Process viewerProcess = null;
        private DataGrid resultsDataGrid;
        private Button updateUsingDataSetButton;
        private Button transactionalUpdateButton;
        private Button singleItemButton;
        private Button retrieveUsingReaderButton;
        private Button singleRowButton;
        private Button retrieveUsingDatasetButton;
        private Label useCaseLabel;
        private Button retrieveUsingXmlReaderButton;
        private Button viewWalkthroughButton;
        private Button quitButton;
        private PictureBox logoPictureBox;
        private TextBox resultsTextBox;

        private SalesData salesData = new SalesData();

        private const string HelpViewerArguments = @"/helpcol ms-help://MS.VSCC.v90/MS.VSIPCC.v90/ms.practices.entlib.2008may /LaunchFKeywordTopic DataAccessQS1";

        public QuickStartForm()
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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(QuickStartForm));
            this.resultsTextBox = new System.Windows.Forms.TextBox();
            this.updateUsingDataSetButton = new System.Windows.Forms.Button();
            this.transactionalUpdateButton = new System.Windows.Forms.Button();
            this.singleItemButton = new System.Windows.Forms.Button();
            this.retrieveUsingReaderButton = new System.Windows.Forms.Button();
            this.singleRowButton = new System.Windows.Forms.Button();
            this.retrieveUsingDatasetButton = new System.Windows.Forms.Button();
            this.useCaseLabel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.retrieveUsingXmlReaderButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.logoPictureBox = new System.Windows.Forms.PictureBox();
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.viewWalkthroughButton = new System.Windows.Forms.Button();
            this.quitButton = new System.Windows.Forms.Button();
            this.resultsDataGrid = new System.Windows.Forms.DataGrid();
            this.groupBox1.SuspendLayout();
            this.groupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.resultsDataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // resultsTextBox
            // 
            this.resultsTextBox.AccessibleDescription = resources.GetString("resultsTextBox.AccessibleDescription");
            this.resultsTextBox.AccessibleName = resources.GetString("resultsTextBox.AccessibleName");
            this.resultsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("resultsTextBox.Anchor")));
            this.resultsTextBox.AutoSize = ((bool)(resources.GetObject("resultsTextBox.AutoSize")));
            this.resultsTextBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("resultsTextBox.BackgroundImage")));
            this.resultsTextBox.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("resultsTextBox.Dock")));
            this.resultsTextBox.Enabled = ((bool)(resources.GetObject("resultsTextBox.Enabled")));
            this.resultsTextBox.Font = ((System.Drawing.Font)(resources.GetObject("resultsTextBox.Font")));
            this.resultsTextBox.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("resultsTextBox.ImeMode")));
            this.resultsTextBox.Location = ((System.Drawing.Point)(resources.GetObject("resultsTextBox.Location")));
            this.resultsTextBox.MaxLength = ((int)(resources.GetObject("resultsTextBox.MaxLength")));
            this.resultsTextBox.Multiline = ((bool)(resources.GetObject("resultsTextBox.Multiline")));
            this.resultsTextBox.Name = "resultsTextBox";
            this.resultsTextBox.PasswordChar = ((char)(resources.GetObject("resultsTextBox.PasswordChar")));
            this.resultsTextBox.ReadOnly = true;
            this.resultsTextBox.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("resultsTextBox.RightToLeft")));
            this.resultsTextBox.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("resultsTextBox.ScrollBars")));
            this.resultsTextBox.Size = ((System.Drawing.Size)(resources.GetObject("resultsTextBox.Size")));
            this.resultsTextBox.TabIndex = ((int)(resources.GetObject("resultsTextBox.TabIndex")));
            this.resultsTextBox.TabStop = false;
            this.resultsTextBox.Text = resources.GetString("resultsTextBox.Text");
            this.resultsTextBox.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("resultsTextBox.TextAlign")));
            this.resultsTextBox.Visible = ((bool)(resources.GetObject("resultsTextBox.Visible")));
            this.resultsTextBox.WordWrap = ((bool)(resources.GetObject("resultsTextBox.WordWrap")));
            // 
            // updateUsingDataSetButton
            // 
            this.updateUsingDataSetButton.AccessibleDescription = resources.GetString("updateUsingDataSetButton.AccessibleDescription");
            this.updateUsingDataSetButton.AccessibleName = resources.GetString("updateUsingDataSetButton.AccessibleName");
            this.updateUsingDataSetButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("updateUsingDataSetButton.Anchor")));
            this.updateUsingDataSetButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("updateUsingDataSetButton.BackgroundImage")));
            this.updateUsingDataSetButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("updateUsingDataSetButton.Dock")));
            this.updateUsingDataSetButton.Enabled = ((bool)(resources.GetObject("updateUsingDataSetButton.Enabled")));
            this.updateUsingDataSetButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("updateUsingDataSetButton.FlatStyle")));
            this.updateUsingDataSetButton.Font = ((System.Drawing.Font)(resources.GetObject("updateUsingDataSetButton.Font")));
            this.updateUsingDataSetButton.Image = ((System.Drawing.Image)(resources.GetObject("updateUsingDataSetButton.Image")));
            this.updateUsingDataSetButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("updateUsingDataSetButton.ImageAlign")));
            this.updateUsingDataSetButton.ImageIndex = ((int)(resources.GetObject("updateUsingDataSetButton.ImageIndex")));
            this.updateUsingDataSetButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("updateUsingDataSetButton.ImeMode")));
            this.updateUsingDataSetButton.Location = ((System.Drawing.Point)(resources.GetObject("updateUsingDataSetButton.Location")));
            this.updateUsingDataSetButton.Name = "updateUsingDataSetButton";
            this.updateUsingDataSetButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("updateUsingDataSetButton.RightToLeft")));
            this.updateUsingDataSetButton.Size = ((System.Drawing.Size)(resources.GetObject("updateUsingDataSetButton.Size")));
            this.updateUsingDataSetButton.TabIndex = ((int)(resources.GetObject("updateUsingDataSetButton.TabIndex")));
            this.updateUsingDataSetButton.Text = resources.GetString("updateUsingDataSetButton.Text");
            this.updateUsingDataSetButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("updateUsingDataSetButton.TextAlign")));
            this.updateUsingDataSetButton.Visible = ((bool)(resources.GetObject("updateUsingDataSetButton.Visible")));
            this.updateUsingDataSetButton.Click += new System.EventHandler(this.updateUsingDataSetButton_Click);
            // 
            // transactionalUpdateButton
            // 
            this.transactionalUpdateButton.AccessibleDescription = resources.GetString("transactionalUpdateButton.AccessibleDescription");
            this.transactionalUpdateButton.AccessibleName = resources.GetString("transactionalUpdateButton.AccessibleName");
            this.transactionalUpdateButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("transactionalUpdateButton.Anchor")));
            this.transactionalUpdateButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("transactionalUpdateButton.BackgroundImage")));
            this.transactionalUpdateButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("transactionalUpdateButton.Dock")));
            this.transactionalUpdateButton.Enabled = ((bool)(resources.GetObject("transactionalUpdateButton.Enabled")));
            this.transactionalUpdateButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("transactionalUpdateButton.FlatStyle")));
            this.transactionalUpdateButton.Font = ((System.Drawing.Font)(resources.GetObject("transactionalUpdateButton.Font")));
            this.transactionalUpdateButton.Image = ((System.Drawing.Image)(resources.GetObject("transactionalUpdateButton.Image")));
            this.transactionalUpdateButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("transactionalUpdateButton.ImageAlign")));
            this.transactionalUpdateButton.ImageIndex = ((int)(resources.GetObject("transactionalUpdateButton.ImageIndex")));
            this.transactionalUpdateButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("transactionalUpdateButton.ImeMode")));
            this.transactionalUpdateButton.Location = ((System.Drawing.Point)(resources.GetObject("transactionalUpdateButton.Location")));
            this.transactionalUpdateButton.Name = "transactionalUpdateButton";
            this.transactionalUpdateButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("transactionalUpdateButton.RightToLeft")));
            this.transactionalUpdateButton.Size = ((System.Drawing.Size)(resources.GetObject("transactionalUpdateButton.Size")));
            this.transactionalUpdateButton.TabIndex = ((int)(resources.GetObject("transactionalUpdateButton.TabIndex")));
            this.transactionalUpdateButton.Text = resources.GetString("transactionalUpdateButton.Text");
            this.transactionalUpdateButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("transactionalUpdateButton.TextAlign")));
            this.transactionalUpdateButton.Visible = ((bool)(resources.GetObject("transactionalUpdateButton.Visible")));
            this.transactionalUpdateButton.Click += new System.EventHandler(this.transactionalUpdateButton_Click);
            // 
            // singleItemButton
            // 
            this.singleItemButton.AccessibleDescription = resources.GetString("singleItemButton.AccessibleDescription");
            this.singleItemButton.AccessibleName = resources.GetString("singleItemButton.AccessibleName");
            this.singleItemButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("singleItemButton.Anchor")));
            this.singleItemButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("singleItemButton.BackgroundImage")));
            this.singleItemButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("singleItemButton.Dock")));
            this.singleItemButton.Enabled = ((bool)(resources.GetObject("singleItemButton.Enabled")));
            this.singleItemButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("singleItemButton.FlatStyle")));
            this.singleItemButton.Font = ((System.Drawing.Font)(resources.GetObject("singleItemButton.Font")));
            this.singleItemButton.Image = ((System.Drawing.Image)(resources.GetObject("singleItemButton.Image")));
            this.singleItemButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("singleItemButton.ImageAlign")));
            this.singleItemButton.ImageIndex = ((int)(resources.GetObject("singleItemButton.ImageIndex")));
            this.singleItemButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("singleItemButton.ImeMode")));
            this.singleItemButton.Location = ((System.Drawing.Point)(resources.GetObject("singleItemButton.Location")));
            this.singleItemButton.Name = "singleItemButton";
            this.singleItemButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("singleItemButton.RightToLeft")));
            this.singleItemButton.Size = ((System.Drawing.Size)(resources.GetObject("singleItemButton.Size")));
            this.singleItemButton.TabIndex = ((int)(resources.GetObject("singleItemButton.TabIndex")));
            this.singleItemButton.Text = resources.GetString("singleItemButton.Text");
            this.singleItemButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("singleItemButton.TextAlign")));
            this.singleItemButton.Visible = ((bool)(resources.GetObject("singleItemButton.Visible")));
            this.singleItemButton.Click += new System.EventHandler(this.singleItemButton_Click);
            // 
            // retrieveUsingReaderButton
            // 
            this.retrieveUsingReaderButton.AccessibleDescription = resources.GetString("retrieveUsingReaderButton.AccessibleDescription");
            this.retrieveUsingReaderButton.AccessibleName = resources.GetString("retrieveUsingReaderButton.AccessibleName");
            this.retrieveUsingReaderButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("retrieveUsingReaderButton.Anchor")));
            this.retrieveUsingReaderButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("retrieveUsingReaderButton.BackgroundImage")));
            this.retrieveUsingReaderButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("retrieveUsingReaderButton.Dock")));
            this.retrieveUsingReaderButton.Enabled = ((bool)(resources.GetObject("retrieveUsingReaderButton.Enabled")));
            this.retrieveUsingReaderButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("retrieveUsingReaderButton.FlatStyle")));
            this.retrieveUsingReaderButton.Font = ((System.Drawing.Font)(resources.GetObject("retrieveUsingReaderButton.Font")));
            this.retrieveUsingReaderButton.Image = ((System.Drawing.Image)(resources.GetObject("retrieveUsingReaderButton.Image")));
            this.retrieveUsingReaderButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("retrieveUsingReaderButton.ImageAlign")));
            this.retrieveUsingReaderButton.ImageIndex = ((int)(resources.GetObject("retrieveUsingReaderButton.ImageIndex")));
            this.retrieveUsingReaderButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("retrieveUsingReaderButton.ImeMode")));
            this.retrieveUsingReaderButton.Location = ((System.Drawing.Point)(resources.GetObject("retrieveUsingReaderButton.Location")));
            this.retrieveUsingReaderButton.Name = "retrieveUsingReaderButton";
            this.retrieveUsingReaderButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("retrieveUsingReaderButton.RightToLeft")));
            this.retrieveUsingReaderButton.Size = ((System.Drawing.Size)(resources.GetObject("retrieveUsingReaderButton.Size")));
            this.retrieveUsingReaderButton.TabIndex = ((int)(resources.GetObject("retrieveUsingReaderButton.TabIndex")));
            this.retrieveUsingReaderButton.Text = resources.GetString("retrieveUsingReaderButton.Text");
            this.retrieveUsingReaderButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("retrieveUsingReaderButton.TextAlign")));
            this.retrieveUsingReaderButton.Visible = ((bool)(resources.GetObject("retrieveUsingReaderButton.Visible")));
            this.retrieveUsingReaderButton.Click += new System.EventHandler(this.retrieveUsingReaderButton_Click);
            // 
            // singleRowButton
            // 
            this.singleRowButton.AccessibleDescription = resources.GetString("singleRowButton.AccessibleDescription");
            this.singleRowButton.AccessibleName = resources.GetString("singleRowButton.AccessibleName");
            this.singleRowButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("singleRowButton.Anchor")));
            this.singleRowButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("singleRowButton.BackgroundImage")));
            this.singleRowButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("singleRowButton.Dock")));
            this.singleRowButton.Enabled = ((bool)(resources.GetObject("singleRowButton.Enabled")));
            this.singleRowButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("singleRowButton.FlatStyle")));
            this.singleRowButton.Font = ((System.Drawing.Font)(resources.GetObject("singleRowButton.Font")));
            this.singleRowButton.Image = ((System.Drawing.Image)(resources.GetObject("singleRowButton.Image")));
            this.singleRowButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("singleRowButton.ImageAlign")));
            this.singleRowButton.ImageIndex = ((int)(resources.GetObject("singleRowButton.ImageIndex")));
            this.singleRowButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("singleRowButton.ImeMode")));
            this.singleRowButton.Location = ((System.Drawing.Point)(resources.GetObject("singleRowButton.Location")));
            this.singleRowButton.Name = "singleRowButton";
            this.singleRowButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("singleRowButton.RightToLeft")));
            this.singleRowButton.Size = ((System.Drawing.Size)(resources.GetObject("singleRowButton.Size")));
            this.singleRowButton.TabIndex = ((int)(resources.GetObject("singleRowButton.TabIndex")));
            this.singleRowButton.Text = resources.GetString("singleRowButton.Text");
            this.singleRowButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("singleRowButton.TextAlign")));
            this.singleRowButton.Visible = ((bool)(resources.GetObject("singleRowButton.Visible")));
            this.singleRowButton.Click += new System.EventHandler(this.singleRowButton_Click);
            // 
            // retrieveUsingDatasetButton
            // 
            this.retrieveUsingDatasetButton.AccessibleDescription = resources.GetString("retrieveUsingDatasetButton.AccessibleDescription");
            this.retrieveUsingDatasetButton.AccessibleName = resources.GetString("retrieveUsingDatasetButton.AccessibleName");
            this.retrieveUsingDatasetButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("retrieveUsingDatasetButton.Anchor")));
            this.retrieveUsingDatasetButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("retrieveUsingDatasetButton.BackgroundImage")));
            this.retrieveUsingDatasetButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("retrieveUsingDatasetButton.Dock")));
            this.retrieveUsingDatasetButton.Enabled = ((bool)(resources.GetObject("retrieveUsingDatasetButton.Enabled")));
            this.retrieveUsingDatasetButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("retrieveUsingDatasetButton.FlatStyle")));
            this.retrieveUsingDatasetButton.Font = ((System.Drawing.Font)(resources.GetObject("retrieveUsingDatasetButton.Font")));
            this.retrieveUsingDatasetButton.Image = ((System.Drawing.Image)(resources.GetObject("retrieveUsingDatasetButton.Image")));
            this.retrieveUsingDatasetButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("retrieveUsingDatasetButton.ImageAlign")));
            this.retrieveUsingDatasetButton.ImageIndex = ((int)(resources.GetObject("retrieveUsingDatasetButton.ImageIndex")));
            this.retrieveUsingDatasetButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("retrieveUsingDatasetButton.ImeMode")));
            this.retrieveUsingDatasetButton.Location = ((System.Drawing.Point)(resources.GetObject("retrieveUsingDatasetButton.Location")));
            this.retrieveUsingDatasetButton.Name = "retrieveUsingDatasetButton";
            this.retrieveUsingDatasetButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("retrieveUsingDatasetButton.RightToLeft")));
            this.retrieveUsingDatasetButton.Size = ((System.Drawing.Size)(resources.GetObject("retrieveUsingDatasetButton.Size")));
            this.retrieveUsingDatasetButton.TabIndex = ((int)(resources.GetObject("retrieveUsingDatasetButton.TabIndex")));
            this.retrieveUsingDatasetButton.Text = resources.GetString("retrieveUsingDatasetButton.Text");
            this.retrieveUsingDatasetButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("retrieveUsingDatasetButton.TextAlign")));
            this.retrieveUsingDatasetButton.Visible = ((bool)(resources.GetObject("retrieveUsingDatasetButton.Visible")));
            this.retrieveUsingDatasetButton.Click += new System.EventHandler(this.retrieveUsingDatasetButton_Click);
            // 
            // useCaseLabel
            // 
            this.useCaseLabel.AccessibleDescription = resources.GetString("useCaseLabel.AccessibleDescription");
            this.useCaseLabel.AccessibleName = resources.GetString("useCaseLabel.AccessibleName");
            this.useCaseLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("useCaseLabel.Anchor")));
            this.useCaseLabel.AutoSize = ((bool)(resources.GetObject("useCaseLabel.AutoSize")));
            this.useCaseLabel.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("useCaseLabel.Dock")));
            this.useCaseLabel.Enabled = ((bool)(resources.GetObject("useCaseLabel.Enabled")));
            this.useCaseLabel.Font = ((System.Drawing.Font)(resources.GetObject("useCaseLabel.Font")));
            this.useCaseLabel.Image = ((System.Drawing.Image)(resources.GetObject("useCaseLabel.Image")));
            this.useCaseLabel.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("useCaseLabel.ImageAlign")));
            this.useCaseLabel.ImageIndex = ((int)(resources.GetObject("useCaseLabel.ImageIndex")));
            this.useCaseLabel.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("useCaseLabel.ImeMode")));
            this.useCaseLabel.Location = ((System.Drawing.Point)(resources.GetObject("useCaseLabel.Location")));
            this.useCaseLabel.Name = "useCaseLabel";
            this.useCaseLabel.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("useCaseLabel.RightToLeft")));
            this.useCaseLabel.Size = ((System.Drawing.Size)(resources.GetObject("useCaseLabel.Size")));
            this.useCaseLabel.TabIndex = ((int)(resources.GetObject("useCaseLabel.TabIndex")));
            this.useCaseLabel.Text = resources.GetString("useCaseLabel.Text");
            this.useCaseLabel.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("useCaseLabel.TextAlign")));
            this.useCaseLabel.Visible = ((bool)(resources.GetObject("useCaseLabel.Visible")));
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
            // retrieveUsingXmlReaderButton
            // 
            this.retrieveUsingXmlReaderButton.AccessibleDescription = resources.GetString("retrieveUsingXmlReaderButton.AccessibleDescription");
            this.retrieveUsingXmlReaderButton.AccessibleName = resources.GetString("retrieveUsingXmlReaderButton.AccessibleName");
            this.retrieveUsingXmlReaderButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("retrieveUsingXmlReaderButton.Anchor")));
            this.retrieveUsingXmlReaderButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("retrieveUsingXmlReaderButton.BackgroundImage")));
            this.retrieveUsingXmlReaderButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("retrieveUsingXmlReaderButton.Dock")));
            this.retrieveUsingXmlReaderButton.Enabled = ((bool)(resources.GetObject("retrieveUsingXmlReaderButton.Enabled")));
            this.retrieveUsingXmlReaderButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("retrieveUsingXmlReaderButton.FlatStyle")));
            this.retrieveUsingXmlReaderButton.Font = ((System.Drawing.Font)(resources.GetObject("retrieveUsingXmlReaderButton.Font")));
            this.retrieveUsingXmlReaderButton.Image = ((System.Drawing.Image)(resources.GetObject("retrieveUsingXmlReaderButton.Image")));
            this.retrieveUsingXmlReaderButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("retrieveUsingXmlReaderButton.ImageAlign")));
            this.retrieveUsingXmlReaderButton.ImageIndex = ((int)(resources.GetObject("retrieveUsingXmlReaderButton.ImageIndex")));
            this.retrieveUsingXmlReaderButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("retrieveUsingXmlReaderButton.ImeMode")));
            this.retrieveUsingXmlReaderButton.Location = ((System.Drawing.Point)(resources.GetObject("retrieveUsingXmlReaderButton.Location")));
            this.retrieveUsingXmlReaderButton.Name = "retrieveUsingXmlReaderButton";
            this.retrieveUsingXmlReaderButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("retrieveUsingXmlReaderButton.RightToLeft")));
            this.retrieveUsingXmlReaderButton.Size = ((System.Drawing.Size)(resources.GetObject("retrieveUsingXmlReaderButton.Size")));
            this.retrieveUsingXmlReaderButton.TabIndex = ((int)(resources.GetObject("retrieveUsingXmlReaderButton.TabIndex")));
            this.retrieveUsingXmlReaderButton.Text = resources.GetString("retrieveUsingXmlReaderButton.Text");
            this.retrieveUsingXmlReaderButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("retrieveUsingXmlReaderButton.TextAlign")));
            this.retrieveUsingXmlReaderButton.Visible = ((bool)(resources.GetObject("retrieveUsingXmlReaderButton.Visible")));
            this.retrieveUsingXmlReaderButton.Click += new System.EventHandler(this.retrieveUsingXmlReaderButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.AccessibleDescription = resources.GetString("groupBox1.AccessibleDescription");
            this.groupBox1.AccessibleName = resources.GetString("groupBox1.AccessibleName");
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("groupBox1.Anchor")));
            this.groupBox1.BackColor = System.Drawing.Color.White;
            this.groupBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("groupBox1.BackgroundImage")));
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.logoPictureBox);
            this.groupBox1.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("groupBox1.Dock")));
            this.groupBox1.Enabled = ((bool)(resources.GetObject("groupBox1.Enabled")));
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
            // logoPictureBox
            // 
            this.logoPictureBox.AccessibleDescription = resources.GetString("logoPictureBox.AccessibleDescription");
            this.logoPictureBox.AccessibleName = resources.GetString("logoPictureBox.AccessibleName");
            this.logoPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("logoPictureBox.Anchor")));
            this.logoPictureBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("logoPictureBox.BackgroundImage")));
            this.logoPictureBox.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("logoPictureBox.Dock")));
            this.logoPictureBox.Enabled = ((bool)(resources.GetObject("logoPictureBox.Enabled")));
            this.logoPictureBox.Font = ((System.Drawing.Font)(resources.GetObject("logoPictureBox.Font")));
            this.logoPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("logoPictureBox.Image")));
            this.logoPictureBox.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("logoPictureBox.ImeMode")));
            this.logoPictureBox.Location = ((System.Drawing.Point)(resources.GetObject("logoPictureBox.Location")));
            this.logoPictureBox.Name = "logoPictureBox";
            this.logoPictureBox.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("logoPictureBox.RightToLeft")));
            this.logoPictureBox.Size = ((System.Drawing.Size)(resources.GetObject("logoPictureBox.Size")));
            this.logoPictureBox.SizeMode = ((System.Windows.Forms.PictureBoxSizeMode)(resources.GetObject("logoPictureBox.SizeMode")));
            this.logoPictureBox.TabIndex = ((int)(resources.GetObject("logoPictureBox.TabIndex")));
            this.logoPictureBox.TabStop = false;
            this.logoPictureBox.Text = resources.GetString("logoPictureBox.Text");
            this.logoPictureBox.Visible = ((bool)(resources.GetObject("logoPictureBox.Visible")));
            // 
            // groupBox
            // 
            this.groupBox.AccessibleDescription = resources.GetString("groupBox.AccessibleDescription");
            this.groupBox.AccessibleName = resources.GetString("groupBox.AccessibleName");
            this.groupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("groupBox.Anchor")));
            this.groupBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("groupBox.BackgroundImage")));
            this.groupBox.Controls.Add(this.viewWalkthroughButton);
            this.groupBox.Controls.Add(this.quitButton);
            this.groupBox.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("groupBox.Dock")));
            this.groupBox.Enabled = ((bool)(resources.GetObject("groupBox.Enabled")));
            this.groupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox.Font = ((System.Drawing.Font)(resources.GetObject("groupBox.Font")));
            this.groupBox.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("groupBox.ImeMode")));
            this.groupBox.Location = ((System.Drawing.Point)(resources.GetObject("groupBox.Location")));
            this.groupBox.Name = "groupBox";
            this.groupBox.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("groupBox.RightToLeft")));
            this.groupBox.Size = ((System.Drawing.Size)(resources.GetObject("groupBox.Size")));
            this.groupBox.TabIndex = ((int)(resources.GetObject("groupBox.TabIndex")));
            this.groupBox.TabStop = false;
            this.groupBox.Text = resources.GetString("groupBox.Text");
            this.groupBox.Visible = ((bool)(resources.GetObject("groupBox.Visible")));
            // 
            // viewWalkthroughButton
            // 
            this.viewWalkthroughButton.AccessibleDescription = resources.GetString("viewWalkthroughButton.AccessibleDescription");
            this.viewWalkthroughButton.AccessibleName = resources.GetString("viewWalkthroughButton.AccessibleName");
            this.viewWalkthroughButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("viewWalkthroughButton.Anchor")));
            this.viewWalkthroughButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("viewWalkthroughButton.BackgroundImage")));
            this.viewWalkthroughButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("viewWalkthroughButton.Dock")));
            this.viewWalkthroughButton.Enabled = ((bool)(resources.GetObject("viewWalkthroughButton.Enabled")));
            this.viewWalkthroughButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("viewWalkthroughButton.FlatStyle")));
            this.viewWalkthroughButton.Font = ((System.Drawing.Font)(resources.GetObject("viewWalkthroughButton.Font")));
            this.viewWalkthroughButton.Image = ((System.Drawing.Image)(resources.GetObject("viewWalkthroughButton.Image")));
            this.viewWalkthroughButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("viewWalkthroughButton.ImageAlign")));
            this.viewWalkthroughButton.ImageIndex = ((int)(resources.GetObject("viewWalkthroughButton.ImageIndex")));
            this.viewWalkthroughButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("viewWalkthroughButton.ImeMode")));
            this.viewWalkthroughButton.Location = ((System.Drawing.Point)(resources.GetObject("viewWalkthroughButton.Location")));
            this.viewWalkthroughButton.Name = "viewWalkthroughButton";
            this.viewWalkthroughButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("viewWalkthroughButton.RightToLeft")));
            this.viewWalkthroughButton.Size = ((System.Drawing.Size)(resources.GetObject("viewWalkthroughButton.Size")));
            this.viewWalkthroughButton.TabIndex = ((int)(resources.GetObject("viewWalkthroughButton.TabIndex")));
            this.viewWalkthroughButton.Text = resources.GetString("viewWalkthroughButton.Text");
            this.viewWalkthroughButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("viewWalkthroughButton.TextAlign")));
            this.viewWalkthroughButton.Visible = ((bool)(resources.GetObject("viewWalkthroughButton.Visible")));
            this.viewWalkthroughButton.Click += new System.EventHandler(this.viewWalkthroughButton_Click);
            // 
            // quitButton
            // 
            this.quitButton.AccessibleDescription = resources.GetString("quitButton.AccessibleDescription");
            this.quitButton.AccessibleName = resources.GetString("quitButton.AccessibleName");
            this.quitButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("quitButton.Anchor")));
            this.quitButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("quitButton.BackgroundImage")));
            this.quitButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.quitButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("quitButton.Dock")));
            this.quitButton.Enabled = ((bool)(resources.GetObject("quitButton.Enabled")));
            this.quitButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("quitButton.FlatStyle")));
            this.quitButton.Font = ((System.Drawing.Font)(resources.GetObject("quitButton.Font")));
            this.quitButton.Image = ((System.Drawing.Image)(resources.GetObject("quitButton.Image")));
            this.quitButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("quitButton.ImageAlign")));
            this.quitButton.ImageIndex = ((int)(resources.GetObject("quitButton.ImageIndex")));
            this.quitButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("quitButton.ImeMode")));
            this.quitButton.Location = ((System.Drawing.Point)(resources.GetObject("quitButton.Location")));
            this.quitButton.Name = "quitButton";
            this.quitButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("quitButton.RightToLeft")));
            this.quitButton.Size = ((System.Drawing.Size)(resources.GetObject("quitButton.Size")));
            this.quitButton.TabIndex = ((int)(resources.GetObject("quitButton.TabIndex")));
            this.quitButton.Text = resources.GetString("quitButton.Text");
            this.quitButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("quitButton.TextAlign")));
            this.quitButton.Visible = ((bool)(resources.GetObject("quitButton.Visible")));
            this.quitButton.Click += new System.EventHandler(this.quitButton_Click);
            // 
            // resultsDataGrid
            // 
            this.resultsDataGrid.AccessibleDescription = resources.GetString("resultsDataGrid.AccessibleDescription");
            this.resultsDataGrid.AccessibleName = resources.GetString("resultsDataGrid.AccessibleName");
            this.resultsDataGrid.AlternatingBackColor = System.Drawing.Color.FromArgb(((System.Byte)(173)), ((System.Byte)(207)), ((System.Byte)(239)));
            this.resultsDataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("resultsDataGrid.Anchor")));
            this.resultsDataGrid.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("resultsDataGrid.BackgroundImage")));
            this.resultsDataGrid.CaptionFont = ((System.Drawing.Font)(resources.GetObject("resultsDataGrid.CaptionFont")));
            this.resultsDataGrid.CaptionText = resources.GetString("resultsDataGrid.CaptionText");
            this.resultsDataGrid.DataMember = "";
            this.resultsDataGrid.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("resultsDataGrid.Dock")));
            this.resultsDataGrid.Enabled = ((bool)(resources.GetObject("resultsDataGrid.Enabled")));
            this.resultsDataGrid.Font = ((System.Drawing.Font)(resources.GetObject("resultsDataGrid.Font")));
            this.resultsDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.resultsDataGrid.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("resultsDataGrid.ImeMode")));
            this.resultsDataGrid.Location = ((System.Drawing.Point)(resources.GetObject("resultsDataGrid.Location")));
            this.resultsDataGrid.Name = "resultsDataGrid";
            this.resultsDataGrid.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("resultsDataGrid.RightToLeft")));
            this.resultsDataGrid.Size = ((System.Drawing.Size)(resources.GetObject("resultsDataGrid.Size")));
            this.resultsDataGrid.TabIndex = ((int)(resources.GetObject("resultsDataGrid.TabIndex")));
            this.resultsDataGrid.TabStop = false;
            this.resultsDataGrid.Visible = ((bool)(resources.GetObject("resultsDataGrid.Visible")));
            // 
            // QuickStartForm
            // 
            this.AccessibleDescription = resources.GetString("$this.AccessibleDescription");
            this.AccessibleName = resources.GetString("$this.AccessibleName");
            this.AutoScroll = ((bool)(resources.GetObject("$this.AutoScroll")));
            this.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMargin")));
            this.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMinSize")));
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.CancelButton = this.quitButton;
            this.ClientSize = ((System.Drawing.Size)(resources.GetObject("$this.ClientSize")));
            this.Controls.Add(this.groupBox);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.retrieveUsingXmlReaderButton);
            this.Controls.Add(this.useCaseLabel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.updateUsingDataSetButton);
            this.Controls.Add(this.transactionalUpdateButton);
            this.Controls.Add(this.singleItemButton);
            this.Controls.Add(this.retrieveUsingReaderButton);
            this.Controls.Add(this.singleRowButton);
            this.Controls.Add(this.retrieveUsingDatasetButton);
            this.Controls.Add(this.resultsTextBox);
            this.Controls.Add(this.resultsDataGrid);
            this.Enabled = ((bool)(resources.GetObject("$this.Enabled")));
            this.Font = ((System.Drawing.Font)(resources.GetObject("$this.Font")));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("$this.ImeMode")));
            this.Location = ((System.Drawing.Point)(resources.GetObject("$this.Location")));
            this.MaximizeBox = false;
            this.MaximumSize = ((System.Drawing.Size)(resources.GetObject("$this.MaximumSize")));
            this.MinimumSize = ((System.Drawing.Size)(resources.GetObject("$this.MinimumSize")));
            this.Name = "QuickStartForm";
            this.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("$this.RightToLeft")));
            this.StartPosition = ((System.Windows.Forms.FormStartPosition)(resources.GetObject("$this.StartPosition")));
            this.Text = resources.GetString("$this.Text");
            this.Load += new System.EventHandler(this.QuickStartForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.resultsDataGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.Run(new QuickStartForm());
        }

        private void QuickStartForm_Load(object sender, EventArgs e)
        {
            // Initialize image on the form to the embedded logo
            this.logoPictureBox.Image = this.GetEmbeddedImage("DataAccessQuickStart.logo.gif");
        }

        /// <summary>
        /// Displays dialog with information about exceptions that occur in the application. 
        /// </summary>
        private void DisplayErrors(Exception ex)
        {
            string errorMsg = string.Format(Resources.Culture, Resources.GeneralExceptionMessage, ex.Message);
            errorMsg += Environment.NewLine + Resources.DbRequirementsMessage;

            DialogResult result = MessageBox.Show(errorMsg, Resources.ApplicationErrorMessage, MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Stop);

            // Exits the program when the user clicks Abort.
            if (result == DialogResult.Abort)
            {
                Application.Exit();
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        /// <summary>
        /// Retrieves the specified embedded image resource.
        /// </summary>
        private Image GetEmbeddedImage(string resourceName)
        {
            Stream resourceStream = Assembly.GetEntryAssembly().GetManifestResourceStream(resourceName);

            if (resourceStream == null)
            {
                return null;
            }

            Image img = Image.FromStream(resourceStream);

            return img;
        }

        /// <summary>
        /// Updates the results textbox on the form with the information for a use case.
        /// </summary>
        private void DisplayResults(string useCase, string results)
        {
            this.useCaseLabel.Text = useCase;
            this.resultsTextBox.Text = results;
            this.resultsDataGrid.Hide();
            this.resultsTextBox.Show();
        }

        /// <summary>
        /// Displays the grid showing the results of a use case.
        /// </summary>
        private void DisplayResults(string useCase)
        {
            this.useCaseLabel.Text = useCase;
            this.resultsDataGrid.Show();
            this.resultsTextBox.Hide();
        }

        /// <summary>
        /// Demonstrates how to retrieve multiple rows of data using
        /// a DataReader.
        /// </summary>
        private void retrieveUsingReaderButton_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                string customerList = salesData.GetCustomerList();

                this.DisplayResults(this.retrieveUsingReaderButton.Text, customerList);
            }
            catch (Exception ex)
            {
                DisplayErrors(ex);
            }
            finally
            {
                Cursor = Cursors.Arrow;
            }
        }

        /// <summary>
        /// Demonstrates how to retrieve multiple rows of data using
        /// a DataSet.
        /// </summary>
        private void retrieveUsingDatasetButton_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                DataSet customerDataSet = salesData.GetProductsInCategory(2);

                // Bind the DataSet to the DataGrid for display. 
                // The Data Access Application Block generates DataSet objects with 
                // default names for the contained DataTable objects, for example, Table, 
                // Table1, Table2, etc. 
                this.resultsDataGrid.SetDataBinding(customerDataSet, "Table");

                this.DisplayResults(this.retrieveUsingDatasetButton.Text);

            }
            catch (Exception ex)
            {
                DisplayErrors(ex);
            }
            finally
            {
                Cursor = Cursors.Arrow;
            }
        }

        /// <summary>
        /// Demonstrates how to update the database by submitting a 
        /// modified DataSet.
        /// </summary>
        private void updateUsingDataSetButton_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                int rowsAffected = salesData.UpdateProducts();

                this.DisplayResults(this.updateUsingDataSetButton.Text, string.Format(Resources.Culture, Resources.AffectedRowsMessage, rowsAffected));

            }
            catch (Exception ex)
            {
                DisplayErrors(ex);
            }
            finally
            {
                Cursor = Cursors.Arrow;
            }
        }

        /// <summary>
        /// Demonstrates how to retrieve a single row of data.
        /// </summary>
        private void singleRowButton_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                string productDetails = salesData.GetProductDetails(5);

                this.DisplayResults(this.singleRowButton.Text, productDetails);

            }
            catch (Exception ex)
            {
                DisplayErrors(ex);
            }
            finally
            {
                Cursor = Cursors.Arrow;
            }
        }

        /// <summary>
        /// Demonstrates how to retrieve a single data item from the database.
        /// </summary>
        private void singleItemButton_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                string productName = salesData.GetProductName(4);

                this.DisplayResults(this.singleItemButton.Text, productName);

            }
            catch (Exception ex)
            {
                DisplayErrors(ex);
            }
            finally
            {
                Cursor = Cursors.Arrow;
            }
        }

        /// <summary>
        /// Demonstrates how to update the database multiple times in the
        /// context of a transaction. All updates will succeed or all will be 
        /// rolled back.
        /// </summary>
        private void transactionalUpdateButton_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                string results = "";

                int amount = 500;
                int sourceAccount = 1200;
                int destinationAccount = 2235;

                if (salesData.Transfer(amount, sourceAccount, destinationAccount))
                {
                    results = Resources.TransferCompletedMessage;
                }
                else
                {
                    results = Resources.TransferFailedMessage;
                }

                this.DisplayResults(this.transactionalUpdateButton.Text, results);

            }
            catch (Exception ex)
            {
                DisplayErrors(ex);
            }
            finally
            {
                Cursor = Cursors.Arrow;
            }

        }

        /// <summary>
        /// Demonstrates how to retrieve XML data from a SQL Server database.
        /// </summary>
        private void retrieveUsingXmlReaderButton_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                string productList = salesData.GetProductList();

                DisplayResults(this.retrieveUsingXmlReaderButton.Text, productList);

            }
            catch (Exception ex)
            {
                DisplayErrors(ex);
            }
            finally
            {
                Cursor = Cursors.Arrow;
            }
        }

        /// <summary>
        /// Quits the application.
        /// </summary>
        private void quitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

		/// <summary>
		/// Returns the path and executable name for the help viewer.
		/// </summary>
		private string GetHelpViewerExecutable()
		{
			string common = Environment.GetEnvironmentVariable("CommonProgramFiles");
			return Path.Combine(common, @"Microsoft Shared\Help 9\dexplore.exe");
		}

        /// <summary>
        /// Displays Quick Start help topics using the Help 2 Viewer.
        /// </summary>
        private void viewWalkthroughButton_Click(object sender, EventArgs e)
        {
            // Process has never been started. Initialize and launch the viewer.
            if (this.viewerProcess == null)
            {
                // Initialize the Process information for the help viewer
                this.viewerProcess = new Process();

				this.viewerProcess.StartInfo.FileName = GetHelpViewerExecutable();
                this.viewerProcess.StartInfo.Arguments = HelpViewerArguments;
                this.viewerProcess.Start();
            }
            else if (this.viewerProcess.HasExited)
            {
                // Process previously started, then exited. Start the process again.
                this.viewerProcess.Start();
            }
            else
            {
                // Process was already started - bring it to the foreground
                IntPtr hWnd = this.viewerProcess.MainWindowHandle;
                if (NativeMethods.IsIconic(hWnd))
                {
                    NativeMethods.ShowWindowAsync(hWnd, NativeMethods.SW_RESTORE);
                }
                NativeMethods.SetForegroundWindow(hWnd);
            }
        }
    }
}
