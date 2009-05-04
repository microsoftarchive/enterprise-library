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

Imports System.Runtime.CompilerServices
Imports System.Web.Security

Module MembershipProviderExtensions
    <Extension()> _
    Public Function CreateUser(ByVal provider As MembershipProvider, ByVal username As String, ByVal password As String) As MembershipUser
        Dim status As MembershipCreateStatus
        Dim user As MembershipUser = Membership.CreateUser(username, password, Nothing, Nothing, Nothing, True, status)
        If (user Is Nothing) Then
            Throw New MembershipCreateUserException(status)
        End If
        Return user
    End Function

    <Extension()> _
    Public Function DeleteUser(ByVal provider As MembershipProvider, ByVal username As String) As Boolean
        Return provider.DeleteUser(username, True)
    End Function

    <Extension()> _
    Public Function GetAllUsers(ByVal provider As MembershipProvider) As MembershipUserCollection
        Dim totalRecords As Integer = 0
        Return provider.GetAllUsers(0, Integer.MaxValue, totalRecords)
    End Function
End Module
