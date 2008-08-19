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
Partial Class FilterQueryForm
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
        Me.CancelEntryButton = New System.Windows.Forms.Button
        Me.OkButton = New System.Windows.Forms.Button
        Me.label2 = New System.Windows.Forms.Label
        Me.label1 = New System.Windows.Forms.Label
        Me.troubleshootingCheckbox = New System.Windows.Forms.CheckBox
        Me.dataAccessCheckbox = New System.Windows.Forms.CheckBox
        Me.uiCheckbox = New System.Windows.Forms.CheckBox
        Me.generalCheckbox = New System.Windows.Forms.CheckBox
        Me.priorityNumericUpDown = New System.Windows.Forms.NumericUpDown
        Me.debugCheckbox = New System.Windows.Forms.CheckBox
        Me.traceCheckBox = New System.Windows.Forms.CheckBox
        Me.label5 = New System.Windows.Forms.Label
        Me.label4 = New System.Windows.Forms.Label
        CType(Me.priorityNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'CancelEntryButton
        '
        Me.CancelEntryButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CancelEntryButton.Location = New System.Drawing.Point(164, 188)
        Me.CancelEntryButton.Name = "CancelEntryButton"
        Me.CancelEntryButton.Size = New System.Drawing.Size(75, 23)
        Me.CancelEntryButton.TabIndex = 27
        Me.CancelEntryButton.Text = "Cancel"
        Me.CancelEntryButton.UseVisualStyleBackColor = True
        '
        'OkButton
        '
        Me.OkButton.Location = New System.Drawing.Point(71, 188)
        Me.OkButton.Name = "OkButton"
        Me.OkButton.Size = New System.Drawing.Size(75, 23)
        Me.OkButton.TabIndex = 26
        Me.OkButton.Text = "OK"
        Me.OkButton.UseVisualStyleBackColor = True
        '
        'label2
        '
        Me.label2.AutoSize = True
        Me.label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.label2.Location = New System.Drawing.Point(12, 74)
        Me.label2.Name = "label2"
        Me.label2.Size = New System.Drawing.Size(248, 13)
        Me.label2.TabIndex = 31
        Me.label2.Text = "Select the categories to use for the category query."
        '
        'label1
        '
        Me.label1.AutoSize = True
        Me.label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.label1.Location = New System.Drawing.Point(12, 9)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(240, 13)
        Me.label1.TabIndex = 30
        Me.label1.Text = "Select the priority to use for the priority filter query."
        '
        'troubleshootingCheckbox
        '
        Me.troubleshootingCheckbox.AutoSize = True
        Me.troubleshootingCheckbox.Location = New System.Drawing.Point(166, 148)
        Me.troubleshootingCheckbox.Name = "troubleshootingCheckbox"
        Me.troubleshootingCheckbox.Size = New System.Drawing.Size(102, 17)
        Me.troubleshootingCheckbox.TabIndex = 25
        Me.troubleshootingCheckbox.Text = "Troubleshooting"
        Me.troubleshootingCheckbox.UseVisualStyleBackColor = True
        '
        'dataAccessCheckbox
        '
        Me.dataAccessCheckbox.AutoSize = True
        Me.dataAccessCheckbox.Location = New System.Drawing.Point(166, 125)
        Me.dataAccessCheckbox.Name = "dataAccessCheckbox"
        Me.dataAccessCheckbox.Size = New System.Drawing.Size(123, 17)
        Me.dataAccessCheckbox.TabIndex = 23
        Me.dataAccessCheckbox.Text = "Data Access Events"
        Me.dataAccessCheckbox.UseVisualStyleBackColor = True
        '
        'uiCheckbox
        '
        Me.uiCheckbox.AutoSize = True
        Me.uiCheckbox.Location = New System.Drawing.Point(166, 102)
        Me.uiCheckbox.Name = "uiCheckbox"
        Me.uiCheckbox.Size = New System.Drawing.Size(73, 17)
        Me.uiCheckbox.TabIndex = 21
        Me.uiCheckbox.Text = "UI Events"
        Me.uiCheckbox.UseVisualStyleBackColor = True
        '
        'generalCheckbox
        '
        Me.generalCheckbox.AutoSize = True
        Me.generalCheckbox.Location = New System.Drawing.Point(86, 148)
        Me.generalCheckbox.Name = "generalCheckbox"
        Me.generalCheckbox.Size = New System.Drawing.Size(63, 17)
        Me.generalCheckbox.TabIndex = 24
        Me.generalCheckbox.Text = "General"
        Me.generalCheckbox.UseVisualStyleBackColor = True
        '
        'priorityNumericUpDown
        '
        Me.priorityNumericUpDown.Location = New System.Drawing.Point(86, 40)
        Me.priorityNumericUpDown.Minimum = New Decimal(New Integer() {1, 0, 0, -2147483648})
        Me.priorityNumericUpDown.Name = "priorityNumericUpDown"
        Me.priorityNumericUpDown.Size = New System.Drawing.Size(36, 20)
        Me.priorityNumericUpDown.TabIndex = 19
        Me.priorityNumericUpDown.Value = New Decimal(New Integer() {1, 0, 0, -2147483648})
        '
        'debugCheckbox
        '
        Me.debugCheckbox.AutoSize = True
        Me.debugCheckbox.Location = New System.Drawing.Point(86, 125)
        Me.debugCheckbox.Name = "debugCheckbox"
        Me.debugCheckbox.Size = New System.Drawing.Size(58, 17)
        Me.debugCheckbox.TabIndex = 22
        Me.debugCheckbox.Text = "Debug"
        Me.debugCheckbox.UseVisualStyleBackColor = True
        '
        'traceCheckBox
        '
        Me.traceCheckBox.AutoSize = True
        Me.traceCheckBox.Location = New System.Drawing.Point(86, 102)
        Me.traceCheckBox.Name = "traceCheckBox"
        Me.traceCheckBox.Size = New System.Drawing.Size(54, 17)
        Me.traceCheckBox.TabIndex = 20
        Me.traceCheckBox.Text = "Trace"
        Me.traceCheckBox.UseVisualStyleBackColor = True
        '
        'label5
        '
        Me.label5.AutoSize = True
        Me.label5.Location = New System.Drawing.Point(20, 102)
        Me.label5.Name = "label5"
        Me.label5.Size = New System.Drawing.Size(60, 13)
        Me.label5.TabIndex = 29
        Me.label5.Text = "Categories:"
        '
        'label4
        '
        Me.label4.AutoSize = True
        Me.label4.Location = New System.Drawing.Point(39, 42)
        Me.label4.Name = "label4"
        Me.label4.Size = New System.Drawing.Size(41, 13)
        Me.label4.TabIndex = 28
        Me.label4.Text = "Priority:"
        '
        'FilterQueryForm
        '
        Me.AcceptButton = Me.OkButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.CancelEntryButton
        Me.ClientSize = New System.Drawing.Size(292, 230)
        Me.Controls.Add(Me.CancelEntryButton)
        Me.Controls.Add(Me.OkButton)
        Me.Controls.Add(Me.label2)
        Me.Controls.Add(Me.label1)
        Me.Controls.Add(Me.troubleshootingCheckbox)
        Me.Controls.Add(Me.dataAccessCheckbox)
        Me.Controls.Add(Me.uiCheckbox)
        Me.Controls.Add(Me.generalCheckbox)
        Me.Controls.Add(Me.priorityNumericUpDown)
        Me.Controls.Add(Me.debugCheckbox)
        Me.Controls.Add(Me.traceCheckBox)
        Me.Controls.Add(Me.label5)
        Me.Controls.Add(Me.label4)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FilterQueryForm"
        Me.Text = "FilterQueryForm"
        CType(Me.priorityNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents CancelEntryButton As System.Windows.Forms.Button
    Private WithEvents OkButton As System.Windows.Forms.Button
    Private WithEvents label2 As System.Windows.Forms.Label
    Private WithEvents label1 As System.Windows.Forms.Label
    Private WithEvents troubleshootingCheckbox As System.Windows.Forms.CheckBox
    Private WithEvents dataAccessCheckbox As System.Windows.Forms.CheckBox
    Private WithEvents uiCheckbox As System.Windows.Forms.CheckBox
    Private WithEvents generalCheckbox As System.Windows.Forms.CheckBox
    Private WithEvents priorityNumericUpDown As System.Windows.Forms.NumericUpDown
    Private WithEvents debugCheckbox As System.Windows.Forms.CheckBox
    Private WithEvents traceCheckBox As System.Windows.Forms.CheckBox
    Private WithEvents label5 As System.Windows.Forms.Label
    Private WithEvents label4 As System.Windows.Forms.Label
End Class
