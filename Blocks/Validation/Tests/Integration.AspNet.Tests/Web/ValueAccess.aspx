<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ValueAccess.aspx.cs" Inherits="_Default" %>

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
            <asp:TextBox ID="Name1" runat="server" Text="0123456789012345" />
            <entlibvalidation:PropertyProxyValidator ID="NameValidator1" runat="server" SourceTypeName="DomainModel.DomainObject"
                PropertyName="Name" ControlToValidate="Name1" SpecificationSource="Attributes"
                RulesetName="" />
            <br />
            <asp:TextBox ID="SurName1" runat="server" Text="LogicalOperationStack" />
            <entlibvalidation:PropertyProxyValidator ID="SurNameValidator1" runat="server" SourceTypeName="DomainModel.DomainObject"
                PropertyName="SurName" ControlToValidate="SurName1" SpecificationSource="Attributes"
                RulesetName="" />&nbsp;<br />
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataSourceID="ObjectDataSource1">
                <Columns>
                    <asp:TemplateField HeaderText="Name">
                        <ItemTemplate>
                            <asp:TextBox ID="Name" runat="server" Text='<%#Bind("Name")%>'></asp:TextBox>
                            <entlibvalidation:PropertyProxyValidator ID="Name_Validator" runat="server" SourceTypeName="DomainModel.DomainObject"
                                PropertyName="Name" ControlToValidate="Name" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="Name" runat="server" Text='<%#Bind("Name")%>'></asp:TextBox>
                            <entlibvalidation:PropertyProxyValidator ID="Name_Validator" runat="server" SourceTypeName="DomainModel.DomainObject"
                                PropertyName="Name" ControlToValidate="Name" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="SurName">
                        <ItemTemplate>
                            <asp:TextBox ID="SurName" runat="server" Text='<%#Bind("SurName")%>'></asp:TextBox>
                            <entlibvalidation:PropertyProxyValidator ID="SurName_Validator" runat="server" SourceTypeName="DomainModel.DomainObject"
                                PropertyName="SurName" ControlToValidate="SurName" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="SurName" runat="server" Text='<%#Bind("SurName")%>'></asp:TextBox>
                            <entlibvalidation:PropertyProxyValidator ID="SurName_Validator" runat="server" SourceTypeName="DomainModel.DomainObject"
                                PropertyName="SurName" ControlToValidate="SurName" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="NumberProperty">
                        <ItemTemplate>
                            <asp:TextBox ID="NumberProperty" runat="server" Text='<%#Bind("NumberProperty")%>'></asp:TextBox>
                            <entlibvalidation:PropertyProxyValidator ID="NumberProperty_Validator" runat="server" SourceTypeName="DomainModel.DomainObject"
                                PropertyName="NumberProperty" ControlToValidate="NumberProperty" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="NumberProperty" runat="server" Text='<%#Bind("NumberProperty")%>'></asp:TextBox>
                            <entlibvalidation:PropertyProxyValidator ID="NumberProperty_Validator" runat="server" SourceTypeName="DomainModel.DomainObject"
                                PropertyName="NumberProperty" ControlToValidate="NumberProperty" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <br />
            <asp:Button ID="Button1" runat="server" Text="Button" />
            <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetDomainObjects"
                TypeName="DomainModel.DomainObjects"></asp:ObjectDataSource>
        </div>
    </form>
</body>
</html>
