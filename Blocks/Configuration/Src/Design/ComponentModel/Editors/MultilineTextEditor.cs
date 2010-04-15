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
using System.Configuration;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Editors
{
    /// <summary>
    /// The <see cref="MultilineTextEditor"/> provides an in-line multi-line text editor for a <see cref="BindableProperty"/>.
    /// </summary>
    /// <remarks>
    /// This may be applied to a <see cref="string"/> property on a <see cref="ConfigurationElement"/>
    /// with the <see cref="EditorAttribute"/> and specifying <see cref="CommonDesignTime.EditorTypes.MultilineText"/>
    /// for the <see cref="EditorAttribute.EditorTypeName"/>.
    /// <code>[Editor(CommonDesignTime.EditorTypes.MultilineText, CommonDesignTime.EditorTypes.FrameworkElement)]</code>
    /// </remarks>
    public class MultilineTextEditor : TextBox
    {
        /// <summary>
        /// Instatiates a new instance of <see cref="MultilineTextEditor"/>.
        /// </summary>
        public MultilineTextEditor()
        {
            this.AcceptsReturn = true;
            this.AcceptsTab = false;
            this.MinLines = 5;

            this.DataContextChanged += new DependencyPropertyChangedEventHandler(MultilineTextEditor_DataContextChanged);
        }

        void MultilineTextEditor_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            BindableProperty bindableProperty = DataContext as BindableProperty;
            if (bindableProperty == null) return;

            CustomEditorBinder.BindProperty(this, bindableProperty);

            var propertyBinding = new Binding("BindableValue");
            propertyBinding.Source = bindableProperty;
            propertyBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            this.SetBinding(TextBox.TextProperty, propertyBinding);
        }
    }
}
