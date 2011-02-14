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
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;

namespace Microsoft.Practices.Unity.Configuration.Design.Editors
{
    public class PopupCollectionEditor : UITypeEditor
    {
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            BindableProperty bindableProperty = (BindableProperty)context.PropertyDescriptor;
            ElementProperty property = (ElementProperty)bindableProperty.Property;
            ElementCollectionViewModel collection = (ElementCollectionViewModel)property.DeclaringElement.ChildElement(property.DeclaringProperty.Name);
            IUIServiceWpf uiService = (IUIServiceWpf)context.GetService(typeof(IUIServiceWpf));

            PopupCollectionEditorWindow w = new PopupCollectionEditorWindow()
            {
                ContainedContent = new HeaderedListLayout(collection, collection.AddCommands)
            };

            uiService.ShowDialog(w);

            return value;
        }

        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
    }
}
