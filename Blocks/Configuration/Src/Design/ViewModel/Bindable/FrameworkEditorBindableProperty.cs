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
using System.Windows;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Hosting;
using System.Collections.Generic;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{
    /// <summary>
    /// A <see cref="BindableProperty"/> class that allow's the UI (User Interface) to interact with the underlying <see cref="Property"/> though a custom control (e.g. a date picker, rather than the default textbox).
    /// </summary>
    /// <remarks>
    /// <para>
    /// <see cref="Property"/> instances are shown in the designer as well in Visual Studio's property grid.<br/>
    /// Threrefore this <see cref="SuggestedValuesBindableProperty"/> class derives from <see cref="PropertyDescriptor"/>.
    /// </para>
    /// <para>
    /// In the Visual Studio Property Grid properties with a custom control will be shown as a drop down.
    /// </para>
    /// </remarks>
    /// <seealso cref="BindableProperty"/>
    /// <seealso cref="Property"/>
    /// <seealso cref="Property.SuggestedValues"/>
    public class FrameworkEditorBindableProperty : BindableProperty
    {
        readonly Property property;
        readonly Type frameworkElementEditorType;

        FrameworkElementUITypeEditor cachedComponentModelEditor;
        List<FrameworkElement> createdElementReferences = new List<FrameworkElement>();

        /// <summary>
        /// initializes a new instance of <see cref="FrameworkEditorBindableProperty"/>.
        /// </summary>
        /// <param name="property">The underlying <see cref="Property"/> instance.</param>
        /// <param name="frameworkElementEditorType">The type of the control that should be used to edit the underlying <see cref="Property"/>. This type must derive from <see cref="FrameworkElement"/>.</param>
        public FrameworkEditorBindableProperty(Property property, Type frameworkElementEditorType)
            : base(property)
        {
            this.property = property;
            this.frameworkElementEditorType = frameworkElementEditorType;
        }

        /// <summary>
        /// Creates a new editor instance for the underlying <see cref="Property"/>.
        /// </summary>
        /// <returns>a new editor instance for the underlying <see cref="Property"/>.</returns>
        public virtual FrameworkElement CreateEditorInstance()
        {
            FrameworkElement editor = (FrameworkElement)Activator.CreateInstance(frameworkElementEditorType);

            editor.DataContext = this;
            createdElementReferences.Add(editor);
            return editor;
        }

        /// <summary>
        /// Gets a new editor instance for the underlying <see cref="Property"/>.
        /// </summary>
        /// <value>
        /// A new editor isntance for the underlying <see cref="Property"/>.
        /// </value>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods")]
        public FrameworkElement Editor
        {
            get
            {
                return CreateEditorInstance();
            }
        }
        
        /// <summary>
        /// Gets a list of the editor instances created for the underlying <see cref="Property"/>.
        /// </summary>
        /// <value>
        /// A list of the editor instances created for the underlying <see cref="Property"/>.
        /// </value>
        public IEnumerable<FrameworkElement> CreatedEditorReferences
        {
            get { return createdElementReferences; }
        }

        /// <summary>
        /// Gets an editor of the specified type.
        /// </summary>
        /// <returns>
        /// If the <paramref name="editorBaseType" /> is <see cref="UITypeEditor"/> a <see cref="UITypeEditor"/> implementation that wraps the <see cref="Editor"/> instance; Otherwise <see langword="null"/>.
        /// </returns>
        /// <param name="editorBaseType">The base type of editor, which is used to differentiate between multiple editors that a property supports. </param>
        public override object GetEditor(Type editorBaseType)
        {
            if (editorBaseType == typeof(UITypeEditor))
            {
                return cachedComponentModelEditor ??
                    (cachedComponentModelEditor = new FrameworkElementUITypeEditor(property, this));
            }

            return null;
        }
    }
}
