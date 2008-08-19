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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using System.Drawing.Design;
using System.IO;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
	/// <summary>
	/// Represents a design time node for the <see cref="FileConfigurationSourceElement"/> configuration.
	/// </summary>
	public class FileConfigurationSourceElementNode : ConfigurationSourceElementNode
	{
		private string filePath;
		private Type type;

		/// <summary>
		/// Initialize a new instance of the <see cref="FileConfigurationSourceElement"/> class.
		/// </summary>
		public FileConfigurationSourceElementNode()
			: this(new FileConfigurationSourceElement())
		{
		}

		/// <summary>
		/// Initialize a new instance of the <see cref="FileConfigurationSourceElement"/> class with a <see cref="FileConfigurationSourceElement"/>.
		/// </summary>
		/// <param name="element">The <see cref="FileConfigurationSourceElement"/> to initialize.</param>
		public FileConfigurationSourceElementNode(FileConfigurationSourceElement element)
			: base(null == element ? string.Empty : element.Name)
		{
			if (null == element) throw new ArgumentNullException("element");

			this.filePath = element.FilePath;
			this.type = element.Type;
		}		

		/// <summary>
		/// Gets the <see cref="Type"/> of the <see cref="IConfigurationSource"/>.
		/// </summary>
		/// <value>
		/// The <see cref="Type"/> of the <see cref="IConfigurationSource"/>.
		/// </value>
		[SRCategory("CategoryGeneral", typeof(Resources))]
		[SRDescription("TypeNameDescription", typeof(Resources))]
		[ReadOnly(true)]
		public Type Type
		{
			get { return type; }
		}


		/// <summary>
		/// Gets or sets the name of the file.
		/// </summary>
		/// <value>
		/// The name of the file.
		/// </value>
		[SRCategory("CategoryGeneral", typeof(Resources))]
		[SRDescription("FileSourceFileDescription", typeof(Resources))]
		[Editor(typeof(SaveFileEditor), typeof(UITypeEditor))]
		[FilteredFileNameEditor(typeof(Resources), "ConfigurationFileDialogFilter")]		
		[ExternalConfigurationFileStorageCreation]
		[FileValidationAttribute]
        [Required]
		public string File
		{
			get 
			{ 
				return filePath;
			}
			set { filePath = value;  }
		}

		/// <summary>
		/// Gets the <see cref="FileConfigurationSourceElement"/> this node represents.
		/// </summary>
		/// <value>
		/// The <see cref="FileConfigurationSourceElement"/> this node represents.
		/// </value>
		public override ConfigurationSourceElement ConfigurationSourceElement
		{
			get { return new FileConfigurationSourceElement(Name, filePath); }
		}

		/// <summary>
		/// Gets the <see cref="IConfigurationSource"/> that this node represents.
		/// </summary>
		/// <value>
		/// The <see cref="IConfigurationSource"/> that this node represents.
		/// </value>
		/// <remarks>
		/// Returns a <see cref="FileConfigurationSource"/>.
		/// </remarks>
		public override IConfigurationSource ConfigurationSource
		{
			get 
			{
				string file = GetFileToSave();
                if (System.IO.File.Exists(file))
                {
                    try
                    {
                        FileConfigurationSource.ResetImplementation(file, false);
                        return new FileConfigurationSource(file);
                    }
                    catch (IOException)
                    {
                        return null;
                    }
                }
                return null;
			}
		}		

		/// <summary>
		/// Gets the <see cref="IConfigurationParameter"/> that this node represents.
		/// </summary>
		/// <value>
		/// The <see cref="IConfigurationParameter"/> that this node represents.
		/// </value>
		/// <remarks>
		/// Returns a <see cref="FileConfigurationParameter"/>.
		/// </remarks>
		public override IConfigurationParameter ConfigurationParameter
		{
			get
			{
				string file = GetFileToSave();
				return new FileConfigurationParameter(file);
			}
		}

		private string GetFileToSave()
		{
			string file = File;
			if (!System.IO.Path.IsPathRooted(file))
			{
				file = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(ServiceHelper.GetCurrentStorageService(Site).ConfigurationFile), file);
			}
			return file;
		}
	}
}
