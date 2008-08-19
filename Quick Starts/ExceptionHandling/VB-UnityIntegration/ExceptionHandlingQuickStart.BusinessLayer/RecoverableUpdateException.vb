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


Public Class RecoverableUpdateException
    Inherits ApplicationException

    ''' <summary>
    '''  Default constructor
    ''' </summary>
    Public Sub New()
        MyBase.New()
    End Sub


    ''' <summary>
    ''' Initializes with a specified error message.
    ''' </summary>
    ''' <param name="message">A message that describes the error.</param>
    Public Sub New(ByVal message As String)
        MyBase.new(message)
    End Sub

    ''' <summary>
    ''' Initializes with a specified error 
    ''' message and a reference to the inner exception that is the cause of this exception.
    ''' </summary>
    ''' <param name="message">The error message that explains the reason for the exception.</param>
    ''' <param name="exception">The exception that is the cause of the current exception.</param>
    Public Sub New(ByVal message As String, ByVal exception As Exception)
        MyBase.New(message, exception)
    End Sub

    ''' <summary>
    ''' Initializes with serialized data.
    ''' </summary>
    ''' <param name="info">The object that holds the serialized object data.</param>
    ''' <param name="context">The contextual information about the source or destination.</param>
    Protected Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
        MyBase.new(info, context)
    End Sub
End Class
