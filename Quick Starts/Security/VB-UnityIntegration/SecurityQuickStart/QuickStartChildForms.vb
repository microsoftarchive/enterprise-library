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

Imports Microsoft.Practices.Unity

Public Class QuickStartChildForms

    Private myAddUserRoleForm As AddUserRoleForm
    Private myCredentialsForm As CredentialsForm
    Private myProfileForm As ProfileForm
    Private myRoleAuthorizationForm As RoleAuthorizationForm
    Private myUserRoleForm As UserRoleForm
    Private myUsersForm As UsersForm

    <Dependency()> _
    Public Property AddUserRoleForm() As AddUserRoleForm
        Get
            Return myAddUserRoleForm
        End Get
        Set(ByVal value As AddUserRoleForm)
            myAddUserRoleForm = value
        End Set
    End Property

    <Dependency()> _
    Public Property CredentialsForm() As CredentialsForm
        Get
            Return myCredentialsForm
        End Get
        Set(ByVal value As CredentialsForm)
            myCredentialsForm = value
        End Set
    End Property

    <Dependency()> _
    Public Property ProfileForm() As ProfileForm
        Get
            Return myProfileForm
        End Get
        Set(ByVal value As ProfileForm)
            myProfileForm = value
        End Set
    End Property

    <Dependency()> _
    Public Property RoleAuthorizationForm() As RoleAuthorizationForm
        Get
            Return myRoleAuthorizationForm
        End Get
        Set(ByVal value As RoleAuthorizationForm)
            myRoleAuthorizationForm = value
        End Set
    End Property

    <Dependency()> _
    Public Property UserRoleForm() As UserRoleForm
        Get
            Return myUserRoleForm
        End Get
        Set(ByVal value As UserRoleForm)
            myUserRoleForm = value
        End Set
    End Property

    <Dependency()> _
    Public Property UsersForm() As UsersForm
        Get
            Return myUsersForm
        End Get
        Set(ByVal value As UsersForm)
            myUsersForm = value
        End Set
    End Property

End Class
