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
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Editors
{
    public class TypeSelectionEditor : UITypeEditor
    {
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            var currentType = value as Type;

            var baseTypeAttribute = GetBaseTypeAttribute(context);
            var constraint =
                    new TypeBuildNodeConstraint(
                        baseTypeAttribute.BaseType,
                        baseTypeAttribute.ConfigurationType,
                        baseTypeAttribute.TypeSelectorIncludes);

            var assemblyGroups = GetAssemblyGroups(context);

            var model = new TypeBrowserViewModel(assemblyGroups, constraint.Matches);

            var window = new TypeBrowser { DataContext = model };
            window.ShowDialog();

            if (window.DialogResult.HasValue && window.DialogResult.Value)
            {
                return model.ConcreteType != null ? model.ConcreteType.AssemblyQualifiedName : null;
            }

            return value;
        }

        private IEnumerable<AssemblyGroup> GetAssemblyGroups(ITypeDescriptorContext context)
        {
            return new[] { new AssemblyGroup("all", AppDomain.CurrentDomain.GetAssemblies()) };
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

            return attribute ?? new BaseTypeAttribute(typeof(object));
        }
    }

    public interface IAssemblyProvider
    {

    }
}
