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
Partial Class AmountEntryForm
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
        Me._cancelButton = New System.Windows.Forms.Button
        Me.okButton = New System.Windows.Forms.Button
        Me.errorProvider = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.amountTextBox = New System.Windows.Forms.TextBox
        Me.promptLabel = New System.Windows.Forms.Label
        CType(Me.errorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        '_cancelButton
        '
        Me._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me._cancelButton.Location = New System.Drawing.Point(186, 92)
        Me._cancelButton.Name = "cancelButton"
        Me._cancelButton.Size = New System.Drawing.Size(75, 23)
        Me._cancelButton.TabIndex = 7
        Me._cancelButton.Text = "Cancel"
        Me._cancelButton.UseVisualStyleBackColor = True
        '
        'okButton
        '
        Me.okButton.Location = New System.Drawing.Point(96, 92)
        Me.okButton.Name = "okButton"
        Me.okButton.Size = New System.Drawing.Size(75, 23)
        Me.okButton.TabIndex = 6
        Me.okButton.Text = "OK"
        Me.okButton.UseVisualStyleBackColor = True
        '
        'errorProvider
        '
        Me.errorProvider.ContainerControl = Me
        '
        'amountTextBox
        '
        Me.amountTextBox.Location = New System.Drawing.Point(12, 38)
        Me.amountTextBox.Name = "amountTextBox"
        Me.amountTextBox.Size = New System.Drawing.Size(119, 20)
        Me.amountTextBox.TabIndex = 5
        '
        'promptLabel
        '
        Me.promptLabel.AutoSize = True
        Me.promptLabel.Location = New System.Drawing.Point(9, 11)
        Me.promptLabel.Name = "promptLabel"
        Me.promptLabel.Size = New System.Drawing.Size(239, 13)
        Me.promptLabel.TabIndex = 4
        Me.promptLabel.Text = "Please enter the amount that you wish to deposit:"
        '
        'AmountEntryForm
        '
        Me.AcceptButton = Me.okButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me._cancelButton
        Me.ClientSize = New System.Drawing.Size(278, 132)
        Me.Controls.Add(Me._cancelButton)
        Me.Controls.Add(Me.okButton)
        Me.Controls.Add(Me.amountTextBox)
        Me.Controls.Add(Me.promptLabel)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "AmountEntryForm"
        Me.Text = "AmountEntryForm"
        CType(Me.errorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents _cancelButton As System.Windows.Forms.Button
    Private WithEvents okButton As System.Windows.Forms.Button
    Private WithEvents errorProvider As System.Windows.Forms.ErrorProvider
    Private WithEvents amountTextBox As System.Windows.Forms.TextBox
    Private WithEvents promptLabel As System.Windows.Forms.Label
End Class
