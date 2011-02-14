<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ValueAccessValueConvert.aspx.cs"
    Inherits="_Default" %>

<%@ Register Namespace="Microsoft.Practices.EnterpriseLibrary.Validation.Integration.AspNet"
    TagPrefix="entlibvalidation" Assembly="Microsoft.Practices.EnterpriseLibrary.Validation.Integration.AspNet" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:TextBox ID="NumberProperty" runat="server" Text="012345" />
            <entlibvalidation:PropertyProxyValidator ID="NumberPropertyValidator" runat="server" SourceTypeName="DomainModel.DomainObject"
                PropertyName="NumberProperty" ControlToValidate="NumberProperty" SpecificationSource="Attributes"
                RulesetName="" />
            <br />
            <asp:Button ID="Button1" runat="server" Text="Button" />
            <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetDomainObjects"
                TypeName="DomainModel.DomainObjects"></asp:ObjectDataSource>
        </div>
    </form>
</body>
</html>
