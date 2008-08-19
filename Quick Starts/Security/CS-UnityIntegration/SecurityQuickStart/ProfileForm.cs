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
	/// Summary description for ProfileForm.
	/// </summary>
	public class ProfileForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button cancelButton;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.TextBox firstNameTextBox;
		private System.Windows.Forms.TextBox lastNameTextBox;
		private System.Windows.Forms.ComboBox themeComboBox;

		// Variable for storing profile
		private ProfileInformation profile;

		public ProfileForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this.profile = new ProfileInformation();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProfileForm));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.firstNameTextBox = new System.Windows.Forms.TextBox();
            this.lastNameTextBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.themeComboBox = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // firstNameTextBox
            // 
            resources.ApplyResources(this.firstNameTextBox, "firstNameTextBox");
            this.firstNameTextBox.Name = "firstNameTextBox";
            // 
            // lastNameTextBox
            // 
            resources.ApplyResources(this.lastNameTextBox, "lastNameTextBox");
            this.lastNameTextBox.Name = "lastNameTextBox";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cancelButton);
            this.groupBox1.Controls.Add(this.okButton);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.cancelButton, "cancelButton");
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // okButton
            // 
            resources.ApplyResources(this.okButton, "okButton");
            this.okButton.Name = "okButton";
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // themeComboBox
            // 
            this.themeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.themeComboBox, "themeComboBox");
            this.themeComboBox.Items.AddRange(new object[] {
            resources.GetString("themeComboBox.Items"),
            resources.GetString("themeComboBox.Items1"),
            resources.GetString("themeComboBox.Items2"),
            resources.GetString("themeComboBox.Items3")});
            this.themeComboBox.Name = "themeComboBox";
            // 
            // ProfileForm
            // 
            this.AcceptButton = this.okButton;
            resources.ApplyResources(this, "$this");
            this.CancelButton = this.cancelButton;
            this.Controls.Add(this.themeComboBox);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lastNameTextBox);
            this.Controls.Add(this.firstNameTextBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProfileForm";
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.ProfileForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
		#endregion

		public ProfileInformation Profile
		{
			get { return this.profile; }
		}

		protected override void OnActivated(EventArgs e)
		{
			this.firstNameTextBox.Focus();
		}


		private void okButton_Click(object sender, System.EventArgs e)
		{
			this.profile.FirstName = this.firstNameTextBox.Text;
			this.profile.LastName = this.lastNameTextBox.Text;
			this.profile.Theme = (ProfileTheme)this.themeComboBox.SelectedIndex;
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void cancelButton_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void ProfileForm_Load(object sender, System.EventArgs e)
		{
			this.themeComboBox.Items.Clear();
			
			foreach ( string theme in Enum.GetNames(typeof(ProfileTheme)) )
			{
				this.themeComboBox.Items.Add(theme);
			}	
			
			this.firstNameTextBox.Text = this.profile.FirstName;	
			this.lastNameTextBox.Text = this.profile.LastName;
			int activeTheme = this.themeComboBox.FindString(this.profile.Theme.ToString());
			this.themeComboBox.SelectedIndex = activeTheme;
		}
	}
}
