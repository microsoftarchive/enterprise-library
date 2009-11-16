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
    partial class ImportArchivedKeyControl
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
            this.pnlChooseImportFile = new System.Windows.Forms.Panel();
            this.txtPasswordImportFile = new System.Windows.Forms.TextBox();
            this.lblImportFilePassword = new System.Windows.Forms.Label();
            this.btnBrowseImportFileLocation = new System.Windows.Forms.Button();
            this.txtChooseImportFileLocation = new System.Windows.Forms.TextBox();
            this.lblChooseImportFileMessage = new System.Windows.Forms.Label();
            this.dlgOpenKeyArchive = new System.Windows.Forms.OpenFileDialog();
            this.pnlChooseImportFile.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlChooseImportFile
            // 
            this.pnlChooseImportFile.Controls.Add(this.txtPasswordImportFile);
            this.pnlChooseImportFile.Controls.Add(this.lblImportFilePassword);
            this.pnlChooseImportFile.Controls.Add(this.btnBrowseImportFileLocation);
            this.pnlChooseImportFile.Controls.Add(this.txtChooseImportFileLocation);
            this.pnlChooseImportFile.Controls.Add(this.lblChooseImportFileMessage);
            this.pnlChooseImportFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlChooseImportFile.Location = new System.Drawing.Point(0, 0);
            this.pnlChooseImportFile.Name = "pnlChooseImportFile";
            this.pnlChooseImportFile.Size = new System.Drawing.Size(340, 192);
            this.pnlChooseImportFile.TabIndex = 6;
            // 
            // txtPasswordImportFile
            // 
            this.txtPasswordImportFile.Location = new System.Drawing.Point(16, 134);
            this.txtPasswordImportFile.Name = "txtPasswordImportFile";
            this.txtPasswordImportFile.PasswordChar = '*';
            this.txtPasswordImportFile.Size = new System.Drawing.Size(237, 20);
            this.txtPasswordImportFile.TabIndex = 4;
            // 
            // lblImportFilePassword
            // 
            this.lblImportFilePassword.Location = new System.Drawing.Point(16, 107);
            this.lblImportFilePassword.Name = "lblImportFilePassword";
            this.lblImportFilePassword.Size = new System.Drawing.Size(321, 23);
            this.lblImportFilePassword.TabIndex = 3;
            // 
            // btnBrowseImportFileLocation
            // 
            this.btnBrowseImportFileLocation.Location = new System.Drawing.Point(259, 63);
            this.btnBrowseImportFileLocation.Name = "btnBrowseImportFileLocation";
            this.btnBrowseImportFileLocation.Size = new System.Drawing.Size(41, 23);
            this.btnBrowseImportFileLocation.TabIndex = 2;
            this.btnBrowseImportFileLocation.Text = "...";
            this.btnBrowseImportFileLocation.UseVisualStyleBackColor = true;
            this.btnBrowseImportFileLocation.Click += new System.EventHandler(this.btnBrowseImportFileLocation_Click);
            // 
            // txtChooseImportFileLocation
            // 
            this.txtChooseImportFileLocation.Location = new System.Drawing.Point(16, 65);
            this.txtChooseImportFileLocation.Name = "txtChooseImportFileLocation";
            this.txtChooseImportFileLocation.Size = new System.Drawing.Size(237, 20);
            this.txtChooseImportFileLocation.TabIndex = 1;
            // 
            // lblChooseImportFileMessage
            // 
            this.lblChooseImportFileMessage.Location = new System.Drawing.Point(13, 13);
            this.lblChooseImportFileMessage.Name = "lblChooseImportFileMessage";
            this.lblChooseImportFileMessage.Size = new System.Drawing.Size(326, 48);
            this.lblChooseImportFileMessage.TabIndex = 0;
            // 
            // ImportKeyControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlChooseImportFile);
            this.Name = "ImportKeyControl";
            this.Size = new System.Drawing.Size(340, 192);
            this.pnlChooseImportFile.ResumeLayout(false);
            this.pnlChooseImportFile.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlChooseImportFile;
        private System.Windows.Forms.TextBox txtPasswordImportFile;
        private System.Windows.Forms.Label lblImportFilePassword;
        private System.Windows.Forms.Button btnBrowseImportFileLocation;
        private System.Windows.Forms.TextBox txtChooseImportFileLocation;
        private System.Windows.Forms.Label lblChooseImportFileMessage;
        private System.Windows.Forms.OpenFileDialog dlgOpenKeyArchive;

    }
}
