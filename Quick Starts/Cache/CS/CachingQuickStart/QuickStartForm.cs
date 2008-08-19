//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block QuickStart
//===============================================================================
// Copyright ? Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using CachingQuickStart.Properties;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;

namespace CachingQuickStart
{
    /// <summary>
    /// Summary description for QuickStartForm.
    /// </summary>
    public class QuickStartForm : Form
    {
        const string HelpViewerArguments = @"/helpcol ms-help://MS.VSCC.v90/MS.VSIPCC.v90/ms.practices.entlib.2008may /LaunchFKeywordTopic CachingQS1";
        const string MasterDataViewerExecutable = "notepad.exe";
        public static Form AppForm;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        Container components = null;

        EnterNewItemForm enterNewItemForm;
        ExpirationType expiration;
        Button flushCacheButton;

        GroupBox groupBox;
        GroupBox groupBox1;
        Label label1;
        Label label2;
        Label label4;
        TextBox loadingResultsTextBox;
        PictureBox logoPictureBox;
        Process masterFileViewerProcess = null;
        Button primitivesAddButton;
        ICacheManager primitivesCache;
        Button primitivesFlushCacheButton;
        Button primitivesReadButton;
        Button primitivesRemoveButton;
        TextBox primitivesResultsTextBox;
        CacheItemPriority priority;
        Button proactiveButton;
        ProductData productData;
        Button quitButton;
        Button reactiveButton;

        SelectItemForm selectItemForm;
        SelectMasterDataItemForm selectMasterDataItemForm;

        TabControl tabControl1;
        TabPage tabPage1;
        TabPage tabPage2;
        Process viewerProcess = null;
        Button viewFileButton;
        Button viewWalkthroughButton;

        public QuickStartForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
        }

        public ExpirationType Expiration
        {
            get { return expiration; }
            set { expiration = value; }
        }

        public CacheItemPriority Priority
        {
            get { return priority; }
            set { priority = value; }
        }

        void AddScenarioSeparator(TextBox textBox)
        {
            textBox.Text += "----------------------------------------------------------" + Environment.NewLine;
            textBox.SelectAll();
            textBox.ScrollToCaret();
        }

