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
Imports ValidationQuickStart.CustomValidators


Public Class Address

    Private _line1 As String
    Private _line2 As String
    Private _city As String
    Private _state As String
    Private _postCode As String
    Private _country As String

    <StringLengthValidator(5, 50, Ruleset:="RuleSetA")> _
    Public Property Line1() As String
        Get
            Return _line1
        End Get
        Set(ByVal value As String)
            _line1 = value
        End Set
    End Property


    <IgnoreNulls()> _
    <ValidatorComposition(CompositionType.Or, Ruleset:="RuleSetA", MessageTemplate:="Address line 2 must be empty, or between 5 and 50 characters")> _
    <StringLengthValidator(0, Ruleset:="RuleSetA")> _
    <StringLengthValidator(5, 50, Ruleset:="RuleSetA")> _
    Public Property Line2() As String
        Get
            Return _line2
        End Get
        Set(ByVal value As String)
            _line2 = value
        End Set
    End Property

    <StringLengthValidator(1, 50, Ruleset:="RuleSetA")> _
    Public Property City() As String
        Get
            Return _city
        End Get
        Set(ByVal value As String)
            _city = value
        End Set
    End Property


    <USStateValidator(Ruleset:="RuleSetA")> _
    Public Property State() As String
        Get
            Return _state
        End Get
        Set(ByVal value As String)
            _state = value
        End Set
    End Property

    <RegexValidator("\d{5}(-\d{4})?", MessageTemplate:="""{0}"" is not a valid US zip code", Ruleset:="RuleSetA")> _
    Public Property PostCode() As String
        Get
            Return _postCode
        End Get
        Set(ByVal value As String)
            _postCode = value
        End Set
    End Property


    <DomainValidator("US", "USA", "United States", "United States of America", Ruleset:="RuleSetA", MessageTemplate:="Country must be USA")> _
    Public Property Country() As String
        Get
            Return _country
        End Get
        Set(ByVal value As String)
            _country = value
        End Set
    End Property


End Class
