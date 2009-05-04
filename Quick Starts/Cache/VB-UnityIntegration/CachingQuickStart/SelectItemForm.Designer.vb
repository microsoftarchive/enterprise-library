'===============================================================================
' Microsoft patterns & practices Enterprise Library
' Caching Application Block QuickStart
'===============================================================================
' Copyright © Microsoft Corporation.  All rights reserved.
' THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
' OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
' LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
' FITNESS FOR A PARTICULAR PURPOSE.
'===============================================================================

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SelectItemForm
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
        Me.cancelationButton = New System.Windows.Forms.Button
        Me.okButton = New System.Windows.Forms.Button
        Me.keyTextBox = New System.Windows.Forms.TextBox
        Me.instructionLabel = New System.Windows.Forms.Label
        Me.label1 = New System.Windows.Forms.Label
        Me.groupBox1 = New System.Windows.Forms.GroupBox
        Me.groupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'cancelationButton
        '
        Me.cancelationButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cancelationButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cancelationButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.cancelationButton.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.cancelationButton.Location = New System.Drawing.Point(220, 9)
        Me.cancelationButton.Name = "cancelationButton"
        Me.cancelationButton.Size = New System.Drawing.Size(104, 32)
        Me.cancelationButton.TabIndex = 1
        Me.cancelationButton.Text = "&Cancel"
        '
        'okButton
        '
        Me.okButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.okButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.okButton.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.okButton.Location = New System.Drawing.Point(96, 9)
        Me.okButton.Name = "okButton"
        Me.okButton.Size = New System.Drawing.Size(104, 32)
        Me.okButton.TabIndex = 0
        Me.okButton.Text = "&OK"
        '
        'keyTextBox
        '
        Me.keyTextBox.Location = New System.Drawing.Point(58, 54)
        Me.keyTextBox.Name = "keyTextBox"
        Me.keyTextBox.Size = New System.Drawing.Size(200, 20)
        Me.keyTextBox.TabIndex = 6
        '
        'instructionLabel
        '
        Me.instructionLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.instructionLabel.Location = New System.Drawing.Point(12, 9)
        Me.instructionLabel.Name = "instructionLabel"
        Me.instructionLabel.Size = New System.Drawing.Size(256, 40)
        Me.instructionLabel.TabIndex = 4
        Me.instructionLabel.Text = "Reason for selecting item."
        '
        'label1
        '
        Me.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.label1.Location = New System.Drawing.Point(12, 58)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(40, 16)
        Me.label1.TabIndex = 5
        Me.label1.Text = "Key:"
        '
        'groupBox1
        '
        Me.groupBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.groupBox1.Controls.Add(Me.cancelationButton)
        Me.groupBox1.Controls.Add(Me.okButton)
        Me.groupBox1.Location = New System.Drawing.Point(-44, 90)
        Me.groupBox1.Name = "groupBox1"
        Me.groupBox1.Size = New System.Drawing.Size(380, 80)
        Me.groupBox1.TabIndex = 7
        Me.groupBox1.TabStop = False
        '
        'SelectItemForm
        '
        Me.AcceptButton = Me.okButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.cancelationButton
        Me.ClientSize = New System.Drawing.Size(292, 137)
        Me.Controls.Add(Me.keyTextBox)
        Me.Controls.Add(Me.instructionLabel)
        Me.Controls.Add(Me.label1)
        Me.Controls.Add(Me.groupBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "SelectItemForm"
        Me.Text = "Select Item Title"
        Me.groupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cancelationButton As System.Windows.Forms.Button
    Friend WithEvents okButton As System.Windows.Forms.Button
    Friend WithEvents keyTextBox As System.Windows.Forms.TextBox
    Friend WithEvents instructionLabel As System.Windows.Forms.Label
    Friend WithEvents label1 As System.Windows.Forms.Label
    Friend WithEvents groupBox1 As System.Windows.Forms.GroupBox
End Class
