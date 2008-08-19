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
using System.Xml;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
	/// <summary>
	/// Creates configuration stored in a external file.
	/// </summary>
    public class ExternalConfigurationFileStorageCreationCommand : FileStorageCreationCommand
	{
		/// <summary>
		/// Initialize a new instance of the <see cref="ExternalConfigurationFileStorageCreationCommand"/> class with a file name and <see cref="IServiceProvider"/>.
		/// </summary>
		/// <param name="fileName">The name of the file to create.</param>
		/// <param name="serviceProvider">
		/// The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.
		/// </param>
		public ExternalConfigurationFileStorageCreationCommand(string fileName, IServiceProvider serviceProvider)
			: base(fileName, serviceProvider)
		{
		}	

		/// <summary>
		/// Executes the creation of the file.
		/// </summary>
	    public override void Execute()
	    {
			string filePath = GetAbsolutePathFromConfigurationPath(ServiceHelper.GetCurrentRootNode(ServiceProvider).ConfigurationFile, FileName);			
			if (!File.Exists(filePath))
			{
                CreateFile(filePath);	
			}
	    }

        private void CreateFile(string filePath)
        {
            XmlDocument document = new XmlDocument();
            XmlElement element = document.CreateElement("configuration");
            document.AppendChild(element);
    
            if (CanOverwriteFile())
            {
                try
                {
                    document.Save(filePath);
                }
                catch (IOException e)
                {
                    LogError(e.Message);
                }
                catch (UnauthorizedAccessException e)
                {
                    LogError(e.Message);
                }
            }
        }
	}
}
