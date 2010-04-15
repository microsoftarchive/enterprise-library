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
using System.Configuration;
using System.Diagnostics;
using System.Drawing.Design;
using System.Security.Permissions;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Editors
{
    ///<summary>
    /// The <see cref="PopupTextEditor"/> provides a text value to edit in a modal window.
    ///</summary>
    /// <remarks>
    /// This editor can be applied to a <see cref="ConfigurationElement"/> property with the
    /// <see cref="EditorAttribute"/> using the <see cref="CommonDesignTime.EditorTypes.PopupTextEditor"/>
    /// for the <see cref="EditorAttribute.EditorTypeName"/>.
    /// </remarks>
    /// <seealso cref="TextEditDialog"/>
    [PermissionSet(SecurityAction.Demand, Name="FullTrust")]        
    public class PopupTextEditor : System.Drawing.Design.UITypeEditor
    {
        /// <summary>
        /// Allows the user to edit the specic value in a dialog by displaying <see cref="TextEditDialog"/> to the user.
        /// </summary>
        /// <returns>
        /// The new value of the object. If the value of the object has not changed, this should return the same object it was passed.
        /// </returns>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that can be used to gain additional context information. 
        ///                 </param><param name="provider">An <see cref="T:System.IServiceProvider"/> that this editor can use to obtain services. 
        ///                 </param><param name="value">The object to edit. 
        ///                 </param>
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            var dialog = new TextEditDialog();
            
            var bindableProperty = EditorUtility.GetBindableProperty(context);
            Debug.Assert(bindableProperty != null, "Could not locate bindable property from ITypeDescriptorContext");

            bool isTextReadOnly = EditorUtility.IsTextReadOnly(bindableProperty);
            var valueToEdit = new PopupEditorValue() {Value = value as string};

            dialog.DataContext = valueToEdit;

            var service = (IUIServiceWpf) provider.GetService(typeof(IUIServiceWpf));
            var result = service.ShowDialog(dialog);

            if (result.HasValue && result.Value)
            {   
                // We can't rely on the VS property grid to properly
                // if the property is attributed with EditorWithReadOnlyText(true)
                // so we set the bindable value ourselves.
                if (isTextReadOnly)
                    bindableProperty.Value = valueToEdit.Value;

                return valueToEdit.Value;
            }

            return value;
        }

        /// <summary>
        /// Gets the editor style used by <see cref="PopupTextEditor"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="UITypeEditorEditStyle.Modal"/>.
        /// </returns>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that can be used to gain additional context information. 
        ///                 </param>
        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
    }

    

    ///<summary>
    /// A value for binding the value in to the view.
    ///</summary>
    public class PopupEditorValue
    {
        ///<summary>
        /// The value to edit.
        ///</summary>
        public string Value { get; set; }
    }
}
