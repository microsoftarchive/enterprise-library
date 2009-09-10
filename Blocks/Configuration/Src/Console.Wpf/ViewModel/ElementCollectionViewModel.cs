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
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Configuration;
using System.ComponentModel;
using System.Windows.Input;
using Console.Wpf.Properties;
using Console.Wpf.ViewModel.Services;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using System.Reflection;

namespace Console.Wpf.ViewModel
{
    public class ElementCollectionViewModel : ElementViewModel
    {
        IServiceProvider serviceProvider;
        ConfigurationElementCollection thisElementCollection;

        public ElementCollectionViewModel(IServiceProvider serviceProvider, ElementViewModel parentElementModel, PropertyDescriptor declaringProperty)
            : base(serviceProvider, parentElementModel, declaringProperty)
        {
            this.serviceProvider = serviceProvider;
            this.thisElementCollection = declaringProperty.GetValue(parentElementModel.ConfigurationElement) as ConfigurationElementCollection;
            
        }

        public Type CollectionElementType
        {
            get
            {
                var collectionAttribute = base.MetadataAttributes.OfType<ConfigurationCollectionAttribute>().FirstOrDefault();
                if (collectionAttribute != null)
                {
                    return collectionAttribute.ItemType;
                }
                
                throw new InvalidOperationException("Collection is not attributed with ConfigurationCollectionAttribute");
            }
        }

        public bool IsFirst(CollectionElementViewModel element)
        {
            return thisElementCollection.OfType<ConfigurationElement>().First() == element.ConfigurationElement;
        }

        public bool IsLast(CollectionElementViewModel element)
        {
            return thisElementCollection.OfType<ConfigurationElement>().Last() == element.ConfigurationElement;
        }

        public override IEnumerable<ICommand> ChildAdders
        {
            get
            {
                if (IsPolymorphicCollection())
                {
                    DiscoverDerivedConfigurationTypesService reflectionFinderService = (DiscoverDerivedConfigurationTypesService)
                        serviceProvider.GetService(typeof(DiscoverDerivedConfigurationTypesService));

                    var types = reflectionFinderService.FindAvailableConfigurationElementTypes(CollectionElementType);

                    var customTypes = GetCustomType();
                    return types.Concat(customTypes)
                            .Select(x => new ElementCollectionViewModelAdder(x, this))
                            .OrderBy(a => a.DisplayName).Cast<ICommand>();
                }

                return new[] { new ElementCollectionViewModelAdder(CollectionElementType, this) };
            }
        }

        private Type[] GetCustomType()
        {
            Type genericType =
                ConfigurationType.FindGenericParent(typeof (NameTypeConfigurationElementCollection<,>));

            if (genericType != null)
            {
                return new[] { ConfigurationType.GetGenericArguments()[1] };
            }

            return new Type[0];
        }

        private bool IsPolymorphicCollection()
        {
            return ConfigurationType.FindGenericParent(typeof(PolymorphicConfigurationElementCollection<>)) != null;
        }

        public override IEnumerable<ElementViewModel> GetAllChildElements()
        {
            var leaf = base.GetAllChildElements();

            var contained = thisElementCollection
                                .OfType<ConfigurationElement>()
                                .Select(x => ContainingSection.CreateCollectionElement(this, x))
                                .Cast<ElementViewModel>();

            return leaf.Union(contained);
        }

        public virtual void CreateNewChildElement(Type elementType)
        {
            var element = (NamedConfigurationElement)Activator.CreateInstance(elementType);
            element.Name = CalculateNameFromType(elementType);

            var mergeableService = serviceProvider.GetService<MergeableConfigurationCollectionService>();
            var mergeableCollection = mergeableService.GetMergeableCollection(thisElementCollection);

            mergeableCollection.ResetCollection(
                thisElementCollection.OfType<ConfigurationElement>()
                .Concat(new [] { element })
                .ToArray());

            RefreshChildElements();

            NotifyCollectionChanged(
                new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Add,
                    ChildElements.Where(x=>x.ConfigurationElement == element)));
        }

        private void NotifyCollectionChanged(NotifyCollectionChangedEventArgs eventArguments)
        {
            var changeSource = serviceProvider.GetService<CollectionChangedSource>();
            changeSource.InvokeCollectionChanged(eventArguments);
        }

        private string CalculateNameFromType(Type elementType)
        {
            var displayNameAttribute =
                TypeDescriptor.GetAttributes(elementType).OfType<DisplayNameAttribute>().FirstOrDefault();

            string baseName = displayNameAttribute == null
                                  ? TypeDescriptor.GetClassName(elementType)
                                  : displayNameAttribute.DisplayName;
            
            return FindUniqueNewName(baseName);
        }

        private string FindUniqueNewName(string baseName)
        {
            int number = 1;
            while(true)
            {
                string proposedName = string.Format(CultureInfo.CurrentUICulture,
                                                    Resources.NewCollectionElementNameFormat, baseName, number);

                if (this.thisElementCollection.OfType<NamedConfigurationElement>().Any(e => e.Name == proposedName))
                    number++;
                else
                    return proposedName;
            }
        }

        public void Delete(CollectionElementViewModel element)
        {
            var mergeableService = serviceProvider.GetService<MergeableConfigurationCollectionService>();
            var mergeableCollection = mergeableService.GetMergeableCollection(thisElementCollection);

            var list =
                thisElementCollection.OfType<ConfigurationElement>().Where(x => x != element.ConfigurationElement).
                    ToArray();
            mergeableCollection.ResetCollection(list);

            RefreshChildElements();
            NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));

            element.OnDeleted();
        }

        public void MoveUp(CollectionElementViewModel elementViewModel)
        {
           MoveElement(elementViewModel, -1);
        }

        public void MoveDown(CollectionElementViewModel elementViewModel)
        {
            MoveElement(elementViewModel, 1);         
        }

        private void MoveElement(CollectionElementViewModel element, int moveDistance)
        {
            var mergeableService = serviceProvider.GetService<MergeableConfigurationCollectionService>();
            var mergeableCollection = mergeableService.GetMergeableCollection(thisElementCollection);

            var list = thisElementCollection.OfType<ConfigurationElement>().ToArray();
            
            MoveConfigurationItem(list, element.ConfigurationElement, moveDistance);
            mergeableCollection.ResetCollection(list);

            RefreshChildElements();
            NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        private void MoveConfigurationItem(ConfigurationElement[] elements, ConfigurationElement element, int relativeMoveIndex)
        {
            for(int i = 0; i < elements.Count(); i++)
            {
                if (elements[i] != element) continue;
                var newIndex = i + relativeMoveIndex;
                if (newIndex >= 0 && newIndex < elements.Count())
                {
                    var tmp = elements[newIndex];
                    elements[newIndex] = element;
                    elements[i] = tmp;
                    return;
                }
            }
        }
     
    }
}
