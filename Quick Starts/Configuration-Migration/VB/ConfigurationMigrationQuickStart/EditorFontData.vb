'===============================================================================
' Microsoft patterns & practices Enterprise Library
' Configuration QuickStart
'===============================================================================
' Copyright © Microsoft Corporation.  All rights reserved.
' THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
' OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
' LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
' FITNESS FOR A PARTICULAR PURPOSE.
'===============================================================================

Public Class EditorFontData : Inherits ConfigurationSection


    Public Sub New()
    End Sub

    <ConfigurationProperty("name")> _
    Public Property Name() As String
        Get
            Return Me.Item("name")
        End Get
        Set(ByVal Value As String)
            Me.Item("name") = Value
        End Set
    End Property

    <ConfigurationProperty("size")> _
    Public Property Size() As Double
        Get
            Return Me.Item("size")
        End Get
        Set(ByVal Value As Double)
            Me.Item("size") = Value
        End Set
    End Property

    <ConfigurationProperty("style")> _
    Public Property Style() As Integer
        Get
            Return Me.Item("style")
        End Get
        Set(ByVal Value As Integer)
            Me.Item("style") = Value
        End Set
    End Property

    Public Overrides Function ToString() As String
        Dim sb As StringBuilder = New StringBuilder
        sb.AppendFormat("Name = {0}; Size = {1}; Style = {2}", Name, Size.ToString(), Style.ToString())

        Return sb.ToString()
    End Function
End Class
