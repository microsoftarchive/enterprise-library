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


Public Class DivisibleByValidatorAttribute : Inherits ValidatorAttribute

    Private _divisor As Integer

    Public Sub New(ByVal divisor As Integer)
        _divisor = divisor
        End sub

    Protected Overrides Function DoCreateValidator(ByVal targetType As Type) As Validator
        Return New DivisibleByValidator(_divisor)
    End Function

End Class
