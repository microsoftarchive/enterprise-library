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

Public Class AppTextExceptionFormatter
    Inherits TextExceptionFormatter

    Public Sub New(ByVal writer As TextWriter, ByVal e As Exception)
        MyBase.new(writer, e)
    End Sub

    Protected Overrides Sub WriteDescription()
        ' An exception of type {0} occurred and was caught.
        Dim line As String = String.Format("An exception of type {0} occurred and was caught.", MyBase.Exception.GetType().FullName)
        Me.Writer.WriteLine(line)
    End Sub

    Protected Overrides Sub WriteExceptionType(ByVal exceptionType As Type)
        MyBase.Indent()
        MyBase.Writer.WriteLine("Type : {0}", exceptionType.FullName)
    End Sub

    Public Overrides Sub Format()
        Me.WriteDescription()
        MyBase.WriteMessage(MyBase.Exception.Message)
    End Sub

End Class
