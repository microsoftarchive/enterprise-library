
<%@ Page Title="" Language="C#" MasterPageFile="~/Styling/Site.Master" AutoEventWireup="true"
    CodeBehind="AddExpense.aspx.cs" Inherits="AExpense.AddExpense" Culture="en-US" ValidateRequest="true" %>

<%@ Register assembly="Microsoft.Practices.EnterpriseLibrary.Validation.Integration.AspNet" namespace="Microsoft.Practices.EnterpriseLibrary.Validation.Integration.AspNet" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceholder" runat="server">
    <link type="text/css" href="Styling/styles/redmond/jquery-ui-1.8.custom.css" rel="stylesheet" />
    <script src="https://ajax.microsoft.com/ajax/jquery/jquery-1.4.2.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui-1.8.custom.min.js" type="text/javascript"></script>
     <script type="text/javascript">
         $(function() {
            $(<%= "\"#" + this.ExpenseDate.ClientID + "\"" %>).datepicker();
         });
	</script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceholder" runat="server">
    <form method="post" action="AddExpense.aspx">
    <div class="infoBox">
        <h2>
            Create a New Expense</h2>
        <p>
            Use the form below to report a new expense.
        </p>
    </div>
    <div id="expenseform">
        <p>
            <asp:Label ID="ExpenseDateLabel" AssociatedControlID="ExpenseDate" runat="server">
            Date:<br />
            <span>Select the expense date by clicking on the text box below.</span>
            </asp:Label>
            <br />
            <asp:TextBox ID="ExpenseDate" runat="server" MaxLength="10" />
            &nbsp;<asp:RequiredFieldValidator ID="ExpenseDateRequiredValidator" runat="server"
                ErrorMessage="*" ControlToValidate="ExpenseDate" />
            <asp:CompareValidator ID="ExpenseDateFormatValidator" runat="server" ErrorMessage="Enter a valid date to continue."
                ControlToValidate="ExpenseDate" Type="Date" Operator="DataTypeCheck" CultureInvariantValues="True" ForeColor="Red" />
        </p>
        <p>
            <asp:Label ID="ExpenseTitleLabel" AssociatedControlID="ExpenseTitle" Text="Title:"
                runat="server" />
        <br />
            <asp:TextBox ID="ExpenseTitle" runat="server" MaxLength="100" />
            &nbsp;<cc1:PropertyProxyValidator ID="ExpenseTitleValidator" runat="server" ControlToValidate="ExpenseTitle" PropertyName="Title" SourceTypeName="AExpense.Model.Expense, AExpense" ForeColor="Red"></cc1:PropertyProxyValidator>
        </p>
        <p>
            <asp:Label ID="ExpenseDetailsLabel" AssociatedControlID="ExpenseItemsGridView"
             Text="Details:" runat="server" />
            <br />
            
            <div style="float: left; display: inline-table">
                <asp:Label ID="ExpenseItemDescriptionLabel" AssociatedControlID="ExpenseItemDescription" runat="server">
                    <span>Description:</span>
                </asp:Label>
                <asp:TextBox ID="ExpenseItemDescription" runat="server" MaxLength="100" />
                    &nbsp;<cc1:PropertyProxyValidator ID="ExpenseItemDescriptionValidator" runat="server" ControlToValidate="ExpenseItemDescription" PropertyName="Description" SourceTypeName="AExpense.Model.ExpenseItem, AExpense" ValidationGroup="AddNewExpenseItem" ForeColor="Red"></cc1:PropertyProxyValidator>
                <br />
                <br />
                <asp:Label ID="ExpenseItemAmountLabel" AssociatedControlID="ExpenseItemAmount" runat="server">
                    <span>Amount:</span>
                </asp:Label>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:TextBox ID="ExpenseItemAmount" runat="server" CssClass="amountText" MaxLength="20" />
                &nbsp;<cc1:PropertyProxyValidator ID="ExpenseItemAmountValidator" runat="server" ControlToValidate="ExpenseItemAmount" PropertyName="Amount" SourceTypeName="AExpense.Model.ExpenseItem, AExpense" ValidationGroup="AddNewExpenseItem" OnValueConvert="AmountValueConvert" ForeColor="Red"></cc1:PropertyProxyValidator>

                <asp:Button ID="AddNewExpenseItem" runat="server" OnClick="OnAddNewExpenseItemClick" Text="˅ Add ˅"  ValidationGroup="AddNewExpenseItem" />
            </div>
            <div style="clear: both"></div>
            <br />
            <asp:GridView Width="85%" ID="ExpenseItemsGridView" runat="server" 
                          AutoGenerateColumns="False" AllowPaging="False" DataKeyNames="Id" EnableModelValidation="false"
                          OnRowDeleting="ExpenseItemsGridViewOnRowDeleting" OnRowDataBound="ExpenseItemsGridViewOnRowDataBound">
                <Columns>        
                    <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" />
                    <asp:TemplateField HeaderText="Amount" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px">
                        <ItemTemplate><%# string.Format(System.Globalization.CultureInfo.CurrentUICulture, "${0}", Math.Round((double)Eval("Amount"), 2)) %></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="25px">
                        <ItemTemplate>
                            <asp:ImageButton id="DeleteButton" CommandName="Delete" ImageUrl="~/Styling/Images/cancel.gif" runat="server" ValidationGroup="DeleteExpenseItem" />
                        </ItemTemplate>
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
            <asp:DropDownList ID="ExpenseReimbursementMethod" runat="server" />
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
            <asp:TextBox ID="Approver" runat="server" MaxLength="50" />
            &nbsp;<cc1:PropertyProxyValidator ID="ExpenseApproverValidator" runat="server" ControlToValidate="Approver" PropertyName="ApproverName" SourceTypeName="AExpense.Model.Expense, AExpense" ForeColor="Red"></cc1:PropertyProxyValidator>
        </p>
        <p>
            <asp:Label ID="pageErrorMessage" runat="server" ForeColor="Red"></asp:Label>
        </p>
        <div id="newexpense-button">
            <asp:Button ID="AddExpenseButton" runat="server" Text="Add »" OnClick="AddExpenseButtonOnClick" />
        </div>
    </div>
    </form>
</asp:Content>
