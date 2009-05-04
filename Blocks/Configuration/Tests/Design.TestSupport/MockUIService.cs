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
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.TestSupport
{
    public class MockUIService : IUIService, IDisposable
    {        
        private int validationErrorsCount;
        private int configurationErrorsCount;
		private Dictionary<Guid, IConfigurationUIHierarchy> dirtyHierarchies;

        public MockUIService()
        {
			dirtyHierarchies = new Dictionary<Guid, IConfigurationUIHierarchy>();
        }

		public void Dispose()
		{
			if (dirtyHierarchies != null)
			{
				dirtyHierarchies.Clear();
			}
		}

        public int ValidationErrorsCount
        {
            get { return validationErrorsCount; }
        }

        public int ConfigurationErrorsCount
        {
            get { return configurationErrorsCount; }
        }

        public virtual void BeginUpdate()
        {
            Console.WriteLine("BeginUpdate called");
        }

        public virtual void EndUpdate()
        {
            Console.WriteLine("EndUpdate called");
        }

        public virtual DialogResult ShowSaveDialog(SaveFileDialog dialog)
        {
            Console.WriteLine("ShowSaveDialog called");
            return DialogResult.OK;
        }

        public virtual DialogResult ShowOpenDialog(OpenFileDialog dialog)
        {
            Console.WriteLine("ShowOpenDialog called");
            return DialogResult.OK;
        }

        public virtual IWin32Window OwnerWindow
        {
            get { return null; }
        }

        public virtual void ActivateNode(ConfigurationNode node)
        {
            Console.WriteLine("ActivateNode:" + node.Name);
        }

        public void DisplayErrorLog(IErrorLogService errorLogService)
        {			
			errorLogService.ForEachConfigurationErrors(new Action<ConfigurationError>(DisplayConfigurationError));
			configurationErrorsCount = errorLogService.ConfigurationErrorCount;
			errorLogService.ForEachValidationErrors(new Action<ValidationError>(DisplayValidationError));
            validationErrorsCount = errorLogService.ValidationErrorCount;			                        
        }

		private void DisplayConfigurationError(ConfigurationError error)
		{
			Console.WriteLine("DisplayConfigurationError:" + error.Message);
		}

		private void DisplayValidationError(ValidationError error)
		{
			Console.WriteLine("DisplayValidationErrors:" + error.Message);
		}

        public virtual void SetUIDirty(IConfigurationUIHierarchy hierarchy)
        {
            if (!dirtyHierarchies.ContainsKey(hierarchy.Id))
            {
                dirtyHierarchies.Add(hierarchy.Id, hierarchy);
            }
            Console.WriteLine("Dirty called for " + hierarchy.Id.ToString());
        }

		public virtual bool IsDirty(IConfigurationUIHierarchy hierarchy)
        {
            Console.WriteLine("IsDirty called for " + hierarchy.Id.ToString());
            return dirtyHierarchies.ContainsKey(hierarchy.Id);
        }

        public virtual void SetStatus(string status)
        {
            Console.WriteLine("SetStatus: " + status);
        }

        public virtual void ClearErrorDisplay()
        {
            Console.WriteLine("ClearErrorDisplay called");
        }

        public virtual void ShowError(Exception e)
        {
            Console.WriteLine("ShowError: " + e.Message);
        }

        public virtual void ShowError(Exception e, string message)
        {
            Console.WriteLine("ShowError: Exception = " + e.Message + " message = " + message);
        }

        public virtual void ShowError(Exception e, string message, string caption)
        {
            Console.WriteLine("ShowError: Exception = " + e.Message + " message = " + message + " caption = " + caption);
        }

        public virtual void ShowError(string message)
        {
            Console.WriteLine("ShowError: message = " + message);
        }

        public virtual void ShowError(string message, string caption)
        {
            Console.WriteLine("ShowError: message = " + message + " caption = " + caption);
        }

        public virtual void ShowMessage(string message)
        {
            Console.WriteLine("ShowError: message = " + message);
        }

        public virtual void ShowMessage(string message, string caption)
        {
            Console.WriteLine("ShowMessage: message = " + message + " caption = " + caption);
        }

        public virtual DialogResult ShowMessage(string message, string caption, MessageBoxButtons buttons)
        {
            Console.WriteLine("ShowMessage: message = " + message + " caption = " + caption);
            return DialogResult.OK;
        }

        public void RefreshPropertyGrid()
        {
            Console.WriteLine("RefreshPropertyGrid called");
        }
    }
}
