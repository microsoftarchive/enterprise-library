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

Imports System.Web


Module RoleProviderExtensions

    <Extension()> _
    Public Sub AddUsersToRole(ByVal provider As RoleProvider, ByVal usernames As String(), ByVal role As String)
        provider.AddUsersToRoles(usernames, New String() {role})
    End Sub

    <Extension()> _
    Public Sub RemoveUserFromRole(ByVal provider As RoleProvider, ByVal username As String, ByVal roleName As String)
        provider.RemoveUsersFromRoles(New String() {username}, New String() {roleName})
    End Sub
End Module
