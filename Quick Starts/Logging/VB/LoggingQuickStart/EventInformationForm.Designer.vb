'===============================================================================
' Microsoft patterns & practices Enterprise Library
' Logging Application Block QuickStart
'===============================================================================
' Copyright © Microsoft Corporation.  All rights reserved.
' THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
' OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
' LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
' FITNESS FOR A PARTICULAR PURPOSE.
'===============================================================================

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class EventInformationForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.eventIdTextBox = New System.Windows.Forms.TextBox
        Me.CancelEntryButton = New System.Windows.Forms.Button
        Me.OkButton = New System.Windows.Forms.Button
        Me.troubleshootingCheckbox = New System.Windows.Forms.CheckBox
        Me.dataAccessCheckbox = New System.Windows.Forms.CheckBox
        Me.uiCheckbox = New System.Windows.Forms.CheckBox
        Me.generalCheckbox = New System.Windows.Forms.CheckBox
        Me.priorityNumericUpDown = New System.Windows.Forms.NumericUpDown
        Me.debugCheckbox = New System.Windows.Forms.CheckBox
        Me.messageTextbox = New System.Windows.Forms.TextBox
        Me.traceCheckBox = New System.Windows.Forms.CheckBox
        Me.label5 = New System.Windows.Forms.Label
        Me.label4 = New System.Windows.Forms.Label
        Me.label3 = New System.Windows.Forms.Label
        Me.label2 = New System.Windows.Forms.Label
        Me.label1 = New System.Windows.Forms.Label
        CType(Me.priorityNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'eventIdTextBox
        '
        Me.eventIdTextBox.Location = New System.Drawing.Point(108, 39)
        Me.eventIdTextBox.Name = "eventIdTextBox"
        Me.eventIdTextBox.Size = New System.Drawing.Size(44, 20)
        Me.eventIdTextBox.TabIndex = 12
        '
        'CancelEntryButton
        '
        Me.CancelEntryButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CancelEntryButton.Location = New System.Drawing.Point(170, 276)
        Me.CancelEntryButton.Name = "CancelEntryButton"
        Me.CancelEntryButton.Size = New System.Drawing.Size(75, 23)
        Me.CancelEntryButton.TabIndex = 26
        Me.CancelEntryButton.Text = "Cancel"
        Me.CancelEntryButton.UseVisualStyleBackColor = True
        '
        'OkButton
        '
        Me.OkButton.Location = New System.Drawing.Point(77, 276)
        Me.OkButton.Name = "OkButton"
        Me.OkButton.Size = New System.Drawing.Size(75, 23)
        Me.OkButton.TabIndex = 25
        Me.OkButton.Text = "OK"
        Me.OkButton.UseVisualStyleBackColor = True
        '
        'troubleshootingCheckbox
        '
        Me.troubleshootingCheckbox.AutoSize = True
        Me.troubleshootingCheckbox.Location = New System.Drawing.Point(188, 225)
        Me.troubleshootingCheckbox.Name = "troubleshootingCheckbox"
        Me.troubleshootingCheckbox.Size = New System.Drawing.Size(102, 17)
        Me.troubleshootingCheckbox.TabIndex = 24
        Me.troubleshootingCheckbox.Text = "Troubleshooting"
        Me.troubleshootingCheckbox.UseVisualStyleBackColor = True
        '
        'dataAccessCheckbox
        '
        Me.dataAccessCheckbox.AutoSize = True
        Me.dataAccessCheckbox.Location = New System.Drawing.Point(188, 202)
        Me.dataAccessCheckbox.Name = "dataAccessCheckbox"
        Me.dataAccessCheckbox.Size = New System.Drawing.Size(123, 17)
        Me.dataAccessCheckbox.TabIndex = 22
        Me.dataAccessCheckbox.Text = "Data Access Events"
        Me.dataAccessCheckbox.UseVisualStyleBackColor = True
        '
        'uiCheckbox
        '
        Me.uiCheckbox.AutoSize = True
        Me.uiCheckbox.Location = New System.Drawing.Point(188, 179)
        Me.uiCheckbox.Name = "uiCheckbox"
        Me.uiCheckbox.Size = New System.Drawing.Size(73, 17)
        Me.uiCheckbox.TabIndex = 20
        Me.uiCheckbox.Text = "UI Events"
        Me.uiCheckbox.UseVisualStyleBackColor = True
        '
        'generalCheckbox
        '
        Me.generalCheckbox.AutoSize = True
        Me.generalCheckbox.Location = New System.Drawing.Point(108, 225)
        Me.generalCheckbox.Name = "generalCheckbox"
        Me.generalCheckbox.Size = New System.Drawing.Size(63, 17)
        Me.generalCheckbox.TabIndex = 23
        Me.generalCheckbox.Text = "General"
        Me.generalCheckbox.UseVisualStyleBackColor = True
        '
        'priorityNumericUpDown
        '
        Me.priorityNumericUpDown.Location = New System.Drawing.Point(108, 144)
        Me.priorityNumericUpDown.Minimum = New Decimal(New Integer() {1, 0, 0, -2147483648})
        Me.priorityNumericUpDown.Name = "priorityNumericUpDown"
        Me.priorityNumericUpDown.Size = New System.Drawing.Size(44, 20)
        Me.priorityNumericUpDown.TabIndex = 15
        Me.priorityNumericUpDown.Value = New Decimal(New Integer() {1, 0, 0, -2147483648})
        '
        'debugCheckbox
        '
        Me.debugCheckbox.AutoSize = True
        Me.debugCheckbox.Location = New System.Drawing.Point(108, 202)
        Me.debugCheckbox.Name = "debugCheckbox"
        Me.debugCheckbox.Size = New System.Drawing.Size(58, 17)
        Me.debugCheckbox.TabIndex = 21
        Me.debugCheckbox.Text = "Debug"
        Me.debugCheckbox.UseVisualStyleBackColor = True
        '
        'messageTextbox
        '
        Me.messageTextbox.Location = New System.Drawing.Point(108, 71)
        Me.messageTextbox.Multiline = True
        Me.messageTextbox.Name = "messageTextbox"
        Me.messageTextbox.Size = New System.Drawing.Size(181, 54)
        Me.messageTextbox.TabIndex = 13
        '
        'traceCheckBox
        '
        Me.traceCheckBox.AutoSize = True
        Me.traceCheckBox.Location = New System.Drawing.Point(108, 179)
        Me.traceCheckBox.Name = "traceCheckBox"
        Me.traceCheckBox.Size = New System.Drawing.Size(54, 17)
        Me.traceCheckBox.TabIndex = 17
        Me.traceCheckBox.Text = "Trace"
        Me.traceCheckBox.UseVisualStyleBackColor = True
        '
        'label5
        '
        Me.label5.AutoSize = True
        Me.label5.Location = New System.Drawing.Point(8, 183)
        Me.label5.Name = "label5"
        Me.label5.Size = New System.Drawing.Size(91, 13)
        Me.label5.TabIndex = 19
        Me.label5.Text = "Event Categories:"
        '
        'label4
        '
        Me.label4.AutoSize = True
        Me.label4.Location = New System.Drawing.Point(10, 152)
        Me.label4.Name = "label4"
        Me.label4.Size = New System.Drawing.Size(72, 13)
        Me.label4.TabIndex = 18
        Me.label4.Text = "Event Priority:"
        '
        'label3
        '
        Me.label3.AutoSize = True
        Me.label3.Location = New System.Drawing.Point(11, 74)
        Me.label3.Name = "label3"
        Me.label3.Size = New System.Drawing.Size(84, 13)
        Me.label3.TabIndex = 16
        Me.label3.Text = "Event Message:"
        '
        'label2
        '
        Me.label2.AutoSize = True
        Me.label2.Location = New System.Drawing.Point(12, 42)
        Me.label2.Name = "label2"
        Me.label2.Size = New System.Drawing.Size(98, 13)
        Me.label2.TabIndex = 14
        Me.label2.Text = "Event ID (numeric):"
        '
        'label1
        '
        Me.label1.AutoSize = True
        Me.label1.Location = New System.Drawing.Point(8, 11)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(137, 13)
        Me.label1.TabIndex = 11
        Me.label1.Text = "Enter the event information."
        '
        'EventInformationForm
        '
        Me.AcceptButton = Me.OkButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.CancelEntryButton
        Me.ClientSize = New System.Drawing.Size(317, 320)
        Me.Controls.Add(Me.eventIdTextBox)
        Me.Controls.Add(Me.CancelEntryButton)
        Me.Controls.Add(Me.OkButton)
        Me.Controls.Add(Me.troubleshootingCheckbox)
        Me.Controls.Add(Me.dataAccessCheckbox)
        Me.Controls.Add(Me.uiCheckbox)
        Me.Controls.Add(Me.generalCheckbox)
        Me.Controls.Add(Me.priorityNumericUpDown)
        Me.Controls.Add(Me.debugCheckbox)
        Me.Controls.Add(Me.messageTextbox)
        Me.Controls.Add(Me.traceCheckBox)
        Me.Controls.Add(Me.label5)
        Me.Controls.Add(Me.label4)
        Me.Controls.Add(Me.label3)
        Me.Controls.Add(Me.label2)
        Me.Controls.Add(Me.label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "EventInformationForm"
        Me.Text = "Event Information"
        CType(Me.priorityNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents eventIdTextBox As System.Windows.Forms.TextBox
    Private WithEvents CancelEntryButton As System.Windows.Forms.Button
    Private WithEvents OkButton As System.Windows.Forms.Button
    Private WithEvents troubleshootingCheckbox As System.Windows.Forms.CheckBox
    Private WithEvents dataAccessCheckbox As System.Windows.Forms.CheckBox
    Private WithEvents uiCheckbox As System.Windows.Forms.CheckBox
    Private WithEvents generalCheckbox As System.Windows.Forms.CheckBox
    Private WithEvents priorityNumericUpDown As System.Windows.Forms.NumericUpDown
    Private WithEvents debugCheckbox As System.Windows.Forms.CheckBox
    Private WithEvents messageTextbox As System.Windows.Forms.TextBox
    Private WithEvents traceCheckBox As System.Windows.Forms.CheckBox
    Private WithEvents label5 As System.Windows.Forms.Label
    Private WithEvents label4 As System.Windows.Forms.Label
    Private WithEvents label3 As System.Windows.Forms.Label
    Private WithEvents label2 As System.Windows.Forms.Label
    Private WithEvents label1 As System.Windows.Forms.Label
End Class
