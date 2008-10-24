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
using System.ComponentModel;
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Manageability.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Manageability
{
	/// <summary>
	/// 
	/// </summary>
	public class ManageableConfigurationSourceElementNode : ConfigurationSourceElementNode
	{
		private string filePath;
		private Type type;
		private bool enableWmi;
		private bool enableGroupPolicies;
		private string applicationName;

		/// <summary>
		/// 
		/// </summary>
		public ManageableConfigurationSourceElementNode()
			: this(new ManageableConfigurationSourceElement())
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="element"></param>
		public ManageableConfigurationSourceElementNode(ManageableConfigurationSourceElement element)
			: base(null == element ? string.Empty : element.Name)
		{
			if (null == element) throw new ArgumentNullException("element");

			this.filePath = element.FilePath;
			this.type = element.Type;
			this.enableWmi = element.EnableWmi;
			this.enableGroupPolicies = element.EnableGroupPolicies;
			this.applicationName = element.ApplicationName;
		}

		/// <summary>
		/// 
		/// </summary>
		[SRCategory("CategoryGeneral", typeof(Resources))]
		[SRDescription("TypeNameDescription", typeof(Resources))]
		[ReadOnly(true)]
		public Type Type
		{
			get { return type; }
		}

		/// <summary>
		/// 
		/// </summary>
		[SRCategory("CategoryGeneral", typeof(Resources))]
		[SRDescription("ManageableSourceFileDescription", typeof(Resources))]
		[Editor(typeof(SaveFileEditor), typeof(UITypeEditor))]
		[FilteredFileNameEditor(typeof(Resources), "ConfigurationFileDialogFilter")]
		[ExternalConfigurationFileStorageCreation]
		[FileValidationAttribute]
		[Required]
		public string File
		{
			get { return filePath; }
			set { filePath = value; }

		}

		/// <summary>
		/// 
		/// </summary>
		[SRCategory("CategoryGeneral", typeof(Resources))]
		[SRDescription("ManageableSourceEnableWmiDescription", typeof(Resources))]
		public bool EnableWmi
		{
			get { return enableWmi; }
			set { enableWmi = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		[SRCategory("CategoryGeneral", typeof(Resources))]
		[SRDescription("ManageableSourceEnableGPDescription", typeof(Resources))]
		public bool EnableGroupPolicies
		{
			get { return enableGroupPolicies; }
			set { enableGroupPolicies = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		[SRCategory("CategoryGeneral", typeof(Resources))]
		[SRDescription("ManageableSourceApplicationNameDescription", typeof(Resources))]
		[Required]
		[MinimumLength(ManageableConfigurationSourceElement.MinimumApplicationNameLength)]
		[MaximumLength(ManageableConfigurationSourceElement.MaximumApplicationNameLength)]
		public String ApplicationName
		{
			get { return applicationName; }
			set { applicationName = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		public override IConfigurationSource ConfigurationSource
		{
			get
			{
				string file = GetFileToSave();
				FileConfigurationSource.ResetImplementation(file, false);

				return new FileConfigurationSource(file);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public override ConfigurationSourceElement ConfigurationSourceElement
		{
			get
			{
				return new ManageableConfigurationSourceElementBuilder(this).Build();
			}
		}

		/// <summary>
		/// 
		/// </summary>
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
