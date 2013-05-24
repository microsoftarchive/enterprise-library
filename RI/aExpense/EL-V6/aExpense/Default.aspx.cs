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

using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using AExpense.DataAccessLayer;
using Microsoft.Practices.Unity;
using Microsoft.Security.Application;

namespace AExpense
{
    public partial class Default : Page
    {
        [Dependency]
        public IExpenseRepository Repository { get; set; }

        protected void Page_Init(object sender, EventArgs e)
        {
            this.ViewStateUserKey = this.User.Identity.Name;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var expenses = Repository.GetExpensesByUser(this.User.Identity.Name);
            this.MyExpensesGridView.DataSource = expenses;
            this.DataBind();
        }

        protected void MyExpensesGridViewOnRowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e == null || e.CommandName == null) return;

            if (e.CommandName == "Select")
            {
                int selectedRow = Convert.ToInt32(e.CommandArgument, CultureInfo.InvariantCulture);
                string expenseId = this.MyExpensesGridView.DataKeys[selectedRow].Value.ToString();
                string expenseDetailsUrl = string.Format(CultureInfo.InvariantCulture, "ExpenseDetails.aspx?id={0}", expenseId);
                this.Response.Redirect(expenseDetailsUrl);
            }
        }

        protected void MyExpensesGridViewOnRowDataBound(object sender, GridViewRowEventArgs eventArgs)
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
    }
}
