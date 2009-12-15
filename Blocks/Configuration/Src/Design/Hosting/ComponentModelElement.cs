using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.ComponentModel;
using Microsoft.Practices.Unity;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Hosting
{
    public class ComponentModelElement : IComponent, ICustomTypeDescriptor , INotifyPropertyChanged
    {
        ElementViewModel elementViewModel;
        ViewModelTypeDescriptorProxy typeDescriptorProxy;
        SiteImpl site;

        public ComponentModelElement(ElementViewModel elementViewModel, IServiceProvider serviceProvider)
        {
            this.site = new SiteImpl(this, serviceProvider);    
            this.elementViewModel = elementViewModel;
            this.typeDescriptorProxy = new ViewModelTypeDescriptorProxy(elementViewModel);

            foreach (PropertyDescriptor property in ((ICustomTypeDescriptor)this).GetProperties())
            {
                property.AddValueChanged(this, (sender, args) => 
                {
                    var handler = PropertyChanged;
                    if (handler != null)
                    {
                        handler(this, new PropertyChangedEventArgs(property.Name) );
                    }
                });
            }
        }


        #region custom type desciptor implementation

        AttributeCollection ICustomTypeDescriptor.GetAttributes()
        {
            return typeDescriptorProxy.GetAttributes();
        }

        string ICustomTypeDescriptor.GetClassName()
        {
            return typeDescriptorProxy.GetClassName();
        }

        string ICustomTypeDescriptor.GetComponentName()
        {
            return typeDescriptorProxy.GetComponentName();
        }

        TypeConverter ICustomTypeDescriptor.GetConverter()
        {
            return typeDescriptorProxy.GetConverter();
        }

        EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
        {
            return typeDescriptorProxy.GetDefaultEvent();
        }

        PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
        {
            return typeDescriptorProxy.GetDefaultProperty();
        }

        object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
        {
            return typeDescriptorProxy.GetEditor(editorBaseType);
        }

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
        {
            return typeDescriptorProxy.GetEvents(attributes);
        }

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
        {
            return typeDescriptorProxy.GetEvents();
        }

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
        {
            return typeDescriptorProxy.GetProperties(attributes);
        }

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
        {
            return typeDescriptorProxy.GetProperties();
        }

        object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
        {
            return typeDescriptorProxy.GetPropertyOwner(pd);
        }

        #endregion



        #region IComponent Members

        public event EventHandler Disposed;

        public ISite Site
        {
            get
            {
                return site;
            }
            set
            {
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion

        private class SiteImpl : ISite
        {
            IComponent component;
            IServiceProvider serviceProvider;
            
            public SiteImpl(IComponent component, IServiceProvider serviceProvider)
            {
                this.component = component;
                this.serviceProvider = serviceProvider;
            }

            #region ISite Members

            public IComponent Component
            {
                get { return component; }
            }

            public IContainer Container
            {
                get { return null; }
            }

            public bool DesignMode
            {
                get { return true ; }
            }

            public string Name
            {
                get
                {
                    return string.Empty;
                }
                set
                {
                }
            }

            #endregion

            #region IServiceProvider Members

            public object GetService(Type serviceType)
            {
                try
                {
                    return serviceProvider.GetService(serviceType);
                }
                catch (ResolutionFailedException)
                {
                    return null;
                }
            }

            #endregion
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
