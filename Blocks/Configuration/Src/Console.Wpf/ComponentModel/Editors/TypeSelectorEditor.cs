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
using System.Drawing.Design;
using System.Threading;
using System.Reflection;
using System.Windows.Forms.Design;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using System.ComponentModel;
using System.Security;
using System.Security.Permissions;

namespace Console.Wpf.ComponentModel.Editors
{
    /// <summary>
    /// Provides a user interface for seleting a <see cref="Type"/>.
    /// </summary>
    /// <remarks>
    /// <see cref="TypeSelectorEditor"/> is a <see cref="UITypeEditor"/> that provides a dialog box for selecting a <see cref="Type"/>.
    /// </remarks>
    [PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
    [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
    public class TypeSelectorEditor : UITypeEditor
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="TypeSelectorEditor"/> class.
        /// </summary>
        public TypeSelectorEditor()
            : base()
        {
        }

        /// <summary>
        /// Edits the specified object's value using the editor style indicated by <seealso cref="UITypeEditor.GetEditStyle()"/>.
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
        /// The fully qualifed type name for the chosen type.
        /// </returns>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            Debug.Assert(provider != null, "No service provider; we cannot edit the value");
            if (provider != null)
            {
                IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

                Debug.Assert(edSvc != null, "No editor service; we cannot edit the value");
                if (edSvc != null)
                {
                    IWindowsFormsEditorService service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                    Type currentType = GetTypeToEdit(value);

                    BaseTypeAttribute baseTypeAttribute = GetBaseType(context);
                    using (TypeSelectorUI form = new TypeSelectorUI(currentType, baseTypeAttribute.BaseType, baseTypeAttribute.TypeSelectorIncludes, baseTypeAttribute.ConfigurationType))
                    {
                        if (service.ShowDialog(form) == DialogResult.OK)
                        {
                            if (form.SelectedType != null)
                            {
                                currentType = form.SelectedType;
                            }
                        }
                    }
                    if (currentType != null)
                    {
                        return currentType.AssemblyQualifiedName;
                    }
                }
            }
            return value;
        }

        private static Type GetTypeToEdit(object value)
        {
            string typeName = value as string;
            if (typeName != null)
            {
                using (CurrentThreadGetTypeHelper helper = new CurrentThreadGetTypeHelper())
                {
                    return helper.GetType(typeName);
                }
            }
            else
            {
                return value as Type;
            }
        }

        private class CurrentThreadGetTypeHelper : IDisposable
        {
            private Thread targetThread;

            public CurrentThreadGetTypeHelper()
            {
                targetThread = Thread.CurrentThread;
                AppDomain.CurrentDomain.AssemblyResolve += OnAssemblyResolve;
            }

            public Type GetType(string typeName)
            {
                return Type.GetType(typeName, false);
            }

            public void Dispose()
            {
                targetThread = null;
                AppDomain.CurrentDomain.AssemblyResolve -= OnAssemblyResolve;
            }

            private Assembly OnAssemblyResolve(object sender, ResolveEventArgs args)
            {
                if (Thread.CurrentThread == targetThread)
                {
                    foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                    {
                        // we only care about exact matches for existing assemblies.
                        if (assembly.GetName().ToString() == args.Name)
                        {
                            return assembly;
                        }
                    }
                }

                return null;
            }
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
        private static BaseTypeAttribute GetBaseType(ITypeDescriptorContext context)
        {
            BaseTypeAttribute baseTypeAttribute = null;
            foreach (Attribute attribute in context.PropertyDescriptor.Attributes)
            {
                baseTypeAttribute = attribute as BaseTypeAttribute;
                if (null != baseTypeAttribute)
                {
                    break;
                }
            }
            if (baseTypeAttribute == null)
            {
                throw new InvalidOperationException("TODO");
            }
            return baseTypeAttribute;
        }
    }
}
