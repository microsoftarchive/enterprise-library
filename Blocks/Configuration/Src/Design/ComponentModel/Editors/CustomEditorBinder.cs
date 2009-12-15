using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Converters;
using System.Windows.Data;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Editors
{
    public class CustomEditorBinder
    {
        public static void BindProperty(FrameworkElement element, BindableProperty property)
        {
            var enabledBinding = new Binding("ReadOnly");
            enabledBinding.Converter = new BooleanInverseConverter();
            enabledBinding.Source = property;
            element.SetBinding(UIElement.IsEnabledProperty, enabledBinding);

        }
    }
}
