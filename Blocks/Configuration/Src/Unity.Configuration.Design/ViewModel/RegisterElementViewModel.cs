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
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Practices.Unity.Configuration;
using Microsoft.Practices.Unity.Configuration.ConfigurationHelpers;
using Microsoft.Practices.Unity.Configuration.Design.ViewModel;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.Unity
{
    public class RegisterElementViewModel : CollectionElementViewModel
    {
        private readonly PropertyChangedEventHandler registrationTypeDependentPropertyChangedHandler;
        private readonly LifetimeElementProperty lifetimeElementProperty;

        private Property typeNameProperty;
        private Property mapToNameProperty;

        public RegisterElementViewModel(IServiceProvider serviceProvider, ElementCollectionViewModel containingCollection, ConfigurationElement thisElement)
            : base(containingCollection, thisElement)
        {
            registrationTypeDependentPropertyChangedHandler = new PropertyChangedEventHandler(RegistrationTypeDependentPropertyChanged);

            var lifetimeElementPropertyDescriptor = TypeDescriptor.GetProperties(thisElement).
                                                        OfType<PropertyDescriptor>().
                                                        Where(x => x.Name == "Lifetime").
                                                        First();


            lifetimeElementProperty = (LifetimeElementProperty)ContainingSection.CreateProperty(
                typeof (LifetimeElementProperty),
                new DependencyOverride<IServiceProvider>(serviceProvider),
                new DependencyOverride<ElementViewModel>(this),
                new DependencyOverride<PropertyDescriptor>(lifetimeElementPropertyDescriptor));
                
           
        }

        public override void Initialize(InitializeContext context)
        {
            base.Initialize(context);

            typeNameProperty = this.Property("TypeName");
            typeNameProperty.PropertyChanged += registrationTypeDependentPropertyChangedHandler;

            mapToNameProperty = this.Property("MapToName");
            mapToNameProperty.PropertyChanged += registrationTypeDependentPropertyChangedHandler;
        }

        protected override IEnumerable<Property> GetAllProperties()
        {
            return base.GetAllProperties().Union(new Property[] { lifetimeElementProperty });
        }

        private void RegistrationTypeDependentPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value") OnRegistrationTypeChanged();
        }

        protected override object CreateBindable()
        {
            return new HierarchicalLayout(this, this.ChildElement("InjectionMembers").ChildElements, 1);
        }

        public Type RegistrationType
        {
            get
            {
                UnityConfigurationSection unitySection = (UnityConfigurationSection)ContainingSection.ConfigurationElement;
                TypeResolver.SetAliases(unitySection);

                string mapToNameValue = Property("MapToName").Value as string;
                Type mapToType = String.IsNullOrEmpty(mapToNameValue) ? null : TypeResolver.ResolveType(mapToNameValue, false);
                if (mapToType != null) return mapToType;

                string typeNameValue = Property("TypeName").Value as string;
                return string.IsNullOrEmpty(typeNameValue) ? null : TypeResolver.ResolveType(typeNameValue, false);
            }
        }

        protected override IEnumerable<ElementViewModel> GetAllChildElements()
        {
            foreach(var childElement in base.GetAllChildElements())
            {
                if (childElement.ConfigurationType != typeof(LifetimeElement))
                {
                    yield return childElement;
                }
            }
        }

        internal virtual void OnRegistrationTypeChanged()
        {
            var handler = RegistrationTypeChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        public event EventHandler<EventArgs> RegistrationTypeChanged;

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (mapToNameProperty != null)
            {
                mapToNameProperty.PropertyChanged -= registrationTypeDependentPropertyChangedHandler;
            }

            if (typeNameProperty != null)
            {
                typeNameProperty.PropertyChanged -= registrationTypeDependentPropertyChangedHandler;
            }
        }
    }
}
