#region license
// ==============================================================================
// Microsoft patterns & practices Enterprise Library
// aExpense Reference Implementation
// ==============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ==============================================================================
#endregion

using AExpense.DataAccessLayer;
using AExpense.Model;
using Microsoft.Practices.Unity;
using Microsoft.Security.Application;
using System;
using System.Globalization;
using System.Web.UI.WebControls;

namespace AExpense
{
    public partial class ExpenseDetails : System.Web.UI.Page
    {
        [Dependency]
        public IExpenseRepository Repository { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        { 
            if (!IsPostBack)
            {
                Guid expenseId;
                try
                {
                    expenseId = Guid.Parse(this.Request.QueryString["id"]);
                }
                catch (ArgumentNullException)
                {
                    throw;
                }

                var expense = Repository.GetExpenseById(expenseId);

                if (expense == null)
                {
                    string errorMessage = string.Format(CultureInfo.CurrentCulture, "There is no expense with the id {0}.", expenseId);
                    throw new ArgumentException(errorMessage);
                }

                if (expense.User.UserName != this.User.Identity.Name)
                {
                    string errorMessage = string.Format(CultureInfo.CurrentCulture, "{0} cannot access the expense with id {1}.", this.User.Identity.Name, expense.Id);
                    throw new UnauthorizedAccessException(errorMessage);
                }

                this.ExpenseDate.Text = expense.Date.ToString("yyyy-MM-dd", CultureInfo.CurrentCulture);
                this.ExpenseTitle.Text = Encoder.HtmlEncode(expense.Title);
                this.ExpenseItemsGridView.DataSource = expense.Details;
                this.ExpenseItemsGridView.DataBind();
                this.ExpenseReimbursementMethod.Text = Encoder.HtmlEncode(Enum.GetName(typeof(ReimbursementMethod), expense.ReimbursementMethod));
                this.ExpenseCostCenter.Text = Encoder.HtmlEncode(expense.CostCenter);
                this.Approver.Text = Encoder.HtmlEncode(expense.ApproverName);
            }
        }

        protected void ExpenseItemsGridViewOnRowDataBound(object sender, GridViewRowEventArgs eventArgs)
        {
            if (eventArgs == null || eventArgs.Row == null) return;

            foreach (TableCell cell in eventArgs.Row.Cells)
            {
                if (!string.IsNullOrEmpty(cell.Text) && !cell.Text.Equals("&nbsp;"))
                {
                    cell.Text = Encoder.HtmlEncode(cell.Text);
                }
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            this.ViewStateUserKey = this.User.Identity.Name;
        }
    }
}
