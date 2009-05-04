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
Imports Microsoft.Practices.EnterpriseLibrary.Common.Configuration
Imports Microsoft.Practices.EnterpriseLibrary.Validation.Configuration


<ConfigurationElementType(GetType(CustomValidatorData))> _
Public Class USStateValidator : Inherits DomainValidator(Of String)
    Private Shared stateCodes As List(Of String) = New List(Of String)(New String() { _
            "AL", "AK", "AS", "AZ", "AR", "CA", "CO", "CT", "DE", "DC", "FM", "FL", _
            "GA", "GU", "HI", "ID", "IL", "IN", "IA", "KS", "KY", "LA", "ME", "MH", _
            "MD", "MA", "MI", "MN", "MS", "MO", "MT", "NE", "NV", "NH", "NJ", "NM", _
            "NY", "NC", "ND", "MP", "OH", "OK", "OR", "PW", "PA", "PR", "RI", "SC", _
            "SD", "TN", "TX", "UT", "VT", "VI", "VA", "WA", "WV", "WI", "WY"})

    Public Sub New()
        MyBase.New(stateCodes)
    End Sub

    Protected Overrides ReadOnly Property DefaultNonNegatedMessageTemplate() As String
        Get
            Return "The supplied code does not represent a valid US State"
        End Get
    End Property

End Class
