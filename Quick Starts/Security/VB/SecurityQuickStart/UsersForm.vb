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

Public Class UsersForm
    Inherits System.Windows.Forms.Form

    Private newUserName As String

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        Me.ResetDataControls()

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
    Friend WithEvents usersComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents label1 As System.Windows.Forms.Label
    Friend WithEvents cancelationButton As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UsersForm))
        Me.groupBox1 = New System.Windows.Forms.GroupBox
        Me.cancelationButton = New System.Windows.Forms.Button
        Me.okButton = New System.Windows.Forms.Button
        Me.usersComboBox = New System.Windows.Forms.ComboBox
        Me.label1 = New System.Windows.Forms.Label
        Me.groupBox1.SuspendLayout()
        Me.SuspendLayout()
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
        'usersComboBox
        '
        Me.usersComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.usersComboBox, "usersComboBox")
        Me.usersComboBox.Name = "usersComboBox"
        '
        'label1
        '
        resources.ApplyResources(Me.label1, "label1")
        Me.label1.Name = "label1"
        '
        'UsersForm
        '
        Me.AcceptButton = Me.okButton
        resources.ApplyResources(Me, "$this")
        Me.CancelButton = Me.cancelationButton
        Me.Controls.Add(Me.groupBox1)
        Me.Controls.Add(Me.usersComboBox)
        Me.Controls.Add(Me.label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "UsersForm"
        Me.ShowInTaskbar = False
        Me.groupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    ' User name to be deleted from database
    Public ReadOnly Property UserName() As String
        Get
            Return Me.newUserName
        End Get
    End Property

    Public Sub ResetDataControls()
        Me.usersComboBox.Items.Clear()
        For Each user As MembershipUser In Membership.GetAllUsers()
            Me.usersComboBox.Items.Add(user.UserName)
        Next
    End Sub

    Private Sub okButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles okButton.Click
        If (Me.usersComboBox.SelectedIndex < 0) Then
            MessageBox.Show(My.Resources.DeleteUserErrorMessage, _
                My.Resources.ErrorMessage, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            Me.newUserName = Convert.ToString(Me.usersComboBox.Items(Me.usersComboBox.SelectedIndex))

            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub cancelationButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cancelationButton.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub
End Class
