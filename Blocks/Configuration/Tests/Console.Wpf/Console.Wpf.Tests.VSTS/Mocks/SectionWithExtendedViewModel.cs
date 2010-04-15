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
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Windows;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using System.ComponentModel;
using System.Windows.Controls;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.Unity;

namespace Console.Wpf.Tests.VSTS.Mocks
{
    [ViewModel(typeof(SectionViewModelEx))]
    public class SectionWithExtendedViewModel : ConfigurationSection
    {

        public SectionWithExtendedViewModel()
        {
            ElementCollection = new ElementCollectionWithExtendedViewModel();

            ElementCollection.Add(new CollectionElementWithExtendedViewModel());
            ElementCollection.Add(new CollectionElementWithExtendedViewModel());
        }

        [ConfigurationProperty("Element")]
        public ElementWithExtendedViewModel Element
        {
            get { return (ElementWithExtendedViewModel)base["Element"]; }
            set { base["Element"] = value; }
        }

        [ViewModel(typeof(ElementViewModelEx2))]
        [ConfigurationProperty("Element2")]
        public ElementWithExtendedViewModel ElementWithViewModelOnAttribute
        {
            get { return (ElementWithExtendedViewModel)base["Element2"]; }
            set { base["Element2"] = value; }
        }

        [ConfigurationProperty("ElementCollection")]
        public ElementCollectionWithExtendedViewModel ElementCollection
        {
            get { return (ElementCollectionWithExtendedViewModel)base["ElementCollection"]; }
            set { base["ElementCollection"] = value; }
        }
    }

    [ViewModel(typeof(ElementViewModelEx))]
    public class ElementWithExtendedViewModel : ConfigurationElement, ICustomProviderData
    {
        public string Name
        {
            get { return "TestName";  }
        }

        public NameValueCollection Attributes
        {
            get { return new NameValueCollection(); }
        }
    }




    [ViewModel(typeof(ElementCollectionViewModelEx))]
    [ConfigurationCollection(typeof(CollectionElementWithExtendedViewModel))]
    public class ElementCollectionWithExtendedViewModel : ConfigurationElementCollection
    {
        public void Add(CollectionElementWithExtendedViewModel element)
        {
            base.BaseAdd(element);
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new CollectionElementWithExtendedViewModel();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return Guid.NewGuid();
        }
    }

    [ViewModel(typeof(CollectionElementViewModelEx))]
    public class CollectionElementWithExtendedViewModel : ConfigurationElement
    {
       
    }


    public class SectionViewModelEx : SectionViewModel
    {
        public SectionViewModelEx(IUnityContainer builder, ConfigurationSection section)
            : base(builder, "sectionName", section)
        {
        }
    }


    public class ElementCollectionViewModelEx : ElementCollectionViewModel
    {
        public ElementCollectionViewModelEx(ElementViewModel parentElementModel, PropertyDescriptor declaringProperty)
            : base(parentElementModel, declaringProperty)
        {
        }
    }

    public class CollectionElementViewModelEx : CollectionElementViewModel
    {
        public CollectionElementViewModelEx(ElementCollectionViewModel containingCollection, ConfigurationElement thisElement)
            : base(containingCollection, thisElement)
        {
        }
    }


    public class ElementViewModelEx : ElementViewModel
    {
        public ElementViewModelEx(ElementViewModel parentElementModel, PropertyDescriptor declaringProperty)
            : base(parentElementModel, declaringProperty)
        {
        }

        protected override IEnumerable<Property> GetAllProperties()
        {
            return base.GetAllProperties().Union(new Property[] { this.ContainingSection.CreateProperty<CustomProperty>() });
        }
    }

    public class ElementViewModelEx2 : ElementViewModel
    {
        public ElementViewModelEx2(ElementViewModel parentElementModel, PropertyDescriptor declaringProperty)
            : base(parentElementModel, declaringProperty)
        {
        }
    }


    public class CustomProperty : CustomProperty<string>
    {
        public bool WasInitialized = false;

        public CustomProperty(IServiceProvider serviceProvider)
            : base(serviceProvider, "CustomProprty")
        {

        }

        public override void Initialize(InitializeContext context)
        {
            WasInitialized = true;
        }
    }
}
