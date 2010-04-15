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
using System.Globalization;
using System.Linq;
using System.Windows.Input;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Utility;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{
    /// <summary>
    /// The <see cref="ElementViewModel"/> provides a description of a <see cref="ConfigurationElement"/> 
    /// displayed in the Enterprise Library Configuration Design tool.
    /// </summary>
    /// <remarks>
    /// The <see cref="ElementViewModel"/> collects metadata about a <see cref="ConfigurationElement"/> using
    /// reflection and additional <see cref="Attribute"/> values provided during construction.</remarks>
    [DebuggerDisplay("Name : {Name} ConfigurationType : {ConfigurationType} Path = {Path}")]
    public class ElementViewModel : ViewModel, INotifyPropertyChanged, IEnvironmentalOverridesElement
    {
        private readonly ConfigurationPropertyAttribute configurationProperty;
        private readonly PropertyDescriptor declaringProperty;
        private readonly MetadataCollection metadata;
        private readonly ElementViewModel parentElementModel;
        private readonly ElementViewModelPathBuilder path;
        private readonly ConfigurationElement thisElement;
        private IApplicationModel applicationModel;
        private ObservableCollection<ElementViewModel> childElements;
        private IEnumerable<CommandModel> commands;
        private ElementLookup elementLookup;
        private Guid elementId = Guid.NewGuid();
        private bool inheritedFromParentConfiguration = false;
        private ExtendedPropertyContainer extendedProperties;
        private ObservableCollection<ValidationResult> validtionResults = new ObservableCollection<ValidationResult>();

        private bool promoteCommands = false;
        private ObservableCollection<Property> properties;

        /// <summary>
        /// Initializes a new instance of <see cref="ElementViewModel" />.
        /// </summary>
        /// <param name="parentElementModel">The parent <see cref="ElementViewModel"/>.</param>
        /// <param name="declaringProperty">The <see cref="PropertyDescriptor"/> describing the <see cref="ConfigurationElement"/> the <see cref="ElementViewModel"/> represents.</param>
        [InjectionConstructor]
        public ElementViewModel(ElementViewModel parentElementModel, PropertyDescriptor declaringProperty)
            : this(parentElementModel, declaringProperty, new Attribute[0])
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ElementViewModel" />.
        /// </summary>
        /// <param name="parentElementModel">The parent <see cref="ElementViewModel"/>.</param>
        /// <param name="declaringProperty">The <see cref="PropertyDescriptor"/> describing the <see cref="ConfigurationElement"/> the <see cref="ElementViewModel"/> represents.</param>        
        /// <param name="additionalAttributes">Additional metadata <see cref="Attribute"/> values to apply to this item.</param>
        public ElementViewModel(ElementViewModel parentElementModel, PropertyDescriptor declaringProperty,
                                IEnumerable<Attribute> additionalAttributes)
            : this(
                parentElementModel,
                declaringProperty.GetValue(parentElementModel.thisElement) as ConfigurationElement,
                declaringProperty.Attributes.OfType<Attribute>(),
                additionalAttributes)
        {
            this.declaringProperty = declaringProperty;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ElementViewModel" />.
        /// </summary>
        /// <param name="parentElementModel">The parent <see cref="ElementViewModel"/>.</param>
        /// <param name="thisElement">The </param>
        /// <param name="additionalAttributes">Additional metadata <see cref="Attribute"/> values to apply to this item.</param>
        protected ElementViewModel(ElementViewModel parentElementModel, ConfigurationElement thisElement,
                                   IEnumerable<Attribute> additionalAttributes)
            : this(parentElementModel,
                    thisElement,
                    TypeDescriptor.GetAttributes(thisElement).OfType<Attribute>(),
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
                this.parentElementModel.PropertyChanged += ParentPropertyChangedHandler;
            }

            promoteCommands = metadata.Attributes.OfType<PromoteCommandsAttribute>().Any();
            configurationProperty = Attributes.OfType<ConfigurationPropertyAttribute>().FirstOrDefault();
        }

        #region INeedInitialization Members

        /// <summary>
        /// Initialization for this element during the <see cref="ConfigurationSourceModel.Load"/> or <see cref="ConfigurationSourceModel.AddSection(string,System.Configuration.ConfigurationSection)"/>.
        /// </summary>
        /// <param name="context">The load context for this call</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Validated with Guard class")]
        public virtual void Initialize(InitializeContext context)
        {
            Guard.ArgumentNotNull(context, "context");

            EnsureHasChildElements();
            inheritedFromParentConfiguration = (context.WasLoadedFromSource && ConfigurationElement != null && ConfigurationElement.ElementInformation.IsPresent == false);
        }

        #endregion

        /// <summary>
        /// Injection Method used to supply additional dependencies to the <see cref="ElementViewModel"/>.
        /// </summary>
        /// <param name="elementLookup">The element registration.</param>
        /// <param name="applicationModel">The <see cref="IApplicationModel"/> this element is part of.</param>
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
            var propertyToListenFor = Property(propertyName).BindableProperty;
            if (propertyToListenFor != null)
            {
                propertyToListenFor.PropertyChanged += (sender, args) =>
                                                           {
                                                               if (args.PropertyName == "BindableValue")
                                                               {
                                                                   OnPropertyChanged(thisPropertyName);
                                                               }
                                                           };
            }
        }

        #region Validation
        ///<summary>
        /// Validates this <see cref="ElementViewModel"/> and it's <see cref="Properties"/>.
        ///</summary>
        public virtual void Validate()
        {
            ValidateElement();

            foreach (var property in Properties)
            {
                property.Validate();
            }

            foreach (var child in ChildElements)
            {
                child.Validate();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private void ValidateElement()
        {
            var validators = GetValidators();
            var results = new List<ValidationResult>();
            foreach (var validator in validators)
            {
                try
                {
                    validator.Validate(this, null, results);
                }
                catch (Exception ex)
                {
                    results.Add(new ElementValidationResult(this, string.Format(CultureInfo.CurrentCulture, Resources.ValidationErrorExceptionMessage, ex.Message), true));
                }
            }

            foreach (IDisposable error in validtionResults)
            {
                error.Dispose();
            }

            if (validtionResults.Any())
            {
                validtionResults.Clear();
            }

            foreach (var result in results)
            {
                validtionResults.Add(result);
            }
        }


        /// <summary>
        /// Retrieves the <see cref="Validator"/> items for this element.
        /// </summary>
        /// <remarks>
        /// Element <see cref="Validator"/> items are specified by using the <see cref="ElementValidationAttribute"/>.</remarks>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Validators")]
        protected virtual IEnumerable<Validator> GetValidators()
        {
            return Attributes.OfType<ElementValidationAttribute>()
                .Select(v => ContainingSection.CreateValidatorInstance(v.ValidatorType));
        }

        ///<summary>
        /// The validation results for this <see cref="ElementViewModel"/>.
        ///</summary>
        public virtual IEnumerable<ValidationResult> ValidationResults
        {
            get
            {
                return validtionResults;
            }
        }

        #endregion


        #region Event Subscription
        private void ParentPropertyChangedHandler(object o, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Path") OnPropertyChanged("Path");
        }

        #endregion

        /// <summary>
        /// Makes this element the selected one on the <see cref="IApplicationModel"/>.
        /// </summary>
        public void Select()
        {
            applicationModel.OnSelectedElementChanged(this);
            IsSelected = true;
        }

        private bool isSelected;
        
        /// <summary>
        /// Gets or sets the value indicating this element is selected.
        /// </summary>
        public bool IsSelected
        {
            get
            {
                return isSelected;
            }
            internal set
            {
                if (isSelected != value)
                {
                    isSelected = value;
                    OnPropertyChanged("IsSelected");
                }
            }
        }


        private bool propertiesShown;
        /// <summary>
        /// Gets or sets the value indicating if this element's <see cref="Properties"/> are shown in the view.
        /// </summary>
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

        private object bindable;
        private IEnumerable<ElementViewModel> cachedAncestors;

        /// <summary>
        /// Creates the element the view binds to.  By default it is the element itself.
        /// </summary>
        /// <returns></returns>
        protected virtual object CreateBindable()
        {
            return this;
        }

        /// <summary>
        /// Gets the element the view should bind to.  By default it is the element itself.
        /// </summary>
        /// <remarks>This is the cached bindable value created from <see cref="CreateBindable"/>.</remarks>
        /// <seealso cref="CreateBindable"/>
        public override object Bindable
        {
            get
            {
                return bindable ?? (bindable = CreateBindable());
            }
        }

        #region Identity

        /// <summary>
        /// Gets or sets a value indicating that this configuratio was inherited from a parent configuration (such as machine.config).
        /// </summary>
        public bool InheritedFromParentConfiguration
        {
            get { return inheritedFromParentConfiguration; }

            //setter available for unittesting.
            set { inheritedFromParentConfiguration = value; }
        }

        /// <summary>
        /// Gets a unique identifier for this element.        
        /// </summary>
        /// <remarks>
        /// This identifier is unique within the lifetime of this element instance, but is not persisted in any way.
        /// </remarks>
        public Guid ElementId
        {
            get { return elementId; }
        }

        
        /// <summary>
        /// Gets a value indicating that the underlying <see cref="ConfigurationElement"/> is <see langword="null"/>
        /// </summary>
        //property with null-value.
        //empty leaf
        public bool IsNull
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
            get { return thisElement == null ? declaringProperty.PropertyType : thisElement.GetType(); }
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
            if (configurationProperty == null) return declaringProperty.Name;
            return configurationProperty.Name;
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
        /// Gets the name of the <see cref="ElementViewModel"/> for use in the user-interface.<br/>
        /// </summary>
        public virtual string Name
        {
            get
            {
                var namePropertyAttribute = metadata.Attributes.OfType<NamePropertyAttribute>().FirstOrDefault();
                if (namePropertyAttribute != null)
                {
                    var namePropertiesDisplay = string.Format(CultureInfo.CurrentCulture, namePropertyAttribute.NamePropertyDisplayFormat,
                                                              Property(namePropertyAttribute.PropertyName).BindableProperty.BindableValue);
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

        /// <summary>
        /// Gets the property that provides the 'name' for the element.
        /// </summary>
        public virtual Property NameProperty
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
        /// Gets the <see cref="NameProperty"/> internval value as a string representation.
        /// </summary>
        public string NamePropertyInternalValue
        {
            get
            {
                if (NameProperty != null)
                {
                    return NameProperty.Value as string;
                }
                return null;
            }
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
        /// Invoked when this element's <see cref="ChildElements"/> collection is changed.
        /// </summary>
        public event NotifyCollectionChangedEventHandler ChildElementsCollectionChange;

        /// <summary>
        /// Removes an element from the <see cref="ChildElements"/> collection.
        /// </summary>
        /// <param name="element"></param>
        protected void RemoveChildFromView(ElementViewModel element)
        {
            EnsureHasChildElements();
            childElements.Remove(element);
        }

        /// <summary>
        /// Adds an element to the <see cref="ChildElements"/> collection.
        /// </summary>
        /// <param name="element"></param>
        protected void AddChildToView(ElementViewModel element)
        {
            EnsureHasChildElements();
            childElements.Add(element);
        }

        /// <summary>
        /// Gets all <see cref="ElementViewModel"/> instances contained as direct child elements.
        /// </summary>
        public IEnumerable<ElementViewModel> ChildElements
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
                return AncestorElements().OfType<SectionViewModel>().First();
            }
        }

        /// <summary>
        /// Gets all the <see cref="Property"/> instances that are part of this <see cref="ElementViewModel"/> instance.
        /// </summary>
        /// <returns>
        /// Returns an un evaluated iterator class. <br/>
        /// </returns>
        protected virtual IEnumerable<Property> GetAllProperties()
        {
            var declaredProperties = TypeDescriptor.GetProperties(thisElement)
                .OfType<PropertyDescriptor>()
                .Where(x => !typeof(ConfigurationElement).IsAssignableFrom(x.PropertyType) || (x.Attributes.OfType<EditorAttribute>().Any())) //filter out configuration elements
                .Where(x => x.Attributes.OfType<ConfigurationPropertyAttribute>().Any())
                .Select(x => ContainingSection.CreateElementProperty(this, x))
                .OfType<Property>();

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
        /// Gets all the <see cref="ElementViewModel"/> instances that are directly contained in this <see cref="ElementViewModel"/> instance.
        /// </summary>
        /// <returns>
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


        /// <summary>
        /// Ensures that that the children of this element have been discovered and populated for <see cref="ChildElements"/>.
        /// </summary>
        protected void EnsureHasChildElements()
        {
            if (childElements == null)
            {
                childElements = new ObservableCollection<ElementViewModel>(GetAllChildElements());
                childElements.CollectionChanged += (sender, args) =>
                {
                    var childElementHandler = ChildElementsCollectionChange;
                    if (childElementHandler != null)
                    {
                        childElementHandler(sender, args);
                    }

                    OnDescendentElementsChanged(this, args);
                };
            }
        }

        /// <summary>
        /// Retrieves a child element whose property name matches the specified property name.
        /// </summary>
        /// <param name="propertyName">The property name to match to the child element's property name.</param>
        /// <returns>The located child element or <see langword="null"/></returns>
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
        /// Returns all descending <see cref="ElementViewModel"/> instances relative to this <see cref="ElementViewModel"/>
        /// whose <see cref="ConfigurationType"/> is assignable to <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>An empty set or a set <see cref="ElementViewModel"/> items.</returns>
        public IEnumerable<ElementViewModel> DescendentConfigurationsOfType<T>()
            where T : ConfigurationElement
        {
            return DescendentElements(x => typeof(T).IsAssignableFrom(x.ConfigurationType));
        }

        /// <summary>
        /// Gets all ancestors <see cref="ElementViewModel"/> for this <see cref="ElementViewModel"/> instance.
        /// </summary>
        public IEnumerable<ElementViewModel> AncestorElements()
        {
            if (cachedAncestors == null)
            {
                cachedAncestors = CollectAncestors().ToArray();
            }

            return cachedAncestors;
        }

        private IEnumerable<ElementViewModel> CollectAncestors()
        {
            if (ParentElement != null)
            {
                yield return ParentElement;

                foreach (var grandParent in ParentElement.AncestorElements())
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
        /// The <see cref="CommandModel"/> command associated with this configuration element.
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
                .Union(CreateOtherCommands())
                .Union(CreateCustomCommands())
                .Union(GetPromotedCommands());
        }

        /// <summary>
        /// Creates a delete <see cref="CommandModel"/> for this element based on it's <see cref="Attributes"/>.
        /// </summary>
        /// <returns></returns>
        protected IEnumerable<CommandModel> CreateDeleteCommand()
        {
            yield return ContainingSection.CreateDeleteCommand(this, Attributes);
        }

        /// <summary>
        /// Creates any custom commands for this element based on it's <see cref="Attributes"/>.
        /// </summary>
        /// <returns></returns>
        protected IEnumerable<CommandModel> CreateCustomCommands()
        {
            foreach (var command in ContainingSection.CreateCustomCommands(this, Attributes))
            {
                yield return command;
            }
        }

        /// <summary>
        /// Creates commands other than Add, Delete, or custom command specified via attribute, for this element.
        /// </summary>
        /// <returns>
        /// An enumerable containing instances of <see cref="ToggleShowPropertiesCommand"/> and <see cref="ValidateCommand"/>.
        /// </returns>
        protected IEnumerable<CommandModel> CreateOtherCommands()
        {
            yield return ContainingSection.CreateCommand<ToggleShowPropertiesCommand>(
                new Dictionary<Type, object>() { { typeof(ElementViewModel), this } });
            yield return ContainingSection.CreateCommand<ValidateCommand>(
                new Dictionary<Type, object>() { { typeof(ElementViewModel), this } });
        }

        /// <summary>
        /// Gets any commands promoted by their child elements.
        /// </summary>
        /// <remarks>
        /// Commands promoted by children via the <see cref="PromoteCommandsAttribute"/> appear in their parent's <see cref="Commands"/> list.</remarks>
        /// <returns></returns>
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
            DeclaringProperty.SetValue(ParentElement.ConfigurationElement, null);
            OnDeleted();
        }

        #endregion

        #region IEnvironmentalOverridesElement Members

        /// <summary>
        /// Gets a value indicating that this Element's <see cref="Path"/> is reliable 
        /// </summary>
        public virtual bool IsElementPathReliableXPath
        {
            get
            {
                if (ParentElement != null && !ParentElement.IsElementPathReliableXPath) return false;

                if (ConfigurationElement == null) return false;

                //we need this to be sure we have a reliable xpath.
                if (configurationProperty == null) return false;

                if (InheritedFromParentConfiguration) return false;

                return true;
            }
        }

        #endregion

        #region Events and Event Invocation

        /// <summary>
        /// Raised when elements visually related to this element change.
        /// </summary>
        public event EventHandler ElementReferencesChanged;

        /// <summary>
        /// Invokes the <see cref="ElementReferencesChanged"/> event.
        /// </summary>
        protected internal void OnElementReferencesChanged()
        {
            var handler = ElementReferencesChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }


        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Invokes the <see cref="Deleted"/> event.
        /// </summary>
        protected internal void OnDeleted()
        {
            var handler = Deleted;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Raised when this element is deleted.
        /// </summary>
        public event EventHandler Deleted;

        /// <summary>
        /// Indicates the object is being disposed.
        /// </summary>
        /// <param name="disposing">Indicates <see cref="ViewModel.Dispose(bool)"/> was invoked through an explicit call to <see cref="ViewModel.Dispose()"/> instead of a finalizer call.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (parentElementModel != null)
                {
                    this.parentElementModel.PropertyChanged -= ParentPropertyChangedHandler;
                }

                if (properties != null)
                {
                    foreach (IDisposable prop in properties)
                    {
                        prop.Dispose();
                    }

                    properties.Clear();
                }

                if (extendedProperties != null)
                {
                    extendedProperties.Dispose();
                    extendedProperties = null;
                }

                if (childElements != null)
                {
                    foreach (var child in childElements)
                    {
                        child.Dispose();
                    }
                }
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Raises the <see cref="DescendentElementsChanged"/> event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="collectionChanged"></param>
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

        /// <summary>
        /// Raised when <see cref="DescendentElements()"/> change.
        /// </summary>
        public event NotifyCollectionChangedEventHandler DescendentElementsChanged;

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName"></param>
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

        private class ExtendedPropertyContainer : IDisposable
        {
            private IElementChangeScope extendedPropetryProviders;
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
                extendedPropetryProviders.CollectionChanged += extendedPropetryProviders_CollectionChanged;

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
                        property.Dispose();
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
                        extendedProperty.Initialize(new InitializeContext());
                        properties.Add(extendedProperty);
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

            #region IDisposable Members

            public void Dispose()
            {
                if (extendedPropetryProviders != null)
                {
                    extendedPropetryProviders.CollectionChanged -= extendedPropetryProviders_CollectionChanged;
                    extendedPropetryProviders.Dispose();
                    extendedPropetryProviders = null;
                }

                GC.SuppressFinalize(this);
            }

            #endregion
        }

        #endregion

        private class ToggleShowPropertiesCommand : CommandModel
        {
            private readonly ElementViewModel viewModel;

            public ToggleShowPropertiesCommand(ElementViewModel viewModel, IUIServiceWpf uiService)
                : base(uiService)
            {
                this.viewModel = viewModel;
            }

            public override string Title
            {
                get
                {
                    return Resources.ToggleShowPropertiesCommandTitle;
                }
            }

            protected override bool InnerCanExecute(object parameter)
            {
                return true;
            }

            protected override void InnerExecute(object parameter)
            {
                viewModel.PropertiesShown = !viewModel.PropertiesShown;
            }


            public override string KeyGesture
            {
                get
                {
                    return new KeyGestureConverter().ConvertToInvariantString(new KeyGesture(Key.Space));
                }
            }
        }

        private class ValidateCommand : CommandModel
        {
            private readonly ElementViewModel viewModel;
            private KeyGestureConverter keyGestureConverter;

            public ValidateCommand(ElementViewModel viewModel, IUIServiceWpf uiService)
                : base(uiService)
            {
                this.viewModel = viewModel;
                this.keyGestureConverter = new KeyGestureConverter();
            }

            public override string Title
            {
                get
                {
                    return Resources.ValidateCommandTitle;
                }
            }

            protected override bool InnerCanExecute(object parameter)
            {
                return true;
            }

            protected override void InnerExecute(object parameter)
            {
                this.viewModel.Validate();
            }

            public override string KeyGesture
            {
                get
                {
                    return
                        keyGestureConverter.ConvertToInvariantString(new KeyGesture(Key.V,
                                                                                    ModifierKeys.Control |
                                                                                    ModifierKeys.Shift));
                }
            }
        }
    }
}
