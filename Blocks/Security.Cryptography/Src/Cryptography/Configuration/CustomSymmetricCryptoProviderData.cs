//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Cryptography Application Block
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
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration
{
    /// <summary>
    /// Configuration object for Custom Providers.
    /// </summary>
    [ResourceDescription(typeof(DesignResources), "CustomSymmetricCryptoProviderDataDescription")]
    [ResourceDisplayName(typeof(DesignResources), "CustomSymmetricCryptoProviderDataDisplayName")]
    [TypePickingCommand(TitleResourceName = "CustomSymmetricCryptoProviderDataAddCommand", TitleResourceType=typeof(DesignResources))]
    public class CustomSymmetricCryptoProviderData
        : SymmetricProviderData, IHelperAssistedCustomConfigurationData<CustomSymmetricCryptoProviderData>
    {
        private readonly CustomProviderDataHelper<CustomSymmetricCryptoProviderData> helper;

        /// <summary>
        /// Initializes with default values.
        /// </summary>
        public CustomSymmetricCryptoProviderData()
        {
            helper = new CustomProviderDataHelper<CustomSymmetricCryptoProviderData>(this);
        }

        /// <summary>
        /// Initializes with name and provider type.
        /// </summary>
        public CustomSymmetricCryptoProviderData(string name, Type type)
        {
            helper = new CustomProviderDataHelper<CustomSymmetricCryptoProviderData>(this);
            Name = name;
            Type = type;
        }

        /// <summary>
        /// Initializes with name and fully qualified type name of the provider type.
        /// </summary>
        public CustomSymmetricCryptoProviderData(string name, string typeName)
        {
            helper = new CustomProviderDataHelper<CustomSymmetricCryptoProviderData>(this);
            Name = name;
            TypeName = typeName;
        }

        /// <summary>
        /// Gets a <see cref="ConfigurationPropertyCollection"/> of the properties that are defined for 
        /// this configuration element when implemented in a derived class. 
        /// </summary>
        /// <value>
        /// A <see cref="ConfigurationPropertyCollection"/> of the properties that are defined for this
        /// configuration element when implemented in a derived class. 
        /// </value>
        protected override ConfigurationPropertyCollection Properties
        {
            get { return helper.Properties; }
        }

        /// <summary>
        /// Overridden in order to apply <see cref="BrowsableAttribute"/>.
        /// </summary>
        [Browsable(true)]
        [Editor(CommonDesignTime.EditorTypes.TypeSelector, CommonDesignTime.EditorTypes.UITypeEditor)]
        [BaseType(typeof(ISymmetricCryptoProvider), typeof(CustomSymmetricCryptoProviderData))]
        [ResourceDescription(typeof(DesignResources), "CustomSymmetricCryptoProviderDataTypeNameDescription")]
        [ResourceDisplayName(typeof(DesignResources), "CustomSymmetricCryptoProviderDataTypeNameDisplayName")]
        public override string TypeName
        {
            get { return base.TypeName; }
            set { base.TypeName = value; }
        }

        #region IHelperAssistedCustomConfigurationData<CustomSymmetricCryptoProviderData> Members

        /// <summary>
        /// Gets or sets custom configuration attributes.
        /// </summary>        		
        public NameValueCollection Attributes
        {
            get { return helper.Attributes; }
        }

        /// <summary>
        /// Gets the helper.
        /// </summary>
        CustomProviderDataHelper<CustomSymmetricCryptoProviderData>
            IHelperAssistedCustomConfigurationData<CustomSymmetricCryptoProviderData>.Helper
        {
            get { return helper; }
        }

        /// <summary>Invokes the inherited behavior.</summary>
        object IHelperAssistedCustomConfigurationData<CustomSymmetricCryptoProviderData>.BaseGetPropertyValue(
            ConfigurationProperty property)
        {
            return base[property];
        }

        /// <summary>Invokes the inherited behavior.</summary>
        void IHelperAssistedCustomConfigurationData<CustomSymmetricCryptoProviderData>.BaseSetPropertyValue(
            ConfigurationProperty property, object value)
        {
            base[property] = value;
        }

        /// <summary>Invokes the inherited behavior.</summary>
        void IHelperAssistedCustomConfigurationData<CustomSymmetricCryptoProviderData>.BaseUnmerge(
            ConfigurationElement sourceElement,
            ConfigurationElement parentElement,
            ConfigurationSaveMode saveMode)
        {
            base.Unmerge(sourceElement, parentElement, saveMode);
        }

        /// <summary>Invokes the inherited behavior.</summary>
        void IHelperAssistedCustomConfigurationData<CustomSymmetricCryptoProviderData>.BaseReset(
            ConfigurationElement parentElement)
        {
            base.Reset(parentElement);
        }

        /// <summary>Invokes the inherited behavior.</summary>
        bool IHelperAssistedCustomConfigurationData<CustomSymmetricCryptoProviderData>.BaseIsModified()
        {
            return base.IsModified();
        }

        #endregion

        /// <summary>
        /// Sets the attribute value for a key.
        /// </summary>
        /// <param name="key">The attribute name.</param>
        /// <param name="value">The attribute value.</param>
        public void SetAttributeValue(string key, string value)
        {
            helper.HandleSetAttributeValue(key, value);
        }

        /// <summary>
        /// Modifies the <see cref="CustomSymmetricCryptoProviderData"/> object to remove all values that should not be saved. 
        /// </summary>
        /// <param name="sourceElement">A <see cref="ConfigurationElement"/> object at the current level containing a merged view of the properties.</param>
        /// <param name="parentElement">A parent <see cref="ConfigurationElement"/> object or <see langword="null"/> if this is the top level.</param>		
        /// <param name="saveMode">One of the <see cref="ConfigurationSaveMode"/> values.</param>
        protected override void Unmerge(ConfigurationElement sourceElement, ConfigurationElement parentElement,
                                        ConfigurationSaveMode saveMode)
        {
            helper.HandleUnmerge(sourceElement, parentElement, saveMode);
        }

        /// <summary>
        /// Resets the internal state of the <see cref="CustomSymmetricCryptoProviderData"/> object, 
        /// including the locks and the properties collection.
        /// </summary>
        /// <param name="parentElement">The parent element.</param>
        protected override void Reset(ConfigurationElement parentElement)
        {
            helper.HandleReset(parentElement);
        }

        /// <summary>
        /// Indicates whether this configuration element has been modified since it was last 
        /// saved or loaded when implemented in a derived class.
        /// </summary>
        /// <returns><see langword="true"/> if the element has been modified; otherwise, <see langword="false"/>. </returns>
        protected override bool IsModified()
        {
            return helper.HandleIsModified();
        }

        /// <summary>
        /// Called when an unknown attribute is encountered while deserializing the <see cref="CustomSymmetricCryptoProviderData"/> object.
        /// </summary>
        /// <param name="name">The name of the unrecognized attribute.</param>
        /// <param name="value">The value of the unrecognized attribute.</param>
        /// <returns><see langword="true"/> if the processing of the element should continue; otherwise, <see langword="false"/>.</returns>
        protected override bool OnDeserializeUnrecognizedAttribute(string name, string value)
        {
            return helper.HandleOnDeserializeUnrecognizedAttribute(name, value);
        }

        /// <summary>
        /// Creates a <see cref="TypeRegistration"/> instance describing the provider represented by 
        /// this configuration object.
        /// </summary>
        /// <returns>A <see cref="TypeRegistration"/> instance describing a provider.</returns>
        public override IEnumerable<TypeRegistration> GetRegistrations(IConfigurationSource configurationSource)
        {
            yield return
                new TypeRegistration(
                    RegistrationExpressionBuilder.BuildExpression(Type, Attributes),
                    typeof (ISymmetricCryptoProvider))
                    {
                        Name = Name,
                        Lifetime = TypeRegistrationLifetime.Transient
                    };
        }
    }
}
