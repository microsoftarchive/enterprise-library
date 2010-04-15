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
using System.Linq;
using System.Text;
using System.Windows.Forms.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Editors
{
    /// <summary>
    /// Provides a user interface for selecting a file name.
    /// </summary>
    /// <seealso cref="FileNameEditor"/>
    public class FilteredFileNameEditor : FileNameEditor
    {
        private FilteredFileNameEditorAttribute editorAttribute;
        private string file;

        /// <summary>
        /// Initialize a new instance of the <see cref="FilteredFileNameEditor"/> class.
        /// </summary>
        public FilteredFileNameEditor()
        {
        }

        /// <summary>
        /// Edits the specified object using the editor style provided by the GetEditStyle method.
        /// </summary>
        /// <param name="context">
        /// An ITypeDescriptorContext that can be used to gain additional context information.
        /// </param>
        /// <param name="provider">
        /// A service provider object through which editing services may be obtained.
        /// </param>
        /// <param name="value">
        /// An instance of the value being edited.
        /// </param>
        /// <returns>
        /// The new value of the object. If the value of the object hasn't changed, this should return the same object it was passed.
        /// </returns>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            file = value as string;

            Property property = context as Property;
            if (property != null)
            {
                editorAttribute = property.Attributes.OfType<FilteredFileNameEditorAttribute>().FirstOrDefault();
            }

            return base.EditValue(context, provider, value);
        }
        
        /// <summary>
        /// Initializes the open file dialog when it is created.
        /// </summary>
        /// <param name="openFileDialog">
        /// The <see cref="OpenFileDialog"/> to use to select a file name. 
        /// </param>
        protected override void InitializeDialog(OpenFileDialog openFileDialog)
        {
            base.InitializeDialog(openFileDialog);
            if (this.editorAttribute != null)
            {
                openFileDialog.CheckFileExists = this.editorAttribute.CheckFileExists;
                openFileDialog.Filter = this.editorAttribute.Filter;
                if (!String.IsNullOrEmpty(file))
                {
                    openFileDialog.InitialDirectory = Path.GetDirectoryName(Path.GetFullPath(file));
                }
            }
        }
    }
}
