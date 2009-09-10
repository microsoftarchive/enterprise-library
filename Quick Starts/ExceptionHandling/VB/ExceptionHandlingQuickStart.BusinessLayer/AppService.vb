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
Imports Microsoft.Practices.EnterpriseLibrary.Common.Configuration

Public Class AppService
    Private _exceptionManager As ExceptionManager

    Public Sub New()
        _exceptionManager = EnterpriseLibraryContainer.Current.GetInstance(Of ExceptionManager)()
    End Sub

    Public Sub ProcessA()
        Throw New System.Exception("Original Exception: Fatal exception in business layer")
    End Sub

    Public Sub ProcessB()
        Throw New System.Data.DBConcurrencyException("Original Exception: Concurrency problem in business layer")
    End Sub

    Private Sub ProcessC()

        ' Assume operation failed due to authentication. Get current
        ' user's identity information.

        'Get the current identity and put it into an identity object.
        Dim myIdentity As WindowsIdentity = WindowsIdentity.GetCurrent()

        ' Put the previous identity into a principal object.
        Dim myPrincipal As WindowsPrincipal = New WindowsPrincipal(myIdentity)

        ' Principal values.
        Dim principalName As String = myPrincipal.Identity.Name
        Dim principalType As String = myPrincipal.Identity.AuthenticationType
        Dim principalAuth As String = myPrincipal.Identity.IsAuthenticated.ToString()

        'Identity values.
        Dim identName As String = myIdentity.Name
        Dim identType As String = myIdentity.AuthenticationType
        Dim identToken As String = myIdentity.Token.ToString()

        ' Print the values.
        Dim identityInfo As String = String.Format("Principal Values for current thread:" + _
         "\n\nPrincipal Name: {0}" + _
         "Principal Type: {1}" + _
         "Principal IsAuthenticated: {2}" + _
         "\n\nIdentity Values for current thread:" + _
         "Identity Name: {3}" + _
         "Identity Type: {4}" + _
         "Identity Token: {5}", _
         principalName, principalType, principalAuth, _
         identName, identType, identToken)

        Throw New System.Security.SecurityException(identityInfo)
    End Sub

    Public Sub ProcessD()
        Throw New BusinessLayerException("Original Exception: Problem in business layer")
    End Sub

    ' Demonstrates handling of exceptions coming out of a layer. The policy
    ' demonstrated here will show how original exceptions can be propagated back out.
    Public Function ProcessWithPropagate() As Boolean
        ' Quick Start is configured so that the Propagate Policy will
        ' log the exception and then recommend a rethrow.

        _exceptionManager.Process(AddressOf ProcessA, "Propagate Policy")
        Return True
    End Function

    ' Demonstrates handling of exceptions coming out of a layer. The policy
    ' demonstrated here will show how original exceptions can be wrapped
    ' with a different exception before being propagated back out.
    Public Function ProcessWithWrap() As Boolean
        ' Quick Start is configured so that the Bubble Policy will
        ' log the exception and then recommend a rethrow.

        _exceptionManager.Process(AddressOf ProcessB, "Wrap Policy")
        Return True
    End Function

    Public Sub ProcessWithReplace()

        ' Invoke our policy that is responsible for making sure no secure information
        ' gets out of our layer.

        _exceptionManager.Process(AddressOf ProcessC, "Replace Policy")
    End Sub

    Public Sub ProcessAndResume()
        ' Invoke our policy that is responsible for making sure no secure information
        ' gets out of our layer.

        _exceptionManager.Process(AddressOf ProcessC, "Handle and Resume Policy")
    End Sub

    Public Sub ProcessAndNotify()
        ' Invoke our policy that is responsible for making sure no secure information
        ' gets out of our layer.

        _exceptionManager.Process(AddressOf ProcessD, "Notify Policy")
    End Sub
End Class
