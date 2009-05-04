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
using System.IO;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using System.Globalization;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// Represents a command that will open an application configuration.
    /// </summary>
    public class OpenFileConfigurationApplicationNodeCommand : ConfigurationNodeCommand
    {
		private string file;

        /// <summary>
		/// Initialize a new instance of the <see cref="OpenFileConfigurationApplicationNodeCommand"/> class with an <see cref="IServiceProvider"/>.
        /// </summary>
        /// <param name="serviceProvider">
        /// The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.
        /// </param>
		/// <param name="file"></param>
        public OpenFileConfigurationApplicationNodeCommand(IServiceProvider serviceProvider, string file) : this(serviceProvider, file, true)
        {
        }

        /// <summary>
		/// Initialize a new instance of the <see cref="OpenFileConfigurationApplicationNodeCommand"/> class with an <see cref="IServiceProvider"/> and if the error service should be cleared after the command executes.
        /// </summary>
        /// <param name="serviceProvider">
        /// The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.
        /// </param>
		/// <param name="file"></param>
        /// <param name="clearErrorLog">
        /// Determines if all the messages in the <see cref="IErrorLogService"/> should be cleared when the command has executed.
        /// </param>
		public OpenFileConfigurationApplicationNodeCommand(IServiceProvider serviceProvider, string file, bool clearErrorLog)
			: base(serviceProvider, clearErrorLog)
        {
			if (string.IsNullOrEmpty(file)) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "file");
			this.file = file;
        }

        /// <summary>
        /// Opens a previously saved configuration.
        /// </summary>
        /// <param name="node">
        /// The node to execute the command upon.
        /// </param>
        protected override void ExecuteCore(ConfigurationNode node)
        {
            try
            {
                UIService.BeginUpdate();
                OpenFile(node);
                if (ErrorLogService.ConfigurationErrorCount > 0)
                {
                    UIService.DisplayErrorLog(ErrorLogService);                    
                }
            }
            finally
            {
                UIService.EndUpdate();
            }
        }

        private void OpenFile(ConfigurationNode node)
        {
			string fileToOpen = file;
            if (!Path.IsPathRooted(file))
			{
				string dir = Directory.GetCurrentDirectory();
				fileToOpen = Path.Combine(dir, file);
			}
			if (!File.Exists(file))
			{
				ServiceHelper.LogError(ServiceProvider, new ConfigurationError(node, string.Format(CultureInfo.CurrentCulture, Resources.ErrorFileCouldNotBeOpened, file)));
				return;
			}
			ConfigurationApplicationFile data = new ConfigurationApplicationFile(Path.GetDirectoryName(fileToOpen), fileToOpen);
			IConfigurationUIHierarchy hierarchy = new ConfigurationUIHierarchy(new ConfigurationApplicationNode(data), ServiceProvider);
            ConfigurationUIHierarchyService.AddHierarchy(hierarchy);
            hierarchy.Open();            
        }
    }
}
