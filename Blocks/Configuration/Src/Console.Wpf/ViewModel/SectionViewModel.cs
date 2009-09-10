﻿//===============================================================================
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
using Console.Wpf.ViewModel.Services;
using System.ComponentModel.Design;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;

namespace Console.Wpf.ViewModel
{
    public class SectionViewModel : ElementViewModel
    {
        IServiceProvider serviceProvider;

        static Type[] SectionViewModelConstructorArgTypes = new[] { typeof(IServiceProvider), typeof(ConfigurationSection) };
        static ConstructorInfo SectionViewModelContructor = typeof(SectionViewModel).GetConstructor(SectionViewModelConstructorArgTypes);

        static Type[] ElementViewModelConstructorArgTypes = new[] { typeof(IServiceProvider), typeof(ElementViewModel), typeof(PropertyDescriptor) };
        static ConstructorInfo ElementViewModelContructor = typeof(ElementViewModel).GetConstructor(ElementViewModelConstructorArgTypes);

        static Type[] ElementCollectionViewModelConstructorArgTypes = new[] { typeof(IServiceProvider), typeof(ElementViewModel), typeof(PropertyDescriptor) };
        static ConstructorInfo ElementCollectionViewModelContructor = typeof(ElementCollectionViewModel).GetConstructor(ElementCollectionViewModelConstructorArgTypes);

        static Type[] CollectionElementViewModelConstructorArgTypes = new[] { typeof(IServiceProvider), typeof(ElementCollectionViewModel), typeof(ConfigurationElement) };
        static ConstructorInfo CollectionElementViewModelContructor = typeof(CollectionElementViewModel).GetConstructor(CollectionElementViewModelConstructorArgTypes);

        static Type[] ElementPropertyConstructorArgTypes = new[] { typeof(IServiceProvider), typeof(CollectionElementViewModel), typeof(PropertyDescriptor) };
        static ConstructorInfo ElementPropertyConstructor = typeof(ElementProperty).GetConstructor(ElementPropertyConstructorArgTypes);
        static ConstructorInfo ReferenceElementPropertyConstructor = typeof(ElementReferenceProperty).GetConstructor(ElementPropertyConstructorArgTypes);


        static Type[] PropertyConstructorArgTypes = new[] { typeof(IServiceProvider), typeof(object), typeof(PropertyDescriptor) };
        static ConstructorInfo PropertyConstructor = typeof(Property).GetConstructor(PropertyConstructorArgTypes);

        public SectionViewModel(IServiceProvider serviceProvider, ConfigurationSection section)
            :this(serviceProvider, section, TypeDescriptor.GetAttributes(section).OfType<Attribute>()) //section has attributes from class-decl by default
        {
        }

        public SectionViewModel(IServiceProvider serviceProvider, ConfigurationSection section, IEnumerable<Attribute> metadataAttributes)
            : base(serviceProvider, null, section, metadataAttributes) //section has attributes from class-decl
        {
            this.serviceProvider = serviceProvider;

            var changeSource = serviceProvider.EnsuredGetService<CollectionChangedSource>();
            changeSource.CollectionChanged += (s, e) => OnChildrenChangedEvent(EventArgs.Empty);
        }

        public virtual void UpdateLayout()
        {
            
        }

        public int Rows
        {
            get
            {
                return GetAllGridVisuals().Max(x => x.Row) + 1;
            }
        }
        public virtual int Columns
        {
            get
            {
                return GetAllGridVisuals().Max(x => x.Column) + 1;
            }
        }

        public virtual IEnumerable<ViewModel> GetAllGridVisuals()
        {
            var children = DescendentElements(x => x.IsShown);
            var childrenAndHeders = children.OfType<ViewModel>().Union(GetAdditionalGridVisuals());

            return childrenAndHeders;
        }

        public virtual IEnumerable<ViewModel> GetAdditionalGridVisuals()
        {
            return Enumerable.Empty<ViewModel>();
        }

        public virtual IEnumerable<ElementViewModel> GetRelatedElements(ElementViewModel element)
        {
            return Enumerable.Empty<ElementViewModel>();
        }

        public event EventHandler ChildrenChangedEvent;

        protected virtual void OnChildrenChangedEvent(EventArgs e)
        {
        	UpdateLayout();
            EventHandler changeEvent = ChildrenChangedEvent;
            if (changeEvent != null) changeEvent(this, e);
        }

        #region  Create XXX methods

        public virtual CollectionElementViewModel CreateCollectionElement(ElementCollectionViewModel parent, ConfigurationElement containedElement)
        {
            IEnumerable<Attribute> metadataForCreatingViewModel = TypeDescriptor.GetAttributes(containedElement).OfType<Attribute>();

            return CreateViewModelInstance<CollectionElementViewModel>(
                metadataForCreatingViewModel,
                CollectionElementViewModelContructor,
                CollectionElementViewModelConstructorArgTypes,
                new object[] { serviceProvider, parent, containedElement });
        }

