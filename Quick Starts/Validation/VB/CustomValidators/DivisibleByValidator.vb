'===============================================================================
' Microsoft patterns & practices Enterprise Library
' Validation Application Block QuickStart
'===============================================================================
' Copyright © Microsoft Corporation.  All rights reserved.
' THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
' OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
' LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
' FITNESS FOR A PARTICULAR PURPOSE.
'===============================================================================

Imports System
Imports System.Collections.Generic
Imports System.Text
Imports Microsoft.Practices.EnterpriseLibrary.Validation
Imports Microsoft.Practices.EnterpriseLibrary.Validation.Validators
Imports Microsoft.Practices.EnterpriseLibrary.Validation.Configuration
Imports Microsoft.Practices.EnterpriseLibrary.Common.Configuration
Imports System.Collections.Specialized

<ConfigurationElementType(GetType(CustomValidatorData))> _
Public Class DivisibleByValidator : Inherits Validator(Of Integer)

    Private _divisor As Integer


    Public Sub New(ByVal attributes As NameValueCollection)
        MyBase.New(Nothing, Nothing)
        _divisor = Int32.Parse(attributes.Get("Divisor"))
    End Sub

    Public Sub New(ByVal divisor As Integer)
        Me.New(divisor, Nothing, Nothing)
    End Sub

    Public Sub New(ByVal divisor As Integer, ByVal messageTemplate As String)
        Me.new(divisor, messageTemplate, Nothing)
    End Sub
  

    Public Sub New(ByVal divisor As Integer, ByVal messageTemplate As String, ByVal tag As String)
        MyBase.new(MessageTemplate, Tag)
        _divisor = divisor
    End Sub


    Protected Overrides Sub DoValidate(ByVal objectToValidate As Integer, ByVal currentTarget As Object, ByVal key As String, ByVal validationResults As ValidationResults)

        If (objectToValidate Mod _divisor <> 0) Then

            Dim message As String = String.Format(Me.MessageTemplate, objectToValidate, _divisor)
            LogValidationResult(validationResults, message, currentTarget, key)
        End If
    End Sub

    Protected Overrides ReadOnly Property DefaultMessageTemplate() As String
        Get
            Return "The value {0} is not divisible by {1}"
        End Get
    End Property
 
End Class
