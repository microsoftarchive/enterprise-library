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

Public Class ProfileForm
    Inherits System.Windows.Forms.Form

    ' Variable for storing profile
    Private userProfile As ProfileInformation = New ProfileInformation

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
    Friend WithEvents themeComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents groupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents okButton As System.Windows.Forms.Button
    Friend WithEvents lastNameTextBox As System.Windows.Forms.TextBox
    Friend WithEvents firstNameTextBox As System.Windows.Forms.TextBox
    Friend WithEvents label4 As System.Windows.Forms.Label
    Friend WithEvents label3 As System.Windows.Forms.Label
    Friend WithEvents label2 As System.Windows.Forms.Label
    Friend WithEvents label1 As System.Windows.Forms.Label
    Friend WithEvents cancelationButton As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ProfileForm))
        Me.themeComboBox = New System.Windows.Forms.ComboBox
        Me.groupBox1 = New System.Windows.Forms.GroupBox
        Me.cancelationButton = New System.Windows.Forms.Button
        Me.okButton = New System.Windows.Forms.Button
        Me.lastNameTextBox = New System.Windows.Forms.TextBox
        Me.firstNameTextBox = New System.Windows.Forms.TextBox
        Me.label4 = New System.Windows.Forms.Label
        Me.label3 = New System.Windows.Forms.Label
        Me.label2 = New System.Windows.Forms.Label
        Me.label1 = New System.Windows.Forms.Label
        Me.groupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'themeComboBox
        '
        Me.themeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.themeComboBox, "themeComboBox")
        Me.themeComboBox.Items.AddRange(New Object() {resources.GetString("themeComboBox.Items"), resources.GetString("themeComboBox.Items1"), resources.GetString("themeComboBox.Items2"), resources.GetString("themeComboBox.Items3")})
        Me.themeComboBox.Name = "themeComboBox"
        '
        'groupBox1
        '
        Me.groupBox1.Controls.Add(Me.cancelationButton)
        Me.groupBox1.Controls.Add(Me.okButton)
        resources.ApplyResources(Me.groupBox1, "groupBox1")
        Me.groupBox1.Name = "groupBox1"
        Me.groupBox1.TabStop = False
        '
        'cancelationButton
        '
        Me.cancelationButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        resources.ApplyResources(Me.cancelationButton, "cancelationButton")
        Me.cancelationButton.Name = "cancelationButton"
        '
        'okButton
        '
        resources.ApplyResources(Me.okButton, "okButton")
        Me.okButton.Name = "okButton"
        '
        'lastNameTextBox
        '
        resources.ApplyResources(Me.lastNameTextBox, "lastNameTextBox")
        Me.lastNameTextBox.Name = "lastNameTextBox"
        '
        'firstNameTextBox
        '
        resources.ApplyResources(Me.firstNameTextBox, "firstNameTextBox")
        Me.firstNameTextBox.Name = "firstNameTextBox"
        '
        'label4
        '
        resources.ApplyResources(Me.label4, "label4")
        Me.label4.Name = "label4"
        '
        'label3
        '
        resources.ApplyResources(Me.label3, "label3")
        Me.label3.Name = "label3"
        '
        'label2
        '
        resources.ApplyResources(Me.label2, "label2")
        Me.label2.Name = "label2"
        '
        'label1
        '
        resources.ApplyResources(Me.label1, "label1")
        Me.label1.Name = "label1"
        '
        'ProfileForm
        '
        Me.AcceptButton = Me.okButton
        resources.ApplyResources(Me, "$this")
        Me.CancelButton = Me.cancelationButton
        Me.Controls.Add(Me.themeComboBox)
        Me.Controls.Add(Me.groupBox1)
        Me.Controls.Add(Me.lastNameTextBox)
        Me.Controls.Add(Me.firstNameTextBox)
        Me.Controls.Add(Me.label4)
        Me.Controls.Add(Me.label3)
        Me.Controls.Add(Me.label2)
        Me.Controls.Add(Me.label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ProfileForm"
        Me.ShowInTaskbar = False
        Me.groupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Public ReadOnly Property Profile() As ProfileInformation
        Get
            Return Me.userProfile
        End Get
    End Property

    Protected Overrides Sub OnActivated(ByVal e As EventArgs)
        Me.firstNameTextBox.Focus()
    End Sub

    Private Sub ProfileForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.themeComboBox.Items.Clear()

        For Each theme As String In System.Enum.GetNames(GetType(ProfileTheme))
            Me.themeComboBox.Items.Add(theme)
        Next

        Me.firstNameTextBox.Text = Me.Profile.FirstName
        Me.lastNameTextBox.Text = Me.Profile.LastName
        Dim activeTheme As Integer = Me.themeComboBox.FindString(Me.userProfile.Theme.ToString())
        Me.themeComboBox.SelectedIndex = activeTheme
    End Sub

    Private Sub okButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles okButton.Click
        Me.Profile.FirstName = Me.firstNameTextBox.Text
        Me.Profile.LastName = Me.lastNameTextBox.Text
        Me.Profile.Theme = CType(Me.themeComboBox.SelectedIndex, ProfileTheme)
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub cancelButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cancelationButton.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

End Class