        /// <summary>
        /// Displays dialog with information about exceptions that occur in the application. 
        /// </summary>
        static void AppThreadException(object source,
                                       ThreadExceptionEventArgs e)
        {
            string errorMsg = string.Format(new CultureInfo("en-us", true), "There are some problems while trying to use the Caching Quick Start, please check the following error messages: \n{0}\n", e.Exception.Message);
            errorMsg += Environment.NewLine;

            DialogResult result = MessageBox.Show(errorMsg, "Application Error", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Stop);

            // Exits the program when the user clicks Abort.
            if (result == DialogResult.Abort)
            {
                Application.Exit();
            }
            AppForm.Cursor = Cursors.Default;
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

                // Dispose forms
                if (enterNewItemForm != null)
                {
                    enterNewItemForm.Dispose();
                }
                if (selectItemForm != null)
                {
                    selectItemForm.Dispose();
                }
                if (selectMasterDataItemForm != null)
                {
                    selectMasterDataItemForm.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        void flushCacheButton_Click(object sender,
                                    EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                productData.FlushCache();
                loadingResultsTextBox.Clear();
                loadingResultsTextBox.Text += Resources.FlushCacheMessage + Environment.NewLine;
                AddScenarioSeparator(loadingResultsTextBox);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        Image GetEmbeddedImage(string resourceName)
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
        /// Returns the path and executable name for the help viewer.
        /// </summary>
        static string GetHelpViewerExecutable()
        {
            string common = Environment.GetEnvironmentVariable("CommonProgramFiles");
            return Path.Combine(common, @"Microsoft Shared\Help 9\dexplore.exe");
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        void InitializeComponent()
        {
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(QuickStartForm));
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.viewWalkthroughButton = new System.Windows.Forms.Button();
            this.quitButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.logoPictureBox = new System.Windows.Forms.PictureBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.primitivesRemoveButton = new System.Windows.Forms.Button();
            this.primitivesResultsTextBox = new System.Windows.Forms.TextBox();
            this.primitivesFlushCacheButton = new System.Windows.Forms.Button();
            this.primitivesReadButton = new System.Windows.Forms.Button();
            this.primitivesAddButton = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.viewFileButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.proactiveButton = new System.Windows.Forms.Button();
            this.flushCacheButton = new System.Windows.Forms.Button();
            this.reactiveButton = new System.Windows.Forms.Button();
            this.loadingResultsTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
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
            // tabControl1
            // 
            this.tabControl1.AccessibleDescription = resources.GetString("tabControl1.AccessibleDescription");
            this.tabControl1.AccessibleName = resources.GetString("tabControl1.AccessibleName");
            this.tabControl1.Alignment = ((System.Windows.Forms.TabAlignment)(resources.GetObject("tabControl1.Alignment")));
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("tabControl1.Anchor")));
            this.tabControl1.Appearance = ((System.Windows.Forms.TabAppearance)(resources.GetObject("tabControl1.Appearance")));
            this.tabControl1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("tabControl1.BackgroundImage")));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("tabControl1.Dock")));
            this.tabControl1.Enabled = ((bool)(resources.GetObject("tabControl1.Enabled")));
            this.tabControl1.Font = ((System.Drawing.Font)(resources.GetObject("tabControl1.Font")));
            this.tabControl1.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("tabControl1.ImeMode")));
            this.tabControl1.ItemSize = ((System.Drawing.Size)(resources.GetObject("tabControl1.ItemSize")));
            this.tabControl1.Location = ((System.Drawing.Point)(resources.GetObject("tabControl1.Location")));
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.Padding = ((System.Drawing.Point)(resources.GetObject("tabControl1.Padding")));
            this.tabControl1.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("tabControl1.RightToLeft")));
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.ShowToolTips = ((bool)(resources.GetObject("tabControl1.ShowToolTips")));
            this.tabControl1.Size = ((System.Drawing.Size)(resources.GetObject("tabControl1.Size")));
            this.tabControl1.TabIndex = ((int)(resources.GetObject("tabControl1.TabIndex")));
            this.tabControl1.Text = resources.GetString("tabControl1.Text");
            this.tabControl1.Visible = ((bool)(resources.GetObject("tabControl1.Visible")));
            // 
            // tabPage1
            // 
            this.tabPage1.AccessibleDescription = resources.GetString("tabPage1.AccessibleDescription");
            this.tabPage1.AccessibleName = resources.GetString("tabPage1.AccessibleName");
            this.tabPage1.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("tabPage1.Anchor")));
            this.tabPage1.AutoScroll = ((bool)(resources.GetObject("tabPage1.AutoScroll")));
            this.tabPage1.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("tabPage1.AutoScrollMargin")));
            this.tabPage1.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("tabPage1.AutoScrollMinSize")));
            this.tabPage1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("tabPage1.BackgroundImage")));
            this.tabPage1.Controls.Add(this.primitivesRemoveButton);
            this.tabPage1.Controls.Add(this.primitivesResultsTextBox);
            this.tabPage1.Controls.Add(this.primitivesFlushCacheButton);
            this.tabPage1.Controls.Add(this.primitivesReadButton);
            this.tabPage1.Controls.Add(this.primitivesAddButton);
            this.tabPage1.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("tabPage1.Dock")));
            this.tabPage1.Enabled = ((bool)(resources.GetObject("tabPage1.Enabled")));
            this.tabPage1.Font = ((System.Drawing.Font)(resources.GetObject("tabPage1.Font")));
            this.tabPage1.ImageIndex = ((int)(resources.GetObject("tabPage1.ImageIndex")));
            this.tabPage1.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("tabPage1.ImeMode")));
            this.tabPage1.Location = ((System.Drawing.Point)(resources.GetObject("tabPage1.Location")));
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("tabPage1.RightToLeft")));
            this.tabPage1.Size = ((System.Drawing.Size)(resources.GetObject("tabPage1.Size")));
            this.tabPage1.TabIndex = ((int)(resources.GetObject("tabPage1.TabIndex")));
            this.tabPage1.Text = resources.GetString("tabPage1.Text");
            this.tabPage1.ToolTipText = resources.GetString("tabPage1.ToolTipText");
            this.tabPage1.Visible = ((bool)(resources.GetObject("tabPage1.Visible")));
            // 
            // primitivesRemoveButton
            // 
            this.primitivesRemoveButton.AccessibleDescription = resources.GetString("primitivesRemoveButton.AccessibleDescription");
            this.primitivesRemoveButton.AccessibleName = resources.GetString("primitivesRemoveButton.AccessibleName");
            this.primitivesRemoveButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("primitivesRemoveButton.Anchor")));
            this.primitivesRemoveButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("primitivesRemoveButton.BackgroundImage")));
            this.primitivesRemoveButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("primitivesRemoveButton.Dock")));
            this.primitivesRemoveButton.Enabled = ((bool)(resources.GetObject("primitivesRemoveButton.Enabled")));
            this.primitivesRemoveButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("primitivesRemoveButton.FlatStyle")));
            this.primitivesRemoveButton.Font = ((System.Drawing.Font)(resources.GetObject("primitivesRemoveButton.Font")));
            this.primitivesRemoveButton.Image = ((System.Drawing.Image)(resources.GetObject("primitivesRemoveButton.Image")));
            this.primitivesRemoveButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("primitivesRemoveButton.ImageAlign")));
            this.primitivesRemoveButton.ImageIndex = ((int)(resources.GetObject("primitivesRemoveButton.ImageIndex")));
            this.primitivesRemoveButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("primitivesRemoveButton.ImeMode")));
            this.primitivesRemoveButton.Location = ((System.Drawing.Point)(resources.GetObject("primitivesRemoveButton.Location")));
            this.primitivesRemoveButton.Name = "primitivesRemoveButton";
            this.primitivesRemoveButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("primitivesRemoveButton.RightToLeft")));
            this.primitivesRemoveButton.Size = ((System.Drawing.Size)(resources.GetObject("primitivesRemoveButton.Size")));
            this.primitivesRemoveButton.TabIndex = ((int)(resources.GetObject("primitivesRemoveButton.TabIndex")));
            this.primitivesRemoveButton.Text = resources.GetString("primitivesRemoveButton.Text");
            this.primitivesRemoveButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("primitivesRemoveButton.TextAlign")));
            this.primitivesRemoveButton.Visible = ((bool)(resources.GetObject("primitivesRemoveButton.Visible")));
            this.primitivesRemoveButton.Click += new System.EventHandler(this.primitivesRemoveButton_Click);
            // 
            // primitivesResultsTextBox
            // 
            this.primitivesResultsTextBox.AccessibleDescription = resources.GetString("primitivesResultsTextBox.AccessibleDescription");
            this.primitivesResultsTextBox.AccessibleName = resources.GetString("primitivesResultsTextBox.AccessibleName");
            this.primitivesResultsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("primitivesResultsTextBox.Anchor")));
            this.primitivesResultsTextBox.AutoSize = ((bool)(resources.GetObject("primitivesResultsTextBox.AutoSize")));
            this.primitivesResultsTextBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("primitivesResultsTextBox.BackgroundImage")));
            this.primitivesResultsTextBox.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("primitivesResultsTextBox.Dock")));
            this.primitivesResultsTextBox.Enabled = ((bool)(resources.GetObject("primitivesResultsTextBox.Enabled")));
            this.primitivesResultsTextBox.Font = ((System.Drawing.Font)(resources.GetObject("primitivesResultsTextBox.Font")));
            this.primitivesResultsTextBox.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("primitivesResultsTextBox.ImeMode")));
            this.primitivesResultsTextBox.Location = ((System.Drawing.Point)(resources.GetObject("primitivesResultsTextBox.Location")));
            this.primitivesResultsTextBox.MaxLength = ((int)(resources.GetObject("primitivesResultsTextBox.MaxLength")));
            this.primitivesResultsTextBox.Multiline = ((bool)(resources.GetObject("primitivesResultsTextBox.Multiline")));
            this.primitivesResultsTextBox.Name = "primitivesResultsTextBox";
            this.primitivesResultsTextBox.PasswordChar = ((char)(resources.GetObject("primitivesResultsTextBox.PasswordChar")));
            this.primitivesResultsTextBox.ReadOnly = true;
            this.primitivesResultsTextBox.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("primitivesResultsTextBox.RightToLeft")));
            this.primitivesResultsTextBox.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("primitivesResultsTextBox.ScrollBars")));
            this.primitivesResultsTextBox.Size = ((System.Drawing.Size)(resources.GetObject("primitivesResultsTextBox.Size")));
            this.primitivesResultsTextBox.TabIndex = ((int)(resources.GetObject("primitivesResultsTextBox.TabIndex")));
            this.primitivesResultsTextBox.Text = resources.GetString("primitivesResultsTextBox.Text");
            this.primitivesResultsTextBox.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("primitivesResultsTextBox.TextAlign")));
            this.primitivesResultsTextBox.Visible = ((bool)(resources.GetObject("primitivesResultsTextBox.Visible")));
            this.primitivesResultsTextBox.WordWrap = ((bool)(resources.GetObject("primitivesResultsTextBox.WordWrap")));
            // 
            // primitivesFlushCacheButton
            // 
            this.primitivesFlushCacheButton.AccessibleDescription = resources.GetString("primitivesFlushCacheButton.AccessibleDescription");
            this.primitivesFlushCacheButton.AccessibleName = resources.GetString("primitivesFlushCacheButton.AccessibleName");
            this.primitivesFlushCacheButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("primitivesFlushCacheButton.Anchor")));
            this.primitivesFlushCacheButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("primitivesFlushCacheButton.BackgroundImage")));
            this.primitivesFlushCacheButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("primitivesFlushCacheButton.Dock")));
            this.primitivesFlushCacheButton.Enabled = ((bool)(resources.GetObject("primitivesFlushCacheButton.Enabled")));
            this.primitivesFlushCacheButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("primitivesFlushCacheButton.FlatStyle")));
            this.primitivesFlushCacheButton.Font = ((System.Drawing.Font)(resources.GetObject("primitivesFlushCacheButton.Font")));
            this.primitivesFlushCacheButton.Image = ((System.Drawing.Image)(resources.GetObject("primitivesFlushCacheButton.Image")));
            this.primitivesFlushCacheButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("primitivesFlushCacheButton.ImageAlign")));
            this.primitivesFlushCacheButton.ImageIndex = ((int)(resources.GetObject("primitivesFlushCacheButton.ImageIndex")));
            this.primitivesFlushCacheButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("primitivesFlushCacheButton.ImeMode")));
            this.primitivesFlushCacheButton.Location = ((System.Drawing.Point)(resources.GetObject("primitivesFlushCacheButton.Location")));
            this.primitivesFlushCacheButton.Name = "primitivesFlushCacheButton";
            this.primitivesFlushCacheButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("primitivesFlushCacheButton.RightToLeft")));
            this.primitivesFlushCacheButton.Size = ((System.Drawing.Size)(resources.GetObject("primitivesFlushCacheButton.Size")));
            this.primitivesFlushCacheButton.TabIndex = ((int)(resources.GetObject("primitivesFlushCacheButton.TabIndex")));
            this.primitivesFlushCacheButton.Text = resources.GetString("primitivesFlushCacheButton.Text");
            this.primitivesFlushCacheButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("primitivesFlushCacheButton.TextAlign")));
            this.primitivesFlushCacheButton.Visible = ((bool)(resources.GetObject("primitivesFlushCacheButton.Visible")));
            this.primitivesFlushCacheButton.Click += new System.EventHandler(this.primitivesFlushCacheButton_Click);
            // 
            // primitivesReadButton
            // 
            this.primitivesReadButton.AccessibleDescription = resources.GetString("primitivesReadButton.AccessibleDescription");
            this.primitivesReadButton.AccessibleName = resources.GetString("primitivesReadButton.AccessibleName");
            this.primitivesReadButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("primitivesReadButton.Anchor")));
            this.primitivesReadButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("primitivesReadButton.BackgroundImage")));
            this.primitivesReadButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("primitivesReadButton.Dock")));
            this.primitivesReadButton.Enabled = ((bool)(resources.GetObject("primitivesReadButton.Enabled")));
            this.primitivesReadButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("primitivesReadButton.FlatStyle")));
            this.primitivesReadButton.Font = ((System.Drawing.Font)(resources.GetObject("primitivesReadButton.Font")));
            this.primitivesReadButton.Image = ((System.Drawing.Image)(resources.GetObject("primitivesReadButton.Image")));
            this.primitivesReadButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("primitivesReadButton.ImageAlign")));
            this.primitivesReadButton.ImageIndex = ((int)(resources.GetObject("primitivesReadButton.ImageIndex")));
            this.primitivesReadButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("primitivesReadButton.ImeMode")));
            this.primitivesReadButton.Location = ((System.Drawing.Point)(resources.GetObject("primitivesReadButton.Location")));
            this.primitivesReadButton.Name = "primitivesReadButton";
            this.primitivesReadButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("primitivesReadButton.RightToLeft")));
            this.primitivesReadButton.Size = ((System.Drawing.Size)(resources.GetObject("primitivesReadButton.Size")));
            this.primitivesReadButton.TabIndex = ((int)(resources.GetObject("primitivesReadButton.TabIndex")));
            this.primitivesReadButton.Text = resources.GetString("primitivesReadButton.Text");
            this.primitivesReadButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("primitivesReadButton.TextAlign")));
            this.primitivesReadButton.Visible = ((bool)(resources.GetObject("primitivesReadButton.Visible")));
            this.primitivesReadButton.Click += new System.EventHandler(this.primitivesReadButton_Click);
            // 
            // primitivesAddButton
            // 
            this.primitivesAddButton.AccessibleDescription = resources.GetString("primitivesAddButton.AccessibleDescription");
            this.primitivesAddButton.AccessibleName = resources.GetString("primitivesAddButton.AccessibleName");
            this.primitivesAddButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("primitivesAddButton.Anchor")));
            this.primitivesAddButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("primitivesAddButton.BackgroundImage")));
            this.primitivesAddButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("primitivesAddButton.Dock")));
            this.primitivesAddButton.Enabled = ((bool)(resources.GetObject("primitivesAddButton.Enabled")));
            this.primitivesAddButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("primitivesAddButton.FlatStyle")));
            this.primitivesAddButton.Font = ((System.Drawing.Font)(resources.GetObject("primitivesAddButton.Font")));
            this.primitivesAddButton.Image = ((System.Drawing.Image)(resources.GetObject("primitivesAddButton.Image")));
            this.primitivesAddButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("primitivesAddButton.ImageAlign")));
            this.primitivesAddButton.ImageIndex = ((int)(resources.GetObject("primitivesAddButton.ImageIndex")));
            this.primitivesAddButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("primitivesAddButton.ImeMode")));
            this.primitivesAddButton.Location = ((System.Drawing.Point)(resources.GetObject("primitivesAddButton.Location")));
            this.primitivesAddButton.Name = "primitivesAddButton";
            this.primitivesAddButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("primitivesAddButton.RightToLeft")));
            this.primitivesAddButton.Size = ((System.Drawing.Size)(resources.GetObject("primitivesAddButton.Size")));
            this.primitivesAddButton.TabIndex = ((int)(resources.GetObject("primitivesAddButton.TabIndex")));
            this.primitivesAddButton.Text = resources.GetString("primitivesAddButton.Text");
            this.primitivesAddButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("primitivesAddButton.TextAlign")));
            this.primitivesAddButton.Visible = ((bool)(resources.GetObject("primitivesAddButton.Visible")));
            this.primitivesAddButton.Click += new System.EventHandler(this.primitivesAddButton_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.AccessibleDescription = resources.GetString("tabPage2.AccessibleDescription");
            this.tabPage2.AccessibleName = resources.GetString("tabPage2.AccessibleName");
            this.tabPage2.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("tabPage2.Anchor")));
            this.tabPage2.AutoScroll = ((bool)(resources.GetObject("tabPage2.AutoScroll")));
            this.tabPage2.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("tabPage2.AutoScrollMargin")));
            this.tabPage2.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("tabPage2.AutoScrollMinSize")));
            this.tabPage2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("tabPage2.BackgroundImage")));
            this.tabPage2.Controls.Add(this.viewFileButton);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.proactiveButton);
            this.tabPage2.Controls.Add(this.flushCacheButton);
            this.tabPage2.Controls.Add(this.reactiveButton);
            this.tabPage2.Controls.Add(this.loadingResultsTextBox);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("tabPage2.Dock")));
            this.tabPage2.Enabled = ((bool)(resources.GetObject("tabPage2.Enabled")));
            this.tabPage2.Font = ((System.Drawing.Font)(resources.GetObject("tabPage2.Font")));
            this.tabPage2.ImageIndex = ((int)(resources.GetObject("tabPage2.ImageIndex")));
            this.tabPage2.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("tabPage2.ImeMode")));
            this.tabPage2.Location = ((System.Drawing.Point)(resources.GetObject("tabPage2.Location")));
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("tabPage2.RightToLeft")));
            this.tabPage2.Size = ((System.Drawing.Size)(resources.GetObject("tabPage2.Size")));
            this.tabPage2.TabIndex = ((int)(resources.GetObject("tabPage2.TabIndex")));
            this.tabPage2.Text = resources.GetString("tabPage2.Text");
            this.tabPage2.ToolTipText = resources.GetString("tabPage2.ToolTipText");
            this.tabPage2.Visible = ((bool)(resources.GetObject("tabPage2.Visible")));
            // 
            // viewFileButton
            // 
            this.viewFileButton.AccessibleDescription = resources.GetString("viewFileButton.AccessibleDescription");
            this.viewFileButton.AccessibleName = resources.GetString("viewFileButton.AccessibleName");
            this.viewFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("viewFileButton.Anchor")));
            this.viewFileButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("viewFileButton.BackgroundImage")));
            this.viewFileButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("viewFileButton.Dock")));
            this.viewFileButton.Enabled = ((bool)(resources.GetObject("viewFileButton.Enabled")));
            this.viewFileButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("viewFileButton.FlatStyle")));
            this.viewFileButton.Font = ((System.Drawing.Font)(resources.GetObject("viewFileButton.Font")));
            this.viewFileButton.Image = ((System.Drawing.Image)(resources.GetObject("viewFileButton.Image")));
            this.viewFileButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("viewFileButton.ImageAlign")));
            this.viewFileButton.ImageIndex = ((int)(resources.GetObject("viewFileButton.ImageIndex")));
            this.viewFileButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("viewFileButton.ImeMode")));
            this.viewFileButton.Location = ((System.Drawing.Point)(resources.GetObject("viewFileButton.Location")));
            this.viewFileButton.Name = "viewFileButton";
            this.viewFileButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("viewFileButton.RightToLeft")));
            this.viewFileButton.Size = ((System.Drawing.Size)(resources.GetObject("viewFileButton.Size")));
            this.viewFileButton.TabIndex = ((int)(resources.GetObject("viewFileButton.TabIndex")));
            this.viewFileButton.Text = resources.GetString("viewFileButton.Text");
            this.viewFileButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("viewFileButton.TextAlign")));
            this.viewFileButton.Visible = ((bool)(resources.GetObject("viewFileButton.Visible")));
            this.viewFileButton.Click += new System.EventHandler(this.viewFileButton_Click);
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
            // proactiveButton
            // 
            this.proactiveButton.AccessibleDescription = resources.GetString("proactiveButton.AccessibleDescription");
            this.proactiveButton.AccessibleName = resources.GetString("proactiveButton.AccessibleName");
            this.proactiveButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("proactiveButton.Anchor")));
            this.proactiveButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("proactiveButton.BackgroundImage")));
            this.proactiveButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("proactiveButton.Dock")));
            this.proactiveButton.Enabled = ((bool)(resources.GetObject("proactiveButton.Enabled")));
            this.proactiveButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("proactiveButton.FlatStyle")));
            this.proactiveButton.Font = ((System.Drawing.Font)(resources.GetObject("proactiveButton.Font")));
            this.proactiveButton.Image = ((System.Drawing.Image)(resources.GetObject("proactiveButton.Image")));
            this.proactiveButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("proactiveButton.ImageAlign")));
            this.proactiveButton.ImageIndex = ((int)(resources.GetObject("proactiveButton.ImageIndex")));
            this.proactiveButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("proactiveButton.ImeMode")));
            this.proactiveButton.Location = ((System.Drawing.Point)(resources.GetObject("proactiveButton.Location")));
            this.proactiveButton.Name = "proactiveButton";
            this.proactiveButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("proactiveButton.RightToLeft")));
            this.proactiveButton.Size = ((System.Drawing.Size)(resources.GetObject("proactiveButton.Size")));
            this.proactiveButton.TabIndex = ((int)(resources.GetObject("proactiveButton.TabIndex")));
            this.proactiveButton.Text = resources.GetString("proactiveButton.Text");
            this.proactiveButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("proactiveButton.TextAlign")));
            this.proactiveButton.Visible = ((bool)(resources.GetObject("proactiveButton.Visible")));
            this.proactiveButton.Click += new System.EventHandler(this.proactiveButton_Click);
            // 
            // flushCacheButton
            // 
            this.flushCacheButton.AccessibleDescription = resources.GetString("flushCacheButton.AccessibleDescription");
            this.flushCacheButton.AccessibleName = resources.GetString("flushCacheButton.AccessibleName");
            this.flushCacheButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("flushCacheButton.Anchor")));
            this.flushCacheButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("flushCacheButton.BackgroundImage")));
            this.flushCacheButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("flushCacheButton.Dock")));
            this.flushCacheButton.Enabled = ((bool)(resources.GetObject("flushCacheButton.Enabled")));
            this.flushCacheButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("flushCacheButton.FlatStyle")));
            this.flushCacheButton.Font = ((System.Drawing.Font)(resources.GetObject("flushCacheButton.Font")));
            this.flushCacheButton.Image = ((System.Drawing.Image)(resources.GetObject("flushCacheButton.Image")));
            this.flushCacheButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("flushCacheButton.ImageAlign")));
            this.flushCacheButton.ImageIndex = ((int)(resources.GetObject("flushCacheButton.ImageIndex")));
            this.flushCacheButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("flushCacheButton.ImeMode")));
            this.flushCacheButton.Location = ((System.Drawing.Point)(resources.GetObject("flushCacheButton.Location")));
            this.flushCacheButton.Name = "flushCacheButton";
            this.flushCacheButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("flushCacheButton.RightToLeft")));
            this.flushCacheButton.Size = ((System.Drawing.Size)(resources.GetObject("flushCacheButton.Size")));
            this.flushCacheButton.TabIndex = ((int)(resources.GetObject("flushCacheButton.TabIndex")));
            this.flushCacheButton.Text = resources.GetString("flushCacheButton.Text");
            this.flushCacheButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("flushCacheButton.TextAlign")));
            this.flushCacheButton.Visible = ((bool)(resources.GetObject("flushCacheButton.Visible")));
            this.flushCacheButton.Click += new System.EventHandler(this.flushCacheButton_Click);
            // 
            // reactiveButton
            // 
            this.reactiveButton.AccessibleDescription = resources.GetString("reactiveButton.AccessibleDescription");
            this.reactiveButton.AccessibleName = resources.GetString("reactiveButton.AccessibleName");
            this.reactiveButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("reactiveButton.Anchor")));
            this.reactiveButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("reactiveButton.BackgroundImage")));
            this.reactiveButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("reactiveButton.Dock")));
            this.reactiveButton.Enabled = ((bool)(resources.GetObject("reactiveButton.Enabled")));
            this.reactiveButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("reactiveButton.FlatStyle")));
            this.reactiveButton.Font = ((System.Drawing.Font)(resources.GetObject("reactiveButton.Font")));
            this.reactiveButton.Image = ((System.Drawing.Image)(resources.GetObject("reactiveButton.Image")));
            this.reactiveButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("reactiveButton.ImageAlign")));
            this.reactiveButton.ImageIndex = ((int)(resources.GetObject("reactiveButton.ImageIndex")));
            this.reactiveButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("reactiveButton.ImeMode")));
            this.reactiveButton.Location = ((System.Drawing.Point)(resources.GetObject("reactiveButton.Location")));
            this.reactiveButton.Name = "reactiveButton";
            this.reactiveButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("reactiveButton.RightToLeft")));
            this.reactiveButton.Size = ((System.Drawing.Size)(resources.GetObject("reactiveButton.Size")));
            this.reactiveButton.TabIndex = ((int)(resources.GetObject("reactiveButton.TabIndex")));
            this.reactiveButton.Text = resources.GetString("reactiveButton.Text");
            this.reactiveButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("reactiveButton.TextAlign")));
            this.reactiveButton.Visible = ((bool)(resources.GetObject("reactiveButton.Visible")));
            this.reactiveButton.Click += new System.EventHandler(this.reactiveButton_Click);
            // 
            // loadingResultsTextBox
            // 
            this.loadingResultsTextBox.AccessibleDescription = resources.GetString("loadingResultsTextBox.AccessibleDescription");
            this.loadingResultsTextBox.AccessibleName = resources.GetString("loadingResultsTextBox.AccessibleName");
            this.loadingResultsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("loadingResultsTextBox.Anchor")));
            this.loadingResultsTextBox.AutoSize = ((bool)(resources.GetObject("loadingResultsTextBox.AutoSize")));
            this.loadingResultsTextBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("loadingResultsTextBox.BackgroundImage")));
            this.loadingResultsTextBox.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("loadingResultsTextBox.Dock")));
            this.loadingResultsTextBox.Enabled = ((bool)(resources.GetObject("loadingResultsTextBox.Enabled")));
            this.loadingResultsTextBox.Font = ((System.Drawing.Font)(resources.GetObject("loadingResultsTextBox.Font")));
            this.loadingResultsTextBox.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("loadingResultsTextBox.ImeMode")));
            this.loadingResultsTextBox.Location = ((System.Drawing.Point)(resources.GetObject("loadingResultsTextBox.Location")));
            this.loadingResultsTextBox.MaxLength = ((int)(resources.GetObject("loadingResultsTextBox.MaxLength")));
            this.loadingResultsTextBox.Multiline = ((bool)(resources.GetObject("loadingResultsTextBox.Multiline")));
            this.loadingResultsTextBox.Name = "loadingResultsTextBox";
            this.loadingResultsTextBox.PasswordChar = ((char)(resources.GetObject("loadingResultsTextBox.PasswordChar")));
            this.loadingResultsTextBox.ReadOnly = true;
            this.loadingResultsTextBox.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("loadingResultsTextBox.RightToLeft")));
            this.loadingResultsTextBox.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("loadingResultsTextBox.ScrollBars")));
            this.loadingResultsTextBox.Size = ((System.Drawing.Size)(resources.GetObject("loadingResultsTextBox.Size")));
            this.loadingResultsTextBox.TabIndex = ((int)(resources.GetObject("loadingResultsTextBox.TabIndex")));
            this.loadingResultsTextBox.Text = resources.GetString("loadingResultsTextBox.Text");
            this.loadingResultsTextBox.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("loadingResultsTextBox.TextAlign")));
            this.loadingResultsTextBox.Visible = ((bool)(resources.GetObject("loadingResultsTextBox.Visible")));
            this.loadingResultsTextBox.WordWrap = ((bool)(resources.GetObject("loadingResultsTextBox.WordWrap")));
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
            // QuickStartForm
            // 
            this.AccessibleDescription = resources.GetString("$this.AccessibleDescription");
            this.AccessibleName = resources.GetString("$this.AccessibleName");
            this.AutoScroll = ((bool)(resources.GetObject("$this.AutoScroll")));
            this.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMargin")));
            this.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMinSize")));
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = ((System.Drawing.Size)(resources.GetObject("$this.ClientSize")));
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.groupBox);
            this.Controls.Add(this.groupBox1);
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
            this.groupBox.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AppForm = new QuickStartForm();

            // Unhandled exceptions will be delivered to our ThreadException handler
            Application.ThreadException += new ThreadExceptionEventHandler(AppThreadException);

            Application.Run(AppForm);
        }

        void primitivesAddButton_Click(object sender,
                                       EventArgs e)
        {
            // Prompt the user to enter information about the product,
            // as well as properties to use when adding it to the cache
            if (enterNewItemForm.ShowDialog() == DialogResult.OK)
            {
                Product product = new Product(
                    enterNewItemForm.ProductID,
                    enterNewItemForm.ProductShortName,
                    enterNewItemForm.ProductPrice);

                // Add the product to the cache, using the expiration and
                // priority settings according to what the user entered.
                switch (enterNewItemForm.Expiration)
                {
                    case (ExpirationType.AbsoluteTime):
                        primitivesCache.Add(product.ProductID, product, enterNewItemForm.Priority, new ProductCacheRefreshAction(),
                                            new AbsoluteTime(enterNewItemForm.AbsoluteTime));
                        break;
                    case (ExpirationType.ExtendedFormat):
                        primitivesCache.Add(product.ProductID, product, enterNewItemForm.Priority, new ProductCacheRefreshAction(),
                                            new ExtendedFormatTime("0 0 * * *"));
                        break;
                    case (ExpirationType.FileDependency):
                        primitivesCache.Add(product.ProductID, product, enterNewItemForm.Priority, new ProductCacheRefreshAction(),
                                            new FileDependency("DependencyFile.txt"));
                        break;
                    case (ExpirationType.SlidingTime):
                        primitivesCache.Add(product.ProductID, product, enterNewItemForm.Priority, new ProductCacheRefreshAction(),
                                            new SlidingTime(TimeSpan.FromMinutes(1)));
                        break;
                }

                // Update the results text box to display information about the item just added
                primitivesResultsTextBox.Text +=
                    string.Format(Resources.Culture, Resources.AddItemToCacheMessage, product.ProductID, product.ProductName, product.ProductPrice,
                                  enterNewItemForm.Expiration.ToString(),
                                  enterNewItemForm.Priority.ToString()) + Environment.NewLine;

                AddScenarioSeparator(primitivesResultsTextBox);
            }
        }

        void primitivesFlushCacheButton_Click(object sender,
                                              EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                primitivesCache.Flush();
                primitivesResultsTextBox.Clear();
                primitivesResultsTextBox.Text += Resources.FlushCacheMessage + Environment.NewLine;
                AddScenarioSeparator(primitivesResultsTextBox);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        void primitivesReadButton_Click(object sender,
                                        EventArgs e)
        {
            selectItemForm.ClearInputTextBox();
            selectItemForm.Text = Resources.ReadItemTitleMessage;
            selectItemForm.SetInstructionLabelText(Resources.ReadItemMessage);

            if (selectItemForm.ShowDialog() == DialogResult.OK)
            {
                Product product = (Product)primitivesCache.GetData(selectItemForm.ItemKey);
                if (product != null)
                {
                    primitivesResultsTextBox.Text += string.Format(Resources.Culture, Resources.ReadItemFromCacheMessage, product.ProductID, product.ProductName, product.ProductPrice) + Environment.NewLine;
                }
                else
                {
                    primitivesResultsTextBox.Text += string.Format(Resources.Culture, Resources.ItemNotFoundMessage, selectItemForm.ItemKey) + Environment.NewLine;
                }
                AddScenarioSeparator(primitivesResultsTextBox);
            }
        }

        /// <summary>
        /// Attempts to remove the item specified by the user from the cache.
        /// </summary>
        void primitivesRemoveButton_Click(object sender,
                                          EventArgs e)
        {
            selectItemForm.ClearInputTextBox();
            selectItemForm.ClearInputTextBox();
            selectItemForm.Text = Resources.RemoveItemTitleMessage;
            selectItemForm.SetInstructionLabelText(Resources.RemoveItemMessage);

            if (selectItemForm.ShowDialog() == DialogResult.OK)
            {
                primitivesCache.Remove(selectItemForm.ItemKey);

                primitivesResultsTextBox.Text += Resources.RemoveItemFromCacheMessage + Environment.NewLine;

                AddScenarioSeparator(primitivesResultsTextBox);
            }
        }

        void proactiveButton_Click(object sender,
                                   EventArgs e)
        {
            productData.LoadAllProducts();

            loadingResultsTextBox.Text += Resources.ProactiveLoadMessage + Environment.NewLine;
            AddScenarioSeparator(loadingResultsTextBox);
        }

        void QuickStartForm_Load(object sender,
                                 EventArgs e)
        {
            // Initialize image to embedded logo
            logoPictureBox.Image = GetEmbeddedImage("CachingQuickStart.logo.gif");

            // Use the default cache manager for the primitive operations
            primitivesCache = CacheFactory.GetCacheManager();

            productData = new ProductData();

            // Initialize primitive operations forms
            enterNewItemForm = new EnterNewItemForm();
            selectItemForm = new SelectItemForm();
            selectMasterDataItemForm = new SelectMasterDataItemForm();
        }

        void quitButton_Click(object sender,
                              EventArgs e)
        {
            Application.Exit();
        }

        void reactiveButton_Click(object sender,
                                  EventArgs e)
        {
            // Prompt the user to see which product should be retrieved
            if (selectMasterDataItemForm.ShowDialog() == DialogResult.OK)
            {
                Product product = productData.ReadProductByID(selectMasterDataItemForm.ItemToRead);

                loadingResultsTextBox.Text += productData.dataSourceMessage;
                AddScenarioSeparator(loadingResultsTextBox);
            }
        }

        /// <summary>
        /// Reads an item and updates the results display.
        /// This method should be used for both proactive and reactive readings of products.
        /// </summary>
        /// <param name="key">Key of the item to be read</param>
        /// <returns>Returns true if item was read, false if not available</returns>
        bool ReadSingleProduct(string key)
        {
            Product product = productData.ReadProductByID(key);
            loadingResultsTextBox.Text += productData.dataSourceMessage;
            return (product != null);
        }

        void viewFileButton_Click(object sender,
                                  EventArgs e)
        {
            // Process has never been started. Initialize and launch notepad.
            if (masterFileViewerProcess == null)
            {
                // Initialize the Process information for notepad
                masterFileViewerProcess = new Process();

                masterFileViewerProcess.StartInfo.FileName = MasterDataViewerExecutable;
                masterFileViewerProcess.StartInfo.Arguments = DataProvider.dataFileName;
                masterFileViewerProcess.Start();
            }
            else if (masterFileViewerProcess.HasExited)
            {
                // Process previously started, then exited. Start the process again.
                masterFileViewerProcess.Start();
            }
            else
            {
                // Process was already started - bring it to the foreground
                IntPtr hWnd = masterFileViewerProcess.MainWindowHandle;
                if (NativeMethods.IsIconic(hWnd))
                {
                    NativeMethods.ShowWindowAsync(hWnd, NativeMethods.SW_RESTORE);
                }

                NativeMethods.SetForegroundWindow(hWnd);
            }
        }

        void viewWalkthroughButton_Click(object sender,
                                         EventArgs e)
        {
            // Process has never been started. Initialize and launch the viewer.
            if (viewerProcess == null)
            {
                // Initialize the Process information for the help viewer
                viewerProcess = new Process();

                viewerProcess.StartInfo.FileName = GetHelpViewerExecutable();
                viewerProcess.StartInfo.Arguments = HelpViewerArguments;
                viewerProcess.Start();
            }
            else if (viewerProcess.HasExited)
            {
                // Process previously started, then exited. Start the process again.
                viewerProcess.Start();
            }
            else
            {
                // Process was already started - bring it to the foreground
                IntPtr hWnd = viewerProcess.MainWindowHandle;
                if (NativeMethods.IsIconic(hWnd))
                {
                    NativeMethods.ShowWindowAsync(hWnd, NativeMethods.SW_RESTORE);
                }
                NativeMethods.SetForegroundWindow(hWnd);
            }
        }
    }
}