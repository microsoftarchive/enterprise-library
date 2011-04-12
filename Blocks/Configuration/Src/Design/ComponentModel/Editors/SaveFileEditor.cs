using System;
using System.Collections.Generic;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Win32;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Editors
{
    /// <summary>
    /// A save file dialog type editor that returns the selected filename.
    /// </summary>
    public class SaveFileEditor : UITypeEditor
    {
        private SaveFileEditorAttribute editorAttribute;

        /// <summary>
        /// Opens a SaveFileDialog.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="provider"></param>
        /// <param name="value"></param>
        /// <returns>The selected filename</returns>
        /// <remarks>Additionnal properties can be specified by adding SaveFileEditorAttribute to the property using the SaveFileEditor.</remarks>
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            string file = value as string;

            Property property = context as Property;
            if (property != null)
            {
                editorAttribute = property.Attributes.OfType<SaveFileEditorAttribute>().FirstOrDefault();
            }

            var dialog = new SaveFileDialog()
            {
                Title = Resources.SaveConfigurationFileDialogTitle,
                FilterIndex = 0
            };

            if (this.editorAttribute != null)
            {
                dialog.CheckFileExists = this.editorAttribute.CheckFileExists;
                dialog.OverwritePrompt = this.editorAttribute.OverwritePrompt;
                dialog.Filter = this.editorAttribute.Filter;

                // If a valid default filename was provided 
                if (!string.IsNullOrEmpty(file) && 
                    Path.GetInvalidPathChars().All(invalidPathChar => !file.Contains(invalidPathChar)))
                {
                    dialog.InitialDirectory = Path.GetDirectoryName(Path.GetFullPath(file));
                    dialog.FileName = Path.GetFileNameWithoutExtension(file);
                }
            }

            var saveDialogResult = dialog.ShowDialog();

            if (saveDialogResult != true) return file;

            return dialog.FileName;
        }

        /// <summary>
        /// Gets the editor style used by <see cref="SaveFileEditor"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="UITypeEditorEditStyle.Modal"/>.
        /// </returns>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that can be used to gain additional context information. 
        /// </param>
        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
    }
}
