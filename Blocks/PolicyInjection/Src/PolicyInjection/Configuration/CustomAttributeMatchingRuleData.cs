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
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using FakeRules = Microsoft.Practices.EnterpriseLibrary.PolicyInjection.MatchingRules;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using System.ComponentModel;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration
{
    /// <summary>
    /// Configuration element for the <see cref="CustomAttributeMatchingRule"/> configuration.
    /// </summary>
    [ResourceDescription(typeof(DesignResources), "CustomAttributeMatchingRuleDataDescription")]
    [ResourceDisplayName(typeof(DesignResources), "CustomAttributeMatchingRuleDataDisplayName")]
    public class CustomAttributeMatchingRuleData : MatchingRuleData
    {
        private static AssemblyQualifiedTypeNameConverter typeConverter = new AssemblyQualifiedTypeNameConverter();

        private const string SearchInheritanceChainPropertyName = "searchInheritanceChain";
        private const string AttributeTypePropertyName = "attributeType";

        /// <summary>
        /// Creates a new <see cref="CustomAttributeMatchingRuleData"/>.
        /// </summary>
        public CustomAttributeMatchingRuleData()
        {
            base.Type = typeof(FakeRules.CustomAttributeMatchingRule);
        }

        /// <summary>
        /// Creates a new <see cref="CustomAttributeMatchingRuleData"/> instance.
        /// </summary>
        /// <param name="name">Name of the matching rule.</param>
        public CustomAttributeMatchingRuleData(string name)
            : base(name, typeof(FakeRules.CustomAttributeMatchingRule))
        {
        }

        /// <summary>
        /// Creates a new <see cref="CustomAttributeMatchingRuleData"/> instance.
        /// </summary>
        /// <param name="name">Name of the matching rule.</param>
        /// <param name="attributeType">Attribute to find on the target.</param>
        /// <param name="searchInheritanceChain">Should we search the inheritance chain to find the attribute?</param>
        public CustomAttributeMatchingRuleData(string name, Type attributeType, bool searchInheritanceChain)
            : this(name, typeConverter.ConvertToString(attributeType), searchInheritanceChain)
        {
        }

        /// <summary>
        /// Creates a new <see cref="CustomAttributeMatchingRuleData"/> instance.
        /// </summary>
        /// <param name="name">Name of the matching rule.</param>
        /// <param name="attributeTypeName">Name of the attribute type to match on the target.</param>
        /// <param name="searchInheritanceChain">Should we search the inheritance chain to find the attribute?</param>
        public CustomAttributeMatchingRuleData(string name, string attributeTypeName, bool searchInheritanceChain)
            : base(name, typeof(FakeRules.CustomAttributeMatchingRule))
        {
            SearchInheritanceChain = searchInheritanceChain;
            AttributeTypeName = attributeTypeName;
        }

        /// <summary>
        /// Should we search the inheritance chain to find the attribute?
        /// </summary>
        /// <value>The "searchInheritanceChain" config attribute.</value>
        [ConfigurationProperty(SearchInheritanceChainPropertyName)]
        [ResourceDescription(typeof(DesignResources), "CustomAttributeMatchingRuleDataSearchInheritanceChainDescription")]
        [ResourceDisplayName(typeof(DesignResources), "CustomAttributeMatchingRuleDataSearchInheritanceChainDisplayName")]
        public bool SearchInheritanceChain
        {
            get { return (bool)base[SearchInheritanceChainPropertyName]; }
            set { base[SearchInheritanceChainPropertyName] = value; }
        }

        /// <summary>
        /// Name of attribute type to match.
        /// </summary>
        /// <value>The "attributeType" config attribute.</value>
        [ConfigurationProperty(AttributeTypePropertyName, IsRequired = true)]
        [ResourceDescription(typeof(DesignResources), "CustomAttributeMatchingRuleDataAttributeTypeNameDescription")]
        [ResourceDisplayName(typeof(DesignResources), "CustomAttributeMatchingRuleDataAttributeTypeNameDisplayName")]
        [Editor(CommonDesignTime.EditorTypes.TypeSelector, CommonDesignTime.EditorTypes.UITypeEditor)]
        [BaseType(typeof(Attribute), TypeSelectorIncludes.BaseType | TypeSelectorIncludes.AbstractTypes)]
        public string AttributeTypeName
        {
            get { return (string)base[AttributeTypePropertyName]; }
            set { base[AttributeTypePropertyName] = value; }
        }

        /// <summary>
        /// The underlying type object for the attribute we want to search for.
        /// </summary>
        /// <value>This wraps the AttributeTypeName property in a type converter.</value>
        public Type AttributeType
        {
            get { return (Type)typeConverter.ConvertFrom(AttributeTypeName); }
            set { AttributeTypeName = typeConverter.ConvertToString(value); }
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
                new TypeRegistration<IMatchingRule>(
                    () => new CustomAttributeMatchingRule(this.AttributeType, this.SearchInheritanceChain))
                {
                    Name = this.Name + nameSuffix,
                    Lifetime = TypeRegistrationLifetime.Transient
                };
        }
    }
}
