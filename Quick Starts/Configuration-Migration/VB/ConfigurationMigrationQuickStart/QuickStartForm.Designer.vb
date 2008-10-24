'===============================================================================
' Microsoft patterns & practices Enterprise Library
' Configuration QuickStart
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(QuickStartForm))
        Me.automaticRefreshCheckBox = New System.Windows.Forms.CheckBox
        Me.clearCacheButton = New System.Windows.Forms.Button
        Me.tabPage1 = New System.Windows.Forms.TabPage
        Me.readXmlConfigDataButton = New System.Windows.Forms.Button
        Me.readResultsTextBox = New System.Windows.Forms.TextBox
        Me.readSampleTextBox = New System.Windows.Forms.RichTextBox
        Me.tabPage2 = New System.Windows.Forms.TabPage
        Me.writeSampleTextBox = New System.Windows.Forms.RichTextBox
        Me.writeXmlConfigDataButton = New System.Windows.Forms.Button
        Me.writeResultsTextBox = New System.Windows.Forms.TextBox
        Me.groupBox1 = New System.Windows.Forms.GroupBox
        Me.label2 = New System.Windows.Forms.Label
        Me.logoPictureBox = New System.Windows.Forms.PictureBox
        Me.label1 = New System.Windows.Forms.Label
        Me.groupBox = New System.Windows.Forms.GroupBox
        Me.quitButton = New System.Windows.Forms.Button
        Me.tabControl1 = New System.Windows.Forms.TabControl
        Me.fontDialog = New System.Windows.Forms.FontDialog
        Me.tabPage1.SuspendLayout()
        Me.tabPage2.SuspendLayout()
        Me.groupBox1.SuspendLayout()
        CType(Me.logoPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.groupBox.SuspendLayout()
        Me.tabControl1.SuspendLayout()
        Me.SuspendLayout()
        '
        'automaticRefreshCheckBox
        '
        Me.automaticRefreshCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.automaticRefreshCheckBox.ImageAlign = System.Drawing.ContentAlignment.TopLeft
        Me.automaticRefreshCheckBox.Location = New System.Drawing.Point(20, 138)
        Me.automaticRefreshCheckBox.Name = "automaticRefreshCheckBox"
        Me.automaticRefreshCheckBox.Size = New System.Drawing.Size(147, 54)
        Me.automaticRefreshCheckBox.TabIndex = 6
        Me.automaticRefreshCheckBox.Text = "Detect changes in configuration storage and automatically refresh"
        '
        'clearCacheButton
        '
        Me.clearCacheButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.clearCacheButton.Location = New System.Drawing.Point(16, 76)
        Me.clearCacheButton.Name = "clearCacheButton"
        Me.clearCacheButton.Size = New System.Drawing.Size(136, 40)
        Me.clearCacheButton.TabIndex = 2
        Me.clearCacheButton.Text = "&Clear configuration cache"
        '
        'tabPage1
        '
        Me.tabPage1.Controls.Add(Me.readXmlConfigDataButton)
        Me.tabPage1.Controls.Add(Me.automaticRefreshCheckBox)
        Me.tabPage1.Controls.Add(Me.readResultsTextBox)
        Me.tabPage1.Controls.Add(Me.clearCacheButton)
        Me.tabPage1.Controls.Add(Me.readSampleTextBox)
        Me.tabPage1.Location = New System.Drawing.Point(4, 22)
        Me.tabPage1.Name = "tabPage1"
        Me.tabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.tabPage1.Size = New System.Drawing.Size(685, 227)
        Me.tabPage1.TabIndex = 0
        Me.tabPage1.Text = "Read"
        Me.tabPage1.UseVisualStyleBackColor = True
        '
        'readXmlConfigDataButton
        '
        Me.readXmlConfigDataButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.readXmlConfigDataButton.Location = New System.Drawing.Point(16, 20)
        Me.readXmlConfigDataButton.Name = "readXmlConfigDataButton"
        Me.readXmlConfigDataButton.Size = New System.Drawing.Size(136, 40)
        Me.readXmlConfigDataButton.TabIndex = 1
        Me.readXmlConfigDataButton.Text = "&Read configuration data from an XML file "
        '
        'readResultsTextBox
        '
        Me.readResultsTextBox.Location = New System.Drawing.Point(173, 13)
        Me.readResultsTextBox.Multiline = True
        Me.readResultsTextBox.Name = "readResultsTextBox"
        Me.readResultsTextBox.ReadOnly = True
        Me.readResultsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.readResultsTextBox.Size = New System.Drawing.Size(496, 104)
        Me.readResultsTextBox.TabIndex = 3
        Me.readResultsTextBox.TabStop = False
        '
        'readSampleTextBox
        '
        Me.readSampleTextBox.Location = New System.Drawing.Point(173, 131)
        Me.readSampleTextBox.Name = "readSampleTextBox"
        Me.readSampleTextBox.ReadOnly = True
        Me.readSampleTextBox.Size = New System.Drawing.Size(494, 83)
        Me.readSampleTextBox.TabIndex = 4
        Me.readSampleTextBox.Text = "Sample Text"
        '
        'tabPage2
        '
        Me.tabPage2.Controls.Add(Me.writeSampleTextBox)
        Me.tabPage2.Controls.Add(Me.writeXmlConfigDataButton)
        Me.tabPage2.Controls.Add(Me.writeResultsTextBox)
        Me.tabPage2.Location = New System.Drawing.Point(4, 22)
        Me.tabPage2.Name = "tabPage2"
        Me.tabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.tabPage2.Size = New System.Drawing.Size(685, 227)
        Me.tabPage2.TabIndex = 1
        Me.tabPage2.Text = "Write"
        Me.tabPage2.UseVisualStyleBackColor = True
        '
        'writeSampleTextBox
        '
        Me.writeSampleTextBox.Location = New System.Drawing.Point(179, 131)
        Me.writeSampleTextBox.Name = "writeSampleTextBox"
        Me.writeSampleTextBox.ReadOnly = True
        Me.writeSampleTextBox.Size = New System.Drawing.Size(494, 83)
        Me.writeSampleTextBox.TabIndex = 6
        Me.writeSampleTextBox.Text = "Sample Text"
        '
        'writeXmlConfigDataButton
        '
        Me.writeXmlConfigDataButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.writeXmlConfigDataButton.Location = New System.Drawing.Point(19, 20)
        Me.writeXmlConfigDataButton.Name = "writeXmlConfigDataButton"
        Me.writeXmlConfigDataButton.Size = New System.Drawing.Size(136, 40)
        Me.writeXmlConfigDataButton.TabIndex = 4
        Me.writeXmlConfigDataButton.Text = "Write &configuration data to an XML file"
        '
        'writeResultsTextBox
        '
        Me.writeResultsTextBox.Location = New System.Drawing.Point(179, 13)
        Me.writeResultsTextBox.Multiline = True
        Me.writeResultsTextBox.Name = "writeResultsTextBox"
        Me.writeResultsTextBox.ReadOnly = True
        Me.writeResultsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.writeResultsTextBox.Size = New System.Drawing.Size(496, 104)
        Me.writeResultsTextBox.TabIndex = 5
        Me.writeResultsTextBox.TabStop = False
        '
        'groupBox1
        '
        Me.groupBox1.BackColor = System.Drawing.Color.White
        Me.groupBox1.Controls.Add(Me.label2)
        Me.groupBox1.Controls.Add(Me.logoPictureBox)
        Me.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.groupBox1.Location = New System.Drawing.Point(-3, -8)
        Me.groupBox1.Name = "groupBox1"
        Me.groupBox1.Size = New System.Drawing.Size(704, 72)
        Me.groupBox1.TabIndex = 9
        Me.groupBox1.TabStop = False
        '
        'label2
        '
        Me.label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.label2.Location = New System.Drawing.Point(16, 24)
        Me.label2.Name = "label2"
        Me.label2.Size = New System.Drawing.Size(384, 24)
        Me.label2.TabIndex = 1
        Me.label2.Text = "Configuration Migration QuickStart"
        '
        'logoPictureBox
        '
        Me.logoPictureBox.Location = New System.Drawing.Point(608, 14)
        Me.logoPictureBox.Name = "logoPictureBox"
        Me.logoPictureBox.Size = New System.Drawing.Size(69, 50)
        Me.logoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.logoPictureBox.TabIndex = 0
        Me.logoPictureBox.TabStop = False
        '
        'label1
        '
        Me.label1.BackColor = System.Drawing.SystemColors.Info
        Me.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.label1.Location = New System.Drawing.Point(6, 72)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(679, 58)
        Me.label1.TabIndex = 12
        Me.label1.Text = resources.GetString("label1.Text")
        '
        'groupBox
        '
        Me.groupBox.Controls.Add(Me.quitButton)
        Me.groupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.groupBox.Location = New System.Drawing.Point(-12, 383)
        Me.groupBox.Name = "groupBox"
        Me.groupBox.Size = New System.Drawing.Size(713, 89)
        Me.groupBox.TabIndex = 10
        Me.groupBox.TabStop = False
        '
        'quitButton
        '
        Me.quitButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.quitButton.Location = New System.Drawing.Point(573, 24)
        Me.quitButton.Name = "quitButton"
        Me.quitButton.Size = New System.Drawing.Size(104, 32)
        Me.quitButton.TabIndex = 1
        Me.quitButton.Text = "&Quit"
        '
        'tabControl1
        '
        Me.tabControl1.Controls.Add(Me.tabPage1)
        Me.tabControl1.Controls.Add(Me.tabPage2)
        Me.tabControl1.Location = New System.Drawing.Point(2, 133)
        Me.tabControl1.Name = "tabControl1"
        Me.tabControl1.SelectedIndex = 0
        Me.tabControl1.Size = New System.Drawing.Size(693, 253)
        Me.tabControl1.TabIndex = 11
        '
        'QuickStartForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(697, 458)
        Me.Controls.Add(Me.groupBox1)
        Me.Controls.Add(Me.label1)
        Me.Controls.Add(Me.groupBox)
        Me.Controls.Add(Me.tabControl1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.Name = "QuickStartForm"
        Me.Text = "Configuration Migration QuickStart"
        Me.tabPage1.ResumeLayout(False)
        Me.tabPage1.PerformLayout()
        Me.tabPage2.ResumeLayout(False)
        Me.tabPage2.PerformLayout()
        Me.groupBox1.ResumeLayout(False)
        Me.groupBox1.PerformLayout()
        CType(Me.logoPictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.groupBox.ResumeLayout(False)
        Me.tabControl1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents automaticRefreshCheckBox As System.Windows.Forms.CheckBox
    Private WithEvents clearCacheButton As System.Windows.Forms.Button
    Private WithEvents tabPage1 As System.Windows.Forms.TabPage
    Private WithEvents readXmlConfigDataButton As System.Windows.Forms.Button
    Private WithEvents readResultsTextBox As System.Windows.Forms.TextBox
    Private WithEvents readSampleTextBox As System.Windows.Forms.RichTextBox
    Private WithEvents tabPage2 As System.Windows.Forms.TabPage
    Private WithEvents writeSampleTextBox As System.Windows.Forms.RichTextBox
    Private WithEvents writeXmlConfigDataButton As System.Windows.Forms.Button
    Private WithEvents writeResultsTextBox As System.Windows.Forms.TextBox
    Private WithEvents groupBox1 As System.Windows.Forms.GroupBox
    Private WithEvents label2 As System.Windows.Forms.Label
    Private WithEvents logoPictureBox As System.Windows.Forms.PictureBox
    Private WithEvents label1 As System.Windows.Forms.Label
    Private WithEvents groupBox As System.Windows.Forms.GroupBox
    Private WithEvents quitButton As System.Windows.Forms.Button
    Private WithEvents tabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents fontDialog As System.Windows.Forms.FontDialog

End Class
