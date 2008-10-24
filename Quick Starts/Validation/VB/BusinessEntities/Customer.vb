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
Imports Microsoft.Practices.EnterpriseLibrary.Validation.Validators
Imports Microsoft.Practices.EnterpriseLibrary
Imports Microsoft.Practices.EnterpriseLibrary.Validation


Public Class Customer

    Private _firstName As String
    Private _lastName As String
    Private _dateOfBirth As DateTime
    Private _email As String
    Private _address As Address
    Private _rewardPoints As Integer

    <StringLengthValidator(1, 50, Ruleset:="RuleSetA", MessageTemplate:="First Name must be between 1 and 50 characters")> _
    Public Property FirstName() As String
        Get
            Return _firstName
        End Get
        Set(ByVal value As String)
            _firstName = value
        End Set
    End Property
 

    <StringLengthValidator(1, 50, Ruleset:="RuleSetA", MessageTemplate:="Last Name must be between 1 and 50 characters")> _
    Public Property LastName() As String
        Get
            Return _lastName
        End Get
        Set(ByVal value As String)
            _lastName = value
        End Set
    End Property


    <RelativeDateTimeValidator(-120, DateTimeUnit.Year, -18, DateTimeUnit.Year, Ruleset:="RuleSetA", MessageTemplate:="Must be 18 years or older.")> _
    Public Property DateOfBirth() As DateTime
        Get
            Return _dateOfBirth
        End Get
        Set(ByVal value As DateTime)
            _dateOfBirth = value
        End Set
    End Property


    <RegexValidator("\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", MessageTemplate:="Invalid e-mail address", Ruleset:="RuleSetA")> _
    Public Property Email() As String
        Get
            Return _email
        End Get
        Set(ByVal value As String)
            _email = value
        End Set
    End Property


    <ObjectValidator("RuleSetA", Ruleset:="RuleSetA")> _
    Public Property Address() As Address
        Get
            Return _address
        End Get
        Set(ByVal value As Address)
            _address = value
        End Set
    End Property

    <RangeValidator(0, RangeBoundaryType.Inclusive, 1000000, RangeBoundaryType.Inclusive, Ruleset:="RuleSetA", MessageTemplate:="Rewards points cannot exceed 1,000,000")> _
    Public Property RewardPoints() As Integer
        Get
            Return _rewardPoints
        End Get
        Set(ByVal value As Integer)
            _rewardPoints = value
        End Set
    End Property

End Class


