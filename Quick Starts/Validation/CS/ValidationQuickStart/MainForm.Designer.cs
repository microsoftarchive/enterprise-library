//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Validation Application Block QuickStart
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

namespace ValidationQuickStart
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
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.birthDatePicker = new System.Windows.Forms.DateTimePicker();
            this.rewardPointTextBox = new System.Windows.Forms.TextBox();
            this.emailTextBox = new System.Windows.Forms.TextBox();
            this.lastNameTextBox = new System.Windows.Forms.TextBox();
            this.firstNameTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.postCodeTextBox = new System.Windows.Forms.TextBox();
            this.stateTextBox = new System.Windows.Forms.TextBox();
            this.countryTextBox = new System.Windows.Forms.TextBox();
            this.cityTextBox = new System.Windows.Forms.TextBox();
            this.line2TextBox = new System.Windows.Forms.TextBox();
            this.line1TextBox = new System.Windows.Forms.TextBox();
            this.validateCustomerButton = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.customerRuleSetCombo = new System.Windows.Forms.ComboBox();
            this.customerResultsTreeView = new System.Windows.Forms.TreeView();
            this.viewWalkthroughButton = new System.Windows.Forms.Button();
            this.exitButton = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.enableWinFormsValidationCheckBox = new System.Windows.Forms.CheckBox();
            this.customerValidationProvider = new Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WinForms.ValidationProvider();
            this.addressValidationProvider = new Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WinForms.ValidationProvider();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.birthDatePicker);
            this.groupBox1.Controls.Add(this.rewardPointTextBox);
            this.groupBox1.Controls.Add(this.emailTextBox);
            this.groupBox1.Controls.Add(this.lastNameTextBox);
            this.groupBox1.Controls.Add(this.firstNameTextBox);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(15, 15);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(315, 172);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Customer";
            // 
            // birthDatePicker
            // 
            this.birthDatePicker.Location = new System.Drawing.Point(81, 78);
            this.birthDatePicker.Name = "birthDatePicker";
            this.customerValidationProvider.SetPerformValidation(this.birthDatePicker, true);
            this.birthDatePicker.Size = new System.Drawing.Size(211, 21);
            this.customerValidationProvider.SetSourcePropertyName(this.birthDatePicker, "DateOfBirth");
            this.birthDatePicker.TabIndex = 5;
            this.customerValidationProvider.SetValidatedProperty(this.birthDatePicker, "Value");
            // 
            // rewardPointTextBox
            // 
            this.rewardPointTextBox.Location = new System.Drawing.Point(81, 134);
            this.rewardPointTextBox.Name = "rewardPointTextBox";
            this.customerValidationProvider.SetPerformValidation(this.rewardPointTextBox, true);
            this.rewardPointTextBox.Size = new System.Drawing.Size(95, 21);
            this.customerValidationProvider.SetSourcePropertyName(this.rewardPointTextBox, "RewardPoints");
            this.rewardPointTextBox.TabIndex = 9;
            // 
            // emailTextBox
            // 
            this.emailTextBox.Location = new System.Drawing.Point(81, 106);
            this.emailTextBox.Name = "emailTextBox";
            this.customerValidationProvider.SetPerformValidation(this.emailTextBox, true);
            this.emailTextBox.Size = new System.Drawing.Size(211, 21);
            this.customerValidationProvider.SetSourcePropertyName(this.emailTextBox, "Email");
            this.emailTextBox.TabIndex = 7;
            // 
            // lastNameTextBox
            // 
            this.lastNameTextBox.Location = new System.Drawing.Point(81, 47);
            this.lastNameTextBox.Name = "lastNameTextBox";
            this.customerValidationProvider.SetPerformValidation(this.lastNameTextBox, true);
            this.lastNameTextBox.Size = new System.Drawing.Size(211, 21);
            this.customerValidationProvider.SetSourcePropertyName(this.lastNameTextBox, "LastName");
            this.lastNameTextBox.TabIndex = 3;
            // 
            // firstNameTextBox
            // 
            this.firstNameTextBox.Location = new System.Drawing.Point(81, 17);
            this.firstNameTextBox.Name = "firstNameTextBox";
            this.customerValidationProvider.SetPerformValidation(this.firstNameTextBox, true);
            this.firstNameTextBox.Size = new System.Drawing.Size(211, 21);
            this.customerValidationProvider.SetSourcePropertyName(this.firstNameTextBox, "FirstName");
            this.firstNameTextBox.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 137);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Reward Points:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 109);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(39, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "E-Mail:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 84);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Date Of Birth:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Last Name:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "First Name:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.postCodeTextBox);
            this.groupBox2.Controls.Add(this.stateTextBox);
            this.groupBox2.Controls.Add(this.countryTextBox);
            this.groupBox2.Controls.Add(this.cityTextBox);
            this.groupBox2.Controls.Add(this.line2TextBox);
            this.groupBox2.Controls.Add(this.line1TextBox);
            this.groupBox2.Location = new System.Drawing.Point(41, 209);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(289, 164);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Address";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(137, 97);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(60, 13);
            this.label10.TabIndex = 8;
            this.label10.Text = "Post Code:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(7, 127);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(50, 13);
            this.label11.TabIndex = 10;
            this.label11.Text = "Country:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(7, 97);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(37, 13);
            this.label9.TabIndex = 6;
            this.label9.Text = "State:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(7, 68);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(30, 13);
            this.label8.TabIndex = 4;
            this.label8.Text = "City:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 41);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(39, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "Line 2:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(39, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Line 1:";
            // 
            // postCodeTextBox
            // 
            this.postCodeTextBox.Location = new System.Drawing.Point(200, 94);
            this.postCodeTextBox.Name = "postCodeTextBox";
            this.addressValidationProvider.SetPerformValidation(this.postCodeTextBox, true);
            this.postCodeTextBox.Size = new System.Drawing.Size(66, 21);
            this.addressValidationProvider.SetSourcePropertyName(this.postCodeTextBox, "PostCode");
            this.postCodeTextBox.TabIndex = 9;
            // 
            // stateTextBox
            // 
            this.stateTextBox.Location = new System.Drawing.Point(60, 94);
            this.stateTextBox.Name = "stateTextBox";
            this.addressValidationProvider.SetPerformValidation(this.stateTextBox, true);
            this.stateTextBox.Size = new System.Drawing.Size(71, 21);
            this.addressValidationProvider.SetSourcePropertyName(this.stateTextBox, "State");
            this.stateTextBox.TabIndex = 7;
            // 
            // countryTextBox
            // 
            this.countryTextBox.Location = new System.Drawing.Point(60, 124);
            this.countryTextBox.Name = "countryTextBox";
            this.addressValidationProvider.SetPerformValidation(this.countryTextBox, true);
            this.countryTextBox.Size = new System.Drawing.Size(206, 21);
            this.addressValidationProvider.SetSourcePropertyName(this.countryTextBox, "Country");
            this.countryTextBox.TabIndex = 11;
            // 
            // cityTextBox
            // 
            this.cityTextBox.Location = new System.Drawing.Point(60, 65);
            this.cityTextBox.Name = "cityTextBox";
            this.addressValidationProvider.SetPerformValidation(this.cityTextBox, true);
            this.cityTextBox.Size = new System.Drawing.Size(206, 21);
            this.addressValidationProvider.SetSourcePropertyName(this.cityTextBox, "City");
            this.cityTextBox.TabIndex = 5;
            // 
            // line2TextBox
            // 
            this.line2TextBox.Location = new System.Drawing.Point(60, 38);
            this.line2TextBox.Name = "line2TextBox";
            this.addressValidationProvider.SetPerformValidation(this.line2TextBox, true);
            this.line2TextBox.Size = new System.Drawing.Size(206, 21);
            this.addressValidationProvider.SetSourcePropertyName(this.line2TextBox, "Line2");
            this.line2TextBox.TabIndex = 3;
            // 
            // line1TextBox
            // 
            this.line1TextBox.Location = new System.Drawing.Point(60, 13);
            this.line1TextBox.Name = "line1TextBox";
            this.addressValidationProvider.SetPerformValidation(this.line1TextBox, true);
            this.line1TextBox.Size = new System.Drawing.Size(206, 21);
            this.addressValidationProvider.SetSourcePropertyName(this.line1TextBox, "Line1");
            this.line1TextBox.TabIndex = 1;
            // 
            // validateCustomerButton
            // 
            this.validateCustomerButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.validateCustomerButton.Location = new System.Drawing.Point(577, 12);
            this.validateCustomerButton.Name = "validateCustomerButton";
            this.validateCustomerButton.Size = new System.Drawing.Size(75, 23);
            this.validateCustomerButton.TabIndex = 4;
            this.validateCustomerButton.Text = "Validate";
            this.validateCustomerButton.UseVisualStyleBackColor = true;
            this.validateCustomerButton.Click += new System.EventHandler(this.validateCustomerButton_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(344, 15);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(51, 13);
            this.label12.TabIndex = 2;
            this.label12.Text = "Rule Set:";
            // 
            // customerRuleSetCombo
            // 
            this.customerRuleSetCombo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.customerRuleSetCombo.FormattingEnabled = true;
            this.customerRuleSetCombo.Items.AddRange(new object[] {
            "RuleSetA",
            "RuleSetB"});
            this.customerRuleSetCombo.Location = new System.Drawing.Point(402, 13);
            this.customerRuleSetCombo.Name = "customerRuleSetCombo";
            this.customerRuleSetCombo.Size = new System.Drawing.Size(169, 21);
            this.customerRuleSetCombo.TabIndex = 3;
            this.customerRuleSetCombo.TextChanged += new System.EventHandler(this.customerRuleSetCombo_TextChanged);
            // 
            // customerResultsTreeView
            // 
            this.customerResultsTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.customerResultsTreeView.Location = new System.Drawing.Point(347, 45);
            this.customerResultsTreeView.Name = "customerResultsTreeView";
            this.customerResultsTreeView.Size = new System.Drawing.Size(304, 328);
            this.customerResultsTreeView.TabIndex = 5;
            // 
            // viewWalkthroughButton
            // 
            this.viewWalkthroughButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.viewWalkthroughButton.Location = new System.Drawing.Point(425, 396);
            this.viewWalkthroughButton.Name = "viewWalkthroughButton";
            this.viewWalkthroughButton.Size = new System.Drawing.Size(111, 26);
            this.viewWalkthroughButton.TabIndex = 10;
            this.viewWalkthroughButton.Text = "View Walkthrough";
            this.viewWalkthroughButton.UseVisualStyleBackColor = true;
            this.viewWalkthroughButton.Click += new System.EventHandler(this.viewWalkthroughButton_Click);
            // 
            // exitButton
            // 
            this.exitButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.exitButton.Location = new System.Drawing.Point(540, 396);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(111, 26);
            this.exitButton.TabIndex = 10;
            this.exitButton.Text = "E&xit";
            this.exitButton.UseVisualStyleBackColor = true;
            this.exitButton.Click += new System.EventHandler(this.exitButton_Click);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(27, 185);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(34, 53);
            this.panel1.TabIndex = 11;
            // 
            // panel2
            // 
            this.panel2.Location = new System.Drawing.Point(29, 185);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(47, 49);
            this.panel2.TabIndex = 12;
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // enableWinFormsValidationCheckBox
            // 
            this.enableWinFormsValidationCheckBox.AutoSize = true;
            this.enableWinFormsValidationCheckBox.Location = new System.Drawing.Point(21, 396);
            this.enableWinFormsValidationCheckBox.Name = "enableWinFormsValidationCheckBox";
            this.enableWinFormsValidationCheckBox.Size = new System.Drawing.Size(213, 17);
            this.enableWinFormsValidationCheckBox.TabIndex = 13;
            this.enableWinFormsValidationCheckBox.Text = "Enable WinForms-Integrated Validation";
            this.enableWinFormsValidationCheckBox.UseVisualStyleBackColor = true;
            this.enableWinFormsValidationCheckBox.CheckedChanged += new System.EventHandler(this.enableWinFormsValidationCheckBox_CheckedChanged);
            // 
            // customerValidationProvider
            // 
            this.customerValidationProvider.Enabled = false;
            this.customerValidationProvider.ErrorProvider = this.errorProvider;
            this.customerValidationProvider.RulesetName = "RuleSetA";
            this.customerValidationProvider.SourceTypeName = "ValidationQuickStart.BusinessEntities.Customer, ValidationQuickStart.BusinessEnti" +
                "ties";
            this.customerValidationProvider.ValueConvert += new System.EventHandler<Microsoft.Practices.EnterpriseLibrary.Validation.Integration.ValueConvertEventArgs>(this.customerValidationProvider_ValueConvert);
            // 
            // addressValidationProvider
            // 
            this.addressValidationProvider.Enabled = false;
            this.addressValidationProvider.ErrorProvider = this.errorProvider;
            this.addressValidationProvider.RulesetName = "ValidAddress";
            this.addressValidationProvider.SourceTypeName = "ValidationQuickStart.BusinessEntities.Address, ValidationQuickStart.BusinessEntit" +
                "ies";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ClientSize = new System.Drawing.Size(666, 434);
            this.Controls.Add(this.enableWinFormsValidationCheckBox);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.exitButton);
            this.Controls.Add(this.viewWalkthroughButton);
            this.Controls.Add(this.customerResultsTreeView);
            this.Controls.Add(this.customerRuleSetCombo);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.validateCustomerButton);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(100000, 470);
            this.MinimumSize = new System.Drawing.Size(576, 470);
            this.Name = "MainForm";
            this.Text = "Validation QuickStart";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DateTimePicker birthDatePicker;
        private System.Windows.Forms.TextBox lastNameTextBox;
        private System.Windows.Forms.TextBox firstNameTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox emailTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox rewardPointTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox postCodeTextBox;
        private System.Windows.Forms.TextBox stateTextBox;
        private System.Windows.Forms.TextBox countryTextBox;
        private System.Windows.Forms.TextBox cityTextBox;
        private System.Windows.Forms.TextBox line2TextBox;
        private System.Windows.Forms.TextBox line1TextBox;
        private System.Windows.Forms.Button validateCustomerButton;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox customerRuleSetCombo;
        private System.Windows.Forms.TreeView customerResultsTreeView;
        private System.Windows.Forms.Button viewWalkthroughButton;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WinForms.ValidationProvider addressValidationProvider;
        private Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WinForms.ValidationProvider customerValidationProvider;
        private System.Windows.Forms.CheckBox enableWinFormsValidationCheckBox;
    }
}

