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
    partial class ChooseDataProtectionScopeControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChooseDataProtectionScopeControl));
            this.pnlChooseDpapiScope = new System.Windows.Forms.Panel();
            this.rbDpapiScopeLocalMachine = new System.Windows.Forms.RadioButton();
            this.rbDpapiScopeCurrentUser = new System.Windows.Forms.RadioButton();
            this.lblChooseDpapiScopeMessage = new System.Windows.Forms.Label();
            this.pnlChooseDpapiScope.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlChooseDpapiScope
            // 
            this.pnlChooseDpapiScope.Controls.Add(this.rbDpapiScopeLocalMachine);
            this.pnlChooseDpapiScope.Controls.Add(this.rbDpapiScopeCurrentUser);
            this.pnlChooseDpapiScope.Controls.Add(this.lblChooseDpapiScopeMessage);
            this.pnlChooseDpapiScope.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlChooseDpapiScope.Location = new System.Drawing.Point(0, 0);
            this.pnlChooseDpapiScope.Name = "pnlChooseDpapiScope";
            this.pnlChooseDpapiScope.Size = new System.Drawing.Size(351, 178);
            this.pnlChooseDpapiScope.TabIndex = 4;
            // 
            // rbDpapiScopeLocalMachine
            // 
            this.rbDpapiScopeLocalMachine.AutoSize = true;
            this.rbDpapiScopeLocalMachine.Location = new System.Drawing.Point(45, 139);
            this.rbDpapiScopeLocalMachine.Name = "rbDpapiScopeLocalMachine";
            this.rbDpapiScopeLocalMachine.Size = new System.Drawing.Size(95, 17);
            this.rbDpapiScopeLocalMachine.TabIndex = 2;
            this.rbDpapiScopeLocalMachine.UseVisualStyleBackColor = true;
            // 
            // rbDpapiScopeCurrentUser
            // 
            this.rbDpapiScopeCurrentUser.AutoSize = true;
            this.rbDpapiScopeCurrentUser.Checked = true;
            this.rbDpapiScopeCurrentUser.Location = new System.Drawing.Point(45, 116);
            this.rbDpapiScopeCurrentUser.Name = "rbDpapiScopeCurrentUser";
            this.rbDpapiScopeCurrentUser.Size = new System.Drawing.Size(76, 17);
            this.rbDpapiScopeCurrentUser.TabIndex = 1;
            this.rbDpapiScopeCurrentUser.TabStop = true;
            this.rbDpapiScopeCurrentUser.UseVisualStyleBackColor = true;
            // 
            // lblChooseDpapiScopeMessage
            // 
            this.lblChooseDpapiScopeMessage.Location = new System.Drawing.Point(13, 13);
            this.lblChooseDpapiScopeMessage.Name = "lblChooseDpapiScopeMessage";
            this.lblChooseDpapiScopeMessage.Size = new System.Drawing.Size(326, 97);
            this.lblChooseDpapiScopeMessage.TabIndex = 0;
            // 
            // ChooseDpapiScopeControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlChooseDpapiScope);
            this.Name = "ChooseDpapiScopeControl";
            this.Size = new System.Drawing.Size(351, 178);
            this.pnlChooseDpapiScope.ResumeLayout(false);
            this.pnlChooseDpapiScope.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlChooseDpapiScope;
        private System.Windows.Forms.RadioButton rbDpapiScopeLocalMachine;
        private System.Windows.Forms.RadioButton rbDpapiScopeCurrentUser;
        private System.Windows.Forms.Label lblChooseDpapiScopeMessage;
    }
}
