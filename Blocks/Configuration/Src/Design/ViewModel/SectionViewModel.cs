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
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Windows.Input;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Extensions;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Commands;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Utility;
using System.Collections.ObjectModel;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{
    /// <summary>
    /// The <see cref="SectionViewModel"/> represents a <see cref="ConfigurationSection"/> in the enterprise
    /// library configuration designer.
    /// </summary>
    public class SectionViewModel : ElementViewModel
    {
        RequirePermissionProperty requirePermissionProperty;
        ProtectionProviderProperty protectionProviderProperty;
        IUnityContainer builder;
        string configurationSectionName;

        ///<summary>
        /// Initializes a new instance of <see cref="SectionViewModel"/>.
        ///</summary>
        ///<param name="builder">The factory to use when creating new elements within the section.</param>
        ///<param name="sectionName">The name of the section.</param>
        ///<param name="section">The underlying configuration section represented.</param>
        [InjectionConstructor]
        public SectionViewModel(IUnityContainer builder, string sectionName, ConfigurationSection section)
            : this(builder, sectionName, section, Enumerable.Empty<Attribute>())
        {
        }

        ///<summary>
        /// Initializes a new instance of <see cref="SectionViewModel"/>.
        ///</summary>
        ///<param name="builder">The factory to use when creating new elements within the section.</param>
        ///<param name="sectionName">The name of the section.</param>
        ///<param name="section">The underlying configuration section represented.</param>
        ///<param name="metadataAttributes">Additional <see cref="Attribute"/> instances to apply to this section.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Justification = "As designed")]
        public SectionViewModel(IUnityContainer builder, string sectionName, ConfigurationSection section, IEnumerable<Attribute> metadataAttributes)
            : base(null, section, metadataAttributes)
        {
            this.builder = builder;

            configurationSectionName = sectionName;

            requirePermissionProperty = CreateProperty<RequirePermissionProperty>();
            requirePermissionProperty.Value = section.SectionInformation.RequirePermission;

            protectionProviderProperty = CreateProperty<ProtectionProviderProperty>();
            if (section.SectionInformation.IsProtected)
            {
                protectionProviderProperty.Value = section.SectionInformation.ProtectionProvider.Name;
            }
        }

        /// <summary>
        /// Creates the element the view binds to.  By default it is the element itself.
        /// </summary>
        protected override object CreateBindable()
        {
            return null;
        }

        private bool isExpanded;
        ///<summary>
        /// Gets or sets a value indicating if the section is expanded in the user-interface.
        ///</summary>
        public bool IsExpanded
        {
            get { return isExpanded; }
            set
            {
                isExpanded = value;
                OnPropertyChanged("IsExpanded");
            }
        }

        ///<summary>
        /// Expands the section in the user-interface.
        ///</summary>
        public virtual void ExpandSection()
        {
            IsExpanded = true;
        }

        ///<summary>
        /// Deletes this element.
        ///</summary>
        public override void Delete()
        {
            var configurationSource = builder.Resolve<ConfigurationSourceModel>();
            configurationSource.RemoveSection(configurationSectionName);
        }

        ///<summary>
        /// Gets the name of this section.
        ///</summary>
        public string SectionName
        {
            get { return configurationSectionName; }
        }

        ///<summary>
        /// Invoked by <see cref="Save"/> before a section is saved.
        ///</summary>
        ///<param name="sectionToSave">The section to save.</param>
        ///<remarks>
        /// Provides an opportunity for the section to anything necessary before saving by <see cref="Save"/>.
        /// </remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Validated with Guard class")]
        protected virtual void BeforeSave(ConfigurationSection sectionToSave)
        {
            Guard.ArgumentNotNull(sectionToSave, "sectionToSave");

            sectionToSave.SectionInformation.RequirePermission = requirePermissionProperty.TypedValue;
        }

        ///<summary>
        /// Invoked so the section will save itself to the <see cref="IDesignConfigurationSource"/>.
        ///</summary>
        ///<param name="configurationSource">The configuration source to save to.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Validated with Guard class")]
        public virtual void Save(IDesignConfigurationSource configurationSource)
        {
            Guard.ArgumentNotNull(configurationSource, "configurationSource");

            ConfigurationSectionCloner cloner = new ConfigurationSectionCloner();
            var savableSection = cloner.Clone((ConfigurationSection)ConfigurationElement);

            BeforeSave(savableSection);

            configurationSource.RemoveLocalSection(configurationSectionName);

            if (protectionProviderProperty.NeedsProtectionProvider)
            {
                configurationSource.Add(configurationSectionName, savableSection, protectionProviderProperty.TypedValue);
            }
            else
            {
                configurationSource.AddLocalSection(configurationSectionName, savableSection);
            }
        }

        ///<summary>
        /// Gets the protection provider property.
        ///</summary>
        public ProtectionProviderProperty ProtectionProviderProperty
        {
            get { return protectionProviderProperty; }
        }

        /// <summary>
        /// Gets the require permission property.
        /// </summary>
        public RequirePermissionProperty RequirePermissionProperty
        {
            get { return requirePermissionProperty; }
        }


        /// <summary>
        /// Gets all the <see cref="ElementViewModel.Property"/> instances that are part of this <see cref="ElementViewModel"/> instance.
        /// </summary>
        /// <returns>
        /// Returns an un evaluated iterator class. <br/>
        /// </returns>
        protected override IEnumerable<Property> GetAllProperties()
        {
            return base.GetAllProperties().Union(new Property[] { requirePermissionProperty, protectionProviderProperty });
        }

        /// <summary>
        /// Gets a value indicating that this Element's <see cref="ElementViewModel.Path"/> is reliable 
        /// </summary>
        public override bool IsElementPathReliableXPath
        {
            get { return true; }
        }

        /// <summary>
        /// Gets a string that can be appended to the parent's <see cref="ElementViewModel.Path"/> to compose a <see cref="ElementViewModel.Path"/> used to uniquely identify this <see cref="ElementViewModel"/>. <br/>
        /// </summary>
        protected override string GetLocalPathPart()
        {
            return string.Format(CultureInfo.InvariantCulture, "configuration/{0}", configurationSectionName);
        }

        ///<summary>
        /// Determines all the elements visually related to the provided <see cref="ElementViewModel"/> within this section.
        ///</summary>
        ///<param name="element">The element to find related elements to.</param>
        ///<returns>The set of visually related elements.</returns>
        public virtual IEnumerable<ElementViewModel> GetRelatedElements(ElementViewModel element)
        {
            Collection<ElementViewModel> relatedElements = new Collection<ElementViewModel>();

            //elements we refer to
            AddReferredToRelatedElements(element, relatedElements);

            //elements that refer to us.
            AddReferredFromRelatedElements(element, relatedElements);

            //our parent
            AddNonCollectionParentRelatedElement(element, relatedElements);

            //our children
            AddChildrenRelatedElements(element, relatedElements);

            //for children that are collection, their children as well.
            AddCollectionChildrenRelatedElement(element, relatedElements);

            //that should be it. by default.
            return relatedElements;
        }

        /// <summary>
        /// Adds elements that an element visually refers to in within the section.
        /// </summary>
        /// <param name="element">The element to find related elements for.</param>
        /// <param name="relatedElements">The set of related elements.</param>
        protected static void AddReferredToRelatedElements(ElementViewModel element, Collection<ElementViewModel> relatedElements)
        {
            relatedElements.AddRange(element.Properties.OfType<ElementReferenceProperty>()
                                                       .Where(x => x.ReferencedElement != null)
                                                       .Select(x => x.ReferencedElement));
        }

        /// <summary>
        /// Adds elements that visually refer to an element within the section.
        /// </summary>
        /// <param name="element">The element to find related elements for.</param>
        /// <param name="relatedElements">The set of related elements.</param>
        protected void AddReferredFromRelatedElements(ElementViewModel element, Collection<ElementViewModel> relatedElements)
        {
            relatedElements.AddRange(DescendentElements(x => x.Properties.OfType<ElementReferenceProperty>().Where(y => y.ReferencedElement == element).Any()));
        }

        /// <summary>
        /// Adds any elements related to the specified element that are not collection parents.
        /// </summary>
        /// <param name="element">The element to find related elements for.</param>
        /// <param name="relatedElements">The set of related elements.</param>
        protected static void AddNonCollectionParentRelatedElement(ElementViewModel element, Collection<ElementViewModel> relatedElements)
        {
            var firstVisibleParent = element.AncestorElements().Where(x => null == x as ElementCollectionViewModel).FirstOrDefault();
            if (firstVisibleParent != null)
            {
                relatedElements.Add(firstVisibleParent);
            }
        }

        /// <summary>
        /// Adds any related child elements to the specified element.
        /// </summary>
        /// <param name="element">The element to find related elements for.</param>
        /// <param name="relatedElements">The set of related elements.</param>
        protected static void AddChildrenRelatedElements(ElementViewModel element, Collection<ElementViewModel> relatedElements)
        {
            relatedElements.AddRange(element.ChildElements);
        }

        /// <summary>
        /// Adds children of any child collections related this the specified element.
        /// </summary>
        /// <param name="element">The element to find related elements for.</param>
        /// <param name="relatedElements">The set of related elements.</param>
        protected static void AddCollectionChildrenRelatedElement(ElementViewModel element, Collection<ElementViewModel> relatedElements)
        {
            relatedElements.AddRange(element.ChildElements.OfType<ElementCollectionViewModel>().SelectMany(x => x.ChildElements).OfType<CollectionElementViewModel>().Cast<ElementViewModel>());
        }

        /// <summary>
        /// Creates or collections all the commands related to this <see cref="ElementViewModel"/>.
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<CommandModel> GetAllCommands()
        {
            return CreateSectionDeleteCommand()
               .Union(CreateOtherCommands())
               .Union(CreateCustomCommands())
               .Union(GetPromotedCommands())
               .Union(GetOtherSectionCommands());
        }

        private IEnumerable<CommandModel> CreateSectionDeleteCommand()
        {
            yield return ContainingSection.CreateDeleteCommand(this, Attributes, typeof(DefaultSectionDeleteCommandModel));
        }

        private IEnumerable<CommandModel> GetOtherSectionCommands()
        {
            var overrides = new Dictionary<Type, object>() { { typeof(SectionViewModel), this } };
            yield return ContainingSection.CreateCommand<ToggleExpandedCommand>(overrides);
        }

        #region  Create XXX methods

        /// <summary>
        /// Creates the collection element add command(s) for the given element type within the <see cref="ElementCollectionViewModel"/>.
        /// </summary>
        /// <param name="elementType">The <see cref="ConfigurationElement"/> <see cref="Type"/> to create add commands for.</param>
        /// <param name="collection">The collection to which new elements of <paramref name="elementType"/> will be added.</param>
        /// <returns>The set of add commands.</returns>
        /// <exception>Errors in creating the underlying object may result in a <see cref="ResolutionFailedException"/></exception>
        public virtual IEnumerable<CommandModel> CreateCollectionElementAddCommand(Type elementType, ElementCollectionViewModel collection)
        {
            yield return CreateDefaultCollectionElementAddCommand(elementType, collection);

            foreach (var customCommand in CreateCustomAddCommands(elementType, collection))
            {
                yield return customCommand;
            }
        }

        ///<summary>
        /// Creates the a delete <see cref="CommandModel"/> for an <see cref="ElementViewModel"/>.
        ///</summary>
        ///<param name="elementViewModel">The element the delete command applies to.</param>
        ///<param name="attributes">Attributes that may override the type of delete command created.</param>
        ///<param name="defaultCommandModelType">The default delete command <see cref="Type"/>.</param>
        ///<returns>The delete command.</returns>
        /// <remarks>
        /// Seeks to find a replacement delete command by seeking a <see cref="CommandAttribute"/> in <paramref name="attributes"/> that replaces the default command.<br/>
        /// If a replacement cannot be found, then creates and returns the command using the <paramref name="defaultCommandModelType"/>.
        /// </remarks>
        /// <exception>Errors in creating the underlying object may result in a <see cref="ResolutionFailedException"/></exception>
        public virtual CommandModel CreateDeleteCommand(ElementViewModel elementViewModel, IEnumerable<Attribute> attributes, Type defaultCommandModelType)
        {
            var deleteCommandReplacement = attributes.OfType<CommandAttribute>().Where(x => x.Replace == CommandReplacement.DefaultDeleteCommandReplacement).FirstOrDefault();
            if (deleteCommandReplacement != null)
            {
                return (CommandModel)builder.Resolve(deleteCommandReplacement.CommandModelType,
                                            new DependencyOverride(deleteCommandReplacement.GetType(), deleteCommandReplacement),
                                            new DependencyOverride<ElementViewModel>(elementViewModel));
            }
            return (CommandModel)builder.Resolve(defaultCommandModelType, new DependencyOverride<ElementViewModel>(elementViewModel));
        }

        ///<summary>
        /// Creates a delete command for the supplied <see cref="ElementViewModel"/>.
        ///</summary>
        ///<param name="elementViewModel">The element the delete command applies to.</param>
        ///<param name="attributes">Attributes that may override the type of delete command created.</param>
        ///<returns>A delete command for the element based on a <see cref="CommandAttribute"/> in <paramref name="attributes"/> or an instance of <see cref="DefaultDeleteCommandModel"/>.</returns>
        ///<seealso cref="CreateDeleteCommand(Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.ElementViewModel,System.Collections.Generic.IEnumerable{System.Attribute},System.Type)"/>
        ///<exception>Errors in creating the underlying object may result in a <see cref="ResolutionFailedException"/></exception>
        public CommandModel CreateDeleteCommand(ElementViewModel elementViewModel, IEnumerable<Attribute> attributes)
        {
            return CreateDeleteCommand(elementViewModel, attributes, typeof(DefaultDeleteCommandModel));
        }

        ///<summary>
        /// Creates the add command for an <see cref="ElementCollectionViewModel"/>.
        ///</summary>
        ///<param name="attributes">The attributes to consider when creating the command.</param>
        ///<param name="collection">The <see cref="ElementCollectionViewModel"/> to create the add command for.</param>
        ///<returns>
        /// If there is a replacement add command in the <paramref name="attributes"/> set, a command of that type is returned.
        ///<br/>
        /// If the collection is a polymorphic collection or there are multiple add commands, then a command of type <see cref="DefaultElementCollectionAddCommand"/> will be returned.
        /// <br/>
        /// If the collection is not polymorphic, then the add command for the <see cref="ElementCollectionViewModel.CollectionElementType"/> is returned
        /// since the collection only has one type that can be added for it and we don't want the user to navigate multiple menus unnecessarily.
        ///</returns>
        /// <exception>Errors in creating the underlying object may result in a <see cref="ResolutionFailedException"/></exception>
        public virtual CommandModel CreateElementCollectionAddCommands(IEnumerable<Attribute> attributes, ElementCollectionViewModel collection)
        {
            var addCommandReplacement = attributes.OfType<CommandAttribute>().Where(x => x.Replace == CommandReplacement.DefaultAddCommandReplacement).FirstOrDefault();
            if (addCommandReplacement != null)
            {
                return (CommandModel)builder.Resolve(addCommandReplacement.CommandModelType,
                                            new DependencyOverride(addCommandReplacement.GetType(), addCommandReplacement),
                                            new DependencyOverride(typeof(ElementCollectionViewModel), collection));
            }

            return CreateDefaultElementCollectionAddCommand(collection);
        }

        /// <summary>
        /// Creates custom commands for an <see cref="ElementViewModel"/>.
        /// </summary>
        /// <param name="target">The target <see cref="ElementViewModel"/> to build custom commands for.</param>
        /// <param name="attributes">The set of <see cref="Attribute"/> values to consider when creating the custom commands.</param>
        /// <returns>A set of custom commands.</returns>
        /// <remarks>
        /// Custom commands are determined by the <see cref="CommandAttribute"/> with a <see cref="CommandPlacement"/> of <see cref="CommandPlacement.ContextCustom"/>.
        /// </remarks>
        /// <exception>Errors in creating the underlying object may result in a <see cref="ResolutionFailedException"/></exception>
        public virtual IEnumerable<CommandModel> CreateCustomCommands(ElementViewModel target, IEnumerable<Attribute> attributes)
        {
            foreach (CommandAttribute command in attributes.OfType<CommandAttribute>().Where(x => x.CommandPlacement == CommandPlacement.ContextCustom))
            {
                yield return (CommandModel)builder.Resolve(command.CommandModelType,
                    new DependencyOverride(typeof(ElementViewModel), target),
                    new DependencyOverride(command.GetType(), command));
            }
        }

        /// <summary>
        /// Creates a <see cref="CommandModel"/> applying override instances.
        /// </summary>
        /// <typeparam name="TCommandModel">The <see cref="CommandModel"/> to create.</typeparam>
        /// <param name="overrides">The dictionary of type and instances to apply as overrides during construction of the <typeparamref name="TCommandModel"/>.</param>
        /// <returns>The <see cref="CommandModel"/> created.</returns>
        /// <exception>Errors in creating the underlying object may result in a <see cref="ResolutionFailedException"/></exception>
        public TCommandModel CreateCommand<TCommandModel>(Dictionary<Type, object> overrides)
            where TCommandModel : CommandModel
        {
            var dependencyOverrides = overrides.Select(x => new DependencyOverride(x.Key, x.Value)).ToArray();
            return builder.Resolve<TCommandModel>(dependencyOverrides);
        }

        ///<summary>
        /// Creates a <see cref="CommandModel"/> with the specified <see cref="ElementViewModel"/> as a constructor parameter.
        ///</summary>
        ///<param name="target">The <see cref="ElementViewModel"/> to provide as constructor context for the command.</param>
        ///<typeparam name="TCommandModel">The <see cref="CommandModel"/> type to create.</typeparam>
        ///<returns>The created <see cref="CommandModel"/></returns>
        /// <exception>Errors in creating the underlying object may result in a <see cref="ResolutionFailedException"/></exception>
        public TCommandModel CreateCommand<TCommandModel>(ElementViewModel target)
            where TCommandModel : CommandModel
        {
            return builder.Resolve<TCommandModel>(new DependencyOverride(typeof(ElementViewModel), target));
        }

        /// <summary>
        /// Creates a child <see cref="ElementViewModel"/> based on a <see cref="PropertyDescriptor"/>.
        /// </summary>
        /// <param name="containingViewModel">The parent <see cref="ElementViewModel"/> for the new element.</param>
        /// <param name="declaringProperty">The <see cref="PropertyDescriptor"/> defining the child element.</param>
        /// <returns>
        /// The child <see cref="ElementViewModel"/> based on the <paramref name="declaringProperty"/> descriptor,
        /// or a custom one based on the <see cref="ViewModelAttribute"/>.<br/>
        /// If the <paramref name="declaringProperty"/> is a collection, the return value will derive from <see cref="ElementCollectionViewModel"/><br/>
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Validated with Guard class")]
        public virtual ElementViewModel CreateChild(ElementViewModel containingViewModel, PropertyDescriptor declaringProperty)
        {
            Guard.ArgumentNotNull(declaringProperty, "declaringProperty");

            if (typeof(ConfigurationElementCollection).IsAssignableFrom(declaringProperty.PropertyType))
            {
                return CreateElementCollection(containingViewModel, declaringProperty);
            }
            return CreateElement(containingViewModel, declaringProperty);
        }

        /// <summary>
        /// Creates a <see cref="CollectionElementViewModel"/> based on a <see cref="ConfigurationElement"/>.
        /// </summary>
        /// <param name="parent">The parent of the created element.</param>
        /// <param name="containedElement">The <see cref="ConfigurationElement"/> for the contained element.</param>
        /// <returns>
        /// The created <see cref="CollectionElementViewModel"/> or a derived custom view model specified by the <see cref="ViewModelAttribute"/>.
        /// </returns>
        public virtual CollectionElementViewModel CreateCollectionElement(ElementCollectionViewModel parent, ConfigurationElement containedElement)
        {
            IEnumerable<Attribute> metadataForCreatingViewModel = TypeDescriptor.GetAttributes(containedElement).OfType<Attribute>();

            return CreateViewModelInstance<CollectionElementViewModel>(
                builder,
                metadataForCreatingViewModel,
                new DependencyOverride(typeof(ElementCollectionViewModel), parent),
                new DependencyOverride(typeof(ConfigurationElement), containedElement),
                new DependencyOverride(typeof(SectionViewModel), this));
        }

        ///<summary>
        /// Creates a <see cref="ElementCollectionViewModel"/> based on a <see cref="PropertyDescriptor"/>.
        ///</summary>
        ///<param name="parent">The parent <see cref="ElementViewModel"/> of the created element.</param>
        ///<param name="declaringProperty">The declaring property whose type should be a <see cref="ConfigurationElementCollection"/> derived type.
        ///</param>
        ///<returns>
        ///The created <see cref="ElementCollectionViewModel"/> or a derived custom view model specified by the <see cref="ViewModelAttribute"/>.
        ///</returns>
        public virtual ElementCollectionViewModel CreateElementCollection(ElementViewModel parent, PropertyDescriptor declaringProperty)
        {
            IEnumerable<Attribute> metadataForCreatingViewModel = GetMetadataAttributesFromProperty(declaringProperty);

            return CreateViewModelInstance<ElementCollectionViewModel>(
                builder,
                metadataForCreatingViewModel,
                new DependencyOverride(typeof(ElementViewModel), parent),
                new DependencyOverride(typeof(PropertyDescriptor), declaringProperty),
                new DependencyOverride(typeof(SectionViewModel), this));
        }

        /// <summary>
        /// Creates an <see cref="ElementViewModel"/> based on a <see cref="PropertyDescriptor"/>.
        /// </summary>
        /// <param name="parent">The parent for the created element.</param>
        /// <param name="declaringProperty">The property descriptor for the element.</param>
        /// <returns>
        /// The created <see cref="ElementViewModel"/> or a custom one specified by a <see cref="ViewModelAttribute"/>.
        /// </returns>
        public virtual ElementViewModel CreateElement(ElementViewModel parent, PropertyDescriptor declaringProperty)
        {
            IEnumerable<Attribute> metadataForCreatingViewModel = GetMetadataAttributesFromProperty(declaringProperty);

            return CreateViewModelInstance<ElementViewModel>(
                builder,
                metadataForCreatingViewModel,
                new DependencyOverride(typeof(ElementViewModel), parent),
                new DependencyOverride(typeof(PropertyDescriptor), declaringProperty),
                new DependencyOverride(typeof(SectionViewModel), this));
        }

        /// <summary>
        /// Creates a <see cref="Property"/> from a <see cref="PropertyDescriptor"/>.
        /// </summary>
        /// <param name="component">The <see cref="object"/> containing the property described by <paramref name="declaringProperty"/>.</param>
        /// <param name="declaringProperty">The descriptor for the property.</param>
        /// <returns>The created <see cref="Property"/> or a custom one as specified by <see cref="ViewModelAttribute"/>.</returns>
        public virtual Property CreateProperty(object component, PropertyDescriptor declaringProperty)
        {
            IEnumerable<Attribute> metadataForCreatingProperty = GetMetadataAttributesFromProperty(declaringProperty);

            return CreateViewModelInstance<Property>(
                builder,
                metadataForCreatingProperty,
                new DependencyOverride(typeof(object), component),
                new DependencyOverride(typeof(PropertyDescriptor), declaringProperty),
                new DependencyOverride(typeof(SectionViewModel), this));

        }
        
        /// <summary>
        /// Creates a <see cref="Property"/>.
        /// </summary>
        /// <typeparam name="TPropertyViewModel">The type of <see cref="Property"/> to create.</typeparam>
        /// <param name="overrides">The overrides to apply during construction of the property.</param>
        /// <returns>A new <see cref="Property"/> or a custom one specified by the <see cref="ViewModelAttribute"/>.</returns>
        public virtual TPropertyViewModel CreateProperty<TPropertyViewModel>(params ResolverOverride[] overrides)
            where TPropertyViewModel : Property
        {
            return (TPropertyViewModel)CreateProperty(typeof(TPropertyViewModel), overrides);
        }

        /// <summary>
        /// Creates a <see cref="Property"/> of the specified <see cref="Type"/>.
        /// </summary>
        /// <param name="propertyViewModelType">The <see cref="Type"/> of <see cref="Property"/>to create.</param>
        /// <param name="overrides">Overrides to apply during construction of the property.</param>
        /// <returns>A new <see cref="Property"/> or a custom one specified by the <see cref="ViewModelAttribute"/>.</returns>
        public virtual Property CreateProperty(Type propertyViewModelType, params ResolverOverride[] overrides)
        {
            ResolverOverride[] defaults = new[] { new DependencyOverride(typeof(SectionViewModel), this) };

            return CreateViewModelInstance<Property>(
                builder,
                new Attribute[] { new ViewModelAttribute(propertyViewModelType) },
                overrides.Union(defaults).ToArray());
        }

        /// <summary>
        /// Creates an <see cref="ElementProperty"/> based on a <see cref="PropertyDescriptor"/>.
        /// </summary>
        /// <param name="parent">The parent <see cref="ElementViewModel"/> for the property.</param>
        /// <param name="declaringProperty">The property descriptor.</param>
        /// <returns>
        /// Returns a <see cref="ElementReferenceProperty"/> or a custom one specified by <see cref="ViewModelAttribute"/> is returned if the <paramref name="declaringProperty"/> provides a <see cref="ReferenceAttribute"/>.<br/>
        /// Returns a <see cref="ElementProperty"/> or a custom one specified by <see cref="ViewModelAttribute"/> otherwise.
        /// </returns>
        public virtual ElementProperty CreateElementProperty(ElementViewModel parent, PropertyDescriptor declaringProperty)
        {
            IEnumerable<Attribute> metadataForCreatingProperty = GetMetadataAttributesFromProperty(declaringProperty);

            if (metadataForCreatingProperty.OfType<ReferenceAttribute>().Any())
            {
                return CreateReferenceElementProperty(parent, declaringProperty);
            }

            return CreateViewModelInstance<ElementProperty>(
                builder,
                metadataForCreatingProperty,
                new DependencyOverride(typeof(ElementViewModel), parent),
                new DependencyOverride(typeof(PropertyDescriptor), declaringProperty),
                new DependencyOverride(typeof(SectionViewModel), this));
        }

        /// <summary>
        /// Creates an <see cref="ElementReferenceProperty"/> from a <see cref="PropertyDescriptor"/>.
        /// </summary>
        /// <param name="parent">The parent element for the property.</param>
        /// <param name="declaringProperty">The property descriptor.</param>
        /// <returns>A new <see cref="ElementReferenceProperty"/> or a custom one specified by the <see cref="ViewModelAttribute"/>.</returns>
        protected virtual ElementProperty CreateReferenceElementProperty(ElementViewModel parent, PropertyDescriptor declaringProperty)
        {
            IEnumerable<Attribute> metadataForCreatingProperty = GetMetadataAttributesFromProperty(declaringProperty);

            return CreateViewModelInstance<ElementProperty>(
                builder,
                metadataForCreatingProperty.Union(new Attribute[] { new ViewModelAttribute(typeof(ElementReferenceProperty)) }),
                new DependencyOverride(typeof(ElementViewModel), parent),
                new DependencyOverride(typeof(PropertyDescriptor), declaringProperty),
                new DependencyOverride(typeof(SectionViewModel), this));
        }

        /// <summary>
        /// Creates a new <see cref="Validator"/> based on a <see cref="Type"/>.
        /// </summary>
        /// <param name="validatorType">The type of <see cref="Validator"/> to create.</param>
        /// <returns>A new validator.</returns>
        public virtual Validator CreateValidatorInstance(Type validatorType)
        {
            return (Validator)builder.Resolve(validatorType);
        }

        /// <summary>
        /// Creates a <see cref="SectionViewModel"/> for a <see cref="ConfigurationSection"/>.
        /// </summary>
        /// <param name="container">The container to use in building the section and its children.</param>
        /// <param name="sectionName">The name of the section.</param>
        /// <param name="section">The <see cref="ConfigurationSection"/> to create the <see cref="SectionViewModel"/> for.</param>
        /// <returns>The created <see cref="SectionViewModel"/>.</returns>
        public static SectionViewModel CreateSection(IUnityContainer container, string sectionName, ConfigurationSection section)
        {
            IEnumerable<Attribute> attributesOnSectionType = TypeDescriptor.GetAttributes(section).OfType<Attribute>();

            var sectionViewModel = CreateViewModelInstance<SectionViewModel>(
                container,
                attributesOnSectionType,
                new DependencyOverride(typeof(ConfigurationSection), section),
                new ParameterOverride("sectionName", sectionName));

            return sectionViewModel;
        }

        /// <summary>
        /// Determines attributes for a <see cref="PropertyDescriptor"/>.
        /// </summary>
        /// <param name="property">The property descriptor to provide attributes for.</param>
        /// <returns>
        /// Provides a set of attributes based on a base set of attributes determined from <see cref="TypeDescriptor.GetAttributes(System.Type)"/>
        /// with overriding attributes determined from <see cref="MemberDescriptor.Attributes"/>.
        /// </returns>
        /// <seealso cref="MetadataCollection"/>
        protected static IEnumerable<Attribute> GetMetadataAttributesFromProperty(PropertyDescriptor property)
        {
            IEnumerable<Attribute> attributesOnElementType = TypeDescriptor.GetAttributes(property.PropertyType).OfType<Attribute>().ToArray();
            IEnumerable<Attribute> attributesOnDeclaringProperty = property.Attributes.OfType<Attribute>().ToArray();

            return MetadataCollection.CombineAttributes(attributesOnElementType, attributesOnDeclaringProperty);

        }

        /// <summary>
        /// Creates a <see cref="ViewModel"/> instance from a container.
        /// </summary>
        /// <typeparam name="T">The type of view model to create.</typeparam>
        /// <param name="builder">The container used to create the model.</param>
        /// <param name="metadataForCreation">The metadata attributes used to create the view model.</param>
        /// <param name="overrides">Any overrides to supply to <paramref name="builder"/> during the creation.</param>
        /// <returns>
        /// Returns a <see cref="ViewModel"/> of type <typeparamref name="T"/> or
        /// if the <paramref name="metadataForCreation"/> provides a <see cref="ViewModelAttribute"/>, a <see cref="ViewModel"/>
        /// of type <see cref="ViewModelAttribute.ModelType"/>.
        /// </returns>
        protected static T CreateViewModelInstance<T>(IUnityContainer builder, IEnumerable<Attribute> metadataForCreation, params ResolverOverride[] overrides)
            where T : ViewModel
        {
            var viewModelAttribute = metadataForCreation.OfType<ViewModelAttribute>().Where(x => typeof(T).IsAssignableFrom(x.ModelType)).FirstOrDefault();

            Type typeToCreate = typeof(T);

            if (viewModelAttribute != null)
            {
                typeToCreate = viewModelAttribute.ModelType;
            }

            T modelElement = (T)builder.Resolve(typeToCreate, overrides);
            return modelElement;
        }

        private CommandModel CreateDefaultElementCollectionAddCommand(ElementCollectionViewModel collection)
        {
            if (collection.IsPolymorphicCollection || HasMultipleAddCommands(collection.CollectionElementType))
            {
                return builder.Resolve<DefaultElementCollectionAddCommand>(new DependencyOverride(typeof(ElementCollectionViewModel), collection));
            }

            return CreateDefaultCollectionElementAddCommand(collection.CollectionElementType, collection);
        }

        private CommandModel CreateDefaultCollectionElementAddCommand(Type elementType, ElementCollectionViewModel collection)
        {
            var replaceCommandAttribute = TypeDescriptor.GetAttributes(elementType)
                                    .OfType<CommandAttribute>()
                                    .Where(x => x.CommandPlacement == CommandPlacement.ContextAdd)
                                    .Where(x => x.Replace == CommandReplacement.DefaultAddCommandReplacement)
                                    .FirstOrDefault();

            if (replaceCommandAttribute != null)
            {
                return (CommandModel)builder.Resolve(replaceCommandAttribute.CommandModelType,
                                            new DependencyOverride(replaceCommandAttribute.GetType(), replaceCommandAttribute),
                                            new DependencyOverride<ConfigurationElementType>(new ConfigurationElementType(elementType)),
                                            new DependencyOverride<ElementCollectionViewModel>(collection));
            }

            return builder.Resolve<DefaultCollectionElementAddCommand>(
                                            new DependencyOverride<ConfigurationElementType>(new ConfigurationElementType(elementType)),
                                            new DependencyOverride<ElementCollectionViewModel>(collection));
        }

        private IEnumerable<CommandModel> CreateCustomAddCommands(Type elementType, ElementCollectionViewModel collection)
        {
            var customAddCommandAttributes = TypeDescriptor.GetAttributes(elementType).OfType<CommandAttribute>()
                         .Where(x => x.CommandPlacement == CommandPlacement.ContextAdd)
                         .Where(x => x.Replace != CommandReplacement.DefaultAddCommandReplacement);

            foreach (var customAddCommandAttribute in customAddCommandAttributes)
            {
                yield return (CommandModel)builder.Resolve(customAddCommandAttribute.CommandModelType,
                                            new DependencyOverride(customAddCommandAttribute.GetType(), customAddCommandAttribute),
                                            new DependencyOverride(typeof(ConfigurationElementType), new ConfigurationElementType(elementType)),
                                            new DependencyOverride(typeof(ElementCollectionViewModel), collection));
            }
        }

        private static bool HasMultipleAddCommands(Type elementType)
        {
            return TypeDescriptor.GetAttributes(elementType)
                .OfType<CommandAttribute>()
                .Where(x => x.CommandPlacement == CommandPlacement.ContextAdd)
                .Where(x => x.Replace != CommandReplacement.DefaultAddCommandReplacement)
                .Any();
        }

        #endregion

    }

    /// <summary>
    /// A <see cref="sectionViewModel"/> command to toggle the <see cref="SectionViewModel.IsExpanded"/> state.
    /// </summary>
    public class ToggleExpandedCommand : CommandModel
    {
        readonly SectionViewModel sectionViewModel;

        /// <summary>
        /// Initializes a new instance of <see cref="SectionViewModel"/>.
        /// </summary>
        /// <param name="sectionViewModel">The section view model to act upon.</param>
        /// <param name="uiService">The user-interface service.</param>
        public ToggleExpandedCommand(SectionViewModel sectionViewModel, IUIServiceWpf uiService)
            : base(uiService)
        {
            this.sectionViewModel = sectionViewModel;
        }

        /// <summary>
        /// Toggles the <see cref="SectionViewModel.IsExpanded"/> value.
        /// </summary>
        /// <param name="parameter">Not used.</param>
        protected override void InnerExecute(object parameter)
        {
            sectionViewModel.IsExpanded = !sectionViewModel.IsExpanded;
        }

        /// <summary>
        /// Provides the title of the <see cref="CommandModel"/> command.  Typically this will appear as the title to a menu in the configuration tool.
        /// </summary>
        public override string Title
        {
            get
            {
                return Resources.ToggleShowHideSectionCommand;
            }
        }

        /// <summary>
        /// Protected method for determinging if command can execute.
        /// </summary>
        /// <returns>
        /// true if this command can be executed; otherwise, false.
        /// </returns>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.
        ///                 </param>
        protected override bool InnerCanExecute(object parameter)
        {
            return (sectionViewModel.Bindable != null);
        }
    }
}
