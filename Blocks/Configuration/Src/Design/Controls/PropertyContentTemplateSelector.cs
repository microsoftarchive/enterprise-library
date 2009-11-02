using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Console.Wpf.ViewModel;

namespace Console.Wpf.Controls
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
