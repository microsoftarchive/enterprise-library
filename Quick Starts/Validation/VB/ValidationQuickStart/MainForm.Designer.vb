'===============================================================================
' Microsoft patterns & practices Enterprise Library
' Validation Application Block QuickStart
'===============================================================================
' Copyright © Microsoft Corporation.  All rights reserved.
' THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
' OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
' LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
' FITNESS FOR A PARTICULAR PURPOSE.
'===============================================================================

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.groupBox2 = New System.Windows.Forms.GroupBox
        Me.label10 = New System.Windows.Forms.Label
        Me.label11 = New System.Windows.Forms.Label
        Me.label9 = New System.Windows.Forms.Label
        Me.label8 = New System.Windows.Forms.Label
        Me.label7 = New System.Windows.Forms.Label
        Me.label6 = New System.Windows.Forms.Label
        Me.postCodeTextBox = New System.Windows.Forms.TextBox
        Me.stateTextBox = New System.Windows.Forms.TextBox
        Me.countryTextBox = New System.Windows.Forms.TextBox
        Me.cityTextBox = New System.Windows.Forms.TextBox
        Me.line2TextBox = New System.Windows.Forms.TextBox
        Me.line1TextBox = New System.Windows.Forms.TextBox
        Me.customerRuleSetCombo = New System.Windows.Forms.ComboBox
        Me.label12 = New System.Windows.Forms.Label
        Me.validateCustomerButton = New System.Windows.Forms.Button
        Me.customerValidationProvider = New Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WinForms.ValidationProvider
        Me.errorProvider = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.enableWinFormsValidationCheckBox = New System.Windows.Forms.CheckBox
        Me.panel2 = New System.Windows.Forms.Panel
        Me.panel1 = New System.Windows.Forms.Panel
        Me.exitButton = New System.Windows.Forms.Button
        Me.viewWalkthroughButton = New System.Windows.Forms.Button
        Me.customerResultsTreeView = New System.Windows.Forms.TreeView
        Me.birthDatePicker = New System.Windows.Forms.DateTimePicker
        Me.rewardPointTextBox = New System.Windows.Forms.TextBox
        Me.emailTextBox = New System.Windows.Forms.TextBox
        Me.lastNameTextBox = New System.Windows.Forms.TextBox
        Me.firstNameTextBox = New System.Windows.Forms.TextBox
        Me.groupBox1 = New System.Windows.Forms.GroupBox
        Me.label5 = New System.Windows.Forms.Label
        Me.label4 = New System.Windows.Forms.Label
        Me.label3 = New System.Windows.Forms.Label
        Me.label2 = New System.Windows.Forms.Label
        Me.label1 = New System.Windows.Forms.Label
        Me.addressValidationProvider = New Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WinForms.ValidationProvider
        Me.groupBox2.SuspendLayout()
        CType(Me.errorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.groupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'groupBox2
        '
        Me.groupBox2.Controls.Add(Me.label10)
        Me.groupBox2.Controls.Add(Me.label11)
        Me.groupBox2.Controls.Add(Me.label9)
        Me.groupBox2.Controls.Add(Me.label8)
        Me.groupBox2.Controls.Add(Me.label7)
        Me.groupBox2.Controls.Add(Me.label6)
        Me.groupBox2.Controls.Add(Me.postCodeTextBox)
        Me.groupBox2.Controls.Add(Me.stateTextBox)
        Me.groupBox2.Controls.Add(Me.countryTextBox)
        Me.groupBox2.Controls.Add(Me.cityTextBox)
        Me.groupBox2.Controls.Add(Me.line2TextBox)
        Me.groupBox2.Controls.Add(Me.line1TextBox)
        Me.groupBox2.Location = New System.Drawing.Point(38, 206)
        Me.groupBox2.Name = "groupBox2"
        Me.groupBox2.Size = New System.Drawing.Size(289, 164)
        Me.addressValidationProvider.SetSourcePropertyName(Me.groupBox2, Nothing)
        Me.customerValidationProvider.SetSourcePropertyName(Me.groupBox2, Nothing)
        Me.groupBox2.TabIndex = 15
        Me.groupBox2.TabStop = False
        Me.groupBox2.Text = "Address"
        '
        'label10
        '
        Me.label10.AutoSize = True
        Me.label10.Location = New System.Drawing.Point(137, 97)
        Me.label10.Name = "label10"
        Me.label10.Size = New System.Drawing.Size(59, 13)
        Me.addressValidationProvider.SetSourcePropertyName(Me.label10, Nothing)
        Me.customerValidationProvider.SetSourcePropertyName(Me.label10, Nothing)
        Me.label10.TabIndex = 8
        Me.label10.Text = "Post Code:"
        '
        'label11
        '
        Me.label11.AutoSize = True
        Me.label11.Location = New System.Drawing.Point(7, 127)
        Me.label11.Name = "label11"
        Me.label11.Size = New System.Drawing.Size(46, 13)
        Me.addressValidationProvider.SetSourcePropertyName(Me.label11, Nothing)
        Me.customerValidationProvider.SetSourcePropertyName(Me.label11, Nothing)
        Me.label11.TabIndex = 10
        Me.label11.Text = "Country:"
        '
        'label9
        '
        Me.label9.AutoSize = True
        Me.label9.Location = New System.Drawing.Point(7, 97)
        Me.label9.Name = "label9"
        Me.label9.Size = New System.Drawing.Size(35, 13)
        Me.addressValidationProvider.SetSourcePropertyName(Me.label9, Nothing)
        Me.customerValidationProvider.SetSourcePropertyName(Me.label9, Nothing)
        Me.label9.TabIndex = 6
        Me.label9.Text = "State:"
        '
        'label8
        '
        Me.label8.AutoSize = True
        Me.label8.Location = New System.Drawing.Point(7, 68)
        Me.label8.Name = "label8"
        Me.label8.Size = New System.Drawing.Size(27, 13)
        Me.addressValidationProvider.SetSourcePropertyName(Me.label8, Nothing)
        Me.customerValidationProvider.SetSourcePropertyName(Me.label8, Nothing)
        Me.label8.TabIndex = 4
        Me.label8.Text = "City:"
        '
        'label7
        '
        Me.label7.AutoSize = True
        Me.label7.Location = New System.Drawing.Point(7, 41)
        Me.label7.Name = "label7"
        Me.label7.Size = New System.Drawing.Size(39, 13)
        Me.addressValidationProvider.SetSourcePropertyName(Me.label7, Nothing)
        Me.customerValidationProvider.SetSourcePropertyName(Me.label7, Nothing)
        Me.label7.TabIndex = 2
        Me.label7.Text = "Line 2:"
        '
        'label6
        '
        Me.label6.AutoSize = True
        Me.label6.Location = New System.Drawing.Point(7, 16)
        Me.label6.Name = "label6"
        Me.label6.Size = New System.Drawing.Size(39, 13)
        Me.addressValidationProvider.SetSourcePropertyName(Me.label6, Nothing)
        Me.customerValidationProvider.SetSourcePropertyName(Me.label6, Nothing)
        Me.label6.TabIndex = 0
        Me.label6.Text = "Line 1:"
        '
        'postCodeTextBox
        '
        Me.postCodeTextBox.Location = New System.Drawing.Point(200, 94)
        Me.postCodeTextBox.Name = "postCodeTextBox"
        Me.addressValidationProvider.SetPerformValidation(Me.postCodeTextBox, True)
        Me.postCodeTextBox.Size = New System.Drawing.Size(66, 20)
        Me.customerValidationProvider.SetSourcePropertyName(Me.postCodeTextBox, Nothing)
        Me.addressValidationProvider.SetSourcePropertyName(Me.postCodeTextBox, "PostCode")
        Me.postCodeTextBox.TabIndex = 9
        '
        'stateTextBox
        '
        Me.stateTextBox.Location = New System.Drawing.Point(60, 94)
        Me.stateTextBox.Name = "stateTextBox"
        Me.addressValidationProvider.SetPerformValidation(Me.stateTextBox, True)
        Me.stateTextBox.Size = New System.Drawing.Size(71, 20)
        Me.addressValidationProvider.SetSourcePropertyName(Me.stateTextBox, "State")
        Me.customerValidationProvider.SetSourcePropertyName(Me.stateTextBox, Nothing)
        Me.stateTextBox.TabIndex = 7
        '
        'countryTextBox
        '
        Me.countryTextBox.Location = New System.Drawing.Point(60, 124)
        Me.countryTextBox.Name = "countryTextBox"
        Me.addressValidationProvider.SetPerformValidation(Me.countryTextBox, True)
        Me.countryTextBox.Size = New System.Drawing.Size(206, 20)
        Me.addressValidationProvider.SetSourcePropertyName(Me.countryTextBox, "Country")
        Me.customerValidationProvider.SetSourcePropertyName(Me.countryTextBox, Nothing)
        Me.countryTextBox.TabIndex = 11
        '
        'cityTextBox
        '
        Me.cityTextBox.Location = New System.Drawing.Point(60, 65)
        Me.cityTextBox.Name = "cityTextBox"
        Me.addressValidationProvider.SetPerformValidation(Me.cityTextBox, True)
        Me.cityTextBox.Size = New System.Drawing.Size(206, 20)
        Me.addressValidationProvider.SetSourcePropertyName(Me.cityTextBox, "City")
        Me.customerValidationProvider.SetSourcePropertyName(Me.cityTextBox, Nothing)
        Me.cityTextBox.TabIndex = 5
        '
        'line2TextBox
        '
        Me.line2TextBox.Location = New System.Drawing.Point(60, 38)
        Me.line2TextBox.Name = "line2TextBox"
        Me.addressValidationProvider.SetPerformValidation(Me.line2TextBox, True)
        Me.line2TextBox.Size = New System.Drawing.Size(206, 20)
        Me.customerValidationProvider.SetSourcePropertyName(Me.line2TextBox, Nothing)
        Me.addressValidationProvider.SetSourcePropertyName(Me.line2TextBox, "Line2")
        Me.line2TextBox.TabIndex = 3
        '
        'line1TextBox
        '
        Me.line1TextBox.Location = New System.Drawing.Point(60, 13)
        Me.line1TextBox.Name = "line1TextBox"
        Me.addressValidationProvider.SetPerformValidation(Me.line1TextBox, True)
        Me.line1TextBox.Size = New System.Drawing.Size(206, 20)
        Me.addressValidationProvider.SetSourcePropertyName(Me.line1TextBox, "Line1")
        Me.customerValidationProvider.SetSourcePropertyName(Me.line1TextBox, Nothing)
        Me.line1TextBox.TabIndex = 1
        '
        'customerRuleSetCombo
        '
        Me.customerRuleSetCombo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.customerRuleSetCombo.FormattingEnabled = True
        Me.customerRuleSetCombo.Items.AddRange(New Object() {"RuleSetA", "RuleSetB"})
        Me.customerRuleSetCombo.Location = New System.Drawing.Point(399, 10)
        Me.customerRuleSetCombo.Name = "customerRuleSetCombo"
        Me.customerRuleSetCombo.Size = New System.Drawing.Size(198, 21)
        Me.customerValidationProvider.SetSourcePropertyName(Me.customerRuleSetCombo, Nothing)
        Me.addressValidationProvider.SetSourcePropertyName(Me.customerRuleSetCombo, Nothing)
        Me.customerRuleSetCombo.TabIndex = 17
        '
        'label12
        '
        Me.label12.AutoSize = True
        Me.label12.Location = New System.Drawing.Point(341, 12)
        Me.label12.Name = "label12"
        Me.label12.Size = New System.Drawing.Size(51, 13)
        Me.addressValidationProvider.SetSourcePropertyName(Me.label12, Nothing)
        Me.customerValidationProvider.SetSourcePropertyName(Me.label12, Nothing)
        Me.label12.TabIndex = 16
        Me.label12.Text = "Rule Set:"
        '
        'validateCustomerButton
        '
        Me.validateCustomerButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.validateCustomerButton.Location = New System.Drawing.Point(603, 8)
        Me.validateCustomerButton.Name = "validateCustomerButton"
        Me.validateCustomerButton.Size = New System.Drawing.Size(75, 23)
        Me.addressValidationProvider.SetSourcePropertyName(Me.validateCustomerButton, Nothing)
        Me.customerValidationProvider.SetSourcePropertyName(Me.validateCustomerButton, Nothing)
        Me.validateCustomerButton.TabIndex = 18
        Me.validateCustomerButton.Text = "Validate"
        Me.validateCustomerButton.UseVisualStyleBackColor = True
        '
        'customerValidationProvider
        '
        Me.customerValidationProvider.Enabled = False
        Me.customerValidationProvider.ErrorProvider = Me.errorProvider
        Me.customerValidationProvider.RulesetName = "RuleSetA"
        Me.customerValidationProvider.SourceTypeName = "ValidationQuickStart.BusinessEntities.Customer, ValidationQuickStart.BusinessEnti" & _
            "ties"
        '
        'errorProvider
        '
        Me.errorProvider.ContainerControl = Me
        '
        'enableWinFormsValidationCheckBox
        '
        Me.enableWinFormsValidationCheckBox.AutoSize = True
        Me.enableWinFormsValidationCheckBox.Location = New System.Drawing.Point(18, 393)
        Me.enableWinFormsValidationCheckBox.Name = "enableWinFormsValidationCheckBox"
        Me.enableWinFormsValidationCheckBox.Size = New System.Drawing.Size(209, 17)
        Me.addressValidationProvider.SetSourcePropertyName(Me.enableWinFormsValidationCheckBox, Nothing)
        Me.customerValidationProvider.SetSourcePropertyName(Me.enableWinFormsValidationCheckBox, Nothing)
        Me.enableWinFormsValidationCheckBox.TabIndex = 24
        Me.enableWinFormsValidationCheckBox.Text = "Enable WinForms-Integrated Validation"
        Me.enableWinFormsValidationCheckBox.UseVisualStyleBackColor = True
        '
        'panel2
        '
        Me.panel2.Location = New System.Drawing.Point(26, 182)
        Me.panel2.Name = "panel2"
        Me.panel2.Size = New System.Drawing.Size(47, 49)
        Me.addressValidationProvider.SetSourcePropertyName(Me.panel2, Nothing)
        Me.customerValidationProvider.SetSourcePropertyName(Me.panel2, Nothing)
        Me.panel2.TabIndex = 23
        '
        'panel1
        '
        Me.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.panel1.Location = New System.Drawing.Point(24, 182)
        Me.panel1.Name = "panel1"
        Me.panel1.Size = New System.Drawing.Size(34, 53)
        Me.addressValidationProvider.SetSourcePropertyName(Me.panel1, Nothing)
        Me.customerValidationProvider.SetSourcePropertyName(Me.panel1, Nothing)
        Me.panel1.TabIndex = 22
        '
        'exitButton
        '
        Me.exitButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.exitButton.Location = New System.Drawing.Point(567, 384)
        Me.exitButton.Name = "exitButton"
        Me.exitButton.Size = New System.Drawing.Size(111, 26)
        Me.addressValidationProvider.SetSourcePropertyName(Me.exitButton, Nothing)
        Me.customerValidationProvider.SetSourcePropertyName(Me.exitButton, Nothing)
        Me.exitButton.TabIndex = 21
        Me.exitButton.Text = "E&xit"
        Me.exitButton.UseVisualStyleBackColor = True
        '
        'viewWalkthroughButton
        '
        Me.viewWalkthroughButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.viewWalkthroughButton.Location = New System.Drawing.Point(452, 384)
        Me.viewWalkthroughButton.Name = "viewWalkthroughButton"
        Me.viewWalkthroughButton.Size = New System.Drawing.Size(111, 26)
        Me.addressValidationProvider.SetSourcePropertyName(Me.viewWalkthroughButton, Nothing)
        Me.customerValidationProvider.SetSourcePropertyName(Me.viewWalkthroughButton, Nothing)
        Me.viewWalkthroughButton.TabIndex = 20
        Me.viewWalkthroughButton.Text = "View Walkthrough"
        Me.viewWalkthroughButton.UseVisualStyleBackColor = True
        '
        'customerResultsTreeView
        '
        Me.customerResultsTreeView.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.customerResultsTreeView.Location = New System.Drawing.Point(344, 42)
        Me.customerResultsTreeView.Name = "customerResultsTreeView"
        Me.customerResultsTreeView.Size = New System.Drawing.Size(334, 328)
        Me.customerValidationProvider.SetSourcePropertyName(Me.customerResultsTreeView, Nothing)
        Me.addressValidationProvider.SetSourcePropertyName(Me.customerResultsTreeView, Nothing)
        Me.customerResultsTreeView.TabIndex = 19
        '
        'birthDatePicker
        '
        Me.birthDatePicker.Location = New System.Drawing.Point(81, 78)
        Me.birthDatePicker.Name = "birthDatePicker"
        Me.customerValidationProvider.SetPerformValidation(Me.birthDatePicker, True)
        Me.birthDatePicker.Size = New System.Drawing.Size(211, 20)
        Me.addressValidationProvider.SetSourcePropertyName(Me.birthDatePicker, Nothing)
        Me.customerValidationProvider.SetSourcePropertyName(Me.birthDatePicker, "DateOfBirth")
        Me.birthDatePicker.TabIndex = 5
        Me.customerValidationProvider.SetValidatedProperty(Me.birthDatePicker, "Value")
        '
        'rewardPointTextBox
        '
        Me.rewardPointTextBox.Location = New System.Drawing.Point(81, 134)
        Me.rewardPointTextBox.Name = "rewardPointTextBox"
        Me.customerValidationProvider.SetPerformValidation(Me.rewardPointTextBox, True)
        Me.rewardPointTextBox.Size = New System.Drawing.Size(95, 20)
        Me.addressValidationProvider.SetSourcePropertyName(Me.rewardPointTextBox, "")
        Me.customerValidationProvider.SetSourcePropertyName(Me.rewardPointTextBox, "RewardPoints")
        Me.rewardPointTextBox.TabIndex = 9
        '
        'emailTextBox
        '
        Me.emailTextBox.Location = New System.Drawing.Point(81, 106)
        Me.emailTextBox.Name = "emailTextBox"
        Me.customerValidationProvider.SetPerformValidation(Me.emailTextBox, True)
        Me.emailTextBox.Size = New System.Drawing.Size(211, 20)
        Me.addressValidationProvider.SetSourcePropertyName(Me.emailTextBox, Nothing)
        Me.customerValidationProvider.SetSourcePropertyName(Me.emailTextBox, "Email")
        Me.emailTextBox.TabIndex = 7
        '
        'lastNameTextBox
        '
        Me.lastNameTextBox.Location = New System.Drawing.Point(81, 47)
        Me.lastNameTextBox.Name = "lastNameTextBox"
        Me.customerValidationProvider.SetPerformValidation(Me.lastNameTextBox, True)
        Me.lastNameTextBox.Size = New System.Drawing.Size(211, 20)
        Me.addressValidationProvider.SetSourcePropertyName(Me.lastNameTextBox, Nothing)
        Me.customerValidationProvider.SetSourcePropertyName(Me.lastNameTextBox, "LastName")
        Me.lastNameTextBox.TabIndex = 3
        '
        'firstNameTextBox
        '
        Me.firstNameTextBox.Location = New System.Drawing.Point(81, 17)
        Me.firstNameTextBox.Name = "firstNameTextBox"
        Me.customerValidationProvider.SetPerformValidation(Me.firstNameTextBox, True)
        Me.firstNameTextBox.Size = New System.Drawing.Size(211, 20)
        Me.addressValidationProvider.SetSourcePropertyName(Me.firstNameTextBox, "")
        Me.customerValidationProvider.SetSourcePropertyName(Me.firstNameTextBox, "FirstName")
        Me.firstNameTextBox.TabIndex = 1
        '
        'groupBox1
        '
        Me.groupBox1.Controls.Add(Me.birthDatePicker)
        Me.groupBox1.Controls.Add(Me.rewardPointTextBox)
        Me.groupBox1.Controls.Add(Me.emailTextBox)
        Me.groupBox1.Controls.Add(Me.lastNameTextBox)
        Me.groupBox1.Controls.Add(Me.firstNameTextBox)
        Me.groupBox1.Controls.Add(Me.label5)
        Me.groupBox1.Controls.Add(Me.label4)
        Me.groupBox1.Controls.Add(Me.label3)
        Me.groupBox1.Controls.Add(Me.label2)
        Me.groupBox1.Controls.Add(Me.label1)
        Me.groupBox1.Location = New System.Drawing.Point(12, 12)
        Me.groupBox1.Name = "groupBox1"
        Me.groupBox1.Size = New System.Drawing.Size(315, 172)
        Me.addressValidationProvider.SetSourcePropertyName(Me.groupBox1, Nothing)
        Me.customerValidationProvider.SetSourcePropertyName(Me.groupBox1, Nothing)
        Me.groupBox1.TabIndex = 14
        Me.groupBox1.TabStop = False
        Me.groupBox1.Text = "Customer"
        '
        'label5
        '
        Me.label5.AutoSize = True
        Me.label5.Location = New System.Drawing.Point(7, 137)
        Me.label5.Name = "label5"
        Me.label5.Size = New System.Drawing.Size(79, 13)
        Me.addressValidationProvider.SetSourcePropertyName(Me.label5, Nothing)
        Me.customerValidationProvider.SetSourcePropertyName(Me.label5, Nothing)
        Me.label5.TabIndex = 8
        Me.label5.Text = "Reward Points:"
        '
        'label4
        '
        Me.label4.AutoSize = True
        Me.label4.Location = New System.Drawing.Point(7, 109)
        Me.label4.Name = "label4"
        Me.label4.Size = New System.Drawing.Size(39, 13)
        Me.addressValidationProvider.SetSourcePropertyName(Me.label4, Nothing)
        Me.customerValidationProvider.SetSourcePropertyName(Me.label4, Nothing)
        Me.label4.TabIndex = 6
        Me.label4.Text = "E-Mail:"
        '
        'label3
        '
        Me.label3.AutoSize = True
        Me.label3.Location = New System.Drawing.Point(7, 84)
        Me.label3.Name = "label3"
        Me.label3.Size = New System.Drawing.Size(71, 13)
        Me.customerValidationProvider.SetSourcePropertyName(Me.label3, Nothing)
        Me.addressValidationProvider.SetSourcePropertyName(Me.label3, Nothing)
        Me.label3.TabIndex = 4
        Me.label3.Text = "Date Of Birth:"
        '
        'label2
        '
        Me.label2.AutoSize = True
        Me.label2.Location = New System.Drawing.Point(7, 50)
        Me.label2.Name = "label2"
        Me.label2.Size = New System.Drawing.Size(61, 13)
        Me.addressValidationProvider.SetSourcePropertyName(Me.label2, Nothing)
        Me.customerValidationProvider.SetSourcePropertyName(Me.label2, Nothing)
        Me.label2.TabIndex = 2
        Me.label2.Text = "Last Name:"
        '
        'label1
        '
        Me.label1.AutoSize = True
        Me.label1.Location = New System.Drawing.Point(7, 20)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(60, 13)
        Me.addressValidationProvider.SetSourcePropertyName(Me.label1, Nothing)
        Me.customerValidationProvider.SetSourcePropertyName(Me.label1, Nothing)
        Me.label1.TabIndex = 0
        Me.label1.Text = "First Name:"
        '
        'addressValidationProvider
        '
        Me.addressValidationProvider.Enabled = False
        Me.addressValidationProvider.ErrorProvider = Me.errorProvider
        Me.addressValidationProvider.RulesetName = "ValidAddress"
        Me.addressValidationProvider.SourceTypeName = "ValidationQuickStart.BusinessEntities.Address, ValidationQuickStart.BusinessEntit" & _
            "ies"
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange
        Me.ClientSize = New System.Drawing.Size(690, 433)
        Me.Controls.Add(Me.groupBox2)
        Me.Controls.Add(Me.customerRuleSetCombo)
        Me.Controls.Add(Me.label12)
        Me.Controls.Add(Me.validateCustomerButton)
        Me.Controls.Add(Me.enableWinFormsValidationCheckBox)
        Me.Controls.Add(Me.panel2)
        Me.Controls.Add(Me.panel1)
        Me.Controls.Add(Me.exitButton)
        Me.Controls.Add(Me.viewWalkthroughButton)
        Me.Controls.Add(Me.customerResultsTreeView)
        Me.Controls.Add(Me.groupBox1)
        Me.Name = "MainForm"
        Me.customerValidationProvider.SetSourcePropertyName(Me, Nothing)
        Me.addressValidationProvider.SetSourcePropertyName(Me, Nothing)
        Me.Text = "Validation QuickStart"
        Me.groupBox2.ResumeLayout(False)
        Me.groupBox2.PerformLayout()
        CType(Me.errorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me.groupBox1.ResumeLayout(False)
        Me.groupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents groupBox2 As System.Windows.Forms.GroupBox
    Private WithEvents label10 As System.Windows.Forms.Label
    Private WithEvents addressValidationProvider As Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WinForms.ValidationProvider
    Private WithEvents errorProvider As System.Windows.Forms.ErrorProvider
    Private WithEvents customerRuleSetCombo As System.Windows.Forms.ComboBox
    Private WithEvents customerValidationProvider As Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WinForms.ValidationProvider
    Private WithEvents label12 As System.Windows.Forms.Label
    Private WithEvents validateCustomerButton As System.Windows.Forms.Button
    Private WithEvents enableWinFormsValidationCheckBox As System.Windows.Forms.CheckBox
    Private WithEvents panel2 As System.Windows.Forms.Panel
    Private WithEvents panel1 As System.Windows.Forms.Panel
    Private WithEvents exitButton As System.Windows.Forms.Button
    Private WithEvents viewWalkthroughButton As System.Windows.Forms.Button
    Private WithEvents customerResultsTreeView As System.Windows.Forms.TreeView
    Private WithEvents groupBox1 As System.Windows.Forms.GroupBox
    Private WithEvents birthDatePicker As System.Windows.Forms.DateTimePicker
    Private WithEvents rewardPointTextBox As System.Windows.Forms.TextBox
    Private WithEvents emailTextBox As System.Windows.Forms.TextBox
    Private WithEvents lastNameTextBox As System.Windows.Forms.TextBox
    Private WithEvents firstNameTextBox As System.Windows.Forms.TextBox
    Private WithEvents label5 As System.Windows.Forms.Label
    Private WithEvents label4 As System.Windows.Forms.Label
    Private WithEvents label3 As System.Windows.Forms.Label
    Private WithEvents label2 As System.Windows.Forms.Label
    Private WithEvents label1 As System.Windows.Forms.Label
    Private WithEvents label11 As System.Windows.Forms.Label
    Private WithEvents label9 As System.Windows.Forms.Label
    Private WithEvents label8 As System.Windows.Forms.Label
    Private WithEvents label7 As System.Windows.Forms.Label
    Private WithEvents label6 As System.Windows.Forms.Label
    Private WithEvents postCodeTextBox As System.Windows.Forms.TextBox
    Private WithEvents stateTextBox As System.Windows.Forms.TextBox
    Private WithEvents countryTextBox As System.Windows.Forms.TextBox
    Private WithEvents cityTextBox As System.Windows.Forms.TextBox
    Private WithEvents line2TextBox As System.Windows.Forms.TextBox
    Private WithEvents line1TextBox As System.Windows.Forms.TextBox

End Class
