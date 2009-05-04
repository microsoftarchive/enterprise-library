//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Cryptography Application Block QuickStart
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using CryptographyQuickStart.Properties;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

namespace CryptographyQuickStart
{
	/// <summary>
	/// Enterprise Library Cryptography Application Block Quick Start Sample.
	/// </summary>
	public class QuickStartForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox groupBox;
		private System.Windows.Forms.Button viewWalkthroughButton;
		private System.Windows.Forms.Button quitButton;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.PictureBox logoPictureBox;
		private System.Windows.Forms.TextBox resultsTextBox;
		private System.Windows.Forms.Button encryptButton;
		private System.Windows.Forms.Button decryptButton;
		private System.Windows.Forms.Button getHashButton;
		private System.Windows.Forms.Button checkTextButton;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private Process viewerProcess = null;
		private const string HelpViewerArguments = @"/helpcol ms-help://MS.VSCC.v90/MS.VSIPCC.v90/ms.practices.entlib.2008oct /LaunchFKeywordTopic CryptographyQS1";

		// The following strings correspond to the names of the providers as 
		// specified by the configuration file.
		private const string hashProvider = "hashprovider";
		private const string symmProvider = "symprovider";

		// The file name where the DPAPI-protected symmetric key is stored
		private static readonly string symmKeyFileName = "SymmetricKeyFile.txt";

		// Encrypted string
		//private byte[] encryptedContents;
		private string encryptedContentsBase64;

		// Generated hash
		private byte[] generatedHash;

		// Form to let users enter text for different scenarios.
		private InputForm inputForm;

		private CryptographyManager cryptographyManager;

		public static System.Windows.Forms.Form AppForm;

