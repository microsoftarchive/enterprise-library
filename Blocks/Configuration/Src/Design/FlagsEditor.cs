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
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{

    /// <summary>
    /// Editor for flag enums.
    /// </summary>
    [System.Security.Permissions.PermissionSetAttribute(System.Security.Permissions.SecurityAction.InheritanceDemand, Name = "FullTrust")]
    [System.Security.Permissions.PermissionSetAttribute(System.Security.Permissions.SecurityAction.LinkDemand, Name = "FullTrust")]
    public class FlagsEditor : UITypeEditor
    {
        /// <summary>
		/// Edits a flags enum value.
        /// </summary>
		/// <param name="context">An <see cref="System.ComponentModel.ITypeDescriptorContext"/> that can be used to gain additional context information.</param>
		/// <param name="provider">An <see cref="IServiceProvider"/> that this editor can use to obtain services.</param>
		/// <param name="value">The object to edit.</param>
        /// <returns>The new value, if changed; otherwise <paramref name="value"/>.</returns>
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (null == context) return null;
            if (null == provider) return null;

            IWindowsFormsEditorService service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (service != null)
            {
                Type enumType = context.PropertyDescriptor.PropertyType;
                if (!enumType.IsEnum) return null;

                int currentValue = Convert.ToInt32(value);
                using (FlagsEditorUI editorUI = new FlagsEditorUI(enumType, currentValue))
                {
                    editorUI.Close += delegate(object sender, EventArgs args) { service.CloseDropDown(); };
                    service.DropDownControl(editorUI);

                    return editorUI.Value;
                }
            }

            return value;
        }

        /// <summary>
		/// Gets the editor style.
        /// </summary>
		/// <returns>The <see cref="UITypeEditorEditStyle.DropDown"/> value.</returns>
        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }
    }
}
