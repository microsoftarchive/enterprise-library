
<%@ Page Language="C#" MasterPageFile="~/Styling/Site.Master" AutoEventWireup="true" CodeBehind="ExpenseDetails.aspx.cs" Inherits="AExpense.ExpenseDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceholder" runat="server">
    <link type="text/css" href="/Styling/styles/redmond/jquery-ui-1.8.custom.css" rel="stylesheet" />
    <script src="https://ajax.microsoft.com/ajax/jquery/jquery-1.4.2.js" type="text/javascript"></script>
    <script src="/Scripts/jquery-ui-1.8.custom.min.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceholder" runat="server">
   
    <div class="infoBox">
        <h2>
            View the Expense Details</h2>
        <p>
            Here you can see the details of the expense.
        </p>
    </div>
    <div id="expenseform">
         <p>
            <asp:Label ID="ExpenseDateLabel" runat="server" Text="Date:" AssociatedControlID="ExpenseDate" />
            <br />
            <asp:Label ID="ExpenseDate" runat="server" />
        </p>
        <p>
            <asp:Label ID="ExpenseTitleLabel" runat="server" Text="Title:" AssociatedControlID="ExpenseTitle" />
            <br />
            <asp:Label ID="ExpenseTitle" runat="server" />
        </p>
        <p>
            <asp:Label ID="ExpenseDetailsLabel" AssociatedControlID="ExpenseItemsGridView"
             Text="Details:" runat="server" />
            <br />
            <asp:GridView Width="85%" ID="ExpenseItemsGridView" runat="server" 
                          AutoGenerateColumns="False" AllowPaging="False" DataKeyNames="Id" EnableModelValidation="false" OnRowDataBound="ExpenseItemsGridViewOnRowDataBound">
                <Columns>        
                    <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" />
                    <asp:TemplateField HeaderText="Amount" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px">
                        <ItemTemplate><%# string.Format(System.Globalization.CultureInfo.CurrentUICulture, "${0}", Math.Round((double)Eval("Amount"), 2)) %></ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle BackColor="#e6e6e6" />
                <EmptyDataTemplate>
                    No details added yet.
                </EmptyDataTemplate>
            </asp:GridView>
        </p>
        <p>
            <asp:Label ID="ExpenseReimbursementMethodLabel" AssociatedControlID="ExpenseReimbursementMethod"
                Text="Reimb. Method:" runat="server" />
        <br />
            <asp:Label ID="ExpenseReimbursementMethod" runat="server" />
        </p>
        <p>
            <asp:Label ID="ExpenseCostCenterLabel" AssociatedControlID="ExpenseCostCenter" Text="Cost Center:"
                runat="server" />
        <br />
            <asp:Label ID="ExpenseCostCenter" runat="server" />
        </p>
        <p>
            <asp:Label ID="ApproverLabel" AssociatedControlID="Approver" Text="Approver:"
                runat="server" />
        <br />
            <asp:Label ID="Approver" runat="server" />
        </p>
    </div>
</asp:Content>
