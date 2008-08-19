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

Public Class FilterQueryForm
    Private _categories As ICollection(Of String) = New List(Of String)(0)

    ''' <summary>
    ''' Priority to use in priority filter query.
    ''' </summary>
    Public ReadOnly Property Priority() As Integer
        Get
            Return Decimal.ToInt32(priorityNumericUpDown.Value)
        End Get
    End Property
 
    ''' <summary>
    ''' Collection of categories to use for category filter query.
    ''' </summary>
    Public ReadOnly Property Categories() As ICollection(Of String)
        Get
            Return _categories
        End Get
    End Property

    Private Sub RecordSelectedCategories()
        If traceCheckBox.Checked Then Categories.Add("Trace")
        If debugCheckbox.Checked Then Categories.Add("Debug")
        If generalCheckbox.Checked Then Categories.Add("General")
        If uiCheckbox.Checked Then Categories.Add("UI Events")
        If dataAccessCheckbox.Checked Then Categories.Add("Data Access Events")
        If troubleshootingCheckbox.Checked Then Categories.Add("Troubleshooting")
    End Sub

    Private Sub OkButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OkButton.Click
        RecordSelectedCategories()

        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub CancelEntryButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CancelEntryButton.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub
End Class