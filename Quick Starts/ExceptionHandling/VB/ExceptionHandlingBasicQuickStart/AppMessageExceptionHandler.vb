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


    Public Function HandleException(ByVal e As Exception, ByVal handlingInstanceID As Guid) As Exception Implements IExceptionHandler.HandleException

        Dim result As DialogResult = Me.ShowThreadExceptionDialog(e)

        Return e
    End Function

    ' Creates the error message and displays it.
    Private Function ShowThreadExceptionDialog(ByVal e As Exception) As DialogResult

        Dim errorMsg As String = "The following exception was caught by the Quick Start Global Exception Handler:" + Environment.NewLine + Environment.NewLine

        Dim sb As StringBuilder = New StringBuilder
        Dim writer As StringWriter = New StringWriter(sb)

        Dim formatter As AppTextExceptionFormatter = New AppTextExceptionFormatter(writer, e)

        ' Format the exception
        formatter.Format()

        errorMsg += sb.ToString()

        Return MessageBox.Show(errorMsg, "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Stop)
    End Function

End Class

