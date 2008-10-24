//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Policy Injection Application Block QuickStart
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

namespace PolicyInjectionQuickStart
{
    partial class MainForm
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
            this.balanceInquiryButton = new System.Windows.Forms.Button();
            this.depositButton = new System.Windows.Forms.Button();
            this.withdrawButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.userComboBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.balanceTextBox = new System.Windows.Forms.TextBox();
            this.exceptionTextBox = new System.Windows.Forms.TextBox();
            this.viewLogButton = new System.Windows.Forms.Button();
            this.openPerfMonButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.viewWalkthroughButton = new System.Windows.Forms.Button();
            this.exitButton = new System.Windows.Forms.Button();
            this.installPerfCountersButton = new System.Windows.Forms.Button();
            this.uninstallPerfCountersButton = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // balanceInquiryButton
            // 
            this.balanceInquiryButton.Location = new System.Drawing.Point(12, 82);
            this.balanceInquiryButton.Name = "balanceInquiryButton";
            this.balanceInquiryButton.Size = new System.Drawing.Size(150, 29);
            this.balanceInquiryButton.TabIndex = 2;
            this.balanceInquiryButton.Text = "Balance Inquiry";
            this.balanceInquiryButton.UseVisualStyleBackColor = true;
            this.balanceInquiryButton.Click += new System.EventHandler(this.balanceInquiryButton_Click);
            // 
            // depositButton
            // 
            this.depositButton.Location = new System.Drawing.Point(12, 12);
            this.depositButton.Name = "depositButton";
            this.depositButton.Size = new System.Drawing.Size(150, 29);
            this.depositButton.TabIndex = 0;
            this.depositButton.Text = "Deposit...";
            this.depositButton.UseVisualStyleBackColor = true;
            this.depositButton.Click += new System.EventHandler(this.depositButton_Click);
            // 
            // withdrawButton
            // 
            this.withdrawButton.Location = new System.Drawing.Point(12, 47);
            this.withdrawButton.Name = "withdrawButton";
            this.withdrawButton.Size = new System.Drawing.Size(150, 29);
            this.withdrawButton.TabIndex = 1;
            this.withdrawButton.Text = "Withdraw...";
            this.withdrawButton.UseVisualStyleBackColor = true;
            this.withdrawButton.Click += new System.EventHandler(this.withdrawButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(187, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Current User:";
            // 
            // userComboBox
            // 
            this.userComboBox.DisplayMember = "Key";
            this.userComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.userComboBox.FormattingEnabled = true;
            this.userComboBox.Location = new System.Drawing.Point(267, 13);
            this.userComboBox.Name = "userComboBox";
            this.userComboBox.Size = new System.Drawing.Size(166, 21);
            this.userComboBox.TabIndex = 6;
            this.userComboBox.SelectedIndexChanged += new System.EventHandler(this.userComboBox_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(187, 90);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Balance:";
            // 
            // balanceTextBox
            // 
            this.balanceTextBox.Location = new System.Drawing.Point(267, 90);
            this.balanceTextBox.Name = "balanceTextBox";
            this.balanceTextBox.ReadOnly = true;
            this.balanceTextBox.Size = new System.Drawing.Size(166, 21);
            this.balanceTextBox.TabIndex = 8;
            // 
            // exceptionTextBox
            // 
            this.exceptionTextBox.ForeColor = System.Drawing.Color.Red;
            this.exceptionTextBox.Location = new System.Drawing.Point(267, 139);
            this.exceptionTextBox.Multiline = true;
            this.exceptionTextBox.Name = "exceptionTextBox";
            this.exceptionTextBox.ReadOnly = true;
            this.exceptionTextBox.Size = new System.Drawing.Size(166, 116);
            this.exceptionTextBox.TabIndex = 10;
            // 
            // viewLogButton
            // 
            this.viewLogButton.Location = new System.Drawing.Point(12, 134);
            this.viewLogButton.Name = "viewLogButton";
            this.viewLogButton.Size = new System.Drawing.Size(150, 29);
            this.viewLogButton.TabIndex = 3;
            this.viewLogButton.Text = "View Log File";
            this.viewLogButton.UseVisualStyleBackColor = true;
            this.viewLogButton.Click += new System.EventHandler(this.viewLogButton_Click);
            // 
            // openPerfMonButton
            // 
            this.openPerfMonButton.Location = new System.Drawing.Point(12, 169);
            this.openPerfMonButton.Name = "openPerfMonButton";
            this.openPerfMonButton.Size = new System.Drawing.Size(150, 29);
            this.openPerfMonButton.TabIndex = 4;
            this.openPerfMonButton.Text = "Open Performance Monitor";
            this.openPerfMonButton.UseVisualStyleBackColor = true;
            this.openPerfMonButton.Click += new System.EventHandler(this.openPerfMonButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(187, 139);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Exception:";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Info;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label3);
            this.panel1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.panel1.Location = new System.Drawing.Point(12, 281);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(421, 72);
            this.panel1.TabIndex = 4;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(16, 34);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(360, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "Performance Counters and Exception Handling on BankAccount.Withdraw";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 50);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(362, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Validation handler applied explicitly via attributes to Deposit and Withdraw";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 19);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(344, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Logging and Authorization for all members in BusinessLogic namespace";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(334, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Default Policies (which you can change in configuration if you want):";
            // 
            // viewWalkthroughButton
            // 
            this.viewWalkthroughButton.Location = new System.Drawing.Point(207, 369);
            this.viewWalkthroughButton.Name = "viewWalkthroughButton";
            this.viewWalkthroughButton.Size = new System.Drawing.Size(107, 32);
            this.viewWalkthroughButton.TabIndex = 11;
            this.viewWalkthroughButton.Text = "View Walkthrough";
            this.viewWalkthroughButton.UseVisualStyleBackColor = true;
            this.viewWalkthroughButton.Click += new System.EventHandler(this.viewWalkthroughButton_Click);
            // 
            // exitButton
            // 
            this.exitButton.Location = new System.Drawing.Point(326, 369);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(107, 32);
            this.exitButton.TabIndex = 12;
            this.exitButton.Text = "Exit";
            this.exitButton.UseVisualStyleBackColor = true;
            this.exitButton.Click += new System.EventHandler(this.exitButton_Click);
            // 
            // installPerfCountersButton
            // 
            this.installPerfCountersButton.Location = new System.Drawing.Point(13, 204);
            this.installPerfCountersButton.Name = "installPerfCountersButton";
            this.installPerfCountersButton.Size = new System.Drawing.Size(149, 28);
            this.installPerfCountersButton.TabIndex = 13;
            this.installPerfCountersButton.Text = "Install Perf Counters";
            this.installPerfCountersButton.UseVisualStyleBackColor = true;
            this.installPerfCountersButton.Click += new System.EventHandler(this.installPerfCountersButton_Click);
            // 
            // uninstallPerfCountersButton
            // 
            this.uninstallPerfCountersButton.Location = new System.Drawing.Point(13, 238);
            this.uninstallPerfCountersButton.Name = "uninstallPerfCountersButton";
            this.uninstallPerfCountersButton.Size = new System.Drawing.Size(149, 28);
            this.uninstallPerfCountersButton.TabIndex = 13;
            this.uninstallPerfCountersButton.Text = "Uninstall Perf Counters";
            this.uninstallPerfCountersButton.UseVisualStyleBackColor = true;
            this.uninstallPerfCountersButton.Click += new System.EventHandler(this.uninstallPerfCountersButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(445, 411);
            this.Controls.Add(this.uninstallPerfCountersButton);
            this.Controls.Add(this.installPerfCountersButton);
            this.Controls.Add(this.exitButton);
            this.Controls.Add(this.viewWalkthroughButton);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.exceptionTextBox);
            this.Controls.Add(this.balanceTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.userComboBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.withdrawButton);
            this.Controls.Add(this.depositButton);
            this.Controls.Add(this.openPerfMonButton);
            this.Controls.Add(this.viewLogButton);
            this.Controls.Add(this.balanceInquiryButton);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Policy Injection QuickStart";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button balanceInquiryButton;
        private System.Windows.Forms.Button depositButton;
        private System.Windows.Forms.Button withdrawButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox userComboBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox balanceTextBox;
        private System.Windows.Forms.TextBox exceptionTextBox;
        private System.Windows.Forms.Button viewLogButton;
        private System.Windows.Forms.Button openPerfMonButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button viewWalkthroughButton;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.Button installPerfCountersButton;
        private System.Windows.Forms.Button uninstallPerfCountersButton;
    }
}

