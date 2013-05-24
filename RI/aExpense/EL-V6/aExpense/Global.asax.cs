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
using System.Diagnostics.Tracing;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using AExpense.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Sinks;
using Microsoft.Practices.Unity;

namespace AExpense
{
    public class Global : HttpApplication
    {
        private bool useInprocEventTracing;
        private EventListener fileListener;
        private EventListener rollingfileListener;
        private EventListener dbListener;

        protected void Application_Start(object sender, EventArgs e)
        {
            this.SetupEventTracing();

            AExpenseEvents.Log.ApplicationStarting();

            IUnityContainer container = new UnityContainer();            
            ContainerBootstrapper.Configure(container);

            // Store the configured container in app state
            Application.SetContainer(container);

            AExpenseEvents.Log.ApplicationStarted();
        }

        private void Application_Error(object sender, EventArgs e)
        {
            // Global error log 
            Exception ex = Server.GetLastError();
            AExpenseEvents.Log.ApplicationError(ex.Message, ex.GetType().FullName);

            IUnityContainer container = Application.GetContainer();
            var exceptionManager = container.Resolve<ExceptionManager>();
            if (exceptionManager != null)
            {
                exceptionManager.HandleException(ex, Constants.GlobalPolicy);
            }
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

        public override void Dispose()
        {
            if (this.useInprocEventTracing)
            {
                this.fileListener.DisableEvents(AExpenseEvents.Log);
                this.fileListener.Dispose();
                this.rollingfileListener.DisableEvents(AExpenseEvents.Log);
                this.rollingfileListener.Dispose();
                this.dbListener.DisableEvents(AExpenseEvents.Log);
                this.dbListener.Dispose();
            }

            base.Dispose();
        }

        private void SetupEventTracing()
        {
            bool.TryParse(WebConfigurationManager.AppSettings["UseInprocEventTracing"], out this.useInprocEventTracing);

            if (this.useInprocEventTracing)
            {
                //Log to file all DataAccess events
                this.fileListener = FlatFileLog.CreateListener("aExpense.DataAccess.log", formatter: new XmlEventTextFormatter(EventTextFormatting.Indented), isAsync: true);
                fileListener.EnableEvents(AExpenseEvents.Log, EventLevel.LogAlways, AExpenseEvents.Keywords.DataAccess);

                //Log to Rolling file informational UI events only
                this.rollingfileListener = RollingFlatFileLog.CreateListener("aExpense.UserInterface.log", rollSizeKB: 10, timestampPattern: "yyyy", rollFileExistsBehavior: RollFileExistsBehavior.Increment, rollInterval: RollInterval.Day, formatter: new JsonEventTextFormatter(EventTextFormatting.Indented), isAsync: true);
                rollingfileListener.EnableEvents(AExpenseEvents.Log, EventLevel.Informational, AExpenseEvents.Keywords.UserInterface);                

                // Log all events to DB 
                this.dbListener = SqlDatabaseLog.CreateListener("aExpense", WebConfigurationManager.ConnectionStrings["Tracing"].ConnectionString, bufferingInterval: TimeSpan.FromSeconds(3), bufferingCount:10);
                dbListener.EnableEvents(AExpenseEvents.Log, EventLevel.LogAlways, Keywords.All);                
            }
        }
    }
}