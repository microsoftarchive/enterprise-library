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
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Console.Wpf.ViewModel
{
    [DebuggerDisplay("Name : {Name} ConfigurationType : {ConfigurationType} Path = {Path}")]
    public class ElementViewModel : ViewModel, INotifyPropertyChanged, ICustomTypeDescriptor
    {
        readonly IServiceProvider serviceProvider;
        readonly ConfigurationElement parentElement;
        readonly PropertyDescriptor declaringProperty;
        readonly ConfigurationElement thisElement;
        readonly NamedConfigurationElement thisElementWithName;
        readonly MetadataCollection metadata;
        readonly ElementViewModel parentElementModel;
        readonly ElementViewModelPath path;


        ExtendedPropertyContainer extendedProperties;
        ObservableCollection<ElementViewModel> childElements;
        ObservableCollection<Property> properties;
        ViewModelTypeDescriptorProxy typeDescriptorProxy;

        public ElementViewModel(IServiceProvider serviceProvider, ElementViewModel parentElementModel, PropertyDescriptor declaringProperty)
            : this(serviceProvider, parentElementModel, declaringProperty, new Attribute[0])
        {
        }

        public ElementViewModel(IServiceProvider serviceProvider, ElementViewModel parentElementModel, PropertyDescriptor declaringProperty, IEnumerable<Attribute> additonalAttributes)
            : this(serviceProvider, parentElementModel, declaringProperty.GetValue(parentElementModel.thisElement) as ConfigurationElement, additonalAttributes)
        {
            this.declaringProperty = declaringProperty;

            this.metadata = new MetadataCollection(TypeDescriptor.GetAttributes(ConfigurationType).OfType<Attribute>().ToArray());
            this.metadata.Override(declaringProperty.Attributes.OfType<Attribute>());
            this.metadata.Override(additonalAttributes);
        }

        protected ElementViewModel(IServiceProvider serviceProvider, ElementViewModel parentElementModel, ConfigurationElement thisElement, IEnumerable<Attribute> additonalAttributes)
        {
            this.typeDescriptorProxy = new ViewModelTypeDescriptorProxy(this);
            this.serviceProvider = serviceProvider;
            this.thisElement = thisElement;
            this.parentElementModel = parentElementModel;
            this.thisElementWithName = thisElement as NamedConfigurationElement;
            this.path = new ElementViewModelPath(parentElementModel, this);

            this.metadata = new MetadataCollection(TypeDescriptor.GetAttributes(ConfigurationType).OfType<Attribute>().ToArray());
            this.metadata.Override(additonalAttributes);

            if (parentElementModel != null)
            {
                this.parentElement = parentElementModel.thisElement;
                this.parentElementModel.PropertyChanged += (sender, args) => { if (args.PropertyName == "Path") DoPropertyChanged("Path"); };
            }
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
            get { return metadata.Attributes; }
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

        public string Path
        {
            get
            {
                return path.PathString;
            }
        }

        public virtual Type ConfigurationType
        {
            get { return thisElement.GetType(); }
        }

        public virtual IEnumerable<Property> GetAllProperties()
        {
            var declaredProperties = TypeDescriptor.GetProperties(thisElement)
                                        .OfType<PropertyDescriptor>()
                                        .Where(x => !typeof(ConfigurationElement).IsAssignableFrom(x.PropertyType)) //filter out configuration elements
                                        .Where(x => x.Attributes.OfType<ConfigurationPropertyAttribute>().Any()) //only the once where we have a configurationPropertyAtt
                                        .Select(x => CreateProperty(x))
                                        .Cast<Property>();

            return declaredProperties;
        }

        private void EnsureHasAllProperties()
        {
            if (properties == null)
            {
                properties = new ObservableCollection<Property>(GetAllProperties());

                extendedProperties = new ExtendedPropertyContainer(serviceProvider.GetService<ElementLookup>(), this, properties);
            }
        }

        public Property Property(string propertyName)
        {
            return Properties.Where(x => x.PropertyName == propertyName).FirstOrDefault();
        }

        public ObservableCollection<Property> Properties
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
                        .Where(x => x.IsBrowsable)
                        .Where(x => typeof(ConfigurationElement).IsAssignableFrom(x.PropertyType)) // only properties that are configuration elements
                        .Where(x => x.Attributes.OfType<ConfigurationPropertyAttribute>().Any())  //that have the configuration property attribute
                        .Select(x => CreateChildCollectionOrLeaf(x)) //create either a collection or a lead
                        .Where(x => x.IsNull == false);//make sure it wasnt an empty leaf
        }

        private void EnsureHasChildElements()
        {
            if (childElements == null)
            {
                childElements = new ObservableCollection<ElementViewModel>(GetAllChildElements());
            }
        }

        public ObservableCollection<ElementViewModel> ChildElements
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
                foreach (var child in ChildElements)
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

        //todo: we can remove this here. though i didnt think i'd hurt having.
        //removing would mean, relying on the contaning collections NotifyCollectionChange
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

            return ConfigurationType.Name;
        }

        #region property changed

        protected virtual void DoPropertyChanged(string propertyName)
        {
            if (propertyName == "Name")
            {
                DoPropertyChanged("Path");
            }

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

        private class ElementViewModelPath
        {
            private const string pathSepperator = "/";
            private const string axisSepperator = ":";

            readonly ElementViewModelPath parentPath;
            readonly ElementViewModel me;
            readonly string myTypeName;

            public ElementViewModelPath(ElementViewModel parentViewModel, ElementViewModel me)
            {
                this.parentPath = (parentViewModel != null) ? parentViewModel.path : null;
                this.me = me;
                this.myTypeName = me.ConfigurationType.Name;
            }

            public string PathString
            {
                get
                {
                    string parentPathString = parentPath == null ? string.Empty : parentPath.PathString;
                    return parentPathString + pathSepperator + myTypeName + axisSepperator + me.Name;
                }
            }

            public string TypePath
            {
                get
                {
                    string parentTypePath = parentPath == null ? string.Empty : parentPath.TypePath;
                    return parentTypePath + pathSepperator + me.ConfigurationType.ToString();
                }
            }
        }

        private class ExtendedPropertyContainer
        {
            readonly IElementChangeScope extendedPropetryProviders;
            readonly Dictionary<IElementExtendedPropertyProvider, Property[]> propertiesByExtensionProviders = new Dictionary<IElementExtendedPropertyProvider, Property[]>();
            readonly ElementViewModel subject;
            readonly ObservableCollection<Property> properties;

            public ExtendedPropertyContainer(ElementLookup lookup, ElementViewModel subject, ObservableCollection<Property> properties)
            {
                this.properties = properties;
                this.subject = subject;
                this.extendedPropetryProviders = lookup.FindExtendedPropertyProviders();
                extendedPropetryProviders.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(extendedPropetryProviders_CollectionChanged);

                Refresh();
            }

            private void Refresh()
            {
                foreach (IElementExtendedPropertyProvider provider in propertiesByExtensionProviders.Keys.ToArray())
                {
                    RemoveExtensionProvider(provider);
                }

                propertiesByExtensionProviders.Clear();
                foreach (IElementExtendedPropertyProvider provider in extendedPropetryProviders.Enum().OfType<IElementExtendedPropertyProvider>().Where(x => x.CanExtend(subject)))
                {
                    AddExtensionProvider(provider);
                }
            }

            private void RemoveExtensionProvider(IElementExtendedPropertyProvider provider)
            {
                Property[] extendedProperties = null;
                if (propertiesByExtensionProviders.TryGetValue(provider, out extendedProperties))
                {
                    propertiesByExtensionProviders.Remove(provider);
                    foreach (Property property in extendedProperties)
                    {
                        properties.Remove(property);
                    }
                }
            }

            private void AddExtensionProvider(IElementExtendedPropertyProvider provider)
            {
                if (provider.CanExtend(subject))
                {
                    var extendedProperties = provider.GetExtendedProperties(subject).ToArray();
                    propertiesByExtensionProviders.Add(provider, extendedProperties);
                    foreach (var extendedProperty in extendedProperties)
                    {
                        properties.Add(extendedProperty);
                    }
                }
            }

            void extendedPropetryProviders_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        foreach (IElementExtendedPropertyProvider extProvider in e.NewItems)
                        {
                            AddExtensionProvider(extProvider);
                        }
                        break;

                    case NotifyCollectionChangedAction.Remove:
                        foreach (IElementExtendedPropertyProvider extProvider in e.OldItems)
                        {
                            RemoveExtensionProvider(extProvider);
                        }
                        break;

                    case NotifyCollectionChangedAction.Reset:
                        Refresh();
                        break;
                }
            }


        }

        /// <summary>
        /// Returns the type path information for an <see cref="ElementViewModel"/> in the
        /// form ParentTypePath/ConfigurationElementType.
        /// </summary>
        public virtual string TypePath
        {
            get
            {
                return path.TypePath;
            }
        }
    }
}
