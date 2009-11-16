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
using System.Collections.Specialized;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration
{
    /// <summary>
    /// A configuration element that lets you configure matching rules
    /// that don't have any explicit configuration support.
    /// </summary>
    [ResourceDescription(typeof(DesignResources), "CustomMatchingRuleDataDescription")]
    [ResourceDisplayName(typeof(DesignResources), "CustomMatchingRuleDataDisplayName")]
    [TypePickingCommand(TitleResourceName = "CustomMatchingRuleDataAddCommand", TitleResourceType=typeof(DesignResources))]
    public class CustomMatchingRuleData : MatchingRuleData, IHelperAssistedCustomConfigurationData<CustomMatchingRuleData>
    {
        CustomProviderDataHelper<CustomMatchingRuleData> helper;

        /// <summary>
        /// Constructs a new <see cref="CustomMatchingRuleData"/> instance.
        /// </summary>
        public CustomMatchingRuleData()
        {
            helper = new CustomProviderDataHelper<CustomMatchingRuleData>(this);
        }

        /// <summary>
        /// Constructs a new <see cref="CustomMatchingRuleData"/> instance.
        /// </summary>
        /// <param name="name">Name of the matching rule instance.</param>
        public CustomMatchingRuleData(string name)
        {
            helper = new CustomProviderDataHelper<CustomMatchingRuleData>(this);
            Name = name;
        }

        /// <summary>
        /// Constructs a new <see cref="CustomMatchingRuleData"/> instance.
        /// </summary>
        /// <param name="name">Name of the matching rule instance.</param>
        /// <param name="type">Type of the matching rule to create.</param>
        public CustomMatchingRuleData(string name, Type type)
        {
            helper = new CustomProviderDataHelper<CustomMatchingRuleData>(this);
            Name = name;
            Type = type;
        }

        /// <summary>
        /// Constructs a new <see cref="CustomMatchingRuleData"/> instance.
        /// </summary>
        /// <param name="name">Name of the matching rule instance.</param>
        /// <param name="typeName">Name of type of matching rule to create.</param>
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
        /// Get the set of <see cref="TypeRegistration"/> objects needed to
        /// register the matching rule represented by this config element and its associated objects.
        /// </summary>
        /// <param name="nameSuffix">A suffix for the names in the generated type registration objects.</param>
        /// <returns>The set of <see cref="TypeRegistration"/> objects.</returns>
        public override IEnumerable<TypeRegistration> GetRegistrations(string nameSuffix)
        {
            yield return
                new TypeRegistration(
                    RegistrationExpressionBuilder.BuildExpression(this.Type, this.Attributes),
                     typeof(IMatchingRule))
                {
                    Name = this.Name + nameSuffix,
                    Lifetime = TypeRegistrationLifetime.Transient
                };
        }
    }
}
