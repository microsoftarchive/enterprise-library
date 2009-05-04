//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block QuickStart
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

namespace SecurityQuickStart
{
	/// <summary>
	/// Used to create new users and authenticate existing ones.
	/// </summary>
	public class CredentialsForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.TextBox usernameTextBox;
		private System.Windows.Forms.TextBox passwordTextBox;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public CredentialsForm()
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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CredentialsForm));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.usernameTextBox = new System.Windows.Forms.TextBox();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
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
            // usernameTextBox
            // 
            this.usernameTextBox.AccessibleDescription = resources.GetString("usernameTextBox.AccessibleDescription");
            this.usernameTextBox.AccessibleName = resources.GetString("usernameTextBox.AccessibleName");
            this.usernameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("usernameTextBox.Anchor")));
            this.usernameTextBox.AutoSize = ((bool)(resources.GetObject("usernameTextBox.AutoSize")));
            this.usernameTextBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("usernameTextBox.BackgroundImage")));
            this.usernameTextBox.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("usernameTextBox.Dock")));
            this.usernameTextBox.Enabled = ((bool)(resources.GetObject("usernameTextBox.Enabled")));
            this.usernameTextBox.Font = ((System.Drawing.Font)(resources.GetObject("usernameTextBox.Font")));
            this.usernameTextBox.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("usernameTextBox.ImeMode")));
            this.usernameTextBox.Location = ((System.Drawing.Point)(resources.GetObject("usernameTextBox.Location")));
            this.usernameTextBox.MaxLength = ((int)(resources.GetObject("usernameTextBox.MaxLength")));
            this.usernameTextBox.Multiline = ((bool)(resources.GetObject("usernameTextBox.Multiline")));
            this.usernameTextBox.Name = "usernameTextBox";
            this.usernameTextBox.PasswordChar = ((char)(resources.GetObject("usernameTextBox.PasswordChar")));
            this.usernameTextBox.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("usernameTextBox.RightToLeft")));
            this.usernameTextBox.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("usernameTextBox.ScrollBars")));
            this.usernameTextBox.Size = ((System.Drawing.Size)(resources.GetObject("usernameTextBox.Size")));
            this.usernameTextBox.TabIndex = ((int)(resources.GetObject("usernameTextBox.TabIndex")));
            this.usernameTextBox.Text = resources.GetString("usernameTextBox.Text");
            this.usernameTextBox.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("usernameTextBox.TextAlign")));
            this.usernameTextBox.Visible = ((bool)(resources.GetObject("usernameTextBox.Visible")));
            this.usernameTextBox.WordWrap = ((bool)(resources.GetObject("usernameTextBox.WordWrap")));
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.AccessibleDescription = resources.GetString("passwordTextBox.AccessibleDescription");
            this.passwordTextBox.AccessibleName = resources.GetString("passwordTextBox.AccessibleName");
            this.passwordTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("passwordTextBox.Anchor")));
            this.passwordTextBox.AutoSize = ((bool)(resources.GetObject("passwordTextBox.AutoSize")));
            this.passwordTextBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("passwordTextBox.BackgroundImage")));
            this.passwordTextBox.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("passwordTextBox.Dock")));
            this.passwordTextBox.Enabled = ((bool)(resources.GetObject("passwordTextBox.Enabled")));
            this.passwordTextBox.Font = ((System.Drawing.Font)(resources.GetObject("passwordTextBox.Font")));
            this.passwordTextBox.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("passwordTextBox.ImeMode")));
            this.passwordTextBox.Location = ((System.Drawing.Point)(resources.GetObject("passwordTextBox.Location")));
            this.passwordTextBox.MaxLength = ((int)(resources.GetObject("passwordTextBox.MaxLength")));
            this.passwordTextBox.Multiline = ((bool)(resources.GetObject("passwordTextBox.Multiline")));
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.PasswordChar = ((char)(resources.GetObject("passwordTextBox.PasswordChar")));
            this.passwordTextBox.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("passwordTextBox.RightToLeft")));
            this.passwordTextBox.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("passwordTextBox.ScrollBars")));
            this.passwordTextBox.Size = ((System.Drawing.Size)(resources.GetObject("passwordTextBox.Size")));
            this.passwordTextBox.TabIndex = ((int)(resources.GetObject("passwordTextBox.TabIndex")));
            this.passwordTextBox.Text = resources.GetString("passwordTextBox.Text");
            this.passwordTextBox.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("passwordTextBox.TextAlign")));
            this.passwordTextBox.Visible = ((bool)(resources.GetObject("passwordTextBox.Visible")));
            this.passwordTextBox.WordWrap = ((bool)(resources.GetObject("passwordTextBox.WordWrap")));
            // 
            // CredentialsForm
            // 
            this.AcceptButton = this.okButton;
            this.AccessibleDescription = resources.GetString("$this.AccessibleDescription");
            this.AccessibleName = resources.GetString("$this.AccessibleName");
            this.AutoScaleBaseSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScaleBaseSize")));
            this.AutoScroll = ((bool)(resources.GetObject("$this.AutoScroll")));
            this.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMargin")));
            this.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMinSize")));
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.CancelButton = this.cancelButton;
            this.ClientSize = ((System.Drawing.Size)(resources.GetObject("$this.ClientSize")));
            this.Controls.Add(this.passwordTextBox);
            this.Controls.Add(this.usernameTextBox);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
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
            this.Name = "CredentialsForm";
            this.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("$this.RightToLeft")));
            this.ShowInTaskbar = false;
            this.StartPosition = ((System.Windows.Forms.FormStartPosition)(resources.GetObject("$this.StartPosition")));
            this.Text = resources.GetString("$this.Text");
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
		#endregion

		/// <summary>
		/// Entered user name
		/// </summary>
		public string Username
		{
			get { return this.usernameTextBox.Text; }
		}

		/// <summary>
		/// Entered user password
		/// </summary>
		public string Password
		{
			get { return this.passwordTextBox.Text; }
		}

		/// <summary>
		/// Accepts user input
		/// </summary>
		/// <param name="sender">Event sender</param>
		/// <param name="e">Event arguments</param>
		private void okButton_Click(object sender, System.EventArgs e)
		{
			if (this.usernameTextBox.Text.CompareTo("")==0 ||
				this.passwordTextBox.Text.CompareTo("")==0)
			{
				MessageBox.Show("Please enter a user name and a password",
					"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			else
			{
				this.DialogResult = DialogResult.OK;
				this.Close();
			}
		}

		/// <summary>
		/// Discards user input
		/// </summary>
		/// <param name="sender">Event sender</param>
		/// <param name="e">Event arguments</param>
		private void cancelButton_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		protected override void OnActivated(EventArgs e)
		{
			base.OnActivated (e);

			this.usernameTextBox.Clear();
			this.passwordTextBox.Clear();

			this.usernameTextBox.Focus();
		}
	}
}
