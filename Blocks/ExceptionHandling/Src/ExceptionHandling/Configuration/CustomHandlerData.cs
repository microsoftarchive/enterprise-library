//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
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
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration
{
    /// <summary>
    /// Configuration object for Custom Providers.
    /// </summary>
    [ResourceDescription(typeof(DesignResources), "CustomHandlerDataDescription")]
    [ResourceDisplayName(typeof(DesignResources), "CustomHandlerDataDisplayName")]
    [TypePickingCommand(TitleResourceName = "CustomHandlerDataDisplayName", TitleResourceType = typeof(DesignResources), Replace = CommandReplacement.DefaultAddCommandReplacement)]
    public class CustomHandlerData
        : ExceptionHandlerData, IHelperAssistedCustomConfigurationData<CustomHandlerData>
    {
        private readonly CustomProviderDataHelper<CustomHandlerData> helper;

        /// <summary>
        /// Initializes with default values.
        /// </summary>
        public CustomHandlerData()
        {
            helper = new CustomProviderDataHelper<CustomHandlerData>(this);
        }

        /// <summary>
        /// Initializes with name and provider type.
        /// </summary>
        public CustomHandlerData(string name, Type type)
        {
            helper = new CustomProviderDataHelper<CustomHandlerData>(this);
            Name = name;
            Type = type;
        }

        /// <summary>
        /// Initializes with name and provider type.
        /// </summary>
        public CustomHandlerData(string name, string typeName)
        {
            helper = new CustomProviderDataHelper<CustomHandlerData>(this);
            Name = name;
            TypeName = typeName;
        }

        /// <summary>
        /// Gets or sets the fully qualified name of the <see cref="Type"/> the element is the configuration for.
        /// </summary>
        /// <value>
        /// the fully qualified name of the <see cref="Type"/> the element is the configuration for.
        /// </value>
        [Editor(CommonDesignTime.EditorTypes.TypeSelector, CommonDesignTime.EditorTypes.UITypeEditor)]
        [BaseType(typeof(IExceptionHandler), typeof(CustomHandlerData))]
        [ResourceDescription(typeof(DesignResources), "CustomHandlerDataTypeNameDescription")]
        [ResourceDisplayName(typeof(DesignResources), "CustomHandlerDataTypeNameDisplayName")]
        [Browsable(true)]
        public override string TypeName
        {
            get
            {
                return base.TypeName;
            }
            set
            {
                base.TypeName = value;
            }
        }

        /// <summary>
        /// Gets the custom configuration attributes.
        /// </summary>
        [Validation(ExceptionHandlingDesignTime.ValidatorTypes.NameValueCollectionValidator)]
        public NameValueCollection Attributes
        {
            get { return helper.Attributes; }
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
        /// Modifies the <see cref="CustomHandlerData"/> object to remove all values that should not be saved. 
        /// </summary>
        /// <param name="sourceElement">A <see cref="ConfigurationElement"/> object at the current level containing a merged view of the properties.</param>
        /// <param name="parentElement">A parent <see cref="ConfigurationElement"/> object or <see langword="null"/> if this is the top level.</param>		
        /// <param name="saveMode">One of the <see cref="ConfigurationSaveMode"/> values.</param>
        protected override void Unmerge(ConfigurationElement sourceElement, ConfigurationElement parentElement, ConfigurationSaveMode saveMode)
        {
            helper.HandleUnmerge(sourceElement, parentElement, saveMode);
        }

        /// <summary>
        /// Resets the internal state of the <see cref="CustomHandlerData"/> object, 
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
        /// Called when an unknown attribute is encountered while deserializing the <see cref="CustomHandlerData"/> object.
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
        CustomProviderDataHelper<CustomHandlerData> IHelperAssistedCustomConfigurationData<CustomHandlerData>.Helper
        {
            get { return helper; }
        }

        /// <summary>Invokes the inherited behavior.</summary>
        object IHelperAssistedCustomConfigurationData<CustomHandlerData>.BaseGetPropertyValue(ConfigurationProperty property)
        {
            return base[property];
        }

        /// <summary>Invokes the inherited behavior.</summary>
        void IHelperAssistedCustomConfigurationData<CustomHandlerData>.BaseSetPropertyValue(ConfigurationProperty property, object value)
        {
            base[property] = value;
        }

        /// <summary>Invokes the inherited behavior.</summary>
        void IHelperAssistedCustomConfigurationData<CustomHandlerData>.BaseUnmerge(ConfigurationElement sourceElement,
                    ConfigurationElement parentElement,
                    ConfigurationSaveMode saveMode)
        {
            base.Unmerge(sourceElement, parentElement, saveMode);
        }

        /// <summary>Invokes the inherited behavior.</summary>
        void IHelperAssistedCustomConfigurationData<CustomHandlerData>.BaseReset(ConfigurationElement parentElement)
        {
            base.Reset(parentElement);
        }

        /// <summary>Invokes the inherited behavior.</summary>
        bool IHelperAssistedCustomConfigurationData<CustomHandlerData>.BaseIsModified()
        {
            return base.IsModified();
        }

        /// <summary>
        /// Builds the exception handler represented by this configuration object.
        /// </summary>
        /// <returns>An <see cref="IExceptionHandler"/>.</returns>
        public override IExceptionHandler BuildExceptionHandler()
        {
            if (string.IsNullOrEmpty(this.TypeName))
            {
                throw new InvalidOperationException(
                    string.Format(CultureInfo.CurrentCulture, Resources.CustomHandlerNoTypeException, this.ElementInformation.Source, this.ElementInformation.LineNumber, this.Name));
            }

            System.Type handlerType;

            try
            {
                handlerType = this.Type;
            }
            catch(Exception e)
            {
                throw new InvalidOperationException(
                    string.Format(CultureInfo.CurrentCulture, Resources.CustomHandlerNoTypeException, this.ElementInformation.Source, this.ElementInformation.LineNumber, this.Name, this.TypeName),
                    e);
            }

            if (!typeof(IExceptionHandler).IsAssignableFrom(handlerType))
            {
                throw new InvalidOperationException(
                    string.Format(CultureInfo.CurrentCulture, Resources.CustomHandlerNotHandlerTypeException, this.ElementInformation.Source, this.ElementInformation.LineNumber, this.Name, this.TypeName));
            }

            var ctor = handlerType.GetConstructor(new[] { typeof(NameValueCollection) });
            if (ctor == null)
            {
                throw new InvalidOperationException(
                    string.Format(CultureInfo.CurrentCulture, Resources.CustomHandlerMissingConstructorException, this.ElementInformation.Source, this.ElementInformation.LineNumber, this.Name, this.TypeName));
            }

            return (IExceptionHandler)Activator.CreateInstance(handlerType, this.Attributes);
        }
    }
}
