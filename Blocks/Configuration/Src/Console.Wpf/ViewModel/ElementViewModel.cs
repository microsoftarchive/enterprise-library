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
using System.Configuration;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Console.Wpf.ViewModel.Services;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using System.Windows.Input;
using System.Diagnostics;
using Console.Wpf.ViewModel.ComponentModel;

namespace Console.Wpf.ViewModel
{
    public class ElementViewModel : ViewModel, INotifyPropertyChanged, ICustomTypeDescriptor
    {
        IServiceProvider serviceProvider;
        ConfigurationElement parentElement;
        PropertyDescriptor declaringProperty;
        ConfigurationElement thisElement;
        NamedConfigurationElement thisElementWithName;
        IEnumerable<Attribute> metadataAttributes;
        ElementViewModel parentElementModel;
        List<ElementViewModel> childElements;
        List<Property> properties;
        ViewModelTypeDescriptorProxy typeDescriptorProxy;

        public ElementViewModel(IServiceProvider serviceProvider, ElementViewModel parentElementModel, PropertyDescriptor declaringProperty)
            : this(serviceProvider, parentElementModel, declaringProperty, declaringProperty.Attributes.OfType<Attribute>())
        {
        }

        public ElementViewModel(IServiceProvider serviceProvider, ElementViewModel parentElementModel, PropertyDescriptor declaringProperty, IEnumerable<Attribute> additonalAttributes)
            : this(serviceProvider, parentElementModel, declaringProperty.GetValue(parentElementModel.thisElement) as ConfigurationElement, additonalAttributes)
        {
            this.declaringProperty = declaringProperty;
        }


        protected ElementViewModel(IServiceProvider serviceProvider, ElementViewModel parentElementModel, ConfigurationElement thisElement, IEnumerable<Attribute> additonalAttributes)
        {
            this.typeDescriptorProxy = new ViewModelTypeDescriptorProxy(this);
            this.serviceProvider = serviceProvider;
            this.thisElement = thisElement;
            this.parentElementModel = parentElementModel;
            this.thisElementWithName = thisElement as NamedConfigurationElement;
            this.metadataAttributes = additonalAttributes;

            this.parentElement = parentElementModel == null ? null : parentElementModel.thisElement;
            
        }

        protected void PropagatePropertyValueChanged(string propertyName, string thisPropertyName)
        {
            var propertyToListenFor = Property(propertyName);
            if (propertyToListenFor != null)
            {
                propertyToListenFor.PropertyChanged += (sender, args) =>
                {
                    if (args.PropertyName == "Value")
                    {
                        this.DoPropertyChanged(thisPropertyName);
                    }
                };
            }
        }

        //property witn null-value.
        //empty leaf
        private bool IsNull
        {
            get { return thisElement == null; }
        }

        public ConfigurationElement ConfigurationElement
        {
            get { return thisElement; }
        }

        #region meta data

        protected IEnumerable<Attribute> MetadataAttributes
        {
            get { return metadataAttributes; }
        }

        protected PropertyDescriptor DeclaringProperty
        {
            get { return declaringProperty; }
        }

        //deafult implementation
        //knows about NamedElement
        //knows about typedescriptors
        public virtual string Name
        {
            get
            {
                return GetName();
            }
        }

        public virtual Type ConfigurationType
        {
            get { return thisElement.GetType(); }
        }

        public virtual IEnumerable<Property> GetExtendedProperties()
        {
            ElementLookup lookup = serviceProvider.GetService<ElementLookup>();
            if (lookup == null) return Enumerable.Empty<Property>();

            return lookup.FindExtendedPropertyProviders().Where(x => x.CanExtend(this)).SelectMany(x => x.GetExtendedProperties(this));
        }

        public virtual IEnumerable<Property> GetAllProperties()
        {
            var declaredProperties = TypeDescriptor.GetProperties(thisElement)
                                        .OfType<PropertyDescriptor>()
                                        .Where(x => !typeof(ConfigurationElement).IsAssignableFrom(x.PropertyType)) //filter out configuration elements
                                        .Where(x => x.Attributes.OfType<ConfigurationPropertyAttribute>().Any()) //only the once where we have a configurationPropertyAtt
                                        .Select(x => CreateProperty(x))
                                        .Cast<Property>();

            var extendedProperties = GetExtendedProperties();

            return declaredProperties.Union(extendedProperties);
        }

