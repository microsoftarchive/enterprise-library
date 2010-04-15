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
    partial class ExportKeyUI
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.pnlWizardControl = new System.Windows.Forms.FlowLayoutPanel();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOk = new System.Windows.Forms.Button();
			this.exportKeyControl1 = new EnterpriseLibrary.Security.Cryptography.Configuration.Design.ExportKeyControl();
			this.pnlWizardControl.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlWizardControl
			// 
			this.pnlWizardControl.Controls.Add(this.btnCancel);
			this.pnlWizardControl.Controls.Add(this.btnOk);
			this.pnlWizardControl.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pnlWizardControl.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
			this.pnlWizardControl.Location = new System.Drawing.Point(0, 197);
			this.pnlWizardControl.Name = "pnlWizardControl";
			this.pnlWizardControl.Padding = new System.Windows.Forms.Padding(3);
			this.pnlWizardControl.Size = new System.Drawing.Size(349, 39);
			this.pnlWizardControl.TabIndex = 1;
			// 
			// btnCancel
			// 
			this.btnCancel.CausesValidation = false;
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(281, 6);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(59, 23);
			this.btnCancel.TabIndex = 4;
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// btnOk
			// 
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOk.Location = new System.Drawing.Point(216, 6);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(59, 23);
			this.btnOk.TabIndex = 5;
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// exportKeyControl1
			// 
			this.exportKeyControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.exportKeyControl1.FileName = "";
			this.exportKeyControl1.Location = new System.Drawing.Point(0, 0);
			this.exportKeyControl1.Name = "exportKeyControl1";
			this.exportKeyControl1.Size = new System.Drawing.Size(349, 197);
			this.exportKeyControl1.TabIndex = 2;
			// 
			// ExportKeyUI
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(349, 236);
			this.Controls.Add(this.exportKeyControl1);
			this.Controls.Add(this.pnlWizardControl);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ExportKeyUI";
			this.pnlWizardControl.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel pnlWizardControl;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private ExportKeyControl exportKeyControl1;
    }
}
