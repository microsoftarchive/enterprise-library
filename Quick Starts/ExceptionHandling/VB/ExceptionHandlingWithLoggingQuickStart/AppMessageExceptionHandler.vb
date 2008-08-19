'===============================================================================
' Microsoft patterns & practices Enterprise Library
' Exception Handling Application Block QuickStart
'===============================================================================
' Copyright © Microsoft Corporation.  All rights reserved.
' THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
' OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
' LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
' FITNESS FOR A PARTICULAR PURPOSE.
'===============================================================================

<ConfigurationElementType(GetType(CustomHandlerData))> _
    Public Class AppMessageExceptionHandler
    Implements IExceptionHandler

    Public Sub New(ByVal ignore As NameValueCollection)

    End Sub

    Public Function HandleException(ByVal e As Exception, ByVal correlationID As Guid) As Exception Implements IExceptionHandler.HandleException

        Dim result As DialogResult = Me.ShowThreadExceptionDialog(e)

        ' Exits the program when the user clicks Abort.
        If (result = DialogResult.Abort) Then
            Application.Exit()
        End If
        Return e
    End Function

    ' Creates the error message and displays it.
    Private Function ShowThreadExceptionDialog(ByVal e As Exception) As DialogResult

        Dim errorMsg As String = e.Message + Environment.NewLine + Environment.NewLine

        Return MessageBox.Show(errorMsg, "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Stop)

    End Function

End Class
