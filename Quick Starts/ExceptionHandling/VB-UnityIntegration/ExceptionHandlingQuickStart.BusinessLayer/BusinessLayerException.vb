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


Public Class BusinessLayerException
    Inherits ApplicationException

    ' Default constructor
    Public Sub New()
        MyBase.New()
    End Sub

    ' Initializes with a specified error message.
    Public Sub New(ByVal message As String)
        MyBase.New(message)
    End Sub


    ' Initializes with a specified error 
    ' message and a reference to the inner exception that is the cause of this exception.
    Public Sub New(ByVal message As String, ByVal exception As Exception)
        MyBase.New(message, exception)
    End Sub

    ' Initializes with serialized data.
    Protected Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
        MyBase.New(info, context)
    End Sub
End Class
