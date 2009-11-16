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

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design
{
    partial class ChooseKeyFileControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.pnlChooseKeyFile = new System.Windows.Forms.Panel();
			this.btnBrowseKeyFileLocation = new System.Windows.Forms.Button();
			this.txtKeyFileLocation = new System.Windows.Forms.TextBox();
			this.lblChooseKeyFileMessage = new System.Windows.Forms.Label();
			this.dlgSaveProtectedKey = new System.Windows.Forms.SaveFileDialog();
			this.pnlChooseKeyFile.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlChooseKeyFile
			// 
			this.pnlChooseKeyFile.Controls.Add(this.btnBrowseKeyFileLocation);
			this.pnlChooseKeyFile.Controls.Add(this.txtKeyFileLocation);
			this.pnlChooseKeyFile.Controls.Add(this.lblChooseKeyFileMessage);
			this.pnlChooseKeyFile.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlChooseKeyFile.Location = new System.Drawing.Point(0, 0);
			this.pnlChooseKeyFile.Name = "pnlChooseKeyFile";
			this.pnlChooseKeyFile.Size = new System.Drawing.Size(351, 178);
			this.pnlChooseKeyFile.TabIndex = 3;
			// 
			// btnBrowseKeyFileLocation
			// 
			this.btnBrowseKeyFileLocation.Location = new System.Drawing.Point(259, 84);
			this.btnBrowseKeyFileLocation.Name = "btnBrowseKeyFileLocation";
			this.btnBrowseKeyFileLocation.Size = new System.Drawing.Size(41, 23);
			this.btnBrowseKeyFileLocation.TabIndex = 2;
			this.btnBrowseKeyFileLocation.Text = "...";
			this.btnBrowseKeyFileLocation.UseVisualStyleBackColor = true;
			this.btnBrowseKeyFileLocation.Click += new System.EventHandler(this.btnBrowseKeyFileLocation_Click);
			// 
			// txtKeyFileLocation
			// 
			this.txtKeyFileLocation.BackColor = System.Drawing.SystemColors.InactiveBorder;
			this.txtKeyFileLocation.Location = new System.Drawing.Point(16, 84);
			this.txtKeyFileLocation.Name = "txtKeyFileLocation";
			this.txtKeyFileLocation.ReadOnly = true;
			this.txtKeyFileLocation.Size = new System.Drawing.Size(237, 20);
			this.txtKeyFileLocation.TabIndex = 1;
			// 
			// lblChooseKeyFileMessage
			// 
			this.lblChooseKeyFileMessage.Location = new System.Drawing.Point(13, 13);
			this.lblChooseKeyFileMessage.Name = "lblChooseKeyFileMessage";
			this.lblChooseKeyFileMessage.Size = new System.Drawing.Size(326, 46);
			this.lblChooseKeyFileMessage.TabIndex = 0;
			// 
			// ChooseKeyFileControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.pnlChooseKeyFile);
			this.Name = "ChooseKeyFileControl";
			this.Size = new System.Drawing.Size(351, 178);
			this.pnlChooseKeyFile.ResumeLayout(false);
			this.pnlChooseKeyFile.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlChooseKeyFile;
        private System.Windows.Forms.Button btnBrowseKeyFileLocation;
        private System.Windows.Forms.TextBox txtKeyFileLocation;
        private System.Windows.Forms.Label lblChooseKeyFileMessage;
        private System.Windows.Forms.SaveFileDialog dlgSaveProtectedKey;
    }
}
