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
using System.Windows;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{
    /// <summary>
    /// A <see cref="FrameworkEditorBindableProperty"/> class specific to environmentally overridden properties.<br/>
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
    public class EnvironmentAwareFrameworkEditorBindableProperty : FrameworkEditorBindableProperty
    {
        readonly EnvironmentSourceViewModel environment;
        readonly Type frameworkElementEditorType;


        /// <summary>
        /// initializes a new instance of <see cref="FrameworkEditorBindableProperty"/>.
        /// </summary>
        /// <param name="property">The underlying <see cref="Property"/> instance.</param>
        /// <param name="frameworkElementEditorType">The type of the control that should be used to edit the underlying <see cref="Property"/>. This type must derive from <see cref="FrameworkElement"/>.</param>
        /// <param name="environment">The environment for which this the editor should be initialized.</param>
        public EnvironmentAwareFrameworkEditorBindableProperty(Property property, Type frameworkElementEditorType, EnvironmentSourceViewModel environment)
            : base(property, frameworkElementEditorType)
        {
            this.environment = environment;
            this.frameworkElementEditorType = frameworkElementEditorType;
        }


        /// <summary>
        /// Creates a new editor instance for the underlying <see cref="Property"/>.
        /// </summary>
        /// <returns>a new editor instance for the underlying <see cref="Property"/>.</returns>
        public override FrameworkElement CreateEditorInstance()
        {
            FrameworkElement editor = (FrameworkElement)Activator.CreateInstance(frameworkElementEditorType);

            var environmentAwareEditor = editor as IEnvironmentalOverridesEditor;
            if (environmentAwareEditor != null)
            {
                environmentAwareEditor.Initialize(environment);
            }

            editor.DataContext = this;

            return editor;
        }
    }
}
