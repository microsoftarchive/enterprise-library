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
    partial class CreateNewKeyControl
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
			this.pnlCreateKey = new System.Windows.Forms.Panel();
			this.generateKeyButton = new System.Windows.Forms.Button();
			this.keyBox = new System.Windows.Forms.TextBox();
			this.lblCreateKeyMessage = new System.Windows.Forms.Label();
			this.pnlCreateKey.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlCreateKey
			// 
			this.pnlCreateKey.Controls.Add(this.generateKeyButton);
			this.pnlCreateKey.Controls.Add(this.keyBox);
			this.pnlCreateKey.Controls.Add(this.lblCreateKeyMessage);
			this.pnlCreateKey.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlCreateKey.Location = new System.Drawing.Point(0, 0);
			this.pnlCreateKey.Name = "pnlCreateKey";
			this.pnlCreateKey.Size = new System.Drawing.Size(351, 178);
			this.pnlCreateKey.TabIndex = 2;
			// 
			// generateKeyButton
			// 
			this.generateKeyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.generateKeyButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.generateKeyButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.generateKeyButton.Location = new System.Drawing.Point(264, 152);
			this.generateKeyButton.Name = "generateKeyButton";
			this.generateKeyButton.Size = new System.Drawing.Size(75, 23);
			this.generateKeyButton.TabIndex = 6;
			this.generateKeyButton.Click += new System.EventHandler(this.generateKeyButton_Click);
			// 
			// keyBox
			// 
			this.keyBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.keyBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
			this.keyBox.Font = new System.Drawing.Font("Courier New", 10F);
			this.keyBox.Location = new System.Drawing.Point(16, 49);
			this.keyBox.Multiline = true;
			this.keyBox.Name = "keyBox";
			this.keyBox.Size = new System.Drawing.Size(323, 97);
			this.keyBox.TabIndex = 5;
			// 
			// lblCreateKeyMessage
			// 
			this.lblCreateKeyMessage.Location = new System.Drawing.Point(13, 13);
			this.lblCreateKeyMessage.Name = "lblCreateKeyMessage";
			this.lblCreateKeyMessage.Size = new System.Drawing.Size(326, 46);
			this.lblCreateKeyMessage.TabIndex = 0;
			// 
			// CreateNewKeyControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.pnlCreateKey);
			this.Name = "CreateNewKeyControl";
			this.Size = new System.Drawing.Size(351, 178);
			this.pnlCreateKey.ResumeLayout(false);
			this.pnlCreateKey.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlCreateKey;
        private System.Windows.Forms.Label lblCreateKeyMessage;
        private System.Windows.Forms.Button generateKeyButton;
        private System.Windows.Forms.TextBox keyBox;
    }
}
