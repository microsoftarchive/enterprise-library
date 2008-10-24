'===============================================================================
' Microsoft patterns & practices Enterprise Library
' Security Application Block QuickStart
'===============================================================================
' Copyright © Microsoft Corporation.  All rights reserved.
' THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
' OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
' LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
' FITNESS FOR A PARTICULAR PURPOSE.
'===============================================================================


Public Class AddUserRoleForm
    Inherits System.Windows.Forms.Form

    Private newUserName As String
    Private userRole As String

    Private membership As MembershipProvider
    Private roles As RoleProvider

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        ResetDataControls()

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
    Friend WithEvents rolesComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents label2 As System.Windows.Forms.Label
    Friend WithEvents usersComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents label1 As System.Windows.Forms.Label
    Friend WithEvents groupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents okButton As System.Windows.Forms.Button
    Friend WithEvents cancelationButton As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(AddUserRoleForm))
        Me.rolesComboBox = New System.Windows.Forms.ComboBox
        Me.label2 = New System.Windows.Forms.Label
        Me.usersComboBox = New System.Windows.Forms.ComboBox
        Me.label1 = New System.Windows.Forms.Label
        Me.groupBox1 = New System.Windows.Forms.GroupBox
        Me.cancelationButton = New System.Windows.Forms.Button
        Me.okButton = New System.Windows.Forms.Button
        Me.groupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'rolesComboBox
        '
        Me.rolesComboBox.AccessibleDescription = resources.GetString("rolesComboBox.AccessibleDescription")
        Me.rolesComboBox.AccessibleName = resources.GetString("rolesComboBox.AccessibleName")
        Me.rolesComboBox.Anchor = CType(resources.GetObject("rolesComboBox.Anchor"), System.Windows.Forms.AnchorStyles)
        Me.rolesComboBox.BackgroundImage = CType(resources.GetObject("rolesComboBox.BackgroundImage"), System.Drawing.Image)
        Me.rolesComboBox.Dock = CType(resources.GetObject("rolesComboBox.Dock"), System.Windows.Forms.DockStyle)
        Me.rolesComboBox.Enabled = CType(resources.GetObject("rolesComboBox.Enabled"), Boolean)
        Me.rolesComboBox.Font = CType(resources.GetObject("rolesComboBox.Font"), System.Drawing.Font)
        Me.rolesComboBox.ImeMode = CType(resources.GetObject("rolesComboBox.ImeMode"), System.Windows.Forms.ImeMode)
        Me.rolesComboBox.IntegralHeight = CType(resources.GetObject("rolesComboBox.IntegralHeight"), Boolean)
        Me.rolesComboBox.ItemHeight = CType(resources.GetObject("rolesComboBox.ItemHeight"), Integer)
        Me.rolesComboBox.Location = CType(resources.GetObject("rolesComboBox.Location"), System.Drawing.Point)
        Me.rolesComboBox.MaxDropDownItems = CType(resources.GetObject("rolesComboBox.MaxDropDownItems"), Integer)
        Me.rolesComboBox.MaxLength = CType(resources.GetObject("rolesComboBox.MaxLength"), Integer)
        Me.rolesComboBox.Name = "rolesComboBox"
        Me.rolesComboBox.RightToLeft = CType(resources.GetObject("rolesComboBox.RightToLeft"), System.Windows.Forms.RightToLeft)
        Me.rolesComboBox.Size = CType(resources.GetObject("rolesComboBox.Size"), System.Drawing.Size)
        Me.rolesComboBox.TabIndex = CType(resources.GetObject("rolesComboBox.TabIndex"), Integer)
        Me.rolesComboBox.Text = resources.GetString("rolesComboBox.Text")
        Me.rolesComboBox.Visible = CType(resources.GetObject("rolesComboBox.Visible"), Boolean)
        '
        'label2
        '
        Me.label2.AccessibleDescription = resources.GetString("label2.AccessibleDescription")
        Me.label2.AccessibleName = resources.GetString("label2.AccessibleName")
        Me.label2.Anchor = CType(resources.GetObject("label2.Anchor"), System.Windows.Forms.AnchorStyles)
        Me.label2.AutoSize = CType(resources.GetObject("label2.AutoSize"), Boolean)
        Me.label2.Dock = CType(resources.GetObject("label2.Dock"), System.Windows.Forms.DockStyle)
        Me.label2.Enabled = CType(resources.GetObject("label2.Enabled"), Boolean)
        Me.label2.Font = CType(resources.GetObject("label2.Font"), System.Drawing.Font)
        Me.label2.Image = CType(resources.GetObject("label2.Image"), System.Drawing.Image)
        Me.label2.ImageAlign = CType(resources.GetObject("label2.ImageAlign"), System.Drawing.ContentAlignment)
        Me.label2.ImageIndex = CType(resources.GetObject("label2.ImageIndex"), Integer)
        Me.label2.ImeMode = CType(resources.GetObject("label2.ImeMode"), System.Windows.Forms.ImeMode)
        Me.label2.Location = CType(resources.GetObject("label2.Location"), System.Drawing.Point)
        Me.label2.Name = "label2"
        Me.label2.RightToLeft = CType(resources.GetObject("label2.RightToLeft"), System.Windows.Forms.RightToLeft)
        Me.label2.Size = CType(resources.GetObject("label2.Size"), System.Drawing.Size)
        Me.label2.TabIndex = CType(resources.GetObject("label2.TabIndex"), Integer)
        Me.label2.Text = resources.GetString("label2.Text")
        Me.label2.TextAlign = CType(resources.GetObject("label2.TextAlign"), System.Drawing.ContentAlignment)
        Me.label2.Visible = CType(resources.GetObject("label2.Visible"), Boolean)
        '
        'usersComboBox
        '
        Me.usersComboBox.AccessibleDescription = resources.GetString("usersComboBox.AccessibleDescription")
        Me.usersComboBox.AccessibleName = resources.GetString("usersComboBox.AccessibleName")
        Me.usersComboBox.Anchor = CType(resources.GetObject("usersComboBox.Anchor"), System.Windows.Forms.AnchorStyles)
        Me.usersComboBox.BackgroundImage = CType(resources.GetObject("usersComboBox.BackgroundImage"), System.Drawing.Image)
        Me.usersComboBox.Dock = CType(resources.GetObject("usersComboBox.Dock"), System.Windows.Forms.DockStyle)
        Me.usersComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.usersComboBox.Enabled = CType(resources.GetObject("usersComboBox.Enabled"), Boolean)
        Me.usersComboBox.Font = CType(resources.GetObject("usersComboBox.Font"), System.Drawing.Font)
        Me.usersComboBox.ImeMode = CType(resources.GetObject("usersComboBox.ImeMode"), System.Windows.Forms.ImeMode)
        Me.usersComboBox.IntegralHeight = CType(resources.GetObject("usersComboBox.IntegralHeight"), Boolean)
        Me.usersComboBox.ItemHeight = CType(resources.GetObject("usersComboBox.ItemHeight"), Integer)
        Me.usersComboBox.Location = CType(resources.GetObject("usersComboBox.Location"), System.Drawing.Point)
        Me.usersComboBox.MaxDropDownItems = CType(resources.GetObject("usersComboBox.MaxDropDownItems"), Integer)
        Me.usersComboBox.MaxLength = CType(resources.GetObject("usersComboBox.MaxLength"), Integer)
        Me.usersComboBox.Name = "usersComboBox"
        Me.usersComboBox.RightToLeft = CType(resources.GetObject("usersComboBox.RightToLeft"), System.Windows.Forms.RightToLeft)
        Me.usersComboBox.Size = CType(resources.GetObject("usersComboBox.Size"), System.Drawing.Size)
        Me.usersComboBox.TabIndex = CType(resources.GetObject("usersComboBox.TabIndex"), Integer)
        Me.usersComboBox.Text = resources.GetString("usersComboBox.Text")
        Me.usersComboBox.Visible = CType(resources.GetObject("usersComboBox.Visible"), Boolean)
        '
        'label1
        '
        Me.label1.AccessibleDescription = resources.GetString("label1.AccessibleDescription")
        Me.label1.AccessibleName = resources.GetString("label1.AccessibleName")
        Me.label1.Anchor = CType(resources.GetObject("label1.Anchor"), System.Windows.Forms.AnchorStyles)
        Me.label1.AutoSize = CType(resources.GetObject("label1.AutoSize"), Boolean)
        Me.label1.Dock = CType(resources.GetObject("label1.Dock"), System.Windows.Forms.DockStyle)
        Me.label1.Enabled = CType(resources.GetObject("label1.Enabled"), Boolean)
        Me.label1.Font = CType(resources.GetObject("label1.Font"), System.Drawing.Font)
        Me.label1.Image = CType(resources.GetObject("label1.Image"), System.Drawing.Image)
        Me.label1.ImageAlign = CType(resources.GetObject("label1.ImageAlign"), System.Drawing.ContentAlignment)
        Me.label1.ImageIndex = CType(resources.GetObject("label1.ImageIndex"), Integer)
        Me.label1.ImeMode = CType(resources.GetObject("label1.ImeMode"), System.Windows.Forms.ImeMode)
        Me.label1.Location = CType(resources.GetObject("label1.Location"), System.Drawing.Point)
        Me.label1.Name = "label1"
        Me.label1.RightToLeft = CType(resources.GetObject("label1.RightToLeft"), System.Windows.Forms.RightToLeft)
        Me.label1.Size = CType(resources.GetObject("label1.Size"), System.Drawing.Size)
        Me.label1.TabIndex = CType(resources.GetObject("label1.TabIndex"), Integer)
        Me.label1.Text = resources.GetString("label1.Text")
        Me.label1.TextAlign = CType(resources.GetObject("label1.TextAlign"), System.Drawing.ContentAlignment)
        Me.label1.Visible = CType(resources.GetObject("label1.Visible"), Boolean)
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
        'AddUserRoleForm
        '
        Me.AccessibleDescription = resources.GetString("$this.AccessibleDescription")
        Me.AccessibleName = resources.GetString("$this.AccessibleName")
        Me.AutoScaleBaseSize = CType(resources.GetObject("$this.AutoScaleBaseSize"), System.Drawing.Size)
        Me.AutoScroll = CType(resources.GetObject("$this.AutoScroll"), Boolean)
        Me.AutoScrollMargin = CType(resources.GetObject("$this.AutoScrollMargin"), System.Drawing.Size)
        Me.AutoScrollMinSize = CType(resources.GetObject("$this.AutoScrollMinSize"), System.Drawing.Size)
        Me.BackgroundImage = CType(resources.GetObject("$this.BackgroundImage"), System.Drawing.Image)
        Me.ClientSize = CType(resources.GetObject("$this.ClientSize"), System.Drawing.Size)
        Me.Controls.Add(Me.rolesComboBox)
        Me.Controls.Add(Me.label2)
        Me.Controls.Add(Me.usersComboBox)
        Me.Controls.Add(Me.label1)
        Me.Controls.Add(Me.groupBox1)
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
        Me.Name = "AddUserRoleForm"
        Me.RightToLeft = CType(resources.GetObject("$this.RightToLeft"), System.Windows.Forms.RightToLeft)
        Me.ShowInTaskbar = False
        Me.StartPosition = CType(resources.GetObject("$this.StartPosition"), System.Windows.Forms.FormStartPosition)
        Me.Text = resources.GetString("$this.Text")
        Me.groupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Public Sub New(ByVal membership As MembershipProvider, ByVal roles As RoleProvider)
        MyBase.New()
        Me.membership = membership
        Me.roles = roles

        InitializeComponent()
        ResetDataControls()

    End Sub

    Public ReadOnly Property UserName() As String
        Get
            Return Me.newUserName
        End Get
    End Property

    Public ReadOnly Property Role() As String
        Get
            Return Me.userRole
        End Get
    End Property

    Public Sub ResetDataControls()
        Me.usersComboBox.Items.Clear()
        Me.rolesComboBox.Items.Clear()

        For Each user As MembershipUser In membership.GetAllUsers()
            Me.usersComboBox.Items.Add(user.UserName)
        Next

        Me.rolesComboBox.Items.AddRange(Roles.GetAllRoles())
        Me.rolesComboBox.Text = ""
    End Sub

    Private Sub okButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles okButton.Click
        If (Me.usersComboBox.SelectedIndex >= 0) Then
            Me.newUserName = CType(Me.usersComboBox.Items(Me.usersComboBox.SelectedIndex), String)
        Else
            Me.newUserName = String.Empty
        End If
        If (Me.rolesComboBox.SelectedIndex >= 0) Then
            Me.userRole = CType(Me.rolesComboBox.Items(Me.rolesComboBox.SelectedIndex), String)
        Else
            Me.userRole = Me.rolesComboBox.Text
        End If

        If (Me.newUserName.CompareTo("") = 0 Or Me.userRole.CompareTo("") = 0) Then
            MessageBox.Show(My.Resources.AddUserRoleErrorMessage, My.Resources.ErrorMessage, _
            MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub cancelButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cancelationButton.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub
End Class

