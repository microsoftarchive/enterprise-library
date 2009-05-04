<%@ Page Language="VB" AutoEventWireup="true"  CodeFile="Default.aspx.vb" Inherits="_Default" %>

<%@ Register Assembly="Microsoft.Practices.EnterpriseLibrary.Validation.Integration.AspNet"
    Namespace="Microsoft.Practices.EnterpriseLibrary.Validation.Integration.AspNet"
    TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Validation ASP.NET QuickStart</title>
    <style type="text/css" >
    body {
         font-family: Calibri, Verdana, sans-serif;
         }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h1>
            Validation Application Block: ASP.NET Integration QuickStart</h1>
        
            <table>
                <tr>
                    <td colspan="2">
                        <h2>
                            Customer</h2>
                    </td>
                </tr>
                <tr>
                    <td style="width: 142px">
                        First Name:</td>
                    <td style="width: 508px">
                        <asp:TextBox ID="firstNameTextBox" runat="server" Width="235px"></asp:TextBox>&nbsp;
                        <br />
                        <cc1:propertyproxyvalidator id="firstNameValidator" runat="server" ControlToValidate="firstNameTextBox" PropertyName="FirstName" RulesetName="RuleSetA" SourceTypeName="ValidationQuickStart.BusinessEntities.Customer"></cc1:propertyproxyvalidator>
                    </td>
                </tr>
                <tr>
                    <td style="width: 142px; height: 21px">
                        Last Name:</td>
                    <td style="width: 508px; height: 21px">
                        <asp:TextBox ID="lastNameTextBox" runat="server" Width="235px"></asp:TextBox><br />
                        <cc1:PropertyProxyValidator ID="lastNameValidator" runat="server" ControlToValidate="lastNameTextBox"
                            PropertyName="LastName" RulesetName="RuleSetA" SourceTypeName="ValidationQuickStart.BusinessEntities.Customer"></cc1:PropertyProxyValidator></td>
                </tr>
                <tr>
                    <td style="width: 142px">
                        Date Of Birth:</td>
                    <td style="width: 508px">
                        <asp:TextBox ID="dateOfBirthTextBox" runat="server"></asp:TextBox><br />
                        <cc1:PropertyProxyValidator ID="dateOfBirthValidator" runat="server" ControlToValidate="dateOfBirthTextBox"
                            OnValueConvert="dateOfBirthValidator_ValueConvert" PropertyName="DateOfBirth"
                            RulesetName="RuleSetA" SourceTypeName="ValidationQuickStart.BusinessEntities.Customer"></cc1:PropertyProxyValidator></td>
                </tr>
                <tr>
                    <td style="width: 142px">
                        E-mail:</td>
                    <td style="width: 508px">
                        <asp:TextBox ID="emailTextBox" runat="server" Width="235px"></asp:TextBox><br />
                        <cc1:PropertyProxyValidator ID="emailValidator" runat="server" ControlToValidate="emailTextBox"
                            PropertyName="Email" RulesetName="RuleSetA" SourceTypeName="ValidationQuickStart.BusinessEntities.Customer"></cc1:PropertyProxyValidator></td>
                </tr>
                <tr>
                    <td style="width: 142px; height: 25px;">
                        Rewards Points:</td>
                    <td style="width: 508px; height: 25px;">
                        <asp:TextBox ID="rewardsPointsTextBox" runat="server"></asp:TextBox><br />
                        <cc1:PropertyProxyValidator ID="rewardPointsValidator" runat="server" ControlToValidate="rewardsPointsTextBox"
                             PropertyName="RewardPoints"
                            RulesetName="RuleSetA" SourceTypeName="ValidationQuickStart.BusinessEntities.Customer" OnValueConvert="rewardsPointsValidator_ValueConvert"></cc1:PropertyProxyValidator></td>
                </tr>
                <tr>
                    <td colspan="2">
                        <h3>
                            Address</h3>
                    </td>
                </tr>
                <tr>
                    <td style="width: 142px">
                        Line 1:</td>
                    <td style="width: 508px">
                        <asp:TextBox ID="line1TextBox" runat="server" Width="235px"></asp:TextBox><br />
                        <cc1:PropertyProxyValidator ID="line1Validator" runat="server" ControlToValidate="line1TextBox"
                            PropertyName="Line1" RulesetName="RuleSetA"
                            SourceTypeName="ValidationQuickStart.BusinessEntities.Address"></cc1:PropertyProxyValidator></td>
                </tr>
                <tr>
                    <td style="width: 142px">
                        Line 2:</td>
                    <td style="width: 508px">
                        <asp:TextBox ID="line2TextBox" runat="server" Width="235px"></asp:TextBox><br />
                        <cc1:PropertyProxyValidator ID="line2Validator" runat="server" ControlToValidate="line2TextBox"
                            PropertyName="Line2" RulesetName="RuleSetA"
                            SourceTypeName="ValidationQuickStart.BusinessEntities.Address"></cc1:PropertyProxyValidator></td>
                </tr>
                <tr>
                    <td style="width: 142px">
                        City:</td>
                    <td style="width: 508px">
                        <asp:TextBox ID="cityTextBox" runat="server" Width="235px"></asp:TextBox><br />
                        <cc1:PropertyProxyValidator ID="cityValidator" runat="server" ControlToValidate="cityTextBox"
                            PropertyName="City" RulesetName="RuleSetA" SourceTypeName="ValidationQuickStart.BusinessEntities.Address"></cc1:PropertyProxyValidator></td>
                </tr>
                <tr>
                    <td style="width: 142px">
                        State:</td>
                    <td style="width: 508px">
                        <asp:TextBox ID="stateTextBox" runat="server" Width="76px"></asp:TextBox><br />
                        <cc1:PropertyProxyValidator
                            ID="stateValidator" runat="server" ControlToValidate="stateTextBox" PropertyName="State"
                            RulesetName="RuleSetA" SourceTypeName="ValidationQuickStart.BusinessEntities.Address"></cc1:PropertyProxyValidator></td>
                </tr>
                <tr>
                    <td style="width: 142px">
                        Post Code:</td>
                    <td style="width: 508px">
                        <asp:TextBox ID="postCodeTextBox" runat="server" Width="76px"></asp:TextBox><br />
                        <cc1:PropertyProxyValidator
                            ID="postCodeValidator" runat="server" ControlToValidate="postCodeTextBox" PropertyName="PostCode"
                            RulesetName="RuleSetA" SourceTypeName="ValidationQuickStart.BusinessEntities.Address"></cc1:PropertyProxyValidator></td>
                </tr>
                <tr>
                    <td style="width: 142px">
                        Country:</td>
                    <td style="width: 508px">
                        <asp:TextBox ID="countryTextBox" runat="server" Width="235px"></asp:TextBox><br />
                        <cc1:PropertyProxyValidator ID="countryValidator" runat="server" ControlToValidate="countryTextBox"
                            PropertyName="Country" RulesetName="RuleSetA" SourceTypeName="ValidationQuickStart.BusinessEntities.Address"></cc1:PropertyProxyValidator></td>
                </tr>
                <tr>
                    <td style="width: 142px">
                    </td>
                    <td style="width: 508px">
                        <asp:Button ID="submitButton" runat="server" Text="Submit" OnClick="submitButton_Click" />&nbsp;
                        <asp:DropDownList ID="ruleSetDropDown" runat="server">
                            <asp:ListItem Selected="True">RuleSetA</asp:ListItem>
                            <asp:ListItem>RuleSetB</asp:ListItem>
                        </asp:DropDownList>
                        <asp:Label ID="validationResultsLabel" runat="server" Font-Bold="True"></asp:Label></td>
                </tr>
            </table>
    </div>
    </form>
</body>
</html>
