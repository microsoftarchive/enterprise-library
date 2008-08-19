'===============================================================================
' Microsoft patterns & practices Enterprise Library
' Policy Injection Application Block QuickStart
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
Imports Microsoft.Practices.EnterpriseLibrary.PolicyInjection
Imports Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers

Namespace BusinessLogic

    Public Class BankAccount : Inherits MarshalByRefObject

        Private balance As Decimal

        Public Function GetCurrentBalance() As Decimal
            Return balance
        End Function

        <ValidationCallHandler()> _
        Public Sub Deposit(<RangeValidator(GetType(Decimal), "0.0", RangeBoundaryType.Exclusive, "0.0", RangeBoundaryType.Ignore, MessageTemplate:="Deposited amount must be more than zero.")> ByVal _
             depositAmount As Decimal)
            balance += depositAmount
        End Sub

        <ValidationCallHandler()> _
        Public Sub Withdraw(<RangeValidator(GetType(Decimal), "0.0", RangeBoundaryType.Exclusive, "1000.0", RangeBoundaryType.Inclusive, MessageTemplate:="Withdrawal amount must be between zero and 1000.")> ByVal _
             withdrawAmount As Decimal)

            If (withdrawAmount > balance) Then
                Throw New ArithmeticException()
            End If
            balance -= withdrawAmount
        End Sub
    End Class

End Namespace