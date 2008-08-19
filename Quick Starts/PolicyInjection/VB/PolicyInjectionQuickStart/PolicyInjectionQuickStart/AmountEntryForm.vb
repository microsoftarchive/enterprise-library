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

Friend Class AmountEntryForm

    Private Const promptString As String = "Please enter the amount that you wish to {0}:"
    Private _amount As Decimal


    Public Sub New(ByVal dialogType As AmountDialogType)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        If dialogType = AmountDialogType.Deposit Then
            promptLabel.Text = String.Format(promptString, "deposit")
            Me.Text = "Deposit"
        ElseIf dialogType = AmountDialogType.Withdraw Then
            promptLabel.Text = String.Format(promptString, "withdraw")
            Me.Text = "Withdraw"
        End If
    End Sub

    Public ReadOnly Property Amount() As Decimal
        Get
            Return _amount
        End Get
    End Property


    Private Sub okButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles okButton.Click
        Dim success As Boolean = Decimal.TryParse(amountTextBox.Text, _amount)
        If Not success Then
            errorProvider.SetError(amountTextBox, "Please enter a valid decimal amount.")
        Else
            Me.DialogResult = DialogResult.OK
            Me.Close()
        End If
    End Sub

End Class

Friend Enum AmountDialogType
    Deposit
    Withdraw
End Enum