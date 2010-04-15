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
    partial class CryptographicKeyWizard
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
			this.btnFinish = new System.Windows.Forms.Button();
			this.btnNext = new System.Windows.Forms.Button();
			this.btnPrevious = new System.Windows.Forms.Button();
			this.pnlWizardContents = new System.Windows.Forms.Panel();
			this.chooseProtectionScopeControl = new EnterpriseLibrary.Security.Cryptography.Configuration.Design.ChooseKeyFileControl();
			this.createNewKeyControl = new EnterpriseLibrary.Security.Cryptography.Configuration.Design.CreateNewKeyControl();
			this.chooseDpapiScopeControl = new EnterpriseLibrary.Security.Cryptography.Configuration.Design.ChooseDataProtectionScopeControl();
			this.importArchivedKeyControl = new EnterpriseLibrary.Security.Cryptography.Configuration.Design.ImportArchivedKeyControl();
			this.supplyKeyControl = new EnterpriseLibrary.Security.Cryptography.Configuration.Design.SupplyKeyControl();
			this.openExistingKeyFileControl = new EnterpriseLibrary.Security.Cryptography.Configuration.Design.OpenExistingKeyFileControl();
			this.pnlWizardControl.SuspendLayout();
			this.pnlWizardContents.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlWizardControl
			// 
			this.pnlWizardControl.Controls.Add(this.btnCancel);
			this.pnlWizardControl.Controls.Add(this.btnFinish);
			this.pnlWizardControl.Controls.Add(this.btnNext);
			this.pnlWizardControl.Controls.Add(this.btnPrevious);
			this.pnlWizardControl.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pnlWizardControl.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
			this.pnlWizardControl.Location = new System.Drawing.Point(0, 178);
			this.pnlWizardControl.Name = "pnlWizardControl";
			this.pnlWizardControl.Padding = new System.Windows.Forms.Padding(3);
			this.pnlWizardControl.Size = new System.Drawing.Size(374, 39);
			this.pnlWizardControl.TabIndex = 0;
			// 
			// btnCancel
			// 
			this.btnCancel.CausesValidation = false;
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(306, 6);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(59, 23);
			this.btnCancel.TabIndex = 4;
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// btnFinish
			// 
			this.btnFinish.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnFinish.Location = new System.Drawing.Point(241, 6);
			this.btnFinish.Name = "btnFinish";
			this.btnFinish.Size = new System.Drawing.Size(59, 23);
			this.btnFinish.TabIndex = 5;
			this.btnFinish.UseVisualStyleBackColor = true;
			this.btnFinish.Click += new System.EventHandler(this.btnFinish_Click);
			// 
			// btnNext
			// 
			this.btnNext.Location = new System.Drawing.Point(178, 6);
			this.btnNext.Name = "btnNext";
			this.btnNext.Size = new System.Drawing.Size(57, 23);
			this.btnNext.TabIndex = 6;
			this.btnNext.UseVisualStyleBackColor = true;
			this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
			// 
			// btnPrevious
			// 
			this.btnPrevious.CausesValidation = false;
			this.btnPrevious.Location = new System.Drawing.Point(97, 6);
			this.btnPrevious.Name = "btnPrevious";
			this.btnPrevious.Size = new System.Drawing.Size(75, 23);
			this.btnPrevious.TabIndex = 7;
			this.btnPrevious.UseVisualStyleBackColor = true;
			this.btnPrevious.Click += new System.EventHandler(this.btnPrevious_Click);
			// 
			// pnlWizardContents
			// 
			this.pnlWizardContents.Controls.Add(this.chooseProtectionScopeControl);
			this.pnlWizardContents.Controls.Add(this.createNewKeyControl);
			this.pnlWizardContents.Controls.Add(this.chooseDpapiScopeControl);
			this.pnlWizardContents.Controls.Add(this.importArchivedKeyControl);
			this.pnlWizardContents.Controls.Add(this.supplyKeyControl);
			this.pnlWizardContents.Controls.Add(this.openExistingKeyFileControl);
			this.pnlWizardContents.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlWizardContents.Location = new System.Drawing.Point(0, 0);
			this.pnlWizardContents.Name = "pnlWizardContents";
			this.pnlWizardContents.Size = new System.Drawing.Size(374, 178);
			this.pnlWizardContents.TabIndex = 1;
			// 
			// chooseProtectionScopeControl
			// 
			this.chooseProtectionScopeControl.FilePath = "";
			this.chooseProtectionScopeControl.Location = new System.Drawing.Point(13, 13);
			this.chooseProtectionScopeControl.Name = "chooseProtectionScopeControl";
			this.chooseProtectionScopeControl.Size = new System.Drawing.Size(351, 178);
			this.chooseProtectionScopeControl.TabIndex = 6;
			this.chooseProtectionScopeControl.Visible = false;
			// 
			// createNewKeyControl
			// 
			this.createNewKeyControl.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
			this.createNewKeyControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.createNewKeyControl.Key = new byte[0];
			this.createNewKeyControl.Location = new System.Drawing.Point(0, 0);
			this.createNewKeyControl.Name = "createNewKeyControl";
			this.createNewKeyControl.Size = new System.Drawing.Size(374, 178);
			this.createNewKeyControl.TabIndex = 4;
			this.createNewKeyControl.Visible = false;
			// 
			// chooseDpapiScopeControl
			// 
			this.chooseDpapiScopeControl.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
			this.chooseDpapiScopeControl.Location = new System.Drawing.Point(13, 13);
			this.chooseDpapiScopeControl.Name = "chooseDpapiScopeControl";
			this.chooseDpapiScopeControl.Scope = System.Security.Cryptography.DataProtectionScope.CurrentUser;
			this.chooseDpapiScopeControl.Size = new System.Drawing.Size(351, 178);
			this.chooseDpapiScopeControl.TabIndex = 3;
			this.chooseDpapiScopeControl.Visible = false;
			// 
			// importArchivedKeyControl
			// 
			this.importArchivedKeyControl.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
			this.importArchivedKeyControl.Location = new System.Drawing.Point(13, 13);
			this.importArchivedKeyControl.Name = "importArchivedKeyControl";
			this.importArchivedKeyControl.Size = new System.Drawing.Size(340, 192);
			this.importArchivedKeyControl.TabIndex = 1;
			this.importArchivedKeyControl.Visible = false;
			// 
			// supplyKeyControl
			// 
			this.supplyKeyControl.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
			this.supplyKeyControl.Location = new System.Drawing.Point(13, 13);
			this.supplyKeyControl.Name = "supplyKeyControl";
			this.supplyKeyControl.Size = new System.Drawing.Size(351, 178);
			this.supplyKeyControl.TabIndex = 0;
			// 
			// openExistingKeyFileControl
			// 
			this.openExistingKeyFileControl.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
			this.openExistingKeyFileControl.FilePath = "";
			this.openExistingKeyFileControl.Location = new System.Drawing.Point(0, 0);
			this.openExistingKeyFileControl.Name = "openExistingKeyFileControl";
			this.openExistingKeyFileControl.Size = new System.Drawing.Size(351, 178);
			this.openExistingKeyFileControl.TabIndex = 7;
			this.openExistingKeyFileControl.Visible = false;
			// 
			// CryptographicKeyWizard
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(374, 217);
			this.Controls.Add(this.pnlWizardContents);
			this.Controls.Add(this.pnlWizardControl);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CryptographicKeyWizard";
			this.pnlWizardControl.ResumeLayout(false);
			this.pnlWizardContents.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel pnlWizardControl;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnFinish;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnPrevious;
        private System.Windows.Forms.Panel pnlWizardContents;
        private ChooseKeyFileControl chooseProtectionScopeControl;
        private CreateNewKeyControl createNewKeyControl;
        private ChooseDataProtectionScopeControl chooseDpapiScopeControl;
        private ImportArchivedKeyControl importArchivedKeyControl;
        private SupplyKeyControl supplyKeyControl;
        private OpenExistingKeyFileControl openExistingKeyFileControl;


    }
}
