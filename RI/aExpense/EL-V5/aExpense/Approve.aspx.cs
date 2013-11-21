// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using AExpense.DataAccessLayer;
using Microsoft.Practices.Unity;
using Microsoft.Security.Application;

namespace AExpense
{
    public partial class Approve : Page
    {
        [Dependency]
        public IExpenseRepository Repository { get; set; }

        protected void OnExpensesSelecting(object sender, ObjectDataSourceMethodEventArgs eventArgs)
        {
            if (eventArgs == null || eventArgs.InputParameters == null) return;
            eventArgs.InputParameters["approverName"] = User.Identity.Name;
        }

        protected void OnExpenseRowDataBound(object sender, GridViewRowEventArgs eventArgs)
        {
            if (eventArgs == null || eventArgs.Row == null) return;
            var row = eventArgs.Row.DataItem as Model.Expense;
            if (row != null)
            {
                if (row.Approved)
                {
                    eventArgs.Row.Enabled = false;
                }
            }

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

        protected void OnExpensesDataSourceObjectCreating(object sender, ObjectDataSourceEventArgs e)
        {
            e.ObjectInstance = this.Repository;
        }

        protected void OnRowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            var approved = (bool?)e.NewValues[0];
            e.Cancel = !approved.HasValue || !approved.Value;
        }
    }
}
