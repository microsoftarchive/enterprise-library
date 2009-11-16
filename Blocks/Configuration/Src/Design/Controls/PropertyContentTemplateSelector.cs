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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Controls
{
    public class PropertyContentTemplateSelector : DataTemplateSelector
    {
        public override System.Windows.DataTemplate SelectTemplate(object item, System.Windows.DependencyObject container)
        {
            var property = item as Property;

            if (property == null) return base.SelectTemplate(item, container); 

            string templateKey = "TextPropertyEditingTemplate";
            if (property.HasSuggestedValues)
            {
                templateKey = "SuggestedValuePropertyEditingTemplate";
            }
            else if (property.HasEditor && property.EditorBehavior == EditorBehavior.DropDown)
            {
                templateKey = "DropDownEditorPropertyTemplate";
            }

            return (DataTemplate) System.Windows.Application.Current.MainWindow.FindResource(templateKey);
        }
    }
}
