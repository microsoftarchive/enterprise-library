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
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    /// <summary>
    /// A configuration element that allows you to configure arbitrary
    /// call handlers that don't otherwise have configuration support.
    /// </summary>
    [ResourceDescription(typeof(DesignResources), "CustomCallHandlerDataDescription")]
    [ResourceDisplayName(typeof(DesignResources), "CustomCallHandlerDataDisplayName")]
    [TypePickingCommand(TitleResourceName = "CustomCallHandlerDataDisplayName", TitleResourceType = typeof(DesignResources), Replace = CommandReplacement.DefaultAddCommandReplacement)]
    public class CustomCallHandlerData : CallHandlerData, IHelperAssistedCustomConfigurationData<CustomCallHandlerData>
    {
        CustomProviderDataHelper<CustomCallHandlerData> helper;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomCallHandlerData"/> class.
        /// </summary>
        public CustomCallHandlerData()
        {
            helper = new CustomProviderDataHelper<CustomCallHandlerData>(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomCallHandlerData"/> class by using the specified handler name and type.
        /// </summary>
        /// <param name="name">The name of the handler instance.</param>
        /// <param name="type">The type of handler to configure.</param>
        public CustomCallHandlerData(string name, Type type)
        {
            helper = new CustomProviderDataHelper<CustomCallHandlerData>(this);
            Name = name;
            Type = type;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomCallHandlerData"/> class by using the specified handler name and type name.
        /// </summary>
        /// <param name="name">The name of the handler instance.</param>
        /// <param name="typeName">The name of the type of handler to configure.</param>
        public CustomCallHandlerData(string name, string typeName)
        {
            helper = new CustomProviderDataHelper<CustomCallHandlerData>(this);
            Name = name;
            TypeName = typeName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomCallHandlerData"/> class by using the specified handler name, type name, and handler order.
        /// </summary>
        /// <param name="name">The name of the handler instance.</param>
        /// <param name="typeName">The name of the type of handler to configure.</param>
        /// <param name="handlerOrder">The order of the handler type to configure.</param>
        public CustomCallHandlerData(string name, string typeName, int handlerOrder)
        {
            helper = new CustomProviderDataHelper<CustomCallHandlerData>(this);
            Name = name;
            TypeName = typeName;
            this.Order = handlerOrder;
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
        /// Overridden in order to apply the <see cref="BrowsableAttribute"/> attribute.
        /// </summary>
        [Browsable(true)]
        [Editor(CommonDesignTime.EditorTypes.TypeSelector, CommonDesignTime.EditorTypes.UITypeEditor)]
        [BaseType(typeof(ICallHandler), typeof(CustomCallHandlerData))]
        [ResourceDescription(typeof(DesignResources), "CustomCallHandlerDataTypeNameDescription")]
        [ResourceDisplayName(typeof(DesignResources), "CustomCallHandlerDataTypeNameDisplayName")]
        public override string TypeName
        {
            get { return base.TypeName; }
            set { base.TypeName = value; }
        }

        /// <summary>
        /// Gets or sets custom configuration attributes.
        /// </summary>
        /// <value>A collection of attributes.</value>
        [Validation(CommonDesignTime.ValidationTypeNames.NameValueCollectionValidator)]
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
        /// Modifies the <see cref="CustomCallHandlerData"/> object to remove all values that should not be saved. 
        /// </summary>
        /// <param name="sourceElement">A <see cref="ConfigurationElement"/> object at the current level that contains a merged view of the properties.</param>
        /// <param name="parentElement">A parent <see cref="ConfigurationElement"/> object or <see langword="null"/> if this is the top level.</param>		
        /// <param name="saveMode">A <see cref="ConfigurationSaveMode"/> that determines which property values to include.</param>
        protected override void Unmerge(ConfigurationElement sourceElement, ConfigurationElement parentElement, ConfigurationSaveMode saveMode)
        {
            helper.HandleUnmerge(sourceElement, parentElement, saveMode);
        }

        /// <summary>
        /// Resets the internal state of the <see cref="CustomCallHandlerData"/> object, 
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
        /// Called when an unknown attribute is encountered while deserializing the <see cref="CustomCallHandlerData"/> object.
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
        CustomProviderDataHelper<CustomCallHandlerData> IHelperAssistedCustomConfigurationData<CustomCallHandlerData>.Helper
        {
            get { return helper; }
        }

        /// <summary>Invokes the inherited behavior.</summary>
        /// <param name="property">Gets the given property value.</param>
        /// <returns>the requested property's value</returns>
        object IHelperAssistedCustomConfigurationData<CustomCallHandlerData>.BaseGetPropertyValue(ConfigurationProperty property)
        {
            return base[property];
        }

        /// <summary>Invokes the inherited behavior.</summary>
        /// <param name="property">Sets the given property.</param>
        /// <param name="value">New value for the property.</param>
        void IHelperAssistedCustomConfigurationData<CustomCallHandlerData>.BaseSetPropertyValue(ConfigurationProperty property, object value)
        {
            base[property] = value;
        }

        /// <summary>Invokes the inherited behavior.</summary>
        /// <param name="sourceElement">Source configuration element</param>
        /// <param name="parentElement">Parent configuration element</param>
        /// <param name="saveMode">ConfigurationSaveMode</param>
        void IHelperAssistedCustomConfigurationData<CustomCallHandlerData>.BaseUnmerge(ConfigurationElement sourceElement,
                                                                                       ConfigurationElement parentElement,
                                                                                       ConfigurationSaveMode saveMode)
        {
            base.Unmerge(sourceElement, parentElement, saveMode);
        }

        /// <summary>Invokes the inherited behavior.</summary>
        /// <param name="parentElement">Parent element</param>
        void IHelperAssistedCustomConfigurationData<CustomCallHandlerData>.BaseReset(ConfigurationElement parentElement)
        {
            base.Reset(parentElement);
        }

        /// <summary>Invokes the inherited behavior.</summary>
        /// <returns>True if element has been modified, false if not.</returns>
        bool IHelperAssistedCustomConfigurationData<CustomCallHandlerData>.BaseIsModified()
        {
            return base.IsModified();
        }

        /// <summary>
        /// Configures an <see cref="IUnityContainer" /> to resolve the represented matching rule by using the specified name.
        /// </summary>
        /// <param name="container">The container to configure.</param>
        /// <param name="registrationName">The name of the registration.</param>
        protected override void DoConfigureContainer(IUnityContainer container, string registrationName)
        {
            container.RegisterType(
                typeof(ICallHandler),
                this.Type,
                registrationName,
                new ContainerControlledLifetimeManager(),
                new InjectionConstructor(this.Attributes),
                new InjectionProperty("Order", this.Order));
        }
    }
}
