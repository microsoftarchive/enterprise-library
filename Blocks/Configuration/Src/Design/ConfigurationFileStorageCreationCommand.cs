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
	sealed class ConfigurationFileStorageCreationCommand : FileStorageCreationCommand
	{
        private const string ConfigurationElementName = "configuration";
        private bool creationCancled;
		
	    public ConfigurationFileStorageCreationCommand(string fileName, IServiceProvider serviceProvider) : base(fileName, serviceProvider)
		{
		}

		public bool CreationCancled
	    {
	        get { return creationCancled; }
	    }

		public override void Execute()
	    {
            if (!EnsureConfigurationFileIsSet())
            {
                creationCancled = true;
                return;
            }
			if (!CanOverwriteFile())
			{
				creationCancled = true;
				return;
			}
	        SaveToFile();
	    }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		private void SaveToFile()
        {
            XmlDocument document = new XmlDocument();
            try
            {
                document.Load(FileName);
				if (document.DocumentElement == null || document.DocumentElement.Name != ConfigurationElementName)
				{
					CreateConfigurationElement(document);
				}
            }
			catch (FileNotFoundException)
			{
				CreateConfigurationElement(document);
			}
            catch (XmlException)
            {
                CreateConfigurationElement(document);
            }
            catch (Exception e)
            {
                LogError(e.Message);
                return;
            }
            try
            {
                document.Save(FileName);
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

        private static void CreateConfigurationElement(XmlDocument document)
        {
            if (document.DocumentElement != null) document.RemoveAll();
            XmlElement element = document.CreateElement(ConfigurationElementName);
            document.AppendChild(element);
        }

        private bool EnsureConfigurationFileIsSet()
        {
            if (string.IsNullOrEmpty(FileName))
            {
                if (DialogResult.OK != GetApplicationFileFromUI())
                {
                    return false;
                }
            }
            return true;
        }

        private DialogResult GetApplicationFileFromUI()
        {
			ConfigurationApplicationNode node = (ConfigurationApplicationNode)ServiceHelper.GetCurrentRootNode(ServiceProvider);
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = Resources.ConfigurationFileDialogFilter;
            dialog.Title = System.String.Concat(Resources.SaveApplicationCaption, " : ", node.Name);
            IUIService uiService = ServiceHelper.GetUIService(ServiceProvider);
            System.Windows.Forms.DialogResult result = uiService.ShowSaveDialog(dialog);
			Application.DoEvents();
			// we need this because the wait cursor gets set back by the save dialog
			Cursor.Current = Cursors.WaitCursor;
            if (System.Windows.Forms.DialogResult.OK == result)
            {
				DeleteFileIfItExists(dialog.FileName);
                FileName = dialog.FileName;
                node.ConfigurationFile = FileName;
                node.Hierarchy.StorageService.ConfigurationFile = FileName;
                uiService.RefreshPropertyGrid();
            }
            return result;
        }

		private static void DeleteFileIfItExists(string fileName)
		{
			if (!File.Exists(fileName)) return;

			using (FileStream stream = new FileStream(fileName, FileMode.Truncate))
			{

			}
		}       
	}
}
