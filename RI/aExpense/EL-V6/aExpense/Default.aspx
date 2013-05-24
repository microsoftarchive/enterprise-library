
<%@ Page Title="" Language="C#" MasterPageFile="~/Styling/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="AExpense.Default" %>

<asp:Content ID="Content2" ContentPlaceholderID="ContentPlaceholder" runat="server">
    <div id="expenselist">
        <asp:GridView ID="MyExpensesGridView" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" OnRowCommand="MyExpensesGridViewOnRowCommand" EnableViewState="true" OnRowDataBound="MyExpensesGridViewOnRowDataBound">
            <Columns>
                <asp:BoundField DataField="Date" HeaderText="Date" SortExpression="Date" DataFormatString="{0:MM/dd/yyyy}" />
                <asp:BoundField DataField="Title" HeaderText="Title" SortExpression="Title" />
                <asp:BoundField DataField="Total" HeaderText="Amount" SortExpression="Amount" DataFormatString="{0:c}" />
                <asp:BoundField DataField="CostCenter" HeaderText="Cost Center" SortExpression="CostCenter" />
                <asp:BoundField DataField="ReimbursementMethod" HeaderText="Reimbursement Method"
                    SortExpression="ReimbursementMethod" />
                <asp:TemplateField HeaderText="Status" SortExpression="Approved"  ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate><%# (Boolean.Parse(Eval("Approved").ToString())) ? "Ready for Processing" : "Pending for Approval"%></ItemTemplate>
                </asp:TemplateField>
                <asp:CommandField ShowSelectButton="true" SelectText="»" HeaderStyle-Width="25px"
                    CausesValidation="false" ItemStyle-HorizontalAlign="Center" />
            </Columns>
            <HeaderStyle BackColor="#e6e6e6" />
            <EmptyDataTemplate>
                There are no expenses registered for <i>
                    <%= this.User.Identity.Name %></i>.
            </EmptyDataTemplate>
        </asp:GridView>
    </div>
</asp:Content>
