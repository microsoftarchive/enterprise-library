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
using System.Windows;
using System.Windows.Forms.Design;
using System.Windows.Forms.Integration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using System.ComponentModel;
using Winforms=System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Forms;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Hosting
{
    public class FrameworkElementUITypeEditor : UITypeEditor
    {
        Property property;
        Winforms.UserControl hosthost;
        ElementHost host;
        ScrollViewer scrollViewer;

        public FrameworkElementUITypeEditor(Property property)
        {
            this.property = property;


            var editor = property.Attributes.OfType<EditorAttribute>().Where(x => Type.GetType(x.EditorBaseTypeName) == typeof(FrameworkElement)).FirstOrDefault();

            Type editorType = Type.GetType(editor.EditorTypeName);
            FrameworkElement editorInstance = property.CreateCustomVisual();
            editorInstance.DataContext = property;
            scrollViewer = new ScrollViewer { HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch, VerticalAlignment = VerticalAlignment.Stretch };
            scrollViewer.Content = editorInstance;
            host = new ElementHost { Child = scrollViewer, Dock = DockStyle.Fill };

            host.Resize += new EventHandler(host_Resize);
            hosthost = new Winforms.UserControl();
            hosthost.Controls.Add(host);
        }

        void host_Resize(object sender, EventArgs e)
        {
            scrollViewer.UpdateLayout();
        }

        public override bool IsDropDownResizable
        {
            get { return true; }
        }

        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService editorSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            editorSvc.DropDownControl(hosthost);
            return context.PropertyDescriptor.GetValue(property);    
        }
    }
}
