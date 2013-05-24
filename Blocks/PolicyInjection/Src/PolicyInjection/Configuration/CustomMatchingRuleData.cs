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
using System.ComponentModel;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design.Validation;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration
{
    /// <summary>
    /// A configuration element that lets you configure matching rules
    /// that don't have any explicit configuration support.
    /// </summary>
    [ResourceDescription(typeof(DesignResources), "CustomMatchingRuleDataDescription")]
    [ResourceDisplayName(typeof(DesignResources), "CustomMatchingRuleDataDisplayName")]
    [TypePickingCommand(TitleResourceName = "CustomMatchingRuleDataDisplayName", TitleResourceType = typeof(DesignResources), Replace = CommandReplacement.DefaultAddCommandReplacement)]
    public class CustomMatchingRuleData : MatchingRuleData, IHelperAssistedCustomConfigurationData<CustomMatchingRuleData>
    {
        CustomProviderDataHelper<CustomMatchingRuleData> helper;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomMatchingRuleData"/> class.
        /// </summary>
        public CustomMatchingRuleData()
        {
            helper = new CustomProviderDataHelper<CustomMatchingRuleData>(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomMatchingRuleData"/> class by using the specified matching rule name and type.
        /// </summary>
        /// <param name="name">The name of the matching rule instance.</param>
        public CustomMatchingRuleData(string name)
        {
            helper = new CustomProviderDataHelper<CustomMatchingRuleData>(this);
            Name = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomMatchingRuleData"/> class by using the specified matching rule name and type.
        /// </summary>
        /// <param name="name">The name of the matching rule instance.</param>
        /// <param name="type">The type of matching rule to configure.</param>
        public CustomMatchingRuleData(string name, Type type)
        {
            helper = new CustomProviderDataHelper<CustomMatchingRuleData>(this);
            Name = name;
            Type = type;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomMatchingRuleData"/> class by using the specified matching rule name and type name.
        /// </summary>
        /// <param name="name">The name of the matching rule instance.</param>
        /// <param name="typeName">The name of the type of matching rule to configure.</param>
        public CustomMatchingRuleData(string name, string typeName)
        {
            helper = new CustomProviderDataHelper<CustomMatchingRuleData>(this);
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
        [BaseType(typeof(IMatchingRule), typeof(CustomMatchingRuleData))]
        [ResourceDescription(typeof(DesignResources), "CustomMatchingRuleDataTypeNameDescription")]
        [ResourceDisplayName(typeof(DesignResources), "CustomMatchingRuleDataTypeNameDisplayName")]
        public override string TypeName
        {
            get { return base.TypeName; }
            set { base.TypeName = value; }
        }

        /// <summary>
        /// Gets or sets custom configuration attributes.
        /// </summary>
        /// <value>The attribute collection.</value>  
        [Validation(PolicyInjectionDesignTime.Validators.NameValueCollectionValidator)]
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
        /// Modifies the <see cref="CustomMatchingRuleData"/> object to remove all values that should not be saved. 
        /// </summary>
        /// <param name="sourceElement">A <see cref="ConfigurationElement"/> object at the current level containing a merged view of the properties.</param>
        /// <param name="parentElement">A parent <see cref="ConfigurationElement"/> object or <see langword="null"/> if this is the top level.</param>		
        /// <param name="saveMode">One of the <see cref="ConfigurationSaveMode"/> values.</param>
        protected override void Unmerge(ConfigurationElement sourceElement, ConfigurationElement parentElement, ConfigurationSaveMode saveMode)
        {
            helper.HandleUnmerge(sourceElement, parentElement, saveMode);
        }

        /// <summary>
        /// Resets the internal state of the <see cref="CustomMatchingRuleData"/> object, 
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
        /// Called when an unknown attribute is encountered while deserializing the <see cref="CustomMatchingRuleData"/> object.
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
        CustomProviderDataHelper<CustomMatchingRuleData> IHelperAssistedCustomConfigurationData<CustomMatchingRuleData>.Helper
        {
            get { return helper; }
        }

        /// <summary>Invokes the inherited behavior.</summary>
        /// <param name="property">The property to get.</param>
        /// <returns>The value of the requested property.</returns>
        object IHelperAssistedCustomConfigurationData<CustomMatchingRuleData>.BaseGetPropertyValue(ConfigurationProperty property)
        {
            return base[property];
        }

        /// <summary>Invokes the inherited behavior.</summary>
        /// <param name="property">Property to set.</param>
        /// <param name="value">New value for property.</param>
        void IHelperAssistedCustomConfigurationData<CustomMatchingRuleData>.BaseSetPropertyValue(ConfigurationProperty property, object value)
        {
            base[property] = value;
        }

        /// <summary>Invokes the inherited behavior.</summary>
        /// <param name="sourceElement">Source configuration element</param>
        /// <param name="parentElement">Parent configuration element</param>
        /// <param name="saveMode">ConfigurationSaveMode</param>
        void IHelperAssistedCustomConfigurationData<CustomMatchingRuleData>.BaseUnmerge(ConfigurationElement sourceElement,
            ConfigurationElement parentElement,
            ConfigurationSaveMode saveMode)
        {
            base.Unmerge(sourceElement, parentElement, saveMode);
        }

        /// <summary>Invokes the inherited behavior.</summary>
        /// <param name="parentElement">Parent element</param>
        void IHelperAssistedCustomConfigurationData<CustomMatchingRuleData>.BaseReset(ConfigurationElement parentElement)
        {
            base.Reset(parentElement);
        }

        /// <summary>Invokes the inherited behavior.</summary>
        /// <returns>True if element has been modified, false if not.</returns>
        bool IHelperAssistedCustomConfigurationData<CustomMatchingRuleData>.BaseIsModified()
        {
            return base.IsModified();
        }

        /// <summary>
        /// Configures an <see cref="IUnityContainer"/> to resolve the represented matching rule by using the specified name.
        /// </summary>
        /// <param name="container">The container to configure.</param>
        /// <param name="registrationName">The name of the registration.</param>
        protected override void DoConfigureContainer(IUnityContainer container, string registrationName)
        {
            container.RegisterType(
                typeof(IMatchingRule),
                this.Type,
                registrationName,
                new InjectionConstructor(this.Attributes));
        }
    }
}
