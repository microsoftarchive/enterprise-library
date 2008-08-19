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
using System.IO;
using System.Text;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using System.Collections.Specialized;

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

        public Exception HandleException(Exception exception, Guid handlingInstanceId)
        {
            DialogResult result = this.ShowThreadExceptionDialog(exception);

            return exception;
        }

		// Creates the error message and displays it.
		private DialogResult ShowThreadExceptionDialog(Exception ex) 
		{
			string errorMsg = "The following exception was caught by the Quick Start Global Exception Handler:" + Environment.NewLine + Environment.NewLine;

			StringBuilder sb = new StringBuilder();
			StringWriter writer = new StringWriter(sb);

			AppTextExceptionFormatter formatter = new AppTextExceptionFormatter(writer, ex);
			
			// Format the exception
			formatter.Format();

			errorMsg +=  sb.ToString();

			return MessageBox.Show(errorMsg, "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
		}
	    
	}
}
