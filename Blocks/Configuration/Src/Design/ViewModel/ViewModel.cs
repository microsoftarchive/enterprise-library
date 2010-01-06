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
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{

    //base class for everything that can be rendered in the grid.
    //actual elements, collections, sections and even headers derive from this.
    public class ViewModel 
    {
        public ViewModel()
        {
        }

        private object bindable;

        protected virtual object CreateBindable()
        {
            return this;
        }

        public object Bindable
        {
            get
            {
                return (bindable == null) ?
                    bindable = CreateBindable() :
                    bindable;
            }
        }

        public Type CustomVisualType { get; set; }

        public virtual FrameworkElement CreateCustomVisual()
        {
            if (CustomVisualType == null) return null;
            var visualElement = (FrameworkElement)Activator.CreateInstance(CustomVisualType);
            visualElement.DataContext = Bindable;
            return visualElement;
        }

        public FrameworkElement Visual
        {
            get
            {
                var visual = CreateCustomVisual();
                if (visual == null)
                {
                    visual = new ContentControl()
                    {
                        Focusable = false,
                        Content = Bindable
                    };
                }
                return visual;
            }
        }
    }
}
