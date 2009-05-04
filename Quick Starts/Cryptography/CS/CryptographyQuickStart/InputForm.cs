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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace CryptographyQuickStart
{
	/// <summary>
	/// Accepts user input to be used in different cryptography scenarios
	/// such as encrypting/decrypting text and creating hashes based on text.
	/// </summary>
	public class InputForm : System.Windows.Forms.Form
	{
		private string input;

		private System.Windows.Forms.Label instructionsLabel;
		private System.Windows.Forms.TextBox inputTextBox;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public InputForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(InputForm));
            this.instructionsLabel = new System.Windows.Forms.Label();
            this.inputTextBox = new System.Windows.Forms.TextBox();
            this.okButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // instructionsLabel
            // 
            this.instructionsLabel.AccessibleDescription = resources.GetString("instructionsLabel.AccessibleDescription");
            this.instructionsLabel.AccessibleName = resources.GetString("instructionsLabel.AccessibleName");
            this.instructionsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("instructionsLabel.Anchor")));
            this.instructionsLabel.AutoSize = ((bool)(resources.GetObject("instructionsLabel.AutoSize")));
            this.instructionsLabel.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("instructionsLabel.Dock")));
            this.instructionsLabel.Enabled = ((bool)(resources.GetObject("instructionsLabel.Enabled")));
            this.instructionsLabel.Font = ((System.Drawing.Font)(resources.GetObject("instructionsLabel.Font")));
            this.instructionsLabel.Image = ((System.Drawing.Image)(resources.GetObject("instructionsLabel.Image")));
            this.instructionsLabel.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("instructionsLabel.ImageAlign")));
            this.instructionsLabel.ImageIndex = ((int)(resources.GetObject("instructionsLabel.ImageIndex")));
            this.instructionsLabel.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("instructionsLabel.ImeMode")));
            this.instructionsLabel.Location = ((System.Drawing.Point)(resources.GetObject("instructionsLabel.Location")));
            this.instructionsLabel.Name = "instructionsLabel";
            this.instructionsLabel.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("instructionsLabel.RightToLeft")));
            this.instructionsLabel.Size = ((System.Drawing.Size)(resources.GetObject("instructionsLabel.Size")));
            this.instructionsLabel.TabIndex = ((int)(resources.GetObject("instructionsLabel.TabIndex")));
            this.instructionsLabel.Text = resources.GetString("instructionsLabel.Text");
            this.instructionsLabel.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("instructionsLabel.TextAlign")));
            this.instructionsLabel.Visible = ((bool)(resources.GetObject("instructionsLabel.Visible")));
            // 
            // inputTextBox
            // 
            this.inputTextBox.AccessibleDescription = resources.GetString("inputTextBox.AccessibleDescription");
            this.inputTextBox.AccessibleName = resources.GetString("inputTextBox.AccessibleName");
            this.inputTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("inputTextBox.Anchor")));
            this.inputTextBox.AutoSize = ((bool)(resources.GetObject("inputTextBox.AutoSize")));
            this.inputTextBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("inputTextBox.BackgroundImage")));
            this.inputTextBox.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("inputTextBox.Dock")));
            this.inputTextBox.Enabled = ((bool)(resources.GetObject("inputTextBox.Enabled")));
            this.inputTextBox.Font = ((System.Drawing.Font)(resources.GetObject("inputTextBox.Font")));
            this.inputTextBox.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("inputTextBox.ImeMode")));
            this.inputTextBox.Location = ((System.Drawing.Point)(resources.GetObject("inputTextBox.Location")));
            this.inputTextBox.MaxLength = ((int)(resources.GetObject("inputTextBox.MaxLength")));
            this.inputTextBox.Multiline = ((bool)(resources.GetObject("inputTextBox.Multiline")));
            this.inputTextBox.Name = "inputTextBox";
            this.inputTextBox.PasswordChar = ((char)(resources.GetObject("inputTextBox.PasswordChar")));
            this.inputTextBox.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("inputTextBox.RightToLeft")));
            this.inputTextBox.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("inputTextBox.ScrollBars")));
            this.inputTextBox.Size = ((System.Drawing.Size)(resources.GetObject("inputTextBox.Size")));
            this.inputTextBox.TabIndex = ((int)(resources.GetObject("inputTextBox.TabIndex")));
            this.inputTextBox.Text = resources.GetString("inputTextBox.Text");
            this.inputTextBox.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("inputTextBox.TextAlign")));
            this.inputTextBox.Visible = ((bool)(resources.GetObject("inputTextBox.Visible")));
            this.inputTextBox.WordWrap = ((bool)(resources.GetObject("inputTextBox.WordWrap")));
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
            // InputForm
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
            this.Controls.Add(this.inputTextBox);
            this.Controls.Add(this.instructionsLabel);
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
            this.Name = "InputForm";
            this.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("$this.RightToLeft")));
            this.ShowInTaskbar = false;
            this.StartPosition = ((System.Windows.Forms.FormStartPosition)(resources.GetObject("$this.StartPosition")));
            this.Text = resources.GetString("$this.Text");
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
		#endregion

		/// <summary>
		/// Input text entered by user.
		/// </summary>
		public string Input
		{
			get { return this.input; }
		}

		/// <summary>
		/// Text for the instructions label. Should be set before displaying
		/// this instance.
		/// </summary>
		public string InstructionsText
		{
			get { return this.instructionsLabel.Text; }
			set { this.instructionsLabel.Text = value; }
		}

		/// <summary>
		/// Text for window title. Should be set before displaying this instance.
		/// </summary>
		public string Title
		{
			get { return this.Text; }
			set { this.Text = value; } 
		}

		/// <summary>
		/// Handles controls in a proper way before this instance can be seen
		/// by user.
		/// </summary>
		protected override void OnActivated(EventArgs e)
		{
			base.OnActivated (e);

			this.inputTextBox.Clear();
			this.inputTextBox.Focus();
		}

		/// <summary>
		/// Accepts the user input.
		/// </summary>
		private void okButton_Click(object sender, System.EventArgs e)
		{
			this.input = this.inputTextBox.Text;
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		/// <summary>
		/// Discards the user input.
		/// </summary>
		private void cancelButton_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}
	}
}
