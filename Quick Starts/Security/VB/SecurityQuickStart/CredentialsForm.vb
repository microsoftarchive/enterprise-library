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


Public Class CredentialsForm
    Inherits System.Windows.Forms.Form

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
    Friend WithEvents passwordTextBox As System.Windows.Forms.TextBox
    Friend WithEvents usernameTextBox As System.Windows.Forms.TextBox
    Friend WithEvents groupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents okButton As System.Windows.Forms.Button
    Friend WithEvents label3 As System.Windows.Forms.Label
    Friend WithEvents label2 As System.Windows.Forms.Label
    Friend WithEvents label1 As System.Windows.Forms.Label
    Friend WithEvents cancelationButton As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(CredentialsForm))
        Me.passwordTextBox = New System.Windows.Forms.TextBox
        Me.usernameTextBox = New System.Windows.Forms.TextBox
        Me.groupBox1 = New System.Windows.Forms.GroupBox
        Me.cancelationButton = New System.Windows.Forms.Button
        Me.okButton = New System.Windows.Forms.Button
        Me.label3 = New System.Windows.Forms.Label
        Me.label2 = New System.Windows.Forms.Label
        Me.label1 = New System.Windows.Forms.Label
        Me.groupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'passwordTextBox
        '
        resources.ApplyResources(Me.passwordTextBox, "passwordTextBox")
        Me.passwordTextBox.Name = "passwordTextBox"
        '
        'usernameTextBox
        '
        resources.ApplyResources(Me.usernameTextBox, "usernameTextBox")
        Me.usernameTextBox.Name = "usernameTextBox"
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
        'CredentialsForm
        '
        Me.AcceptButton = Me.okButton
        resources.ApplyResources(Me, "$this")
        Me.CancelButton = Me.cancelationButton
        Me.Controls.Add(Me.passwordTextBox)
        Me.Controls.Add(Me.usernameTextBox)
        Me.Controls.Add(Me.groupBox1)
        Me.Controls.Add(Me.label3)
        Me.Controls.Add(Me.label2)
        Me.Controls.Add(Me.label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "CredentialsForm"
        Me.ShowInTaskbar = False
        Me.groupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    ' Entered user name
    Public ReadOnly Property Username() As String
        Get
            Return Me.usernameTextBox.Text
        End Get
    End Property

    ' Entered user password
    Public ReadOnly Property Password() As String
        Get
            Return Me.passwordTextBox.Text
        End Get
    End Property

    Protected Overrides Sub OnActivated(ByVal e As EventArgs)
        MyBase.OnActivated(e)

        Me.usernameTextBox.Clear()
        Me.passwordTextBox.Clear()

        Me.usernameTextBox.Focus()
    End Sub

    ' Accepts user input
    Private Sub okButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles okButton.Click
        If (Me.usernameTextBox.Text.CompareTo("") = 0 Or _
         Me.passwordTextBox.Text.CompareTo("") = 0) Then
            MessageBox.Show("Please enter a user name and a password", _
              "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else

            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    ' Discards user input
    Private Sub cancelButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cancelationButton.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub
End Class
