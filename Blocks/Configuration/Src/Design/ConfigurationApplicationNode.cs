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
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// Represents the root node of the configuration application.
    /// </summary>
    [Image(typeof(ConfigurationApplicationNode))]
    [SelectedImage(typeof(ConfigurationApplicationNode))]
    public class ConfigurationApplicationNode : ConfigurationNode
    {
        private ConfigurationApplicationFile configurationApplicationFile;

		/// <summary>
		/// Initialize a new instance of the <see cref="ConfigurationApplicationNode"/> class.
		/// </summary>
		public ConfigurationApplicationNode() : this(ConfigurationApplicationFile.FromCurrentAppDomain())
		{
            
		}

		/// <summary>
		/// Initialize a new instance of the <see cref="ConfigurationApplicationNode"/> class with a <see cref="ConfigurationApplicationFile"/> instance..
		/// </summary>
		/// <param name="configurationApplicationFile">A <see cref="ConfigurationApplicationFile"/> instance.</param>
        public ConfigurationApplicationNode(ConfigurationApplicationFile configurationApplicationFile) : base()
        {
			if (configurationApplicationFile == null) throw new ArgumentNullException("configurationApplicationFile"); 

            this.configurationApplicationFile = configurationApplicationFile;
            Rename(string.IsNullOrEmpty(configurationApplicationFile.ConfigurationFilePath) ? Resources.ApplicationNodeName : configurationApplicationFile.ConfigurationFilePath);
        }


		/// <summary>
		/// Gets the name of the node.
		/// </summary>		
		/// <value>
		/// The name of the node.
		/// </value>
		[Browsable(false)]
		[ReadOnly(true)]
		public override string Name
		{
			get
			{
				return base.Name;
			}			
		}
        
        /// <summary>
        /// Gets or sets the application's configuration file.
        /// </summary>
		/// <value>
		/// The application's configuration file.
		/// </value>
		[SRCategory("CategoryGeneral", typeof(Resources))]
		[SRDescription("ConfigurationFilePathDescription", typeof(Resources))]
		[ReadOnly(true)]
        [FileValidationAttribute]
		public virtual string ConfigurationFile
		{
			get { return configurationApplicationFile.ConfigurationFilePath; }
			set 
			{ 
				configurationApplicationFile.ConfigurationFilePath = value;
				Name = value;
			}
		}
    }
}