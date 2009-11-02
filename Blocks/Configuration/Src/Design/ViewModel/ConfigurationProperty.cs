using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Microsoft.Practices.Unity;

namespace Console.Wpf.ViewModel
{
    public class ConfigurationProperty : ElementProperty
    {
        [InjectionConstructor]
        public ConfigurationProperty(IServiceProvider serviceProvider, ElementViewModel parent, PropertyDescriptor declaringProperty)
            : base(serviceProvider, parent, declaringProperty)
        {
        }

        public ConfigurationProperty(IServiceProvider serviceProvider, ElementViewModel parent, PropertyDescriptor declaringProperty, IEnumerable<Attribute> additionalAttributes)
            : base(serviceProvider, parent, declaringProperty, additionalAttributes)
        {
        }


        protected override object GetUnConvertedValueDirect()
        {
            var element = this.DeclaringElement.ConfigurationElement;
            var elementProperty = element.ElementInformation.Properties[base.ConfigurationName];
            return elementProperty.Value;
        }

        protected override void SetConvertedValueDirect(object value)
        {
            var element = this.DeclaringElement.ConfigurationElement;
            var elementProperty = element.ElementInformation.Properties[base.ConfigurationName];
            elementProperty.Value = value;

            OnPropertyChanged("Value");
        }


        public override bool ReadOnly
        {
            get
            {
                return false;
            }
        }
    }
}
