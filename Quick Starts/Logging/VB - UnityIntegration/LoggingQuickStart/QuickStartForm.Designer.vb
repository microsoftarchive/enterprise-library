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
Partial Class QuickStartForm
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
        Me.logExtraInformationButton = New System.Windows.Forms.Button
        Me.logEventInformationButton = New System.Windows.Forms.Button
        Me.viewTraceLogButton = New System.Windows.Forms.Button
        Me.customizedSinkButton = New System.Windows.Forms.Button
        Me.traceButton = New System.Windows.Forms.Button
        Me.checkLogginButton = New System.Windows.Forms.Button
        Me.groupBox2 = New System.Windows.Forms.GroupBox
        Me.viewEventLogButton = New System.Windows.Forms.Button
        Me.groupBox = New System.Windows.Forms.GroupBox
        Me.viewWalkthroughButton = New System.Windows.Forms.Button
        Me.quitButton = New System.Windows.Forms.Button
        Me.groupBox1 = New System.Windows.Forms.GroupBox
        Me.label2 = New System.Windows.Forms.Label
        Me.logoPictureBox = New System.Windows.Forms.PictureBox
        Me.resultsTextBox = New System.Windows.Forms.TextBox
        Me.groupBox2.SuspendLayout()
        Me.groupBox.SuspendLayout()
        Me.groupBox1.SuspendLayout()
        CType(Me.logoPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'logExtraInformationButton
        '
        Me.logExtraInformationButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.logExtraInformationButton.Location = New System.Drawing.Point(17, 128)
        Me.logExtraInformationButton.Name = "logExtraInformationButton"
        Me.logExtraInformationButton.Size = New System.Drawing.Size(158, 45)
        Me.logExtraInformationButton.TabIndex = 27
        Me.logExtraInformationButton.Text = "&Populate a log message with additional context information"
        '
        'logEventInformationButton
        '
        Me.logEventInformationButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.logEventInformationButton.Location = New System.Drawing.Point(17, 75)
        Me.logEventInformationButton.Name = "logEventInformationButton"
        Me.logEventInformationButton.Size = New System.Drawing.Size(158, 45)
        Me.logEventInformationButton.TabIndex = 26
        Me.logEventInformationButton.Text = "&Log event information"
        '
        'viewTraceLogButton
        '
        Me.viewTraceLogButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.viewTraceLogButton.Location = New System.Drawing.Point(14, 19)
        Me.viewTraceLogButton.Name = "viewTraceLogButton"
        Me.viewTraceLogButton.Size = New System.Drawing.Size(132, 32)
        Me.viewTraceLogButton.TabIndex = 0
        Me.viewTraceLogButton.Text = "View T&race Log"
        '
        'customizedSinkButton
        '
        Me.customizedSinkButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.customizedSinkButton.Location = New System.Drawing.Point(17, 232)
        Me.customizedSinkButton.Name = "customizedSinkButton"
        Me.customizedSinkButton.Size = New System.Drawing.Size(158, 45)
        Me.customizedSinkButton.TabIndex = 29
        Me.customizedSinkButton.Text = "Log an event using a customized &sink"
        '
        'traceButton
        '
        Me.traceButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.traceButton.Location = New System.Drawing.Point(17, 180)
        Me.traceButton.Name = "traceButton"
        Me.traceButton.Size = New System.Drawing.Size(158, 45)
        Me.traceButton.TabIndex = 28
        Me.traceButton.Text = "&Trace activities and propogate context information"
        '
        'checkLogginButton
        '
        Me.checkLogginButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.checkLogginButton.Location = New System.Drawing.Point(18, 283)
        Me.checkLogginButton.Name = "checkLogginButton"
        Me.checkLogginButton.Size = New System.Drawing.Size(158, 45)
        Me.checkLogginButton.TabIndex = 30
        Me.checkLogginButton.Text = "&Determine if event will be logged"
        '
        'groupBox2
        '
        Me.groupBox2.Controls.Add(Me.viewTraceLogButton)
        Me.groupBox2.Controls.Add(Me.viewEventLogButton)
        Me.groupBox2.Location = New System.Drawing.Point(18, 334)
        Me.groupBox2.Name = "groupBox2"
        Me.groupBox2.Size = New System.Drawing.Size(157, 104)
        Me.groupBox2.TabIndex = 31
        Me.groupBox2.TabStop = False
        Me.groupBox2.Text = "Trace listener viewers"
        '
        'viewEventLogButton
        '
        Me.viewEventLogButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.viewEventLogButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.viewEventLogButton.Location = New System.Drawing.Point(14, 60)
        Me.viewEventLogButton.Name = "viewEventLogButton"
        Me.viewEventLogButton.Size = New System.Drawing.Size(132, 32)
        Me.viewEventLogButton.TabIndex = 1
        Me.viewEventLogButton.Text = "View &Event Log"
        '
        'groupBox
        '
        Me.groupBox.Controls.Add(Me.viewWalkthroughButton)
        Me.groupBox.Controls.Add(Me.quitButton)
        Me.groupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.groupBox.Location = New System.Drawing.Point(-28, 448)
        Me.groupBox.Name = "groupBox"
        Me.groupBox.Size = New System.Drawing.Size(737, 86)
        Me.groupBox.TabIndex = 32
        Me.groupBox.TabStop = False
        '
        'viewWalkthroughButton
        '
        Me.viewWalkthroughButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.viewWalkthroughButton.Location = New System.Drawing.Point(472, 31)
        Me.viewWalkthroughButton.Name = "viewWalkthroughButton"
        Me.viewWalkthroughButton.Size = New System.Drawing.Size(113, 32)
        Me.viewWalkthroughButton.TabIndex = 0
        Me.viewWalkthroughButton.Text = "View &Walkthrough"
        '
        'quitButton
        '
        Me.quitButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.quitButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.quitButton.Location = New System.Drawing.Point(605, 31)
        Me.quitButton.Name = "quitButton"
        Me.quitButton.Size = New System.Drawing.Size(114, 32)
        Me.quitButton.TabIndex = 1
        Me.quitButton.Text = "&Quit"
        '
        'groupBox1
        '
        Me.groupBox1.BackColor = System.Drawing.Color.White
        Me.groupBox1.Controls.Add(Me.label2)
        Me.groupBox1.Controls.Add(Me.logoPictureBox)
        Me.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.groupBox1.Location = New System.Drawing.Point(-13, -9)
        Me.groupBox1.Name = "groupBox1"
        Me.groupBox1.Size = New System.Drawing.Size(722, 72)
        Me.groupBox1.TabIndex = 34
        Me.groupBox1.TabStop = False
        '
        'label2
        '
        Me.label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.label2.Location = New System.Drawing.Point(25, 26)
        Me.label2.Name = "label2"
        Me.label2.Size = New System.Drawing.Size(244, 31)
        Me.label2.TabIndex = 1
        Me.label2.Text = "Logging"
        '
        'logoPictureBox
        '
        Me.logoPictureBox.Location = New System.Drawing.Point(646, 14)
        Me.logoPictureBox.Name = "logoPictureBox"
        Me.logoPictureBox.Size = New System.Drawing.Size(58, 43)
        Me.logoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.logoPictureBox.TabIndex = 0
        Me.logoPictureBox.TabStop = False
        '
        'resultsTextBox
        '
        Me.resultsTextBox.Location = New System.Drawing.Point(220, 75)
        Me.resultsTextBox.Multiline = True
        Me.resultsTextBox.Name = "resultsTextBox"
        Me.resultsTextBox.ReadOnly = True
        Me.resultsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.resultsTextBox.Size = New System.Drawing.Size(473, 363)
        Me.resultsTextBox.TabIndex = 33
        Me.resultsTextBox.TabStop = False
        '
        'QuickStartForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(703, 523)
        Me.Controls.Add(Me.logExtraInformationButton)
        Me.Controls.Add(Me.logEventInformationButton)
        Me.Controls.Add(Me.customizedSinkButton)
        Me.Controls.Add(Me.traceButton)
        Me.Controls.Add(Me.checkLogginButton)
        Me.Controls.Add(Me.groupBox2)
        Me.Controls.Add(Me.groupBox)
        Me.Controls.Add(Me.groupBox1)
        Me.Controls.Add(Me.resultsTextBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "QuickStartForm"
        Me.Text = "Logging Application Block QuickStart"
        Me.groupBox2.ResumeLayout(False)
        Me.groupBox.ResumeLayout(False)
        Me.groupBox1.ResumeLayout(False)
        Me.groupBox1.PerformLayout()
        CType(Me.logoPictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents logExtraInformationButton As System.Windows.Forms.Button
    Private WithEvents logEventInformationButton As System.Windows.Forms.Button
    Private WithEvents viewTraceLogButton As System.Windows.Forms.Button
    Private WithEvents customizedSinkButton As System.Windows.Forms.Button
    Private WithEvents traceButton As System.Windows.Forms.Button
    Private WithEvents checkLogginButton As System.Windows.Forms.Button
    Private WithEvents groupBox2 As System.Windows.Forms.GroupBox
    Private WithEvents viewEventLogButton As System.Windows.Forms.Button
    Private WithEvents groupBox As System.Windows.Forms.GroupBox
    Private WithEvents viewWalkthroughButton As System.Windows.Forms.Button
    Private WithEvents quitButton As System.Windows.Forms.Button
    Private WithEvents groupBox1 As System.Windows.Forms.GroupBox
    Private WithEvents label2 As System.Windows.Forms.Label
    Private WithEvents logoPictureBox As System.Windows.Forms.PictureBox
    Private WithEvents resultsTextBox As System.Windows.Forms.TextBox

End Class
