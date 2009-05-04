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

Public Class SelectItemForm
    Private Sub SelectItemForm_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Me.keyTextBox.Clear()
        Me.keyTextBox.Focus()
    End Sub


    ' Key of item selected by user to be read from cache.
    Public ReadOnly Property ItemKey() As String
        Get
            Return Me.keyTextBox.Text
        End Get
    End Property

    Public Sub SetInstructionLabelText(ByVal text As String)
        Me.instructionLabel.Text = text
    End Sub

    Public Sub ClearInputTextBox()
        Me.keyTextBox.Clear()
    End Sub

    Private Sub okButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles okButton.Click
        If (Me.keyTextBox.Text.CompareTo("") <> 0) Then
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        Else
            MessageBox.Show(My.Resources.InvalidKeyMessage, My.Resources.QuickStartTitleMessage, MessageBoxButtons.OK)
        End If
    End Sub

    Private Sub cancelationButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cancelationButton.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub
End Class
