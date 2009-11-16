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
    partial class SupplyKeyControl
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
            this.pnlSupplyKey = new System.Windows.Forms.Panel();
            this.rbImportKey = new System.Windows.Forms.RadioButton();
            this.rbUseExistingKey = new System.Windows.Forms.RadioButton();
            this.rbCreateNewKey = new System.Windows.Forms.RadioButton();
            this.lblSupplyKeyMessage = new System.Windows.Forms.Label();
            this.pnlSupplyKey.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlSupplyKey
            // 
            this.pnlSupplyKey.Controls.Add(this.rbImportKey);
            this.pnlSupplyKey.Controls.Add(this.rbUseExistingKey);
            this.pnlSupplyKey.Controls.Add(this.rbCreateNewKey);
            this.pnlSupplyKey.Controls.Add(this.lblSupplyKeyMessage);
            this.pnlSupplyKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSupplyKey.Location = new System.Drawing.Point(0, 0);
            this.pnlSupplyKey.Name = "pnlSupplyKey";
            this.pnlSupplyKey.Size = new System.Drawing.Size(351, 178);
            this.pnlSupplyKey.TabIndex = 1;
            // 
            // rbImportKey
            // 
            this.rbImportKey.AutoSize = true;
            this.rbImportKey.Location = new System.Drawing.Point(45, 108);
            this.rbImportKey.Name = "rbImportKey";
            this.rbImportKey.Size = new System.Drawing.Size(195, 17);
            this.rbImportKey.TabIndex = 3;
            this.rbImportKey.UseVisualStyleBackColor = true;
            // 
            // rbUseExistingKey
            // 
            this.rbUseExistingKey.AutoSize = true;
            this.rbUseExistingKey.Location = new System.Drawing.Point(45, 85);
            this.rbUseExistingKey.Name = "rbUseExistingKey";
            this.rbUseExistingKey.Size = new System.Drawing.Size(219, 17);
            this.rbUseExistingKey.TabIndex = 2;
            this.rbUseExistingKey.UseVisualStyleBackColor = true;
            // 
            // rbCreateNewKey
            // 
            this.rbCreateNewKey.AutoSize = true;
            this.rbCreateNewKey.Checked = true;
            this.rbCreateNewKey.Location = new System.Drawing.Point(45, 62);
            this.rbCreateNewKey.Name = "rbCreateNewKey";
            this.rbCreateNewKey.Size = new System.Drawing.Size(108, 17);
            this.rbCreateNewKey.TabIndex = 1;
            this.rbCreateNewKey.TabStop = true;
            this.rbCreateNewKey.UseVisualStyleBackColor = true;
            // 
            // lblSupplyKeyMessage
            // 
            this.lblSupplyKeyMessage.Location = new System.Drawing.Point(13, 13);
            this.lblSupplyKeyMessage.Name = "lblSupplyKeyMessage";
            this.lblSupplyKeyMessage.Size = new System.Drawing.Size(326, 46);
            this.lblSupplyKeyMessage.TabIndex = 0;
            // 
            // SupplyKeyControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlSupplyKey);
            this.Name = "SupplyKeyControl";
            this.Size = new System.Drawing.Size(351, 178);
            this.pnlSupplyKey.ResumeLayout(false);
            this.pnlSupplyKey.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlSupplyKey;
        private System.Windows.Forms.RadioButton rbImportKey;
        private System.Windows.Forms.RadioButton rbUseExistingKey;
        private System.Windows.Forms.RadioButton rbCreateNewKey;
        private System.Windows.Forms.Label lblSupplyKeyMessage;
    }
}
