// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Web.Security;

namespace AExpense
{
    public partial class SimulatedWindowsAuthentication : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            this.ViewStateUserKey = this.User.Identity.Name;            
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request.IsAuthenticated && !string.IsNullOrEmpty(Request.QueryString["ReturnUrl"]))
                {
                    this.Response.Redirect("~/401.htm");
                }
            }
        }

        protected void ContinueButtonClick(object sender, EventArgs e)
        {
            FormsAuthentication.RedirectFromLoginPage(this.UserList.SelectedValue, false);
        }
    }
}
