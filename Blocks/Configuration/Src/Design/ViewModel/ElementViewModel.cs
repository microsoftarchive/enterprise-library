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
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Practices.Unity;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{
    [DebuggerDisplay("Name : {Name} ConfigurationType : {ConfigurationType} Path = {Path}")]
    public class ElementViewModel : ViewModel, INotifyPropertyChanged, INeedInitialization
    {
        private readonly ConfigurationPropertyAttribute configurationProperty;
        private readonly PropertyDescriptor declaringProperty;
        private readonly MetadataCollection metadata;
        private readonly ConfigurationElement parentElement;
        private readonly ElementViewModel parentElementModel;
        private readonly ElementViewModelPathBuilder path;
        private readonly ConfigurationElement thisElement;
        private IApplicationModel applicationModel;
        private ObservableCollection<ElementViewModel> childElements;
        private IEnumerable<CommandModel> commands;
        private ElementLookup elementLookup;

        private ExtendedPropertyContainer extendedProperties;

        private bool promoteCommands = false;
        private ObservableCollection<Property> properties;

        [InjectionConstructor]
        public ElementViewModel(ElementViewModel parentElementModel, PropertyDescriptor declaringProperty)
            : this(parentElementModel, declaringProperty, new Attribute[0])
        {
        }

        public ElementViewModel(ElementViewModel parentElementModel, PropertyDescriptor declaringProperty,
                                IEnumerable<Attribute> additionalAttributes)
            : this(
                parentElementModel, declaringProperty.GetValue(parentElementModel.thisElement) as ConfigurationElement,
                declaringProperty.Attributes.OfType<Attribute>(), additionalAttributes)
        {
            this.declaringProperty = declaringProperty;
        }

        protected ElementViewModel(ElementViewModel parentElementModel, ConfigurationElement thisElement,
                                   IEnumerable<Attribute> additionalAttributes)
            : this(
                parentElementModel, thisElement, TypeDescriptor.GetAttributes(thisElement).OfType<Attribute>(),
                additionalAttributes)
        {
        }

        private ElementViewModel(ElementViewModel parentElementModel, ConfigurationElement thisElement,
                                 IEnumerable<Attribute> componentModelAttributes,
                                 IEnumerable<Attribute> additionalAttributes)
        {
            this.thisElement = thisElement;
            this.parentElementModel = parentElementModel;
            path = new ElementViewModelPathBuilder(parentElementModel, this);

            metadata = new MetadataCollection(componentModelAttributes);
            metadata.Override(additionalAttributes);

            if (parentElementModel != null)
            {
                parentElement = parentElementModel.thisElement;
                this.parentElementModel.PropertyChanged +=
                    (sender, args) => { if (args.PropertyName == "Path") OnPropertyChanged("Path"); };
            }

            promoteCommands = metadata.Attributes.OfType<PromoteCommandsAttribute>().Any();
            configurationProperty = Attributes.OfType<ConfigurationPropertyAttribute>().FirstOrDefault();
        }

        #region INeedInitialization Members

        /// <summary>
        /// Initialization for this element during the <see cref="ConfigurationSourceModel.Load"/> or <see cref="ConfigurationSourceModel.AddSection"/>.
        /// </summary>
        /// <param name="context">The load context for this call</param>
        public virtual void Initialize(InitializeContext context)
        {
        }

        #endregion

        /// <summary>
        /// Injection Method used to supply additional dependencies to the <see cref="ElementViewModel"/>
        /// </summary>
        /// <param name="elementLookup"></param>
        [InjectionMethod]
        public void ElementViewModelServiceDependencies(ElementLookup elementLookup, IApplicationModel applicationModel)
        {
            this.elementLookup = elementLookup;
            this.applicationModel = applicationModel;
        }

        private void PropagateNamePropertyChanges()
        {
            foreach (var nameProperty in metadata.Attributes.OfType<NamePropertyAttribute>())
            {
                PropagatePropertyValueChanged(nameProperty.PropertyName, "Name");
            }
        }

        private void PropagatePropertyValueChanged(string propertyName, string thisPropertyName)
        {
            var propertyToListenFor = Property(propertyName);
            if (propertyToListenFor != null)
            {
                propertyToListenFor.PropertyChanged += (sender, args) =>
                                                           {
                                                               if (args.PropertyName == "Value")
                                                               {
                                                                   OnPropertyChanged(thisPropertyName);
                                                               }
                                                           };
            }
        }

        ///<summary>
        /// Validates the <see cref="Properties"/> of this <see cref="ElementViewModel"/>
        ///</summary>
        public virtual void Validate()
        {
            foreach (var property in Properties)
            {
                property.Validate();
            }

            foreach (var child in ChildElements)
            {
                child.Validate();
            }
        }

        public void Select()
        {
            applicationModel.OnSelectedElementChanged(this);
            IsSelected = true;
        }

        private bool isSelected;
        public bool IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                if (isSelected != value)
                {
                    isSelected = value;
                    OnPropertyChanged("IsSelected");
                }
            }
        }


        private bool propertiesShown;
        public bool PropertiesShown
        {
            get
            {
                return propertiesShown;
            }

            set
            {
                if (propertiesShown != value)
                {
                    propertiesShown = value;
                    OnPropertyChanged("PropertiesShown");
                }
            }
        }

        #region Identity

        //property witn null-value.
        //empty leaf
        private bool IsNull
        {
            get { return thisElement == null; }
        }

        /// <summary>
        /// Gets the type of the configuration element this <see cref="ElementViewModel"/> was created for. <br/>
        /// </summary>
        /// <remarks>
        /// A <see cref="ElementViewModel"/>'s ConfigurationType is often used to identify or attach behavior to <see cref="ElementViewModel"/> instances.
        /// </remarks>
        public virtual Type ConfigurationType
        {
            get { return thisElement.GetType(); }
        }

        /// <summary>
        /// Gets the configuration element instance this <see cref="ElementViewModel"/>  was created for.
        /// </summary>
        public ConfigurationElement ConfigurationElement
        {
            get { return thisElement; }
        }

        /// <summary>
        /// Gets the property on the containing configuration element, this <see cref="ElementViewModel"/>  was created for.
        /// </summary>
        public PropertyDescriptor DeclaringProperty
        {
            get { return declaringProperty; }
        }

        #endregion

        #region meta data

        /// <summary>
        /// Gets the attributes that where supplied to this <see cref="ElementViewModel"/> instance.
        /// </summary>
        public IEnumerable<Attribute> Attributes
        {
            get { return metadata.Attributes; }
        }

        /// <summary>
        /// Gets a boolean that indicates whether this <see cref="ElementViewModel"/>'s add commands should be promoted to its parent <see cref="ElementViewModel"/>.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if this <see cref="ElementViewModel"/>'s add commands should be promoted to its parent <see cref="ElementViewModel"/>. Otherwise <see langword="false"/>.
        /// </value>
        public virtual bool PromoteCommands
        {
            get { return promoteCommands; }
        }

        /// <summary>
        /// Gets the name of the <see cref="ElementViewModel"/> for use in the UI (User Interface).<br/>
        /// </summary>
        public virtual string Name
        {
            get
            {
                var namePropertyAttribute = metadata.Attributes.OfType<NamePropertyAttribute>().FirstOrDefault();
                if (namePropertyAttribute != null)
                {
                    var namePropertiesDisplay = string.Format(namePropertyAttribute.NamePropertyDisplayFormat,
                                                              Property(namePropertyAttribute.PropertyName).Value);
                    return namePropertiesDisplay;
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

        public Property NameProperty
        {
            get
            {
                var namePropertyAttribute = metadata.Attributes.OfType<NamePropertyAttribute>().FirstOrDefault();
                if (namePropertyAttribute != null)
                {
                    return Property(namePropertyAttribute.PropertyName);
                }
                return null;
            }
        }

        /// <summary>
        /// Gets a string that can be used to uniquely identify this <see cref="ElementViewModel"/>. <br/>
        /// </summary>
        public virtual string Path
        {
            get { return path.XPath; }
        }


        /// <summary>
        /// Gets the type path information for an <see cref="ElementViewModel"/> in the
        /// form ParentTypePath/ConfigurationElementType.
        /// </summary>
        public virtual string TypePath
        {
            get { return path.TypePath; }
        }

        /// <summary>
        /// Gets a string that can be appended to the parent's <see cref="ElementViewModel.Path"/> to compose a <see cref="ElementViewModel.Path"/> used to uniquely identify this <see cref="ElementViewModel"/>. <br/>
        /// </summary>
        protected virtual string GetLocalPathPart()
        {
            return configurationProperty.Name;
        }

        #endregion

        #region Navigation

        /// <summary>
        /// Gets all <see cref="Property"/> instances for this <see cref="ElementViewModel"/> instance.
        /// </summary>
        public ObservableCollection<Property> Properties
        {
            get
            {
                EnsureHasProperties();
                return properties;
            }
        }

        /// <summary>
        /// Gets all <see cref="ElementViewModel"/> instances contained as direct child elements.
        /// </summary>
        public ObservableCollection<ElementViewModel> ChildElements
        {
            get
            {
                EnsureHasChildElements();
                return childElements;
            }
        }

        /// <summary>
        /// Gets the parent <see cref="ElementViewModel"/> for this <see cref="ElementViewModel"/> instance.
        /// </summary>
        public ElementViewModel ParentElement
        {
            get { return parentElementModel; }
        }

        /// <summary>
        /// Gets the <see cref="SectionViewModel"/> that contains this <see cref="ElementViewModel"/> instance.
        /// </summary>
        public SectionViewModel ContainingSection
        {
            get
            {
                if (this is SectionViewModel) return (SectionViewModel)this;
                return AncesterElements().OfType<SectionViewModel>().First();
            }
        }

        /// <summary>
        /// Gets all the <see cref="Property"/> intstances that are part of this <see cref="ElementViewModel"/> instance.
        /// </summary>
        /// <returns>
        /// Returns an un evaluated iterator class. <br/>
        /// </returns>
        protected virtual IEnumerable<Property> GetAllProperties()
        {
            var declaredProperties = TypeDescriptor.GetProperties(thisElement)
                .OfType<PropertyDescriptor>()
                .Where(
                x =>
                !typeof(ConfigurationElement).IsAssignableFrom(x.PropertyType) ||
                (x.GetEditor(typeof(FrameworkElement)) != null)) //filter out configuration elements
                .Where(x => x.Attributes.OfType<ConfigurationPropertyAttribute>().Any())
                //only the once where we have a configurationPropertyAtt
                .Select(x => ContainingSection.CreateElementProperty(this, x))
                .Cast<Property>();

            return declaredProperties;
        }

        private void EnsureHasProperties()
        {
            if (properties == null)
            {
                properties = new ObservableCollection<Property>(GetAllProperties());
                //second set: to make sure the properties arent created twice when filter.
                properties = new ObservableCollection<Property>(properties.OrderBy(x => x.Category).ThenBy(x => x.DisplayName));

                extendedProperties = new ExtendedPropertyContainer(elementLookup, this, properties);
                PropagateNamePropertyChanges();
            }
        }

        /// <summary>
        /// Gets the <see cref="Property"/> with the specified <paramref name="propertyName"/>. <br/>
        /// If the property cannot be found, returns <see langword="null"/>.
        /// </summary>
        public Property Property(string propertyName)
        {
            return Properties.Where(x => x.PropertyName == propertyName).FirstOrDefault();
        }

        /// <summary>
        /// Gets all the <see cref="ElementViewModel"/> intstances that are directly contained in this <see cref="ElementViewModel"/> instance.
        /// </summary>
        /// <returns>
        /// An un evaluated iterator class. <br/>
        /// </returns>
        protected virtual IEnumerable<ElementViewModel> GetAllChildElements()
        {
            return TypeDescriptor.GetProperties(thisElement)
                .OfType<PropertyDescriptor>()
                .Where(x => x.IsBrowsable)
                .Where(x => typeof(ConfigurationElement).IsAssignableFrom(x.PropertyType))
                // only properties that are configuration elements
                .Where(x => x.Attributes.OfType<ConfigurationPropertyAttribute>().Any())
                //that have the configuration property attribute
                .Select(x => ContainingSection.CreateChild(this, x)) //create either a collection or a lead
                .Where(x => x.IsNull == false); //make sure it wasnt an empty leaf
        }

        private void EnsureHasChildElements()
        {
            if (childElements == null)
            {
                childElements = new ObservableCollection<ElementViewModel>(GetAllChildElements());
                childElements.CollectionChanged += (sender, args) => { OnDescendentElementsChanged(this, args); };
            }
        }

        public ElementViewModel ChildElement(string propertyName)
        {
            return ChildElements.Where(x => x.declaringProperty.Name == propertyName).FirstOrDefault();
        }


        /// <summary>
        /// Gets all descending <see cref="ElementViewModel"/> that match the supplied <paramref name="filter"/> instances relative to this <see cref="ElementViewModel"/> instance.
        /// </summary>
        public IEnumerable<ElementViewModel> DescendentElements(Func<ElementViewModel, bool> filter)
        {
            foreach (var childElement in ChildElements)
            {
                if (filter(childElement))
                {
                    yield return childElement;
                }

                foreach (var grandChild in childElement.DescendentElements(filter))
                {
                    yield return grandChild;
                }
            }
        }

        /// <summary>
        /// Returns all descending <see cref="ElementViewModel"/> instances relative to this <see cref="ElementViewModel"/> instance.
        /// </summary>
        public IEnumerable<ElementViewModel> DescendentElements()
        {
            return DescendentElements(x => true);
        }

        /// <summary>
        /// Gets all ancester <see cref="ElementViewModel"/> for this <see cref="ElementViewModel"/> instance.
        /// </summary>
        public IEnumerable<ElementViewModel> AncesterElements()
        {
            if (ParentElement != null)
            {
                yield return ParentElement;

                foreach (var grandParent in ParentElement.AncesterElements())
                {
                    yield return grandParent;
                }
            }
        }

        #endregion

        #region Commands and Operations

        ///<summary>
        /// Returns the sub-set of add <see cref="CommandModel"/> commands from <see cref="Commands"/>
        ///</summary>
        public IEnumerable<CommandModel> AddCommands
        {
            get { return Commands.Where(x => x.Placement == CommandPlacement.ContextAdd); }
        }

        ///<summary>
        /// Returns the subset of custom <see cref="CommandModel"/> commands from <see cref="Commands"/>
        ///</summary>
        public IEnumerable<CommandModel> CustomCommands
        {
            get { return Commands.Where(x => x.Placement == CommandPlacement.ContextCustom); }
        }

        ///<summary>
        /// The <see cref="CommandModel"/> command associated with this configuraiton element.
        ///</summary>
        ///<remarks>
        /// Default command are typically provided by the <see cref="ContainingSection"/>.  These may be overridden
        /// for each configuration element through the use of a <see cref="CommandAttribute"/>
        /// </remarks>
        public IEnumerable<CommandModel> Commands
        {
            get
            {
                EnsureCommands();
                return commands;
            }
        }

        ///<summary>
        /// The delete <see cref="CommandModel"/>.
        ///</summary>
        /// <remarks>
        /// This command is provided by the <see cref="ContainingSection"/> during construction, but
        /// may be overriden through the use of <see cref="CommandAttribute"/>.
        /// </remarks>
        public virtual CommandModel DeleteCommand
        {
            get { return Commands.Where(x => x.Placement == CommandPlacement.ContextDelete).FirstOrDefault(); }
        }

        private void EnsureCommands()
        {
            if (commands == null)
            {
                commands = GetAllCommands()
                    .Where(x => x.Browsable)
                    .ToArray();
            }
        }

        /// <summary>
        /// Creates or collections all the commands related to this <see cref="ElementViewModel"/>.
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerable<CommandModel> GetAllCommands()
        {
            return CreateDeleteCommand()
                .Union( new CommandModel[] { new ToggleShowPropertiesCommand(this) })
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

        ///<summary>
        /// Deletes this element.
        ///</summary>
        public virtual void Delete()
        {
        }

        #endregion

        #region Events and Event Invocation

        public event PropertyChangedEventHandler PropertyChanged;

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

        protected virtual void OnDescendentElementsChanged(ElementViewModel sender,
                                                           NotifyCollectionChangedEventArgs collectionChanged)
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

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (propertyName == "Name")
            {
                OnPropertyChanged("Path");
            }

            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region Nested type: ElementViewModelPathBuilder

        private class ElementViewModelPathBuilder
        {
            private const string pathSepperator = "/";

            private readonly ElementViewModel me;
            private readonly ElementViewModelPathBuilder parentPath;

            public ElementViewModelPathBuilder(ElementViewModel parentViewModel, ElementViewModel me)
            {
                parentPath = (parentViewModel != null) ? parentViewModel.path : null;
                this.me = me;
            }

            public string XPath
            {
                get
                {
                    var parentPathString = parentPath == null ? string.Empty : parentPath.XPath;
                    var localPathPart = me.GetLocalPathPart();
                    if (string.IsNullOrEmpty(localPathPart)) return parentPathString;

                    return parentPathString + pathSepperator + localPathPart;
                }
            }

            public string TypePath
            {
                get
                {
                    var parentTypePath = parentPath == null ? string.Empty : parentPath.TypePath;
                    return parentTypePath + pathSepperator + me.ConfigurationType.Name;
                }
            }
        }

        #endregion

        #region Nested type: ExtendedPropertyContainer

        private class ExtendedPropertyContainer
        {
            private readonly IElementChangeScope extendedPropetryProviders;
            private readonly ObservableCollection<Property> properties;

            private readonly Dictionary<IElementExtendedPropertyProvider, Property[]> propertiesByExtensionProviders =
                new Dictionary<IElementExtendedPropertyProvider, Property[]>();

            private readonly ElementViewModel subject;

            public ExtendedPropertyContainer(ElementLookup lookup, ElementViewModel subject,
                                             ObservableCollection<Property> properties)
            {
                this.properties = properties;
                this.subject = subject;
                extendedPropetryProviders = lookup.FindExtendedPropertyProviders();
                extendedPropetryProviders.CollectionChanged +=
                    new NotifyCollectionChangedEventHandler(extendedPropetryProviders_CollectionChanged);

                Refresh();
            }

            private void Refresh()
            {
                foreach (var provider in propertiesByExtensionProviders.Keys.ToArray())
                {
                    RemoveExtensionProvider(provider);
                }

                propertiesByExtensionProviders.Clear();
                foreach (
                    var provider in
                        extendedPropetryProviders.OfType<IElementExtendedPropertyProvider>().Where(
                            x => x.CanExtend(subject)))
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
                    foreach (var property in extendedProperties)
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
                        extendedProperty.Initialize(new InitializeContext());
                    }
                }
            }

            private void extendedPropetryProviders_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
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

        #endregion


        private class ToggleShowPropertiesCommand : CommandModel
        {
            private readonly ElementViewModel viewModel;

            public ToggleShowPropertiesCommand(ElementViewModel viewModel)
            {
                this.viewModel = viewModel;
            }

            public override string Title
            {
                get
                {
                    return "Toggle Properties";
                }
            }

            public override bool CanExecute(object parameter)
            {
                return true;
            }

            public override void Execute(object parameter)
            {
                viewModel.PropertiesShown = !viewModel.PropertiesShown;
            }

            public override string KeyGesture
            {
                get
                {
                    return new KeyGestureConverter().ConvertToString(new KeyGesture(Key.Space));
                }
            }
        }
    }
}