        public virtual ElementCollectionViewModel CreateCollectionElement(ElementViewModel parent, PropertyDescriptor declaringProperty)
        {
            IEnumerable<Attribute> metadataForCreatingViewModel = GetMetaDataAttributesFromProperty(declaringProperty);

            return CreateViewModelInstance<ElementCollectionViewModel>(
                metadataForCreatingViewModel,
                ElementCollectionViewModelContructor,
                ElementCollectionViewModelConstructorArgTypes,
                new object[] { serviceProvider, parent, declaringProperty });
        }

        public virtual ElementViewModel CreateElement(ElementViewModel parent, PropertyDescriptor declaringProperty)
        {
            IEnumerable<Attribute> metadataForCreatingViewModel = GetMetaDataAttributesFromProperty(declaringProperty);

            return CreateViewModelInstance<ElementViewModel>(
                metadataForCreatingViewModel,
                ElementViewModelContructor,
                ElementViewModelConstructorArgTypes,
                new object[] { serviceProvider, parent, declaringProperty });
        }

        public virtual Property CreateProperty(object component, PropertyDescriptor declaringProperty)
        {
            IEnumerable<Attribute> metadataForCreatingProperty = GetMetaDataAttributesFromProperty(declaringProperty);

            return CreateViewModelInstance<Property>(
                metadataForCreatingProperty,
                PropertyConstructor,
                PropertyConstructorArgTypes,
                new object[] { serviceProvider, component, declaringProperty });

        }

        public virtual ElementProperty CreateElementProperty(ElementViewModel parent, PropertyDescriptor declaringProperty)
        {
            IEnumerable<Attribute> metadataForCreatingProperty = GetMetaDataAttributesFromProperty(declaringProperty);

            if (metadataForCreatingProperty.OfType<ReferenceAttribute>().Any())
            {
                return CreateReferenceElementProperty(parent, declaringProperty);
            }
        
            return CreateViewModelInstance<ElementProperty>(
                metadataForCreatingProperty,
                ElementPropertyConstructor,
                ElementPropertyConstructorArgTypes,
                new object[] { serviceProvider, parent, declaringProperty });
        }

        protected virtual ElementProperty CreateReferenceElementProperty(ElementViewModel parent, PropertyDescriptor declaringProperty)
        {
            IEnumerable<Attribute> metadataForCreatingProperty = GetMetaDataAttributesFromProperty(declaringProperty);

            return CreateViewModelInstance<ElementProperty>(
                metadataForCreatingProperty,
                ReferenceElementPropertyConstructor,
                ElementPropertyConstructorArgTypes,
                new object[] { serviceProvider, parent, declaringProperty });
        }


        public static SectionViewModel CreateSection(IServiceProvider serviceProvider, ConfigurationSection section)
        {
            IEnumerable<Attribute> attributesOnSectionType = TypeDescriptor.GetAttributes(section).OfType<Attribute>();

            var sectionViewModel = CreateViewModelInstance<SectionViewModel>(
                attributesOnSectionType,
                SectionViewModelContructor,
                SectionViewModelConstructorArgTypes,
                new object[] { serviceProvider, section });
                
            var lookup = serviceProvider.EnsuredGetService<ElementLookup>();
            lookup.AddSection(sectionViewModel);

            return sectionViewModel;
        }

        protected static IEnumerable<Attribute> GetMetaDataAttributesFromProperty(PropertyDescriptor property)
        {
            IEnumerable<Attribute> attributesOnElementType = TypeDescriptor.GetAttributes(property.PropertyType).OfType<Attribute>().ToArray();
            IEnumerable<Attribute> attributesOnDeclaringProperty = property.Attributes.OfType<Attribute>().ToArray();

            //todo: check does Union Order elements
            return attributesOnDeclaringProperty.Union(attributesOnElementType).ToArray();

        }

        protected static T CreateViewModelInstance<T>(IEnumerable<Attribute> metadataAttributes, ConstructorInfo constructorForDefaultType, Type[] constructorArgTypes, object[] consturctorArgValues)
        {
            ConstructorInfo contructorToCall = constructorForDefaultType;
            ViewModelAttribute viewModelAttribute = metadataAttributes.OfType<ViewModelAttribute>().FirstOrDefault();

            if (viewModelAttribute != null)
            {
                Type typeToCreate = viewModelAttribute.ModelType;
                if (!typeof(T).IsAssignableFrom(typeToCreate)) throw new InvalidOperationException("TODO");

                contructorToCall = typeToCreate.GetConstructor(constructorArgTypes);
                if (contructorToCall == null) throw new InvalidOperationException("TODO");
            }

            return (T)contructorToCall.Invoke(consturctorArgValues);

        }

        #endregion


    }
}