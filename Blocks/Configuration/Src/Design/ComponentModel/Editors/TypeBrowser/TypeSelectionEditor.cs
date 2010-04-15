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
using System.Linq;
using System.Security.Permissions;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Editors
{
    /// <summary>
    /// Editor for type name properties that opens a type browser.
    /// </summary>
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public class TypeSelectionEditor : UITypeEditor
    {
        /// <summary>
        /// Edits the value of the specified object.
        /// </summary>
        /// <param name="context">An <see cref="ITypeDescriptorContext"/> that can be used to gain
        /// additional context information.</param>
        /// <param name="provider">An <see cref="IServiceProvider"/> that this editor can use to obtain services.</param>
        /// <param name="value">The object to edit.</param>
        /// <returns>The new value of the object. If the value of the object has not changed, this should return the same object it was passed.</returns>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            var baseTypeAttribute = GetBaseTypeAttribute(context);
            var constraint =
                    new TypeBuildNodeConstraint(
                        baseTypeAttribute.BaseType,
                        baseTypeAttribute.ConfigurationType,
                        baseTypeAttribute.TypeSelectorIncludes);

            var model = new TypeBrowserViewModel(constraint, provider);
            var window =
                new TypeBrowser(model, (IAssemblyDiscoveryService)provider.GetService(typeof(IAssemblyDiscoveryService)));

            var service = (IUIServiceWpf)provider.GetService(typeof(IUIServiceWpf));
            if (service != null)
            {
                service.ShowDialog(window);
            }
            else
            {
                window.ShowDialog();
            }

            if (window.DialogResult.HasValue && window.DialogResult.Value)
            {
                return window.SelectedType != null ? window.SelectedType.AssemblyQualifiedName : null;
            }

            return value;
        }

        /// <summary>
        /// Gets the editor style used by the <seealso cref="UITypeEditor.EditValue(ITypeDescriptorContext, IServiceProvider, object)"/> method.
        /// </summary>
        /// <param name="context">
        /// An <see cref="ITypeDescriptorContext"/> that can be used to gain additional context information
        /// </param>
        /// <returns>
        /// <see cref="UITypeEditorEditStyle.Modal"/> for this editor.
        /// </returns>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        /// <devdoc>
        /// Get the base type of the object to use to filter possible types.
        /// </devdoc>        
        private static BaseTypeAttribute GetBaseTypeAttribute(ITypeDescriptorContext context)
        {
            BaseTypeAttribute attribute = null;

            if (context.PropertyDescriptor != null)
            {
                attribute = context.PropertyDescriptor.Attributes.OfType<BaseTypeAttribute>().FirstOrDefault();
            }

            return attribute ?? new BaseTypeAttribute(typeof(object), TypeSelectorIncludes.All);
        }
    }
}
