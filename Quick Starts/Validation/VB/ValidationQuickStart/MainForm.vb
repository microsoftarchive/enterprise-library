'===============================================================================
' Microsoft patterns & practices Enterprise Library
' Validation Application Block QuickStart
'===============================================================================
' Copyright ? Microsoft Corporation.  All rights reserved.
' THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
' OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
' LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
' FITNESS FOR A PARTICULAR PURPOSE.
'===============================================================================

Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports Microsoft.Practices.EnterpriseLibrary.Common.Configuration
Imports Microsoft.Practices.EnterpriseLibrary.Validation
Imports Microsoft.Practices.EnterpriseLibrary.Validation.Validators
Imports ValidationQuickStart.BusinessEntities


Partial Public Class MainForm : Inherits Form

    Private viewerProcess As Process
    Private Const HelpViewerArguments As String = "/helpcol ms-help://MS.VSCC.v90/MS.VSIPCC.v90/ms.practices.entlib.2008may /LaunchFKeywordTopic ValidationQS1"

    Public Sub New()

        InitializeComponent()
        customerRuleSetCombo.SelectedIndex = 0
    End Sub

    Private Function CreateCustomer() As Customer

        ' Build the customer object
        Dim customer As New Customer()
        customer.FirstName = firstNameTextBox.Text
        customer.LastName = lastNameTextBox.Text
        customer.DateOfBirth = birthDatePicker.Value
        customer.Email = emailTextBox.Text
        customer.Address = CreateAddress()
        Dim rewardPoints As Integer
        Dim success As Boolean = Int32.TryParse(rewardPointTextBox.Text, rewardPoints)
        If success Then
            customer.RewardPoints = rewardPoints
        Else
            MessageBox.Show("Rewards Points must be a valid integer." & Environment.NewLine & "Note that the Validation Application Block cannot validate data if it is incompatible with the object's type system.", _
                    Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return Nothing
        End If
        Return customer
    End Function


    Private Function CreateAddress() As Address
        Dim address As New Address()
        address.Line1 = line1TextBox.Text
        address.Line2 = line2TextBox.Text
        address.City = cityTextBox.Text
        address.PostCode = postCodeTextBox.Text
        address.State = stateTextBox.Text
        address.Country = countryTextBox.Text
        Return address
    End Function


    Private Sub validateCustomerButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles validateCustomerButton.Click
        Dim customer As Customer = CreateCustomer()
        If Not customer Is Nothing Then
            Dim validator As Validator(Of Customer) = ValidationFactory.CreateValidator(Of Customer)(customerRuleSetCombo.Text)
            Dim results As ValidationResults = validator.Validate(customer)
            DisplayValidationResults(results, customerResultsTreeView)
        End If
    End Sub



    Private Sub DisplayValidationResults(ByVal results As ValidationResults, ByVal resultsTreeView As TreeView)

        resultsTreeView.Nodes.Clear()
        For Each result As ValidationResult In results
            Dim node As TreeNode = New TreeNode(result.Message)
            node.Nodes.Add(String.Format("Key = ""{0}""", result.Key.ToString()))
            node.Nodes.Add(String.Format("Tag = ""{0}""", result.Tag))
            node.Nodes.Add(String.Format("Target = ""{0}""", result.Target.ToString()))
            node.Nodes.Add(String.Format("Validator = ""{0}""", result.Validator.ToString()))
            resultsTreeView.Nodes.Add(node)
        Next
    End Sub


    Private Sub exitButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles exitButton.Click
        Application.Exit()
    End Sub



    Private Sub customerRuleSetCombo_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles customerRuleSetCombo.TextChanged
        ' Assume that both classes use the same ruleset, for the purposes of the Winforms integration
        customerValidationProvider.RulesetName = customerRuleSetCombo.Text
        addressValidationProvider.RulesetName = customerRuleSetCombo.Text

        If enableWinFormsValidationCheckBox.Checked Then
            ' Force validation of all controls
            ValidateChildren()
        End If

    End Sub

    Private Sub enableWinFormsValidationCheckBox_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles enableWinFormsValidationCheckBox.CheckedChanged
        customerValidationProvider.Enabled = enableWinFormsValidationCheckBox.Checked
        addressValidationProvider.Enabled = enableWinFormsValidationCheckBox.Checked
        If enableWinFormsValidationCheckBox.Checked Then

            ' Force validation of all controls
            ValidateChildren()
        End If
    End Sub


    Private Sub customerValidationProvider_ValueConvert(ByVal sender As System.Object, ByVal e As Microsoft.Practices.EnterpriseLibrary.Validation.Integration.ValueConvertEventArgs) Handles customerValidationProvider.ValueConvert
        ' Get the value of the RewardPoints control and convert to an integer
        If e.SourcePropertyName = "RewardPoints" Then

            Dim originalValue As String = CType(e.ValueToConvert, String)
            Dim convertedValue As Integer
            Dim success As Boolean = Int32.TryParse(originalValue, convertedValue)
            If success Then
                e.ConvertedValue = convertedValue
            Else
                e.ConversionErrorMessage = "Reward Points must be a valid integer"
            End If
        End If
    End Sub

    Private Function GetHelpViewerExecutable() As String
        Dim common As String = Environment.GetEnvironmentVariable("CommonProgramFiles")
        Return Path.Combine(common, "Microsoft Shared\Help 8\dexplore.exe")
    End Function

    Private Sub viewWalkthroughButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles viewWalkthroughButton.Click
        ' Process has never been started. Initialize and launch the viewer.
        If (Me.viewerProcess Is Nothing) Then

            ' Initialize the Process information for the help viewer
            Me.viewerProcess = New Process

            Me.viewerProcess.StartInfo.FileName = GetHelpViewerExecutable()
            Me.viewerProcess.StartInfo.Arguments = HelpViewerArguments
            Me.viewerProcess.Start()

        ElseIf (Me.viewerProcess.HasExited) Then

            ' Process previously started, then exited. Start the process again.
            Me.viewerProcess.Start()
        Else
            ' Process was already started - bring it to the foreground
            Dim hWnd As IntPtr = Me.viewerProcess.MainWindowHandle
            If (NativeMethods.IsIconic(hWnd)) Then
                NativeMethods.ShowWindowAsync(hWnd, NativeMethods.SW_RESTORE)
            End If
            NativeMethods.SetForegroundWindow(hWnd)
        End If
    End Sub
End Class
