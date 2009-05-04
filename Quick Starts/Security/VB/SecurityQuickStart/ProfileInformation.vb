'===============================================================================
' Microsoft patterns & practices Enterprise Library
' Security Application Block QuickStart
'===============================================================================
' Copyright © Microsoft Corporation.  All rights reserved.
' THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
' OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
' LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
' FITNESS FOR A PARTICULAR PURPOSE.
'===============================================================================


Public Enum ProfileTheme
    Spring
    Summer
    Fall
    Winter
End Enum

' Class to store common profile information.
Public Class ProfileInformation
    Private userFirstName As String
    Private userLastName As String
    Private preferredTheme As ProfileTheme

    Public Sub New()
    End Sub

    Public Sub New(ByVal firstName As String, ByVal lastName As String, ByVal theme As ProfileTheme)
        Me.userFirstName = firstName
        Me.userLastName = lastName
        Me.preferredTheme = theme
    End Sub

    ' First name for the user.
    Public Property FirstName() As String
        Get
            Return Me.userFirstName
        End Get
        Set(ByVal Value As String)
            Me.userFirstName = Value
        End Set
    End Property

    ' Last name for the user.
    Public Property LastName() As String
        Get
            Return Me.userLastName
        End Get
        Set(ByVal Value As String)
            Me.userLastName = Value
        End Set
    End Property

    ' Preferred theme for a user.
    Public Property Theme() As ProfileTheme
        Get
            Return Me.preferredTheme
        End Get
        Set(ByVal Value As ProfileTheme)
            Me.preferredTheme = Value
        End Set
    End Property

    Public Overrides Function ToString() As String
        Dim result As String = String.Format(My.Resources.ProfileStringMessage, Me.userFirstName, Me.userLastName, Me.preferredTheme)
        Return result
    End Function
End Class
