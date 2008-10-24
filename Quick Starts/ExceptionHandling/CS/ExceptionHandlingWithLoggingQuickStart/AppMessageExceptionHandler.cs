//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block QuickStart
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;

namespace ExceptionHandlingQuickStart
{
    /// <summary>
    /// Summary description for GlobalPolicyExceptionHandler.
    /// </summary>
    [ConfigurationElementType(typeof(CustomHandlerData))]
    public class AppMessageExceptionHandler : IExceptionHandler
    {
        public AppMessageExceptionHandler(NameValueCollection ignore)
        {
        }


        public Exception HandleException(Exception exception, Guid correlationID)
        {
            DialogResult result = this.ShowThreadExceptionDialog(exception);

            // Exits the program when the user clicks Abort.
            if (result == DialogResult.Abort)
                Application.Exit();

            return exception;
        }

        // Creates the error message and displays it.
        private DialogResult ShowThreadExceptionDialog(Exception e)
        {
            string errorMsg = e.Message + Environment.NewLine + Environment.NewLine;

            return MessageBox.Show(errorMsg, "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }
    }
}
