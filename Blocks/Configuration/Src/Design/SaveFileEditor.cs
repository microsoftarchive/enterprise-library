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
using System.Diagnostics;
using System.Drawing.Design;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// Provides a user interface for saving a file name.
    /// </summary>
    [System.Security.Permissions.PermissionSetAttribute(System.Security.Permissions.SecurityAction.InheritanceDemand, Name="FullTrust")]
    [System.Security.Permissions.PermissionSetAttribute(System.Security.Permissions.SecurityAction.LinkDemand, Name="FullTrust")]
    public class SaveFileEditor : UITypeEditor
    {
        private string file;
        private FilteredFileNameEditorAttribute editorAttribute;

        /// <summary>
        /// Edits the specified object's value using the editor style indicated by GetEditStyle.
        /// </summary>
        /// <param name="context">
        /// An <see cref="ITypeDescriptorContext"/> that can be used to gain additional context information.
        /// </param>
        /// <param name="provider">
        /// An <see cref="IServiceProvider"/> that this editor can use to obtain services.
        /// </param>
        /// <param name="value">
        /// The object to edit.
        /// </param>
        /// <returns>
        /// The new value of the object.
        /// </returns>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
			if (null == context) return value;
			if (null == provider) return value;

            file = value as string;
            foreach (Attribute attribute in context.PropertyDescriptor.Attributes)
            {
                editorAttribute = attribute as FilteredFileNameEditorAttribute;
                if (editorAttribute != null)
                {
                    break;
                }
            }

            using(SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                InitializeDialog(saveFileDialog);						
                if (null != file && -1 == file.IndexOfAny(Path.GetInvalidFileNameChars()))
                {
                    saveFileDialog.FileName = file;
                }

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    value = saveFileDialog.FileName;
                }
            }
            return value;
        }

        /// <summary>
        /// Gets the editor style used by the <see cref="EditValue"/> method.
        /// </summary>
        /// <param name="context">
        /// An <see cref="ITypeDescriptorContext"/> that can be used to gain additional context information.
        /// </param>
        /// <returns>
        /// <see cref="UITypeEditorEditStyle.Modal"/>
        /// </returns>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        /// <summary>
        /// Initializes the save file dialog when it is created.
        /// </summary>
        /// <param name="fileDialog">
        /// A <see cref="FileDialog"/> instance.
        /// </param>
        protected virtual void InitializeDialog(FileDialog fileDialog)
        {
            if (editorAttribute == null)
            {
                fileDialog.Filter = Resources.GenericFileFilter;
            }
            else
            {
                fileDialog.Filter = editorAttribute.Filter;
            }
            fileDialog.Title = Resources.GenericSaveFile;
            if (file != null && file.Length > 0 && -1 == file.IndexOfAny(Path.GetInvalidFileNameChars()))
            {
                fileDialog.InitialDirectory = Path.GetDirectoryName(Path.GetFullPath(file));
            }
        }
    }
}
