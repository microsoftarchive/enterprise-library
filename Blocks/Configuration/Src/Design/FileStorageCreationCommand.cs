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
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{

    /// <summary>
    /// Creates configuration stored in a file. This class is abstract.
    /// </summary>
	public abstract class FileStorageCreationCommand : StorageCreationCommand
	{
	    private string fileName;

        /// <summary>
        /// Initialize a new instance of the <see cref="FileStorageCreationCommand"/> class with a file name and <see cref="IServiceProvider"/>.
        /// </summary>
        /// <param name="fileName">The name of the file to create.</param>
        /// <param name="serviceProvider">
        /// The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.
        /// </param>
	    protected FileStorageCreationCommand(string fileName, IServiceProvider serviceProvider) : base(fileName, serviceProvider)
		{
	        this.fileName = fileName;
		}

        /// <summary>
        /// Gets or sets the name of the file to create.
        /// </summary>
        /// <value>
        /// The name of the file to create.
        /// </value>
	    protected string FileName
	    {
	        get { return GetAbsolutePathFromConfigurationPath(ServiceHelper.GetCurrentStorageService(ServiceProvider).ConfigurationFile, fileName); }
            set { fileName = value; }
	    }

        /// <summary>
        /// Determines if the current <see cref="FileName"/> can be overwrriten. It will prompt the user throuth the user interface if the user wants to overwrite the file.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if the file can be overwritten; otherwise, <see langword="false"/>.
        /// </returns>
	    protected bool CanOverwriteFile()
        {
            if (IsFileReadOnly(fileName))
            {
                IUIService uiService = ServiceHelper.GetUIService(ServiceProvider);            

				DialogResult result = uiService.ShowMessage(string.Format(CultureInfo.CurrentUICulture, Resources.OverwriteFileMessage, fileName), Resources.OverwriteFileCaption, System.Windows.Forms.MessageBoxButtons.YesNo);
                if (DialogResult.Yes == result)
                {
                    ChangeFileAttributesToWritable(fileName);
                }
                else
                {                    
					ServiceHelper.LogError(ServiceProvider, new ConfigurationError(null, Resources.ExceptionFilesNotSaved));
                    return false;
                }
            }
            return true;
        }
		
		/// <summary>
		/// Gets the absolute path for a file based on the application's configuration file location.
		/// </summary>
		/// <param name="configurationFile">The application's configuration file.</param>
		/// <param name="filePath">The file to get the absolute path for.</param>
		/// <returns>The absolute path to the file.</returns>
        protected static string GetAbsolutePathFromConfigurationPath(string configurationFile, string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return filePath;
            if (!Path.IsPathRooted(filePath))
            {
                if (configurationFile == null || configurationFile.Length == 0) return filePath;
                filePath = Path.Combine(Path.GetDirectoryName(configurationFile), filePath);
            }
            return filePath.ToLower(CultureInfo.InvariantCulture);
        }

		private static void ChangeFileAttributesToWritable(string filePath)
		{
			if (!File.Exists(filePath))
			{
				return;
			}
			FileAttributes attributes = File.GetAttributes(filePath);
			FileAttributes attr = attributes | FileAttributes.ReadOnly;
			if (attr == attributes)
			{
				attributes ^= FileAttributes.ReadOnly;
				File.SetAttributes(filePath, attributes);
			}
		}

		private static bool IsFileReadOnly(string filePath)
		{
			if (!File.Exists(filePath))
			{
				return false;
			}
			FileAttributes attributes = File.GetAttributes(filePath);
			FileAttributes attr = attributes | FileAttributes.ReadOnly;
			if (attr == attributes)
			{
				return true;
			}
			return false;
		}
	}
}
