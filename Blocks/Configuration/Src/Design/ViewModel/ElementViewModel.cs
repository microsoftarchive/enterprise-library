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
using Microsoft.Practices.Unity;
using System.Windows;

namespace Console.Wpf.ViewModel
{
    [DebuggerDisplay("Name : {Name} ConfigurationType : {ConfigurationType} Path = {Path}")]
    public class ElementViewModel : ViewModel, INotifyPropertyChanged, ICustomTypeDescriptor
    {
        readonly ConfigurationElement parentElement;
        readonly PropertyDescriptor declaringProperty;
        readonly ConfigurationElement thisElement;
        readonly MetadataCollection metadata;
        readonly ElementViewModel parentElementModel;
        readonly ElementViewModelPathBuilder path;
        readonly ConfigurationPropertyAttribute configurationProperty;

        ElementLookup elementLookup;

        ExtendedPropertyContainer extendedProperties;
        ObservableCollection<ElementViewModel> childElements;
        ObservableCollection<Property> properties;
        ViewModelTypeDescriptorProxy typeDescriptorProxy;
        IEnumerable<CommandModel> commands;

        bool promoteCommands = false;

        [InjectionConstructor]
        public ElementViewModel(ElementViewModel parentElementModel, PropertyDescriptor declaringProperty)
            : this(parentElementModel, declaringProperty, new Attribute[0])
        {
        }

        public ElementViewModel(ElementViewModel parentElementModel, PropertyDescriptor declaringProperty, IEnumerable<Attribute> additionalAttributes)
            : this(parentElementModel, declaringProperty.GetValue(parentElementModel.thisElement) as ConfigurationElement, declaringProperty.Attributes.OfType<Attribute>(),  additionalAttributes)
        {
            this.declaringProperty = declaringProperty;
        }

        protected ElementViewModel(ElementViewModel parentElementModel, ConfigurationElement thisElement, IEnumerable<Attribute> additionalAttributes)
            : this(parentElementModel, thisElement, TypeDescriptor.GetAttributes(thisElement).OfType<Attribute>(), additionalAttributes)
        {
        }

        private ElementViewModel(ElementViewModel parentElementModel, ConfigurationElement thisElement, IEnumerable<Attribute> componentModelAttributes, IEnumerable<Attribute> additionalAttributes)
        {
            this.typeDescriptorProxy = new ViewModelTypeDescriptorProxy(this);
            this.thisElement = thisElement;
            this.parentElementModel = parentElementModel;
            this.path = new ElementViewModelPathBuilder(parentElementModel, this);

            this.metadata = new MetadataCollection(componentModelAttributes);
            this.metadata.Override(additionalAttributes);

            if (parentElementModel != null)
            {
                this.parentElement = parentElementModel.thisElement;
                this.parentElementModel.PropertyChanged += (sender, args) => { if (args.PropertyName == "Path") DoPropertyChanged("Path"); };
            }

            promoteCommands = metadata.Attributes.OfType<PromoteCommandsAttribute>().Any();
            configurationProperty = Attributes.OfType<ConfigurationPropertyAttribute>().FirstOrDefault();
        }

        [InjectionMethod]
        public void ElementViewModelServiceDependencies(ElementLookup elementLookup)
        {
            this.elementLookup = elementLookup;
        }