        private void EnsureHasAllProperties()
        {
            if (properties == null)
            {
                properties = GetAllProperties().ToList();
            }
        }

        public Property Property(string propertyName)
        {
            return Properties.Where(x => x.PropertyName == propertyName).FirstOrDefault();
        }

        public IEnumerable<Property> Properties
        {
            get
            {
                EnsureHasAllProperties();
                return properties;
            }
        }

        #endregion

        #region Navigation


        public virtual IEnumerable<ElementViewModel> GetAllChildElements()
        {
            return TypeDescriptor.GetProperties(thisElement)
                        .OfType<PropertyDescriptor>()
                        .Where(x => typeof(ConfigurationElement).IsAssignableFrom(x.PropertyType)) // only properties that are configuration elements
                        .Where(x => x.Attributes.OfType<ConfigurationPropertyAttribute>().Any())  //that have the configuration property attribute
                        .Select(x => CreateChildCollectionOrLeaf(x)) //create either a collection or a lead
                        .Where(x => x.IsNull == false);//make sure it wasnt an empty leaf
        }

        private void EnsureHasChildElements()
        {
            if (childElements == null)
            {
                RefreshChildElements();
            }
        }

        protected void RefreshChildElements()
        {
            childElements = GetAllChildElements().ToList();
        }

        public IEnumerable<ElementViewModel> ChildElements
        {
            get
            {
                EnsureHasChildElements();
                return childElements;
            }
        }

        public IEnumerable<ElementViewModel> DescendentElements(Func<ElementViewModel, bool> filter)
        {
            foreach (ElementViewModel childElement in ChildElements)
            {
                if (filter(childElement))
                {
                    yield return childElement;
                }

                foreach (ElementViewModel grandChild in childElement.DescendentElements(filter))
                {
                    yield return grandChild;
                }
            }
        }

        public IEnumerable<ElementViewModel> DescendentElements()
        {
            return DescendentElements(x => true);
        }

        public ElementViewModel ParentElement
        {
            get
            {
                return parentElementModel;
            }
        }

        public IEnumerable<ElementViewModel> AncesterElements()
        {
            if (ParentElement != null)
            {
                yield return ParentElement;

                foreach (ElementViewModel grandParent in ParentElement.AncesterElements())
                {
                    yield return grandParent;
                }
            }
        }

        public virtual IEnumerable<ICommand> ChildAdders
        {
            get
            {
                foreach(var child in ChildElements)
                {
                    foreach (var adder in child.ChildAdders)
                    {
                        yield return adder;
                    }
                }
            }
        }


        public SectionViewModel ContainingSection
        {
            get
            {
                if (this is SectionViewModel) return this as SectionViewModel;

                return AncesterElements().OfType<SectionViewModel>().FirstOrDefault();
            }
        }

        #endregion

        internal virtual void OnDeleted()
        {
            var handler = Deleted;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        public event EventHandler Deleted;

        protected ElementViewModel CreateChildCollectionOrLeaf(PropertyDescriptor declaringProperty)
        {
            if (typeof(ConfigurationElementCollection).IsAssignableFrom(declaringProperty.PropertyType))
            {
                return ContainingSection.CreateCollectionElement(this, declaringProperty);
            }
            return ContainingSection.CreateElement(this, declaringProperty);
        }

        public virtual ElementProperty CreateProperty(PropertyDescriptor declaringProperty)
        {
            return ContainingSection.CreateElementProperty(this, declaringProperty);
        }

        private string GetName()
        {
            if (thisElementWithName != null)
            {
                PropagatePropertyValueChanged("Name", "Name");
                return thisElementWithName.Name;
            }

            if (declaringProperty != null)
            {
                return declaringProperty.DisplayName;
            }

            var displayNameAttribute = MetadataAttributes.OfType<DisplayNameAttribute>().FirstOrDefault();
            if (displayNameAttribute != null)
            {
                return displayNameAttribute.DisplayName;
            }

            return string.Empty;
        }

        #region property changed

        protected virtual void DoPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion


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
    }
}
