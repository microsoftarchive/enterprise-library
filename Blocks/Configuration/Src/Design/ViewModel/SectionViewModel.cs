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
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Converters;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Practices.Unity;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{
    public class SectionViewModel : ElementViewModel
    {
        RequirePermissionProperty requirePermissionProperty;
        ProtectionProviderProperty protectionProviderProperty;
        IUnityContainer builder;
        ElementLookup lookup;
        string configurationSectionName;

        [InjectionConstructor]
        public SectionViewModel(IUnityContainer builder, string sectionName, ConfigurationSection section)
            : this(builder, sectionName, section, TypeDescriptor.GetAttributes(section).OfType<Attribute>().Union(section.GetType().GetCustomAttributes(true).OfType<Attribute>())) //section has attributes from class-decl by default
        {
        }

        public SectionViewModel(IUnityContainer builder, string sectionName, ConfigurationSection section, IEnumerable<Attribute> metadataAttributes)
            : base(null, section, metadataAttributes) //section has attributes from class-decl
        {
            this.builder = builder;
            this.lookup = builder.Resolve<ElementLookup>();

            configurationSectionName = sectionName;

            requirePermissionProperty = CreateProperty<RequirePermissionProperty>();
            requirePermissionProperty.Value = section.SectionInformation.RequirePermission;

            protectionProviderProperty = CreateProperty<ProtectionProviderProperty>();
            if (section.SectionInformation.IsProtected)
            {
                protectionProviderProperty.Value = section.SectionInformation.ProtectionProvider.Name;
            }
        }

        protected override object CreateBindable()
        {
            return null;
        }

        public override void Delete()
        {
            var configurationSource = builder.Resolve<ConfigurationSourceModel>();
            configurationSource.RemoveSection(configurationSectionName);

            lookup.RemoveSection(this);
        }

        public string SectionName
        {
            get { return configurationSectionName; }
        }

        public virtual void BeforeSave(ConfigurationSection sectionToSave)
        {
            sectionToSave.SectionInformation.RequirePermission = requirePermissionProperty.TypedValue;
        }

        public virtual void Save(IDesignConfigurationSource configurationSource)
        {
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

        public ProtectionProviderProperty ProtectionProviderProperty
        {
            get { return protectionProviderProperty; }
        }

        public RequirePermissionProperty RequirePermissionProperty
        {
            get { return requirePermissionProperty; }
        }

        protected override IEnumerable<Property> GetAllProperties()
        {
            return base.GetAllProperties().Union(new Property[] { requirePermissionProperty, protectionProviderProperty });
        }

        protected override string GetLocalPathPart()
        {
            return string.Format("configuration/{0}", configurationSectionName);
        }

        public virtual IEnumerable<ViewModel> GetAdditionalGridVisuals()
        {
            return Enumerable.Empty<ViewModel>();
        }

        public virtual IEnumerable<ElementViewModel> GetRelatedElements(ElementViewModel element)
        {
            List<ElementViewModel> relatedElements = new List<ElementViewModel>();

            //elements we refer to
            relatedElements.AddRange(element.Properties.OfType<ElementReferenceProperty>()
                                                       .Where(x => x.ReferencedElement != null)
                                                       .Select(x => x.ReferencedElement));


            //elements that refer to us.
            relatedElements.AddRange(DescendentElements(x => x.Properties.OfType<ElementReferenceProperty>().Where(y => y.ReferencedElement == element).Any()));

            //our parent
            var firstVisibleParent = element.AncesterElements().Where(x => null == x as ElementCollectionViewModel).FirstOrDefault();
            if (firstVisibleParent != null)
            {
                relatedElements.Add(firstVisibleParent);
            }

            //our children
            relatedElements.AddRange(element.ChildElements);

            //for children that are collection, their children as well.
            relatedElements.AddRange(element.ChildElements.OfType<ElementCollectionViewModel>().SelectMany(x => x.ChildElements).OfType<CollectionElementViewModel>().Cast<ElementViewModel>());

            //that should be it. by default.
            return relatedElements;
        }

        #region  Create XXX methods


        public virtual IEnumerable<CommandModel> CreateCollectionElementAddCommand(Type elementType, ElementCollectionViewModel collection)
        {
            yield return CreateDefaultCollectionElementAddCommand(elementType, collection);

            foreach (var customCommand in CreateCustomAddCommands(elementType, collection))
            {
                yield return customCommand;
            }
        }

        public virtual CommandModel CreateDeleteCommand(ElementViewModel elementViewModel, IEnumerable<Attribute> attributes)
        {
            var deleteCommandReplacement = attributes.OfType<CommandAttribute>().Where(x => x.Replace == CommandReplacement.DefaultDeleteCommandReplacement).FirstOrDefault();
            if (deleteCommandReplacement != null)
            {
                return (CommandModel)builder.Resolve(deleteCommandReplacement.CommandModelType,
                                            new DependencyOverride(deleteCommandReplacement.GetType(), deleteCommandReplacement),
                                            new DependencyOverride<ElementViewModel>(elementViewModel));
            }
            return builder.Resolve<DefaultDeleteCommandModel>(new DependencyOverride<ElementViewModel>(elementViewModel));
        }

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




        public virtual IEnumerable<CommandModel> CreateCustomCommands(ElementViewModel target, IEnumerable<Attribute> attributes)
        {
            foreach (CommandAttribute command in attributes.OfType<CommandAttribute>().Where(x => x.CommandPlacement == CommandPlacement.ContextCustom))
            {
                yield return (CommandModel)builder.Resolve(command.CommandModelType,
                    new DependencyOverride(typeof(ElementViewModel), target),
                    new DependencyOverride(command.GetType(), command));
            }
        }

        public TCommandModel CreateCommand<TCommandModel>(ElementViewModel target)
        {
            return builder.Resolve<TCommandModel>(new DependencyOverride(typeof(ElementViewModel), target));
        }

        public virtual ElementViewModel CreateChild(ElementViewModel containingViewModel, PropertyDescriptor declaringProperty)
        {
            if (typeof(ConfigurationElementCollection).IsAssignableFrom(declaringProperty.PropertyType))
            {
                return CreateElementCollection(containingViewModel, declaringProperty);
            }
            return CreateElement(containingViewModel, declaringProperty);
        }

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

        public virtual ElementCollectionViewModel CreateElementCollection(ElementViewModel parent, PropertyDescriptor declaringProperty)
        {
            IEnumerable<Attribute> metadataForCreatingViewModel = GetMetaDataAttributesFromProperty(declaringProperty);

            return CreateViewModelInstance<ElementCollectionViewModel>(
                builder,
                metadataForCreatingViewModel,
                new DependencyOverride(typeof(ElementViewModel), parent),
                new DependencyOverride(typeof(PropertyDescriptor), declaringProperty),
                new DependencyOverride(typeof(SectionViewModel), this));
        }

        public virtual ElementViewModel CreateElement(ElementViewModel parent, PropertyDescriptor declaringProperty)
        {
            IEnumerable<Attribute> metadataForCreatingViewModel = GetMetaDataAttributesFromProperty(declaringProperty);

            return CreateViewModelInstance<ElementViewModel>(
                builder,
                metadataForCreatingViewModel,
                new DependencyOverride(typeof(ElementViewModel), parent),
                new DependencyOverride(typeof(PropertyDescriptor), declaringProperty),
                new DependencyOverride(typeof(SectionViewModel), this));
        }

        public virtual Property CreateProperty(object component, PropertyDescriptor declaringProperty)
        {
            IEnumerable<Attribute> metadataForCreatingProperty = GetMetaDataAttributesFromProperty(declaringProperty);

            return CreateViewModelInstance<Property>(
                builder,
                metadataForCreatingProperty,
                new DependencyOverride(typeof(object), component),
                new DependencyOverride(typeof(PropertyDescriptor), declaringProperty),
                new DependencyOverride(typeof(SectionViewModel), this));

        }
        //TODO: possible take Dictionary<String, Object> instead.
        public virtual TPropertyViewModel CreateProperty<TPropertyViewModel>(params ResolverOverride[] overrides)
            where TPropertyViewModel : Property
        {
            return (TPropertyViewModel)CreateProperty(typeof(TPropertyViewModel), overrides);
        }

        public virtual Property CreateProperty(Type propertyViewModelType, params ResolverOverride[] overrides)
        {
            ResolverOverride[] defaults = new[] { new DependencyOverride(typeof(SectionViewModel), this) };

            return CreateViewModelInstance<Property>(
                builder,
                new Attribute[] { new ViewModelAttribute(propertyViewModelType) },
                overrides.Union(defaults).ToArray());
        }

        public virtual ElementProperty CreateElementProperty(ElementViewModel parent, PropertyDescriptor declaringProperty)
        {
            IEnumerable<Attribute> metadataForCreatingProperty = GetMetaDataAttributesFromProperty(declaringProperty);

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

        protected virtual ElementProperty CreateReferenceElementProperty(ElementViewModel parent, PropertyDescriptor declaringProperty)
        {
            IEnumerable<Attribute> metadataForCreatingProperty = GetMetaDataAttributesFromProperty(declaringProperty);

            return CreateViewModelInstance<ElementProperty>(
                builder,
                metadataForCreatingProperty.Union(new Attribute[] { new ViewModelAttribute(typeof(ElementReferenceProperty)) }),
                new DependencyOverride(typeof(ElementViewModel), parent),
                new DependencyOverride(typeof(PropertyDescriptor), declaringProperty),
                new DependencyOverride(typeof(SectionViewModel), this));
        }

        public virtual Validator CreateValidatorInstance(Type validatorType)
        {
            return (Validator)builder.Resolve(validatorType);
        }

        public static SectionViewModel CreateSection(IUnityContainer container, string sectionName, ConfigurationSection section)
        {
            IEnumerable<Attribute> attributesOnSectionType = TypeDescriptor.GetAttributes(section).OfType<Attribute>();

            var sectionViewModel = CreateViewModelInstance<SectionViewModel>(
                container,
                attributesOnSectionType,
                new DependencyOverride(typeof(ConfigurationSection), section),
                new ParameterOverride("sectionName", sectionName));

            var lookup = container.Resolve<ElementLookup>();
            lookup.AddSection(sectionViewModel);

            return sectionViewModel;
        }

        protected static IEnumerable<Attribute> GetMetaDataAttributesFromProperty(PropertyDescriptor property)
        {
            IEnumerable<Attribute> attributesOnElementType = TypeDescriptor.GetAttributes(property.PropertyType).OfType<Attribute>().ToArray();
            IEnumerable<Attribute> attributesOnDeclaringProperty = property.Attributes.OfType<Attribute>().ToArray();

            //todo: check does Union Order elements
            return MetadataCollection.CombineAttributes(attributesOnElementType, attributesOnDeclaringProperty);

        }

        protected static T CreateViewModelInstance<T>(IUnityContainer builder, IEnumerable<Attribute> metadataForCreation, params ResolverOverride[] overrides)
            where T : ViewModel
        {
            var viewModelAttribute = metadataForCreation.OfType<ViewModelAttribute>().FirstOrDefault();

            Type typeToCreate = typeof(T);
            Type visualType = null;

            if (viewModelAttribute != null)
            {
                typeToCreate = viewModelAttribute.ModelType;
                if (!typeof(T).IsAssignableFrom(typeToCreate)) throw new InvalidOperationException("TODO");
                visualType = viewModelAttribute.ModelVisualType;
            }

            T modelElement = (T)builder.Resolve(typeToCreate, overrides);
            modelElement.CustomVisualType = visualType;
            return modelElement;
        }

        private CommandModel CreateDefaultElementCollectionAddCommand(ElementCollectionViewModel collection)
        {
            if (collection.IsPolymorphicCollection || HasMultipleAddCommands(collection.CollectionElementType))
            {
                return new DefaultElementCollectionAddCommand(collection);
            }

            return CreateDefaultCollectionElementAddCommand(collection.CollectionElementType, collection);
        }

        private CommandModel CreateDefaultCollectionElementAddCommand(Type elementType, ElementCollectionViewModel collection)
        {
            var replaceCommandAttribute = Attribute.GetCustomAttributes(elementType)
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
            var customAddCommandAttributes = Attribute.GetCustomAttributes(elementType).OfType<CommandAttribute>()
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
            return Attribute.GetCustomAttributes(elementType)
                .OfType<CommandAttribute>()
                .Where(x => x.CommandPlacement == CommandPlacement.ContextAdd)
                .Where(x => x.Replace != CommandReplacement.DefaultAddCommandReplacement)
                .Any();
        }

        #endregion


    }

    public class RequirePermissionProperty : CustomProperty<bool>
    {
        public RequirePermissionProperty(IServiceProvider serviceProvider)
            : base(serviceProvider, new BooleanConverter(), "Require Permission")
        {

        }
    }

    public class ProtectionProviderProperty : CustomProperty<string>
    {
        public ProtectionProviderProperty(IServiceProvider serviceProvider)
            : base(serviceProvider, new ProtectionProviderTypeConverter(), "Protection Provider")
        {
            TypedValue = ProtectionProviderTypeConverter.NoProtectionValue;
        }

        public bool NeedsProtectionProvider
        {
            get { return ((string)Value != ProtectionProviderTypeConverter.NoProtectionValue); }
        }
    }
}
