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
            if (Request.IsAuthenticated)
            {
                FormsAuthentication.RedirectFromLoginPage(this.User.Identity.Name, false);
            }

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
