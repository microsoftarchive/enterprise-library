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
using System.Web;
using System.Web.UI;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.Unity;

namespace AExpense
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            IUnityContainer container = new UnityContainer();
            ContainerBootstrapper.Configure(container);

            // Store the configured container in app state
            Application.SetContainer(container);
        }

        private void Application_Error(object sender, EventArgs e)
        {
            // Global error log 
            Exception ex = Server.GetLastError();
            ExceptionPolicy.HandleException(ex, Constants.GlobalPolicy);
        }

        protected void Application_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                return;
            }

            Page handler = HttpContext.Current.Handler as Page;
            if (handler != null)
            {
                IUnityContainer container = Application.GetContainer();
                container.BuildUp(handler.GetType(), handler);
            }
        }
    }
}