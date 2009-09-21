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

using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Properties;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;

using DesigntimeResource = Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design.Resources;
using RuntimeResources = Microsoft.Practices.EnterpriseLibrary.Common.Properties.Resources;
using System.ComponentModel;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    /// <summary>
    /// Represents the configuration settings that describe a <see cref="FileConfigurationSource"/>.
    /// </summary>
    [ResourceDisplayName(typeof(DesigntimeResource), "FileConfigurationSourceElementDisplayName")]
    [ResourceDescription(typeof(DesigntimeResource), "FileConfigurationSourceElementDescription")]
    public class FileConfigurationSourceElement : ConfigurationSourceElement
    {
        private const string filePathProperty = "filePath";

        /// <summary>
		/// Initializes a new instance of the <see cref="FileConfigurationSourceElement"/> class with a default name and an empty path.
        /// </summary>
        public FileConfigurationSourceElement()
            : this(RuntimeResources.FileConfigurationSourceName, string.Empty)
        {
        }

        /// <summary>
		/// Initializes a new instance of the <see cref="FileConfigurationSourceElement"/> class with a name and an path.
		/// </summary>
        /// <param name="name">The instance name.</param>
        /// <param name="filePath">The file path.</param>
        public FileConfigurationSourceElement(string name, string filePath)
            : base(name, typeof(FileConfigurationSource))
		{
            this.FilePath = filePath;
        }


        /// <summary>
        /// Gets or sets the file path. This is a required field.
        /// </summary>
        [ConfigurationProperty(filePathProperty, IsRequired = true)]
        [ResourceDisplayName(typeof(DesigntimeResource), "FileConfigurationSourceElementFilePathDisplayName")]
        [ResourceDescription(typeof(DesigntimeResource), "FileConfigurationSourceElementFilePathDescription")]
        [Editor(EditorTypes.FilteredFilePath, EditorTypes.UITypeEditor)]
        [FilteredFileNameEditorAttribute(typeof(DesigntimeResource), "FileConfigurationSourceElementFilePathFilter")]
        public string FilePath
        {
            get { return (string)this[filePathProperty]; }
            set { this[filePathProperty] = value; }
        }

		/// <summary>
		/// Returns a new <see cref="FileConfigurationSource"/> configured with the receiver's settings.
		/// </summary>
		/// <returns>A new configuration source.</returns>
		public override IConfigurationSource CreateSource()
		{
			IConfigurationSource createdObject = new FileConfigurationSource(FilePath);

			return createdObject;
		}
    }
}
