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
using System.Windows.Input;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Commands;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{
    /// <summary>
    /// A <see cref="BindableProperty"/> implementation that allows the user to interact with the underlying <see cref="Property"/>'s value through a model popup editor.
    /// </summary>
    /// <remarks>
    /// <see cref="Property"/> instances are shown in the designer as well in Visual Studio's property grid.<br/>
    /// Threrefore the <see cref="BindableProperty"/> class derives from <see cref="PropertyDescriptor"/>.
    /// </remarks>
    /// <seealso cref="Property"/>
    /// <seealso cref="BindableProperty"/>
    public class PopupEditorBindableProperty : BindableProperty
    {
        readonly Property property;
        readonly UITypeEditor popupEditor;
        readonly bool textReadOnly;

        /// <summary>
        /// Initializes a new instance of <see cref="PopupEditorBindableProperty"/>.
        /// </summary>
        /// <param name="property">The underlying <see cref="Property"/> instance.</param>
        public PopupEditorBindableProperty(Property property)
            : base(property)
        {
            this.property = property;

            this.popupEditor = property.Attributes
                        .OfType<EditorAttribute>()
                        .Where(x => Type.GetType(x.EditorBaseTypeName, false) == typeof(UITypeEditor))
                        .Select(x => Type.GetType(x.EditorTypeName))
                        .Select(x => Activator.CreateInstance(x))
                        .Cast<UITypeEditor>()
                        .First();

            textReadOnly = property.Attributes.OfType<EditorWithReadOnlyTextAttribute>().Where(x => x.ReadonlyText).Any();
        }

        /// <summary>
        /// Gets the <see cref="UITypeEditor"/> implementation that can be used to edit the underlying <see cref="Property"/>'s value.
        /// </summary>
        /// <value>
        /// The <see cref="UITypeEditor"/> implementation that can be used to edit the underlying <see cref="Property"/>'s value.
        /// </value>
        public UITypeEditor PopupEditor
        {
            get
            {
                return popupEditor;
            }
        }

        /// <summary>
        /// Gets an <see cref="ICommand"/> implementation that can be used to open the underlying <see cref="Property"/>'s editor.
        /// </summary>
        /// <value>
        /// An <see cref="ICommand"/> implementation that can be used to open the underlying <see cref="Property"/>'s editor.
        /// </value>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public ICommand LaunchEditor
        {
            get
            {
                return new DelegateCommand(arg =>
                    {
                        try
                        {
                            property.Value = PopupEditor.EditValue(property, property, property.Value);
                        }
                        catch (Exception e)
                        {
                            var uiService = (IUIServiceWpf)((IServiceProvider)this.property).GetService(typeof(IUIServiceWpf));
                            uiService.ShowError(e, Resources.ErrorOpeningEditor);
                        }
                    }
                    );
            }
        }

        /// <summary>
        /// Gets whether this property allows user input.
        /// </summary>
        /// <returns>
        /// false if this property allows user input; Otherwise true.
        /// </returns>
        public bool TextReadOnly
        {
            get { return textReadOnly; }
        }

        /// <summary>
        /// Gets an editor of the specified type.
        /// </summary>
        /// <returns>
        /// If <paramref name="editorBaseType"/> is <see cref="UITypeEditor"/> returns the <see cref="PopupEditor"/>; Otherwise <see langword="null"/>.
        /// </returns>
        /// <param name="editorBaseType">The base type of editor, which is used to differentiate between multiple editors that a property supports. </param>
        public override object GetEditor(Type editorBaseType)
        {
            if (!base.IsReadOnly)
            {
                if (editorBaseType == typeof(UITypeEditor))
                {
                    return PopupEditor;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets whether this property is read-only.
        /// </summary>
        /// <returns>
        /// true if the property is read-only; otherwise, false.
        /// </returns>
        public override bool IsReadOnly
        {
            get
            {
                return TextReadOnly || base.IsReadOnly;
            }
        }
    }
}
