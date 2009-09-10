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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Console.Wpf.ViewModel;
using System.ComponentModel;

namespace Console.Wpf.Tests.VSTS.Mocks
{
    [ViewModel(typeof(SectionViewModelEx))]
    public class SectionWithExtendedViewModel : ConfigurationSection
    {

        public SectionWithExtendedViewModel()
        {
            ElementCollection = new ElementCollectionWithExtendedViewmodel();

            ElementCollection.Add(new CollectionElementWithExtendedViewmodel()); 
            ElementCollection.Add(new CollectionElementWithExtendedViewmodel());
        }

        [ConfigurationProperty("Element")]
        public ElementWithExtendedViewModel Element
        {
            get { return (ElementWithExtendedViewModel) base["Element"]; }
            set { base["Element"] = value; }
        }
        
        [ViewModel(typeof(ElementViewModelEx2))]
        [ConfigurationProperty("Element2")]
        public ElementWithExtendedViewModel ElementWithViewModelOnAttribute
        {
            get { return (ElementWithExtendedViewModel) base["Element2"]; }
            set { base["Element2"] = value; }
        }

        [ConfigurationProperty("ElementCollection")]
        public ElementCollectionWithExtendedViewmodel ElementCollection
        {
            get { return (ElementCollectionWithExtendedViewmodel)base["ElementCollection"]; }
            set { base["ElementCollection"] = value; }
        }
    }

    [ViewModel(typeof(ElementViewModelEx))]
    public class ElementWithExtendedViewModel : ConfigurationElement
    {

    }

    [ViewModel(typeof(ElementCollectionViewModelEx))]
    [ConfigurationCollection(typeof(CollectionElementWithExtendedViewmodel))]
    public class ElementCollectionWithExtendedViewmodel : ConfigurationElementCollection
    {
        public void Add(CollectionElementWithExtendedViewmodel element)
        {
            base.BaseAdd(element);
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new CollectionElementWithExtendedViewmodel();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return Guid.NewGuid();
        }
    }



    [ViewModel(typeof(CollectionElementViewModelEx))]
    public class CollectionElementWithExtendedViewmodel : ConfigurationElement
    {
    }


    public class SectionViewModelEx : SectionViewModel
    {
        public SectionViewModelEx(IServiceProvider serviceProvider, ConfigurationSection section)
            :base(serviceProvider, section)
        {
        }
    }


    public class ElementCollectionViewModelEx : ElementCollectionViewModel
    {
        public ElementCollectionViewModelEx(IServiceProvider serviceProvider, ElementViewModel parentElementModel, PropertyDescriptor declaringProperty)
            : base(serviceProvider, parentElementModel, declaringProperty)
        {
        }
    }

    public class CollectionElementViewModelEx : CollectionElementViewModel
    {
        public CollectionElementViewModelEx(IServiceProvider serviceProvider, ElementCollectionViewModel containingCollection, ConfigurationElement thisElement)
            : base(serviceProvider, containingCollection, thisElement)
        {
        }
    }


    public class ElementViewModelEx : ElementViewModel
    {
        public ElementViewModelEx(IServiceProvider serviceProvider, ElementViewModel parentElementModel, PropertyDescriptor declaringProperty)
            :base(serviceProvider, parentElementModel, declaringProperty)
        {
        }
    }

    public class ElementViewModelEx2 : ElementViewModel
    {
        public ElementViewModelEx2(IServiceProvider serviceProvider, ElementViewModel parentElementModel, PropertyDescriptor declaringProperty)
            : base(serviceProvider, parentElementModel, declaringProperty)
        {
        }
    }
}
