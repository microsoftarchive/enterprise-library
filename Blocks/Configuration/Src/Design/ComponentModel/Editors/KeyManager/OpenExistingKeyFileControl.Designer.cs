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
    partial class OpenExistingKeyFileControl
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
            this.pnlChooseExistingKeyFileLocation = new System.Windows.Forms.Panel();
            this.btnBrowseExistingKeyFileLocation = new System.Windows.Forms.Button();
            this.txtChooseExistingKeyFileLocation = new System.Windows.Forms.TextBox();
            this.lblChooseExistingKeyFileLocation = new System.Windows.Forms.Label();
            this.dlgLoadKeyfile = new System.Windows.Forms.OpenFileDialog();
            this.pnlChooseExistingKeyFileLocation.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlChooseExistingKeyFileLocation
            // 
            this.pnlChooseExistingKeyFileLocation.Controls.Add(this.btnBrowseExistingKeyFileLocation);
            this.pnlChooseExistingKeyFileLocation.Controls.Add(this.txtChooseExistingKeyFileLocation);
            this.pnlChooseExistingKeyFileLocation.Controls.Add(this.lblChooseExistingKeyFileLocation);
            this.pnlChooseExistingKeyFileLocation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlChooseExistingKeyFileLocation.Location = new System.Drawing.Point(0, 0);
            this.pnlChooseExistingKeyFileLocation.Name = "pnlChooseExistingKeyFileLocation";
            this.pnlChooseExistingKeyFileLocation.Size = new System.Drawing.Size(351, 178);
            this.pnlChooseExistingKeyFileLocation.TabIndex = 5;
            // 
            // btnBrowseExistingKeyFileLocation
            // 
            this.btnBrowseExistingKeyFileLocation.Location = new System.Drawing.Point(259, 70);
            this.btnBrowseExistingKeyFileLocation.Name = "btnBrowseExistingKeyFileLocation";
            this.btnBrowseExistingKeyFileLocation.Size = new System.Drawing.Size(41, 23);
            this.btnBrowseExistingKeyFileLocation.TabIndex = 2;
            this.btnBrowseExistingKeyFileLocation.Text = "...";
            this.btnBrowseExistingKeyFileLocation.UseVisualStyleBackColor = true;
            this.btnBrowseExistingKeyFileLocation.Click += new System.EventHandler(this.btnBrowseExistingKeyFileLocation_Click);
            // 
            // txtChooseExistingKeyFileLocation
            // 
            this.txtChooseExistingKeyFileLocation.Location = new System.Drawing.Point(16, 72);
            this.txtChooseExistingKeyFileLocation.Name = "txtChooseExistingKeyFileLocation";
            this.txtChooseExistingKeyFileLocation.Size = new System.Drawing.Size(237, 20);
            this.txtChooseExistingKeyFileLocation.TabIndex = 1;
            // 
            // lblChooseExistingKeyFileLocation
            // 
            this.lblChooseExistingKeyFileLocation.Location = new System.Drawing.Point(12, 9);
            this.lblChooseExistingKeyFileLocation.Name = "lblChooseExistingKeyFileLocation";
            this.lblChooseExistingKeyFileLocation.Size = new System.Drawing.Size(330, 43);
            this.lblChooseExistingKeyFileLocation.TabIndex = 0;
            // 
            // LoadDpapiKeyFile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlChooseExistingKeyFileLocation);
            this.Name = "LoadDpapiKeyFile";
            this.Size = new System.Drawing.Size(351, 178);
            this.pnlChooseExistingKeyFileLocation.ResumeLayout(false);
            this.pnlChooseExistingKeyFileLocation.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlChooseExistingKeyFileLocation;
        private System.Windows.Forms.Button btnBrowseExistingKeyFileLocation;
        private System.Windows.Forms.TextBox txtChooseExistingKeyFileLocation;
        private System.Windows.Forms.Label lblChooseExistingKeyFileLocation;
        private System.Windows.Forms.OpenFileDialog dlgLoadKeyfile;
    }
}
