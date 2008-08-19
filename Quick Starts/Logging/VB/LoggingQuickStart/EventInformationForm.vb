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

Public Class EventInformationForm

    Private _eventID As Integer = -1
    Private _categories As ICollection(Of String) = New List(Of String)(0)

    '''<summary>
    ''' Event ID for event to be logged.
    ''' </summary>
    Public ReadOnly Property EventId() As Integer
        Get
            Return EventId
        End Get
    End Property
   

    ''' <summary>
    ''' Message for event to be logged.
    ''' </summary>
    Public ReadOnly Property Message() As String
        Get
            Return messageTextbox.Text
        End Get
    End Property
 
    ''' <summary>
    ''' Priority of event to be logged.
    ''' </summary>
    Public ReadOnly Property Priority() As Integer
        Get
            Return Decimal.ToInt32(priorityNumericUpDown.Value)
        End Get
    End Property


    ''' <summary>
    ''' Collection of categories for the event.
    ''' </summary>
    Public ReadOnly Property Categories() As ICollection(Of String)
        Get
            Return _categories
        End Get
    End Property
        

    Private Sub EventInformationForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        messageTextbox.Focus()
    End Sub

    Private Sub CancelEntryButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CancelEntryButton.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub OkButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OkButton.Click
        RecordSelectedCategories()

        If ValidateInput() Then
            _eventID = Convert.ToInt32(eventIdTextBox.Text)
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub RecordSelectedCategories()
        If traceCheckBox.Checked Then Categories.Add("Trace")
        If debugCheckbox.Checked Then Categories.Add("Debug")
        If generalCheckbox.Checked Then Categories.Add("General")
        If uiCheckbox.Checked Then Categories.Add("UI Events")
        If dataAccessCheckbox.Checked Then Categories.Add("Data Access Events")
        If troubleshootingCheckbox.Checked Then Categories.Add("Troubleshooting")
    End Sub

    ''' <summary>
    ''' Validate contents for the event ID text box, the priority text box and the category.
    ''' </summary>
    ''' <returns></returns>
    Private Function ValidateInput() As Boolean
        Dim errorMessage As String = My.Resources.InvalidDataMessage
        Dim validationError As Boolean = False

        Try
            Dim id As Integer = Convert.ToInt32(eventIdTextBox.Text)
        Catch
            errorMessage += My.Resources.InvalidEventIDMessage
            validationError = True
        End Try
        If (validationError) Then

            MessageBox.Show(errorMessage, My.Resources.QuickStartTitleMessage, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End If

        Return True
    End Function

End Class