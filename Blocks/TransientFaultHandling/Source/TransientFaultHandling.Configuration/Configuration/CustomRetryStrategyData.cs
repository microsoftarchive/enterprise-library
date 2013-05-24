#region license
// ==============================================================================
// Microsoft patterns & practices Enterprise Library
// Transient Fault Handling Application Block
// ==============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ==============================================================================
#endregion

using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling.Configuration
{
    using System;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Configuration;
    using System.Globalization;

    using Common.Configuration;
    using Common.Configuration.Design;
    using Common.Configuration.Design.Validation;

    using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;

    /// <summary>
    /// Holds configuration information for custom retry strategies.
    /// </summary>
    [TypePickingCommand(Replace = CommandReplacement.DefaultAddCommandReplacement)]
    [ResourceDescription(typeof(DesignResources), "CustomRetryStrategyDataDescription")]
    [ResourceDisplayName(typeof(DesignResources), "CustomRetryStrategyDataDisplayName")]
    public sealed class CustomRetryStrategyData : RetryStrategyData, IHelperAssistedCustomConfigurationData<CustomRetryStrategyData>
    {
        private readonly CustomProviderDataHelper<CustomRetryStrategyData> helper;

        /// <summary>
        /// Initializes a new instance of the CustomRetryStrategyData class.
        /// </summary>
        public CustomRetryStrategyData()
        {
            this.helper = new CustomProviderDataHelper<CustomRetryStrategyData>(this);
        }

        /// <summary>
        /// Initializes a new instance of the CustomRetryStrategyData class by using the specified name and provider type.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The provider type.</param>
        public CustomRetryStrategyData(string name, Type type)
        {
            this.helper = new CustomProviderDataHelper<CustomRetryStrategyData>(this);
            this.Name = name;
            this.Type = type;
        }

        /// <summary>
        /// Initializes a new instance of the CustomRetryStrategyData class by using the specified name and fully qualified provider type name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="typeName">The fully qualified type name of the provider type.</param>
        public CustomRetryStrategyData(string name, string typeName)
        {
            this.helper = new CustomProviderDataHelper<CustomRetryStrategyData>(this);
            this.Name = name;
            this.TypeName = typeName;
        }

        /// <summary>
        /// Gets or sets the name of the retry strategy.
        /// </summary>
        [ConfigurationProperty(nameProperty, DefaultValue = "Custom Retry Strategy")]
        public override string Name
        {
            get { return base.Name; }
            set { base.Name = value; }
        }

        /// <summary>
        /// Overridden in order to apply <see cref="BrowsableAttribute"/>.
        /// </summary>
        [Browsable(true)]
        [Editor(CommonDesignTime.EditorTypes.TypeSelector, CommonDesignTime.EditorTypes.UITypeEditor)]
        [BaseType(typeof(RetryStrategy), typeof(CustomRetryStrategyData))]
        public override string TypeName
        {
            get { return base.TypeName; }
            set { base.TypeName = value; }
        }

        /// <summary>
        /// Gets custom configuration attributes.
        /// </summary>
        [Validation(TransientFaultHandlingDesignTime.ValidatorTypes.NameValueCollectionValidator)]
        public NameValueCollection Attributes
        {
            get { return this.helper.Attributes; }
        }

        /// <summary>
        /// Gets the helper.
        /// </summary>
        CustomProviderDataHelper<CustomRetryStrategyData>
            IHelperAssistedCustomConfigurationData<CustomRetryStrategyData>.Helper
        {
            get { return this.helper; }
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
            get { return this.helper.Properties; }
        }

        /// <summary>Invokes the inherited behavior.</summary>
        /// <param name="property">Gets the given property value.</param>
        /// <returns>The requested property's value.</returns>
        object IHelperAssistedCustomConfigurationData<CustomRetryStrategyData>.BaseGetPropertyValue(
            ConfigurationProperty property)
        {
            return base[property];
        }

        /// <summary>Invokes the inherited behavior.</summary>
        /// <param name="property">Sets the given property.</param>
        /// <param name="value">New value for the property.</param>
        void IHelperAssistedCustomConfigurationData<CustomRetryStrategyData>.BaseSetPropertyValue(
            ConfigurationProperty property, object value)
        {
            base[property] = value;
        }

        /// <summary>Invokes the inherited behavior.</summary>
        /// <param name="sourceElement">Source configuration element.</param>
        /// <param name="parentElement">Parent configuration element.</param>
        /// <param name="saveMode">The configuration save mode.</param>
        void IHelperAssistedCustomConfigurationData<CustomRetryStrategyData>.BaseUnmerge(
            ConfigurationElement sourceElement,
            ConfigurationElement parentElement,
            ConfigurationSaveMode saveMode)
        {
            base.Unmerge(sourceElement, parentElement, saveMode);
        }

        /// <summary>Invokes the inherited behavior.</summary>
        /// <param name="parentElement">Parent element.</param>
        void IHelperAssistedCustomConfigurationData<CustomRetryStrategyData>.BaseReset(
            ConfigurationElement parentElement)
        {
            base.Reset(parentElement);
        }

        /// <summary>Invokes the inherited behavior.</summary>
        /// <returns>true if the element has been modified; otherwise, false.</returns>
        bool IHelperAssistedCustomConfigurationData<CustomRetryStrategyData>.BaseIsModified()
        {
            return base.IsModified();
        }

        /// <summary>
        /// Sets the attribute value for a key.
        /// </summary>
        /// <param name="key">The attribute name.</param>
        /// <param name="value">The attribute value.</param>
        public void SetAttributeValue(string key, string value)
        {
            this.helper.HandleSetAttributeValue(key, value);
        }

        /// <summary>
        /// Builds the <see cref="RetryStrategy"/> from the configuration settings.
        /// </summary>
        /// <returns>The retry strategy to use when transient errors occur.</returns>
        public override RetryStrategy BuildRetryStrategy()
        {
            var collectionArgument = this.Attributes ?? new NameValueCollection();

            var constructor = this.Type.GetConstructor(new[] { typeof(string), typeof(bool), typeof(NameValueCollection) });

            if (constructor == null)
            {
                throw new ArgumentException(
                    string.Format(CultureInfo.CurrentCulture, Resources.CustomRetryStrategyDoesNotProvideCorrectConstructor, this.Type.FullName));
            }

            return (RetryStrategy)constructor.Invoke(new object[] { this.Name, this.FirstFastRetry, collectionArgument });
        }

        /// <summary>
        /// Modifies the <see cref="CustomRetryStrategyData"/> object to remove all values that should not be saved. 
        /// </summary>
        /// <param name="sourceElement">A <see cref="ConfigurationElement"/> object at the current level that contains a merged view of the properties.</param>
        /// <param name="parentElement">A parent <see cref="ConfigurationElement"/> object or <see langword="null"/> if this is the top level.</param>
        /// <param name="saveMode">One of the enumeration values that specifies the save mode.</param>
        protected override void Unmerge(ConfigurationElement sourceElement, ConfigurationElement parentElement, ConfigurationSaveMode saveMode)
        {
            this.helper.HandleUnmerge(sourceElement, parentElement, saveMode);
        }

        /// <summary>
        /// Resets the internal state of the <see cref="CustomRetryStrategyData"/> object, 
        /// including the locks and the properties collection.
        /// </summary>
        /// <param name="parentElement">The parent element.</param>
        protected override void Reset(ConfigurationElement parentElement)
        {
            this.helper.HandleReset(parentElement);
        }

        /// <summary>
        /// Indicates whether this configuration element has been modified since it was last 
        /// saved or loaded when implemented in a derived class.
        /// </summary>
        /// <returns><see langword="true"/> if the element has been modified; otherwise, <see langword="false"/>. </returns>
        protected override bool IsModified()
        {
            return this.helper.HandleIsModified();
        }

        /// <summary>
        /// Called when an unknown attribute is encountered while deserializing the <see cref="CustomRetryStrategyData"/> object.
        /// </summary>
        /// <param name="name">The name of the unrecognized attribute.</param>
        /// <param name="value">The value of the unrecognized attribute.</param>
        /// <returns><see langword="true"/> if the processing of the element should continue; otherwise, <see langword="false"/>.</returns>
        protected override bool OnDeserializeUnrecognizedAttribute(string name, string value)
        {
            return this.helper.HandleOnDeserializeUnrecognizedAttribute(name, value);
        }
    }
}
