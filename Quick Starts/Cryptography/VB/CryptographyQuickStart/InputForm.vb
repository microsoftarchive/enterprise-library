'===============================================================================
' Microsoft patterns & practices Enterprise Library
' Cryptography Application Block QuickStart
'===============================================================================
' Copyright © Microsoft Corporation.  All rights reserved.
' THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
' OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
' LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
' FITNESS FOR A PARTICULAR PURPOSE.
'===============================================================================

Public Class InputForm
    Inherits System.Windows.Forms.Form

    Private inputText As String

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents groupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents okButton As System.Windows.Forms.Button
    Friend WithEvents inputTextBox As System.Windows.Forms.TextBox
    Friend WithEvents instructionsLabel As System.Windows.Forms.Label
    Friend WithEvents cancelationButton As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(InputForm))
        Me.groupBox1 = New System.Windows.Forms.GroupBox
        Me.cancelationButton = New System.Windows.Forms.Button
        Me.okButton = New System.Windows.Forms.Button
        Me.inputTextBox = New System.Windows.Forms.TextBox
        Me.instructionsLabel = New System.Windows.Forms.Label
        Me.groupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'groupBox1
        '
        Me.groupBox1.AccessibleDescription = resources.GetString("groupBox1.AccessibleDescription")
        Me.groupBox1.AccessibleName = resources.GetString("groupBox1.AccessibleName")
        Me.groupBox1.Anchor = CType(resources.GetObject("groupBox1.Anchor"), System.Windows.Forms.AnchorStyles)
        Me.groupBox1.BackgroundImage = CType(resources.GetObject("groupBox1.BackgroundImage"), System.Drawing.Image)
        Me.groupBox1.Controls.Add(Me.cancelationButton)
        Me.groupBox1.Controls.Add(Me.okButton)
        Me.groupBox1.Dock = CType(resources.GetObject("groupBox1.Dock"), System.Windows.Forms.DockStyle)
        Me.groupBox1.Enabled = CType(resources.GetObject("groupBox1.Enabled"), Boolean)
        Me.groupBox1.Font = CType(resources.GetObject("groupBox1.Font"), System.Drawing.Font)
        Me.groupBox1.ImeMode = CType(resources.GetObject("groupBox1.ImeMode"), System.Windows.Forms.ImeMode)
        Me.groupBox1.Location = CType(resources.GetObject("groupBox1.Location"), System.Drawing.Point)
        Me.groupBox1.Name = "groupBox1"
        Me.groupBox1.RightToLeft = CType(resources.GetObject("groupBox1.RightToLeft"), System.Windows.Forms.RightToLeft)
        Me.groupBox1.Size = CType(resources.GetObject("groupBox1.Size"), System.Drawing.Size)
        Me.groupBox1.TabIndex = CType(resources.GetObject("groupBox1.TabIndex"), Integer)
        Me.groupBox1.TabStop = False
        Me.groupBox1.Text = resources.GetString("groupBox1.Text")
        Me.groupBox1.Visible = CType(resources.GetObject("groupBox1.Visible"), Boolean)
        '
        'cancelationButton
        '
        Me.cancelationButton.AccessibleDescription = resources.GetString("cancelationButton.AccessibleDescription")
        Me.cancelationButton.AccessibleName = resources.GetString("cancelationButton.AccessibleName")
        Me.cancelationButton.Anchor = CType(resources.GetObject("cancelationButton.Anchor"), System.Windows.Forms.AnchorStyles)
        Me.cancelationButton.BackgroundImage = CType(resources.GetObject("cancelationButton.BackgroundImage"), System.Drawing.Image)
        Me.cancelationButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cancelationButton.Dock = CType(resources.GetObject("cancelationButton.Dock"), System.Windows.Forms.DockStyle)
        Me.cancelationButton.Enabled = CType(resources.GetObject("cancelationButton.Enabled"), Boolean)
        Me.cancelationButton.FlatStyle = CType(resources.GetObject("cancelationButton.FlatStyle"), System.Windows.Forms.FlatStyle)
        Me.cancelationButton.Font = CType(resources.GetObject("cancelationButton.Font"), System.Drawing.Font)
        Me.cancelationButton.Image = CType(resources.GetObject("cancelationButton.Image"), System.Drawing.Image)
        Me.cancelationButton.ImageAlign = CType(resources.GetObject("cancelationButton.ImageAlign"), System.Drawing.ContentAlignment)
        Me.cancelationButton.ImageIndex = CType(resources.GetObject("cancelationButton.ImageIndex"), Integer)
        Me.cancelationButton.ImeMode = CType(resources.GetObject("cancelationButton.ImeMode"), System.Windows.Forms.ImeMode)
        Me.cancelationButton.Location = CType(resources.GetObject("cancelationButton.Location"), System.Drawing.Point)
        Me.cancelationButton.Name = "cancelationButton"
        Me.cancelationButton.RightToLeft = CType(resources.GetObject("cancelationButton.RightToLeft"), System.Windows.Forms.RightToLeft)
        Me.cancelationButton.Size = CType(resources.GetObject("cancelationButton.Size"), System.Drawing.Size)
        Me.cancelationButton.TabIndex = CType(resources.GetObject("cancelationButton.TabIndex"), Integer)
        Me.cancelationButton.Text = resources.GetString("cancelationButton.Text")
        Me.cancelationButton.TextAlign = CType(resources.GetObject("cancelationButton.TextAlign"), System.Drawing.ContentAlignment)
        Me.cancelationButton.Visible = CType(resources.GetObject("cancelationButton.Visible"), Boolean)
        '
        'okButton
        '
        Me.okButton.AccessibleDescription = resources.GetString("okButton.AccessibleDescription")
        Me.okButton.AccessibleName = resources.GetString("okButton.AccessibleName")
        Me.okButton.Anchor = CType(resources.GetObject("okButton.Anchor"), System.Windows.Forms.AnchorStyles)
        Me.okButton.BackgroundImage = CType(resources.GetObject("okButton.BackgroundImage"), System.Drawing.Image)
        Me.okButton.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.okButton.Dock = CType(resources.GetObject("okButton.Dock"), System.Windows.Forms.DockStyle)
        Me.okButton.Enabled = CType(resources.GetObject("okButton.Enabled"), Boolean)
        Me.okButton.FlatStyle = CType(resources.GetObject("okButton.FlatStyle"), System.Windows.Forms.FlatStyle)
        Me.okButton.Font = CType(resources.GetObject("okButton.Font"), System.Drawing.Font)
        Me.okButton.Image = CType(resources.GetObject("okButton.Image"), System.Drawing.Image)
        Me.okButton.ImageAlign = CType(resources.GetObject("okButton.ImageAlign"), System.Drawing.ContentAlignment)
        Me.okButton.ImageIndex = CType(resources.GetObject("okButton.ImageIndex"), Integer)
        Me.okButton.ImeMode = CType(resources.GetObject("okButton.ImeMode"), System.Windows.Forms.ImeMode)
        Me.okButton.Location = CType(resources.GetObject("okButton.Location"), System.Drawing.Point)
        Me.okButton.Name = "okButton"
        Me.okButton.RightToLeft = CType(resources.GetObject("okButton.RightToLeft"), System.Windows.Forms.RightToLeft)
        Me.okButton.Size = CType(resources.GetObject("okButton.Size"), System.Drawing.Size)
        Me.okButton.TabIndex = CType(resources.GetObject("okButton.TabIndex"), Integer)
        Me.okButton.Text = resources.GetString("okButton.Text")
        Me.okButton.TextAlign = CType(resources.GetObject("okButton.TextAlign"), System.Drawing.ContentAlignment)
        Me.okButton.Visible = CType(resources.GetObject("okButton.Visible"), Boolean)
        '
        'inputTextBox
        '
        Me.inputTextBox.AccessibleDescription = resources.GetString("inputTextBox.AccessibleDescription")
        Me.inputTextBox.AccessibleName = resources.GetString("inputTextBox.AccessibleName")
        Me.inputTextBox.Anchor = CType(resources.GetObject("inputTextBox.Anchor"), System.Windows.Forms.AnchorStyles)
        Me.inputTextBox.AutoSize = CType(resources.GetObject("inputTextBox.AutoSize"), Boolean)
        Me.inputTextBox.BackgroundImage = CType(resources.GetObject("inputTextBox.BackgroundImage"), System.Drawing.Image)
        Me.inputTextBox.Dock = CType(resources.GetObject("inputTextBox.Dock"), System.Windows.Forms.DockStyle)
        Me.inputTextBox.Enabled = CType(resources.GetObject("inputTextBox.Enabled"), Boolean)
        Me.inputTextBox.Font = CType(resources.GetObject("inputTextBox.Font"), System.Drawing.Font)
        Me.inputTextBox.ImeMode = CType(resources.GetObject("inputTextBox.ImeMode"), System.Windows.Forms.ImeMode)
        Me.inputTextBox.Location = CType(resources.GetObject("inputTextBox.Location"), System.Drawing.Point)
        Me.inputTextBox.MaxLength = CType(resources.GetObject("inputTextBox.MaxLength"), Integer)
        Me.inputTextBox.Multiline = CType(resources.GetObject("inputTextBox.Multiline"), Boolean)
        Me.inputTextBox.Name = "inputTextBox"
        Me.inputTextBox.PasswordChar = CType(resources.GetObject("inputTextBox.PasswordChar"), Char)
        Me.inputTextBox.RightToLeft = CType(resources.GetObject("inputTextBox.RightToLeft"), System.Windows.Forms.RightToLeft)
        Me.inputTextBox.ScrollBars = CType(resources.GetObject("inputTextBox.ScrollBars"), System.Windows.Forms.ScrollBars)
        Me.inputTextBox.Size = CType(resources.GetObject("inputTextBox.Size"), System.Drawing.Size)
        Me.inputTextBox.TabIndex = CType(resources.GetObject("inputTextBox.TabIndex"), Integer)
        Me.inputTextBox.Text = resources.GetString("inputTextBox.Text")
        Me.inputTextBox.TextAlign = CType(resources.GetObject("inputTextBox.TextAlign"), System.Windows.Forms.HorizontalAlignment)
        Me.inputTextBox.Visible = CType(resources.GetObject("inputTextBox.Visible"), Boolean)
        Me.inputTextBox.WordWrap = CType(resources.GetObject("inputTextBox.WordWrap"), Boolean)
        '
        'instructionsLabel
        '
        Me.instructionsLabel.AccessibleDescription = resources.GetString("instructionsLabel.AccessibleDescription")
        Me.instructionsLabel.AccessibleName = resources.GetString("instructionsLabel.AccessibleName")
        Me.instructionsLabel.Anchor = CType(resources.GetObject("instructionsLabel.Anchor"), System.Windows.Forms.AnchorStyles)
        Me.instructionsLabel.AutoSize = CType(resources.GetObject("instructionsLabel.AutoSize"), Boolean)
        Me.instructionsLabel.Dock = CType(resources.GetObject("instructionsLabel.Dock"), System.Windows.Forms.DockStyle)
        Me.instructionsLabel.Enabled = CType(resources.GetObject("instructionsLabel.Enabled"), Boolean)
        Me.instructionsLabel.Font = CType(resources.GetObject("instructionsLabel.Font"), System.Drawing.Font)
        Me.instructionsLabel.Image = CType(resources.GetObject("instructionsLabel.Image"), System.Drawing.Image)
        Me.instructionsLabel.ImageAlign = CType(resources.GetObject("instructionsLabel.ImageAlign"), System.Drawing.ContentAlignment)
        Me.instructionsLabel.ImageIndex = CType(resources.GetObject("instructionsLabel.ImageIndex"), Integer)
        Me.instructionsLabel.ImeMode = CType(resources.GetObject("instructionsLabel.ImeMode"), System.Windows.Forms.ImeMode)
        Me.instructionsLabel.Location = CType(resources.GetObject("instructionsLabel.Location"), System.Drawing.Point)
        Me.instructionsLabel.Name = "instructionsLabel"
        Me.instructionsLabel.RightToLeft = CType(resources.GetObject("instructionsLabel.RightToLeft"), System.Windows.Forms.RightToLeft)
        Me.instructionsLabel.Size = CType(resources.GetObject("instructionsLabel.Size"), System.Drawing.Size)
        Me.instructionsLabel.TabIndex = CType(resources.GetObject("instructionsLabel.TabIndex"), Integer)
        Me.instructionsLabel.Text = resources.GetString("instructionsLabel.Text")
        Me.instructionsLabel.TextAlign = CType(resources.GetObject("instructionsLabel.TextAlign"), System.Drawing.ContentAlignment)
        Me.instructionsLabel.Visible = CType(resources.GetObject("instructionsLabel.Visible"), Boolean)
        '
        'InputForm
        '
        Me.AcceptButton = Me.okButton
        Me.AccessibleDescription = resources.GetString("$this.AccessibleDescription")
        Me.AccessibleName = resources.GetString("$this.AccessibleName")
        Me.AutoScaleBaseSize = CType(resources.GetObject("$this.AutoScaleBaseSize"), System.Drawing.Size)
        Me.AutoScroll = CType(resources.GetObject("$this.AutoScroll"), Boolean)
        Me.AutoScrollMargin = CType(resources.GetObject("$this.AutoScrollMargin"), System.Drawing.Size)
        Me.AutoScrollMinSize = CType(resources.GetObject("$this.AutoScrollMinSize"), System.Drawing.Size)
        Me.BackgroundImage = CType(resources.GetObject("$this.BackgroundImage"), System.Drawing.Image)
        Me.CancelButton = Me.cancelationButton
        Me.ClientSize = CType(resources.GetObject("$this.ClientSize"), System.Drawing.Size)
        Me.Controls.Add(Me.groupBox1)
        Me.Controls.Add(Me.inputTextBox)
        Me.Controls.Add(Me.instructionsLabel)
        Me.Enabled = CType(resources.GetObject("$this.Enabled"), Boolean)
        Me.Font = CType(resources.GetObject("$this.Font"), System.Drawing.Font)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.ImeMode = CType(resources.GetObject("$this.ImeMode"), System.Windows.Forms.ImeMode)
        Me.Location = CType(resources.GetObject("$this.Location"), System.Drawing.Point)
        Me.MaximizeBox = False
        Me.MaximumSize = CType(resources.GetObject("$this.MaximumSize"), System.Drawing.Size)
        Me.MinimizeBox = False
        Me.MinimumSize = CType(resources.GetObject("$this.MinimumSize"), System.Drawing.Size)
        Me.Name = "InputForm"
        Me.RightToLeft = CType(resources.GetObject("$this.RightToLeft"), System.Windows.Forms.RightToLeft)
        Me.ShowInTaskbar = False
        Me.StartPosition = CType(resources.GetObject("$this.StartPosition"), System.Windows.Forms.FormStartPosition)
        Me.Text = resources.GetString("$this.Text")
        Me.groupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    ' Input text entered by user.
    Public ReadOnly Property Input() As String
        Get
            Return Me.inputText
        End Get
    End Property

    ' Text for the instructions label. Should be set before displaying
    ' this instance.
    Public Property InstructionsText() As String

        Get
            Return Me.instructionsLabel.Text
        End Get
        Set(ByVal Value As String)
            Me.instructionsLabel.Text = Value
        End Set
    End Property

    ' Text for window title. Should be set before displaying this instance.
    Public Property Title() As String
        Get
            Return Me.Text
        End Get
        Set(ByVal Value As String)
            Me.Text = Value
        End Set
    End Property

    ' Handles controls in a proper way before this instance can be seen
    ' by user.
    Protected Overrides Sub OnActivated(ByVal e As EventArgs)

        MyBase.OnActivated(e)

        Me.inputTextBox.Clear()
        Me.inputTextBox.Focus()
    End Sub

    ' Accepts the user input.
    Private Sub okButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles okButton.Click
        Me.inputText = Me.inputTextBox.Text
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    ' Discards the user input.
    Private Sub cancelationButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cancelationButton.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

End Class