        private void PropagateNamePropertyChanges()
        {
            foreach (var nameProperty in metadata.Attributes.OfType<NamePropertyAttribute>())
            {
                PropagatePropertyValueChanged(nameProperty.PropertyName, "Name");
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

        #region Identity 
        
        //property witn null-value.
        //empty leaf
        private bool IsNull
        {
            get { return thisElement == null; }
        }

        public virtual Type ConfigurationType
        {
            get { return thisElement.GetType(); }
        }

        public ConfigurationElement ConfigurationElement
        {
            get { return thisElement; }
        }

        public PropertyDescriptor DeclaringProperty
        {
            get { return declaringProperty; }
        }

        #endregion

        #region meta data

        public  IEnumerable<Attribute> Attributes
        {
            get { return metadata.Attributes; }
        }

        public virtual bool PromoteCommands
        {
            get { return promoteCommands; }
        }

        public virtual string Name
        {
            get
            {
                var nameProperties = metadata.Attributes.OfType<NamePropertyAttribute>();
                if (nameProperties.Any())
                {
                    var namePropertiesDisplay = nameProperties.OrderBy(x=>x.Order).Select(x => string.Format(x.NamePropertyDisplayFormat, Property(x.PropertyName).Value));
                    return string.Join("", namePropertiesDisplay.ToArray());
                }

                if (declaringProperty != null)
                {
                    return declaringProperty.DisplayName;
                }

                var displayNameAttribute = Attributes.OfType<DisplayNameAttribute>().FirstOrDefault();
                if (displayNameAttribute != null)
                {
                    return displayNameAttribute.DisplayName;
                }

                return ConfigurationType.Name;
            }
        }

        public virtual string Path
        {
            get
            {
                return path.XPath;
            }
        }

        protected virtual string GetLocalPathPart()
        {
            return configurationProperty.Name;
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

        #endregion

        #region Navigation

        protected virtual IEnumerable<Property> GetAllProperties()
        {
            var declaredProperties = TypeDescriptor.GetProperties(thisElement)
                                        .OfType<PropertyDescriptor>()
                                        .Where(x => !typeof(ConfigurationElement).IsAssignableFrom(x.PropertyType) || (x.GetEditor(typeof(FrameworkElement)) != null)) //filter out configuration elements
                                        .Where(x => x.Attributes.OfType<ConfigurationPropertyAttribute>().Any()) //only the once where we have a configurationPropertyAtt
                                        .Select(x => ContainingSection.CreateElementProperty(this, x))
                                        .Cast<Property>();

            return declaredProperties;
        }

        private void EnsureHasProperties()
        {
            if (properties == null)
            {
                properties = new ObservableCollection<Property>(GetAllProperties().OrderBy(x => x.Category).ThenBy(x => x.DisplayName));

                extendedProperties = new ExtendedPropertyContainer(elementLookup, this, properties);

                PropagateNamePropertyChanges();
            }
        }

        /// <summary>Returns all <see cref="Property"/> instances for this <see cref="ElementViewModel"/> instance.</summary>
        public ObservableCollection<Property> Properties
        {
            get
            {
                EnsureHasProperties();
                return properties;
            }
        }

        /// <summary>Returns a <see cref="Property"/> with the specified <paramref name="propertyName"/>. If the property cannot be found, returns <see langword="null"/>.</summary>
        public Property Property(string propertyName)
        {
            return Properties.Where(x => x.PropertyName == propertyName).FirstOrDefault();
        }

        protected virtual IEnumerable<ElementViewModel> GetAllChildElements()
        {
            return TypeDescriptor.GetProperties(thisElement)
                        .OfType<PropertyDescriptor>()
                        .Where(x => x.IsBrowsable)
                        .Where(x => typeof(ConfigurationElement).IsAssignableFrom(x.PropertyType)) // only properties that are configuration elements
                        .Where(x => x.Attributes.OfType<ConfigurationPropertyAttribute>().Any())  //that have the configuration property attribute
                        .Select(x => ContainingSection.CreateChild(this, x)) //create either a collection or a lead
                        .Where(x => x.IsNull == false);//make sure it wasnt an empty leaf
        }

        private void EnsureHasChildElements()
        {
            if (childElements == null)
            {
                childElements = new ObservableCollection<ElementViewModel>(GetAllChildElements());
                childElements.CollectionChanged += (sender, args) => { OnDescendentElementsChanged(this, args); };
            }
        }

        /// <summary>Returns all <see cref="ElementViewModel"/> instances contained as direct child elements.</summary>
        public ObservableCollection<ElementViewModel> ChildElements
        {
            get
            {
                EnsureHasChildElements();
                return childElements;
            }
        }

        /// <summary>Returns all descending <see cref="ElementViewModel"/> that match the supplied <paramref name="filter"/> instances relative to this <see cref="ElementViewModel"/> instance.</summary>
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

        /// <summary>Returns all descending <see cref="ElementViewModel"/> instances relative to this <see cref="ElementViewModel"/> instance.</summary>
        public IEnumerable<ElementViewModel> DescendentElements()
        {
            return DescendentElements(x => true);
        }

        /// <summary>Returns the parent <see cref="ElementViewModel"/> for this <see cref="ElementViewModel"/> instance.</summary>
        public ElementViewModel ParentElement
        {
            get
            {
                return parentElementModel;
            }
        }

        /// <summary>Returns all anscester <see cref="ElementViewModel"/> for this <see cref="ElementViewModel"/> instance.</summary>
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

        /// <summary>Returns the <see cref="SectionViewModel"/> that contains this <see cref="ElementViewModel"/> instance.</summary>
        public SectionViewModel ContainingSection
        {
            get
            {
                if (this is SectionViewModel) return (SectionViewModel)this;
                return AncesterElements().OfType<SectionViewModel>().First();
            }
        }

        #endregion

        #region Commands and Operations

        public IEnumerable<CommandModel> CustomCommands
        {
            get
            {
                return Commands.Where(x => x.Placement == CommandPlacement.ContextCustom);
            }
        }

        public IEnumerable<CommandModel> Commands
        {
            get
            {
                EnsureCommands();
                return commands;
            }
        }

        private void EnsureCommands()
        {
            if (commands == null)
            {
                commands = GetAllCommands().ToArray();
            }
        }

        /// <summary>
        /// Creates or collections all the commands related to this <see cref="ElementViewModel"/>.
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerable<CommandModel> GetAllCommands()
        {
            return  CreateDeleteCommand()
                    .Union(CreateCustomCommands())
                    .Union(GetPromotedCommands());
        }

        protected IEnumerable<CommandModel> CreateDeleteCommand()
        {
            yield return ContainingSection.CreateDeleteCommand(this, Attributes);
        }

        protected IEnumerable<CommandModel> CreateCustomCommands()
        {
            foreach (var command in ContainingSection.CreateCustomCommands(this, Attributes))
            {
                yield return command;
            }
        }

        protected IEnumerable<CommandModel> GetPromotedCommands()
        {
            foreach (var child in ChildElements)
            {
                if (child.PromoteCommands)
                {
                    foreach (var command in child.Commands)
                    {
                        yield return command;
                    }
                }
            }
        }

        public virtual void Delete()
        {

        }

        public virtual CommandModel DeleteCommand
        {
            get { return Commands.Where(x => x.Placement == CommandPlacement.ContextDelete).FirstOrDefault(); }
        }

        #endregion

        #region Events and Event Invocation

        public virtual void OnDeleted()
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

        protected virtual void OnDescendentElementsChanged(ElementViewModel sender, NotifyCollectionChangedEventArgs collectionChanged)
        {
            var handler = DescendentElementsChanged;
            if (handler != null)
            {
                DescendentElementsChanged(sender, collectionChanged);
            }
            if (ParentElement != null)
            {
                ParentElement.OnDescendentElementsChanged(sender, collectionChanged);
            }
        }

        public event NotifyCollectionChangedEventHandler DescendentElementsChanged;

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

        private class ElementViewModelPathBuilder
        {
            private const string pathSepperator = "/";

            readonly ElementViewModelPathBuilder parentPath;
            readonly ElementViewModel me;

            public ElementViewModelPathBuilder(ElementViewModel parentViewModel, ElementViewModel me)
            {
                this.parentPath = (parentViewModel != null) ? parentViewModel.path : null;
                this.me = me;
            }

            public string XPath
            {
                get
                {
                    string parentPathString = parentPath == null ? string.Empty : parentPath.XPath;
                    string localPathPart = me.GetLocalPathPart();
                    if (string.IsNullOrEmpty(localPathPart)) return parentPathString;

                    return parentPathString + pathSepperator + localPathPart;
                }
            }

            public string TypePath
            {
                get
                {
                    string parentTypePath = parentPath == null ? string.Empty : parentPath.TypePath;
                    return parentTypePath + pathSepperator + me.ConfigurationType.Name;
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
                foreach (IElementExtendedPropertyProvider provider in extendedPropetryProviders.OfType<IElementExtendedPropertyProvider>().Where(x => x.CanExtend(subject)))
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

    }
}
