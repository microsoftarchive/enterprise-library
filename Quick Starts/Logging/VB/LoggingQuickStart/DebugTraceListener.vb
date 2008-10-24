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

''' <summary>
''' Trace listener that writes formatted messages to the Visual Studio debugger output.
''' </summary>
<ConfigurationElementType(GetType(CustomTraceListenerData))> _
Public Class DebugTraceListener : Inherits CustomTraceListener


    Public Overrides Sub TraceData(ByVal eventCache As TraceEventCache, ByVal source As String, ByVal eventType As TraceEventType, ByVal id As Integer, ByVal data As Object)
        If (TypeOf data Is LogEntry) And Me.Formatter IsNot Nothing Then

            WriteLine(Me.Formatter.Format(DirectCast(data, LogEntry)))
        Else
            WriteLine(data.ToString())
        End If
    End Sub


    ''' <summary>
    ''' Writes a message to the debug window 
    ''' </summary>
    ''' <param name="message">The string to write to the debug window</param>
    Public Overrides Sub Write(ByVal message As String)
        Debug.Write(message)
    End Sub

    ''' <summary>
    ''' Writes a message to the debug window 
    ''' </summary>
    ''' <param name="message">The string to write to the debug window</param>
    Public Overrides Sub WriteLine(ByVal message As String)
        Debug.WriteLine(message)
    End Sub

End Class
