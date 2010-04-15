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
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.EnvironmentalOverrides;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{
    ///<summary>
    /// A property model from a property discovered on a <see cref="ConfigurationElement"/>.
    ///</summary>
    public class ElementProperty : Property, ILogicalPropertyContainerElement, IEnvironmentalOverridesProperty
    {
        private readonly ConfigurationPropertyAttribute configurationPropertyAttribute;
        private readonly PropertyInformation configurationProperty;
        private readonly ElementViewModel declaringElement;

        ///<summary>
        /// Initializes an instance of ElementProperty.
        ///</summary>
        ///<param name="serviceProvider">Service provider used to locate services for the configuration system.</param>
        ///<param name="parent">The parent <see cref="ElementViewModel"/> owning the property.</param>
        ///<param name="declaringProperty">The description of the property.</param>
        [InjectionConstructor]
        public ElementProperty(IServiceProvider serviceProvider, ElementViewModel parent, PropertyDescriptor declaringProperty)
            : this(serviceProvider, parent, declaringProperty, new Attribute[0])
        {
        }

        ///<summary>
        /// Initializes an instance of ElementProperty.
        ///</summary>
        ///<param name="serviceProvider">Service provider used to locate certain services for the configuration system.</param>
        ///<param name="parent">The parent <see cref="ElementViewModel"/> owning the property.</param>
        ///<param name="declaringProperty">The description of the property.</param>
        ///<param name="additionalAttributes">Additional attributes made available to the ElementProperty.</param>
        ///<exception cref="ArgumentNullException"></exception>
        public ElementProperty(IServiceProvider serviceProvider, ElementViewModel parent, PropertyDescriptor declaringProperty, IEnumerable<Attribute> additionalAttributes)
            : base(serviceProvider, parent == null ? null : parent.ConfigurationElement, declaringProperty, additionalAttributes)
        {
            if (parent == null) throw new ArgumentNullException("parent");
            declaringElement = parent;

            ConfigurationElement parentElement = parent.ConfigurationElement;

            configurationPropertyAttribute = declaringProperty.Attributes.OfType<ConfigurationPropertyAttribute>().FirstOrDefault();
            if (configurationPropertyAttribute == null)
            {
                configurationPropertyAttribute = additionalAttributes.OfType<ConfigurationPropertyAttribute>().FirstOrDefault();
            }
            if (configurationPropertyAttribute != null)
            {
                configurationProperty = parentElement.ElementInformation.Properties[configurationPropertyAttribute.Name];
            }

            this.declaringElement.PropertyChanged += DeclaringElementPropertyChanged;
        }

        /// <summary>
        /// The name of the configuration element or attribute.
        /// </summary>
        public virtual string ConfigurationName
        {
            get { return configurationPropertyAttribute != null ? configurationPropertyAttribute.Name : string.Empty; }
        }

        ///<summary>
        /// Returns <see langword="true" /> if the property is required.
        ///</summary>
        public override bool IsRequired
        {
            get { return configurationProperty != null ? configurationProperty.IsRequired : false; }
        }

        ///<summary>
        /// The element that contains the property.
        ///</summary>
        public ElementViewModel DeclaringElement
        {
            get { return declaringElement; }
        }

        /// <summary>
        /// Indicates the object is being disposed.
        /// </summary>
        /// <param name="disposing">Indicates <see cref="ViewModel.Dispose(bool)"/> was invoked through an explicit call to <see cref="ViewModel.Dispose()"/> instead of a finalizer call.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.declaringElement.PropertyChanged -= DeclaringElementPropertyChanged;
            }

            base.Dispose(disposing);
        }

        private void DeclaringElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Name")
            {
                this.OnPropertyChanged("ContainingElementDisplayName");
            }
        }

        #region ILogicalPropertyContainerElement Members

        ElementViewModel ILogicalPropertyContainerElement.ContainingElement
        {
            get 
            { 
                return DeclaringElement; 
            }
        }

        string ILogicalPropertyContainerElement.ContainingElementDisplayName
        {
            get { return DeclaringElement.Name; }
        }

        #endregion

        #region IEnvironmentalOverridesProperty Members

        /// <summary>
        /// Gets a value indicating that this property supports the ability to be over-ridden.
        /// </summary>
        public virtual bool SupportsOverride
        {
            get
            {
                if (!DeclaringElement.IsElementPathReliableXPath) return false;

                //we need this to be sure the property attribute name is correct.
                if (configurationProperty == null) return false;

                if (Hidden) return false;

                if (ReadOnly) return false;

                var overridesAttribute = Attributes.OfType<EnvironmentalOverridesAttribute>().FirstOrDefault();
                if (overridesAttribute == null || overridesAttribute.StorageConverterType == null)
                {
                    if ((!typeof(IConvertible).IsAssignableFrom(PropertyType)) &&
                        (!typeof(ConfigurationElement).IsAssignableFrom(PropertyType)))
                        return false;
                }

                return true;
            }
        }


        /// <summary>
        /// The name of the attribute which is used to serialize this configuration property in XML.
        /// </summary>
        public virtual string PropertyAttributeName
        {
            get { return configurationProperty.Name; }
        }


        /// <summary>
        /// The XPath to the XML element that declares the attribute for this configuration property.
        /// </summary>
        public virtual string ContainingElementXPath 
        {
            get { return DeclaringElement.Path; }
        }

        /// <summary>
        /// The name of the configuration section that contains the property.
        /// </summary>
        public virtual string ConfigurationSectionName
        {
            get { return declaringElement.ContainingSection.SectionName; }
        }

        /// <summary>
        /// The XPath to the XML element that declares the containing configuration section.
        /// </summary>
        public virtual string ContainingSectionXPath
        {
            get { return declaringElement.ContainingSection.Path; }
        }

        /// <summary>
        /// The <see cref="TypeConverter"/> that converts the internal overridden value to a string that can be stored in the delta configuration file.<br/>
        /// </summary>
        /// <remarks>
        /// In order to use a default implementation, return an instance of <see cref="Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.EnvironmentalOverrides.DefaultDeltaConfigurationStorageConverter"/>.
        /// </remarks>
        public virtual TypeConverter DeltaConfigurationStorageConverter 
        {
            get
            {
                var overridesAttribute = Attributes.OfType<EnvironmentalOverridesAttribute>().FirstOrDefault();
                if (overridesAttribute == null || overridesAttribute.StorageConverterType == null)
                {
                    return new DefaultDeltaConfigurationStorageConverter();
                }
                TypeConverter storageConverter = Activator.CreateInstance(overridesAttribute.StorageConverterType) as TypeConverter;
                return new InteropDeltaStorageConverter(storageConverter);
            }
        }
        

        #endregion
    }
}
