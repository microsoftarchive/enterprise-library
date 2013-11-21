// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

namespace AExpense
{
    using System;
    using System.Web;

    public partial class Site : System.Web.UI.MasterPage
    {
        protected void LoginStatusOnLoggedOut(object sender, EventArgs e)
        {
            this.Session.Abandon();
            var sessionCookie = new HttpCookie("ASP.NET_SessionId", string.Empty);
            sessionCookie.Expires = DateTime.Now.AddDays(-1d);
            Response.Cookies.Add(sessionCookie);
            Response.Redirect("default.aspx");
        }
    }
}
