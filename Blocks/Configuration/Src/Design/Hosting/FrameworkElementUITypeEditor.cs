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
using System.Security.Permissions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Windows.Forms.Integration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.Unity.Utility;
using Winforms = System.Windows.Forms;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Hosting
{
    /// <summary>
    /// <see cref="UITypeEditor"/> implementation that allows to edit an component's value using a <see cref="FrameworkElement"/>.
    /// </summary>
    /// <seealso cref="Property"/>
    /// <seealso cref="BindableProperty"/>
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]        
    public class FrameworkElementUITypeEditor : UITypeEditor, IDisposable
    {
        readonly Winforms.UserControl hostContainer;
        readonly ElementHost host;
        readonly ScrollViewer scrollViewer;
        private readonly Property property;

        /// <summary>
        /// Initializes a new instance of <see cref="FrameworkElementUITypeEditor"/>.
        /// </summary>
        /// <param name="property">The <see cref="Property"/> instance that should be edited.</param>
        /// <param name="bindableProperty">The <see cref="FrameworkEditorBindableProperty"/> instance that contains the UI interaction logic for <paramref name="property"/>.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Validated with Guard class")]
        public FrameworkElementUITypeEditor(Property property, FrameworkEditorBindableProperty bindableProperty)
        {
            Guard.ArgumentNotNull(bindableProperty, "bindableProperty");

            this.property = property;

            FrameworkElement editorInstance = bindableProperty.CreateEditorInstance();
            editorInstance.DataContext = bindableProperty;
            scrollViewer = new ScrollViewer { HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch, VerticalAlignment = VerticalAlignment.Stretch };
            scrollViewer.Content = editorInstance;
            host = new ElementHost { Child = scrollViewer, Dock = DockStyle.Fill };

            host.Resize += HostResize;
            hostContainer = new Winforms.UserControl();
            hostContainer.Controls.Add(host);
        }

        void HostResize(object sender, EventArgs e)
        {
            scrollViewer.UpdateLayout();
        }

        /// <summary>
        /// Gets a value indicating whether drop-down editors should be resizable by the user.
        /// </summary>
        /// <value>
        /// Always returns <see langword="true"/>.
        /// </value>
        public override bool IsDropDownResizable
        {
            get { return true; }
        }

        /// <summary>
        /// Gets the editor style used by the <see cref="EditValue"/>  method.
        /// </summary>
        /// <returns>Always returns <see cref="UITypeEditorEditStyle.DropDown"/>.</returns>
        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        /// <summary>
        /// Displays the editor, a <see cref="FrameworkElement"/>, and returns the value provided by the user.
        /// </summary>
        /// <param name="context">An <see cref="System.ComponentModel.ITypeDescriptorContext" /> instance that can be used to gain additional context information on the property being edited.</param>
        /// <param name="provider">An <see cref="IServiceProvider"/> that this editor can use to obtain services.</param>
        /// <param name="value">The value to edit.</param>
        /// <returns>The value provided by the user.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Validated with Guard class")]
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            Guard.ArgumentNotNull(provider, "provider");

            IWindowsFormsEditorService editorSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            editorSvc.DropDownControl(hostContainer);
            return context.PropertyDescriptor.GetValue(property);
        }

        #region IDisposable Members

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="FrameworkElementUITypeEditor"/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing"><see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources. </param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                host.Resize -= HostResize;
                host.Dispose();

                hostContainer.Dispose();
            }
        }

        /// <summary>
        /// Releases all resources used by the <see cref="FrameworkElementUITypeEditor"/>.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
