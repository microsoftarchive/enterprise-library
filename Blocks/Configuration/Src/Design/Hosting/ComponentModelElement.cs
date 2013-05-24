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
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.ComponentModel;
using Microsoft.Practices.Unity;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Hosting
{
    /// <summary>
    /// <see cref="IComponent"/> implemention for <see cref="ElementViewModel"/> classes.
    /// </summary>
    public class ComponentModelElement : IComponent, ICustomTypeDescriptor , INotifyPropertyChanged
    {
        ViewModelTypeDescriptorProxy typeDescriptorProxy;
        SiteImpl site;
        
        /// <summary>
        /// Initializes a new instance of <see cref="ComponentModelElement"/>.
        /// </summary>
        /// <param name="elementViewModel">The <see cref="ElementViewModel"/> that will be represented by this <see cref="IComponent"/>.</param>
        /// <param name="serviceProvider">A <see cref="IServiceProvider"/> instance that will be used to obtain services.</param>
        public ComponentModelElement(ElementViewModel elementViewModel, IServiceProvider serviceProvider)
        {
            this.site = new SiteImpl(this, serviceProvider);    
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

        object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor propertyDescriptor)
        {
            return typeDescriptorProxy.GetPropertyOwner(propertyDescriptor);
        }

        #endregion



        #region IComponent Members

        /// <summary>
        /// Occurs when the <see cref="IComponent"/> is disposed.
        /// </summary>
        public event EventHandler Disposed;

        private void OnDisposed()
        {
            var handlers = Disposed;
            if (handlers != null)
            {
                handlers(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets the <see cref="ISite"/> of the Component.
        /// </summary>
        /// <remarks>
        /// This <see cref="IComponent"/> implementation does not allow the site to be set.
        /// </remarks>
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

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="ComponentModelElement"/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing"><see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources. </param>
        protected virtual void Dispose(bool disposing)
        {
            OnDisposed();    
        }

        /// <summary>
        /// Releases all resources used by the <see cref="ComponentModelElement"/>.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
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

        /// <summary>
        /// Occurs when a property value changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
