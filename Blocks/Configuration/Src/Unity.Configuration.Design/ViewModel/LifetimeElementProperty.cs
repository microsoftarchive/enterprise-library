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
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.Unity.Configuration.Design.ComponentModel;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;

namespace Microsoft.Practices.Unity.Configuration.Design.ViewModel
{
    public class LifetimeElementProperty : CustomProperty<LifetimeElement>
    {
        private readonly static KnownLifetimeElementConverter KnownLifetimeElementConverter = new KnownLifetimeElementConverter();
        
        readonly ElementViewModel parentViewModel;
        readonly PropertyDescriptor lifetimeElementPropertyDescriptor;
        ElementViewModel lifetimeElementViewModel;

        public LifetimeElementProperty(IServiceProvider serviceProvider, ElementViewModel parentViewModel, PropertyDescriptor lifetimeElementPropertyDescriptor)
            : base(serviceProvider, parentViewModel.ConfigurationElement, lifetimeElementPropertyDescriptor, KnownLifetimeElementConverter, lifetimeElementPropertyDescriptor.Name, 
                    new ResourceCategoryAttribute(typeof(DesignResources), "CategoryLifetime"))
        {
            this.parentViewModel = parentViewModel;
            this.lifetimeElementPropertyDescriptor = lifetimeElementPropertyDescriptor;
        }

        public override void Initialize(InitializeContext context)
        {
            base.Initialize(context);

            var lifetimeElement = (LifetimeElement)lifetimeElementPropertyDescriptor.GetValue(parentViewModel.ConfigurationElement);
            if (lifetimeElement != null && !lifetimeElement.ElementInformation.IsPresent)
            {
                lifetimeElementPropertyDescriptor.SetValue(parentViewModel.ConfigurationElement, null);
            }

            RecreateLifetimeElementViewModel();
        }

        //public for unittesting purposes.
        public void RecreateLifetimeElementViewModel()
        {
            lifetimeElementViewModel = parentViewModel.ContainingSection.
                                            CreateElement(parentViewModel, lifetimeElementPropertyDescriptor);
            
        }


        public override IEnumerable<Validator> GetValidators()
        {
            return Enumerable.Empty<Validator>();
        }

        protected override object GetValue()
        {
            if (lifetimeElementViewModel.IsNull)
            {
                return KnownLifetimeElementConverter.NullLifetimeManagerDisplay;
            }

            return lifetimeElementViewModel.Property("TypeName").Value;
        }

        protected override void SetValue(object value)
        {
            var stringValue = ((string)value);

            if (stringValue == KnownLifetimeElementConverter.NullLifetimeManagerDisplay)
            {
                lifetimeElementPropertyDescriptor.SetValue(parentViewModel.ConfigurationElement, null);
            } 
            else
            {
                if (lifetimeElementViewModel.IsNull)
                {
                    lifetimeElementPropertyDescriptor.SetValue(parentViewModel.ConfigurationElement,
                                                               new LifetimeElement
                                                                   {
                                                                       TypeName = stringValue == DesignResources.RegistrationLifetimeCustom ? "" : stringValue
                                                                   });
                } else
                {
                    if (stringValue == DesignResources.RegistrationLifetimeCustom)
                    {
                        lifetimeElementViewModel.Property("TypeName").Value = "";
                    }
                    else
                    {
                        lifetimeElementViewModel.Property("TypeName").Value = stringValue;
                        lifetimeElementViewModel.Property("TypeConverterTypeName").Value = string.Empty;
                        lifetimeElementViewModel.Property("Value").Value = string.Empty;
                    }
                     
                }
            }


            RecreateLifetimeElementViewModel();
            OnPropertyChanged("ChildProperties");
            OnPropertyChanged("HasChildProperties");
        }


        public override IEnumerable<Property> ChildProperties
        {
            get
            {
                return !HasChildProperties
                           ? Enumerable.Empty<Property>()
                           : lifetimeElementViewModel.Properties;
            }
        }

        public override bool HasChildProperties
        {
            get
            {
                return ! (lifetimeElementViewModel.IsNull || !KnownLifetimeElementConverter.IsCustomLifetimeManager((string)lifetimeElementViewModel.Property("TypeName").Value));
            }
        }

    }
}
