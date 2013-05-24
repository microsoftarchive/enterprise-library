
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SimulatedWindowsAuthentication.aspx.cs" Inherits="AExpense.SimulatedWindowsAuthentication" %>
<%@ OutputCache Location="None" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1">
    <meta http-equiv="content-type" content="text/html; charset=UTF-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <title>Simulated windows authentication</title>
    <link href="styling/styles/login.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <div id="page">
        <div id="login">
            <form id="form1" runat="server">
                This page simulates the Windows Integrated Authentication login. Please select a User to continue:
                <div id="UserOptions">
                    <asp:RadioButtonList ID="UserList" runat="server">
                        <asp:ListItem Text="ADATUM\johndoe (Role:'Employee')" Value="ADATUM\johndoe" Selected="True" />
                        <asp:ListItem Text="ADATUM\mary (Roles:'Employee' & 'Manager')" Value="ADATUM\mary" />
                    </asp:RadioButtonList>
                </div>
                <asp:Button ID="ContinueButton" runat="server" class="tooltip" Text="Continue with login..." OnClick="ContinueButtonClick"  />
            </form>
        </div>
        <div id="windows-auth-popup">
            If Windows Authentication is configured, this popup will be displayed to log you in.<br />
           <img src="Styling/images/windows_auth_popup.png" alt="Windows integrated authentication popup" />
        </div>
    </div>
</body>
</html>
