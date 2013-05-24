//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
    /// <summary>
    /// Configuration object for custom log formatters.
    /// </summary>
    [ResourceDescription(typeof(DesignResources), "CustomFormatterDataDescription")]
    [ResourceDisplayName(typeof(DesignResources), "CustomFormatterDataDisplayName")]
    [TypePickingCommand(TitleResourceName = "CustomFormatterDataDisplayName", TitleResourceType = typeof(DesignResources), Replace = CommandReplacement.DefaultAddCommandReplacement)]
    public class CustomFormatterData
        : FormatterData, IHelperAssistedCustomConfigurationData<CustomFormatterData>
    {
        private readonly CustomProviderDataHelper<CustomFormatterData> helper;

        /// <summary>
        /// Initializes with default values.
        /// </summary>
        public CustomFormatterData()
        {
            helper = new CustomProviderDataHelper<CustomFormatterData>(this);
        }

        /// <summary>
        /// Initializes with name and provider type.
        /// </summary>
        public CustomFormatterData(string name, Type type)
        {
            helper = new CustomProviderDataHelper<CustomFormatterData>(this);
            Name = name;
            Type = type;
        }

        /// <summary>
        /// Initializes with name and provider type.
        /// </summary>
        public CustomFormatterData(string name, string typeName)
        {
            helper = new CustomProviderDataHelper<CustomFormatterData>(this);
            Name = name;
            TypeName = typeName;
        }

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
        /// Overridden in order to apply <see cref="BrowsableAttribute"/>.
        /// </summary>
        [Browsable(true)]
        [Editor(CommonDesignTime.EditorTypes.TypeSelector, CommonDesignTime.EditorTypes.UITypeEditor)]
        [BaseType(typeof(ILogFormatter), typeof(CustomFormatterData))]
        [ResourceDescription(typeof(DesignResources), "CustomFormatterDataTypeNameDescription")]
        [ResourceDisplayName(typeof(DesignResources), "CustomFormatterDataTypeNameDisplayName")]
        public override string TypeName
        {
            get { return base.TypeName; }
            set { base.TypeName = value; }
        }

        /// <summary>
        /// Gets or sets custom configuration attributes.
        /// </summary>
        [Validation(LoggingDesignTime.ValidatorTypes.NameValueCollectionValidator)]
        public NameValueCollection Attributes
        {
            get { return helper.Attributes; }
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
        /// Modifies the <see cref="CustomFormatterData"/> object to remove all values that should not be saved. 
        /// </summary>
        /// <param name="sourceElement">A <see cref="ConfigurationElement"/> object at the current level containing a merged view of the properties.</param>
        /// <param name="parentElement">A parent <see cref="ConfigurationElement"/> object or <see langword="null"/> if this is the top level.</param>		
        /// <param name="saveMode">One of the <see cref="ConfigurationSaveMode"/> values.</param>
        protected override void Unmerge(ConfigurationElement sourceElement, ConfigurationElement parentElement, ConfigurationSaveMode saveMode)
        {
            helper.HandleUnmerge(sourceElement, parentElement, saveMode);
        }

        /// <summary>
        /// Resets the internal state of the <see cref="CustomFormatterData"/> object, 
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
        /// Called when an unknown attribute is encountered while deserializing the <see cref="CustomFormatterData"/> object.
        /// </summary>
        /// <param name="name">The name of the unrecognized attribute.</param>
        /// <param name="value">The value of the unrecognized attribute.</param>
        /// <returns><see langword="true"/> if the processing of the element should continue; otherwise, <see langword="false"/>.</returns>
        protected override bool OnDeserializeUnrecognizedAttribute(string name, string value)
        {
            return helper.HandleOnDeserializeUnrecognizedAttribute(name, value);
        }

        /// <summary>
        /// Gets the helper.
        /// </summary>
        CustomProviderDataHelper<CustomFormatterData> IHelperAssistedCustomConfigurationData<CustomFormatterData>.Helper
        {
            get { return helper; }
        }

        /// <summary>Invokes the inherited behavior.</summary>
        object IHelperAssistedCustomConfigurationData<CustomFormatterData>.BaseGetPropertyValue(ConfigurationProperty property)
        {
            return base[property];
        }

        /// <summary>Invokes the inherited behavior.</summary>
        void IHelperAssistedCustomConfigurationData<CustomFormatterData>.BaseSetPropertyValue(ConfigurationProperty property, object value)
        {
            base[property] = value;
        }

        /// <summary>Invokes the inherited behavior.</summary>
        void IHelperAssistedCustomConfigurationData<CustomFormatterData>.BaseUnmerge(ConfigurationElement sourceElement,
                    ConfigurationElement parentElement,
                    ConfigurationSaveMode saveMode)
        {
            base.Unmerge(sourceElement, parentElement, saveMode);
        }

        /// <summary>Invokes the inherited behavior.</summary>
        void IHelperAssistedCustomConfigurationData<CustomFormatterData>.BaseReset(ConfigurationElement parentElement)
        {
            base.Reset(parentElement);
        }

        /// <summary>Invokes the inherited behavior.</summary>
        bool IHelperAssistedCustomConfigurationData<CustomFormatterData>.BaseIsModified()
        {
            return base.IsModified();
        }

        /// <summary>
        /// Builds the <see cref="ILogFormatter" /> object represented by this configuration object.
        /// </summary>
        /// <returns>
        /// A formatter.
        /// </returns>
        public override ILogFormatter BuildFormatter()
        {
            return CustomProviderBuildHelper.Build<ILogFormatter, CustomFormatterData>(
                this,
                () => Resources.ExceptionCustomFormatterDataHasNoType,
                () => Resources.ExceptionCustomFormatterDataTypeCannotBeLoaded,
                () => Resources.ExceptionCustomFormatterDataNotFormatter,
                () => Resources.ExceptionCustomFormatterTypeDoesNotHaveConstructor);
        }
    }
}
