//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Policy Injection Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.Collections.Specialized;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration
{
    /// <summary>
    /// A configuration element that allows you to configure arbitrary
    /// call handlers that don't otherwise have configuration support.
    /// </summary>
    [Assembler(typeof(CustomProviderAssembler<ICallHandler, CallHandlerData, CustomCallHandlerData>))]
    public class CustomCallHandlerData : CallHandlerData, IHelperAssistedCustomConfigurationData<CustomCallHandlerData>
    {
        CustomProviderDataHelper<CustomCallHandlerData> helper;

        /// <summary>
        /// Constructs a new <see cref="CustomCallHandlerData"/> instance.
        /// </summary>
        public CustomCallHandlerData()
        {
            helper = new CustomProviderDataHelper<CustomCallHandlerData>(this);
        }

        /// <summary>
        /// Constructs a new <see cref="CustomCallHandlerData"/> instance.
        /// </summary>
        /// <param name="name">Name of handler instance.</param>
        /// <param name="type">Type of handler to configure.</param>
        public CustomCallHandlerData(string name, Type type)
        {
            helper = new CustomProviderDataHelper<CustomCallHandlerData>(this);
            Name = name;
            Type = type;
        }

        /// <summary>
        /// Constructs a new <see cref="CustomCallHandlerData"/> instance.
        /// </summary>
        /// <param name="name">Name of handler instance.</param>
        /// <param name="typeName">Name of the handler type to configure.</param>
        public CustomCallHandlerData(string name, string typeName)
        {
            helper = new CustomProviderDataHelper<CustomCallHandlerData>(this);
            Name = name;
            TypeName = typeName;
        }

        /// <summary>
        /// Constructs a new <see cref="CustomCallHandlerData"/> instance.
        /// </summary>
        /// <param name="name">Name of handler instance.</param>
        /// <param name="typeName">Name of the handler type to configure.</param>
        /// <param name="handlerOrder">Order of the handler type to configure.</param>
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
        /// Gets or sets custom configuration attributes.
        /// </summary>
        /// <value>Collection of attributes.</value>
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
        /// <param name="sourceElement">A <see cref="ConfigurationElement"/> object at the current level containing a merged view of the properties.</param>
        /// <param name="parentElement">A parent <see cref="ConfigurationElement"/> object or <see langword="null"/> if this is the top level.</param>		
        /// <param name="saveMode">One of the <see cref="ConfigurationSaveMode"/> values.</param>
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
    }
}