		public QuickStartForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this.inputForm = new InputForm();
		}

		public QuickStartForm(CryptographyManager cryptographyManager)
			: this()
		{
			this.cryptographyManager = cryptographyManager;
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
			this.groupBox = new System.Windows.Forms.GroupBox();
			this.viewWalkthroughButton = new System.Windows.Forms.Button();
			this.quitButton = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label2 = new System.Windows.Forms.Label();
			this.logoPictureBox = new System.Windows.Forms.PictureBox();
			this.resultsTextBox = new System.Windows.Forms.TextBox();
			this.encryptButton = new System.Windows.Forms.Button();
			this.decryptButton = new System.Windows.Forms.Button();
			this.getHashButton = new System.Windows.Forms.Button();
			this.checkTextButton = new System.Windows.Forms.Button();
			this.groupBox.SuspendLayout();
			this.groupBox1.SuspendLayout();
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
			// encryptButton
			// 
			this.encryptButton.AccessibleDescription = resources.GetString("encryptButton.AccessibleDescription");
			this.encryptButton.AccessibleName = resources.GetString("encryptButton.AccessibleName");
			this.encryptButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("encryptButton.Anchor")));
			this.encryptButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("encryptButton.BackgroundImage")));
			this.encryptButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("encryptButton.Dock")));
			this.encryptButton.Enabled = ((bool)(resources.GetObject("encryptButton.Enabled")));
			this.encryptButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("encryptButton.FlatStyle")));
			this.encryptButton.Font = ((System.Drawing.Font)(resources.GetObject("encryptButton.Font")));
			this.encryptButton.Image = ((System.Drawing.Image)(resources.GetObject("encryptButton.Image")));
			this.encryptButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("encryptButton.ImageAlign")));
			this.encryptButton.ImageIndex = ((int)(resources.GetObject("encryptButton.ImageIndex")));
			this.encryptButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("encryptButton.ImeMode")));
			this.encryptButton.Location = ((System.Drawing.Point)(resources.GetObject("encryptButton.Location")));
			this.encryptButton.Name = "encryptButton";
			this.encryptButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("encryptButton.RightToLeft")));
			this.encryptButton.Size = ((System.Drawing.Size)(resources.GetObject("encryptButton.Size")));
			this.encryptButton.TabIndex = ((int)(resources.GetObject("encryptButton.TabIndex")));
			this.encryptButton.Text = resources.GetString("encryptButton.Text");
			this.encryptButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("encryptButton.TextAlign")));
			this.encryptButton.Visible = ((bool)(resources.GetObject("encryptButton.Visible")));
			this.encryptButton.Click += new System.EventHandler(this.encryptButton_Click);
			// 
			// decryptButton
			// 
			this.decryptButton.AccessibleDescription = resources.GetString("decryptButton.AccessibleDescription");
			this.decryptButton.AccessibleName = resources.GetString("decryptButton.AccessibleName");
			this.decryptButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("decryptButton.Anchor")));
			this.decryptButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("decryptButton.BackgroundImage")));
			this.decryptButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("decryptButton.Dock")));
			this.decryptButton.Enabled = ((bool)(resources.GetObject("decryptButton.Enabled")));
			this.decryptButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("decryptButton.FlatStyle")));
			this.decryptButton.Font = ((System.Drawing.Font)(resources.GetObject("decryptButton.Font")));
			this.decryptButton.Image = ((System.Drawing.Image)(resources.GetObject("decryptButton.Image")));
			this.decryptButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("decryptButton.ImageAlign")));
			this.decryptButton.ImageIndex = ((int)(resources.GetObject("decryptButton.ImageIndex")));
			this.decryptButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("decryptButton.ImeMode")));
			this.decryptButton.Location = ((System.Drawing.Point)(resources.GetObject("decryptButton.Location")));
			this.decryptButton.Name = "decryptButton";
			this.decryptButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("decryptButton.RightToLeft")));
			this.decryptButton.Size = ((System.Drawing.Size)(resources.GetObject("decryptButton.Size")));
			this.decryptButton.TabIndex = ((int)(resources.GetObject("decryptButton.TabIndex")));
			this.decryptButton.Text = resources.GetString("decryptButton.Text");
			this.decryptButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("decryptButton.TextAlign")));
			this.decryptButton.Visible = ((bool)(resources.GetObject("decryptButton.Visible")));
			this.decryptButton.Click += new System.EventHandler(this.decryptButton_Click);
			// 
			// getHashButton
			// 
			this.getHashButton.AccessibleDescription = resources.GetString("getHashButton.AccessibleDescription");
			this.getHashButton.AccessibleName = resources.GetString("getHashButton.AccessibleName");
			this.getHashButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("getHashButton.Anchor")));
			this.getHashButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("getHashButton.BackgroundImage")));
			this.getHashButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("getHashButton.Dock")));
			this.getHashButton.Enabled = ((bool)(resources.GetObject("getHashButton.Enabled")));
			this.getHashButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("getHashButton.FlatStyle")));
			this.getHashButton.Font = ((System.Drawing.Font)(resources.GetObject("getHashButton.Font")));
			this.getHashButton.Image = ((System.Drawing.Image)(resources.GetObject("getHashButton.Image")));
			this.getHashButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("getHashButton.ImageAlign")));
			this.getHashButton.ImageIndex = ((int)(resources.GetObject("getHashButton.ImageIndex")));
			this.getHashButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("getHashButton.ImeMode")));
			this.getHashButton.Location = ((System.Drawing.Point)(resources.GetObject("getHashButton.Location")));
			this.getHashButton.Name = "getHashButton";
			this.getHashButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("getHashButton.RightToLeft")));
			this.getHashButton.Size = ((System.Drawing.Size)(resources.GetObject("getHashButton.Size")));
			this.getHashButton.TabIndex = ((int)(resources.GetObject("getHashButton.TabIndex")));
			this.getHashButton.Text = resources.GetString("getHashButton.Text");
			this.getHashButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("getHashButton.TextAlign")));
			this.getHashButton.Visible = ((bool)(resources.GetObject("getHashButton.Visible")));
			this.getHashButton.Click += new System.EventHandler(this.getHashButton_Click);
			// 
			// checkTextButton
			// 
			this.checkTextButton.AccessibleDescription = resources.GetString("checkTextButton.AccessibleDescription");
			this.checkTextButton.AccessibleName = resources.GetString("checkTextButton.AccessibleName");
			this.checkTextButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("checkTextButton.Anchor")));
			this.checkTextButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("checkTextButton.BackgroundImage")));
			this.checkTextButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("checkTextButton.Dock")));
			this.checkTextButton.Enabled = ((bool)(resources.GetObject("checkTextButton.Enabled")));
			this.checkTextButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("checkTextButton.FlatStyle")));
			this.checkTextButton.Font = ((System.Drawing.Font)(resources.GetObject("checkTextButton.Font")));
			this.checkTextButton.Image = ((System.Drawing.Image)(resources.GetObject("checkTextButton.Image")));
			this.checkTextButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("checkTextButton.ImageAlign")));
			this.checkTextButton.ImageIndex = ((int)(resources.GetObject("checkTextButton.ImageIndex")));
			this.checkTextButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("checkTextButton.ImeMode")));
			this.checkTextButton.Location = ((System.Drawing.Point)(resources.GetObject("checkTextButton.Location")));
			this.checkTextButton.Name = "checkTextButton";
			this.checkTextButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("checkTextButton.RightToLeft")));
			this.checkTextButton.Size = ((System.Drawing.Size)(resources.GetObject("checkTextButton.Size")));
			this.checkTextButton.TabIndex = ((int)(resources.GetObject("checkTextButton.TabIndex")));
			this.checkTextButton.Text = resources.GetString("checkTextButton.Text");
			this.checkTextButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("checkTextButton.TextAlign")));
			this.checkTextButton.Visible = ((bool)(resources.GetObject("checkTextButton.Visible")));
			this.checkTextButton.Click += new System.EventHandler(this.checkTextButton_Click);
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
			this.Controls.Add(this.checkTextButton);
			this.Controls.Add(this.getHashButton);
			this.Controls.Add(this.decryptButton);
			this.Controls.Add(this.groupBox);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.resultsTextBox);
			this.Controls.Add(this.encryptButton);
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
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			try
			{
				CreateKeys();
			}
			catch (IOException ioe)
			{
				MessageBox.Show(String.Format(Resources.UnableToWriteKeyFileErrorMessage, ioe.Message),
					Resources.KeyFileErrorTitle,
					MessageBoxButtons.OK,
					MessageBoxIcon.Error);
				return;
			}

			IUnityContainer container = new UnityContainer();
			UnityConfigurationSection section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
			section.Containers.Default.Configure(container);

			AppForm = container.Resolve<QuickStartForm>();

			// Unhandled exceptions will be delivered to our ThreadException handler
			Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(AppThreadException);

			Application.Run(AppForm);
		}

		private static void CreateKeys()
		{
			string installedPath = ConfigurationManager.AppSettings["InstallPath"];
			string fileName = Path.Combine(installedPath, symmKeyFileName);

			ProtectedKey symmetricKey = KeyManager.GenerateSymmetricKey(typeof(RijndaelManaged), DataProtectionScope.LocalMachine);
			using (FileStream keyStream = new FileStream(fileName, FileMode.Create))
			{
				KeyManager.Write(keyStream, symmetricKey);
			}
		}

		/// <summary>
		/// Displays dialog with information about exceptions that occur in the application. 
		/// </summary>
		private static void AppThreadException(object source, System.Threading.ThreadExceptionEventArgs e)
		{
			ProcessUnhandledException(e.Exception);
		}

		private void QuickStartForm_Load(object sender, System.EventArgs e)
		{
			// Initialize image to embedded logo
			this.logoPictureBox.Image = GetEmbeddedImage("CryptographyQuickStart.logo.gif");

			this.decryptButton.Enabled = false;
			this.checkTextButton.Enabled = false;
		}

		private System.Drawing.Image GetEmbeddedImage(string resourceName)
		{
			Stream resourceStream = Assembly.GetEntryAssembly().GetManifestResourceStream(resourceName);

			if (resourceStream == null)
			{
				return null;
			}

			System.Drawing.Image img = System.Drawing.Image.FromStream(resourceStream);

			return img;
		}

		private void DisplayResults(string results)
		{
			this.resultsTextBox.Text += results + Environment.NewLine;
			this.resultsTextBox.SelectAll();
			this.resultsTextBox.ScrollToCaret();
		}

		private void quitButton_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		/// <summary>
		/// Encrypt a sample text using the default symmetric provider.
		/// </summary>
		/// <param name="sender">Event sender</param>
		/// <param name="e">Event arguments</param>
		private void encryptButton_Click(object sender, System.EventArgs e)
		{
			if (this.inputForm != null)
			{
				this.inputForm.Title = Resources.EncryptTitleMessage;
				this.inputForm.InstructionsText = Resources.EncryptInstructionsMessage;
				if (this.inputForm.ShowDialog() == DialogResult.OK)
				{
					try
					{
						this.Cursor = Cursors.WaitCursor;

						string valueToEncrypt = this.inputForm.Input;

						this.encryptedContentsBase64 = this.cryptographyManager.EncryptSymmetric(symmProvider, this.inputForm.Input);

						this.DisplayResults(string.Format(Resources.Culture, Resources.OriginalTextMessage, this.inputForm.Input));
						this.DisplayResults(string.Format(Resources.EncryptedTextMessage, this.encryptedContentsBase64));
					}
					catch (Exception ex)
					{
						ProcessUnhandledException(ex);
					}
					finally
					{
						this.decryptButton.Enabled = true;
						this.Cursor = Cursors.Default;
					}
				}
			}
		}

		/// <summary>
		/// Decrypts a set of bytes and displayed the decrypted contents.
		/// </summary>
		/// <param name="sender">Event sender.</param>
		/// <param name="e">Event arguments.</param>
		private void decryptButton_Click(object sender, System.EventArgs e)
		{
			if (this.encryptedContentsBase64 != null)
			{
				try
				{
					this.Cursor = Cursors.WaitCursor;

					// Descrypt and display value
					string readableString = this.cryptographyManager.DecryptSymmetric(symmProvider, this.encryptedContentsBase64);
					this.DisplayResults(string.Format(Resources.Culture, Resources.DecryptedTextMessage, readableString));
				}
				catch (Exception ex)
				{
					ProcessUnhandledException(ex);
				}
				finally
				{
					this.Cursor = Cursors.Default;
				}
			}
			else
			{
				this.DisplayResults(Resources.DecryptErrorMessage);
			}
		}

		/// <summary>
		/// Creates a hash based on a sample text, for further comparison.
		/// </summary>
		/// <param name="sender">Event sender.</param>
		/// <param name="e">Event arguments.</param>
		private void getHashButton_Click(object sender, System.EventArgs e)
		{
			if (this.inputForm != null)
			{
				this.inputForm.Title = Resources.HashTitleMessage;
				this.inputForm.InstructionsText = Resources.HashInstructionsMessage;
				if (this.inputForm.ShowDialog() == DialogResult.OK)
				{
					try
					{

						this.Cursor = Cursors.WaitCursor;

						this.generatedHash = GetHash(this.inputForm.Input);

						this.DisplayResults(string.Format(Resources.Culture, Resources.HashMessage, Convert.ToBase64String(this.generatedHash)));

						this.checkTextButton.Enabled = true;

						this.Cursor = Cursors.Arrow;
					}
					catch (Exception ex)
					{
						ProcessUnhandledException(ex);
					}
					finally
					{
						this.Cursor = Cursors.Default;
					}

				}
			}
		}

		private byte[] GetHash(string plainText)
		{
			byte[] valueToHash = System.Text.Encoding.UTF8.GetBytes(plainText);

			byte[] generatedHash = this.cryptographyManager.CreateHash(hashProvider, valueToHash);

			// Clear the byte array memory
			Array.Clear(valueToHash, 0, valueToHash.Length);

			return generatedHash;
		}

		private bool CompareHash(string plainText, byte[] existingHashValue)
		{
			byte[] valueToHash = System.Text.Encoding.UTF8.GetBytes(plainText);

			bool matched = this.cryptographyManager.CompareHash(hashProvider, valueToHash, existingHashValue);

			// Clear the byte array memory
			Array.Clear(valueToHash, 0, valueToHash.Length);

			return matched;
		}
		/// <summary>
		/// Check a previously generated hash in order to determine whether the original text
		/// has been tampered or not.
		/// </summary>
		/// <param name="sender">Event sender.</param>
		/// <param name="e">Event arguments.</param>
		private void checkTextButton_Click(object sender, System.EventArgs e)
		{
			if (this.inputForm != null)
			{
				this.inputForm.Title = Resources.HashTitleMessage;
				this.inputForm.InstructionsText = Resources.HashInstructionsMessage;

				if (this.inputForm.ShowDialog() == DialogResult.OK)
				{
					if (this.generatedHash != null)
					{
						try
						{
							this.Cursor = Cursors.WaitCursor;

							bool comparisonSucceeded = this.CompareHash(this.inputForm.Input, this.generatedHash);

							if (comparisonSucceeded)
							{
								this.DisplayResults(Resources.TextNotTamperedMessage);
							}
							else
							{
								this.DisplayResults(Resources.TextTamperedMessage);
							}

						}
						catch (Exception ex)
						{
							ProcessUnhandledException(ex);
						}
						finally
						{
							this.Cursor = Cursors.Default;
						}

					}
				}
			}
		}

		/// <summary>
		/// Returns the path and executable name for the help viewer.
		/// </summary>
		private string GetHelpViewerExecutable()
		{
            string commonX86 = Environment.GetEnvironmentVariable("CommonProgramFiles(x86)");
            if (!string.IsNullOrEmpty(commonX86))
            {
                string pathX86 = Path.Combine(commonX86, @"Microsoft Shared\Help 9\dexplore.exe");
                if (File.Exists(pathX86))
                {
                    return pathX86;
                }
            }
            string common = Environment.GetEnvironmentVariable("CommonProgramFiles");
            return Path.Combine(common, @"Microsoft Shared\Help 9\dexplore.exe");
        }

		private void viewWalkthroughButton_Click(object sender, System.EventArgs e)
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

		/// <summary>
		/// Process any unhandled exceptions that occur in the application.
		/// This code is called by all UI entry points in the application (e.g. button click events)
		/// when an unhandled exception occurs.
		/// You could also achieve this by handling the Application.ThreadException event, however
		/// the VS2005 debugger will break before this event is called.
		/// </summary>
		/// <param name="ex">The unhandled exception</param>
		private static void ProcessUnhandledException(Exception ex)
		{
			StringBuilder errorMessage = new StringBuilder();
			errorMessage.AppendFormat(new CultureInfo("en-us", true), "The following error occured during execution of the Cryptography QuickStart.\n\n{0}\n\n", ex.Message);
			errorMessage.Append("Exceptions can be caused by invalid configuration information.\n");
			errorMessage.Append(Environment.NewLine);
			errorMessage.Append("Do you want to exit the application?");

			DialogResult result = MessageBox.Show(errorMessage.ToString(), "Application Error", MessageBoxButtons.YesNo, MessageBoxIcon.Stop);

			// Exits the program when the user clicks Abort.
			if (result == DialogResult.Yes)
			{
				Application.Exit();
			}
			QuickStartForm.AppForm.Cursor = Cursors.Default;
		}
	}
}
