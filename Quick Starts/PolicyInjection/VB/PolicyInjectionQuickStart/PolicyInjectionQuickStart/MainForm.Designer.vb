'===============================================================================
' Microsoft patterns & practices Enterprise Library
' Policy Injection Application Block QuickStart
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
        Me.label5 = New System.Windows.Forms.Label
        Me.label3 = New System.Windows.Forms.Label
        Me.exitButton = New System.Windows.Forms.Button
        Me.viewWalkthroughButton = New System.Windows.Forms.Button
        Me.panel1 = New System.Windows.Forms.Panel
        Me.label7 = New System.Windows.Forms.Label
        Me.label6 = New System.Windows.Forms.Label
        Me.exceptionTextBox = New System.Windows.Forms.TextBox
        Me.balanceTextBox = New System.Windows.Forms.TextBox
        Me.label2 = New System.Windows.Forms.Label
        Me.label4 = New System.Windows.Forms.Label
        Me.userComboBox = New System.Windows.Forms.ComboBox
        Me.label1 = New System.Windows.Forms.Label
        Me.withdrawButton = New System.Windows.Forms.Button
        Me.depositButton = New System.Windows.Forms.Button
        Me.openPerfMonButton = New System.Windows.Forms.Button
        Me.balanceInquiryButton = New System.Windows.Forms.Button
        Me.viewLogButton = New System.Windows.Forms.Button
        Me.installPerfCountersButton = New System.Windows.Forms.Button
        Me.uninstallPerfCounters = New System.Windows.Forms.Button
        Me.panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'label5
        '
        Me.label5.AutoSize = True
        Me.label5.Location = New System.Drawing.Point(16, 19)
        Me.label5.Name = "label5"
        Me.label5.Size = New System.Drawing.Size(344, 13)
        Me.label5.TabIndex = 0
        Me.label5.Text = "Logging and Authorization for all members in BusinessLogic namespace"
        '
        'label3
        '
        Me.label3.AutoSize = True
        Me.label3.Location = New System.Drawing.Point(3, 5)
        Me.label3.Name = "label3"
        Me.label3.Size = New System.Drawing.Size(334, 13)
        Me.label3.TabIndex = 0
        Me.label3.Text = "Default Policies (which you can change in configuration if you want):"
        '
        'exitButton
        '
        Me.exitButton.Location = New System.Drawing.Point(326, 377)
        Me.exitButton.Name = "exitButton"
        Me.exitButton.Size = New System.Drawing.Size(107, 32)
        Me.exitButton.TabIndex = 26
        Me.exitButton.Text = "Exit"
        Me.exitButton.UseVisualStyleBackColor = True
        '
        'viewWalkthroughButton
        '
        Me.viewWalkthroughButton.Location = New System.Drawing.Point(213, 377)
        Me.viewWalkthroughButton.Name = "viewWalkthroughButton"
        Me.viewWalkthroughButton.Size = New System.Drawing.Size(107, 32)
        Me.viewWalkthroughButton.TabIndex = 25
        Me.viewWalkthroughButton.Text = "View Walkthrough"
        Me.viewWalkthroughButton.UseVisualStyleBackColor = True
        '
        'panel1
        '
        Me.panel1.BackColor = System.Drawing.SystemColors.Info
        Me.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.panel1.Controls.Add(Me.label7)
        Me.panel1.Controls.Add(Me.label6)
        Me.panel1.Controls.Add(Me.label5)
        Me.panel1.Controls.Add(Me.label3)
        Me.panel1.ForeColor = System.Drawing.SystemColors.InfoText
        Me.panel1.Location = New System.Drawing.Point(12, 289)
        Me.panel1.Name = "panel1"
        Me.panel1.Size = New System.Drawing.Size(421, 72)
        Me.panel1.TabIndex = 17
        '
        'label7
        '
        Me.label7.AutoSize = True
        Me.label7.Location = New System.Drawing.Point(16, 34)
        Me.label7.Name = "label7"
        Me.label7.Size = New System.Drawing.Size(360, 13)
        Me.label7.TabIndex = 0
        Me.label7.Text = "Performance Counters and Exception Handling on BankAccount.Withdraw"
        '
        'label6
        '
        Me.label6.AutoSize = True
        Me.label6.Location = New System.Drawing.Point(3, 50)
        Me.label6.Name = "label6"
        Me.label6.Size = New System.Drawing.Size(362, 13)
        Me.label6.TabIndex = 0
        Me.label6.Text = "Validation handler applied explicitly via attributes to Deposit and Withdraw"
        '
        'exceptionTextBox
        '
        Me.exceptionTextBox.ForeColor = System.Drawing.Color.Red
        Me.exceptionTextBox.Location = New System.Drawing.Point(267, 139)
        Me.exceptionTextBox.Multiline = True
        Me.exceptionTextBox.Name = "exceptionTextBox"
        Me.exceptionTextBox.ReadOnly = True
        Me.exceptionTextBox.Size = New System.Drawing.Size(166, 113)
        Me.exceptionTextBox.TabIndex = 24
        '
        'balanceTextBox
        '
        Me.balanceTextBox.Location = New System.Drawing.Point(267, 90)
        Me.balanceTextBox.Name = "balanceTextBox"
        Me.balanceTextBox.ReadOnly = True
        Me.balanceTextBox.Size = New System.Drawing.Size(166, 21)
        Me.balanceTextBox.TabIndex = 22
        '
        'label2
        '
        Me.label2.AutoSize = True
        Me.label2.Location = New System.Drawing.Point(187, 139)
        Me.label2.Name = "label2"
        Me.label2.Size = New System.Drawing.Size(58, 13)
        Me.label2.TabIndex = 23
        Me.label2.Text = "Exception:"
        '
        'label4
        '
        Me.label4.AutoSize = True
        Me.label4.Location = New System.Drawing.Point(187, 90)
        Me.label4.Name = "label4"
        Me.label4.Size = New System.Drawing.Size(48, 13)
        Me.label4.TabIndex = 21
        Me.label4.Text = "Balance:"
        '
        'userComboBox
        '
        Me.userComboBox.DisplayMember = "Key"
        Me.userComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.userComboBox.FormattingEnabled = True
        Me.userComboBox.Location = New System.Drawing.Point(267, 13)
        Me.userComboBox.Name = "userComboBox"
        Me.userComboBox.Size = New System.Drawing.Size(166, 21)
        Me.userComboBox.TabIndex = 20
        '
        'label1
        '
        Me.label1.AutoSize = True
        Me.label1.Location = New System.Drawing.Point(187, 17)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(73, 13)
        Me.label1.TabIndex = 19
        Me.label1.Text = "Current User:"
        '
        'withdrawButton
        '
        Me.withdrawButton.Location = New System.Drawing.Point(12, 47)
        Me.withdrawButton.Name = "withdrawButton"
        Me.withdrawButton.Size = New System.Drawing.Size(150, 29)
        Me.withdrawButton.TabIndex = 14
        Me.withdrawButton.Text = "Withdraw..."
        Me.withdrawButton.UseVisualStyleBackColor = True
        '
        'depositButton
        '
        Me.depositButton.Location = New System.Drawing.Point(12, 12)
        Me.depositButton.Name = "depositButton"
        Me.depositButton.Size = New System.Drawing.Size(150, 29)
        Me.depositButton.TabIndex = 13
        Me.depositButton.Text = "Deposit..."
        Me.depositButton.UseVisualStyleBackColor = True
        '
        'openPerfMonButton
        '
        Me.openPerfMonButton.Location = New System.Drawing.Point(12, 169)
        Me.openPerfMonButton.Name = "openPerfMonButton"
        Me.openPerfMonButton.Size = New System.Drawing.Size(150, 29)
        Me.openPerfMonButton.TabIndex = 18
        Me.openPerfMonButton.Text = "Open Performance Monitor"
        Me.openPerfMonButton.UseVisualStyleBackColor = True
        '
        'balanceInquiryButton
        '
        Me.balanceInquiryButton.Location = New System.Drawing.Point(12, 82)
        Me.balanceInquiryButton.Name = "balanceInquiryButton"
        Me.balanceInquiryButton.Size = New System.Drawing.Size(150, 29)
        Me.balanceInquiryButton.TabIndex = 15
        Me.balanceInquiryButton.Text = "Balance Inquiry"
        Me.balanceInquiryButton.UseVisualStyleBackColor = True
        '
        'viewLogButton
        '
        Me.viewLogButton.Location = New System.Drawing.Point(12, 134)
        Me.viewLogButton.Name = "viewLogButton"
        Me.viewLogButton.Size = New System.Drawing.Size(150, 29)
        Me.viewLogButton.TabIndex = 16
        Me.viewLogButton.Text = "View Log File"
        Me.viewLogButton.UseVisualStyleBackColor = True
        '
        'installPerfCountersButton
        '
        Me.installPerfCountersButton.Location = New System.Drawing.Point(12, 204)
        Me.installPerfCountersButton.Name = "installPerfCountersButton"
        Me.installPerfCountersButton.Size = New System.Drawing.Size(150, 29)
        Me.installPerfCountersButton.TabIndex = 18
        Me.installPerfCountersButton.Text = "Install Perf Counters"
        Me.installPerfCountersButton.UseVisualStyleBackColor = True
        '
        'uninstallPerfCounters
        '
        Me.uninstallPerfCounters.Location = New System.Drawing.Point(12, 239)
        Me.uninstallPerfCounters.Name = "uninstallPerfCounters"
        Me.uninstallPerfCounters.Size = New System.Drawing.Size(150, 29)
        Me.uninstallPerfCounters.TabIndex = 18
        Me.uninstallPerfCounters.Text = "Uninstall Perf Counters"
        Me.uninstallPerfCounters.UseVisualStyleBackColor = True
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(444, 421)
        Me.Controls.Add(Me.exitButton)
        Me.Controls.Add(Me.viewWalkthroughButton)
        Me.Controls.Add(Me.panel1)
        Me.Controls.Add(Me.exceptionTextBox)
        Me.Controls.Add(Me.balanceTextBox)
        Me.Controls.Add(Me.label2)
        Me.Controls.Add(Me.label4)
        Me.Controls.Add(Me.userComboBox)
        Me.Controls.Add(Me.label1)
        Me.Controls.Add(Me.withdrawButton)
        Me.Controls.Add(Me.depositButton)
        Me.Controls.Add(Me.uninstallPerfCounters)
        Me.Controls.Add(Me.installPerfCountersButton)
        Me.Controls.Add(Me.openPerfMonButton)
        Me.Controls.Add(Me.balanceInquiryButton)
        Me.Controls.Add(Me.viewLogButton)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Policy Injection QuickStart"
        Me.panel1.ResumeLayout(False)
        Me.panel1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents label5 As System.Windows.Forms.Label
    Private WithEvents label3 As System.Windows.Forms.Label
    Private WithEvents exitButton As System.Windows.Forms.Button
    Private WithEvents viewWalkthroughButton As System.Windows.Forms.Button
    Private WithEvents panel1 As System.Windows.Forms.Panel
    Private WithEvents label7 As System.Windows.Forms.Label
    Private WithEvents label6 As System.Windows.Forms.Label
    Private WithEvents exceptionTextBox As System.Windows.Forms.TextBox
    Private WithEvents balanceTextBox As System.Windows.Forms.TextBox
    Private WithEvents label2 As System.Windows.Forms.Label
    Private WithEvents label4 As System.Windows.Forms.Label
    Private WithEvents userComboBox As System.Windows.Forms.ComboBox
    Private WithEvents label1 As System.Windows.Forms.Label
    Private WithEvents withdrawButton As System.Windows.Forms.Button
    Private WithEvents depositButton As System.Windows.Forms.Button
    Private WithEvents openPerfMonButton As System.Windows.Forms.Button
    Private WithEvents balanceInquiryButton As System.Windows.Forms.Button
    Private WithEvents viewLogButton As System.Windows.Forms.Button
    Private WithEvents installPerfCountersButton As System.Windows.Forms.Button
    Private WithEvents uninstallPerfCounters As System.Windows.Forms.Button

End Class
