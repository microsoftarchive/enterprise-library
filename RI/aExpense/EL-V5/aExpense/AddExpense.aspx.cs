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
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using AExpense.DataAccessLayer;
using AExpense.Model;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration;
using Microsoft.Practices.Unity;
using Microsoft.Security.Application;

namespace AExpense
{
    public partial class AddExpense : Page
    {
        [Dependency]
        public IExpenseRepository Repository { get; set; }
        
        [Dependency]
        public IUserRepository UserRepository { get; set; }

        private List<ExpenseItem> ExpenseItems
        {
            get
            {
                if (this.Session["ExpenseItems"] == null)
                {
                   this.Session["ExpenseItems"] = new List<ExpenseItem>();
                }

                return (List<ExpenseItem>)this.Session["ExpenseItems"];
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.Session["ExpenseItems"] = null;
                this.InitializeControls();
            }
        }

        protected void AddExpenseButtonOnClick(object sender, EventArgs e)
        {
            if (this.IsValid)
            {
                if (this.SaveExpense()) 
                    Response.Redirect("~/Default.aspx", true);
            }
        }
        
        protected void OnAddNewExpenseItemClick(object sender, EventArgs e)
        {
            this.Validate("AddNewExpenseItem");
            /// Here must be placed all the extra validations for the inputs 
            /// (like length, potentially dangerous characters, etc.)
            if (this.IsValid)
            {
                var item = new ExpenseItem
                        {
                            Id = Guid.NewGuid(),
                            Description = this.ExpenseItemDescription.Text,
                            Amount = double.Parse(this.ExpenseItemAmount.Text, CultureInfo.CurrentCulture),
                        };

                this.ExpenseItems.Add(item);
                this.ExpenseItemDescription.Text = string.Empty;
                this.ExpenseItemAmount.Text = string.Empty;
            }

            this.ExpenseItemsGridView.DataSource = this.ExpenseItems;
            this.ExpenseItemsGridView.DataBind();
        }

        protected void ExpenseItemsGridViewOnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (e == null) return;

            this.ExpenseItems.RemoveAt(e.RowIndex);
            this.ExpenseItemsGridView.DataSource = this.ExpenseItems;
            this.ExpenseItemsGridView.DataBind();
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

        protected void AmountValueConvert(object sender, ValueConvertEventArgs e)
        {
            double convertedValue;

            if (double.TryParse(e.ValueToConvert as string, NumberStyles.AllowDecimalPoint, NumberFormatInfo.CurrentInfo, out convertedValue))
            {
                e.ConvertedValue = convertedValue;
            }
            else
            {
                e.ConversionErrorMessage = Properties.Resources.AmountRangeTypeValidation;
                e.ConvertedValue = null;
            }
        }

        private bool SaveExpense()
        {
            try
            {
                var user = UserRepository.GetUser(this.User.Identity.Name);
                var newExpense = new Model.Expense();
                
                newExpense.Title = this.ExpenseTitle.Text;
                newExpense.Details = new List<ExpenseItem>(this.ExpenseItems);
                newExpense.CostCenter = user.CostCenter;
                newExpense.ReimbursementMethod = (ReimbursementMethod)Enum.Parse(typeof(ReimbursementMethod), this.ExpenseReimbursementMethod.SelectedItem.Value);
                newExpense.User = user;
                newExpense.Date = DateTime.Parse(this.ExpenseDate.Text, CultureInfo.CurrentCulture);
                newExpense.ApproverName = this.Approver.Text;
                newExpense.Total = this.ExpenseItems.Sum(i => i.Amount);

                Repository.SaveExpense(newExpense);

                user.PreferredReimbursementMethod = (ReimbursementMethod)Enum.Parse(typeof(ReimbursementMethod), this.ExpenseReimbursementMethod.SelectedValue);
                UserRepository.UpdateUserPreferredReimbursementMethod(user);
            }
            catch (NotifyException exception)
            {
                this.pageErrorMessage.Text = exception.Message;
                return false;
            }
            return true;
        }

        private void InitializeControls()
        {
            var user = UserRepository.GetUser(this.User.Identity.Name);
            if (user == null)
            {
                throw new InvalidOperationException("User does not exist");
            }

            this.ExpenseReimbursementMethod.Items.Add(new ListItem("Check", ReimbursementMethod.Check.ToString()));
            this.ExpenseReimbursementMethod.Items.Add(new ListItem("Cash", ReimbursementMethod.Cash.ToString()));
            this.ExpenseReimbursementMethod.Items.Add(new ListItem("Direct Deposit", ReimbursementMethod.DirectDeposit.ToString()));
            if (user.PreferredReimbursementMethod != ReimbursementMethod.NotSet)
            {
                this.ExpenseReimbursementMethod.SelectedValue = user.PreferredReimbursementMethod.ToString();
            }

            this.ExpenseCostCenter.Text = Encoder.HtmlEncode(user.CostCenter);
            this.Approver.Text = user.Manager;

            this.ExpenseItemsGridView.DataSource = this.ExpenseItems;
            this.ExpenseItemsGridView.DataBind();
        }
    }
}
