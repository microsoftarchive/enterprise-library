// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

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