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
using System.Collections.Specialized;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration
{
    /// <summary>
    /// A configuration element that allows configuration of arbitrary
    /// policy injectors that don't otherwise have configuration support.
    /// </summary>
    [Assembler(typeof(CustomInjectorAssembler))]
    public class CustomInjectorData : InjectorData, IHelperAssistedCustomConfigurationData<CustomInjectorData>
    {
        CustomProviderDataHelper<CustomInjectorData> helper;

        /// <summary>
        /// Constructs a new <see cref="CustomInjectorData"/> instance.
        /// </summary>
        public CustomInjectorData()
        {
            helper = new CustomProviderDataHelper<CustomInjectorData>(this);
        }

        /// <summary>
        /// Constructs a new <see cref="CustomInjectorData"/> instance.
        /// </summary>
        /// <param name="name">Name of injector instance.</param>
        public CustomInjectorData(string name)
        {
            helper = new CustomProviderDataHelper<CustomInjectorData>(this);
            Name = name;
        }
        /// <summary>
        /// Constructs a new <see cref="CustomInjectorData"/> instance.
        /// </summary>
        /// <param name="name">Name of injector instance.</param>
        /// <param name="type">Type of injector to configure.</param>
        public CustomInjectorData(string name, Type type)
        {
            helper = new CustomProviderDataHelper<CustomInjectorData>(this);
            Name = name;
            Type = type;
        }

        /// <summary>
        /// Constructs a new <see cref="CustomInjectorData"/> instance.
        /// </summary>
        /// <param name="name">Name of injector instance.</param>
        /// <param name="typeName">Name of the injector type to configure.</param>
        public CustomInjectorData(string name, string typeName)
        {
            helper = new CustomProviderDataHelper<CustomInjectorData>(this);
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
        /// Modifies the <see cref="CustomInjectorData"/> object to remove all values that should not be saved. 
        /// </summary>
        /// <param name="sourceElement">A <see cref="ConfigurationElement"/> object at the current level containing a merged view of the properties.</param>
        /// <param name="parentElement">A parent <see cref="ConfigurationElement"/> object or <see langword="null"/> if this is the top level.</param>		
        /// <param name="saveMode">One of the <see cref="ConfigurationSaveMode"/> values.</param>
        protected override void Unmerge(ConfigurationElement sourceElement, ConfigurationElement parentElement, ConfigurationSaveMode saveMode)
        {
            helper.HandleUnmerge(sourceElement, parentElement, saveMode);
        }

        /// <summary>
        /// Resets the internal state of the <see cref="CustomInjectorData"/> object, 
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
        /// Called when an unknown attribute is encountered while deserializing the <see cref="CustomInjectorData"/> object.
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
        CustomProviderDataHelper<CustomInjectorData> IHelperAssistedCustomConfigurationData<CustomInjectorData>.Helper
        {
            get { return helper; }
        }

        /// <summary>Invokes the inherited behavior.</summary>
        /// <param name="property">Gets the given property value.</param>
        /// <returns>the requested property's value</returns>
        object IHelperAssistedCustomConfigurationData<CustomInjectorData>.BaseGetPropertyValue(ConfigurationProperty property)
        {
            return base[property];
        }

        /// <summary>Invokes the inherited behavior.</summary>
        /// <param name="property">Sets the given property.</param>
        /// <param name="value">New value for the property.</param>
        void IHelperAssistedCustomConfigurationData<CustomInjectorData>.BaseSetPropertyValue(ConfigurationProperty property, object value)
        {
            base[property] = value;
        }

        /// <summary>Invokes the inherited behavior.</summary>
        /// <param name="sourceElement">Source configuration element</param>
        /// <param name="parentElement">Parent configuration element</param>
        /// <param name="saveMode">ConfigurationSaveMode</param>
        void IHelperAssistedCustomConfigurationData<CustomInjectorData>.BaseUnmerge(ConfigurationElement sourceElement,
            ConfigurationElement parentElement,
            ConfigurationSaveMode saveMode)
        {
            base.Unmerge(sourceElement, parentElement, saveMode);
        }

        /// <summary>Invokes the inherited behavior.</summary>
        /// <param name="parentElement">Parent element</param>
        void IHelperAssistedCustomConfigurationData<CustomInjectorData>.BaseReset(ConfigurationElement parentElement)
        {
            base.Reset(parentElement);
        }

        /// <summary>Invokes the inherited behavior.</summary>
        /// <returns>True if element has been modified, false if not.</returns>
        bool IHelperAssistedCustomConfigurationData<CustomInjectorData>.BaseIsModified()
        {
            return base.IsModified();
        }
    }

    class CustomInjectorAssembler : CustomProviderAssembler<PolicyInjector, InjectorData, CustomInjectorData>, IAssembler<PolicyInjector, InjectorData>
    {
        /// <summary>
        /// Builds an instance of the subtype of <see cref="PolicyInjector"/> type the receiver knows how to build,  based on 
        /// an a configuration object.
        /// </summary>
        /// <param name="context">The <see cref="IBuilderContext"/> that represents the current building process.</param>
        /// <param name="objectConfiguration">The configuration object that describes the object to build.</param>
        /// <param name="configurationSource">The source for configuration objects.</param>
        /// <param name="reflectionCache">The cache to use retrieving reflection information.</param>
        /// <returns>A fully initialized instance of the <see cref="PolicyInjector"/> subtype.</returns>
        PolicyInjector IAssembler<PolicyInjector, InjectorData>.Assemble(IBuilderContext context,
                                                                    InjectorData objectConfiguration,
                                                                    IConfigurationSource configurationSource,
                                                                    ConfigurationReflectionCache reflectionCache)
        {
            PolicyInjector injector =
                 base.Assemble(context, objectConfiguration, configurationSource, reflectionCache);
            PolicySetFactory policySetFactory = new PolicySetFactory(configurationSource);
            PolicySet policies = policySetFactory.Create();
            injector.Policies = policies;
            return injector;
        }
    }
}
