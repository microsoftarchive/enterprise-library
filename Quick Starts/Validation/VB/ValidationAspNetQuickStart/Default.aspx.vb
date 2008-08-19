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
Imports System.Data
Imports System.Configuration
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls
Imports Microsoft.Practices.EnterpriseLibrary.Validation.Integration.AspNet

Partial Public Class _Default : Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Set the ruleset for every validator control
        For Each v As IValidator In Me.GetValidators(Nothing)
            If TypeOf v Is PropertyProxyValidator Then
                Dim validator As PropertyProxyValidator = CType(v, PropertyProxyValidator)
                validator.RulesetName = ruleSetDropDown.Text
            End If
        Next
    End Sub


    Protected Sub submitButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles submitButton.Click
        If Page.IsValid Then
            validationResultsLabel.Text = "Data is valid."
            ' Now you would create the objects and process them
        Else

            validationResultsLabel.Text = "Data is invalid."
        End If
    End Sub


    Protected Sub rewardsPointsValidator_ValueConvert(ByVal sender As Object, ByVal e As Microsoft.Practices.EnterpriseLibrary.Validation.Integration.ValueConvertEventArgs) Handles rewardPointsValidator.ValueConvert
        Dim stringValue As String = CStr(e.ValueToConvert)
        Dim intValue As Integer

        Dim success As Boolean = Int32.TryParse(stringValue, intValue)
        If success Then

            e.ConvertedValue = intValue
        Else

            e.ConversionErrorMessage = "Rewards points is not a valid integer"
            e.ConvertedValue = Nothing
        End If
    End Sub


    Protected Sub dateOfBirthValidator_ValueConvert(ByVal sender As Object, ByVal e As Microsoft.Practices.EnterpriseLibrary.Validation.Integration.ValueConvertEventArgs) Handles dateOfBirthValidator.ValueConvert
        Dim stringValue As String = CStr(e.ValueToConvert)
        Dim dateValue As DateTime

        Dim success As Boolean = DateTime.TryParse(stringValue, dateValue)
        If success Then

            e.ConvertedValue = dateValue
        Else

            e.ConversionErrorMessage = "Date Of Birth is not in the correct format."
            e.ConvertedValue = Nothing
        End If
    End Sub

End Class
