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
    partial class ExportKeyControl
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
            this.pnlExportKeyFile = new System.Windows.Forms.Panel();
            this.btnBrowseExportKeyLocation = new System.Windows.Forms.Button();
            this.lblPasswordExportKey = new System.Windows.Forms.Label();
            this.txtPassword2 = new System.Windows.Forms.TextBox();
            this.lblConfirmPasswordExportKey = new System.Windows.Forms.Label();
            this.txtPassword1 = new System.Windows.Forms.TextBox();
            this.txtExportFileLocation = new System.Windows.Forms.TextBox();
            this.lblExportKeyFileMessage = new System.Windows.Forms.Label();
            this.browseDialog = new System.Windows.Forms.SaveFileDialog();
            this.pnlExportKeyFile.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlExportKeyFile
            // 
            this.pnlExportKeyFile.Controls.Add(this.btnBrowseExportKeyLocation);
            this.pnlExportKeyFile.Controls.Add(this.lblPasswordExportKey);
            this.pnlExportKeyFile.Controls.Add(this.txtPassword2);
            this.pnlExportKeyFile.Controls.Add(this.lblConfirmPasswordExportKey);
            this.pnlExportKeyFile.Controls.Add(this.txtPassword1);
            this.pnlExportKeyFile.Controls.Add(this.txtExportFileLocation);
            this.pnlExportKeyFile.Controls.Add(this.lblExportKeyFileMessage);
            this.pnlExportKeyFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlExportKeyFile.Location = new System.Drawing.Point(0, 0);
            this.pnlExportKeyFile.Name = "pnlExportKeyFile";
            this.pnlExportKeyFile.Size = new System.Drawing.Size(351, 178);
            this.pnlExportKeyFile.TabIndex = 7;
            // 
            // btnBrowseExportKeyLocation
            // 
            this.btnBrowseExportKeyLocation.Location = new System.Drawing.Point(264, 52);
            this.btnBrowseExportKeyLocation.Name = "btnBrowseExportKeyLocation";
            this.btnBrowseExportKeyLocation.Size = new System.Drawing.Size(41, 23);
            this.btnBrowseExportKeyLocation.TabIndex = 7;
            this.btnBrowseExportKeyLocation.Text = "...";
            this.btnBrowseExportKeyLocation.UseVisualStyleBackColor = true;
            this.btnBrowseExportKeyLocation.Click += new System.EventHandler(this.btnBrowseExportKeyLocation_Click);
            // 
            // lblPasswordExportKey
            // 
            this.lblPasswordExportKey.Location = new System.Drawing.Point(14, 79);
            this.lblPasswordExportKey.Name = "lblPasswordExportKey";
            this.lblPasswordExportKey.Size = new System.Drawing.Size(323, 21);
            this.lblPasswordExportKey.TabIndex = 6;
            this.lblPasswordExportKey.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // txtPassword2
            // 
            this.txtPassword2.Location = new System.Drawing.Point(16, 152);
            this.txtPassword2.Name = "txtPassword2";
            this.txtPassword2.PasswordChar = '*';
            this.txtPassword2.Size = new System.Drawing.Size(130, 20);
            this.txtPassword2.TabIndex = 5;
            // 
            // lblConfirmPasswordExportKey
            // 
            this.lblConfirmPasswordExportKey.Location = new System.Drawing.Point(12, 128);
            this.lblConfirmPasswordExportKey.Name = "lblConfirmPasswordExportKey";
            this.lblConfirmPasswordExportKey.Size = new System.Drawing.Size(323, 21);
            this.lblConfirmPasswordExportKey.TabIndex = 4;
            this.lblConfirmPasswordExportKey.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // txtPassword1
            // 
            this.txtPassword1.Location = new System.Drawing.Point(16, 105);
            this.txtPassword1.Name = "txtPassword1";
            this.txtPassword1.PasswordChar = '*';
            this.txtPassword1.Size = new System.Drawing.Size(130, 20);
            this.txtPassword1.TabIndex = 3;
            // 
            // txtExportFileLocation
            // 
            this.txtExportFileLocation.Location = new System.Drawing.Point(16, 54);
            this.txtExportFileLocation.Name = "txtExportFileLocation";
            this.txtExportFileLocation.Size = new System.Drawing.Size(237, 20);
            this.txtExportFileLocation.TabIndex = 1;
            // 
            // lblExportKeyFileMessage
            // 
            this.lblExportKeyFileMessage.Location = new System.Drawing.Point(13, 13);
            this.lblExportKeyFileMessage.Name = "lblExportKeyFileMessage";
            this.lblExportKeyFileMessage.Size = new System.Drawing.Size(326, 37);
            this.lblExportKeyFileMessage.TabIndex = 0;
            // 
            // ExportKeyControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlExportKeyFile);
            this.Name = "ExportKeyControl";
            this.Size = new System.Drawing.Size(351, 178);
            this.pnlExportKeyFile.ResumeLayout(false);
            this.pnlExportKeyFile.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlExportKeyFile;
        private System.Windows.Forms.Button btnBrowseExportKeyLocation;
        private System.Windows.Forms.Label lblPasswordExportKey;
        private System.Windows.Forms.TextBox txtPassword2;
        private System.Windows.Forms.Label lblConfirmPasswordExportKey;
        private System.Windows.Forms.TextBox txtPassword1;
        private System.Windows.Forms.TextBox txtExportFileLocation;
        private System.Windows.Forms.Label lblExportKeyFileMessage;
        private System.Windows.Forms.SaveFileDialog browseDialog;
    }
}
