//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides.Console.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides.Console
{
    class ConsoleUIService : IUIService
    {
        #region IUIService Members

        public void BeginUpdate()
        {
        }

        public void EndUpdate()
        {
        }

        public DialogResult ShowSaveDialog(SaveFileDialog dialog)
        {
            return DialogResult.Cancel;
        }

        public DialogResult ShowOpenDialog(OpenFileDialog dialog)
        {
            return DialogResult.Cancel;
        }

        public IWin32Window OwnerWindow
        {
            get { return null; }
        }

        public void ActivateNode(ConfigurationNode node)
        {
        }

        public void DisplayErrorLog(IErrorLogService errorLogService)
        {
            bool containsErrors = false;

            errorLogService.ForEachConfigurationErrors(delegate(ConfigurationError error)
            {
                string configurationErrorLine = string.Format(Resources.ConfigurationErrorFmt, error.Message);
                System.Console.WriteLine(configurationErrorLine);
                containsErrors = true;
            });

            errorLogService.ForEachValidationErrors(delegate(ValidationError validationError)
            {
                string configurationErrorLine = string.Format(Resources.ConfigurationErrorFmt, validationError.InvalidItem.Path, validationError.PropertyName, validationError.Message);
                System.Console.WriteLine(configurationErrorLine);
                containsErrors = true;
            });

            if (containsErrors)
            {
                Environment.Exit(1);
            }
        }

        public void SetUIDirty(IConfigurationUIHierarchy hierarchy)
        {
            
        }

        public bool IsDirty(IConfigurationUIHierarchy hierarchy)
        {
            return false;   
        }

        public void SetStatus(string status)
        {
            
        }

        public void ClearErrorDisplay()
        {
            
        }

        public void ShowError(Exception e)
        {
            ShowError(e, Resources.GenericExceptionMessage);   
        }

        public void ShowError(Exception e, string message)
        {
            ShowError(e, message, string.Empty);
        }

        public void ShowError(Exception e, string message, string caption)
        {
            string.Format(Resources.ExceptionFmt, message, e);
            System.Console.WriteLine(e);

            Environment.Exit(1);
        }

        public void ShowError(string message)
        {
            System.Console.WriteLine(message);
            Environment.Exit(1);
        }

        public void ShowError(string message, string caption)
        {
            System.Console.WriteLine(caption);
            System.Console.WriteLine(message);
            Environment.Exit(1);
        }

        public void ShowMessage(string message)
        {
            
        }

        public void ShowMessage(string message, string caption)
        {
            
        }

        public DialogResult ShowMessage(string message, string caption, MessageBoxButtons buttons)
        {
            return DialogResult.Cancel;
        }

        public void RefreshPropertyGrid()
        {
        }

        #endregion
    }
}
